namespace Bars.GisIntegration.Base.Tasks.PrepareData.Payment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Payment;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.PaymentAsync;
    using Bars.GisIntegration.Base.Tasks.PrepareData;

    /// <summary>
    /// Задача подготовки дданных документов «Извещение о принятии к исполнению распоряжения»
    /// </summary>
    public class NotificationsOfOrderExecutionPrepareDataTask : BasePrepareDataTask<importNotificationsOfOrderExecutionRequest>
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
                this.notifications = this.RunExtractor(extractor, parameters);
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

                if (item.Inn.IsEmpty())
                {
                    messages.Append("RecipientInfoInn ");
                }

                if ((item.RecipientEntprFirstName.IsEmpty() || item.RecipientEntprSurname.IsEmpty()) &&
                    (item.RecipientLegalName.IsEmpty() || item.RecipientLegalKpp.IsEmpty()))
                {
                    messages.Append("RecipientInfoEntpr/Legal ");
                }

                if (item.BankName.IsEmpty())
                {
                    messages.Append("BankName ");
                }

                if (item.RecipientBik.IsEmpty())
                {
                    messages.Append("RecipientBIK ");
                }

                if (item.RecipientAccount.IsEmpty())
                {
                    messages.Append("RecipientAccount ");
                }

                if (item.RecipientName.IsEmpty())
                {
                    messages.Append("RecipientName ");
                }

                if (item.OrderId.IsEmpty())
                {
                    messages.Append("OrderId ");
                }

                if (!item.OrderDate.HasValue)
                {
                    messages.Append("OrderDate ");
                }

                if (!item.Amount.HasValue)
                {
                    messages.Append("Amount ");
                }

                var validateResult = new ValidateObjectResult
                {
                    Id = item.Id,
                    State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                    Message = messages.ToString(),
                    Description = "Извещение о принятии к исполнению распоряжения"
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
        protected override Dictionary<importNotificationsOfOrderExecutionRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importNotificationsOfOrderExecutionRequest, Dictionary<Type, Dictionary<string, long>>>();

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
        private importNotificationsOfOrderExecutionRequest GetRequestObject(IEnumerable<NotificationOfOrderExecution> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var notificationList = new List<importNotificationsOfOrderExecutionRequestNotificationOfOrderExecutionType>();
            var contractTransportGuidDictionary = new Dictionary<string, long>();

            foreach (var notification in listForImport)
            {
                var listItem = this.GetNotificationOfOrderExecutionTypeItem(notification);
                notificationList.Add(listItem);

                contractTransportGuidDictionary.Add(listItem.TransportGUID, notification.Id);
            }

            transportGuidDictionary.Add(typeof(NotificationOfOrderExecution), contractTransportGuidDictionary);

            return new importNotificationsOfOrderExecutionRequest
            {
                NotificationOfOrderExecutionType = notificationList.ToArray(),
                Id = Guid.NewGuid().ToStr(),
                //Item = this.Contragent.OrgPpaGuid,
                //ItemElementName = ItemChoiceType.paymentorganizationppaguid
            };
        }

        /// <summary>
        /// Получить объект списка уведомлений для импорта
        /// </summary>
        /// <param name="notification">Уведомление</param>
        /// <returns>Объект списка уведомлений для импорта</returns>
        private importNotificationsOfOrderExecutionRequestNotificationOfOrderExecutionType GetNotificationOfOrderExecutionTypeItem(NotificationOfOrderExecution notification)
        {
            return new importNotificationsOfOrderExecutionRequestNotificationOfOrderExecutionType
            {
                TransportGUID = Guid.NewGuid().ToString(),
                SupplierInfo = new NotificationOfOrderExecutionTypeSupplierInfo
                {
                    SupplierID = notification.SupplierId,
                    SupplierName = notification.SupplierName
                },
                RecipientInfo = new NotificationOfOrderExecutionTypeRecipientInfo
                {
                    INN = notification.Inn,
                    Item = this.GetRecipientInfoItem(notification),
                    PaymentInformation = new PaymentInformationType
                    {
                        RecipientINN = notification.RecipientInn,
                        RecipientKPP = notification.RecipientKpp,
                        BankName = notification.BankName,
                        BankBIK = notification.RecipientBik,
                        CorrespondentBankAccount = notification.CorrespondentBankAccount,
                        operatingAccountNumber = notification.RecipientAccount,
                        PaymentRecipient = notification.RecipientName
                    }
                },
                OrderInfo = new NotificationOfOrderExecutionTypeOrderInfo
                {
                    OrderID = notification.OrderId,
                    OrderDate = notification.OrderDate ?? DateTime.MinValue,
                    OrderNum = notification.OrderNum,
                    Amount = notification.Amount ?? 0,
                    PaymentPurpose = notification.PaymentPurpose,
                    Comment = notification.Comment,
                    PaymentDocumentID = notification.PaymentDocumentID,
                    PaymentDocumentNumber = notification.PaymentDocumentNumber,
                    Year = notification.Year ?? 0,
                    Month = notification.Month ?? 0,
                    UnifiedAccountNumber = notification.UnifiedAccountNumber,
                    AddressAndConsumer = this.GetAddressAndConsumer(notification),
                    Service = new NotificationOfOrderExecutionTypeOrderInfoService
                    {
                        ServiceID = notification.ServiceID
                    },
                    AccountNumber = notification.AccountNumber
                }
            };
        }

        /// <summary>
        /// Получить раздел AddressAndConsumer
        /// </summary>
        /// <param name="notification">Уведомление</param>
        /// <returns>Раздел AddressAndConsumer</returns>
        private NotificationOfOrderExecutionTypeOrderInfoAddressAndConsumer GetAddressAndConsumer(NotificationOfOrderExecution notification)
        {
            object item = null;
            var items = new List<string>();
            var itemsElementName = new List<ItemsChoiceType3>();

            if (!notification.NonLivingApartment.IsEmpty())
            {
                items.Add(notification.NonLivingApartment);
                itemsElementName.Add(ItemsChoiceType3.NonLivingApartment);
            }
            else
            {
                items.Add(notification.Apartment);
                itemsElementName.Add(ItemsChoiceType3.Apartment);
                items.Add(notification.Placement);
                itemsElementName.Add(ItemsChoiceType3.Placement);
            }

            if (!notification.ConsumerInn.IsEmpty())
            {
                item = notification.ConsumerInn;
            }
            else if (!notification.ConsumerFirstName.IsEmpty() && !notification.ConsumerSurname.IsEmpty())
            {
                item = new FIOType
                {
                    FirstName = notification.ConsumerFirstName,
                    Surname = notification.ConsumerSurname,
                    Patronymic = notification.ConsumerPatronymic
                };
            }

            return new NotificationOfOrderExecutionTypeOrderInfoAddressAndConsumer
            {
                FIASHouseGuid = notification.FiasHouseGuid,
                Items = items.ToArray(),
                ItemsElementName = itemsElementName.ToArray(),
                Item = item
            };
        }

        /// <summary>
        /// Получить Item блока RecipientInfo
        /// </summary>
        /// <param name="notification">Уведомление</param>
        /// <returns>Item блока RecipientInfo</returns>
        private object GetRecipientInfoItem(NotificationOfOrderExecution notification)
        {
            object result;

            if (!notification.RecipientLegalName.IsEmpty() && !notification.RecipientLegalKpp.IsEmpty())
            {
                result = new NotificationOfOrderExecutionTypeRecipientInfoLegal
                {
                    Name = notification.RecipientLegalName,
                    KPP = notification.RecipientLegalKpp
                };
            }
            else if (!notification.RecipientEntprFirstName.IsEmpty() && !notification.RecipientEntprSurname.IsEmpty())
            {
                result = new FIOType
                {
                    FirstName = notification.RecipientEntprFirstName,
                    Surname = notification.RecipientEntprSurname,
                    Patronymic = notification.RecipientEntprPatronymic
                };
            }
            else
            {
                result = notification.RecipientEntprFio;
            }

            return result;
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
                    result.Add(this.notifications.Skip(startIndex).Take(NotificationsOfOrderExecutionPrepareDataTask.Portion));
                    startIndex += NotificationsOfOrderExecutionPrepareDataTask.Portion;
                }
                while (startIndex < this.notifications.Count);
            }

            return result;
        }
    }
}
