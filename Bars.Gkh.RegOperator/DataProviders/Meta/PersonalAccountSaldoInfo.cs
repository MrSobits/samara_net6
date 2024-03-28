namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    /// <summary>
    /// Данные об ЛС при экспорте сальдо в Excel (используется в шаблоне)
    /// </summary>
    public class PersonalAccountSaldoInfo
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Номер Л/С
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public string Municipality { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Сальдо по базовому тарифу
        /// </summary>
        public decimal SaldoByBaseTariff { get; set; }

        /// <summary>
        /// Сальдо по тарифу решения
        /// </summary>
        public decimal SaldoByDecisionTariff { get; set; }

        /// <summary>
        /// Сальдо по пени
        /// </summary>
        public decimal SaldoByPenalty { get; set; }
    }
}