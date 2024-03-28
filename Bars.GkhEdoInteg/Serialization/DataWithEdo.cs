namespace Bars.GkhEdoInteg.Serialization
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public sealed class DataWithEdo
    {
        [JsonProperty("documents")]
        public Dictionary<string, Docs> Documents { get; set; }
    }
}
