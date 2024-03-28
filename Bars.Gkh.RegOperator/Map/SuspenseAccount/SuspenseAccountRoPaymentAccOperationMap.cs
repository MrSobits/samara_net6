/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
///     
///     public class SuspenseAccountRoPaymentAccOperationMap : BaseImportableEntityMap<SuspenseAccountRoPaymentAccOperation>
///     {
///         public SuspenseAccountRoPaymentAccOperationMap()
///             : base("REG_OP_SSP_ACC_RO_PA")
///         {
///             References(x => x.Operation, "OP_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.SuspenseAccount, "SA_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.SuspenseAccountRoPaymentAccOperation"</summary>
    public class SuspenseAccountRoPaymentAccOperationMap : BaseImportableEntityMap<SuspenseAccountRoPaymentAccOperation>
    {
        
        public SuspenseAccountRoPaymentAccOperationMap() : 
                base("Bars.Gkh.RegOperator.Entities.SuspenseAccountRoPaymentAccOperation", "REG_OP_SSP_ACC_RO_PA")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SuspenseAccount, "SuspenseAccount").Column("SA_ID").NotNull().Fetch();
            Reference(x => x.Operation, "Operation").Column("OP_ID").NotNull().Fetch();
        }
    }
}
