namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Оказываемые услуги по договору управления (duouusluga.csv)
    /// </summary>
    public class DuOuUslugaProxy : IHaveId
    {
        /// <summary>
        /// Уникальный код
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 1. Объект управления
        /// </summary>
        public long? OuId { get; set; }

        /// <summary>
        /// 2. Услуга
        /// </summary>
        public long? ServiceId { get; set; }

        /// <summary>
        /// 3. Дата начала предоставления услуг дому
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 4. Дата окончания предоставления услуг дому
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 5. Услуга предоставляется в рамках текущего устава
        /// </summary>
        public int IsServiceByThisContract { get; set; }

        /// <summary>
        /// 6. Ссылка на файл с дополнительным соглашением на оказание услуги
        /// </summary>
        public FileInfo AttachmentFile { get; set; }
    }
}