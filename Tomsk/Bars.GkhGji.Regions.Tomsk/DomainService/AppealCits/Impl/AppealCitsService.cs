namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Regions.Tomsk.Entities;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tomsk.Entities.AppealCits;

    /// <summary>
    /// Сервис для Обращение граждан
    /// </summary>
    public class AppealCitsService : Bars.GkhGji.DomainService.AppealCitsService<ViewAppealCitizens>
    {
        /// <summary>
        /// Добавить пользовательские фильтры
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <returns>Модифицированный запрос</returns>
        public override IQueryable<ViewAppealCitizens> AddUserFilters(IQueryable<ViewAppealCitizens> query)
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var serviceAppealCitsExecutant = Container.Resolve<IDomainService<AppealCitsExecutant>>();

            try
            {
                var municipalityList = userManager.GetMunicipalityIds();
                var inspectorsList = userManager.GetInspectorIds();

                return query.WhereIf(
                    municipalityList.Count > 0,
                    x => municipalityList.Contains((long)x.MunicipalityId) || !x.MunicipalityId.HasValue)
                    .WhereIf(inspectorsList.Count > 0,
                        x => serviceAppealCitsExecutant.GetAll()
                                                            .Any(y => x.AppealCits.Id == y.AppealCits.Id &&
                                                            (inspectorsList.Contains(y.Author.Id) ||
                                                             inspectorsList.Contains(y.Executant.Id) ||
                                                             inspectorsList.Contains(y.Controller.Id))));
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(serviceAppealCitsExecutant);
            }
        }

        /// <summary>
        /// Добавить пользовательские фильтры
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="loadParams">Параметры загрузки</param>
        /// <returns>Модифицированный запрос</returns>
        protected override IQueryable<ViewAppealCitizens> ApplyAddressFilter(IQueryable<ViewAppealCitizens> query, LoadParam loadParams)
        {
            //фильтрация будет идти через вьюху, а не через таблицу-связку
            return query;
        }

        /// <summary>
        /// Добавить фильтры источника обращения
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="loadParams">Параметры загрузки</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IQueryable<ViewAppealCitizens> ApplyAppealSourceFilter(IQueryable<ViewAppealCitizens> query, LoadParam loadParams)
        {
            if (this.CheckFilters(loadParams))
            {
                return query;
            }

            var revenueSourceNumbersFilter =
                loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "RevenueSourceNumbers");
            var revenueSourceDatesFilter =
                loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "RevenueSourceDates") != null
                    ? DateTime.Parse(
                        loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "RevenueSourceDates").Value)
                    : DateTime.MinValue;
            var filters = new[] { "RevenueSourceNumbers", "RevenueSourceDates" };

            IQueryable<long> appealIdsFromSourceFilter = null;
            if (revenueSourceNumbersFilter != null && revenueSourceNumbersFilter.Value.IsNotEmpty() ||
                revenueSourceDatesFilter != DateTime.MinValue)
            {
                var appealCitsSourceService = this.Container.Resolve<IDomainService<AppealCitsSource>>();

                appealIdsFromSourceFilter = appealCitsSourceService.GetAll()
                    .WhereIf(revenueSourceNumbersFilter != null && revenueSourceNumbersFilter.Value.IsNotEmpty(),
                        s => s.RevenueSourceNumber.Contains(revenueSourceNumbersFilter.Value))
                    .WhereIf(revenueSourceDatesFilter != DateTime.MinValue,
                        s => s.RevenueDate >= revenueSourceDatesFilter &&
                             s.RevenueDate < revenueSourceDatesFilter.AddDays(1))
                    .Select(s => s.AppealCits.Id)
                    .Distinct();

                loadParams.DataFilter.Filters = loadParams.DataFilter.Filters.Where(f => !filters.Contains(f.DataIndex));
            }
            return query.WhereIf(appealIdsFromSourceFilter != null, x => appealIdsFromSourceFilter.Contains(x.Id));
        }

        /// <summary>
        /// Получить список из вью
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <param name="totalCount">Общее количество</param>
        /// <param name="usePaging">Использовать ли пагинацию</param>
        /// <returns>Результат выполнения запроса</returns>
        public override IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging)
        {
            var serviceAppealCitizensGji = Container.Resolve<IDomainService<ViewAppealCitizens>>();
            var serviceTomskOperator = Container.Resolve<IDomainService<TomskOperator>>();
            var userManager = Container.Resolve<IGkhUserManager>();
            var appealCitsSourceService = Container.ResolveDomain<AppealCitsSource>();
            var serviceAppealCitsExecutant = Container.Resolve<IDomainService<AppealCitsExecutant>>();

            try
            {
                var loadParams = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
                var ids = baseParams.Params.ContainsKey("Id") ? baseParams.Params["Id"].ToStr() : string.Empty;

                var excludeIds = baseParams.Params.GetAs<string>("excludeIds");

                var excludeIdsList = new List<long>();
                if (!string.IsNullOrEmpty(excludeIds))
                {
                    excludeIdsList = excludeIds.Split(',').Select(id => id.ToLong()).ToList();
                }

                var listIds = new List<long>();
                if (!string.IsNullOrEmpty(ids))
                {
                    listIds = ids.Split(',').Select(id => id.ToLong()).ToList();
                }

                var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
                var authorId = baseParams.Params.GetAs<long>("authorId");
                var executantId = baseParams.Params.GetAs<long>("executantId");
                var controllerId = baseParams.Params.GetAs<long>("controllerId");

                var dateFromStart = baseParams.Params.GetAs("dateFromStart", DateTime.MinValue);
                var dateFromEnd = baseParams.Params.GetAs("dateFromEnd", DateTime.MinValue);

                var checkTimeStart = baseParams.Params.GetAs("checkTimeStart", DateTime.MinValue);
                var checkTimeEnd = baseParams.Params.GetAs("checkTimeEnd", DateTime.MinValue);
                var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", false);

                if (dateFromStart != DateTime.MinValue)
                {
                    dateFromStart = dateFromStart.AddDays(-1);
                }

                if (checkTimeStart != DateTime.MinValue)
                {
                    checkTimeStart = checkTimeStart.AddDays(-1);
                }

                if (dateFromEnd != DateTime.MinValue)
                {
                    dateFromEnd = dateFromEnd.AddDays(1);
                }

                if (checkTimeEnd != DateTime.MinValue)
                {
                    checkTimeEnd = checkTimeEnd.AddDays(1);
                }

                var municipalityList = userManager.GetMunicipalityIds();
                var loadParam = baseParams.GetLoadParam();

                string filterForNumberGji = null;
                string filterForCorrespondenti = null;
                string filterForExecutantNames = null;

                var complexFilter = loadParam.FindInComplexFilter("NumberGji");

                if (complexFilter != null)
                {
                    filterForNumberGji = complexFilter.Value.ToStr();
                    loadParam.SetComplexFilterNull("NumberGji");
                }

                complexFilter = loadParam.FindInComplexFilter("Correspondent");
                if (complexFilter != null)
                {
                    filterForCorrespondenti = complexFilter.Value.ToStr();
                    loadParam.SetComplexFilterNull("Correspondent");
                }

                complexFilter = loadParam.FindInComplexFilter("ExecutantNames");
                if (complexFilter != null)
                {
                    filterForExecutantNames = complexFilter.Value.ToStr();
                    loadParam.SetComplexFilterNull("ExecutantNames");
                }

                /*
                 * Если в операторе указаны инспектора то фильтруем по инспекторам
                 * Но если указан Признак Пкоазывать неназначенные обращения то тогда над опоказывать x.Executant = null
                 * Соответсвенн оесли и указаны инспектораи признак то над оучитыват ьвсе условия
                 */

                var query = AddUserFilters(ApplyFilters(serviceAppealCitizensGji.GetAll(), baseParams))
                    .WhereIf(excludeIdsList.Count > 0, x => !excludeIdsList.Contains(x.Id))
                    .WhereIf(
                        municipalityList.Count > 0,
                        x => municipalityList.Contains((long) x.MunicipalityId) || !x.MunicipalityId.HasValue)
                    .WhereIf(appealCitizensId > 0, x => x.Id != appealCitizensId)
                    .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id))
                    .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains(string.Format("/{0}/", realityObjectId)))
                    .WhereIf(dateFromStart != DateTime.MinValue, x => x.DateFrom > dateFromStart)
                    .WhereIf(dateFromEnd != DateTime.MinValue, x => x.DateFrom < dateFromEnd)
                    .WhereIf(checkTimeStart != DateTime.MinValue, x => x.CheckTime > checkTimeStart)
                    .WhereIf(checkTimeEnd != DateTime.MinValue, x => x.CheckTime < checkTimeEnd)
                    .WhereIf(!showCloseAppeals, x => x.State == null || !x.State.FinalState)
                    .WhereIf(authorId > 0, x => serviceAppealCitsExecutant.GetAll().Any(y => y.AppealCits.Id == x.Id && y.Author.Id == authorId))
                    .WhereIf(executantId > 0, x => serviceAppealCitsExecutant.GetAll().Any(y => y.AppealCits.Id == x.Id && y.Executant.Id == executantId))
                    .WhereIf(
                        controllerId > 0,
                        x => serviceAppealCitsExecutant.GetAll().Any(y => y.AppealCits.Id == x.Id && y.Controller.Id == controllerId))
                    .WhereIf(
                        !filterForNumberGji.IsEmpty(),
                        x => x.NumberGji != null
                            && (x.NumberGji.ToUpper().Contains(filterForNumberGji.ToUpper())
                                || x.NumberGji.Replace(" ", "")
                                    .ToUpper()
                                    .Contains(filterForNumberGji.Replace(" ", "").ToUpper())))
                    .WhereIf(
                        !filterForCorrespondenti.IsEmpty(),
                        x => x.Correspondent != null
                            && (x.Correspondent.ToUpper().Contains(filterForCorrespondenti.ToUpper())
                                || x.Correspondent.Replace(" ", "")
                                    .ToUpper()
                                    .Contains(filterForCorrespondenti.Replace(" ", "").ToUpper())))
                    .WhereIf(
                        !filterForExecutantNames.IsEmpty(),
                        x => x.ExecutantNames != null
                            && (x.ExecutantNames.ToUpper().Contains(filterForExecutantNames.ToUpper())
                                || x.ExecutantNames.Replace(" ", "")
                                    .ToUpper()
                                    .Contains(filterForExecutantNames.Replace(" ", "").ToUpper())))
                    .Select(
                        x =>
                            new
                            {
                                x.Id,
                                x.State,
                                Name = string.Format("{0} ({1})", x.Number, x.NumberGji), // Для отображения в строке масового выбора
                                ManagingOrganization = x.ContragentName,
                                x.Number,
                                x.NumberGji,
                                x.DateFrom,
                                x.CheckTime,
                                x.QuestionsCount,
                                x.Municipality,
                                x.CountRealtyObj,
                                x.ExecutantNames,
                                Tester = x.Tester.Fio,
                                SuretyResolve = x.SuretyResolve.Name,
                                x.ExecuteDate,
                                x.SuretyDate,
                                x.ZonalInspection,
                                ZoneName = x.ZonalInspection != null ? x.ZonalInspection.ZoneName : "",
                                x.Correspondent,
                                x.CorrespondentAddress,
                                x.RealObjAddresses,
                                x.AppealCits.SpecialControl,
                                x.AppealCits.DescriptionLocationProblem,
                                x.RevenueSourceNames,
                                x.RevenueSourceNumbers,
                                x.RevenueSourceDates
                            })
                    .Filter(loadParams, this.Container);

                totalCount = query.Count();

                query = query
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .Order(loadParams);

                if (usePaging)
                {
                    query = query.Paging(loadParams);
                }

                return query.ToList();
            }
            finally
            {
                Container.Release(serviceAppealCitizensGji);
                Container.Release(userManager);
                Container.Release(serviceTomskOperator);
                Container.Release(appealCitsSourceService);
                Container.Release(serviceAppealCitsExecutant);
            }
        }
    }
}