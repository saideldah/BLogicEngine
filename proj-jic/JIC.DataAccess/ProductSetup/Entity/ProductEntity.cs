using JIC.DataAccess.Entity;
using System;

namespace JIC.DataAccess.ProductSetup.Entity
{
    public class ProductEntity : BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public Guid PackageId { get; set; }
    }
}
