Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.UI
Imports DevExpress.Web.ASPxGridView
Imports DevExpress.Web.ASPxUploadControl
Imports System.IO
Imports DevExpress.Web.ASPxEditors
Imports DevExpress.Web.ASPxHtmlEditor
Imports DevExpress.Web.Data

Namespace DevExpress.Upload
	Partial Public Class [Default]
		Inherits Page
		Private _data As List(Of Data)

        Private ReadOnly Property DataSource() As List(Of Data)
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
            Return If((DataSource.Count = 0), 0, DataSource.Max(Function(data) data.ID) + 1)
        End Function

        Private Function FindData(ByVal id As Int32) As Data
            Return DataSource.FirstOrDefault(Function(x) x.ID = id)
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
            If (Not IsPostBack) Then
                grid.DataBind()
            End If
        End Sub

        Protected Sub grid_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            grid.DataSource = DataSource
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
			If (Not String.IsNullOrEmpty(e.Parameters)) Then
				Dim args = e.Parameters.Split(New Char() { ":"c }, 2)
				Select Case args(0)
					Case "upload-file"
							Dim id = Int32.Parse(args(1))
							Dim data = FindData(id)
							Dim editor = grid.FindDetailRowTemplateControl(id, "Editor")
							UpdateFile(editor, data.Name)
							Exit Select
				End Select
			End If
		End Sub

		Protected Sub grid_RowDeleting(ByVal sender As Object, ByVal e As ASPxDataDeletingEventArgs)
			Dim id = Int32.Parse(e.Keys("ID").ToString())
			Dim data = FindData(id)
			Dim filepath = LocalHelper.ResolveFilePath(data.Name)
            DataSource.Remove(data)
			File.Delete(filepath)
			e.Cancel = True
		End Sub

		Protected Sub grid_RowInserting(ByVal sender As Object, ByVal e As ASPxDataInsertingEventArgs)
			If IsFileUploaded Then
                Dim data = New Data With {.ID = GetNewID(), .UploadDate = DateTime.Now, .Name = Filename, .Comment = e.NewValues("Comment").ToString(), .FileType = LocalHelper.GetFileType(Filename)}
                DataSource.Add(data)
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