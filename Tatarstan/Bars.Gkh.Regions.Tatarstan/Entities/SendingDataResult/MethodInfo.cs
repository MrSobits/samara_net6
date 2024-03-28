namespace Bars.Gkh.Regions.Tatarstan.Entities.SendingDataResult
{
    using Newtonsoft.Json;

    /// <summary>
    /// Информация по методам интеграции
    /// </summary>
    public class MethodInfo
    {
        /// <summary>
        /// Наименование метода
        /// </summary>
        [JsonProperty(PropertyName = "MethodName")]
        public string MethodName { get; set; }
        
        /// <summary>
        /// Общее кол-во объектов
        /// </summary>
        [JsonProperty(PropertyName = "TotalCount")]
        public int TotalCount { get; set; }
        
        /// <summary>
        /// Кол-во отправленных объектов в ГИС
        /// </summary>
        [JsonProperty(PropertyName = "SentCount")]
        public int SentCount { get; set; }
        
        /// <summary>
        /// Ссылка на реестр
        /// </summary>
        [JsonProperty(PropertyName = "UrlRegistry")]
        public string UrlRegistry { get; set; }
    }
}