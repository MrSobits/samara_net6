namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;

    /// <summary>
    /// Пачка оплат ЖКУ
    /// </summary>
    public class OplataPackProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код пачки
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Дата платежного поручения
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 3. Номер платежного поручения
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 4. Сумма платежей
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// 5. Количество платежей, вошедших в пачку
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 6. Статус записи
        /// </summary>
        public int State => 1; // Действует

        #region OPLATA
        /// <summary>
        /// Гуид
        /// </summary>
        public string TransferGuid { get; set; }

        /// <summary>
        /// OPLATA 14. Наименование плательщика
        /// </summary>
        public string PayerName { get; set; }

        /// <summary>
        /// OPLATA 15. Назначение платежа
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// OPLATA 6. Дата учета
        /// </summary>
        public DateTime? OperationDate { get; set; }
        #endregion
    }
}