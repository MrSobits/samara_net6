namespace Bars.Gkh.ClaimWork.Entities
{
    using System;
    using B4.Modules.FileStorage;
    using Modules.ClaimWork.Entities;
    using Modules.ClaimWork.Enums;

    /// <summary>
    /// Претензия
    /// </summary>
    public class PretensionClw : DocumentClw
    {
        /// <summary>
        /// Дата рассмотрения
        /// </summary>/
        public virtual DateTime? DateReview { get; set; }

        /// <summary>
        /// Сумма долга по базовому тарифу
        /// </summary>
        public virtual decimal DebtBaseTariffSum { get; set; }

        /// <summary>
        /// Сумма долга по тарифу решения
        /// </summary>
        public virtual decimal DebtDecisionTariffSum { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Пени
        /// </summary>
        public virtual decimal Penalty { get; set; }
        
        /// <summary>
        /// Расчет суммы претензии (пени)
        /// </summary>
        public virtual string SumPenaltyCalc { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Дата отправки
        /// </summary>
        public virtual DateTime? SendDate { get; set; }

        /// <summary>
        /// Удовлетворение требований
        /// </summary>
        public virtual RequirementSatisfaction RequirementSatisfaction { get; set; }

        /// <summary>
        /// Номер претензии
        /// </summary>
        public virtual string NumberPretension { get; set; }
    }
}