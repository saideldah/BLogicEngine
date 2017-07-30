
using System;
using JIC.DataAccess.Entity;
namespace JIC.DataAccess.ProductSetup.Entity
{
    public class CoverEntity : BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string CodeBack { get; set; }
    }
}
