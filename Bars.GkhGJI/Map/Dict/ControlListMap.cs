
namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.Dict.ControlActivity"</summary>
    public class ControlListMap : BaseEntityMap<ControlList>
    {
        
        public ControlListMap() : 
                base("Bars.GkhGji.Entities.Dict.ControlActivity", "GJI_DICT_CONTROL_LIST")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(1500).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Property(x => x.KindKNDGJI, "Вид контроля/надзора").Column("KIND_KND").NotNull();
            Property(x => x.ActualFrom, "Дата начала актуальности").Column("DATE_START").NotNull();
            Property(x => x.ActualTo, "Дата окончания актуальности").Column("DATE_END");
            Property(x => x.ERKNMGuid, "Код").Column("ERKNM_GUID").Length(50);

        }
    }
}
