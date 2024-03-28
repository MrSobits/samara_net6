namespace Bars.Gkh.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.ClaimWork.Entities;
    
    /// <summary>Маппинг для "График реструктуризации"</summary>
    public class RestructDebtScheduleMap : BaseEntityMap<RestructDebtSchedule>
    {
        public RestructDebtScheduleMap() : base("График реструктуризации", "CLW_RESTRUCT_SCHEDULE")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.TotalDebtSum, "Общая сумма долга").Column("TOTAL_DEBT_SUM").NotNull();
            this.Property(x => x.PlanedPaymentDate, "Планируемая дата оплаты").Column("PLANED_PAYMENT_DATE").NotNull();
            this.Property(x => x.PlanedPaymentSum, "Сумма к оплате").Column("PLANED_PAYMENT_SUM").NotNull();
            this.Property(x => x.PaymentDate, "Фактическая дата оплаты").Column("PAYMENT_DATE");
            this.Property(x => x.PaymentSum, "Оплачено").Column("PAYMENT_SUM").NotNull();
            this.Property(x => x.IsExpired, "Признак просроченной оплаты").Column("IS_EXPIRED").NotNull();
            this.Reference(x => x.RestructDebt, "Реструктуризация долга").Column("RESTRUCT_DEBT_ID").NotNull();
            this.Reference(x => x.PersonalAccount, "Абонент").Column("PERS_ACC_ID").NotNull();
        }
    }
}
