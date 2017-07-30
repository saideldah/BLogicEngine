using JIC.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIC.Business.ProductSetup.Model
{
    public class ProductSetupFileModel : BaseModel
    {
        public string LibraryName { get; set; }
        public string FolderPath { get; set; }
        public string FilePath { get; set; }
        public string UploadedFileName { get; set; }
        public string OriginFileName { get; set; }
        public string FileType { get; set; }
        public int Version { get; set; }
    }
}
