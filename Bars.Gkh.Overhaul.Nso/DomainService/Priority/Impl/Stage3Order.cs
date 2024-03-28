namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using Entities;

    public class Stage3Order
    {
        public long Id;

        public int Year;

        public IStage3Entity Stage3;

        public Dictionary<string, object> OrderDict = new Dictionary<string, object>();

        public long RoId;

        public long CeoId;

        public string RoCeoKey;

        public decimal? AreaLiving;

        public int? NumberLiving;

        public DateTime? PrivatizDate;

        public int? BuildYear;

        public decimal? PhysicalWear;

        public DateTime? DateTechInspection;

        public DateTime? DateCommissioning;
    }

    public class Stage1Node
    {
        public long Stage3Id { get; set; }
        public int PlanYear { get; set; }
        public long RoId { get; set; }
        public long RoStructElId { get; set; }
        public int Lifetime { get; set; }
        public int LifetimeAfterRepair { get; set; }
        public int OverhaulYear { get; set; }
        public int? BuildYear { get; set; }
    }
}