Imports System
Imports System.IO
Imports System.Linq
Imports System.Web

Namespace DevExpress.Upload
    Public Class Data
        Public Property ID() As Int32
        Public Property [Date]() As Date
        Public Property Name() As String
        Public Property Comment() As String
        Public Property FileType() As FileType
    End Class

    Public Enum FileType
        Unknown
        Text
        Html
        Image
    End Enum
End Namespace
