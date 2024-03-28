namespace Bars.Gkh.Regions.Tatarstan.Entities.RisDebtInfo
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities;

    using Newtonsoft.Json;

    /// <summary>
    /// Запрос задолженности за ЖКУ
    /// </summary>
    public class DebtSubRequest
    {
        /// <summary>
        /// ОГРН 
        /// </summary>
        [JsonProperty("ogrn")]
        public string Ogrn { get; set; }

        /// <summary>
        /// инн
        /// </summary>
        [JsonProperty("inn")]
        public string Inn { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        [JsonProperty("kpp")]
        public string Kpp { get; set; }
        
        /// <summary>
        /// Информация о задолженностях
        /// </summary>
        [JsonProperty("subrequestsInfo")]
        public List<DebtSubRequestInfo> SubRequestsInfo { get; set; }
    }
}