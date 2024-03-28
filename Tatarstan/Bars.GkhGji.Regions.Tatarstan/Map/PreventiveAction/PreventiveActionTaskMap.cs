namespace Bars.GkhGji.Regions.Tatarstan.Map.PreventiveAction
{
    using Bars.Gkh.Map;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class PreventiveActionTaskMap : GkhJoinedSubClassMap<PreventiveActionTask>
    {
        /// <inheritdoc />
        public PreventiveActionTaskMap()
            : base("GJI_DOCUMENT_PREVENTIVE_ACTION_TASK")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            Property(x => x.ActionType, "Вид мероприятия").Column("ACTION_TYPE");
            Property(x => x.VisitType, "Тип визита").Column("VISIT_TYPE");
            Property(x => x.CounselingType, "Способ консультирования").Column("COUNSELING_TYPE");
            Property(x => x.ActionStartDate, "Дата начала проведения мероприятия").Column("ACTION_DATE");
            Property(x => x.ActionEndDate, "Дата окончания проведения мероприятия").Column("ACTION_END_DATE");
            Property(x => x.ActionStartTime, "Время начала мероприятия").Column("ACTION_START_TIME").Length(10);
            Property(x => x.StructuralSubdivision, "Структурное подразделение").Column("STRUCTURAL_SUBDIVISION").Length(500);
            Property(x => x.NotificationDate, "Дата уведомления").Column("NOTIFICATION_DATE");
            Property(x => x.OutgoingLetterDate, "Уведомление. Дата исходящего письма").Column("OUTGOING_LETTER_DATE");
            Property(x => x.NotificationSent, "Уведомление передано").Column("NOTIFICATION_SENT");
            Property(x => x.NotificationType, "Способ уведомления").Column("NOTIFICATION_TYPE");
            Property(x => x.NotificationDocumentNumber, "Уведомление. Номер документа").Column("NOTIFICATION_DOCUMENT_NUMBER").Length(25);
            Property(x => x.OutgoingLetterNumber, "Уведомление. Номер исходящего письма").Column("OUTGOING_LETTER_NUMBER").Length(25);
            Property(x => x.NotificationReceived, "Уведомление получено").Column("NOTIFICATION_RECIEVED");
            Property(x => x.ParticipationRejection, "Отказ от участия в профилактическом мероприятии").Column("PARTICIPATION_REJECTION");
            Reference(x => x.Executor, "Ответственный за исполнение").Column("EXECUTOR_ID").Fetch();
            Reference(x => x.TaskingInspector, "ДЛ, вынесшее задание").Column("TASKING_INSPECTOR_ID").Fetch();
            Reference(x => x.ActionLocation, "Место проведения мероприятия").Column("ACTION_LOCATION_ID").Fetch();
        }
    }
}