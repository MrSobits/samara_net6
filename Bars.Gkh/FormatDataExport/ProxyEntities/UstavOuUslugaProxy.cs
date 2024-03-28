namespace Bars.Gkh.FormatDataExport.ProxyEntities
{
    using System;

    using Bars.B4.DataModels;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Оказываемые услуги по уставу (ustavouusluga.csv)
    /// </summary>
    public class UstavOuUslugaProxy : IHaveId
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
        /// 3. Дата начала предоставления услуги
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 4. Дата окончания предоставления услуги
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 5. Услуга предоставляется в рамках текущего устава
        /// </summary>
        public int IsServiceByThisContract { get; set; } = 1;

        /// <summary>
        /// 6. Файл с протоколом собрания собственников
        /// </summary>
        public FileInfo ProtocolMeetingOwnersFile { get; set; }
    }
}