namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;


    /// <summary>Маппинг для "Связи Расчета категории риска с МКД в управлении"</summary>
    public class ROMCategoryMKDMap : BaseEntityMap<ROMCategoryMKD>
    {
        
        public ROMCategoryMKDMap() : 
                base("Дома в управлении рассчитываемой организации", "GJI_CH_ROM_CATEGORY_ROBJECT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DateStart, "Дата начала управления").Column("DATE_START");
            Reference(x => x.RealityObject, "МКД").Column("RO_ID").NotNull().Fetch();
            Reference(x => x.ROMCategory, "Расчет").Column("ROM_CATEGORY_ID").NotNull().Fetch();

        }
    }
}
