/// <mapping-converter-backup>
/// using Bars.B4.DataAccess.ByCode;
/// using Bars.Gkh.Entities.Hcs;
/// namespace Bars.Gkh.Map.Hcs
/// {
///     public class HouseOverallBalanceMap : BaseImportableEntityMap<HouseOverallBalance>
///     {
///         public HouseOverallBalanceMap()
///             : base("HCS_HOUSE_OVERALL_BALANCE")
///         {
///             Map(x => x.Service, "SERVICE");
///             Map(x => x.InnerBalance, "INNER_BALANCE");
///             Map(x => x.MonthCharge, "MOUNTH_CHARGE");
///             Map(x => x.Payment, "PAYMENT");
///             Map(x => x.Paid, "PAID");
///             Map(x => x.OuterBalance, "OUTER_BALANCE");
///             Map(x => x.CorrectionCoef, "CORRECTION_COEF");
///             Map(x => x.HouseExpense, "HOUSE_EXPENSE");
///             Map(x => x.AccountsExpense, "ACCOUNTS_EXPENSE");
///             Map(x => x.DateCharging, "DATE_CHARGING");
///             Map(x => x.CompositeKey, "COMPOSITE_KEY", false, 100);
/// 
///             References(x => x.RealityObject, "REALITY_OBJECT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.Hcs
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Hcs;
    
    
    /// <summary>Маппинг для "Общее сальдо по дому"</summary>
    public class HouseOverallBalanceMap : BaseImportableEntityMap<HouseOverallBalance>
    {
        
        public HouseOverallBalanceMap() : 
                base("Общее сальдо по дому", "HCS_HOUSE_OVERALL_BALANCE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull().Fetch();
            Property(x => x.DateCharging, "Дата начисления").Column("DATE_CHARGING");
            Property(x => x.InnerBalance, "Вх. Сальдо").Column("INNER_BALANCE");
            Property(x => x.MonthCharge, "Начислено за месяц").Column("MOUNTH_CHARGE");
            Property(x => x.Payment, "К оплате").Column("PAYMENT");
            Property(x => x.Paid, "Оплачено").Column("PAID");
            Property(x => x.OuterBalance, "Исх. Сальдо").Column("OUTER_BALANCE");
            Property(x => x.Service, "Услуга").Column("SERVICE").Length(250);
            Property(x => x.CorrectionCoef, "Коэффициент коррекции").Column("CORRECTION_COEF");
            Property(x => x.HouseExpense, "Расход по дому").Column("HOUSE_EXPENSE");
            Property(x => x.AccountsExpense, "Расход по лицевым счетам").Column("ACCOUNTS_EXPENSE");
            Property(x => x.CompositeKey, "Составной ключ вида RealityObject.CodeErc#DateCharging#Service, формируется в инт" +
                    "ерцепторе на Create и Update Не отображать в клиентской части").Column("COMPOSITE_KEY").Length(100);
        }
    }
}
