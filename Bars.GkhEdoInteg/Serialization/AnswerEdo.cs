namespace Bars.GkhEdoInteg.Serialization
{
    using Newtonsoft.Json;

    public sealed class AnswerEdo
    {
        [JsonProperty("data")]
        public DataWithEdo Data { get; set; }

        [JsonProperty("errors")]
        public Errors Errors { get; set; }
    }
}
