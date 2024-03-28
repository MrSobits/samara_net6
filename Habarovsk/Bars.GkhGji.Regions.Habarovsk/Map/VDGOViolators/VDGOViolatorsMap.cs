namespace Bars.GkhGji.Regions.Habarovsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Entities;


    /// <summary>Маппинг для списка документов по экспорту в РИС</summary>
    public class VDGOViolatorsMap : BaseEntityMap<VDGOViolators>
    {

        public VDGOViolatorsMap() : base("Реестр нарушителей ВДГО", "GJI_VDGO_VIOLATORS")
        {
        }

        protected override void Map()
        {
            Reference(x => x.Contragent, "Контрагент").Column("GJI_CONTRAGENT_ID");
            Reference(x => x.MinOrgContragent, "Контрагент УК").Column("GJI_MIN_ORG_CONTRAGENT_ID");
            Reference(x => x.Address, "Адрес").Column("GJI_RO_ID");
            Reference(x => x.File, "Доп файлы").Column("GJI_FILE_ID");
            Reference(x => x.NotificationFile, "Файл уведомления").Column("GJI_NOTIFICATION_FILE_ID");
            Property(x => x.NotificationNumber, "Номер уведомления").Column("NOTIFICATION_NUMBER");
            Property(x => x.NotificationDate, "Дата уведомления").Column("NOTIFICATION_DATE");
            Property(x => x.DateExecution, "Дата испомления").Column("DATE_EXECUTION");
            Property(x => x.FIO, "ФИО").Column("FIO");
            Property(x => x.PhoneNumber, "Номер телефона").Column("PHONE_NUMBER");
            Property(x => x.Email, "Электронная почта").Column("EMAIL");
            Property(x => x.Description, "Примечание").Column("DESCRIPTION");
            Property(x => x.MarkOfExecution, "Отметка об исполнении").Column("MARK_OF_EXECUTION");
            Property(x => x.MarkOfMessage, "Отметка отправки сообщения").Column("MARK_OF_MESSAGE");
        }
    }
}
