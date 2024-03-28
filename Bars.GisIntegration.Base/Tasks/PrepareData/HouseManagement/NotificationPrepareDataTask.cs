namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.HouseManagementAsync;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.Gkh.Quartz.Scheduler.Log;
    using Castle.Core.Internal;

    /// <summary>
    /// Задача подготовки данных по новостям
    /// </summary>
    public class NotificationPrepareDataTask : BasePrepareDataTask<importNotificationRequest>
    {
        private List<RisNotification> notificationList;
        private List<RisNotificationAttachment> notificationAttachmentLinkList;
        private List<RisNotificationAddressee> notificationAddresseeList;

        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 100;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var notificationDataExtractor = this.Container.Resolve<IDataExtractor<RisNotification>>("NotificationDataExtractor");
            var notificationAddresseeExtractor = this.Container.Resolve<IDataExtractor<RisNotificationAddressee>>("NotificationAddresseeExtractor");
            var notificationAttachmentLinkExtractor = this.Container.Resolve<IDataExtractor<RisNotificationAttachment>>("NotificationAttachmentExtractor");

            try
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато извлечение данных по новостям"));

                this.notificationList = this.RunExtractor(notificationDataExtractor, parameters);
                this.notificationAttachmentLinkList = this.RunExtractor(notificationAttachmentLinkExtractor, parameters);
                this.notificationAddresseeList = this.RunExtractor(notificationAddresseeExtractor, parameters);

                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершено извлечение данных по новостям"));
            }
            finally
            {
                this.Container.Release(notificationDataExtractor);
                this.Container.Release(notificationAddresseeExtractor);
                this.Container.Release(notificationAttachmentLinkExtractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var validationResult = new List<ValidateObjectResult>();

            validationResult.AddRange(this.notificationList
                .Select(this.CheckNotification)
                .Where(checkResult => checkResult.State != ObjectValidateState.Success)
                .ToList());

            return validationResult;
        }

        private ValidateObjectResult CheckNotification(RisNotification notification)
        {
            var messages = new StringBuilder();

            if ((notification.IsShipOff == null || !notification.IsShipOff.Value) && (notification.Deleted == null || !notification.Deleted.Value))
            {
                if (notification.Topic.IsNullOrEmpty() || notification.Topic.Length > 2000)
                {
                    messages.Append("Topic ");
                }
                if (notification.Content.Length > 2000)
                {
                    messages.Append("Content ");
                }
                if ((notification.IsNotLimit == null || !notification.IsNotLimit.Value) && notification.EndDate == null)
                {
                    messages.Append("EndDate ");
                }
            }

            return new ValidateObjectResult
            {
                Id = notification.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Новость"
            };
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importNotificationRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importNotificationRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }


        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса.
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<RisNotification>> GetPortions()
        {
            var result = new List<IEnumerable<RisNotification>>();

            if (this.notificationList.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.notificationList.Skip(startIndex).Take(NotificationPrepareDataTask.Portion));
                    startIndex += NotificationPrepareDataTask.Portion;
                }
                while (startIndex < this.notificationList.Count);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса.
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Список объектов для импорта</param>
        /// <returns>Объект запроса</returns>
        private importNotificationRequest GetRequestObject(IEnumerable<RisNotification> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var notificationsList = new List<importNotificationRequestNotification>();
            var notificationTransportGuidDictionary = new Dictionary<string, long>();

            foreach (var notification in listForImport)
            {
                var listItem = this.GetImportNotificationRequestContract(notification);
                notificationsList.Add(listItem);

                notificationTransportGuidDictionary.Add(listItem.TransportGUID, notification.Id);
            }

            transportGuidDictionary.Add(typeof(RisAccount), notificationTransportGuidDictionary);

            return new importNotificationRequest { notification = notificationsList.ToArray() };
        }

        /// <summary>
        /// Создать объект importNotificationRequestNotification по RisNotification
        /// </summary>
        /// <param name="notification">Объект типа RisNotification</param>
        /// <returns>Объект типа importNotificationRequestNotification</returns>
        private importNotificationRequestNotification GetImportNotificationRequestContract(RisNotification notification)
        {
            var importContractRequest = new importNotificationRequestNotification
            {
                TransportGUID = Guid.NewGuid().ToString(),
                Item = this.GetRequestNotificationItem(notification)
            };

            if (notification.Operation != RisEntityOperation.Create)
            {
                importContractRequest.NotificationGUID = notification.Guid;
            }

            return importContractRequest;
        }

        /// <summary>
        /// Получить объект Item раздела importNotificationRequestNotification
        /// </summary>
        /// <param name="notification">Объект типа RisNotification</param>
        /// <returns>Объект Item</returns>
        private object GetRequestNotificationItem(RisNotification notification)
        {
            if (notification.Operation == RisEntityOperation.Delete)
            {
                return new DeleteDocType
                {
                    Delete = true
                };
            }

            if (notification.IsShipOff != null && notification.IsShipOff.Value && !notification.Guid.IsNullOrEmpty())
            {
                return true;
            }

            object[] items;
            object[] items1;

            List<ItemsChoiceType16> listItemsNames;
            Items1ChoiceType[] items1Names;

            //todo должна быть реализована логика для выбора из 5-ти типов, но на данный момент проработана бизнес модель только для 2-х (IsAll,FIASHouseGuid)
            if (notification.IsAll != null && notification.IsAll.Value)
            {
                items = new object[] { true };
                listItemsNames = new List<ItemsChoiceType16> { ItemsChoiceType16.IsAll };
            }
            else
            {
                items = this.GetHouseFiasList(notification);
                listItemsNames = new List<ItemsChoiceType16> { ItemsChoiceType16.FIASHouseGuid };
            }
     
            if (notification.IsNotLimit.HasValue && (bool)notification.IsNotLimit)
            {
                items1 = new object[] { true };
                items1Names = new[] { Items1ChoiceType.IsNotLimit };
            }
            else
            {
                var items1List = new List<object>();
                var items1NamesList = new List<Items1ChoiceType>();
                if (notification.StartDate != null)
                {
                    items1List.Add(notification.StartDate.Value);
                    items1NamesList.Add(Items1ChoiceType.StartDate);
                }
                if (notification.EndDate != null)
                {
                    items1List.Add(notification.EndDate.Value);
                    items1NamesList.Add(Items1ChoiceType.EndDate);
                }

                items1 = items1List.ToArray();
                items1Names = items1NamesList.ToArray();
            }

            return new importNotificationRequestNotificationCreate
            {
                Topic = notification.Topic,
                IsImportant = notification.IsImportant ?? false,
                content = notification.Content,
                Items = items,
                ItemsElementName = listItemsNames.ToArray(),
                Items1 = items1,
                Items1ElementName = items1Names,
                Attachment = this.GetAttachments(notification)
            };
        }

        private object[] GetHouseFiasList(RisNotification notification)
        {
            return this.notificationAddresseeList
                .Where(x => x.Notification.Id == notification.Id)
                .Select(x => (object)x.House.FiasHouseGuid)
                .ToArray();
        }

        private AttachmentType[] GetAttachments(RisNotification notification)
        {
            List<AttachmentType> result = new List<AttachmentType>();

            foreach (var attach in this.notificationAttachmentLinkList.Where(x => x.Notification.Id == notification.Id))
            {
                if (attach.Attachment != null)
                {
                    result.Add(new AttachmentType
                    {
                        Name = attach.Attachment.Name,
                        Description = attach.Attachment.Description,
                        Attachment = new Attachment
                        {
                            AttachmentGUID = attach.Attachment.Guid
                        },
                        AttachmentHASH = attach.Attachment.Hash
                    });
                }
            }

            return result.ToArray();
        }
    }
}