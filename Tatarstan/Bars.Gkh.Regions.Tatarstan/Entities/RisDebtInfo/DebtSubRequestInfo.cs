namespace Bars.Gkh.Regions.Tatarstan.Entities.RisDebtInfo
{
    using System;

    using Newtonsoft.Json;

    /// <summary>
    /// Информация о запросе о задолженности за ЖКУ
    /// </summary>
    public class DebtSubRequestInfo
    {
        /// <summary>
        /// Номер запроса
        /// </summary>
        [JsonProperty("request_number")]
        public string RequestNumber { get; set; }

        /// <summary>
        /// Дата окончания 
        /// </summary>
        [JsonProperty("final_date")]
        public string FinalDate { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }
        
        /// <summary>
        /// Статус ответа
        /// </summary>
        [JsonProperty("responseStatus")]
        public string ResponseStatus { get; set; }
    }
}