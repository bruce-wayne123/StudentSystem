using Newtonsoft.Json;
using SQLite;

namespace StudXam.Models
{
    public class FileModel
    {
        [PrimaryKey, AutoIncrement]
        public int FileNo { get; set; }

        public int RollNo { get; set; }
        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string Base6String { get; set; }

       
        public bool IsFiledPicked { get; set; }

       
        public string Message { get; set; }

        public string SysfileName { get; set; }

        public bool Active { get; set; }

        public long NewUpload { get; set; }

        public long IsDelete { get; set; }
    }
}