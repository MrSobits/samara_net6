namespace Bars.GkhEdoInteg.Serialization
{
    using Newtonsoft.Json;

    public sealed class DocumentEdo
    {
        [JsonProperty("file_id")]
        public string FileId { get; set; }
    }
}
