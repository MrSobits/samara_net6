namespace Bars.Gkh.Regions.Tatarstan.Interceptors.ConstructionObject
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Regions.Tatarstan.Entities;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.AddressPattern;

    /// <summary>
    /// Интерцептор
    /// </summary>
    public class ConstructionObjectInterceptor : EmptyDomainInterceptor<ConstructionObject>
    {
        /// <summary>
        /// Действие перед созданием
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат</returns>
        public override IDataResult BeforeCreateAction(IDomainService<ConstructionObject> service, ConstructionObject entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.FiasAddress);

            this.BaseValidConstructionObject(entity);

            entity.MoSettlement = Utils.GetMoSettlement(this.Container, entity.FiasAddress);

            var mcp = Utils.GetMunicipality(this.Container, entity.FiasAddress) ?? (entity.MoSettlement.ReturnSafe(x => x.ParentMo) ?? entity.MoSettlement);

            if (mcp == null)
            {
                return this.Failure("По адресу не удалось определить Муниципальное образование");
            }

            entity.Municipality = mcp;

            // Перед добавлением формируем адрес
            entity.Address = null;
            if (entity.FiasAddress != null)
            {
                entity.Address = entity.Municipality != null
                    ? this.Container.Resolve<IAddressPattern>().FormatShortAddress(entity.Municipality, entity.FiasAddress)
                    : entity.FiasAddress.AddressName;
            }

            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }

        /// <summary>
        /// Действие до удаления
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<ConstructionObject> service, ConstructionObject entity)
        {
            var contractDomain = this.Container.Resolve<IDomainService<ConstructionObjectContract>>();
            var documentDomain = this.Container.Resolve<IDomainService<ConstructionObjectDocument>>();
            var typeWorkDomain = this.Container.Resolve<IDomainService<ConstructionObjectTypeWork>>();
            var photoDomain = this.Container.Resolve<IDomainService<ConstructionObjectPhoto>>();
            var participantDomain = this.Container.Resolve<IDomainService<ConstructionObjectParticipant>>();
            var smrDomain = this.Container.Resolve<IDomainService<ConstructObjMonitoringSmr>>();

            try
            {
                var checkContracts = contractDomain.GetAll().Any(x => x.ConstructionObject == entity);
                var checkDocuments = documentDomain.GetAll().Any(x => x.ConstructionObject == entity);
                var checktypeWorks = typeWorkDomain.GetAll().Any(x => x.ConstructionObject == entity);
                var checkPhotos = photoDomain.GetAll().Any(x => x.ConstructionObject == entity);
                var checkParticipants = participantDomain.GetAll().Any(x => x.ConstructionObject == entity);

                if (checkContracts || checkDocuments || checktypeWorks || checkPhotos || checkParticipants)
                {
                    return this.Failure("Удаление невозможно по причине наличия связанных записей. Вначале необходимо удалить все связанные документы");
                }

                var smrId = smrDomain.GetAll().Where(x => x.ConstructionObject == entity).Select(x => x.Id).FirstOrDefault();

                smrDomain.Delete(smrId);

                return this.Success();
            }
            finally
            {
                this.Container.Release(contractDomain);
                this.Container.Release(documentDomain);
                this.Container.Release(typeWorkDomain);
                this.Container.Release(photoDomain);
                this.Container.Release(participantDomain);
                this.Container.Release(smrDomain);
            }
        }

        /// <summary>
        /// Действие после создания
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат</returns>
        public override IDataResult AfterCreateAction(IDomainService<ConstructionObject> service, ConstructionObject entity)
        {
            var smrDomain = this.Container.Resolve<IDomainService<ConstructObjMonitoringSmr>>();
            using (this.Container.Using(smrDomain))
            {
                var monitoringSmr = new ConstructObjMonitoringSmr { ConstructionObject = entity };
                smrDomain.Save(monitoringSmr);
            }

            return this.Success();
        }

        /// <summary>
        /// Действие перед обновлением
        /// </summary>
        /// <param name="service">Домен-сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<ConstructionObject> service, ConstructionObject entity)
        {
            Utils.SaveFiasAddress(this.Container, entity.FiasAddress);

            this.BaseValidConstructionObject(entity);

            entity.MoSettlement = Utils.GetMoSettlement(this.Container, entity.FiasAddress);

            var mcp = Utils.GetMunicipality(this.Container, entity.FiasAddress) ?? (entity.MoSettlement.ReturnSafe(x => x.ParentMo) ?? entity.MoSettlement);

            if (mcp == null)
            {
                return this.Failure("По адресу не удалось определить Муниципальное образование");
            }

            entity.Municipality = mcp;

            // Перед добавлением формируем адрес
            entity.Address = null;
            if (entity.FiasAddress != null)
            {
                entity.Address = entity.Municipality != null
                    ? this.Container.Resolve<IAddressPattern>().FormatShortAddress(entity.Municipality, entity.FiasAddress)
                    : entity.FiasAddress.AddressName;
            }

            return this.Success();
        }

        /// проверяем уникальность адреса
        private bool ValidFiasAddress(long id, FiasAddress fiasaddress)
        {
            // Для проверки уникальности дома намеренно используем IRepository,
            // так как необходима проверка среди всех домов, а не только допустимых пользователю

            var constructObjectRepository = this.Container.Resolve<IRepository<ConstructionObject>>();

            bool valid;

            using (this.Container.Using(constructObjectRepository))
            {
                // И не надо переделывать нижеследующий запрос под Any ! 
                valid = constructObjectRepository.GetAll()
                    .Where(x => x.Id != id)
                    .Where(x => x.FiasAddress != null)
                    .Count(x => x.FiasAddress.AddressName.ToLower() == fiasaddress.AddressName.ToLower()) == 0;
            }

            return valid;
        }

        private void BaseValidConstructionObject(ConstructionObject constrcutObject)
        {
            if (constrcutObject.FiasAddress == null)
            {
                throw new ValidationException("Не заполнен адрес");
            }

            var address = constrcutObject.FiasAddress;
            if (string.IsNullOrEmpty(address.AddressName))
            {
                throw new ValidationException("Не заполнен адрес");
            }

            if (!this.ValidFiasAddress(constrcutObject.Id, address))
            {
                throw new ValidationException("Дом с таким адресом уже существует в системе");
            }
        }
    }
}