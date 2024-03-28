namespace Bars.Gkh.Map.RealityObj
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    public class EntranceMap : BaseImportableEntityMap<Entrance>
    {
        public EntranceMap()
            : base("Подъезд", "GKH_ENTRANCE")
        {
        }

        protected override void Map()
        {
            Property(x => x.Number, "Номер подъезда").Column("CNUMBER");

            Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
            Reference(x => x.RealEstateType, "Тип дома").Column("RET_ID").Fetch();
        }
    }
}