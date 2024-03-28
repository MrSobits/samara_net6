namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    class LivingSquareCostMap : BaseEntityMap<LivingSquareCost>
    {
       

        public  LivingSquareCostMap()
            : base("Bars.Gkh.Entities.Dicts.LivingSquareCost", "GKH_DICT_LIVING_SQUARE_COST")
        {
           
        }

        protected override void Map()
        {
            Reference(x => x.Municipality, "Муниципальное образование").Column("MO_ID").NotNull().Fetch();
            Property(x => x.Cost, "Стоимость").Column("COST");
            Property(x => x.Year, "Год").Column("YEAR");

        }
    }
}
