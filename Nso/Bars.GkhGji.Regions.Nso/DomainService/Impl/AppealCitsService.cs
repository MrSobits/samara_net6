namespace Bars.GkhGji.Regions.Nso.DomainService.Impl
{
    using System;
    using System.Collections;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Authentification;
    using Gkh.Domain;
    using GkhGji.Entities;

    public class AppealCitsService : GkhGji.DomainService.AppealCitsService<ViewAppealCitizens>
    {
        public IQueryable<ViewAppealCitizens> AddUserFilters(IQueryable<ViewAppealCitizens> query)
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
            var serviceAppealCitsExecutant = Container.Resolve<IDomainService<AppealCitsExecutant>>();
            var serviceAppealCitizensGji = Container.Resolve<IDomainService<ViewAppealCitizens>>();
            var appealCitsSourceService = Container.Resolve<IDomainService<AppealCitsSource>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();


                var ids = baseParams.Params.GetAs<string>("Id").ToLongArray();

                var appealCitizensId = baseParams.Params.GetAsId("appealCitizensId");
                var realityObjectId = baseParams.Params.GetAsId("realityObjectId");

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

                var query = ApplyFilters(AddUserFilters(serviceAppealCitizensGji.GetAll()), baseParams)
                    .WhereIf(appealCitizensId > 0, x => x.Id != appealCitizensId)
                    .WhereIf(ids.Length > 0, x => ids.Contains(x.Id))
                    .WhereIf(realityObjectId > 0,
                        x => x.RealityObjectIds.Contains(string.Format("/{0}/", realityObjectId)))
                    .WhereIf(dateFromStart != DateTime.MinValue, x => x.DateFrom > dateFromStart)
                    .WhereIf(dateFromEnd != DateTime.MinValue, x => x.DateFrom < dateFromEnd)
                    .WhereIf(checkTimeStart != DateTime.MinValue, x => x.CheckTime > checkTimeStart)
                    .WhereIf(checkTimeEnd != DateTime.MinValue, x => x.CheckTime < checkTimeEnd)
                    .WhereIf(!showCloseAppeals, x => x.State == null || !x.State.FinalState)
                    .Select(x => new
                    {
                        x.Id,
                        x.State,
                        Name = string.Format("{0} ({1})", x.Number, x.NumberGji),
                        // Для отображения в строке масового выбора
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
                        x.ZonalInspection,
                        ZoneName = x.ZonalInspection != null ? x.ZonalInspection.ZoneName : "",
                        x.Correspondent,
                        x.RealObjAddresses,
                        HasExecutant = serviceAppealCitsExecutant.GetAll().Any(y => y.AppealCits.Id == x.Id),
                        x.MoSettlement,
                        x.PlaceName,
                        x.RevenueSourceNames,
                        x.RevenueSourceDates,
                        x.RevenueSourceNumbers
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
                Container.Release(serviceAppealCitsExecutant);
                Container.Release(serviceAppealCitizensGji);
                Container.Release(appealCitsSourceService);
            }
        }
    }
}
