/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     public class ViewActivityTsjMap : PersistentObjectMap<ViewActivityTsj>
///     {
///         public ViewActivityTsjMap() : base("VIEW_GJI_ACTIVITY_TSJ")
///         {
///             Map(x => x.ManOrgName, "CONTRAGENT_NAME");
///             Map(x => x.MunicipalityName, "MU_NAME");
///             Map(x => x.Inn, "INN");
///             Map(x => x.HasStatute, "HAS_STATUTE");
///             Map(x => x.ContragentId, "CONTRAGENT_ID");
///             Map(x => x.MunicipalityId, "MU_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Вьюха деятельности тсж"</summary>
    public class ViewActivityTsjMap : PersistentObjectMap<ViewActivityTsj>
    {
        
        public ViewActivityTsjMap() : 
                base("Вьюха деятельности тсж", "VIEW_GJI_ACTIVITY_TSJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ManOrgName, "Наименование управляющей организации").Column("CONTRAGENT_NAME");
            Property(x => x.MunicipalityName, "Муниципальное образование").Column("MU_NAME");
            Property(x => x.Inn, "ИНН контрагента управляющей организации").Column("INN");
            Property(x => x.HasStatute, "Наличие устава").Column("HAS_STATUTE");
            Property(x => x.ContragentId, "Идентификатор контрагента").Column("CONTRAGENT_ID");
            Property(x => x.MunicipalityId, "Идентификатор муниципального образования контрагента").Column("MU_ID");
        }
    }
}
