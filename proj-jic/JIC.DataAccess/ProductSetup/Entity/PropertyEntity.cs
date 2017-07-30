using JIC.DataAccess.Entity;

namespace JIC.DataAccess.ProductSetup.Entity
{
    public class PropertyEntity : BaseEntity
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public string DataType { get; set; }
        public string PropertyType { get; set; }
        public string Value { get; set; }
    }
}
