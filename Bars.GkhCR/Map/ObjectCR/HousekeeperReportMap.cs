namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    /// <summary>Маппинг для "Вид работы КР"</summary>
    public class HousekeeperReportMap : BaseEntityMap<HousekeeperReport>
    {

        public HousekeeperReportMap()
            : base("Вид работы КР", "CR_OBJ_HOUSEKEEPER_REPORT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.ObjectCr, "Объект капитального ремонта").Column("OBJECT_ID").NotNull().Fetch();
            this.Reference(x => x.RealityObjectHousekeeper, "Старший по дому").Column("HOUSEKEEPER_ID").Fetch();
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);
            this.Property(x => x.IsArranged, "Устраен").Column("IS_ARRANGED");
            this.Property(x => x.Answer, "Ответ").Column("ANSWER");
            this.Property(x => x.CheckDate, "Дата проверки").Column("CKECK_DATE");
            this.Property(x => x.CheckTime, "Время проверки").Column("CHECK_TIME");
            this.Property(x => x.ReportDate, "Дата окончания работ").Column("REPORT_DATE").NotNull();
            this.Property(x => x.ReportNumber, "Номер отчета").Column("REPORT_NUMBER");          
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();         
        }
    }
}