/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.Gkh.Entities;
///     using Bars.Gkh.Enums;
/// 
///     public class ViewRealityObjectManOrgContractMap : PersistentObjectMap<ViewRealityObjectManOrgContract>
///     {
///         public ViewRealityObjectManOrgContractMap() : base("view_real_obj_manorg_contr")
///         {
///             Map(x => x.ManagingOrgname, "MANORG_NAME").Length(300);
///             Map(x => x.TypeContractManOrgRealObj, "TYPE_CONTRACT").Not.Nullable().CustomType<TypeContractManOrg>();
///             Map(x => x.StartDate, "START_DATE");
///             Map(x => x.EndDate, "END_DATE");
/// 
///             References(x => x.RealityObject, "RO_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ManagingOrganization, "MO_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Представление "Договора с жилым домом""</summary>
    public class ViewRealityObjectManOrgContractMap : PersistentObjectMap<ViewRealityObjectManOrgContract>
    {
        
        public ViewRealityObjectManOrgContractMap() : 
                base("Представление \"Договора с жилым домом\"", "VIEW_REAL_OBJ_MANORG_CONTR")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ManagingOrgname, "Наименование упр орг-ии (контрагента)").Column("MANORG_NAME").Length(300);
            Property(x => x.TypeContractManOrgRealObj, "Тип договора с упр орг").Column("TYPE_CONTRACT").NotNull();
            Property(x => x.StartDate, "Дата начала договора").Column("START_DATE");
            Property(x => x.EndDate, "Дата окончания договора").Column("END_DATE");
            Reference(x => x.RealityObject, "Муниципальное образование").Column("RO_ID").NotNull().Fetch();
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MO_ID").Fetch();
        }
    }
}
