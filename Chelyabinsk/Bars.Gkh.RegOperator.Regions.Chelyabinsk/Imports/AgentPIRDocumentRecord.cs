namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Imports
{
    using System;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Запись импорта документов ПИР
    /// </summary>
    public sealed class AgentPIRDocumentRecord
    {
        /// <summary>
        /// Конструктор, принимающий строку таблицы
        /// </summary>
        /// <param name="rowNumber"></param>
        public AgentPIRDocumentRecord(int rowNumber)
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
        /// Сумма основного долга
        /// </summary>
        public decimal? DebtSum { get; set; }

        /// <summary>
        /// Сумма долга по пени
        /// </summary>
        public decimal? PeniSum { get; set; }

        /// <summary>
        /// Размер госпошлины
        /// </summary>
        public decimal? Duty { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public AgentPIRDocumentType DocumentType { get; set; }

        /// <summary>
        /// ФИО должника
        /// </summary>
        public string FIO { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public string House { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        public string Corp { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        public string Flat { get; set; }

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