namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Imports
{
    using System;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Запись импорта начислений должников ПИР
    /// </summary>
    public sealed class AgentPIRDebtorCreditRecord
    {
        /// <summary>
        /// Конструктор, принимающий строку таблицы
        /// </summary>
        /// <param name="rowNumber"></param>
        public AgentPIRDebtorCreditRecord(int rowNumber)
        {
            this.RowNumber = rowNumber;
        }

        /// <summary>
        /// Номер ЛС
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Начислено
        /// </summary>
        public decimal? Credit { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public FileInfo File { get; set; }

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