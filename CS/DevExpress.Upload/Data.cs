using System;
using System.IO;
using System.Linq;
using System.Web;
using DevExpress.Upload.Properties;

namespace DevExpress.Upload {
    public class Data {
        public Int32 ID { get; set; }
        public DateTime Date { get; set; }
        public String Name { get; set; }
        public String Comment { get; set; }
        public FileType FileType { get; set; }
    }

    public enum FileType { Unknown, Text, Html, Image }
}
