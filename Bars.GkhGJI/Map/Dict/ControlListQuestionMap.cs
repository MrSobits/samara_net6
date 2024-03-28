
namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.Dict.ControlActivity"</summary>
    public class ControlListQuestionMap : BaseEntityMap<ControlListQuestion>
    {
        
        public ControlListQuestionMap() : 
                base("Bars.GkhGji.Entities.Dict.ControlActivity", "GJI_DICT_CONTROL_LIST_QUESTION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(3500).NotNull();
            Property(x => x.NPDName, "Наименование NPD").Column("NPD_NAME").Length(3500).NotNull();
            Property(x => x.Description, "Наименование NPD").Column("DESCRIPTION").Length(1500).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Reference(x => x.ControlList, "ControlList").Column("LIST_ID").NotNull().Fetch();
            Property(x => x.ERKNMGuid, "Код").Column("ERKNM_GUID").Length(50);
        }
    }
}
