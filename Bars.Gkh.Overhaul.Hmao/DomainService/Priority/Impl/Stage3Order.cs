namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using Entities;

    public class Stage3Order
    {
        public long Id;

        public int Year;

        public int? BuildYear;

        public IStage3Entity Stage3;

        public Dictionary<string, object> OrderDict = new Dictionary<string, object>();

        public DateTime? PrivatizDate;

        public long RoSeId;

        public long RoId;

        public long CeoId;

        public decimal? AreaLiving;

        public int? NumberLiving;

        public decimal? PhysicalWear;

        public DateTime? DateTechInspection;

        public DateTime? DateCommissioning;
    }
}