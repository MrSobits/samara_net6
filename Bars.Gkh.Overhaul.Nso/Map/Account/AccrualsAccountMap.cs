/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map
/// {
///     using Entities;
/// 
///     using FluentNHibernate.Mapping;
/// 
///     public class AccrualsAccountMap : SubclassMap<AccrualsAccount>
///     {
///         public AccrualsAccountMap()
///         {
///             Table("OVRHL_ACCRUALS_ACCOUNT");
///             KeyColumn("ID");
/// 
///             Map(x => x.OpeningBalance, "OPENING_BALANCE");
///             References(x => x.RealityObject, "REALITY_OBJECT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.AccrualsAccount"</summary>
    public class AccrualsAccountMap : JoinedSubClassMap<AccrualsAccount>
    {
        
        public AccrualsAccountMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.AccrualsAccount", "OVRHL_ACCRUALS_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.OpeningBalance, "OpeningBalance").Column("OPENING_BALANCE");
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJECT_ID").NotNull().Fetch();
        }
    }
}
