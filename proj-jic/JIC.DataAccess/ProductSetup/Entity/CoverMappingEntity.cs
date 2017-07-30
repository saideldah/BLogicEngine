using JIC.DataAccess.Entity;
using System;

namespace JIC.DataAccess.ProductSetup.Entity
{
    public class CoverMappingEntity : BaseEntity
    {
        public Guid CoverId { get; set; }
        public string CodeBack { get; set; }
    }
}
