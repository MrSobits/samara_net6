namespace Bars.Gkh.RegionLoader.Base.JsonProxyClasses
{
    using Newtonsoft.Json;

    public class Module
    {
        [JsonProperty(PropertyName = "baseModules")]
        public string[] BaseModules { get; set; }

        [JsonProperty(PropertyName = "regions")]
        public RegionModule[] RegionModules { get; set; }
    }
}
