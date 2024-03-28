namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVValidPassportMap : BaseEntityMap<SMEVValidPassport>
    {
        
        public SMEVValidPassportMap() : 
                base("Запрос к ВС Паспорт", "GJI_CH_SMEV_VALID_PASSPORT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.DocSerie, "Серия паспорта").Column("DOC_SERIE");
            Property(x => x.DocNumber, "Номер паспорта").Column("DOC_NUMBER");
            Property(x => x.MessageId, "Id запроса в системе СМЭВ3").Column("MESSAGEID");
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.DocIssueDate, "DocIssueDate").Column("DOC_ISSUE_DATE").NotNull();
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.DocStatus, "Статус паспорта").Column("DOC_STATUS");
            Property(x => x.InvaliditySince, "Недействительно с").Column("INVALIDITY_SINCE");
            Property(x => x.InvalidityReason, "Причина недействительности").Column("INVALIDITY_REASON");
            Property(x => x.Answer, "Ответ").Column("ANSWER");
            Property(x => x.TaskId, "TaskId").Column("TASK_ID");
        }
    }
}
