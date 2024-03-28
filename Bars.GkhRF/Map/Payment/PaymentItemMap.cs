/// <mapping-converter-backup>
/// namespace Bars.GkhRf.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhRf.Entities;
///     using Bars.GkhRf.Enums;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Оплата КР"
///     /// </summary>
///     public class PaymentItemMap : BaseGkhEntityMap<PaymentItem>
///     {
///         public PaymentItemMap() : base("RF_PAYMENT_ITEM")
///         {
///             Map(x => x.TypePayment, "TYPE_PAYMENT").Not.Nullable().CustomType<TypePayment>();
///             Map(x => x.ChargeDate, "CHARGE_DATE");
///             Map(x => x.IncomeBalance, "BALANCE_IN");
///             Map(x => x.OutgoingBalance, "BALANCE_OUT");
///             Map(x => x.ChargePopulation, "CHARGE_POPULATION");
///             Map(x => x.PaidPopulation, "PAID_POPULATION");
///             Map(x => x.Recalculation, "RECALCULATION");
///             Map(x => x.TotalArea, "TOTAL_AREA");
/// 
///             References(x => x.Payment, "PAYMENT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.ManagingOrganization, "MANAG_ORG_ID").Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhRf.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhRf.Entities;
    
    
    /// <summary>Маппинг для "Оплата КР"</summary>
    public class PaymentItemMap : BaseEntityMap<PaymentItem>
    {
        
        public PaymentItemMap() : 
                base("Оплата КР", "RF_PAYMENT_ITEM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Property(x => x.TypePayment, "Тип оплаты").Column("TYPE_PAYMENT").NotNull();
            Property(x => x.ChargeDate, "Дата начисления").Column("CHARGE_DATE");
            Property(x => x.IncomeBalance, "Входящее сальдо").Column("BALANCE_IN");
            Property(x => x.OutgoingBalance, "Исходящее сальдо").Column("BALANCE_OUT");
            Property(x => x.ChargePopulation, "Начислено населению").Column("CHARGE_POPULATION");
            Property(x => x.PaidPopulation, "Оплачено населением").Column("PAID_POPULATION");
            Property(x => x.Recalculation, "Перерасчет прошлого периода").Column("RECALCULATION");
            Property(x => x.TotalArea, "Общая площадь").Column("TOTAL_AREA");
            Reference(x => x.Payment, "Оплата").Column("PAYMENT_ID").NotNull().Fetch();
            Reference(x => x.ManagingOrganization, "Управляющая организация").Column("MANAG_ORG_ID").Fetch();
        }
    }
}
