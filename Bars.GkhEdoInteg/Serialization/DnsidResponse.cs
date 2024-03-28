namespace Bars.GkhEdoInteg.Serialization
{
    using Newtonsoft.Json;

    /// <summary>
    /// Ответ от сервиса авторизации в системе ЭДО
    /// </summary>
    public sealed class DnsidResponse
    {
        /// <summary>
        /// DNSID для работы с сервисом
        /// </summary>
        [JsonProperty("DNSID")]
        public string Dnsid { get; set; }

        /// <summary>
        /// Ключ куки для авторизации
        /// </summary>
        [JsonProperty("auth_token")]
        public string AuthToken { get; set; }

        /// <summary>
        /// Ошибки при выполнении авторизации
        /// </summary>
        [JsonProperty("errors")]
        public string[] Errors { get; set; }
    }
}
