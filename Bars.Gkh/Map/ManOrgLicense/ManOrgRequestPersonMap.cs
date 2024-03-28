/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.B4.DataAccess;
/// 
///     /// <summary>
///     /// связь должностного лица и заявки на получнеие лицензии
///     /// </summary>
///     public class ManOrgRequestPersonMap : BaseImportableEntityMap<ManOrgRequestPerson>
///     {
///         public ManOrgRequestPersonMap()
///             : base("GKH_MANORG_REQ_PERSON")
///         {
///             References(x => x.LicRequest, "LIC_REQUEST_ID").Not.Nullable().LazyLoad();
///             References(x => x.Person, "PERSON_ID").Not.Nullable().LazyLoad();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Должностное лицо заявки на лицензию"</summary>
    public class ManOrgRequestPersonMap : BaseImportableEntityMap<ManOrgRequestPerson>
    {
        
        public ManOrgRequestPersonMap() : 
                base("Должностное лицо заявки на лицензию", "GKH_MANORG_REQ_PERSON")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.LicRequest, "Заявка на лицензию").Column("LIC_REQUEST_ID").NotNull();
            Reference(x => x.Person, "Должностное лицо").Column("PERSON_ID").NotNull();
        }
    }
}
