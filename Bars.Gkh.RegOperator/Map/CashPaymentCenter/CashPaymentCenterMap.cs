/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class CashPaymentCenterMap : BaseImportableEntityMap<CashPaymentCenter>
///     {
///         public CashPaymentCenterMap()
///             : base("REGOP_CASHPAYMENT_CENTER")
///         {
///             Map(x => x.Identifier, "IDENTIFIER");
///             Map(x => x.ConductsAccrual, "CONDUCTSACCRUAL");
///             Map(x => x.ShowPersonalData, "SHOW_PERSONAL_DATA");
/// 
///             References(x => x.Contragent, "CONTRAGENT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Расчетно-кассовый центр"</summary>
    public class CashPaymentCenterMap : BaseImportableEntityMap<CashPaymentCenter>
    {
        
        public CashPaymentCenterMap() : 
                base("Расчетно-кассовый центр", "REGOP_CASHPAYMENT_CENTER")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull().Fetch();
            Property(x => x.Identifier, "Идентификатор РКЦ").Column("IDENTIFIER").Length(250);
            Property(x => x.ConductsAccrual, "РКЦ проводит начисления").Column("CONDUCTSACCRUAL");
            Property(x => x.ShowPersonalData, "Настройка скрытия / отображения ПДн при выгрузке данных сервисом GetChargePayment" +
                    "Rkc").Column("SHOW_PERSONAL_DATA");
        }
    }
}
