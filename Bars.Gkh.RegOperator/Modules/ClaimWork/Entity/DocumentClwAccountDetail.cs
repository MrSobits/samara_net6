namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Детализация документа ПИР по каждому лс
    /// </summary>
    public class DocumentClwAccountDetail : BaseEntity
    {
        /// <summary>
        /// Сумма задолженности по базовому тарифу
        /// </summary>
        public virtual decimal DebtBaseTariffSum { get; set; }

        /// <summary>
        /// Сумма задолженности по тарифу решения
        /// </summary>
        public virtual decimal DebtDecisionTariffSum { get; set; }

        /// <summary>
        /// Сумма задолженности
        /// </summary>
        public virtual decimal DebtSum { get; set; }

        /// <summary>
        /// Сумма задолженности по пени
        /// </summary>
        public virtual decimal PenaltyDebtSum { get; set; }

        /// <summary>
        /// Формула расчета пени
        /// </summary>
        public virtual string PenaltyCalcFormula { get; set; }

        /// <summary>
        /// Лицевой счет
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Документ ПИР
        /// </summary>
        public virtual DocumentClw Document { get; set; }
    }
}