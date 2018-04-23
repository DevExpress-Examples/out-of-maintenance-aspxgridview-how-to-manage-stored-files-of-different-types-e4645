using System;
using System.Web.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxGridView;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.Data;

namespace DevExpress.Upload {
    public class DetailTemplate : ITemplate {

        protected internal DetailTemplate() {
        }

        public void InstantiateIn(Control container) {
            var dataRow = (container as GridViewDetailRowTemplateContainer).DataItem as WebCachedDataRow;
            var fileType = (FileType)dataRow["FileType"];
            var name = dataRow["Name"] as String;
            var editor = GetDetailControl(name, fileType);
            container.Controls.Add(editor);
            if (fileType == FileType.Html || fileType == FileType.Text)
            {
                var id = (Int32)dataRow["ID"];
                var btnUpload = CreateUploadButton(id);
                container.Controls.Add(btnUpload);
            }
        }

        Control CreateUploadButton(Int32 id) {
            var button = new ASPxButton() {
                ID = "btnUpload",
                Text = "Save",
                AutoPostBack = false
            };
            button.ClientSideEvents.Click = String.Format("function() {{ grid.PerformCallback(\"upload-file:{0}\"); }}", id);
            return button;
        }

        Control GetBinaryImage(String name) {
            return new ASPxBinaryImage() {
                ID = "Editor",
                ContentBytes = LocalHelper.ReadFileAsBytes(name)
            };
        }

        Control GetHtmlEditor(String name) {
            return new ASPxHtmlEditor() {
                ID = "Editor",
                Html = LocalHelper.ReadFileAsString(name)
            };
        }

        Control GetMemo(String name) {
            return new ASPxMemo() {
                ID = "Editor",
                Height = 200,
                Width = 250,
                Text = LocalHelper.ReadFileAsString(name)
            };
        }

        Control GetDetailControl(String name, FileType fileType) {
            switch (fileType) {
                case FileType.Image: return GetBinaryImage(name);
                case FileType.Html: return GetHtmlEditor(name);
                case FileType.Text: return GetMemo(name);
                case FileType.Unknown:
                default: return null;
            }
        }
    }
}