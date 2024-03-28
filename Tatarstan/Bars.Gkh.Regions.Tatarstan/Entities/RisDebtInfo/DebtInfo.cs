namespace Bars.Gkh.Regions.Tatarstan.Entities.RisDebtInfo
{
    using Bars.B4.DataAccess;
    using Newtonsoft.Json;

    /// <summary>
    /// Информация о задолженностях за ЖКУ
    /// </summary>
    public class DebtInfo : PersistentObject
    {
        /// <summary>
        /// Запросы по которым требуется предоставить ответ
        /// </summary>
        [JsonProperty("need_response")]
        public int NeedResponse { get; set; }

        // <summary>
        /// Поступившие запросы без ответа 
        /// </summary>
        [JsonProperty("notsent_response")]
        public int NotSentResponse { get; set; }

        /// <summary>
        /// Поступившие запросы с предоставленным ответом 
        /// </summary>
        [JsonProperty("sent_response")]
        public int SentResponse { get; set; }

        /// <summary>
        /// Ошибка получения информации
        /// </summary>
        [JsonProperty("error_message")]
        public string ErrorMessage { get; set; }
    }
}