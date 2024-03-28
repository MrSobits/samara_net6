namespace Bars.Gkh.ClaimWork.Entities
{
    using System;

    using B4.Modules.FileStorage;

    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// Реструктуризация долга
    /// </summary>
    public class RestructDebt : DocumentClw
    {
        /// <summary>
        /// Сумма задолженности по базовому тарифу
        /// </summary>
        public virtual decimal BaseTariffDebtSum { get; set; }

        /// <summary>
        /// Сумма задолженности по тарифу решения
        /// </summary>
        public virtual decimal DecisionTariffDebtSum { get; set; }

        /// <summary>
        /// Сумма задолженности
        /// </summary>
        public virtual decimal DebtSum { get; set; }

        /// <summary>
        /// Сумма задолженности по пени
        /// </summary>
        public virtual decimal PenaltyDebtSum { get; set; }

        /// <summary>
        /// Сумма реструктуризации
        /// </summary>
        public virtual decimal RestructSum { get; set; }

        /// <summary>
        /// в т.ч. проценты (руб.)
        /// </summary>
        public virtual decimal? PercentSum { get; set; }

        /// <summary>
        /// Файл документа
        /// </summary>
        public virtual FileInfo DocFile { get; set; }

        /// <summary>
        /// Файл графика платежей
        /// </summary>
        public virtual FileInfo PaymentScheduleFile { get; set; }

        /// <summary>
        /// Статус договора
        /// </summary>
        public virtual RestructDebtDocumentState DocumentState { get; set; }

        /// <summary>
        /// Дата расторжения
        /// </summary>
        public virtual DateTime? TerminationDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string TerminationNumber { get; set; }

        /// <summary>
        /// Документ-основание
        /// </summary>
        public virtual FileInfo TerminationFile { get; set; }

        /// <summary>
        /// "Расторжение договора" Причина
        /// </summary>
        public virtual string TerminationReason { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public virtual string Reason { get; set; }
    }
}