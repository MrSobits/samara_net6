namespace Bars.Gkh.Entities
{
    using System;
    using B4.Modules.Security;
    using Bars.B4.DataAccess;
    using Enums;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class LogOperation : BaseEntity
    {
        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Время старта
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// Время окончания
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Comment { get; set; }

        /// <summary>
        /// Файл лога
        /// </summary>
        public virtual FileInfo LogFile { get; set; }

        /// <summary>
        /// Тип операций
        /// </summary>
        public virtual LogOperationType OperationType { get; set; }
    }
}