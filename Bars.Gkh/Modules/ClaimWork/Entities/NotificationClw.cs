namespace Bars.Gkh.Modules.ClaimWork.Entities
{
    using System;
    using B4.Modules.FileStorage;

    /// <summary>
    /// Уведомление ПИР
    /// </summary>
    public class NotificationClw : DocumentClw
    {
        /// <summary>
        /// Дата отправки
        /// </summary>
        public virtual DateTime? SendDate { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Срок устранения
        /// </summary>
        public virtual DateTime? DateElimination { get; set; }

        /// <summary>
        /// Способ устранения
        /// </summary>
        public virtual string EliminationMethod { get; set; }
    }
}