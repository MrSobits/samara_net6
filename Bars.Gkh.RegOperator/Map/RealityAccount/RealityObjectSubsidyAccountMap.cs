/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.RealityAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class RealityObjectSubsidyAccountMap : BaseImportableEntityMap<RealityObjectSubsidyAccount>
///     {
///         public RealityObjectSubsidyAccountMap()
///             : base("REGOP_RO_SUBSIDY_ACCOUNT")
///         {
///             Map(x => x.DateOpen, "DATE_OPEN", true);
///             Map(x => x.AccountNumber, "ACC_NUM");
/// 
///             References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Счет cубсидий дома"</summary>
    public class RealityObjectSubsidyAccountMap : BaseImportableEntityMap<RealityObjectSubsidyAccount>
    {
        
        public RealityObjectSubsidyAccountMap() : 
                base("Счет cубсидий дома", "REGOP_RO_SUBSIDY_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.AccountNumber, "Номер счета").Column("ACC_NUM").Length(250);
            Property(x => x.DateOpen, "Дата открытия счета").Column("DATE_OPEN").NotNull();
            Reference(x => x.RealityObject, "Объект недвижимости").Column("RO_ID").NotNull().Fetch();
        }
    }
}
