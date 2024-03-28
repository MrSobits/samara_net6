namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;

    public class SubsidyRecordVersion : BaseEntity
    {
        /// <summary>
        /// Версия ДПКР
        /// </summary>
        public virtual ProgramVersion Version { get; set; }

        /// <summary>
        /// Ссылка на строку субсидирования
        /// </summary>
        public virtual SubsidyRecord SubsidyRecord { get; set; }

        /// <summary>
        /// Бюджет на КР (Привязываю к версии посколку все это расчитывается на суммы именно той версии)
        /// </summary>
        public virtual decimal BudgetCr { get; set; }

        /// <summary>
        /// Потребность в финансировании 
        /// </summary>
        public virtual decimal CorrectionFinance { get; set; }

        /// <summary>
        /// Остаток  средств после проведения КР на конец года
        /// </summary>
        public virtual decimal BalanceAfterCr { get; set; }
    }
}