namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Administration.PrintCertHistory;

    /// <summary>Маппинг для "Контроль выдачи справок по лс"</summary>
    public class PrintCertHistoryMap : BaseEntityMap<PrintCertHistory>
    {
        public PrintCertHistoryMap() : base("Контроль выдачи справок по лс", "GKH_PRINT_CERT_HISTORY")
        {
        }

        protected override void Map()
        {
            Property(x => x.AccNum, "Номер лицевого счета").Column("ACC_NUM").Length(20);
            Property(x => x.Address, "Адрес").Column("ADDRESS").Length(255);
            Property(x => x.Type, "Тип").Column("TYPE").Length(50);
            Property(x => x.Name, "Наименование/ФИО").Column("NAME").Length(255);
            Property(x => x.PrintDate, "Дата печати отчета").Column("PRINT_DATE");
            Property(x => x.Username, "Имя пользователя").Column("USERNAME").Length(255);
            Property(x => x.Role, "Роль").Column("ROLE").Length(255);
        }
    }
}
