namespace Bars.GisIntegration.Base.Tasks.PrepareData.Payment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Payment;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.PaymentAsync;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.GisIntegration.Base.Utils;

    /// <summary>
    /// Задача подготовки данных "Экспорт документов «Аннулирование извещения о принятии к исполнению распоряжения»"
    /// </summary>
    public class NotificationsOfOrderExecutionCancellationPrepareDataTask : BasePrepareDataTask<importNotificationsOfOrderExecutionCancellationRequest>
    {
        /// <summary>
        /// Домен сервис "Уведомление о выполнении распоряжения"
        /// </summary>
        public IDomainService<NotificationOfOrderExecution> NotificationOfOrderExecutionDomain { get; set; }

        private IList<NotificationOfOrderExecution> data;
        private const int packageSize = 1000;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var dataSelector = this.Container.Resolve<IDataSelector<NotificationOfOrderExecution>>("NotificationOfOrderExecutionCancellationDataSelector");
            parameters.Add("Contragent", this.Contragent);

            using (this.Container.Using(dataSelector))
            {
                this.data = dataSelector.GetExternalEntities(parameters);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData() => new List<ValidateObjectResult>();

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importNotificationsOfOrderExecutionCancellationRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importNotificationsOfOrderExecutionCancellationRequest, Dictionary<Type, Dictionary<string, long>>>();
            
            foreach (var dataByOrgGuid in this.data.GroupBy(x => x.ServiceID))
            {
                foreach (var section in dataByOrgGuid.Section(NotificationsOfOrderExecutionCancellationPrepareDataTask.packageSize))
                {
                    var transportGuids = new Dictionary<string, long>();
                    var request = this.PrepareSectionRequest(section, transportGuids);

                   // request.paymentorganizationguid = dataByOrgGuid.Key;
                    result.Add(request, new Dictionary<Type, Dictionary<string, long>>
                    {
                        {typeof(NotificationOfOrderExecution), transportGuids }
                    });
                }
            }

            return result;
        }

        private importNotificationsOfOrderExecutionCancellationRequest PrepareSectionRequest(
            IEnumerable<NotificationOfOrderExecution> data,
            Dictionary<string, long> transportGuids)
        {
            var listNotifications = new List<NotificationOfOrderExecutionCancellationType>();
            foreach (var notificationOfOrderExecution in data)
            {
                var guid = Guid.NewGuid().ToString();

                listNotifications.Add(new NotificationOfOrderExecutionCancellationType
                {
                    TransportGUID = guid,
                    CancellationDate = notificationOfOrderExecution.ObjectEditDate,
                    OrderID = notificationOfOrderExecution.OrderId
                });

                transportGuids.Add(guid, notificationOfOrderExecution.Id);
            }

            return new importNotificationsOfOrderExecutionCancellationRequest
            {
                NotificationOfOrderExecutionCancellation = listNotifications.ToArray()
            };
        }
    }
}