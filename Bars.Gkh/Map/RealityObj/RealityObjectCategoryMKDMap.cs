namespace Bars.Gkh.Map.RealityObj
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    public class RealityObjectCategoryMKDMap : BaseEntityMap<RealityObjectCategoryMKD>
    {
        public RealityObjectCategoryMKDMap()
            : base("Категории МКД", "GKH_RO_CATEGORYCS")
        {
        }

        protected override void Map()
        {
           
            Reference(x => x.RealityObject, "Жилой дом").Column("RO_ID").NotNull().Fetch();
            Reference(x => x.CategoryCSMKD, "Тип дома").Column("CATEGOTY_ID").Fetch();
        }
    }
}