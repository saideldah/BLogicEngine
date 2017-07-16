using Newtonsoft.Json;
namespace LovManager.Business
{
    public class ListOfValueModel
    {
        public string Id { get; set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "ParentCode")]
        public string ParentCode { get; set; }

        public ListOfValueModel ParentLov { get; set; }

    }
}
