namespace Bars.Gkh.Ris.Tasks.Payment.SupplierNotificationsOfOrderExecution
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4.Utils;
    using Entities.Payment;
    using Enums;
    using Integration;
    using PaymentAsync;

    /// <summary>
    /// Задача подготовки дданных документов «Извещение о принятии к исполнению распоряжения, размещаемых исполнителем»
    /// </summary>
    public class SupplierNotificationsOfOrderExecutionPrepareDataTask : BasePrepareDataTask<importSupplierNotificationsOfOrderExecutionRequest>
    {
        private List<NotificationOfOrderExecution> notifications;

        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 1000;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var extractor = this.Container.Resolve<IDataExtractor<NotificationOfOrderExecution>>("NotificationOfOrderExecutionExtractor");

            try
            {
                extractor.Contragent = this.Contragent;
                this.notifications = extractor.Extract(parameters);
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            var itemsToRemove = new List<NotificationOfOrderExecution>();

            foreach (var item in this.notifications)
            {
                var messages = new StringBuilder();
                
                if (!item.OrderDate.HasValue)
                {
                    messages.Append("OrderDate ");
                }


                if (!item.Month.HasValue)
                {
                    messages.Append("Month ");
                }

                if (!item.Year.HasValue)
                {
                    messages.Append("Year ");
                }

                if (item.PaymentDocumentID.IsEmpty() && item.ServiceID.IsEmpty())
                {
                    messages.Append("PaymentDocumentID/ServiceID ");
                }
                
                var validateResult = new ValidateObjectResult
                {
                    Id = item.Id,
                    State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                    Message = messages.ToString(),
                    Description = "Извещение о принятии к исполнению распоряжения, размещаемых исполнителем"
                };

                if (validateResult.State != ObjectValidateState.Success)
                {
                    result.Add(validateResult);
                    itemsToRemove.Add(item);
                }
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                this.notifications.Remove(itemToRemove);
            }

            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importSupplierNotificationsOfOrderExecutionRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importSupplierNotificationsOfOrderExecutionRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект запроса</returns>
        private importSupplierNotificationsOfOrderExecutionRequest GetRequestObject(IEnumerable<NotificationOfOrderExecution> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var notificationList = new List<importSupplierNotificationsOfOrderExecutionRequestSupplierNotificationOfOrderExecution>();
            var contractTransportGuidDictionary = new Dictionary<string, long>();

            foreach (var notification in listForImport)
            {
                var listItem = this.GetNotificationOfOrderExecutionTypeItem(notification);
                notificationList.Add(listItem);

                contractTransportGuidDictionary.Add(listItem.TransportGUID, notification.Id);
            }

            transportGuidDictionary.Add(typeof(NotificationOfOrderExecution), contractTransportGuidDictionary);

            return new importSupplierNotificationsOfOrderExecutionRequest
            {
                Id = Guid.NewGuid().ToStr(),
                SenderID = this.Contragent.SenderId,
                SupplierNotificationOfOrderExecution = notificationList.ToArray(),
                
            };
        }

        /// <summary>
        /// Получить объект списка уведомлений для импорта
        /// </summary>
        /// <param name="notification">Уведомление</param>
        /// <returns>Объект списка уведомлений для импорта</returns>
        private importSupplierNotificationsOfOrderExecutionRequestSupplierNotificationOfOrderExecution GetNotificationOfOrderExecutionTypeItem(
            NotificationOfOrderExecution notification)
        {
            var itemElementName = string.IsNullOrEmpty(notification.ServiceID) ? ItemChoiceType.PaymentDocumentID : ItemChoiceType.ServiceID;
            var itemValue = string.IsNullOrEmpty(notification.ServiceID) ? notification.PaymentDocumentID : notification.ServiceID;

            return new importSupplierNotificationsOfOrderExecutionRequestSupplierNotificationOfOrderExecution
            {
                ItemElementName = itemElementName,
                Item = itemValue,
                TransportGUID = Guid.NewGuid().ToString(),
                Amount = notification.Amount ?? 0,
                OrderDate = notification.OrderDate ?? DateTime.MinValue,
                OrderPeriod = new SupplierNotificationOfOrderExecutionTypeOrderPeriod()
                {
                    Month = notification.Month ?? 0,
                    Year = notification.Year ?? 0
                }
            };
        }
        
        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<NotificationOfOrderExecution>> GetPortions()
        {
            var result = new List<IEnumerable<NotificationOfOrderExecution>>();

            if (this.notifications.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.notifications.Skip(startIndex).Take(Portion));
                    startIndex += Portion;
                }
                while (startIndex < this.notifications.Count);
            }

            return result;
        }
    }
}
