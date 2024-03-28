/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Учет платежных документов по начислениям и оплатам на КР"
///     /// </summary>
///     public class ConfirmContributionMap : BaseEntityMap<ConfirmContribution>
///     {
///         public ConfirmContributionMap() : base("REGOP_CONFCONTRIB")
///         {
///             References(x => x.ManagingOrganization, "MANAGORG_ID", ReferenceMapConfig.NotNull);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Regions.Tatarstan.Entities.ConfirmContribution"</summary>
    public class ConfirmContributionMap : BaseEntityMap<ConfirmContribution>
    {
        
        public ConfirmContributionMap() : 
                base("Bars.Gkh.RegOperator.Regions.Tatarstan.Entities.ConfirmContribution", "REGOP_CONFCONTRIB")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ManagingOrganization, "ManagingOrganization").Column("MANAGORG_ID").NotNull();
        }
    }
}
