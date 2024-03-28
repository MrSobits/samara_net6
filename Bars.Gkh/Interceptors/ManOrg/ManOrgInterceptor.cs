namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities.ManOrg;
    using Bars.Gkh.Entities.Suggestion;
    using System.Reflection;
    using Entities;
    using Utils;

    /// <summary>
    /// Интерцептор для сущности Управляющая операция
    /// </summary>
    public class ManOrgInterceptor : EmptyDomainInterceptor<ManagingOrganization>
    {
        public void ToDerived(FiasAddress tBase, FiasAddress tDerived)
        {
            foreach (PropertyInfo propBase in tBase.GetType().GetProperties())
            {
                if (propBase.Name == "Id")
                    continue;

                PropertyInfo propDerived = tDerived.GetType().GetProperty(propBase.Name);
                propDerived.SetValue(tDerived, propBase.GetValue(tBase, null), null);
            }
        }

        /// <summary>
        /// Действие, выполняемое до добавления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Управляющая операция"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeCreateAction(IDomainService<ManagingOrganization> service, ManagingOrganization entity)
        {
            try
            {
                return CheckContragent(entity)
                       ? Failure("Для указанного контрагента уже существует управляющая организация.")
                       : Success();
            }
            catch(Exception exc){
                return new BaseDataResult(false, exc.Message);
            }
            
        }

        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Управляющая операция"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<ManagingOrganization> service, ManagingOrganization entity)
        {
            try
            {
                return CheckContragent(entity)
                           ? Failure("Для указанного контрагента уже существует управляющая организация.")
                           : Success();
            }
            catch(Exception exc){
                return new BaseDataResult(false, exc.Message);
            }
        }

        private bool CheckContragent(ManagingOrganization entity)
        {
            // Если выставлена галочка "Адрес диспетчерской службы соответствует фактическому адресу" то копируем адресс
            if(entity.IsDispatchCrrespondedFact && entity.Contragent.FiasFactAddress != null)
            {
                var factAddress = Container.Resolve<IRepository<FiasAddress>>().Load(entity.Contragent.FiasFactAddress.Id);

                if(entity.DispatchAddress != null && entity.DispatchAddress.AddressName != factAddress.AddressName)
                {
                    ToDerived(factAddress, entity.DispatchAddress);
                }
                else if(entity.DispatchAddress == null) 
                {
                    entity.DispatchAddress = new FiasAddress();
                    ToDerived(factAddress, entity.DispatchAddress);
                }

            }
            else if (entity.IsDispatchCrrespondedFact && entity.Contragent.FactAddress == null)
            {
                throw new Exception("Необходимо заполнить фактичекий адресс контрагента");
            }


            Utils.SaveFiasAddress(Container, entity.DispatchAddress);

            return
                Container.Resolve<IRepository<ManagingOrganization>>().GetAll()
                    .Any(x => x.Contragent.Id == entity.Contragent.Id && x.Id != entity.Id);
        }

        /// <summary>
        /// Действие, выполняемое до удаления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Управляющая операция"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<ManagingOrganization> service, ManagingOrganization entity)
        {
            var belayManOrgActServ = Container.Resolve<IDomainService<BelayManOrgActivity>>();
            var manOrgContrTransfServ = Container.Resolve<IDomainService<ManOrgContractTransfer>>();
            var manOrgJskTsjContrServ = Container.Resolve<IDomainService<ManOrgJskTsjContract>>();
            var manOrgClaimServ = Container.Resolve<IDomainService<ManagingOrgClaim>>();
            var manOrgDocumServ = Container.Resolve<IDomainService<ManagingOrgDocumentation>>();
            var manOrgMemberServ = Container.Resolve<IDomainService<ManagingOrgMembership>>();
            var manOrgMunicServ = Container.Resolve<IDomainService<ManagingOrgMunicipality>>();
            var manOrgRoServ = Container.Resolve<IDomainService<ManagingOrgRealityObject>>();
            var manOrgRegistryServ = Container.Resolve<IDomainService<ManagingOrgRegistry>>();
            var manOrgServServ = Container.Resolve<IDomainService<ManagingOrgService>>();
            var manOrgWorkModeServ = Container.Resolve<IDomainService<ManagingOrgWorkMode>>();
            var citizenSuggServ = Container.Resolve<IDomainService<CitizenSuggestion>>();
            var manOrgContrOwnServ = Container.Resolve<IDomainService<ManOrgContractOwners>>();
            var manOrgBaseContrServ = Container.Resolve<IDomainService<ManOrgBaseContract>>();

            try
            {
                belayManOrgActServ.GetAll().Where(x => x.ManagingOrganization.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => belayManOrgActServ.Delete(x));

                manOrgContrTransfServ.GetAll().Where(x => x.ManagingOrganization.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgContrTransfServ.Delete(x));

                manOrgJskTsjContrServ.GetAll().Where(x => x.ManagingOrganization.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgJskTsjContrServ.Delete(x));

                manOrgClaimServ.GetAll().Where(x => x.ManagingOrganization.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgClaimServ.Delete(x));

                manOrgDocumServ.GetAll().Where(x => x.ManagingOrganization.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgDocumServ.Delete(x));

                manOrgMemberServ.GetAll().Where(x => x.ManagingOrganization.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgMemberServ.Delete(x));

                manOrgMunicServ.GetAll().Where(x => x.ManOrg.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgMunicServ.Delete(x));

                manOrgRoServ.GetAll().Where(x => x.ManagingOrganization.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgRoServ.Delete(x));

                manOrgRegistryServ.GetAll().Where(x => x.ManagingOrganization.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgRegistryServ.Delete(x));

                manOrgServServ.GetAll().Where(x => x.ManagingOrganization.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgServServ.Delete(x));

                manOrgWorkModeServ.GetAll().Where(x => x.ManagingOrganization.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgWorkModeServ.Delete(x));

                manOrgContrOwnServ.GetAll().Where(x => x.ManagingOrganization.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgContrOwnServ.Delete(x));

                manOrgBaseContrServ.GetAll().Where(x => x.ManagingOrganization.Id == entity.Id)
                    .Select(x => x.Id).ForEach(x => manOrgBaseContrServ.Delete(x));

                var citizenSuggList =
                    citizenSuggServ.GetAll().Where(x => x.ExecutorManagingOrganization.Id == entity.Id);
                foreach (var citizenSugg in citizenSuggList)
                {
                    citizenSugg.ExecutorManagingOrganization = null;
                    citizenSuggServ.Update(citizenSugg);
                }

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(belayManOrgActServ);
                Container.Release(manOrgContrTransfServ);
                Container.Release(manOrgJskTsjContrServ);
                Container.Release(manOrgClaimServ);
                Container.Release(manOrgDocumServ);
                Container.Release(manOrgMemberServ);
                Container.Release(manOrgMunicServ);
                Container.Release(manOrgRoServ);
                Container.Release(manOrgRegistryServ);
                Container.Release(manOrgServServ);
                Container.Release(manOrgWorkModeServ);
                Container.Release(citizenSuggServ);
                Container.Release(manOrgContrOwnServ);
                Container.Release(manOrgBaseContrServ);
            }
        }
    }
}