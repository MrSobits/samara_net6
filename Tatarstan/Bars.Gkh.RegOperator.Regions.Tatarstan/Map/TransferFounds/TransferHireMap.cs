/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Map
/// {
///     using B4.DataAccess;
/// 
///     using Entities;
/// 
///     public class TransferHireMap : BaseEntityMap<TransferHire>
///     {
///         public TransferHireMap(): base("REGOP_TRANSFER_HIRE")
///         {
///             Map(x => x.Transferred, "TRANSFERRED");
///             Map(x => x.TransferredSum, "TRANSFERRED_SUM");
/// 
///             References(x => x.TransferRecord, "REC_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Account, "ACCOUNT_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Regions.Tatarstan.Entities.TransferHire"</summary>
    public class TransferHireMap : BaseEntityMap<TransferHire>
    {
        
        public TransferHireMap() : 
                base("Bars.Gkh.RegOperator.Regions.Tatarstan.Entities.TransferHire", "REGOP_TRANSFER_HIRE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Transferred, "Transferred").Column("TRANSFERRED");
            Property(x => x.TransferredSum, "TransferredSum").Column("TRANSFERRED_SUM");
            Reference(x => x.TransferRecord, "TransferRecord").Column("REC_ID").NotNull().Fetch();
            Reference(x => x.Account, "Account").Column("ACCOUNT_ID").NotNull().Fetch();
        }
    }
}
