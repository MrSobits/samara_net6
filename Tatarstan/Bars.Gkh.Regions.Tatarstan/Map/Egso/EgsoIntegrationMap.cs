namespace Bars.Gkh.Regions.Tatarstan.Map.Egso
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.Egso;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    public class EgsoIntegrationMap : BaseEntityMap<EgsoIntegration>
    {
        public EgsoIntegrationMap() :
            base("Интеграция с ЕГСО ОВ", "GKH_EGSO_INTEGRATION")
        {      
        }

        protected override void Map()
        {
            this.Property(x => x.EndDate, "Дата завершения").Column("END_DATE");
            this.Property(x => x.TaskType, "Тип задачи").Column("TASK_TYPE").NotNull();
            this.Property(x => x.StateType, "Статус задачи").Column("STATUS_TYPE").NotNull().DefaultValue((int)EgsoTaskStateType.Undefined);
            this.Reference(x => x.User, "Пользователь").Column("B4_USER_ID").NotNull();
            this.Reference(x => x.Log, "Лог").Column("B4_LOG_INFO_ID");
            this.Property(x => x.Year, "Год отправки").Column("REPORT_YEAR").NotNull();
        }
    }
}