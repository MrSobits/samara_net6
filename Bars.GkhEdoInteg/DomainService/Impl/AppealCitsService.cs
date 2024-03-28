namespace Bars.GkhEdoInteg.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Utils;
    using Bars.GkhEdoInteg.Entities;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.ConfigSections.Appeal;
    using Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;
    using Bars.GkhGji.Regions.Tatarstan.Enums;
    using Bars.GkhGji.Utils;

    /// <summary>
	/// Сервис для работы с Обращения граждан
	/// </summary>
    public class AppealCitsService : GkhGji.DomainService.AppealCitsService
    {
        /// <summary>
        /// Вернуть список обращений
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <param name="totalCount">Количество записей (выходной параметр)</param>
        /// <param name="usePaging">Маркер использования постраничного вывода</param>
        /// <returns>Результирующий список</returns>
        public override IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceView = this.Container.Resolve<IDomainService<ViewAppealCitizensEdoInteg>>();
            var serviceAppealRo = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var appealCitsSourceService = this.Container.Resolve<IDomainService<AppealCitsSource>>();
            var statSubject = this.Container.Resolve<IDomainService<AppealCitsStatSubject>>();
            var disposalDomain = this.Container.Resolve<IDomainService<Disposal>>();
            var inspectionAppCittsDomain = this.Container.Resolve<IDomainService<InspectionAppealCits>>();
            var rapidResponseSystemAppealDomain = this.Container.ResolveDomain<RapidResponseSystemAppeal>();
            var rapidResponseSystemAppealDetailsDomain = this.Container.ResolveDomain<RapidResponseSystemAppealDetails>();

            using (this.Container.Using(serviceView, serviceAppealRo, appealCitsSourceService, statSubject,
                disposalDomain, inspectionAppCittsDomain, rapidResponseSystemAppealDomain, rapidResponseSystemAppealDetailsDomain))
            {
                var config = this.Container.GetGkhConfig<AppealConfig>();

                var revenueSourceNamesFilter = baseParams.GetValueFromComplexFilter("RevenueSourceNames") as string;
                var revenueSourceNumbersFilter = baseParams.GetValueFromComplexFilter("RevenueSourceNumbers") as string;
                var revenueSourceDatesFilter = baseParams.GetValueFromComplexFilter("RevenueSourceDates") as DateTime?;

                var loadParams = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);

                var ids = baseParams.Params.ContainsKey("Id") ? baseParams.Params["Id"].ToStr() : string.Empty;

                var listIds = new List<long>();
                if (!string.IsNullOrEmpty(ids))
                {
                    if (ids.Contains(','))
                    {
                        listIds.AddRange(ids.Split(',').Select(id => id.ToLong()).ToList());
                    }
                    else
                    {
                        listIds.Add(ids.ToLong());
                    }
                }

                var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

                var dateFromStart = baseParams.Params.GetAs<DateTime>("dateFromStart");
                var dateFromEnd = baseParams.Params.GetAs<DateTime>("dateFromEnd");

                var checkTimeStart = baseParams.Params.GetAs<DateTime>("checkTimeStart");
                var checkTimeEnd = baseParams.Params.GetAs<DateTime>("checkTimeEnd");

                var showSoprAppeals = baseParams.Params.GetAs("showSoprAppeals", false);
                var showProcessedAppeals = baseParams.Params.GetAs("showProcessedAppeals", false);
                var showNotProcessedAppeals = baseParams.Params.GetAs("showNotProcessedAppeals", false);
                var showInWorkAppeals = baseParams.Params.GetAs("showInWorkAppeals", false);
                var showClosedAppeals = baseParams.Params.GetAs("showClosedAppeals", true);

                var municipalityList = userManager.GetMunicipalityIds();
                var inspectorsList = userManager.GetInspectorIds();

                var appealCitsIds = new List<long>();

                if (realityObjectId > 0)
                {
                    appealCitsIds.AddRange(serviceAppealRo.GetAll()
                        .Where(x => x.RealityObject.Id == realityObjectId)
                        .Select(x => x.AppealCits.Id)
                        .ToArray());
                }

                IQueryable<long> appealIdsFromSourceFilter = null;
                if (revenueSourceDatesFilter.HasValue || revenueSourceNamesFilter.IsNotEmpty() || revenueSourceNumbersFilter.IsNotEmpty())
                {
                    // здесь реализуется фильтрация по источникам жалобы
                    appealIdsFromSourceFilter = appealCitsSourceService.GetAll()
                        .WhereIf(revenueSourceNamesFilter.IsNotEmpty(), s => s.RevenueSource.Name.Contains(revenueSourceNamesFilter))
                        .WhereIf(revenueSourceNumbersFilter.IsNotEmpty(), s => s.RevenueSourceNumber.Contains(revenueSourceNumbersFilter))
                        .WhereIf(
                            revenueSourceDatesFilter > DateTime.MinValue, 
                            s => s.RevenueDate >= revenueSourceDatesFilter && s.RevenueDate < revenueSourceDatesFilter.Value.AddDays(1))
                        .Select(s => s.AppealCits.Id)
                        .Distinct();
                }

                var soprAppealsQuery = rapidResponseSystemAppealDomain.GetAll()
                    .Join(rapidResponseSystemAppealDetailsDomain.GetAll(),
                        x => x.Id,
                        y => y.RapidResponseSystemAppeal.Id,
                        (x, y) => new { Appeal = x, AppealDetails = y });

                // Фильтрация по инспектору была изменена по задаче 33244
                var query = serviceView.GetAll()
                    .WhereIf(
                        municipalityList.Count > 0,
                        x => (x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value)) || !x.MunicipalityId.HasValue)
                    .WhereIf(
                        inspectorsList.Count > 0,
                        x => inspectorsList.Contains(x.Executant.Id) || inspectorsList.Contains(x.Tester.Id) || inspectorsList.Contains(x.AppealCits.Surety.Id))
                    .WhereIf(appealCitsIds.Count > 0, x => appealCitsIds.Contains(x.Id))
                    .WhereIf(appealCitizensId > 0, x => x.Id != appealCitizensId)
                    .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id))
                    .WhereIf(dateFromStart != DateTime.MinValue, x => x.DateFrom >= dateFromStart)
                    .WhereIf(dateFromEnd != DateTime.MinValue, x => x.DateFrom < dateFromEnd)
                    .WhereIf(checkTimeStart != DateTime.MinValue, x => x.CheckTime >= dateFromStart)
                    .WhereIf(checkTimeEnd != DateTime.MinValue, x => x.CheckTime < checkTimeEnd)
                    .WhereIf(!showClosedAppeals, x => x.State == null || !x.State.FinalState)
                    .WhereIf(appealIdsFromSourceFilter != null, x => appealIdsFromSourceFilter.Contains(x.Id))
                    .WhereIf(showSoprAppeals || showProcessedAppeals || showNotProcessedAppeals || showInWorkAppeals,
                        x => soprAppealsQuery.Any(y => y.Appeal.AppealCits.Id == x.Id &&
                            (showSoprAppeals || showProcessedAppeals && y.AppealDetails.State.Code == "3" || // Статус "Обработано"
                                showNotProcessedAppeals && y.AppealDetails.State.Code == "4" || // Статус "Не обработано"
                                showInWorkAppeals && !y.AppealDetails.State.StartState && !y.AppealDetails.State.FinalState)))
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        Name = $"{x.Number} ({x.NumberGji})",
                        // Для отображения в строке масового выбора
                        ManagingOrganization = x.Contragent.Name,
                        x.Contragent,
                        x.Number,
                        x.NumberGji,
                        x.DateFrom,
                        x.CheckTime,
                        x.QuestionsCount,
                        x.Municipality,
                        x.CountRealtyObj,
                        x.IsEdo,
                        Executant = x.Executant.Fio,
                        Tester = x.Tester.Fio,
                        SuretyResolve = x.SuretyResolve.Name,
                        x.ExecuteDate,
                        x.ZonalInspection,
                        x.RealObjAddresses,
                        x.Correspondent,
                        x.AddressEdo,
                        x.CountSubject,

                        //как бы все значения удовлетворяют фильтр
                        RevenueSourceNames = revenueSourceNamesFilter,
                        RevenueSourceNumbers = revenueSourceNumbersFilter,
                        RevenueSourceDates = revenueSourceDatesFilter
                    })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .Filter(loadParams, this.Container);

                if (usePaging)
                {
                    // Для скорости, если нет этих фильтров то считаем количество просто от таблицы, а не от вьюхи
                    // такое происходит при первом открытии реестра.
                    // проверка необходима, так как не делаем Select, через который мы получаем простой объект
                    if (loadParams.DataFilter == null
                        && loadParams.GetRuleValue("ManagingOrganization") == null
                        && loadParams.GetRuleValue("Municipality") == null
                        && loadParams.GetRuleValue("CountRealtyObj") == null
                        && loadParams.GetRuleValue("IsEdo") == null
                        && loadParams.GetRuleValue("RealObjAddresses") == null
                        && loadParams.GetRuleValue("AddressEdo") == null
                        && loadParams.GetRuleValue("CountSubject") == null
                        && loadParams.GetRuleValue("ManagingOrganization") == null
                        && loadParams.GetRuleValue("Executant") == null
                        && loadParams.GetRuleValue("SuretyResolve") == null
                        && loadParams.GetRuleValue("Tester") == null
                        && municipalityList.Count == 0)
                    {
                        var appealCitsDomainService = this.Container.ResolveDomain<AppealCits>();
                        using (this.Container.Using(appealCitsDomainService))
                        {
                            totalCount = appealCitsDomainService.GetAll()
                                .WhereIf(
                                    inspectorsList.Count > 0,
                                    x => inspectorsList.Contains(x.Executant.Id) || inspectorsList.Contains(x.Tester.Id) || inspectorsList.Contains(x.Surety.Id))
                                .WhereIf(appealCitsIds.Count > 0, x => appealCitsIds.Contains(x.Id))
                                .WhereIf(appealCitizensId > 0, x => x.Id != appealCitizensId)
                                .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id))
                                .WhereIf(dateFromStart != DateTime.MinValue, x => x.DateFrom >= dateFromStart)
                                .WhereIf(dateFromEnd != DateTime.MinValue, x => x.DateFrom < dateFromEnd)
                                .WhereIf(checkTimeStart != DateTime.MinValue, x => x.CheckTime >= dateFromStart)
                                .WhereIf(checkTimeEnd != DateTime.MinValue, x => x.CheckTime < checkTimeEnd)
                                .WhereIf(!showClosedAppeals, x => x.State == null || !x.State.FinalState)
                                .WhereIf(appealIdsFromSourceFilter != null, x => appealIdsFromSourceFilter.Contains(x.Id))
                                .WhereIf(showSoprAppeals || showProcessedAppeals || showNotProcessedAppeals || showInWorkAppeals,
                                    x => soprAppealsQuery.Any(y => y.Appeal.AppealCits.Id == x.Id &&
                                        (showSoprAppeals || showProcessedAppeals && y.AppealDetails.State.Code == "3" || // Статус "Обработано"
                                            showNotProcessedAppeals && y.AppealDetails.State.Code == "4" || // Статус "Не обработано"
                                            showInWorkAppeals && !y.AppealDetails.State.StartState && !y.AppealDetails.State.FinalState)))
                                .Filter(loadParams, this.Container)
                                .Count();
                        }
                    }
                    else
                    {
                        totalCount = query.Count();
                    }
                    
                    query = query.Order(loadParams).Paging(loadParams);
                }
                else
                {
                    query = query.Order(loadParams);
                    totalCount = query.Count();
                }

                var data = query.ToList();
                var appealIds = data.Select(x => x.Id);

                const string separator = ", ";

                var appealSourcesDict = appealCitsSourceService.GetAll()
                    .Where(s => appealIds.Contains(s.AppealCits.Id))
                    .Select(x => new
                    {
                        AppealCitsId = x.AppealCits.Id,
                        RevenueSourceName = x.RevenueSource.Name,
                        x.RevenueSourceNumber,
                        x.RevenueDate
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.AppealCitsId)
                    .ToDictionary(x => x.Key,
                        y => new
                        {
                            RevenueSourceNames = y.AggregateWithSeparator(x => x.RevenueSourceName, separator),
                            RevenueSourceNumbers = y.AggregateWithSeparator(s => s.RevenueSourceNumber, separator),
                            RevenueSourceDates = y.AggregateWithSeparator(s => s.RevenueDate.ToDateString(), separator)
                        });

                var subjectsDict = statSubject.GetAll()
                    .Where(s => appealIds.Contains(s.AppealCits.Id))
                    .Select(x => new
                    {
                        AppealCitsId = x.AppealCits.Id,
                        SubjectName = x.Subject.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.AppealCitsId)
                    .ToDictionary(x => x.Key,
                        y => y.AggregateWithSeparator(x => x.SubjectName, separator));

                var inspectionDict = inspectionAppCittsDomain.GetAll()
                    .Where(s => appealIds.Contains(s.AppealCits.Id))
                    .Select(x => new
                    {
                        x.AppealCits.Id,
                        InspectionId = x.Inspection.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.InspectionId).ToList());

                var inspectionList = inspectionDict.SelectMany(x => x.Value);

                var disposalDict = disposalDomain.GetAll()
                    .Where(s => inspectionList.Contains(s.Inspection.Id)&& 
                        (s.TypeDisposal == TypeDisposalGji.Base || s.TypeDisposal == TypeDisposalGji.Licensing))
                    .Select(x => new
                    {
                        x.Inspection.Id,
                        x.DocumentNumber
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.DocumentNumber).ToList());

                var disposalResult = inspectionDict.Select(x => new
                {
                    x.Key,
                    Num = x.Value.Where(y => disposalDict.ContainsKey(y)).SelectMany(y => disposalDict[y]).AggregateWithSeparator(separator)
                });

                var soprAppealsInfoDict = new Dictionary<long, SoprAppealInfoDto>();
    
                if (config.RapidResponseSystemConfig.EnableGjiAppealRecordsBacklight)
                {
                    soprAppealsInfoDict = rapidResponseSystemAppealDomain.GetAll()
                        .Where(x => appealIds.Contains(x.AppealCits.Id))
                        .Join(rapidResponseSystemAppealDetailsDomain.GetAll(),
                            x => x.Id,
                            y => y.RapidResponseSystemAppeal.Id,
                            (x, y) => new { Appeal = x, AppealDetails = y })
                        .AsEnumerable()
                        .GroupBy(x => x.Appeal.AppealCits.Id)
                        .ToDictionary(x => x.Key,
                            y => new SoprAppealInfoDto
                            {
                                // Детализаций в статусе "Обработано" >= 50%
                                IsProcessedStateOver = y.Count(z => z.AppealDetails.State.Code == "3") * 2 >= y.Count(),
                                // Детализаций в статусе "Не обработано" > 50%
                                IsNotProcessedStateOver = y.Count(z => z.AppealDetails.State.Code == "4") * 2 > y.Count()
                            });
                }

                var result = data.Select(ac =>
                {
                    var appealSource = appealSourcesDict.Get(ac.Id);
                    var soprAppealInfo = soprAppealsInfoDict.Get(ac.Id);

                    return new
                    {
                        ac.Id,
                        ac.State,
                        ac.Name,
                        // Для отображения в строке масового выбора
                        ac.ManagingOrganization,
                        ac.Contragent,
                        ac.Number,
                        ac.NumberGji,
                        ac.DateFrom,
                        ac.CheckTime,
                        ac.QuestionsCount,
                        ac.Municipality,
                        ac.CountRealtyObj,
                        ac.IsEdo,
                        ac.Executant,
                        ac.Tester,
                        ac.SuretyResolve,
                        ac.ExecuteDate,
                        ac.ZonalInspection,
                        ac.RealObjAddresses,
                        ac.Correspondent,
                        ac.AddressEdo,
                        ac.CountSubject,
                        //прицепляем источники жалоб к новым колонкам в результате
                        appealSource?.RevenueSourceNames,
                        appealSource?.RevenueSourceNumbers,
                        appealSource?.RevenueSourceDates,
                        SubjectName = subjectsDict.Get(ac.Id),
                        DocumentNumber = disposalResult.Where(s => s.Key == ac.Id).AggregateWithSeparator(x => x.Num, separator),
                        soprAppealInfo?.IsProcessedStateOver,
                        soprAppealInfo?.IsNotProcessedStateOver
                    };
                }).ToList();

                return result;
            }
        }
        
        public override IDataResult GetInfo(long? appealCitsId)
        {
            var motivatedPresentationAppealCitsDomain = this.Container.ResolveDomain<MotivatedPresentationAppealCits>();
            
            using (this.Container.Using(motivatedPresentationAppealCitsDomain))
            {
                var result = base.GetInfo(appealCitsId).Data.CastAs<GetInfoDto>();

                var hasMotivatedPresentationResult = motivatedPresentationAppealCitsDomain.GetAll()
                    .Where(x => x.AppealCits.Id == appealCitsId)
                    .Any(x => x.ResultType == MotivatedPresentationResultType.NeedKnmExecuting);

                return new BaseDataResult(new
                {
                    result.relatedAppealIds,
                    result.relatedAppealNames,
                    hasMotivatedPresentationResult
                });
            }
        }

        /// <summary>
        /// Dto-шка с информацией об обращении в СОПР
        /// </summary>
        private class SoprAppealInfoDto
        {
            /// <summary>
            /// Признак того, что детализаций в статусе "Обработано" >= 50%
            /// </summary>
            public bool IsProcessedStateOver { get; set; }

            /// <summary>
            /// Признак того, что детализаций в статусе "Не обработано" > 50%
            /// </summary>
            public bool IsNotProcessedStateOver { get; set; }
        }
    }
}