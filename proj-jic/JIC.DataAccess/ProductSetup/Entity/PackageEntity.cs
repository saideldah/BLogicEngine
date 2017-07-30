using JIC.DataAccess.Entity;
using System;

namespace JIC.DataAccess.ProductSetup.Entity
{
    public class PackageEntity : BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public Guid LineOfBusinessId { get; set; }
        public string SmiWizardType { get; set; }
        public string SavingsCalculator { get; set; }
        public string Status { get; set; }
    }
}
