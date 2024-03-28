namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using Bars.B4.DataAccess;
    /// <summary>
    /// Выгруженный ЛС для изменения сальдо.
    /// <remarks>Будут удалены после импортирования изменений</remarks>
    /// </summary>
    public class AccountExcelSaldoChange : BaseEntity
    {
        /// <summary>
        /// Экспорт, в рамках которого был выгружен данный ЛС
        /// </summary>
        public virtual SaldoChangeExport SaldoChangeExport { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Сальдо по базовому тарифу до изменений
        /// </summary>
        public virtual decimal SaldoByBaseTariffBefore { get; set; }

        /// <summary>
        /// Сальдо по тарифу решения до изменений
        /// </summary>
        public virtual decimal SaldoByDecisinTariffBefore { get; set; }

        /// <summary>
        /// Сальдо по пени до изменений
        /// </summary>
        public virtual decimal SaldoByPenaltyBefore { get; set; }
    }
}