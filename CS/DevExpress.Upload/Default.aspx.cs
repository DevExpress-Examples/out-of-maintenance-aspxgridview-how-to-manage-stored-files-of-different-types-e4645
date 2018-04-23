using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using DevExpress.Web;
using System.IO;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.Data;

namespace DevExpress.Upload {
    public partial class Default : Page {
        List<Data> _data;

        List<Data> Data {
            get {
                const String sessionName = "_Data";
                if(_data == null) {
                    if(Session[sessionName] != null)
                        _data = Session[sessionName] as List<Data>;
                    else
                        Session[sessionName] = _data = new List<Data>();
                }
                return _data;
            }
        }

        String Filename {
            get { return Session["_Filename"] as String; }
            set { Session["_Filename"] = value; }
        }

        Boolean IsFileUploaded {
            get { return !String.IsNullOrEmpty(Filename); }
        }

        Int32 GetNewID() {
            return (Data.Count == 0) ? 0 : Data.Max(data => data.ID) + 1;
        }

        Data FindData(Int32 id) {
            return Data.FirstOrDefault(x => x.ID == id);
        }

        void UpdateFile(Control editor, String name) {
            if(editor is ASPxHtmlEditor) {
                var control = editor as ASPxHtmlEditor;
                var html = control.Html;
                LocalHelper.SaveToFile(html, name);
                return;
            }
            if(editor is ASPxMemo) {
                var control = editor as ASPxMemo;
                var text = control.Text;
                LocalHelper.SaveToFile(text, name);
            }
        }

        protected void Page_Init(Object sender, EventArgs e) {
            grid.Templates.DetailRow = new DetailTemplate();
        }

        protected void Page_Load(Object sender, EventArgs e) {
            if(!IsPostBack)
                grid.DataBind();
        }

        protected void grid_DataBinding(Object sender, EventArgs e) {
            grid.DataSource = Data;
        }

        protected void upload_FileUploadComplete(Object sender, FileUploadCompleteEventArgs e) {
            if(IsFileUploaded) {
                e.ErrorText = "Unable to unload more than one file. Update or cancel previous unload operation.";
                e.IsValid = false;
                return;
            }
            if (e.IsValid) {
                var name = e.UploadedFile.FileName;
                var filepath = LocalHelper.ResolveFilePath(name);
                e.UploadedFile.SaveAs(filepath, true);
                Filename = name;
            }
        }

        protected void grid_CustomCallback(Object sender, ASPxGridViewCustomCallbackEventArgs e) {
            if(!String.IsNullOrEmpty(e.Parameters)) {
                var args = e.Parameters.Split(new[] { ':' }, 2);
                switch(args[0]) {
                    case "upload-file": {
                            var id = Int32.Parse(args[1]);
                            var data = FindData(id);
                            var editor = grid.FindDetailRowTemplateControl(id, "Editor");
                            UpdateFile(editor, data.Name);
                            break;
                        }
                }
            }
        }

        protected void grid_RowDeleting(Object sender, ASPxDataDeletingEventArgs e) {
            var id = Int32.Parse(e.Keys["ID"].ToString());
            var data = FindData(id);
            var filepath = LocalHelper.ResolveFilePath(data.Name);
            Data.Remove(data);
            File.Delete(filepath);
            e.Cancel = true;
        }

        protected void grid_RowInserting(Object sender, ASPxDataInsertingEventArgs e) {
            if(IsFileUploaded) {
                var data = new Data {
                    ID = GetNewID(),
                    Date = DateTime.Now,
                    Name = Filename,
                    Comment = e.NewValues["Comment"].ToString(),
                    FileType = LocalHelper.GetFileType(Filename)
                };
                Data.Add(data);
                Filename = null;
                grid.CancelEdit();
            }
            e.Cancel = true;
        }

        protected void grid_CancelRowEditing(Object sender, ASPxStartRowEditingEventArgs e) {
            if(IsFileUploaded) {
                var filepath = LocalHelper.ResolveFilePath(Filename);
                File.Delete(filepath);
                Filename = null;
            }
        }
    }
}