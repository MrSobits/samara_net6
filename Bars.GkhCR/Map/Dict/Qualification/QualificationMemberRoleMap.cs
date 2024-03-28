/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Роли участника квалификационного отбора"
///     /// </summary>
///     public class QualificationMemberRoleMap : BaseGkhEntityMap<QualificationMemberRole>
///     {
///         public QualificationMemberRoleMap() : base("CR_DICT_QMEM_ROLE")
///         {
///             References(x => x.QualificationMember, "QUAL_MEMBER_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Role, "ROLE_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Роли участника квалификационного отбора"</summary>
    public class QualificationMemberRoleMap : BaseImportableEntityMap<QualificationMemberRole>
    {
        
        public QualificationMemberRoleMap() : 
                base("Роли участника квалификационного отбора", "CR_DICT_QMEM_ROLE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.QualificationMember, "Период").Column("QUAL_MEMBER_ID").NotNull().Fetch();
            Reference(x => x.Role, "Роль").Column("ROLE_ID").NotNull().Fetch();
        }
    }
}
