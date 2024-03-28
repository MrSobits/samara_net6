namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    /// <summary>Маппинг для "Вид работы КР"</summary>
    public class HousekeeperReportFileMap : BaseEntityMap<HousekeeperReportFile>
    {

        public HousekeeperReportFileMap()
            : base("Вид работы КР", "CR_OBJ_HOUSEKEEPER_REPORT_FILE")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.HousekeeperReport, "Отчет").Column("REPORT_ID").NotNull().Fetch();
            this.Reference(x => x.FileInfo, "Старший по дому").Column("FILE_INFO_ID").Fetch();
            this.Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(500);            
        }
    }
}