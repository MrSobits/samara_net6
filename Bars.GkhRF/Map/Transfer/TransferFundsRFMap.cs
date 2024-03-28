/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhRf.Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Перечисление ден средств средств"
///     /// </summary>
///     public class TransferFundsRfMap : BaseGkhEntityMap<TransferFundsRf>
///     {
///         public TransferFundsRfMap()
///             : base("RF_TRANSFER_FUNDS")
///         {
///             Map(x => x.WorkKind, "WORK_KIND").Length(300);
///             Map(x => x.PayAllocate, "PAY_ALLOCATE").Length(300);
///             Map(x => x.PersonalAccount, "PERSONAL_ACCOUNT").Length(300);
///             Map(x => x.Sum, "SUM");
/// 
///             References(x => x.RequestTransferRf, "REQUEST_TRANSFER_RF_ID").Not.Nullable().Fetch.Join();
///             References(x => x.RealityObject, "REALITY_OBJ_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Перечисление ден средств средств рег. фонда"</summary>
    public class TransferFundsRfMap : BaseEntityMap<TransferFundsRf>
    {
        
        public TransferFundsRfMap() : 
                base("Перечисление ден средств средств рег. фонда", "RF_TRANSFER_FUNDS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.WorkKind, "Разновидность работы").Column("WORK_KIND").Length(300);
            Property(x => x.PayAllocate, "Назначение платежа").Column("PAY_ALLOCATE").Length(300);
            Property(x => x.PersonalAccount, "Лицевой счет").Column("PERSONAL_ACCOUNT").Length(300);
            Property(x => x.Sum, "Сумма").Column("SUM");
            Reference(x => x.RequestTransferRf, "Заявка на перечисление ден. средств").Column("REQUEST_TRANSFER_RF_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Объект недвижимости").Column("REALITY_OBJ_ID").Fetch();
        }
    }
}
