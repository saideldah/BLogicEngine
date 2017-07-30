using JIC.DataAccess.Entity;
using System;

namespace JIC.DataAccess.ProductSetup.Entity
{
    public class SmiEntity : BaseEntity
    {
        public string Properties { get; set; }
        public string ApplicablePackageProperties { get; set; }
        public string DataType { get; set; }
        public bool RetrievedFromBank { get; set; }
        public string FieldControlOnScreen { get; set; }
        public string Description { get; set; }
    }
}