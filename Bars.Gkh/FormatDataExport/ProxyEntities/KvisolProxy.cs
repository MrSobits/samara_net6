namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;

    /// <summary>
    /// 2.16.12 Результаты квитирования (kvisol.csv)
    /// </summary>
    public class KvisolProxy : IHaveId
    {
        /// <summary>
        /// 1. Уникальный код записи
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 2. Код оплаты ЖКУ
        /// </summary>
        public long? OplataId { get; set; }

        /// <summary>
        /// 3. Результат квитирования
        /// </summary>
        public int? Result { get; set; }

        /// <summary>
        /// 4. Расчетный период платежного документа
        /// </summary>
        public DateTime? CalculationPeriod { get; set; }

        /// <summary>
        /// 5. Уникальный номер платежного документа
        /// </summary>
        public long? EpdId { get; set; }

        /// <summary>
        /// 6. Причина, по которой отсутствует возможность сопоставления
        /// </summary>
        public string ReasonImpossibility { get; set; }

        /// <summary>
        /// 7. Статус квитирования
        /// </summary>
        public int? State { get; set; }

        /// <summary>
        /// 8. Сумма квитирования (в копейках)
        /// </summary>
        public decimal Sum { get; set; }
    }
}