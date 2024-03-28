namespace Bars.Gkh.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.ClaimWork.Entities;
    
    public class RestructDebtScheduleDetailMap : PersistentObjectMap<RestructDebtScheduleDetail>
    {
        public RestructDebtScheduleDetailMap()
            : base("Детализация графика реструктуризации", "CLW_RESTRUCT_SCHEDULE_DETAIL")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.TransferId, "Идентификатор трансфера").Column("TRANSFER_ID").NotNull();
            this.Property(x => x.Sum, "Сумма оплаты трансфера").Column("SUM").NotNull();
            this.Reference(x => x.ScheduleRecord, "Запись графика реструктуризации").Column("SCHEDULE_ID").NotNull();
        }
    }
}
