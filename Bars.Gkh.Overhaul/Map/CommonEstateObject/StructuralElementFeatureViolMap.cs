namespace Bars.Gkh.Overhaul.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Entities;


    /// <summary>Маппинг для "Характеристики нарушений"</summary>
    public class StructuralElementFeatureViolMap : BaseEntityMap<StructuralElementFeatureViol>
    {
        
        public StructuralElementFeatureViolMap() : 
                base("Характеристики нарушений", "OVRHL_STRUCT_EL_FEATURE_VIOL")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.StructuralElement, "Конструктивный элемент").Column("STRUCT_EL_ID").NotNull().Fetch();
            this.Reference(x => x.FeatureViol, "Группа нарушений").Column("FEATURE_VIOL_ID").NotNull().Fetch();
        }
    }
}
