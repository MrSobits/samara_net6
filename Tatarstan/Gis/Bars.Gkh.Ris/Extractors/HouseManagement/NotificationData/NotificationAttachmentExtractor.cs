namespace Bars.Gkh.Ris.Extractors.HouseManagement.NotificationData
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.External.Housing.Notif;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Service;
    using Bars.Gkh.Domain;
    using Quartz.Scheduler.Log;

    /// <summary>
    /// Экстрактор связей новостей - вложений
    /// </summary>
    public class NotificationAttachmentExtractor : BaseDataExtractor<RisNotificationAttachment, NotifDoc>
    {
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<NotifDoc> GetExternalEntities(DynamicDictionary parameters)
        {
            var extNotifRepository = this.Container.ResolveDomain<NotifDoc>();

            long[] selectedNotificationIds = { };

            var selectedNotifications = parameters.GetAs("selectedNotifications", string.Empty);
            if (selectedNotifications.ToUpper() == "ALL")
            {
                selectedNotificationIds = new long[0]; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedNotificationIds = selectedNotifications.ToLongArray();
            }

            try
            {
                var res = extNotifRepository
                    .GetAll()
                    .Where(x => x.DataSupplier != null && x.DataSupplier.Ogrn == this.Contragent.Ogrn)
                    .Where(x => x.Notif != null && x.Attachment != null)
                    .WhereIf(selectedNotificationIds.Any(), x => selectedNotificationIds.Contains(x.Notif.Id))
                    .ToList();
                return res;
            }
            finally
            {
                this.Container.Release(extNotifRepository);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(NotifDoc externalEntity, RisNotificationAttachment risEntity)
        {
            var notificationRepository = this.Container.ResolveDomain<RisNotification>();
            var fileUploadService = this.Container.Resolve<IAttachmentService>();
            var file = externalEntity.Attachment.FileInfo;

            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "ris";
            risEntity.Contragent = this.Contragent;
            risEntity.Operation = string.IsNullOrEmpty(risEntity.Guid)
                ? RisEntityOperation.Create
                : RisEntityOperation.Update;
            risEntity.Notification = notificationRepository
                .GetAll()
                .FirstOrDefault(x => x.ExternalSystemEntityId == externalEntity.Notif.Id);
            try
            {
                if (file != null)
                {
                    risEntity.Attachment = fileUploadService.CreateAttachment(
                        file,
                        externalEntity.Attachment.Note);
                }
            }
            catch (FileNotFoundException)
            {
                risEntity.Attachment = null;
                this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл уведомления " + file?.FullName + " не найден"));
            }
            finally
            {
                this.Container.Release(notificationRepository);
                this.Container.Release(fileUploadService);
            }
        }
    }
}