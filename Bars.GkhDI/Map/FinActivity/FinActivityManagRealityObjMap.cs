/// <mapping-converter-backup>
/// namespace Bars.GkhDi.Map
/// {
///     using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
///     using Bars.GkhDi.Entities;
///     using Bars.B4.DataAccess;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Управление по домам финансовой деятельности"
///     /// </summary>
///     public class FinActivityManagRealityObjMap : BaseGkhEntityMap<FinActivityManagRealityObj>
///     {
///         public FinActivityManagRealityObjMap(): base("DI_DISINFO_FINACT_REALOBJ")
///         {
///             Map(x => x.PresentedToRepay, "PRESENTED_TO_REPAY");
///             Map(x => x.ReceivedProvidedService, "RECEIVED_PROVIDED_SERV");
///             Map(x => x.SumDebt, "SUM_DEBT");
///             Map(x => x.SumFactExpense, "SUM_FACT_EXPENSE");
///             Map(x => x.SumIncomeManage, "SUM_INCOME_MANAGE");
/// 
///             References(x => x.DisclosureInfo, "DISINFO_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJ_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhDi.Map
{
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map; using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhDi.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhDi.Entities.FinActivityManagRealityObj"</summary>
    public class FinActivityManagRealityObjMap : BaseImportableEntityMap<FinActivityManagRealityObj>
    {
        
        public FinActivityManagRealityObjMap() : 
                base("Bars.GkhDi.Entities.FinActivityManagRealityObj", "DI_DISINFO_FINACT_REALOBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.PresentedToRepay, "PresentedToRepay").Column("PRESENTED_TO_REPAY");
            Property(x => x.ReceivedProvidedService, "ReceivedProvidedService").Column("RECEIVED_PROVIDED_SERV");
            Property(x => x.SumDebt, "SumDebt").Column("SUM_DEBT");
            Property(x => x.SumFactExpense, "SumFactExpense").Column("SUM_FACT_EXPENSE");
            Property(x => x.SumIncomeManage, "SumIncomeManage").Column("SUM_INCOME_MANAGE");
            Reference(x => x.DisclosureInfo, "DisclosureInfo").Column("DISINFO_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("REALITY_OBJ_ID").Fetch();
        }
    }
}
