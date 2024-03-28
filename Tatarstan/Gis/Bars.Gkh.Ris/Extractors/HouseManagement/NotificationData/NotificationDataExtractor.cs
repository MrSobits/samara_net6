namespace Bars.Gkh.Ris.Extractors.HouseManagement.NotificationData
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.External.Housing.Notif;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Domain;

    /// <summary>
    /// Экстрактор новостей
    /// </summary>
    public class NotificationDataExtractor : BaseDataExtractor<RisNotification, Notif>
    {
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<Notif> GetExternalEntities(DynamicDictionary parameters)
        {
            var extNotifRepository = this.Container.ResolveDomain<Notif>();

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
                    .WhereIf(selectedNotificationIds.Any(), x => selectedNotificationIds.Contains(x.Id))
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
        protected override void UpdateRisEntity(Notif externalEntity, RisNotification risEntity)
        {
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "ris";
            risEntity.Contragent = this.Contragent;
            risEntity.Operation = string.IsNullOrEmpty(risEntity.Guid)
                ? RisEntityOperation.Create
                : RisEntityOperation.Update;
            risEntity.Content = externalEntity.NotifContent;
            risEntity.Topic = externalEntity.NotifTopic;
            risEntity.Deleted = externalEntity.IsDel;
            risEntity.EndDate = externalEntity.NotifFrom;
            risEntity.StartDate = externalEntity.NotifTo;
            risEntity.IsAll = externalEntity.IsAll;
            risEntity.IsImportant = externalEntity.IsImportant;
            risEntity.IsNotLimit = externalEntity.IsUnlim;
            risEntity.IsShipOff = externalEntity.IsSend;
        }
    }
}