namespace Bars.Gkh.RegOperator.Imports.DebtorClaimWork
{
    using System;
    using System.ComponentModel;

    using Bars.B4.Utils;

    /// <summary>
    /// Запись импорта ПИР
    /// </summary>
    public sealed class DebtorClaimWorkRecord
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="rowNumber"></param>
        public DebtorClaimWorkRecord(int rowNumber)
        {
            this.RowNumber = rowNumber;
        }

        /// <summary>
        /// LS
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// AMOUNT_DUE
        /// </summary>
        public decimal DebtSum { get; set; }

        /// <summary>
        /// AMOUNT_DUE_PENI
        /// </summary>
        public decimal PenaltyDebt { get; set; }

        /// <summary>
        /// START_DATE
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// TYPE_OF_APPLICATION
        /// </summary>
        public ApplicationType ApplicationType { get; set; }

        /// <summary>
        /// APPLICATION_DATE
        /// </summary>
        public DateTime? ApplicationDate { get; set; }

        /// <summary>
        /// AMOUNT_OF_DEBT
        /// </summary>
        public decimal? LawsuitDebtSum { get; set; }

        /// <summary>
        /// AMOUNT_OF_PENI
        /// </summary>
        public decimal? LawsuitPenaltyDebt { get; set; }

        /// <summary>
        /// Номер строки
        /// </summary>
        public int RowNumber { get; internal set; }

        /// <summary>
        /// Признак корректности строки
        /// </summary>
        public bool IsValidRecord { get; internal set; }
    }

    public enum ApplicationType
    {
        /// <summary>
        /// Без создания документов
        /// </summary>
        [Display("Без создания документов")]
        NoDocuments = 0,

        /// <summary>
        /// Заявление о выдаче судебного приказа
        /// </summary>
        [Display("Заявление о выдаче судебного приказа")]
        CourtOrderClaim = 1,

        /// <summary>
        /// Исковое заявление
        /// </summary>
        [Display("Исковое заявление")]
        Lawsuit = 2
    }
}