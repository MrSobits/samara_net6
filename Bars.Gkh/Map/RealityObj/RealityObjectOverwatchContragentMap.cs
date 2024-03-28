namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    public class RealityObjectOverwatchContragentMap : BaseEntityMap<RealityObjectOverwatchContragent>
    {
        public RealityObjectOverwatchContragentMap() : 
                base("Организация видеонаблюдения", "GKH_REALITY_VIDEO_OVERWATCH")
        {
        }

        protected override void Map()
        {
            Property(x => x.DateFrom, "Дата с").Column("DATE_FROM").NotNull();
            Property(x => x.DateTo, "Дата по").Column("DATE_TO");
            this.Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
        }
    }
}
