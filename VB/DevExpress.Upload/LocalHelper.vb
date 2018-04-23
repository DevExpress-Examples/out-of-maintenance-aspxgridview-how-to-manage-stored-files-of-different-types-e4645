Imports System
Imports System.IO
Imports System.Linq
Imports System.Web

Namespace DevExpress.Upload
    Friend NotInheritable Class LocalHelper

        Private Sub New()
        End Sub


        Friend Shared Function ReadFileAsString(ByVal name As String) As String
            Using file = New StreamReader(ResolveFilePath(name))
                Dim text = file.ReadToEnd()
                Return text
            End Using
        End Function

        Friend Shared Function ReadFileAsBytes(ByVal name As String) As Byte()
            Using file = New FileStream(ResolveFilePath(name), FileMode.Open)
                Dim bytes = New Byte(file.Length - 1){}
                file.Read(bytes, 0, bytes.Length)
                Return bytes
            End Using
        End Function

        Friend Shared Function GetFileType(ByVal name As String) As FileType
            Dim ext = Path.GetExtension(name)
            If ( { ".html", ".htm" }).Contains(ext) Then
                Return FileType.Html
            End If
            If ( { ".txt" }).Contains(ext) Then
                Return FileType.Text
            End If
            If ( { ".jpg", ".jpeg", ".jpe" }).Contains(ext) Then
                Return FileType.Image
            End If
            Return FileType.Unknown
        End Function

        Friend Shared Sub SaveToFile(ByVal value As String, ByVal name As String)
            Using file = New StreamWriter(ResolveFilePath(name), False)
                file.Write(value)
            End Using
        End Sub

        Friend Shared Function ResolveFilePath(ByVal name As String) As String
            Return HttpContext.Current.Server.MapPath(My.Settings.Default.StoragePath) & name
        End Function
    End Class
End Namespace