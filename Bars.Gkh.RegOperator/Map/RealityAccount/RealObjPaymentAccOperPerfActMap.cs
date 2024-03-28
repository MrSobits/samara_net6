/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.RealityAccount
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class RealObjPaymentAccOperPerfActMap : BaseImportableEntityMap<RealObjPaymentAccOperPerfAct>
///     {
///         public RealObjPaymentAccOperPerfActMap()
///             : base("REGOP_PAY_ACC_OP_ACT")
///         {
///             References(x => x.PayAccOperation, "PAY_ACC_OP_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.PerformedWorkActPayment, "PERF_ACT_PAY_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Ñâÿçü îïåðàöèè ïî ñ÷åòó îïëàò äîìà è îïëàòû àêòà âûïîëíåííûõ ðàáîò"</summary>
    public class RealObjPaymentAccOperPerfActMap : BaseImportableEntityMap<RealObjPaymentAccOperPerfAct>
    {
        
        public RealObjPaymentAccOperPerfActMap() : 
                base("Ñâÿçü îïåðàöèè ïî ñ÷åòó îïëàò äîìà è îïëàòû àêòà âûïîëíåííûõ ðàáîò", "REGOP_PAY_ACC_OP_ACT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PayAccOperation, "Îïåðàöèÿ ïî ñ÷åòó îïëàò äîìà").Column("PAY_ACC_OP_ID").NotNull().Fetch();
            Reference(x => x.PerformedWorkActPayment, "Îïëàòà â àêòàõ âûïîëíåííûõ ðàáîò").Column("PERF_ACT_PAY_ID").NotNull().Fetch();
        }
    }
}
