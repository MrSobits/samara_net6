namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Utils;

    using Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для Контрагент
    /// </summary>
    public class ContragentService : IContragentService
    {
        private const string ManagerPositionCode = "1";

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен сервис для Контрагент
        /// </summary>
        public IDomainService<Contragent> ContragentDomain { get; set; }

        /// <summary>
        /// Репозиторий для Контрагент
        /// </summary>
        public IRepository<Contragent> ContragentRepository { get; set; }

        /// <summary>
        /// Домен сервис для Муниципальные образования контрагенты
        /// </summary>
        public IDomainService<ContragentMunicipality> ContragentMunicipalityDomain { get; set; }

        /// <summary>
        /// Домен сервис для Контактная информация по контрагенту
        /// </summary>
        public IDomainService<ContragentContact> ContragentContactDomainService { get; set; }

        /// <summary>
        /// Домен сервис для Управляющая организация
        /// </summary>
        public IDomainService<ManagingOrganization> ManOrgDomain { get; set; }

        /// <summary>
        /// Домен сервис для Подрядчики
        /// </summary>
        public IDomainService<Builder> BuilderDomain { get; set; }

        /// <summary>
        /// Домен сервис для Подрядчики
        /// </summary>
        public IDomainService<PersonPlaceWork> PersonPlaceWorkDomain { get; set; }

        /// <summary>
        /// Домен сервис для Органы местного самоуправления
        /// </summary>
        public IDomainService<LocalGovernment> LocalGovDomain { get; set; }

        /// <summary>
        /// Домен сервис для Поставщик коммунальных услуг
        /// </summary>
        public IDomainService<SupplyResourceOrg> SupplyResOrgDomain { get; set; }

        /// <summary>
        /// Домен сервис для Обслуживающая организация (Поставщик жилищных услуг)
        /// </summary>
        public IDomainService<ServiceOrganization> ServiceOrgDomain { get; set; }

        /// <summary>
        /// Домен сервис для Органы государственной власти
        /// </summary>
        public IDomainService<PoliticAuthority> PoliticAuthorityDomain { get; set; }

        /// <summary>
        /// Домен сервис для Муниципальное образование
        /// </summary>
        public IDomainService<Municipality> MunicipalityDomain { get; set; }

        /// <summary>
        /// Домен сервис для Региональный оператор
        /// </summary>
        public IDomainService<RegOperator> RegOperatorDomain { get; set; }

        /// <summary>
        /// Домен сервис для Поставщик ресурсов
        /// </summary>
        public IDomainService<PublicServiceOrg> PublicServiceOrgDomain { get; set; }

        /// <summary>
        /// Получить список без потомков
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult ListExceptChildren(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var id = baseParams.Params.ContainsKey("contragentId") ? baseParams.Params["contragentId"].ToInt() : 0;

            if (id > 0)
            {
                var result = new List<long> { id };

                this.GetExceptedContragents(result, this.ContragentDomain, id);

                var data = this.ContragentDomain.GetAll()
                    .Where(x => !result.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        Municipality = x.Municipality.Name,
                        x.Inn,
                        x.Kpp
                    })
                    .Filter(loadParam, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }

            return new ListDataResult(null, 0);
        }

        /// <summary>
        /// Получить список контрагентов для специального счета
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult ListForSpecialAccount(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var data = this.Container.Resolve<IDomainService<ManagingOrganization>>().GetAll()
                         .Where(x => x.TypeManagement == TypeManagementManOrg.JSK || x.TypeManagement == TypeManagementManOrg.TSJ)
                         .Select(x => new
                         {
                             x.Contragent.Id,
                             Municipality = x.Contragent.Municipality.Name,
                             x.Contragent.Name,
                         })
                         .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                         .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            data = data.Order(loadParam).Paging(loadParam);

            return new ListDataResult(data.ToList(), totalCount);
        }

        /// <summary>
        /// Получаем выборку контрагентов по типу Исполнителя
        /// </summary>
        /// <param name="type">Тип исполнителя это код в справочнике</param>
        /// <returns>Модифицированный запрос</returns>
        public IQueryable<Contragent> ListForTypeExecutant(int type)
        {

            var result = this.ContragentRepository.GetAll();

            switch (type)
            {
                // ссылка на реестр управляющих организаций, тип УК
                case 0:
                case 1:
                    {
                        result = result.Where(x => this.ManOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Id
                                                            && y.TypeManagement == TypeManagementManOrg.UK));
                    }
                    break;

                // ссылка на реестр управляющих организаций, тип ТСЖ 
                case 2:
                case 3:
                    {
                        result = result.Where(x => this.ManOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Id
                                                            && y.TypeManagement == TypeManagementManOrg.TSJ));
                    }
                    break;

                // ссылка на реестр управляющих организаций, тип ЖСК 
                case 4:
                case 5:
                    {
                        result = result.Where(x => this.ManOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Id
                                                            && y.TypeManagement == TypeManagementManOrg.JSK));
                    }
                    break;

                // ссылка на реестр подрядчиков
                case 17:
                case 18:
                    {
                        result = result.Where(x => this.BuilderDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
                    }
                    break;

                // ссылка на реестр органы местного самоуправления
                case 15:
                case 16:
                    {
                        result = result.Where(x => this.LocalGovDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
                    }
                    break;

                // ссылка на реестр поставщики коммунальных услуг(ресурсосберегающие орг-ии)
                case 6:
                case 19:
                    {
                        result = result.Where(x => this.SupplyResOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
                    }
                    break;

                // ссылка на реестр поставщики жилищных услуг
                case 10:
                case 11:
                    {
                        result = result.Where(x => this.ServiceOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
                    }
                    break;

                // ссылка на реестр региональных операторов
                case 7:
                    {
                        result = result.Where(x => this.RegOperatorDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// Получаем выборку контрагентов по типу Юр лица
        /// </summary>
        /// <param name="type">Тип юр лица это в ГЖИ поэтому пока непривязваюсь к Enum потому что эт онад оставить ссылку либо переносить енум в модуль Gkh</param>
        /// <returns>Модифицированный запрос</returns>
        public IQueryable<Contragent> ListForTypeJurOrg(int type)
        {
            var result = this.ContragentDomain.GetAll();

            var serviceContragentForTypeJurOrg = this.Container.ResolveAll<IContragentListForTypeJurOrg>();

            foreach (var serv in serviceContragentForTypeJurOrg)
            {
                serv.GetQueryableForTypeJurOrg(ref result, (TypeJurPerson)type);
            }

            return result;
        }

        /// <summary>
        /// Получаем выборку контрагентов по типу Поставщиков услуг 
        /// </summary>
        /// <param name="type">Тип поставщика услуг </param>
        /// <returns>Модифицированный запрос</returns>
        public IQueryable<Contragent> ListForTypeServOrg(int type)
        {
            var result = this.ContragentDomain.GetAll();

            switch (type)
            {
                // Поставщик коммунальных услуг (SupplyResourceOrg)
                case 10:
                    {
                        result = result.Where(x => this.SupplyResOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
                    }
                    break;

                // Поставщик жилищных услуг (ServiceOrganization)
                case 20:
                    {
                        result = result.Where(x => this.ServiceOrgDomain.GetAll().Any(y => y.Contragent.Id == x.Id));
                    }
                    break;
            }

            return result;
        }

        /// <summary>
        /// Получить тип юридического лица
        /// </summary>
        /// <param name="contragent">Контрагент</param>
        /// <returns>Тип юридического лица</returns>
        public TypeJurPerson GetTypeJurPerson(Contragent contragent)
        {
            if (this.ManOrgDomain.GetAll().Any(x => x.Contragent.Id == contragent.Id))
            {
                return TypeJurPerson.ManagingOrganization;
            }

            if (this.SupplyResOrgDomain.GetAll().Any(x => x.Contragent.Id == contragent.Id))
            {
                return TypeJurPerson.SupplyResourceOrg;
            }

            if (this.LocalGovDomain.GetAll().Any(x => x.Contragent.Id == contragent.Id))
            {
                return TypeJurPerson.LocalGovernment;
            }

            if (this.PoliticAuthorityDomain.GetAll().Any(x => x.Contragent.Id == contragent.Id))
            {
                return TypeJurPerson.PoliticAuthority;
            }

            if (this.BuilderDomain.GetAll().Any(x => x.Contragent.Id == contragent.Id))
            {
                return TypeJurPerson.Builder;
            }

            if (this.ServiceOrgDomain.GetAll().Any(x => x.Contragent.Id == contragent.Id))
            {
                return TypeJurPerson.ServOrg;
            }

            if (this.RegOperatorDomain.GetAll().Any(x => x.Contragent.Id == contragent.Id))
            {
                return TypeJurPerson.RegOp;
            }

            if (this.PublicServiceOrgDomain.GetAll().Any(x => x.Contragent.Id == contragent.Id))
            {
                return TypeJurPerson.PublicServiceOrg;
            }

            return 0;
        }

        /// <summary>
        /// Получение МО, которые еще не привязаны к контрагенту
        /// </summary>
        public IDataResult ListAvailableMunicipality(BaseParams baseParams)
        {
            var contrId = baseParams.Params.GetAs<long>("contragentId");

            var alreadyMunicipalities = this.ContragentMunicipalityDomain.GetAll()
                                              .Where(x => x.Contragent.Id == contrId)
                                              .Select(x => x.Municipality.Id)
                                              .ToList();

            var loadParam = baseParams.GetLoadParam();

            var data = this.MunicipalityDomain.GetAll()
                                         .Where(x => !alreadyMunicipalities.Contains(x.Id))
                                         .Select(x => new { x.Id, x.Name })
                                         .Filter(loadParam, this.Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), data.Count());
        }

        /// <summary>
        /// Получить контакт действующего руководителя для контрагента
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult GetActualManagerContact(BaseParams baseParams)
        {
            var contragentId = baseParams.Params.GetAs<long>("contragentId");

            var contact = this.GetActualManagerContacts(contragentId).Get(contragentId);
            return contact != null ? new BaseDataResult(contact) : new BaseDataResult(false);
        }

        public IDataResult GetContactsFromDL(BaseParams baseParams, Int64 contragentId)
        {           
            var contact = this.GetActualManagerContacts(contragentId).Get(contragentId);

            return  new BaseDataResult(true);
        }
        public IDataResult UpdateContactsFromDL(BaseParams baseParams, Int64 contragentId)
        {
            var result = this.ContragentContactDomainService.GetAll()
              .Where(x => x.Contragent.Id == contragentId)
              .Where(x => x.DateStartWork.HasValue).ToList();

            var persons = PersonPlaceWorkDomain.GetAll()
                .Where(x => x.Contragent != null && x.Contragent.Id == contragentId)
                .Select(x => x.Person).ToList();
            if (persons.Count > 0)
            {
                foreach (var person in persons)
                {
                    var existsContact = result.Where(x => x.FullName == person.FullName).FirstOrDefault();
                    if (existsContact == null)
                    {
                        var personPosition = PersonPlaceWorkDomain.GetAll().FirstOrDefault(x => x.Person == person);
                        ContragentContact newContact = new ContragentContact
                        {
                            FullName = person.FullName,
                            Contragent = new Contragent {Id = contragentId },
                            Name = person.Name,
                            Patronymic = person.Patronymic,
                            Surname = person.Surname,
                            Position = personPosition?.Position,
                            BirthDate = person.Birthdate,
                            DateStartWork = personPosition?.StartDate,
                            Email = person.Email,
                            Annotation = "Перенесено из реестра должностных лиц"
                        };
                        try
                        {
                            ContragentContactDomainService.Save(newContact);
                        }
                        catch(Exception e)
                        {
                            
                        }
                    }
                }
            }

            return new BaseDataResult(true);
        }


        /// <summary>
        /// Вернуть сгенерированный код поставщика
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Код поставщика</returns>
        public IDataResult GenerateProviderCode(BaseParams baseParams)
        {
            var providerName = baseParams.Params.GetAs("providerName", string.Empty);
            if (providerName.IsEmpty())
            {
                return BaseDataResult.Error("Не заполнено обязательное поле: Краткое наименование");
            }

            return new BaseDataResult(new { ProviderCode = CrcGenerator.CalcCRC(providerName).ToString() });
        }

        /// <summary>
        /// Вернуть всех активных контрагентов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        public IDataResult GetAllActiveContragent(BaseParams baseParams)
        {
            return this.ContragentRepository.GetAll()
                .Where(x => x.ContragentState == ContragentState.Active)
                .Select(x => new
                {
                    x.Id,
                    x.ShortName,
                    x.Name,
                    Municipality = x.Municipality.Name,
                    x.Inn
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }

        /// <inheritdoc />
        public IDataResult ListAdditionRole(BaseParams baseParams)
        {
            var additionRoleDomain = this.Container.ResolveDomain<ContragentAdditionRole>();

            using (this.Container.Using(additionRoleDomain))
            {
                var contragentId = baseParams.Params.GetAsId("entityId");

                return additionRoleDomain.GetAll().Where(x => x.Contragent.Id == contragentId).Select(x => new
                {
                    x.Id,
                    x.Role.Name
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }
        
        /// <summary>
        /// Получить контакты действующих руководителей для контрагентов
        /// </summary>
        /// <param name="contragentIds">Идентификаторы контрагентов</param>
        /// <returns>Словарь контактов действующих руководителей</returns>
        public IDictionary<long, ContragentContact> GetActualManagerContacts(params long[] contragentIds)
        {
            var result = this.ContragentContactDomainService.GetAll()
                .Where(x => contragentIds.Contains(x.Contragent.Id))
                .Where(x => x.DateStartWork.HasValue)
                .Where(x => x.DateStartWork.Value <= DateTime.Now)
                .Where(x => !x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)
                .Where(x => x.Position.Code == ContragentService.ManagerPositionCode)
                .GroupBy(x => x.Contragent.Id)
                .ToDictionary(x => x.Key, x => x.First());

            return result;
        }

        private void GetExceptedContragents(List<long> list, IDomainService<Contragent> service, long parentId)
        {
            var children = service.GetAll().Where(x => x.Parent.Id == parentId).Select(x => x.Id).ToList();

            foreach (var id in children)
            {
                list.Add(id);
                this.GetExceptedContragents(list, service, id);
            }
        }
    }
}