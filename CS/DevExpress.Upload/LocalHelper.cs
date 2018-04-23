using System;
using System.IO;
using System.Linq;
using System.Web;
using DevExpress.Upload.Properties;

namespace DevExpress.Upload {
    internal static class LocalHelper {

        internal static String ReadFileAsString(String name) {
            using(var file = new StreamReader(ResolveFilePath(name))) {
                var text = file.ReadToEnd();
                return text;
            }
        }

        internal static Byte[] ReadFileAsBytes(String name) {
            using(var file = new FileStream(ResolveFilePath(name), FileMode.Open)) {
                var bytes = new Byte[file.Length];
                file.Read(bytes, 0, bytes.Length);
                return bytes;
            }
        }

        internal static FileType GetFileType(String name) {
            var ext = Path.GetExtension(name);
            if((new[] { ".html", ".htm" }).Contains(ext))
                return FileType.Html;
            if((new[] { ".txt" }).Contains(ext))
                return FileType.Text;
            if((new[] { ".jpg", ".jpeg", ".jpe" }).Contains(ext))
                return FileType.Image;
            return FileType.Unknown;
        }

        internal static void SaveToFile(String value, String name) {
            using(var file = new StreamWriter(ResolveFilePath(name), false)) {
                file.Write(value);
            }
        }

        internal static String ResolveFilePath(String name) {
            return HttpContext.Current.Server.MapPath(Settings.Default.StoragePath) + name;
        }
    }
}