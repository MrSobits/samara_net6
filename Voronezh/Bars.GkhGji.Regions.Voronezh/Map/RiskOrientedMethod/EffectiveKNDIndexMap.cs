namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;


    /// <summary>Маппинг для "справочника видов КНД"</summary>
    public class EffectiveKNDIndexMap : BaseEntityMap<EffectiveKNDIndex>
    {
        
        public EffectiveKNDIndexMap() : 
                base("Показатели эффективности КНД", "GJI_CH_EFFECTIVE_INDEX")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.KindKND, "Тип КНД").Column("TYPE_KND");
            Property(x => x.YearEnums, "Рассчетный год").Column("YEAR_ENUM");
            Property(x => x.CurrentIndex, "Текущий индекс").Column("CURRENT_INDEX");
            Property(x => x.Code, "Код записи").Column("CODE");
            Property(x => x.Name, "Наименование показателя").Column("NAME");
            Property(x => x.TargetIndex, "Целеаой индекс").Column("TARGET_INDEX");

        }
    }
}
