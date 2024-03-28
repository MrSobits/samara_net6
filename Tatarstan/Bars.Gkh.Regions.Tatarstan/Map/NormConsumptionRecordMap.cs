namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using System;
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    public class NormConsumptionRecordMap : BaseEntityMap<NormConsumptionRecord>
    {
        public NormConsumptionRecordMap() 
            : base("Нормативы потребления", "GKH_TAT_NORM_CONS_REC")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
            this.Reference(x => x.NormConsumption, "Норматив потребления").Column("NORM_CONS_ID").NotNull().Fetch();
        }
    }
}