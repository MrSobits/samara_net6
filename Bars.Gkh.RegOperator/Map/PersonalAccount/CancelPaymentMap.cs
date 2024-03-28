/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.PersonalAccount;
/// 
///     public class CancelPaymentMap : BaseImportableEntityMap<CancelPayment>
///     {
///         public CancelPaymentMap() : base("REGOP_CANCEL_PAYMENT")
///         {
///             Map(x => x.Sum, "DOWN_SUM", true, 0m);
///             Map(x => x.OperationDate, "OPERATION_DATE", true);
///             Map(x => x.Reason, "REASON", true, 500);
/// 
///             References(x => x.Account, "ACCOUNT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Document, "DOCUMENT_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using System;

    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Отмена оплаты"</summary>
    public class CancelPaymentMap : BaseImportableEntityMap<CancelPayment>
    {
        
        public CancelPaymentMap() : 
                base("Отмена оплаты", "REGOP_CANCEL_PAYMENT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Account, "Лицевой счет").Column("ACCOUNT_ID").NotNull().Fetch();
            Property(x => x.OperationDate, "Дата операции").Column("OPERATION_DATE").NotNull();
            Property(x => x.Sum, "Сумма списания").Column("DOWN_SUM").DefaultValue(0m).NotNull();
            Reference(x => x.Document, "Документ основание").Column("DOCUMENT_ID").Fetch();
            Property(x => x.Reason, "Причина").Column("REASON").Length(500).NotNull();
        }
    }
}
