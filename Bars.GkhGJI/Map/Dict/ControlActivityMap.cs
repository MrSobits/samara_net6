/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map.Dict
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities.Dict;
/// 
///     class ControlActivityMap : BaseGkhEntityMap<ControlActivity>
///     {
///         public ControlActivityMap() : base("GJI_DICT_CON_ACTIVITY")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Code, "CODE").Length(300);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities.Dict;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Entities.Dict.ControlActivity"</summary>
    public class ControlActivityMap : BaseEntityMap<ControlActivity>
    {
        
        public ControlActivityMap() : 
                base("Bars.GkhGji.Entities.Dict.ControlActivity", "GJI_DICT_CON_ACTIVITY")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Property(x => x.ERKNMGuid, "Код").Column("ERKNM_GUID").Length(50);
        }
    }
}
