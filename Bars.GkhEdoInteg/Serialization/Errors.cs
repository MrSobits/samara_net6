namespace Bars.GkhEdoInteg.Serialization
{
    using Newtonsoft.Json;

    public sealed class Errors
    {
        [JsonProperty("err_code")]
        public string ErrCode { get; set; }

        [JsonProperty("err_message")]
        public string ErrMessage { get; set; }
    }
}
