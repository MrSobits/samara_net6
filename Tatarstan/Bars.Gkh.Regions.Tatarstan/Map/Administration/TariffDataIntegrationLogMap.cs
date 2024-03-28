using Bars.B4.Modules.Mapping.Mappers;
using Bars.Gkh.Regions.Tatarstan.Entities.Administration;

namespace Bars.Gkh.Regions.Tatarstan.Map.Administration
{
    public class TariffDataIntegrationLogMap : BaseEntityMap<TariffDataIntegrationLog>
    {
        public TariffDataIntegrationLogMap()
            : base("Лог интеграции данных по тарифам", "GKH_TAT_TARIFF_DATA_INTEGRATION_LOG")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.TariffDataIntegrationMethod, "Метод интеграции данных по тарифам").Column("TARIFF_DATA_INTEGRATION_METHOD").NotNull();
            this.Property(x => x.StartMethodTime, "Время запуска метода").Column("START_METHOD_TIME");
            this.Property(x => x.Parameters, "Параметры").Column("PARAMETERS");
            this.Property(x => x.ExecutionStatus, "Статус выполнения").Column("EXECUTION_STATUS").NotNull();
            this.Reference(x => x.User, "Пользователь").Column("B4_USER_ID").Fetch();
            this.Reference(x => x.LogFile, "Файл лога").Column("LOG_FILE_ID").Fetch();
        }
    }
}