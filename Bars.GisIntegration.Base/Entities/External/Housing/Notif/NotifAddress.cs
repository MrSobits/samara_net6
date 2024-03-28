namespace Bars.GisIntegration.Base.Entities.External.Housing.Notif
{
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Housing.House;

    /// <summary>
    /// Адресаты сообщения
    /// </summary>
    public class NotifAddress : BaseEntity
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
        /// Дом
        /// </summary>
        public virtual House House { get; set; }

        /// <summary>
        /// Помещение 
        /// </summary>
        public virtual Premise Premise { get; set; }

        /// <summary>
        /// Пользователь 
        /// </summary>
        public virtual int ChangedBy { get; set; }
    }
}
