namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.General;
    using Bars.Gkh.ConfigSections.General.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Extension;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.AddressPattern;
    using Bars.Gkh.Utils.EntityExtensions;

    using NHibernate.Linq;

    /// <summary>
    /// Интерцептор для сущности Жилой дом
    /// </summary>
    public class RealityObjectInterceptor : EmptyDomainInterceptor<RealityObject>
    {
        /// <summary>
        /// Домен-сервис "Жилой дом"
        /// </summary>
        public IDomainService<RealityObject> RoDomain { get; set; }

        /// <summary>
        /// Сервис для синхронизации данных жилого дома и тех. паспорта
        /// </summary>
        public IRealityObjectTpSyncService TpSyncService { get; set; }

        /// <summary>
        /// Действие, выполняемое до добавления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Жилой дом"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            var fiasAddress = entity.FiasAddress;

            if (fiasAddress.IsNotNull())
            {
                // сохранение ФИАС адреса при созданиии нового дома отличается 
                this.SaveFiasAddressBeforeCreate(fiasAddress);
            }

            this.BaseValidRealityObject(entity);

            entity.MoSettlement = Utils.GetSettlementByRealityObject(this.Container, entity);

            var mcp = Utils.GetMunicipality(this.Container, entity.FiasAddress) ?? (entity.MoSettlement.ReturnSafe(x => x.ParentMo) ?? entity.MoSettlement);
            
            if (mcp == null)
            {
                return this.Failure("По адресу не удалось определить Муниципальное образование");
            }
            
            var unProxy = this.Container.Resolve<IUnProxy>();
            entity.Municipality = (Municipality) unProxy.GetUnProxyObject(mcp);

            // Перед добавлением формируем адрес
            entity.Address = null;
            if (entity.FiasAddress != null)
            {
                entity.Address = entity.Municipality != null
                    ? this.Container.Resolve<IAddressPattern>().FormatShortAddress(entity.Municipality, entity.FiasAddress)
                    : entity.FiasAddress.AddressName;
            }

            var settlementMo = Utils.GetSettlementByRealityObject(this.Container, entity);

            entity.MoSettlement = settlementMo;

            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }

        private void SaveFiasAddressBeforeCreate(FiasAddress fiasAddress)
        {
            var configProvider = this.Container.Resolve<IGkhConfigProvider>();
            var useFiasIdentification = configProvider.Get<GeneralConfig>().UseFiasHouseIdentification == UseFiasHouseIdentification.Use;

            var fiasAddressDomain = this.Container.Resolve<IDomainService<FiasAddress>>();
            var roRepository = this.Container.ResolveRepository<RealityObject>();

            using (this.Container.Using(configProvider, fiasAddressDomain, roRepository))
            {
                if (useFiasIdentification && fiasAddress.HouseGuid != null)
                {
                    var fiasExists = roRepository.GetAll()
                        .WhereIf(fiasAddress.Id > 0, x => x.FiasAddress.Id != fiasAddress.Id)
                        .Any(x => x.FiasAddress.HouseGuid == fiasAddress.HouseGuid);

                    if (fiasExists)
                    {
                        throw new ValidationException($"Дом с данным идентификатором '{fiasAddress.HouseGuid}' уже существует в системе");
                    }
                }

                fiasAddressDomain.SaveOrUpdate(fiasAddress);
            }
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Жилой дом"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            if (entity.FiasAddress.Id == 0)
            {
                var fiasAddressDomain = this.Container.Resolve<IDomainService<FiasAddress>>();

                using (this.Container.Using(fiasAddressDomain))
                {
                    fiasAddressDomain.Save(entity.FiasAddress);
                }
            }

            this.BaseValidRealityObject(entity);

            if (this.CheckDirtyAddress(entity))
            {
                Utils.SaveFiasAddress(this.Container, entity.FiasAddress);

                entity.MoSettlement = Utils.GetSettlementByRealityObject(this.Container, entity);

                var mcp = Utils.GetMunicipality(this.Container, entity.FiasAddress) ?? (entity.MoSettlement.ReturnSafe(x => x.ParentMo) ?? entity.MoSettlement);

                if (mcp == null)
                {
                    return this.Failure("По адресу не удалось определить Муниципальное образование");
                }

                entity.Municipality = mcp;

                // Перед обновлением обновляем и адрес
                entity.Address = null;

                /* Тут если проходит update при обновлении адреса ругается на то что Id адреса уже 
                 * задействован в текущей сессии */

                if (entity.FiasAddress != null)
                {
                    entity.Address = entity.Municipality != null
                        ? this.Container.Resolve<IAddressPattern>().FormatShortAddress(entity.Municipality, entity.FiasAddress)
                        : entity.FiasAddress.AddressName;
                }
            }
            
            return this.Success();
        }

        private void BaseValidRealityObject(RealityObject realtyObject)
        {
            if (realtyObject.FiasAddress == null)
            {
                throw new ValidationException("Не заполнен адрес");
            }

            var address = realtyObject.FiasAddress;
            if (string.IsNullOrEmpty(address.AddressName))
            {
                throw new ValidationException("Не заполнен адрес");
            }

            if (!this.ValidFiasAddress(realtyObject.Id, address))
            {
                throw new ValidationException("Дом с таким адресом уже существует в системе");
            }
        }

        /// <summary>
        /// Последействие
        /// </summary>
        /// <param name="service">Домен-сервис "Жилой дом"</param>
        /// <param name="actionType">Тип операции</param>
        /// <param name="entity">Сущность</param>
        public void AfterAction(IDomainService<RealityObject> service, ServiceOperationType actionType, RealityObject entity)
        {
            switch (actionType)
            {
                case ServiceOperationType.Update:
                {
                    // Если DeleteAddressId > 0 то значит его надо удалить
                    if (entity.DeleteAddressId > 0)
                    {
                        var addressRepository = this.Container.Resolve<IRepository<FiasAddress>>();
                        addressRepository.Delete(entity.DeleteAddressId);
                    }
                }
                    break;
            }
        }

        /// проверяем уникальность адреса
        private bool ValidFiasAddress(long id, FiasAddress fiasaddress)
        {
            // Для проверки уникальности дома намеренно используем IRepository,
            // так как необходима проверка среди всех домов, а не только допустимых пользователю

            var realityObjectRepository = this.Container.Resolve<IRepository<RealityObject>>();
            using (this.Container.Using(realityObjectRepository))
            {
                // И не надо переделывать нижеследующий запрос под Any !

                //try
                //{
                //    var v = realityObjectRepository.GetAll()
                //      .WhereIf(id > 0, x => x.Id != id)
                //      .Where(x => x.ConditionHouse != ConditionHouse.Razed)
                //      .Where(x => x.FiasAddress != null).ToList();

                //}
                //catch (Exception e)
                //{
                //    string str = e.ToString();
                //}

                
                return realityObjectRepository.GetAll()

                    .WhereIf(id > 0, x => x.Id != id)
                    .Where(x => x.ConditionHouse != ConditionHouse.Razed)
                    .Where(x => x.FiasAddress != null)
                    .Count(x => x.FiasAddress.AddressName.ToLower() == fiasaddress.AddressName.ToLower()) == 0;
            }
        }

        /// <summary>
        /// Действие, выполняемое до удаления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Жилой дом"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            var supplierRelationServ = this.Container.Resolve<IDomainService<SupplyResourceOrgRealtyObject>>();
            var realtions = supplierRelationServ.GetAll().Where(x => x.RealityObject.Id == entity.Id);

            foreach (var rel in realtions)
            {
                supplierRelationServ.Delete(rel.Id);
            }
            return base.BeforeDeleteAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterCreateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            var result = this.UpdateTechPassportFields(entity);
            if (!result.Success)
            {
                return result;
            }

            return this.Success();
        }

        /// <summary>
        /// Метод вызывается после обновления объекта
        /// </summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult AfterUpdateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            var config = this.Container.GetGkhConfig<GeneralConfig>().AutoIsNotInvolved;

            if (config == AutoIsNotInvolved.Use)
            {
                entity.IsNotInvolvedCrReason = 0;

                if (entity.PhysicalWear > 70)
                {
                    entity.IsNotInvolvedCr = true;

                    entity.IsNotInvolvedCrReason |= IsNotInvolvedCrReason.PhysicalWear;
                }

                if (entity.TypeHouse == TypeHouse.BlockedBuilding)
                {
                    entity.IsNotInvolvedCr = true;

                    entity.IsNotInvolvedCrReason |= IsNotInvolvedCrReason.BlockedBuilding;
                }

                if (entity.NumberApartments <= 2)
                {
                    entity.IsNotInvolvedCr = true;

                    entity.IsNotInvolvedCrReason |= IsNotInvolvedCrReason.NumberApartments;
                }

                if (entity.IsNotInvolvedCrReason == 0)
                {
                    entity.IsNotInvolvedCr = false;
                }
            }

            var result = this.UpdateTechPassportFields(entity);
            if (!result.Success)
            {
                return result;
            }

            return this.Success();
        }

        private BaseDataResult UpdateTechPassportFields(RealityObject entity)
        {
            try
            {
                this.TpSyncService.Sync(entity);
            }
            catch (Exception ex)
            {
                return new BaseDataResult(false, ex.Message);
            }

            return new BaseDataResult(true);
        }

        private bool CheckDirtyAddress(RealityObject realityObject)
        {
            using (var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession())
            {
                var oldAddressId = session.Query<RealityObject>()
                    .Where(x => x.Id == realityObject.Id)
                    .Select(x => x.FiasAddress.Id)
                    .FirstOrDefault();

                if (oldAddressId != realityObject.FiasAddress?.Id)
                {
                    return true;
                }

                var newAddress = realityObject.FiasAddress;
                var oldAddress = session.Get<FiasAddress>(oldAddressId);
                var properties = typeof(FiasAddress).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(x => x.PropertyType == typeof(string));
                foreach (var propertyInfo in properties)
                {
                    var oldValue = propertyInfo.GetValue(oldAddress) as string;
                    var newValue = propertyInfo.GetValue(newAddress) as string;
                    if (oldValue != newValue)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}