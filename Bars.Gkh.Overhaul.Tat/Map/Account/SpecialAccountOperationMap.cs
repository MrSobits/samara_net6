/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.B4.Utils;
///     using Entities;
/// 
///     public class SpecialAccountOperationMap : BaseEntityMap<SpecialAccountOperation>
///     {
///         public SpecialAccountOperationMap()
///             : base("OVRHL_ACCOUNT_SPEC_OPERATION")
///         {
///             Map(x => x.OperationDate, "OPERATION_DATE").Not.Nullable();
///             Map(x => x.Payer, "PAYER");
///             Map(x => x.Purpose, "PURPOSE");
///             Map(x => x.Receiver, "RECEIVER");
///             Map(x => x.Sum, "SUM");
/// 
///             References(x => x.Account, "ACCOUNT_ID").Fetch.Join();
///             References(x => x.Name, "OPERATION_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.SpecialAccountOperation"</summary>
    public class SpecialAccountOperationMap : BaseEntityMap<SpecialAccountOperation>
    {
        
        public SpecialAccountOperationMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.SpecialAccountOperation", "OVRHL_ACCOUNT_SPEC_OPERATION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.OperationDate, "OperationDate").Column("OPERATION_DATE").NotNull();
            Property(x => x.Payer, "Payer").Column("PAYER");
            Property(x => x.Purpose, "Purpose").Column("PURPOSE");
            Property(x => x.Receiver, "Receiver").Column("RECEIVER");
            Property(x => x.Sum, "Sum").Column("SUM");
            Reference(x => x.Account, "Account").Column("ACCOUNT_ID").Fetch();
            Reference(x => x.Name, "Name").Column("OPERATION_ID").Fetch();
        }
    }
}
