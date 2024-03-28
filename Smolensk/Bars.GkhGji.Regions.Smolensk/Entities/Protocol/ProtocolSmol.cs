namespace Bars.GkhGji.Regions.Smolensk.Entities.Protocol
{
    using System;

    using GkhGji.Entities;

    public class ProtocolSmol : Protocol
    {
        /// <summary>
        /// Номер уведомления
        /// </summary>
        public virtual string NoticeDocNumber { get; set; }

        /// <summary>
        /// Дата уведомления
        /// </summary>
        public virtual DateTime? NoticeDocDate { get; set; }

        /// <summary>
        /// Описание нарушений
        /// </summary>
        public virtual string ViolationDescription { get; set; }

        /// <summary>
        /// Объяснения, заявления, замечания
        /// </summary>
        public virtual string ExplanationsComments { get; set; }
    }
}
