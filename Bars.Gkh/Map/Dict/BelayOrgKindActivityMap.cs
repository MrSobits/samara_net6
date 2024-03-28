/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Dict
/// {
///     using Bars.Gkh.Entities.Dicts;
/// 
///     /// <summary>
///     /// Маппинг сущности "Вид деятельности страховой организации"
///     /// </summary>
///     public class BelayOrgKindActivityMap : BaseGkhEntityMap<BelayOrgKindActivity>
///     {
///         public BelayOrgKindActivityMap() : base("GKH_DICT_BELAY_KIND_ACTIV")
///         {
///             Map(x => x.Name, "NAME").Not.Nullable().Length(300);
///             Map(x => x.Code, "CODE").Not.Nullable().Length(300);
///             Map(x => x.Description, "DESCRIPTION").Length(1000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Dicts
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;
    
    
    /// <summary>Маппинг для "Вид деятельности страховой организации"</summary>
    public class BelayOrgKindActivityMap : BaseImportableEntityMap<BelayOrgKindActivity>
    {
        
        public BelayOrgKindActivityMap() : 
                base("Вид деятельности страховой организации", "GKH_DICT_BELAY_KIND_ACTIV")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            Property(x => x.Code, "Код").Column("CODE").Length(300).NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(1000);
        }
    }
}
