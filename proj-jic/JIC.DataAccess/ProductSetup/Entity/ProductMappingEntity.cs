using JIC.DataAccess.Entity;
using System;

namespace JIC.DataAccess.ProductSetup.Entity
{
    public class ProductMappingEntity : BaseEntity
    {
        public string MappingRule { get; set; }
        public string CodeBack { get; set; }
        public string Status { get; set; }
        public Guid ProductId { get; set; }
    }
}
