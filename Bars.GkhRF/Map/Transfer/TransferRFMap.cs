/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhRf.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Перечисление средств рег. фонда"
///     /// </summary>
///     public class TransferRfMap : BaseGkhEntityMap<TransferRf>
///     {
///         public TransferRfMap() : base("RF_TRANSFER")
///         {
///             References(x => x.ContractRf, "CONTRACT_RF_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Перечисление средств рег. фонда В ГИСУ"</summary>
    public class TransferRfMap : BaseEntityMap<TransferRf>
    {
        
        public TransferRfMap() : 
                base("Перечисление средств рег. фонда В ГИСУ", "RF_TRANSFER")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.ContractRf, "Договор рег. фонда").Column("CONTRACT_RF_ID").NotNull().Fetch();
        }
    }
}
