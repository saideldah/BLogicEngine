using JIC.DataAccess.Entity;
using System;

namespace JIC.DataAccess.ProductSetup.Entity
{
    public class ProductSetupFileEntity : BaseEntity
    {
        public string LibraryName { get; set; }
        public int FolderPath { get; set; }
        public string OriginFileName { get; set; }
        public string UploadedFileName { get; set; }
        public string FilePath { get; set; }
        public string FileType { get; set; }
        public int Version { get; set; }
    }
}
