Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Linq
Imports System.Web
Imports DevExpress.Upload.Properties

Namespace DevExpress.Upload
	Public Class Data
		Private privateID As Int32
		Public Property ID() As Int32
			Get
				Return privateID
			End Get
			Set(ByVal value As Int32)
				privateID = value
			End Set
		End Property
		Private privateDate As DateTime
		Public Property UploadDate() As DateTime
			Get
				Return privateDate
			End Get
			Set(ByVal value As DateTime)
				privateDate = value
			End Set
		End Property
		Private privateName As String
		Public Property Name() As String
			Get
				Return privateName
			End Get
			Set(ByVal value As String)
				privateName = value
			End Set
		End Property
		Private privateComment As String
		Public Property Comment() As String
			Get
				Return privateComment
			End Get
			Set(ByVal value As String)
				privateComment = value
			End Set
		End Property
		Private privateFileType As FileType
		Public Property FileType() As FileType
			Get
				Return privateFileType
			End Get
			Set(ByVal value As FileType)
				privateFileType = value
			End Set
		End Property
	End Class

	Public Enum FileType
		Unknown
		Text
		Html
		Image
	End Enum
End Namespace
