namespace Bars.GkhGji.Regions.Smolensk.Entities
{
    using System;

    using Bars.B4.DataAccess;

    using Enums;
    using GkhGji.Entities;

    public class PrescriptionViolDescription : BaseEntity
    {
        public virtual PrescriptionViol PrescriptionViol { get; set; }

        public virtual byte[] Description { get; set; }

        public virtual byte[] Action { get; set; }
    }
}