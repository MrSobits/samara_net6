namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Hmao.Entities;


    /// <summary>Маппинг для "Критерии для актуализации регпрограммы"</summary>
    public class CriteriaForActualizeVersionMap : BaseEntityMap<CriteriaForActualizeVersion>
    {
        
        public CriteriaForActualizeVersionMap() : 
                base("Критерии для актуализации регпрограммы", "OVRHL_DICT_CRITERIA_FOR_ACTUALIZE_VERSION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.CriteriaType, "Наименование критерия").Column("CRITERIA_TYPE").NotNull();
            Property(x => x.ValueFrom, "Нижнее пороговое значение").Column("VALUE_FROM").NotNull();
            Property(x => x.ValueTo, "Верхнее пороговое значение").Column("VALUE_TO").NotNull();
            Property(x => x.Points, "Количество баллов").Column("POINTS").NotNull();
            Property(x => x.Weight, "Весовой коэффициент").Column("WEIGHT").NotNull();
        }
    }
}
