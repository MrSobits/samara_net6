namespace Bars.GkhDi.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Config;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.ConfigSections;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.PercentCalculationProvider;
    using Bars.GkhDi.Tasks;

    using Castle.Windsor;

    /// <summary>
    /// Сервис по расчёту процентов
    /// </summary>
    public class DisclosureInfoService : IDisclosureInfoService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <inheritdoc />
        public IDataResult GetDisclosureOfManOrg(BaseParams baseParams)
        {
            var manorgId = baseParams.Params.GetAs<long>("manorgId");

            var exist = this.Container.Resolve<IDomainService<DisclosureInfo>>()
                .GetAll()
                .Any(x => x.ManagingOrganization.Id == manorgId);

            return new BaseDataResult(new { exist }) { Success = true };
        }

        /// <inheritdoc />
        public DisclosureInfoRealityObj GetDiRoByDi(long disclosureInfoId, long realityObjId, long manOrgId)
        {
            return this.Container.Resolve<IDomainService<DisclosureInfoRelation>>()
                .GetAll()
                .Where(x => x.DisclosureInfo.Id == disclosureInfoId && x.DisclosureInfoRealityObj.RealityObject.Id == realityObjId &&
                    x.DisclosureInfoRealityObj.ManagingOrganization.Id == manOrgId)
                .Select(x => x.DisclosureInfoRealityObj)
                .FirstOrDefault();
        }

        /// <inheritdoc />
        public DisclosureInfoRealityObj GetDiRoProxyByDi(long disclosureInfoId, long realityObjId)
        {
            var diDomain = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();
            using (this.Container.Using(diDomain))
            {
                var id = diDomain.GetAll()
                    .Where(x => x.DisclosureInfo.Id == disclosureInfoId)
                    .Where(x => x.DisclosureInfoRealityObj.RealityObject.Id == realityObjId)
                    .Select(x => x.DisclosureInfoRealityObj.Id)
                    .FirstOrDefault();

                return new DisclosureInfoRealityObj() { Id = id };
            }
        }

        /// <inheritdoc />
        public IDataResult GetDisclosureInfo(BaseParams baseParams)
        {
            try
            {
                var managingOrgId = baseParams.Params.GetAs<long>("managingOrgId");
                var periodId = baseParams.Params.GetAs<long>("periodId");

                var service = Container.Resolve<IDomainService<DisclosureInfo>>();

                var disclosureInfo = service.GetAll()
                    .FirstOrDefault(x => x.ManagingOrganization.Id == managingOrgId && x.PeriodDi.Id == periodId);

                if (disclosureInfo == null)
                {
                    return new BaseDataResult(true);
                }

                var percentDate =
                    this.Container.Resolve<IDomainService<DisclosureInfoPercent>>()
                        .GetAll()
                        .Where(
                            x =>
                            (x.Code == "DisclosureInfoPercentProvider" || x.Code == "ManOrgInfoPercent"
                             || x.Code == "RealObjsPercent") && x.DisclosureInfo.Id == disclosureInfo.Id)
                        .OrderByDescending(x => x.CalcDate ?? x.ObjectEditDate)
                        .GroupBy(x => x.Code)
                        .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                DisclosureInfoPercent disclosureInfoPercent = null;
                if (percentDate.ContainsKey("DisclosureInfoPercentProvider"))
                {
                    disclosureInfoPercent = percentDate["DisclosureInfoPercentProvider"];
                }

                decimal? diPerc = null, manOrgInfoPerc = null, realObjsPerc = null;
                if (disclosureInfoPercent != null)
                {
                    diPerc = disclosureInfoPercent.Percent;

                    if (percentDate.ContainsKey("ManOrgInfoPercent"))
                    {
                        var manOrgInfoPercentItem = percentDate["ManOrgInfoPercent"];
                        manOrgInfoPerc = manOrgInfoPercentItem.Percent;
                    }

                    if (percentDate.ContainsKey("RealObjsPercent"))
                    {
                        var realObjsPercentItem = percentDate["RealObjsPercent"];
                        realObjsPerc = realObjsPercentItem.Percent;
                    }
                }

                return new BaseDataResult(new { recDi = disclosureInfo, diperc = diPerc, manorginfoperc = manOrgInfoPerc, realobjsperc = realObjsPerc })
                    {
                        Success = true
                    };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        /// <inheritdoc />
        public IDataResult GetOperatorManOrg(BaseParams baseParams)
        {
            try
            {
                var service = this.Container.Resolve<IDomainService<OperatorContragent>>();

                var userManager = Container.Resolve<IGkhUserManager>();
                var oper = userManager.GetActiveOperator();

                if (oper != null)
                {

                    var contragentList = service.GetAll()
                        .Where(x => x.Operator.Id == oper.Id)
                        .Select(x => x.Contragent.Id);

                    var manOrgList = new List<ManagingOrganization>();
                    if (contragentList.Count() == 1)
                    {
                        var contragent_id = contragentList.FirstOrDefault();

                        manOrgList = this.Container.Resolve<IDomainService<ManagingOrganization>>()
                                .GetAll()
                                .Where(x => x.Contragent.Id == contragent_id)
                                .Select(x => new ManagingOrganization { Id = x.Id, ContragentName = x.Contragent.Name, TypeManagement = x.TypeManagement })
                                .ToList();
                    }

                    var manOrgId = manOrgList.Count == 1 ? manOrgList[0].Id : 0;
                    var manOrgName = manOrgList.Count == 1 ? manOrgList[0].ContragentName : string.Empty;
                    var typeManagement = manOrgList.Count == 1 ? manOrgList[0].TypeManagement : 0;
                    var manOrg = manOrgId > 0 ? new { Id = manOrgId, ContragentName = manOrgName, TypeManagement = typeManagement } : null;

                    return new BaseDataResult(new { count = manOrgList.Count, manOrg })
                    {
                        Success = true
                    };
                }

                return new BaseDataResult(new { count = 0 })
                {
                    Success = true
                };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        /// <inheritdoc />
        public IDataResult GetDateStartByPeriod(BaseParams baseParams)
        {
            try
            {
                var periodDiId = baseParams.Params.GetAs<long>("periodDiId");
                var periodDi = this.Container.Resolve<IDomainService<PeriodDi>>().Load(periodDiId);

                return new BaseDataResult(new { periodDi.DateStart, periodDi.DateEnd })
                {
                    Success = true
                };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        /// <inheritdoc />
        public IDataResult GetTypeManagingByDisinfo(BaseParams baseParams)
        {
            try
            {
                var obj = this.Container.Resolve<IDomainService<DisclosureInfo>>().Get(baseParams.Params["disclosureInfoId"].To<long>());

                if (obj.ManagingOrganization != null)
                {
                    return new BaseDataResult { Success = true, Data = obj.ManagingOrganization.TypeManagement };
                }

                return new BaseDataResult { Success = false };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        /// <summary>
        /// Получаем ФИО позиции по коду
        /// </summary>
        /// <param name="contragentId"> контрагент ид</param>
        /// <param name="periodDi"> период раскрытия</param>
        /// <param name="listCodes"> код позиции</param>
        /// <returns></returns>
        public string GetPositionByCode(long contragentId, PeriodDi periodDi, List<string> listCodes)
        {
            try
            {
                var service = this.Container.Resolve<IDomainService<ContragentContact>>();

                var data =
                    service.GetAll()
                           .Where(
                               x =>
                               x.Contragent.Id == contragentId && listCodes.Contains(x.Position.Code)
                               && ((x.DateStartWork.HasValue
                                    && (x.DateStartWork.Value >= periodDi.DateStart.Value
                                        && periodDi.DateEnd.Value >= x.DateStartWork.Value) || !x.DateStartWork.HasValue)
                                   || (periodDi.DateStart.Value >= x.DateStartWork.Value
                                       && ((x.DateEndWork.HasValue && x.DateEndWork.Value >= periodDi.DateStart.Value)
                                           || !x.DateEndWork.HasValue))))
                           .Select(x => x.FullName);

                var result = data.Any()
                                 ? (data.Count() == 1
                                        ? data.FirstOrDefault()
                                        : data.Aggregate((current, next) => current + ", " + next))
                                 : string.Empty;

                return result;
            }
            catch (System.NotSupportedException)
            {
                return string.Empty;
            }
        }

        /// <inheritdoc />
        public IDataResult PercentCalculation(BaseParams baseParams)
        {
            var disclosureInfoService = this.Container.Resolve<IDomainService<DisclosureInfo>>();
            var configProvider = this.Container.Resolve<IGkhConfigProvider>();
            var userManagrer = this.Container.Resolve<IGkhUserManager>();

            using (this.Container.Using(disclosureInfoService))
            {
                var currentUser = userManagrer.GetActiveUser();
                var isAdmin = currentUser.Roles.Any(x => x.Role.Name == "Администратор");
                var generalConfigs = configProvider.Get<DiConfig>();

                if (generalConfigs.PercentCalculation.DiCalcOnExecutor)
                {
                    return this.TaskManager.CreateTasks(new PercentCalculationTaskProvider(this.Container), baseParams);
                }

                if (!isAdmin)
                {
                    var allowedCount = generalConfigs.PercentCalculation.IsCalculationCount;

                    var count = disclosureInfoService.GetAll().Count(x => x.IsCalculation);
                    if (count >= allowedCount)
                    {
                        return new BaseDataResult(false, "В настоящий момент сервер загружен. Попробуйте сделать расчет позже.");
                    }
                }

                return this.StartPercentCalculation(baseParams);
            }
        }

        /// <inheritdoc />
        public IDataResult StartPercentCalculation(BaseParams baseParams)
        {
            var disclosureInfoService = this.Container.Resolve<IDomainService<DisclosureInfo>>();
            using (this.Container.Using(disclosureInfoService))
            {
                var municipalityIdList = baseParams.Params.GetAs("municipalityIds", string.Empty);

                var municipalityIds = !string.IsNullOrEmpty(municipalityIdList)
                    ? municipalityIdList.Split(',').Select(id => id.ToLong()).ToArray()
                    : new long[0];

                var periodDiId = baseParams.Params.GetAs<long>("period");

                var disclosureInfoId = baseParams.Params.GetAs<long>("disclosureInfoId");

                var period = this.Container.Resolve<IDomainService<PeriodDi>>().Get(periodDiId);

                if (period != null && municipalityIds.Length != 0)
                {
                    IPercentCalculation[] percentCalculations = null;

                    try
                    {
                        percentCalculations = this.Container.ResolveAll<IPercentCalculation>();

                        this.ActualizeEmptyLog(period, municipalityIds);

                        return percentCalculations.First(x => x.CheckByPeriod(period)).MassCalculate(period, municipalityIds);
                    }
                    finally
                    {
                        if (percentCalculations.IsNotEmpty())
                        {
                            percentCalculations.ForEach(this.Container.Release);
                        }
                    }
                }

                if (disclosureInfoId != 0)
                {
                    var disInfo = disclosureInfoService.Get(disclosureInfoId);

                    // если пришли с сервера расчётов, то проверка не нужна
                    if (disInfo.InCalculation)
                    {
                        return new BaseDataResult(false, "По данной управляющей компании в этом периоде уже идет расчет процентов!");
                    }

                    IPercentCalculation[] percentCalculations = null;
                    try
                    {
                        percentCalculations = this.Container.ResolveAll<IPercentCalculation>();
                        this.ActualizeEmptyLog(disInfo);

                        return percentCalculations.First(x => x.CheckByPeriod(disInfo.PeriodDi)).Calculate(disInfo);
                    }
                    finally
                    {
                        if (percentCalculations.IsNotEmpty())
                        {
                            percentCalculations.ForEach(this.Container.Release);
                        }
                    }
                }

                return new BaseDataResult();
            }
        }

        /// <summary>
        /// Удаляет из таблицы лога записи, по выбранным УО и периоду
        /// </summary>
        protected void ActualizeEmptyLog(DisclosureInfo disInfo)
        {
            var moEmptyLog = this.Container.Resolve<IDomainService<DisclosureInfoEmptyFields>>();
            var roEmptyLog = this.Container.Resolve<IDomainService<DisclosureInfoRealityObjEmptyFields>>();
            var relationsDomain = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();

            try
            {
                this.Container.InTransaction(() =>
                {
                    var idsToDelete = moEmptyLog.GetAll()
                        .Where(x => x.ManOrg.Id == disInfo.Id)
                        .Select(x => x.Id)
                        .ToArray();

                    foreach (var id in idsToDelete)
                    {
                        moEmptyLog.Delete(id);
                    }

                    var roEmptyLogIds = relationsDomain.GetAll()
                        .Where(x => x.DisclosureInfo.Id == disInfo.Id)
                        .Select(x => x.DisclosureInfoRealityObj.Id);

                    idsToDelete = roEmptyLog.GetAll()
                        .Where(x => roEmptyLogIds.Contains(x.RealityObj.Id))
                        .Select(x => x.Id)
                        .ToArray();

                    foreach (var id in idsToDelete)
                    {
                        roEmptyLog.Delete(id);
                    }

                });
            }
            finally
            {
                this.Container.Release(moEmptyLog);
                this.Container.Release(roEmptyLog);
                this.Container.Release(relationsDomain);
            }
        }

        /// <summary>
        /// Удаляет из таблицы лога записи, по выбранным УО и периоду
        /// </summary>
        protected void ActualizeEmptyLog(PeriodDi periodDi, long[] muIds)
        {
            var moEmptyLog = this.Container.Resolve<IDomainService<DisclosureInfoEmptyFields>>();
            var roEmptyLog = this.Container.Resolve<IDomainService<DisclosureInfoRealityObjEmptyFields>>();
            var relationsDomain = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();
            var disInfoDomain = this.Container.Resolve<IDomainService<DisclosureInfo>>();

            var disInfoIds = disInfoDomain.GetAll()
                .Where(x => x.ManagingOrganization.Contragent.Municipality != null)
                .Where(x => x.PeriodDi.Id == periodDi.Id)
                .Where(x => muIds.Contains(x.ManagingOrganization.Contragent.Municipality.Id))
                .Select(x => x.Id);

            try
            {
                this.Container.InTransaction(() =>
                {
                    var idsToDelete = moEmptyLog.GetAll()
                        .Where(x => disInfoIds.Contains(x.ManOrg.Id))
                        .Select(x => x.Id)
                        .ToArray();

                    foreach (var id in idsToDelete)
                    {
                        moEmptyLog.Delete(id);
                    }

                    var roEmptyLogIds = relationsDomain.GetAll()
                        .Where(x => disInfoIds.Contains(x.DisclosureInfo.Id))
                        .Select(x => x.DisclosureInfoRealityObj.Id);

                    idsToDelete = roEmptyLog.GetAll()
                        .Where(x => roEmptyLogIds.Contains(x.RealityObj.Id))
                        .Select(x => x.Id)
                        .ToArray();

                    foreach (var id in idsToDelete)
                    {
                        roEmptyLog.Delete(id);
                    }

                });
            }
            finally
            {
                this.Container.Release(moEmptyLog);
                this.Container.Release(roEmptyLog);
                this.Container.Release(relationsDomain);
                this.Container.Release(disInfoDomain);
            }
        }
    }
}
