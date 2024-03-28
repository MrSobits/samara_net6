namespace Bars.GkhGji.Regions.Smolensk.Entities
{
    using System;
    using GkhGji.Entities;

    public class ActRemovalSmol : ActRemoval
    {
        /// <summary>
        /// Дата уведомления о проверке
        /// </summary>
        public virtual DateTime? DateNotification { get; set; }

        /// <summary>
        /// Номер уведомления о проверке
        /// </summary>
        public virtual string NumberNotification { get; set; }
    }
}
