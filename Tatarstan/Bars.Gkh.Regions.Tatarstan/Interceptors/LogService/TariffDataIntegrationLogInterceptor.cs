using Bars.B4;
using Bars.Gkh.Regions.Tatarstan.Entities.Administration;
using Bars.Gkh.Regions.Tatarstan.Enums.Administration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.Regions.Tatarstan.Interceptors.LogService
{
    public class TariffDataIntegrationLogInterceptor : EmptyDomainInterceptor<TariffDataIntegrationLog>
    {
        public override IDataResult BeforeCreateAction(IDomainService<TariffDataIntegrationLog> service, TariffDataIntegrationLog entity)
        {
            this.CheckEntity(entity);
            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult AfterUpdateAction(IDomainService<TariffDataIntegrationLog> service, TariffDataIntegrationLog entity)
        {
            this.CheckEntity(entity);
            return base.AfterUpdateAction(service, entity);
        }

        private void CheckEntity(TariffDataIntegrationLog entity)
        {
            var result = new List<string>();
            if (entity.ExecutionStatus == default(ExecutionStatus)) result.Add("Статус выполнения");
            if (entity.Parameters == string.Empty) result.Add("Параметры");
            if (entity.User == null) result.Add("Пользователь");
            if (entity.StartMethodTime == default(DateTime)) result.Add("Время запуска метода");

            if (result.Any())
            {
                throw new ArgumentNullException($"\"Лог интеграции данных по тарифам\": Не заполнены обязательные поля: {string.Join(", ", result)}");
            }
        }
    }
}