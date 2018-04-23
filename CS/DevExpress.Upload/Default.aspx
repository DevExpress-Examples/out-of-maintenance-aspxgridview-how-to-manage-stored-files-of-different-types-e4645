<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DevExpress.Upload.Default" %>
<%@ Register Assembly="DevExpress.Web.v12.2, Version=12.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v12.2, Version=12.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v12.2, Version=12.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>
<%@ Register assembly="DevExpress.Web.v12.2, Version=12.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxGridView" tagprefix="dx" %>
<%@ Register assembly="DevExpress.Web.v12.2, Version=12.2.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script type="text/javascript">
    </script>
    <form id="form1" runat="server">
    <div>
        <dx:ASPxGridView ID="grid" runat="server" ClientInstanceName="grid" KeyFieldName="ID"
                OnDataBinding="grid_DataBinding" OnCustomCallback="grid_CustomCallback" 
                OnRowDeleting="grid_RowDeleting" OnRowInserting="grid_RowInserting" OnCancelRowEditing="grid_CancelRowEditing"
                AutoGenerateColumns="False">
            <Columns>
                <dx:GridViewCommandColumn Caption="Add/Remove">
                    <NewButton Visible="true" Text="Add"/>
                    <DeleteButton Visible="true" Text="Remove"/>
                </dx:GridViewCommandColumn>
                <dx:GridViewDataTextColumn FieldName="ID" Visible="false" />
                <dx:GridViewDataTextColumn FieldName="FileType" Visible="false" />
                <dx:GridViewDataDateColumn Caption="Date" FieldName="Date" />
                <dx:GridViewDataTextColumn Caption="Name" FieldName="Name" />
                <dx:GridViewDataMemoColumn Caption="Comment" FieldName="Comment" />
            </Columns>
            <SettingsDetail ShowDetailRow="true" AllowOnlyOneMasterRowExpanded="false"/>
            <Templates>
                <EditForm>
                    <dx:ASPxLabel ID="lblComment" runat="server" Text="Comment:"></dx:ASPxLabel>
                    <dx:ASPxMemo ID="comment" runat="server" Height="70px" Width="170px" Text='<%#Bind("Comment")%>'
                                 ClientInstanceName="comment" EnableClientSideAPI="true"></dx:ASPxMemo>
                    <dx:ASPxLabel ID="lblFile" runat="server" Text="File:"></dx:ASPxLabel>
                    <dx:ASPxUploadControl ID="upload" runat="server" Width="280px" ClientInstanceName="upload" FileUploadMode="OnPageLoad"
                            UploadMode="Advanced" ShowProgressPanel="true" OnFileUploadComplete="upload_FileUploadComplete">
                        <ValidationSettings AllowedFileExtensions=".txt,.htm,.html,.jpg,.jpeg,.jpe" MaxFileSize="10000000" />
                        <AdvancedModeSettings EnableMultiSelect="false"/>
                    </dx:ASPxUploadControl>
                    <dx:ASPxButton ID="btnUpload" runat="server" Text="Upload" AutoPostBack="false">
                        <ClientSideEvents Click="function() { upload.Upload(); }" />
                    </dx:ASPxButton>
                    <dx:ASPxGridViewTemplateReplacement ID="updateButton" ReplacementType="EditFormUpdateButton" runat="server" />
                    <dx:ASPxGridViewTemplateReplacement ID="cancelButton" ReplacementType="EditFormCancelButton" runat="server" />
                </EditForm>
            </Templates>
        </dx:ASPxGridView>
    </div>
    </form>
</body>
</html>
