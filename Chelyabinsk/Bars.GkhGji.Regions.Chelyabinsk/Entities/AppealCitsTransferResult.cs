namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Enums;

    /// <summary>
    /// Результат обмена данными обращений граждан
    /// </summary>
    public class AppealCitsTransferResult : BaseEntity
    {
        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Тип операции
        /// </summary>
        public virtual AppealCitsTransferType Type { get; set; }

        /// <summary>
        /// Статус операции
        /// </summary>
        public virtual AppealCitsTransferStatus Status { get; set; }

        /// <summary>
        /// Дата и время начала
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// Дата и время окончания
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Файл лога
        /// </summary>
        public virtual FileInfo LogFile { get; set; }

        /// <summary>
        /// Параметры запроса
        /// </summary>
        public virtual BaseParams ExportParams { get; set; }
    }
}