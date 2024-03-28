namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    /// <summary>Маппинг для "История изменения вида работы объекта КР"</summary>
    public class SpecialTypeWorkCrHistoryMap : BaseImportableEntityMap<SpecialTypeWorkCrHistory>
    {
        public SpecialTypeWorkCrHistoryMap() : 
                base("История изменения вида работы объекта КР", "CR_SPECIAL_OBJ_TYPE_WORK_HIST")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.Volume, "Объем выполнения").Column("VOLUME");
            this.Property(x => x.Sum, "Сумма расходов").Column("SUM");
            this.Property(x => x.YearRepair, "Год ремонта").Column("YEAR_REPAIR");
            this.Property(x => x.NewYearRepair, "Новый год ремонта").Column("NEW_YEAR_REPAIR");
            this.Property(x => x.UserName, "Имя пользователя").Column("USER_NAME").Length(300);
            this.Property(x => x.TypeAction, "Тип действия для истории вида работ объекта КР").Column("TYPE_ACTION").NotNull();
            this.Property(x => x.TypeReason, "Причина").Column("TYPE_REASON").NotNull();
            this.Property(x => x.StructElement, "Конструктивный элемент").Column("STRUCT_EL").Length(500);

            this.Reference(x => x.TypeWorkCr, "Вид работы объекта КР").Column("TYPE_WORK_ID").NotNull().Fetch();
            this.Reference(x => x.FinanceSource, "Разрез финансирования").Column("FIN_SOURCE_ID").Fetch();
        }
    }
}
