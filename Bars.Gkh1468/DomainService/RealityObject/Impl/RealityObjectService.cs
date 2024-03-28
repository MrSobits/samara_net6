namespace Bars.Gkh1468.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Modules.Security;
    using B4.Utils;

    using Bars.Gkh.Modules.Gkh1468.Entities;

    using Castle.Windsor;

    using Entities;

    using Gkh.Authentification;
    using Gkh.Domain;
    using Gkh.Entities;
    using Gkh.Enums;
    using Gkh.Utils;

    /// <summary>
    /// Интерфейс сервиса работы с домами
    /// </summary>
    public class RealityObjectService : IRealityObjectService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ManOrgContractRealityObject"/>
        /// </summary>
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRo { get; set; }

        /// <summary>
        /// Интерфейс идентификатора пользователя
        /// </summary>
        public IUserIdentity UserIdentity { get; set; }

        /// <summary>
        /// Интерфейс описывающий сервис авторизации
        /// </summary>
        public IAuthorizationService AuthorizationService { get; set; }

        /// <summary>
        /// Репозиторий <see cref="RealityObject"/>
        /// </summary>
        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        /// <inheritdoc />
        public IDataResult ListRealityObjectInfo()
        {
            var service = this.Container.Resolve<IDomainService<RealityObject>>();

            var userId = this.Container.Resolve<IUserIdentity>().UserId;

            var oper = this.Container.Resolve<IDomainService<Operator>>().FirstOrDefault(x => x.User.Id == userId);

            var passService = this.Container.Resolve<IHousePassportService>();

            var d = DateTime.Now;
            var percents = passService.GetPassportsPercentageByHouse(d.Year, d.Month);

            // TODO получать информацию по паспортам (% заполненности по провайдерам информации)
            var data =
                service.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.FiasAddress.AddressName,
                            UserType = oper != null ? (int) oper.ContragentType : -1
                        })
                    .AsEnumerable()
                    .Select(
                        x => new
                        {
                            x.AddressName,
                            Percent = percents.ContainsKey(x.Id) ? percents[x.Id] : 0M,
                            x.UserType
                        })
                    .ToArray();

            return new ListDataResult(data, data.Count());
        }

        /// <inheritdoc />
        public IDataResult ListView(BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var userRoleDomain = this.Container.ResolveDomain<UserRole>();
            var roResOrgDomain = this.Container.ResolveDomain<RealityObjectResOrg>();
            var roPublServOrgDomain = this.Container.ResolveDomain<PublicServiceOrgContract>();

            try
            {
                var currOperator = userManager.GetActiveOperator();
                var loadParam = baseParams.GetLoadParam();

                // если нет параметра, то ставим true, т.к. запрос списка домов может быть может быть не из реестра
                // Если true показываем снесенные дома
                var filterShowDemolished = baseParams.Params.GetAs("filterShowDemolished", false);

                // Если true показываем индивидуальные дома
                var filterShowIndividual = baseParams.Params.GetAs("filterShowIndividual", false);

                // Если true показываем дома блокированной застройки
                var filterShowBlockedBuilding = baseParams.Params.GetAs("filterShowBlockedBuilding", false);

                // Данный костыль добавляю потому как Пользователи при фильтрации иногда забывают (или нехотят) вбивать пробелы
                // Но при этом хотят чтобы их значения находились
                // соответсвенно я получаю Фильтр по Адресу и проверяю наличие данных как по Переданному фильтру ,так и с удалением пробелов
                var complexFilter = loadParam.FindInComplexFilter("Address");

                string filterForAddress = null;

                if (complexFilter != null)
                {
                    filterForAddress = complexFilter.Value.ToStr();
                    loadParam.SetComplexFilterNull("Address");
                }

                var roleName = currOperator != null
                    ? userRoleDomain.GetAll()
                        .Where(x => x.User.Id == currOperator.User.Id)
                        .Select(x => x.Role.Name)
                        .First()
                    : string.Empty;

                var filterCondition = currOperator != null && roleName == "УО (МО)" && currOperator.Contragent != null;

                var data = this.Container.ResolveDomain<RealityObject>().GetAll()
                    .WhereIf(!filterShowBlockedBuilding, x => x.TypeHouse != TypeHouse.BlockedBuilding)
                    .WhereIf(!filterShowIndividual, x => x.TypeHouse != TypeHouse.Individual)
                    .WhereIf(!filterShowDemolished, x => x.ConditionHouse != ConditionHouse.Razed)
                    .WhereIf(
                        !filterForAddress.IsEmpty(),
                        x => x.Address != null
                            && (x.Address.ToUpper().Contains(filterForAddress.ToUpper())
                                || x.Address.Replace(" ", string.Empty).ToUpper()
                                    .Contains(filterForAddress.Replace(" ", string.Empty).ToUpper())))

                    .WhereIf(
                        filterCondition && currOperator.ContragentType == ContragentType.Pku,
                        y => roResOrgDomain.GetAll()
                            .Where(x => x.ResourceOrg.Contragent.Id == currOperator.Contragent.Id)
                            .Any(x => x.RealityObject.Id == y.Id))

                    .WhereIf(
                        filterCondition && currOperator.ContragentType == ContragentType.Pr,
                        y => roPublServOrgDomain.GetAll()
                            .Where(x => x.PublicServiceOrg.Contragent.Id == currOperator.Contragent.Id)
                            .Any(x => x.RealityObject.Id == y.Id))
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.ExternalId,
                            Municipality = x.Municipality.Name,
                            Settlement = x.MoSettlement.Name,
                            x.Address,
                            FullAddress = x.FiasAddress.AddressName,
                            x.TypeHouse,
                            x.ConditionHouse,
                            x.DateDemolition,
                            x.Floors,
                            x.NumberEntrances,
                            x.NumberLiving,
                            x.NumberApartments,
                            x.AreaMkd,
                            x.AreaLiving,
                            x.PhysicalWear,
                            x.NumberLifts,
                            x.HeatingSystem,
                            WallMaterialName = x.WallMaterial.Name,
                            RoofingMaterialName = x.RoofingMaterial.Name,
                            x.TypeRoof,
                            x.DateLastOverhaul,
                            x.DateCommissioning,
                            ManOrgNames = x.ManOrgs,
                            TypeContracts = x.TypesContract,
                            x.CodeErc,
                            x.IsInsuredObject,
                            x.GkhCode,
                            x.IsBuildSocialMortgage,
                            x.State,
                            IsRepairInadvisable = (bool?) x.IsRepairInadvisable,
                            IsInvolvedCr = !x.IsNotInvolvedCr
                        })
                    .Filter(loadParam, this.Container)
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.Address);

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(userRoleDomain);
                this.Container.Release(roResOrgDomain);
                this.Container.Release(roPublServOrgDomain);
            }
        }

        /// <inheritdoc />
        public IDataResult ListForPassport(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var selectedPeriod = baseParams.Params.GetAs<DateTime>("selectedPeriod");

            var periodStart = selectedPeriod;
            var periodEnd = selectedPeriod.AddMonths(1).AddDays(-1);

            var userManager = this.Container.Resolve<IGkhUserManager>();
            var currOperator = userManager.GetActiveOperator();
            var realObjResOrgService = this.Container.ResolveDomain<RealityObjectResOrg>();
            var realObjPublServOrgService = this.Container.ResolveDomain<PublicServiceOrgContract>();
            var realityObjectServiceOrgService = this.Container.ResolveDomain<ServiceOrgRealityObjectContract>();
            var createByAllRo = this.AuthorizationService.Grant(this.UserIdentity, "Gkh1468.Passport.MyHouse.CreateByAllRo");

            try
            {
                if (createByAllRo)
                {
                    var data1 = this.RealityObjectRepository.GetAll()
                        .Select(x => new {x.Id, Municipality = x.Municipality.Name, x.Address})
                        .Filter(loadParams, this.Container);

                    var totalCount1 = data1.Count();

                    return new ListDataResult(data1.Order(loadParams).Paging(loadParams).ToList(), totalCount1);
                }
                else
                {
                    if (currOperator == null)
                    {
                        return new ListDataResult(new List<string>(), 0);
                    }

                    var contragentId = currOperator.Contragent != null ? (long?) currOperator.Contragent.Id : null;

                    /*запрос идентификаторов домов, имеющих действующий договор за указанный месяц с УО текущего оператора */
                    var activeManOrgContractRoIdsQuery = this.ManOrgContractRo.GetAll()
                        .Where(y => y.ManOrgContract.StartDate <= periodEnd)
                        .Where(y => y.ManOrgContract.EndDate == null || y.ManOrgContract.EndDate >= periodStart)
                        .WhereIf(contragentId != null, x => x.ManOrgContract.ManagingOrganization.Contragent.Id == contragentId)
                        .Select(x => x.RealityObject.Id);

                    /*запрос идентификаторов домов, имеющих действующий договор за указанный месяц с поставщиком коммунальных услуг текущего оператора */
                    var activeResOrgContractRoIdsQuery = realObjResOrgService.GetAll()
                        .Where(y => y.DateEnd == null || y.DateEnd >= periodStart)
                        .Where(y => y.DateStart == null || y.DateStart <= periodEnd)
                        .WhereIf(contragentId != null, x => x.ResourceOrg.Contragent.Id == contragentId)
                        .Select(x => x.RealityObject.Id);

                    /*запрос идентификаторов домов, имеющих действующий договор за указанный месяц с поставщиком ресурсов текущего оператора */
                    var activePublServOrgContractRoIdsQuery = realObjPublServOrgService.GetAll()
                        .Where(y => y.DateStart == null || y.DateStart <= periodEnd)
                        .Where(y => y.DateEnd == null || y.DateEnd >= periodStart)
                        .WhereIf(contragentId != null, x => x.PublicServiceOrg.Contragent.Id == contragentId)
                        .Select(x => x.RealityObject.Id);

                    /*запрос идентификаторов домов, имеющих действующий договор за указанный месяц с поставщиком жилищных услуг текущего оператора */
                    var serviceOrgContactRoIdsQuery = realityObjectServiceOrgService.GetAll()
                        .Where(y => y.ServOrgContract.DateStart == null || y.ServOrgContract.DateStart <= periodEnd)
                        .Where(y => y.ServOrgContract.DateEnd == null || y.ServOrgContract.DateEnd >= periodStart)
                        .WhereIf(contragentId != null, x => x.ServOrgContract.ServOrg.Contragent.Id == contragentId)
                        .Select(x => x.RealityObject.Id);

                    var data = this.RealityObjectRepository.GetAll()
                        .WhereIf(
                            contragentId != null,
                            y => activeResOrgContractRoIdsQuery.Contains(y.Id)
                                || activePublServOrgContractRoIdsQuery.Contains(y.Id)
                                || serviceOrgContactRoIdsQuery.Contains(y.Id)
                                || activeManOrgContractRoIdsQuery.Contains(y.Id))
                        .Select(x => new {x.Id, Municipality = x.Municipality.Name, x.Address})
                        .Filter(loadParams, this.Container);

                    var totalCount = data.Count();

                    return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                }
            }
            finally
            {
                this.Container.Release(userManager);
                this.Container.Release(realObjResOrgService);
                this.Container.Release(realObjPublServOrgService);
                this.Container.Release(realityObjectServiceOrgService);
            }
        }

        /// <inheritdoc />
        public IDataResult ListByPublicServOrg(BaseParams baseParams)
        {
            var domain = this.Container.ResolveDomain<RealityObject>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var data = domain.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Address,
                            Municipality = x.Municipality.Name,
                            x.ManOrgs
                        })
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(domain);
            }
        }

        /// <inheritdoc />
        public IDataResult ListRoByPublicServOrg(BaseParams baseParams)
        {
            var publicServOrgDomain = this.Container.ResolveDomain<PublicServiceOrgMunicipality>();
            var roDomain = this.Container.ResolveRepository<RealityObject>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var publicSupplyResOrgId = baseParams.Params.GetAsId("publicSupplyResOrgId");

                var data = roDomain.GetAll()
                    .Where(
                        x => publicServOrgDomain.GetAll().Any(y => y.PublicServiceOrg.Id == publicSupplyResOrgId && y.Municipality.Id == x.Municipality.Id))
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.Address,
                            Municipality = x.Municipality.Name
                        })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(publicServOrgDomain);
                this.Container.Release(roDomain);
            }
        }

        /// <inheritdoc />
        public IDataResult ListByPublicServOrgContract(BaseParams baseParams)
        {
            var domain = this.Container.ResolveDomain<PublicServiceOrgContractRealObj>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var contractId = baseParams.Params.GetAs<long>("contractId");

                var data = domain.GetAll()
                    .Where(x => x.RsoContract.Id == contractId)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.RealityObject.Address,
                            Municipality = x.RealityObject.Municipality.Name,
                            x.RealityObject.ManOrgs
                        })
                    .Filter(loadParams, this.Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(domain);
            }
        }
    }
}