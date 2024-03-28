namespace Bars.Gkh.Ris.Extractors.HouseManagement.NotificationData
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.External.Housing.House;
    using Bars.GisIntegration.Base.Entities.External.Housing.Notif;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Экстрактор новостей
    /// </summary>
    public class NotificationAddresseeExtractor : BaseDataExtractor<RisNotificationAddressee, NotifAddress>
    {
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<NotifAddress> GetExternalEntities(DynamicDictionary parameters)
        {
            var extNotifAddressRepository = this.Container.ResolveDomain<NotifAddress>();

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

            var contragent = parameters.GetAs("Contragent", this.Contragent);

            try
            {
                var res = extNotifAddressRepository
                    .GetAll()
                    .Where(x => x.Notif != null)
                    .Where(x => x.DataSupplier != null && x.DataSupplier.Ogrn == contragent.Ogrn)
                    .WhereIf(selectedNotificationIds.Any(), x => selectedNotificationIds.Contains(x.Notif.Id))
                    .ToList();

                return res.ToList();
            }
            finally
            {
                this.Container.Release(extNotifAddressRepository);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(NotifAddress externalEntity, RisNotificationAddressee risEntity)
        {
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "ris";
            risEntity.Notification = this.GetRisNotification(externalEntity.Notif);
            risEntity.House = this.GetRisHouse(externalEntity.House);
        }

        /// <summary>
        /// Получение объекта RisNotification
        /// </summary>
        private RisNotification GetRisNotification(Notif externalNotification)
        {
            var notificationRepository = this.Container.ResolveDomain<RisNotification>();

            try
            {
                return notificationRepository
                    .GetAll()
                    .FirstOrDefault(x => x.ExternalSystemEntityId == externalNotification.Id);
            }
            finally
            {
                this.Container.Release(notificationRepository);
            }
        }

        /// <summary>
        /// Получение объекта RisHouse
        /// </summary>
        private RisHouse GetRisHouse(House externalHouse)
        {
            var realityObjectRepository = this.Container.ResolveDomain<RealityObject>();
            var houseRepository = this.Container.ResolveDomain<RisHouse>();

            try
            {
                //ищем дом в gkh_reality_object через FiasAddress
                var realityObject = realityObjectRepository
                    .GetAll()
                    .FirstOrDefault(x => x.FiasAddress == externalHouse.FiasAddress);

                if (realityObject != null)
                {
                    //Если такой есть, то ищем его в ris_house
                    return houseRepository
                        .GetAll()
                        .FirstOrDefault(x => x.ExternalSystemName == "gkh" && x.ExternalSystemEntityId == realityObject.Id);
                }

                return null;
            }
            finally
            {
                this.Container.Release(realityObjectRepository);
                this.Container.Release(houseRepository);
            }
        }
    }
}