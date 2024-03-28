namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class PrescriptionCancelTatarstanMap : JoinedSubClassMap<PrescriptionCancelTatarstan>
    {
        public PrescriptionCancelTatarstanMap()
            :
            base("Bars.GkhGji.Regions.Tatarstan.Entities.PrescriptionCancelTatarstan", "GJI_PRESCRIPTION_CANCEL_TATARSTAN")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Date, "Дата").Column("DATE");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").Length(255);
            this.Property(x => x.OutMailDate, "Дата исходящего письма").Column("OUT_MAIL_DATE");
            this.Property(x => x.OutMailNumber, "Номер исходящего письма").Column("OUT_MAIL_NUMBER").Length(255);
            this.Property(x => x.NotificationTransmission, "Уведомление передано").Column("NOTIFICATION_TRANSMISSION");
            this.Property(x => x.NotificationReceive, "Уведомление получено").Column("NOTIFICATION_RECEIVE");
            this.Property(x => x.NotificationType, "Способ уведомления").Column("NOTIFICATION_TYPE");
            this.Property(x => x.ProlongationDate, "Срок продления").Column("PROLONGATION_DATE");
        }
    }
}