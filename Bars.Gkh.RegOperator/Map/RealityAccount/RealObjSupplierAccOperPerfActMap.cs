/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.RealityAccount
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class RealObjSupplierAccOperPerfActMap : BaseImportableEntityMap<RealObjSupplierAccOperPerfAct>
///     {
///         public RealObjSupplierAccOperPerfActMap()
///             : base("REGOP_SUPP_ACC_OP_ACT")
///         {
///             References(x => x.SupplierAccOperation, "SUPP_ACC_OP_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.PerformedWorkAct, "PERF_ACT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Ñâÿçü îïåðàöèè ïî ðàññ÷åòó ñ ïîñòàâùèêàìè è àêòîì âûïîëíåííûõ ðàáîò"</summary>
    public class RealObjSupplierAccOperPerfActMap : BaseImportableEntityMap<RealObjSupplierAccOperPerfAct>
    {
        
        public RealObjSupplierAccOperPerfActMap() : 
                base("Ñâÿçü îïåðàöèè ïî ðàññ÷åòó ñ ïîñòàâùèêàìè è àêòîì âûïîëíåííûõ ðàáîò", "REGOP_SUPP_ACC_OP_ACT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SupplierAccOperation, "Îïåðàöèÿ ñ÷åòà ïî ðàññ÷åòó ñ ïîñòàâùèêàìè").Column("SUPP_ACC_OP_ID").NotNull().Fetch();
            Reference(x => x.PerformedWorkAct, "Àêò âûïîëíåííûõ ðàáîò").Column("PERF_ACT_ID").NotNull().Fetch();
        }
    }
}
