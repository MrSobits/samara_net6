namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction;

    public class SurveyActionMap : JoinedSubClassMap<SurveyAction>
    {
        public SurveyActionMap()
            : base("Действие акта проверки с типом \"Опрос\"", "GJI_ACTCHECK_SURVEY_ACTION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ContinueDate, "Дата продолжения").Column("CONTINUE_DATE");
            this.Property(x => x.ContinueStartTime, "Время начала продолжения").Column("CONTINUE_START_TIME");
            this.Property(x => x.ContinueEndTime, "Время окончания продолжения").Column("CONTINUE_END_TIME");
            this.Property(x => x.ProtocolReaded, "Протокол прочитан?").Column("PROTOCOL_READED");
            this.Property(x => x.HasRemark, "Замечания?").Column("HAS_REMARK");
        }
    }
}