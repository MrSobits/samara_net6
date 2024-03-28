namespace Bars.Gkh.Regions.Tatarstan.Entities.SendingDataResult
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;

    using Newtonsoft.Json;

    /// <summary>
    /// Результат отправки данных с ГИС ЖКХ
    /// </summary>
    public class SendingDataResult : PersistentObject
    {
        /// <summary>
        /// Наименование организации
        /// </summary>
        [JsonProperty(PropertyName = "OrgName")]
        public string ContragentName { get; set; }
        
        /// <summary>
        /// Id рганизации
        /// </summary>
        [JsonProperty(PropertyName = "OrgID")]
        public long ContragentId { get; set; }
        
        /// <summary>
        /// Флаг наличия у контрагента загруженных платежные документы за предыдущий период
        /// (относительно месяца запуска метода)
        /// </summary>
        [JsonProperty(PropertyName = "FormatFileLoaded")]
        public bool FormatFileLoaded { get; set; }
        
        /// <summary>
        /// Список информации о методах интеграции
        /// </summary>
        [JsonProperty(PropertyName = "Info")]
        public List<MethodInfo> MethodsInfo { get; set; }
        
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        [JsonProperty(PropertyName = "ErrorMessage")]
        public string ErrorMessage { get; set; }
    }
}