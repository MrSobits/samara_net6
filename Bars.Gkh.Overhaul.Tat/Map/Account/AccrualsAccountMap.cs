/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.Gkh.Overhaul.Tat.Entities;
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
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.AccrualsAccount"</summary>
    public class AccrualsAccountMap : JoinedSubClassMap<AccrualsAccount>
    {
        
        public AccrualsAccountMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.AccrualsAccount", "OVRHL_ACCRUALS_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.OpeningBalance, "OpeningBalance").Column("OPENING_BALANCE");
        }
    }
}
