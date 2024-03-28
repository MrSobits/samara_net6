/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Map
/// {
///     using Bars.Gkh.Map;;
///     using Bars.GkhCr.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Участник квалификационного отбора"
///     /// </summary>
///     public class QualificationMemberMap : BaseGkhEntityMap<QualificationMember>
///     {
///         public QualificationMemberMap() : base("CR_DICT_QUAL_MEMBER")
///         {
///             Map(x => x.IsPrimary, "IS_PRIMARY");
///             Map(x => x.Name, "NAME").Length(300);
/// 
///             References(x => x.Period, "PERIOD_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Role, "ROLE_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Участник квалификационного отбора"</summary>
    public class QualificationMemberMap : BaseImportableEntityMap<QualificationMember>
    {
        
        public QualificationMemberMap() : 
                base("Участник квалификационного отбора", "CR_DICT_QUAL_MEMBER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.IsPrimary, "Является основным").Column("IS_PRIMARY");
            Property(x => x.Name, "Наименование").Column("NAME").Length(300);
            Reference(x => x.Period, "Период").Column("PERIOD_ID").NotNull().Fetch();
            Reference(x => x.Role, "Роль").Column("ROLE_ID").Fetch();
        }
    }
}
