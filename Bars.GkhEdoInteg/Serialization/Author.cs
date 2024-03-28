namespace Bars.GkhEdoInteg.Serialization
{
    using Newtonsoft.Json;

    public sealed class Author
    {
        /// <summary>
        /// Имя
        /// </summary>
        [JsonProperty("fn")]
        public string Name { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [JsonProperty("ln")]
        public string Surname { get; set; }

        /// <summary>
        /// отчество
        /// </summary>
        [JsonProperty("pn")]
        public string Patronymic { get; set; }
    }
}
