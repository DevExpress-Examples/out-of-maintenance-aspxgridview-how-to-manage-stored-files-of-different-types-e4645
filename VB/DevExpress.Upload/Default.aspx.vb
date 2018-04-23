Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.UI
Imports DevExpress.Web
Imports System.IO
Imports DevExpress.Web.ASPxHtmlEditor
Imports DevExpress.Web.Data

Namespace DevExpress.Upload
    Partial Public Class [Default]
        Inherits Page

        Private _data As List(Of Data)

        Private ReadOnly Property Data() As List(Of Data)
            Get
                Const sessionName As String = "_Data"
                If _data Is Nothing Then
                    If Session(sessionName) IsNot Nothing Then
                        _data = TryCast(Session(sessionName), List(Of Data))
                    Else
                        _data = New List(Of Data)()
                        Session(sessionName) = _data
                    End If
                End If
                Return _data
            End Get
        End Property

        Private Property Filename() As String
            Get
                Return TryCast(Session("_Filename"), String)
            End Get
            Set(ByVal value As String)
                Session("_Filename") = value
            End Set
        End Property

        Private ReadOnly Property IsFileUploaded() As Boolean
            Get
                Return Not String.IsNullOrEmpty(Filename)
            End Get
        End Property

        Private Function GetNewID() As Int32
            Return If(Data.Count = 0, 0, Data.Max(Function(data) data.ID) + 1)
        End Function

        Private Function FindData(ByVal id As Int32) As Data
            Return Data.FirstOrDefault(Function(x) x.ID = id)
        End Function

        Private Sub UpdateFile(ByVal editor As Control, ByVal name As String)
            If TypeOf editor Is ASPxHtmlEditor Then
                Dim control = TryCast(editor, ASPxHtmlEditor)
                Dim html = control.Html
                LocalHelper.SaveToFile(html, name)
                Return
            End If
            If TypeOf editor Is ASPxMemo Then
                Dim control = TryCast(editor, ASPxMemo)
                Dim text = control.Text
                LocalHelper.SaveToFile(text, name)
            End If
        End Sub

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            grid.Templates.DetailRow = New DetailTemplate()
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            If Not IsPostBack Then
                grid.DataBind()
            End If
        End Sub

        Protected Sub grid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            grid.DataSource = Data
        End Sub

        Protected Sub upload_FileUploadComplete(ByVal sender As Object, ByVal e As FileUploadCompleteEventArgs)
            If IsFileUploaded Then
                e.ErrorText = "Unable to unload more than one file. Update or cancel previous unload operation."
                e.IsValid = False
                Return
            End If
            If e.IsValid Then
                Dim name = e.UploadedFile.FileName
                Dim filepath = LocalHelper.ResolveFilePath(name)
                e.UploadedFile.SaveAs(filepath, True)
                Filename = name
            End If
        End Sub

        Protected Sub grid_CustomCallback(ByVal sender As Object, ByVal e As ASPxGridViewCustomCallbackEventArgs)
            If Not String.IsNullOrEmpty(e.Parameters) Then
                Dim args = e.Parameters.Split( { ":"c }, 2)
                Select Case args(0)
                    Case "upload-file"

                            Dim id_Renamed = Int32.Parse(args(1))

                            Dim data_Renamed = FindData(id_Renamed)
                            Dim editor = grid.FindDetailRowTemplateControl(id_Renamed, "Editor")
                            UpdateFile(editor, data_Renamed.Name)
                            Exit Select
                End Select
            End If
        End Sub

        Protected Sub grid_RowDeleting(ByVal sender As Object, ByVal e As ASPxDataDeletingEventArgs)

            Dim id_Renamed = Int32.Parse(e.Keys("ID").ToString())

            Dim data_Renamed = FindData(id_Renamed)
            Dim filepath = LocalHelper.ResolveFilePath(data_Renamed.Name)
            Data.Remove(data_Renamed)
            File.Delete(filepath)
            e.Cancel = True
        End Sub

        Protected Sub grid_RowInserting(ByVal sender As Object, ByVal e As ASPxDataInsertingEventArgs)
            If IsFileUploaded Then

                Dim data_Renamed = New Data With {.ID = GetNewID(), .Date = Date.Now, .Name = Filename, .Comment = e.NewValues("Comment").ToString(), .FileType = LocalHelper.GetFileType(Filename)}
                Data.Add(data_Renamed)
                Filename = Nothing
                grid.CancelEdit()
            End If
            e.Cancel = True
        End Sub

        Protected Sub grid_CancelRowEditing(ByVal sender As Object, ByVal e As ASPxStartRowEditingEventArgs)
            If IsFileUploaded Then
                Dim filepath = LocalHelper.ResolveFilePath(Filename)
                File.Delete(filepath)
                Filename = Nothing
            End If
        End Sub
    End Class
End Namespace