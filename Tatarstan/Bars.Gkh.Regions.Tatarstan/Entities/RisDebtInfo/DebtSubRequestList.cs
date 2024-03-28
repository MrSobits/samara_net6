namespace Bars.Gkh.Regions.Tatarstan.Entities.RisDebtInfo
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    /// <summary>
    /// Список запросов по задолженностям за ЖКУ из РИС ЖКХ
    /// </summary>
    public class DebtSubRequestList
    {
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Запросы 
        /// </summary>
        [JsonProperty("subrequests")]
        public List<DebtSubRequest> SubRequests { get; set; }
    }
}