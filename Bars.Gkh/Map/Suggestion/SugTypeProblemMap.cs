namespace Bars.Gkh.Map.Suggestion
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Suggestion;
    
    
    /// <summary>Маппинг для "ТИп проблемы"</summary>
    public class SugTypeProblemMap : BaseEntityMap<SugTypeProblem>
    {
        
        public SugTypeProblemMap() : 
                base("Тип проблемы с шаблонами", "GKH_DICT_TYPE_PROBLEM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Name, "Name").Column("NAME");
            Property(x => x.RequestTemplate, "RequestTemplate").Column("REQUEST_TEMPLATE");
            Property(x => x.ResponceTemplate, "ResponceTemplate").Column("RESPONCE_TEMPLATE");
            Reference(x => x.Rubric, "Rubric").Column("RUBRIC_ID").NotNull().Fetch();
        }
    }
}
