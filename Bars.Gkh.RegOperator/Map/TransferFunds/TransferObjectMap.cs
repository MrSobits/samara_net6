/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess;
///     using Entities;
/// 
///     public class TransferObjectMap : BaseImportableEntityMap<TransferObject>
///     {
///         public TransferObjectMap() : base("REGOP_TRANSFER_OBJ")
///         {
///             Map(x => x.Transferred, "TRANSFERRED");
///             Map(x => x.TransferredSum, "TRANSFERRED_SUM");
/// 
///             References(x => x.TransferRecord, "REC_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "RO_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.TransferObject"</summary>
    public class TransferObjectMap : BaseImportableEntityMap<TransferObject>
    {
        
        public TransferObjectMap() : 
                base("Bars.Gkh.RegOperator.Entities.TransferObject", "REGOP_TRANSFER_OBJ")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Transferred, "Transferred").Column("TRANSFERRED");
            Property(x => x.TransferredSum, "TransferredSum").Column("TRANSFERRED_SUM");
            Reference(x => x.TransferRecord, "TransferRecord").Column("REC_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "RealityObject").Column("RO_ID").NotNull().Fetch();
        }
    }
}
