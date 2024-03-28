namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>Маппинг для "Категория потребителей, приравненных к населению"</summary>
    public class CategoryConsumersEqualPopulationMap : BaseEntityMap<CategoryConsumersEqualPopulation>
    {
        public CategoryConsumersEqualPopulationMap() : 
                base("Категория потребителей, приравненных к населению", "GKH_DICT_CATEGORY_CONSUMERS_EQUAL_POPULATION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(255).NotNull();
        }
    }
}
