namespace Bars.GkhGji.Regions.Khakasia.DomainService
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
    using Bars.GkhGji.Regions.Khakasia.Entities;

    /// <summary>
    /// Сервис для Обращение ГЖИ
    /// </summary>
    public class AppealCitsService : Bars.GkhGji.DomainService.AppealCitsService<ViewAppealCitizens>
    {
        /// <summary>
        /// Добавить пользовательские фильтры
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <returns>Запрос с фильтрами</returns>
        public override IQueryable<ViewAppealCitizens> AddUserFilters(IQueryable<ViewAppealCitizens> query)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var serviceAppealCitsExecutant = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();

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

                this.Container.Release(userManager);
                this.Container.Release(serviceAppealCitsExecutant);
            }
        }

        /// <summary>
        /// Получить список из view
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <param name="totalCount">Общее количество</param>
        /// <param name="usePaging">Нужна ли пагинация</param>
        /// <returns></returns>
        public override IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging)
        {
            var serviceAppealCitizensGji = this.Container.Resolve<IDomainService<ViewAppealCitizens>>();
            var serviceAppealCitsExecutant = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();
            var appealCitsSourceService = this.Container.Resolve<IDomainService<AppealCitsSource>>();

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

                var query = this.ApplyFilters(this.AddUserFilters(serviceAppealCitizensGji.GetAll()), baseParams)
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
                            x.AppealCits.SpecialControl,
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
                this.Container.Release(serviceAppealCitizensGji);
                this.Container.Release(serviceAppealCitsExecutant);
                this.Container.Release(appealCitsSourceService);
            }
        }
    }
}