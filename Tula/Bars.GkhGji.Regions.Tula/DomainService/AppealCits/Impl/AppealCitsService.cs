namespace Bars.GkhGji.Regions.Tula.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tula.Entities;
    using GkhGji.Utils;
    using Gkh.Utils;

    public class AppealCitsService : Bars.GkhGji.DomainService.AppealCitsService
    {
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
                         .WhereIf(
                             inspectorsList.Count > 0,
                             x =>
                             serviceAppealCitsExecutant.GetAll()
                                                       .Any(
                                                           y =>
                                                           inspectorsList.Contains(y.Author.Id)
                                                           || inspectorsList.Contains(y.Executant.Id)));
            }
            finally
            {

                Container.Release(userManager);
                Container.Release(serviceAppealCitsExecutant);
            }
        }

        public override IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging)
        {
            var serviceAppealCitizensGji = Container.Resolve<IDomainService<ViewAppealCitizens>>();
            var serviceAppealCitsExecutant = Container.Resolve<IDomainService<AppealCitsExecutant>>();
            var appealCitsSourceService = Container.Resolve<IDomainService<AppealCitsSource>>();

            try
            {
                var loadParams = baseParams.Params.Read<LoadParam>().Execute(Converter.ToLoadParam);
                var ids = baseParams.Params.ContainsKey("Id") ? baseParams.Params["Id"].ToStr() : string.Empty;

                var listIds = new List<long>();
                if (!string.IsNullOrEmpty(ids))
                {
                    listIds = ids.Split(',').Select(id => id.ToLong()).ToList();
                }

                var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
                var authorId = baseParams.Params.GetAs<long>("authorId");
                var executantId = baseParams.Params.GetAs<long>("executantId");

                var dateFromStart = baseParams.Params.GetAs("dateFromStart", DateTime.MinValue);
                var dateFromEnd = baseParams.Params.GetAs("dateFromEnd", DateTime.MinValue);

                var checkTimeStart = baseParams.Params.GetAs("checkTimeStart", DateTime.MinValue);
                var checkTimeEnd = baseParams.Params.GetAs("checkTimeEnd", DateTime.MinValue);
                var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", true);

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
                
                /*
                 * Если в операторе указаны инспектора то фильтруем по инспекторам
                 * Но если указан Признак Пкоазывать неназначенные обращения то тогда над опоказывать x.Executant = null
                 * Соответсвенн оесли и указаны инспектораи признак то над оучитыват ьвсе условия
                 */

                var query = ApplyFilters(AddUserFilters(serviceAppealCitizensGji.GetAll()),loadParams)
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
                    .Select(
                        x =>
                        new
                        {
                            x.Id,
                            x.State,
                            Name = string.Format("{0} ({1})", x.Number, x.NumberGji),  // Для отображения в строке масового выбора
                            ManagingOrganization = x.ContragentName,
                            x.Number,
                            x.NumberGji,
                            x.DateFrom,
                            x.CheckTime,
                            x.QuestionsCount,
                            x.Municipality,
                            x.CountRealtyObj,
                            Executant = x.Executant.Fio,
                            Tester = x.Tester.Fio,
                            SuretyResolve = x.SuretyResolve.Name,
                            x.ExecuteDate,
                            x.SuretyDate,
                            x.ZonalInspection,
                            ZoneName = x.ZonalInspection != null ? x.ZonalInspection.ZoneName : "",
                            x.Correspondent,
                            x.CorrespondentAddress,
                            x.RealObjAddresses,
                            x.AppealCits.SpecialControl
                        })
                    .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                    .Filter(loadParams, Container);

                if (usePaging)
                {
                    totalCount = query.Count();
                    query = query.Order(loadParams).Paging(loadParams);
                }
                else
                {
                    query = query.Order(loadParams);
                    totalCount = query.Count();
                }

                var data = query.ToList();

                var dataIds = data.Select(x => x.Id);

                var sources = appealCitsSourceService.GetAll().Where(s => dataIds.Contains(s.AppealCits.Id)).ToList();
                const string separator = ", ";

                var result = data.Select(ac => new
                {
                    ac.Id,
                    ac.State,
                    ac.Name,  // Для отображения в строке масового выбора
                    ac.ManagingOrganization,
                    ac.Number,
                    ac.NumberGji,
                    ac.DateFrom,
                    ac.CheckTime,
                    ac.QuestionsCount,
                    ac.Municipality,
                    ac.CountRealtyObj,
                    ac.Executant,
                    ac.Tester,
                    ac.SuretyResolve,
                    ac.ExecuteDate,
                    ac.SuretyDate,
                    ac.ZonalInspection,
                    ac.ZoneName,
                    ac.Correspondent,
                    ac.CorrespondentAddress,
                    ac.RealObjAddresses,
                    ac.SpecialControl,

                    //прицепляем источники жалоб к новым колонкам в результате
                    RevenueSourceNames =
                        sources.Where(s => s.AppealCits.Id == ac.Id)
                            .AggregateWithSeparator(s => s.RevenueSource.Name, separator),
                    RevenueSourceNumbers =
                        sources.Where(s => s.AppealCits.Id == ac.Id)
                            .AggregateWithSeparator(s => s.RevenueSourceNumber, separator),
                    RevenueSourceDates =
                        sources.Where(s => s.AppealCits.Id == ac.Id)
                            .AggregateWithSeparator(s => s.RevenueDate.ToDateString(), separator)
                }).ToList();

                return result;
            }
            finally
            {
                Container.Release(serviceAppealCitizensGji);
                Container.Release(serviceAppealCitsExecutant);
                Container.Release(appealCitsSourceService);
            }
        }
    }
}