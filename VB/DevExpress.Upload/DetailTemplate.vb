Imports Microsoft.VisualBasic
Imports System
Imports System.Web.UI
Imports DevExpress.Web.ASPxEditors
Imports DevExpress.Web.ASPxGridView
Imports DevExpress.Web.ASPxHtmlEditor
Imports DevExpress.Web.Data

Namespace DevExpress.Upload
	Public Class DetailTemplate
		Implements ITemplate

		Protected Friend Sub New()
		End Sub

		Public Sub InstantiateIn(ByVal container As Control) Implements ITemplate.InstantiateIn
			Dim dataRow = TryCast((TryCast(container, GridViewDetailRowTemplateContainer)).DataItem, WebCachedDataRow)
			Dim fileType = CType(dataRow("FileType"), FileType)
			Dim name = TryCast(dataRow("Name"), String)
			Dim editor = GetDetailControl(name, fileType)
			container.Controls.Add(editor)
			If fileType = FileType.Html OrElse fileType = FileType.Text Then
				Dim id = CInt(Fix(dataRow("ID")))
				Dim btnUpload = CreateUploadButton(id)
				container.Controls.Add(btnUpload)
			End If
		End Sub

		Private Function CreateUploadButton(ByVal id As Int32) As Control
			Dim button = New ASPxButton() With {.ID = "btnUpload", .Text = "Save", .AutoPostBack = False}
			button.ClientSideEvents.Click = String.Format("function() {{ grid.PerformCallback(""upload-file:{0}""); }}", id)
			Return button
		End Function

		Private Function GetBinaryImage(ByVal name As String) As Control
			Return New ASPxBinaryImage() With {.ID = "Editor", .ContentBytes = LocalHelper.ReadFileAsBytes(name)}
		End Function

		Private Function GetHtmlEditor(ByVal name As String) As Control
			Return New ASPxHtmlEditor() With {.ID = "Editor", .Html = LocalHelper.ReadFileAsString(name)}
		End Function

		Private Function GetMemo(ByVal name As String) As Control
			Return New ASPxMemo() With {.ID = "Editor", .Height = 200, .Width = 250, .Text = LocalHelper.ReadFileAsString(name)}
		End Function

		Private Function GetDetailControl(ByVal name As String, ByVal fileType As FileType) As Control
			Select Case fileType
				Case FileType.Image
					Return GetBinaryImage(name)
				Case FileType.Html
					Return GetHtmlEditor(name)
				Case FileType.Text
					Return GetMemo(name)
				Case Else
					Return Nothing
			End Select
		End Function
	End Class
End Namespace