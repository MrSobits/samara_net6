namespace Bars.GkhEdoInteg.Serialization
{
    using Newtonsoft.Json;

    public sealed class Correspondent
    {
        [JsonProperty("address")]
        public Address Address { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }
    }
}
