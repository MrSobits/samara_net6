namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Imports
{
    using System;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Запись импорта погашений ПИР
    /// </summary>
    public sealed class AgentPIRDutyRecord
    {
        /// <summary>
        /// Конструктор, принимающий строку таблицы
        /// </summary>
        /// <param name="rowNumber"></param>
        public AgentPIRDutyRecord(int rowNumber)
        {
            this.RowNumber = rowNumber;
        }

        /// <summary>
        /// Номер ЛС
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public DateTime DocumentDate { get; set; }

        /// <summary>
        /// Погашено
        /// </summary>
        public decimal? Repaid { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public AgentPIRDocumentType DocumentType { get; set; }

        /// <summary>
        /// Номер строки
        /// </summary>
        public int RowNumber { get; internal set; }

        /// <summary>
        /// Признак корректности строки
        /// </summary>
        public bool IsValidRecord { get; internal set; }
    }
}