namespace Bars.Gkh.RegionLoader.Base.JsonProxyClasses
{
    using Newtonsoft.Json;

    public class RegionModule
    {
        [JsonProperty(PropertyName = "regionName")]
        public string RegionName { get; set; }

        [JsonProperty(PropertyName = "modules")]
        public string[] Modules { get; set; }
    }
}
