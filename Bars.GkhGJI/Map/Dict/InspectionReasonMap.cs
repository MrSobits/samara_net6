
namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;


    /// <summary>Маппинг для "Bars.GkhGji.Entities.Dict.InspectionReason"</summary>
    public class InspectionReasonMap : BaseEntityMap<InspectionReason>
    {
        
        public InspectionReasonMap() : 
                base("Bars.GkhGji.Entities.Dict.InspectionReason", "GJI_DICT_DECISION_REASON")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
        }
    }
}
