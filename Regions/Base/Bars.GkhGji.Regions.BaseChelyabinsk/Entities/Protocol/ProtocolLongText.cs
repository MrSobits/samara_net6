namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol
{
    using System;

    using Bars.B4.DataAccess;

    public class ProtocolLongText : BaseEntity
    {
        public virtual GkhGji.Entities.Protocol Protocol { get; set; }

        [Obsolete("Удалено из визуального представления")]
        public virtual byte[] Description { get; set; }

        public virtual byte[] Witnesses { get; set; }

        public virtual byte[] Victims { get; set; }
    }
}