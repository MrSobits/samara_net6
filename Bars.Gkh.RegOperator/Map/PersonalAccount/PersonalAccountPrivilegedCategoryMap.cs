/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.PersonalAccount
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities.PersonalAccount;
/// 
///     public class PersonalAccountPrivilegedCategoryMap : BaseImportableEntityMap<PersonalAccountPrivilegedCategory>
///     {
///         public PersonalAccountPrivilegedCategoryMap()
///             : base("REGOP_PERS_ACC_PRIV_CAT")
///         {
///             Map(x => x.DateFrom, "DATE_FROM", true);
///             Map(x => x.DateTo, "DATE_TO", false);
///             References(x => x.PersonalAccount, "ACC_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///             References(x => x.PrivilegedCategory, "PRIV_CAT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.PersonalAccount.PersonalAccountPrivilegedCategory"</summary>
    public class PersonalAccountPrivilegedCategoryMap : BaseImportableEntityMap<PersonalAccountPrivilegedCategory>
    {
        
        public PersonalAccountPrivilegedCategoryMap() : 
                base("Bars.Gkh.RegOperator.Entities.PersonalAccount.PersonalAccountPrivilegedCategory", "REGOP_PERS_ACC_PRIV_CAT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PersonalAccount, "PersonalAccount").Column("ACC_ID").NotNull().Fetch();
            Reference(x => x.PrivilegedCategory, "PrivilegedCategory").Column("PRIV_CAT_ID").NotNull().Fetch();
            Property(x => x.DateFrom, "DateFrom").Column("DATE_FROM").NotNull();
            Property(x => x.DateTo, "DateTo").Column("DATE_TO");
        }
    }
}
