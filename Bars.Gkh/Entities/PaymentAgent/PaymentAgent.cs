namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Платежный агент
    /// </summary>
    public class PaymentAgent : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Код (Идентификатор) платежного агента
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Id договора загрузки суммы (используется для импорта)
        /// </summary>
        public virtual string SumContractId { get; set; }

        /// <summary>
        /// Id договора загрузки пени (используется для импорта)
        /// </summary>
        public virtual string PenaltyContractId { get; set; }
    }
}
