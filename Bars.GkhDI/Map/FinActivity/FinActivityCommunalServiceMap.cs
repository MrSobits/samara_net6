/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
///     using Bars.GkhDi.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Коммунальные услуги финансовой деятельности"
///     /// </summary>
///     public class FinActivityCommunalServiceMap : BaseGkhEntityMap<FinActivityCommunalService>
///     {
///         public FinActivityCommunalServiceMap(): base("DI_DISINFO_FINACT_COMMUN")
///         {
///             Map(x => x.TypeServiceDi, "TYPE_SERVICE_DI").Not.Nullable().CustomType<TypeServiceDi>();
///             Map(x => x.Exact, "EXACT");
///             Map(x => x.IncomeFromProviding, "INCOME_PROVIDING");
///             Map(x => x.DebtPopulationStart, "DEBT_POPULATION_START");
///             Map(x => x.DebtPopulationEnd, "DEBT_POPULATION_END");
///             Map(x => x.DebtManOrgCommunalService, "DEBT_MANORG_COMMUNAL");
///             Map(x => x.PaidByMeteringDevice, "PAID_METERING_DEVICE");
///             Map(x => x.PaidByGeneralNeeds, "PAID_GENERAL_NEEDS");
///             Map(x => x.PaymentByClaim, "PAYMENT_CLAIM");
/// 
///             References(x => x.DisclosureInfo, "DISINFO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.FinActivityCommunalService"</summary>
    public class FinActivityCommunalServiceMap : BaseImportableEntityMap<FinActivityCommunalService>
    {
        
        public FinActivityCommunalServiceMap() : 
                base("Bars.GkhDi.Entities.FinActivityCommunalService", "DI_DISINFO_FINACT_COMMUN")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypeServiceDi, "TypeServiceDi").Column("TYPE_SERVICE_DI").NotNull();
            Property(x => x.Exact, "Exact").Column("EXACT");
            Property(x => x.IncomeFromProviding, "IncomeFromProviding").Column("INCOME_PROVIDING");
            Property(x => x.DebtPopulationStart, "DebtPopulationStart").Column("DEBT_POPULATION_START");
            Property(x => x.DebtPopulationEnd, "DebtPopulationEnd").Column("DEBT_POPULATION_END");
            Property(x => x.DebtManOrgCommunalService, "DebtManOrgCommunalService").Column("DEBT_MANORG_COMMUNAL");
            Property(x => x.PaidByMeteringDevice, "PaidByMeteringDevice").Column("PAID_METERING_DEVICE");
            Property(x => x.PaidByGeneralNeeds, "PaidByGeneralNeeds").Column("PAID_GENERAL_NEEDS");
            Property(x => x.PaymentByClaim, "PaymentByClaim").Column("PAYMENT_CLAIM");
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
        }
    }
}
