namespace Bars.GisIntegration.Base.Entities.External.Housing.Notif
{
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Attachment;

    /// <summary>
    /// Уведомление
    /// </summary>
    public class NotifDoc : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }

        /// <summary>
        /// Уведомление
        /// </summary>
        public virtual Notif Notif { get; set; }

        /// <summary>
        /// Приложение
        /// </summary>
        public virtual ExtAttachment Attachment { get; set; }

        /// <summary>
        /// Пользователь 
        /// </summary>
        public virtual int ChangedBy { get; set; }
    }
}