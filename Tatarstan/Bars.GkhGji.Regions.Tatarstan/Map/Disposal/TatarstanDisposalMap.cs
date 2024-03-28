namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class TatarstanDisposalMap : JoinedSubClassMap<TatarstanDisposal>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public TatarstanDisposalMap() :
                base("Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDisposal", "GJI_TAT_DISPOSAL")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ErpRegistrationNumber, "Учетный номер проверки в ЕРП").Column("REGISTRATION_NUMBER_ERP");
            this.Property(x => x.ErpId, "Учетный номер проверки в ЕРП").Column("ERP_ID");
            this.Property(x => x.ErpRegistrationDate, "Дата присвоения учетного номера / идентификатора ЕРП").Column("REGISTRATION_DATE_ERP");
            this.Property(x => x.CountDays, "Срок проверки (количество дней)").Column("COUNT_DAYS");
            this.Property(x => x.CountHours, "Срок проверки (количество часов)").Column("COUNT_HOURS");
            this.Property(x => x.ReasonErpChecking, "Основание проверки ЕРП").Column("REASON_ERP_CHECKING");
            this.Property(x => x.NotificationType, "Способ уведомления").Column("NOTIFICATION_TYPE");
            this.Property(x => x.IsSentToErp, "Параметр, показывающий было ли распоряжение отправлено в ЕРП.").Column("IS_SENT_TO_ERP");
            this.Reference(x => x.Prosecutor, "Наименование прокуратуры").Column("PROSECUTOR_ID");
            this.Reference(x => x.InspectionBase, "Основание проверки").Column("INSPECTION_BASE_TYPE_ID");
            this.Reference(x => x.ControlType, "Вид контроля").Column("CONTROL_TYPE_ID");
            this.Property(x => x.ResultTorId, "Идентификатор ТОР Результата проверки").Column("RESULT_TOR_ID");
            this.Property(x => x.ControlCardId, "Идентификатор ТОР карточки проверки").Column("CONTROL_CARD_TOR_ID");
            this.Property(x => x.DocumentTime, "Время документа").Column("DOCUMENT_TIME");
            this.Property(x => x.InteractionPersonHour, "Срок взаимодействия с контролируемым лицом не более, часов").Column("INTERACTION_PERSON_HOUR");
            this.Property(x => x.InteractionPersonMinutes, "Срок взаимодействия с контролируемым лицом не более, минут").Column("INTERACTION_PERSON_MINUTES");
            this.Property(x => x.SuspensionInspectionBase, "Основание для приостановления проведения проверки").Column("SUSPENSION_INSPECTION_BASE");
            this.Property(x => x.SuspensionDateFrom, "Дата начала периода приостановления проведения проверки").Column("SUSPENSION_DATE_FROM");
            this.Property(x => x.SuspensionDateTo, "Дата окончания периода приостановления проведения проверки").Column("SUSPENSION_DATE_TO");
            this.Property(x => x.SuspensionTimeFrom, "Время начала периода приостановления проведения проверки").Column("SUSPENSION_TIME_FROM");
            this.Property(x => x.SuspensionTimeTo, "Время окончания периода приостановления проведения проверки").Column("SUSPENSION_TIME_TO");
            this.Property(x => x.InformationAboutHarm, "Сведения о причинении вреда (ущерба) (ст. 66 ФЗ)").Column("INFORMATION_ABOUT_HARM");
        }
    }
}
