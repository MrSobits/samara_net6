/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.Hcs
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Entities.Hcs;
/// 
///     public class HouseAccountChargeMap : BaseImportableEntityMap<HouseAccountCharge>
///     {
///         public HouseAccountChargeMap()
///             : base("HCS_HOUSE_ACCOUNT_CHARGE")
///         {
///             Map(x => x.Service, "SERVICE");
///             Map(x => x.Tariff, "TARIFF");
///             Map(x => x.Expense, "EXPENSE");
///             Map(x => x.CompleteCalc, "COMPLETE_CALC");
///             Map(x => x.Underdelivery, "UNDERDELIVERY");
///             Map(x => x.Charged, "CHARGED");
///             Map(x => x.Recalc, "RECALC");
///             Map(x => x.Changed, "CHANGED");
///             Map(x => x.Payment, "PAYMENT");
///             Map(x => x.ChargedPayment, "CHARGED_PAYMENT");
///             Map(x => x.OuterBalance, "OUTER_BALANCE");
///             Map(x => x.InnerBalance, "INNER_BALANCE");
///             Map(x => x.DateCharging, "DATE_CHARGING");
///             Map(x => x.Supplier, "SUPPLIER");
///             Map(x => x.CompositeKey, "COMPOSITE_KEY", false, 100);
///             Map(x => x.PaymentSizeCr, "PAYMENT_SIZE_CR");
///             Map(x => x.PaymentChargeAll, "PAYMENT_CHARGE_ALL");
///             Map(x => x.PaymentPaidAll, "PAYMENT_PAID_ALL");
///             Map(x => x.PaymentDebtAll, "PAYMENT_DEBT_ALL");
///             Map(x => x.PaymentChargeMonth, "PAYMENT_CHARGE_MONTH");
///             Map(x => x.PaymentPaidMonth, "PAYMENT_PAID_MONTH");
///             Map(x => x.PaymentDebtMonth, "PAYMENT_DEBT_MONTH");
///             Map(x => x.PenaltiesChargeAll, "PENALTIES_CHARGE_ALL");
///             Map(x => x.PenaltiesPaidAll, "PENALTIES_PAID_ALL");
///             Map(x => x.PenaltiesDebtAll, "PENALTIES_DEBT_ALL");
///             Map(x => x.PenaltiesChargeMonth, "PENALTIES_CHARGE_MONTH");
///             Map(x => x.PenaltiesPaidMonth, "PENALTIES_PAID_MONTH");
///             Map(x => x.PenaltiesDebtMonth, "PENALTIES_DEBT_MONTH");
/// 
///             References(x => x.Account, "ACCOUNT_ID", ReferenceMapConfig.Fetch);
///             References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Hcs
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Hcs;
    
    
    /// <summary>Маппинг для "Начисления лицевого счета"</summary>
    public class HouseAccountChargeMap : BaseImportableEntityMap<HouseAccountCharge>
    {
        
        public HouseAccountChargeMap() : 
                base("Начисления лицевого счета", "HCS_HOUSE_ACCOUNT_CHARGE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Reference(x => x.Account, "Лицевой счет дома").Column("ACCOUNT_ID").Fetch();
            Property(x => x.Service, "Услуга").Column("SERVICE").Length(250);
            Property(x => x.Tariff, "Тариф").Column("TARIFF");
            Property(x => x.Expense, "Расход").Column("EXPENSE");
            Property(x => x.DateCharging, "Дата начисления").Column("DATE_CHARGING");
            Property(x => x.CompleteCalc, "Полный расчет").Column("COMPLETE_CALC");
            Property(x => x.Underdelivery, "Недопоставка").Column("UNDERDELIVERY");
            Property(x => x.Charged, "Начислено").Column("CHARGED");
            Property(x => x.Recalc, "Перерасчет").Column("RECALC");
            Property(x => x.Changed, "Изменен").Column("CHANGED");
            Property(x => x.Payment, "Оплата").Column("PAYMENT");
            Property(x => x.ChargedPayment, "Начислено к оплате").Column("CHARGED_PAYMENT");
            Property(x => x.InnerBalance, "Вх. Сальдо").Column("INNER_BALANCE");
            Property(x => x.OuterBalance, "Исх. Сальдо").Column("OUTER_BALANCE");
            Property(x => x.CompositeKey, "Составной ключ вида Account.PaymentCode#DateCharging#Service, формируется в интер" +
                    "цепторе на Create и Update").Column("COMPOSITE_KEY").Length(100);
            Property(x => x.Supplier, "Поставщик").Column("SUPPLIER").Length(250);
            Property(x => x.PaymentSizeCr, "Размер взноса на КР").Column("PAYMENT_SIZE_CR");
            Property(x => x.PaymentChargeAll, "Начислено взносов Всего").Column("PAYMENT_CHARGE_ALL");
            Property(x => x.PaymentPaidAll, "Оплачено взносов Всего").Column("PAYMENT_PAID_ALL");
            Property(x => x.PaymentDebtAll, "Задолженность по взносам Всего").Column("PAYMENT_DEBT_ALL");
            Property(x => x.PaymentChargeMonth, "Начислено взносов за месяц").Column("PAYMENT_CHARGE_MONTH");
            Property(x => x.PaymentPaidMonth, "Оплачено взносов за месяц").Column("PAYMENT_PAID_MONTH");
            Property(x => x.PaymentDebtMonth, "Задолженность по взносам за месяц").Column("PAYMENT_DEBT_MONTH");
            Property(x => x.PenaltiesChargeAll, "Начислено пени Всего").Column("PENALTIES_CHARGE_ALL");
            Property(x => x.PenaltiesPaidAll, "Оплачено пени Всего").Column("PENALTIES_PAID_ALL");
            Property(x => x.PenaltiesDebtAll, "Задолженность по пени Всего").Column("PENALTIES_DEBT_ALL");
            Property(x => x.PenaltiesChargeMonth, "Начислено пени за месяц").Column("PENALTIES_CHARGE_MONTH");
            Property(x => x.PenaltiesPaidMonth, "Оплачено пени за месяц").Column("PENALTIES_PAID_MONTH");
            Property(x => x.PenaltiesDebtMonth, "Задолженность по пени за месяц").Column("PENALTIES_DEBT_MONTH");
        }
    }
}
