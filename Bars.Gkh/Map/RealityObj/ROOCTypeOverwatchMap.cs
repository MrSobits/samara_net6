namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    public class ROOCTypeOverwatchMap : BaseEntityMap<ROOCTypeOverwatch>
    {
        public ROOCTypeOverwatchMap() : 
                base("Виды видеонаблюдения на доме", "GKH_RO_OVERWATCH_CGNT_TYPE")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.RealityObjectOverwatchContragent, "Жилой дом").Column("ROOC_ID").NotNull().Fetch();
            this.Reference(x => x.VideoOverwatchType, "Тип наблюдения").Column("TYPE_ID").NotNull().Fetch();
        }
    }
}
