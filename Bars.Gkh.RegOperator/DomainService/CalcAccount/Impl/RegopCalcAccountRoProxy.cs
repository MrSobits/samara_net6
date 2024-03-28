namespace Bars.Gkh.RegOperator.DomainService
{
    /// <summary>
    /// Расчет реестр домов регионального оператора
    /// </summary>
    public class RegopCalcAccountRoProxy
    {
        /// <summary>
        /// Счет
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// Номер счета
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public string MoSettlement { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Владелец счета
        /// </summary>
        public string AccountOwner { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public long RealityObjectId { get; set; }

        /// <summary>
        /// Начислено
        /// </summary>
        public decimal ChargeTotal { get; set; }

        /// <summary>
        /// Уплачено 
        /// </summary>
        public decimal PaidTotal { get; set; }

        /// <summary>
        /// Задолженность
        /// </summary>
        public decimal Debt { get; set; }

        /// <summary>
        /// Сальдо
        /// </summary>
        public decimal Saldo { get; set; }
    }
}
