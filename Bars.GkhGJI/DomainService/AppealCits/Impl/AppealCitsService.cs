namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;
    using Bars.Gkh.Entities;

    public class AppealCitsService : AppealCitsService<ViewAppealCitizens>
    {
    }

    /// <summary>
    /// Сервис для работы с Обращения граждан
    /// </summary>
    public class AppealCitsService<T>: IAppealCitsService<T>  where T : ViewAppealCitizens
    {
		/// <summary>
		/// Контейнер
		/// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Добавить обращения
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult AddAppealCitizens(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDomainService<RelatedAppealCits>>();

            try
            {
                var appealCitizensId = baseParams.Params.ContainsKey("appealCitizensId")
                                           ? baseParams.Params["appealCitizensId"].ToLong()
                                           : 0;

                var objectIds = baseParams.Params.ContainsKey("objectIds")
                                    ? baseParams.Params["objectIds"].ToString()
                                    : "";

                if (!string.IsNullOrEmpty(objectIds))
                {
                    // в этом списке будут обращения которые уже связаны с этой инспекцией (чтобы недобавлять несколько одинаковых)
                    var listObjects =
                        service.GetAll()
                               .Where(x => x.Parent.Id == appealCitizensId)
                               .ToList();
                    var oldIds = listObjects.Select(rac => rac.Children.Id).Distinct().ToArray();

                    var newIds = objectIds.Split(',').Select(x => x.ToLong()).ToArray();
                    var idsToAdd = newIds.Except(oldIds).ToArray();
                    var idsToRemove = oldIds.Except(newIds).ToArray();

                    foreach (var newId in idsToAdd)
                    {
                        var newObj = new RelatedAppealCits
                                         {
                                             Parent = new AppealCits { Id = appealCitizensId },
                                             Children = new AppealCits { Id = newId }
                                         };

                        service.Save(newObj);
                    }

                    foreach (var id in idsToRemove)
                    {
                        var objToRemove = listObjects.Single(rac => rac.Children.Id == id);
                        service.Delete(objToRemove.Id);
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, message = e.Message });
            }
            finally
            {
                this.Container.Release(service);
            }
        }

		/// <summary>
		/// Удалить связанные обращения
		/// </summary>
		/// <param name="id">Идентификатор дочернего обращения</param>
		/// <param name="parentId">Идентификатор родительского обращения</param>
		/// <returns>Результат выполнения запроса</returns>
        public IDataResult RemoveRelated(long id, long parentId)
        {
            var service = this.Container.Resolve<IDomainService<RelatedAppealCits>>();
            try
            {
                var relation = service.GetAll().FirstOrDefault(x => x.Parent.Id == parentId && x.Children.Id == id);
                if (relation != null)
                {
                    service.Delete(relation.Id);
                }
                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
		/// Удалить связанные обращения
		/// </summary>
		/// <param name="id">Идентификатор дочернего обращения</param>
		/// <param name="parentId">Идентификатор родительского обращения</param>
		/// <returns>Результат выполнения запроса</returns>
        public IDataResult RemoveAllRelated(long parentId)
        {
            var service = this.Container.Resolve<IDomainService<RelatedAppealCits>>();
            try
            {
                var relation = service.GetAll().Where(x => x.Parent.Id == parentId).Select(x=> x.Id).ToList();
                foreach(var id in relation)
                {
                    service.Delete(id);
                }
                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить информацию по обращению
        /// </summary>
        /// <param name="appealCitsId">Идентификатор обращения</param>
        /// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult GetInfo(long? appealCitsId)
        {
            var service = this.Container.Resolve<IDomainService<RelatedAppealCits>>();

            try
            {
                var relatedAppealNames = string.Empty;
                var relatedAppealIds = string.Empty;

                var data =
                    service.GetAll()
                           .Where(x => x.Parent.Id == appealCitsId)
                           .Select(x => new { ChildrenId = x.Children.Id, x.Children.NumberGji, x.Children.DateFrom })
                           .ToList();

                foreach (var item in data)
                {
                    if (!string.IsNullOrEmpty(relatedAppealNames))
                    {
                        relatedAppealNames += ", ";
                    }

                    relatedAppealNames += item.NumberGji
                                          + (item.DateFrom.HasValue
                                                 ? "-" + item.DateFrom.Value.ToShortDateString()
                                                 : string.Empty);

                    if (!string.IsNullOrEmpty(relatedAppealIds))
                    {
                        relatedAppealIds += ", ";
                    }

                    relatedAppealIds += item.ChildrenId.ToStr();
                }

                return new BaseDataResult(new GetInfoDto
                {
                    relatedAppealIds = relatedAppealIds,
                    relatedAppealNames = relatedAppealNames
                });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
            finally
            {
                this.Container.Release(service);
            }
        }

       
        /// <summary>
        /// Получить зональную жилищную инспекцию по умолчанию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult GetDefaultZji(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDomainService<ZonalInspection>>();

            try
            {
                var data = service.GetAll().Select(x => new { x.Id, x.Name });

                var totalCount = data.Count();

                if (totalCount > 1)
                {
                    return new BaseDataResult(null);
                }
                
                return new BaseDataResult(data.FirstOrDefault());
            }
            finally 
            {
                this.Container.Release(service);
            }
            
        }

		/// <summary>
		/// Добавить пользовательские фильтры к запросу
		/// </summary>
		/// <param name="query">Запрос</param>
		/// <returns>Модифицированный запрос</returns>
        public virtual IQueryable<T> AddUserFilters(IQueryable<T> query)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            try
            {
                var municipalityList = userManager.GetMunicipalityIds();
                var inspectorsList = userManager.GetInspectorIds();

                return
                    query.WhereIf(
                        municipalityList.Count > 0,
                        x => municipalityList.Contains((long)x.MunicipalityId) || !x.MunicipalityId.HasValue)
                         .WhereIf(
                             inspectorsList.Count > 0,
                             x =>
                             inspectorsList.Contains(x.Executant.Id) 
                             || inspectorsList.Contains(x.Tester.Id)
                             || inspectorsList.Contains(x.Surety.Id));
            }
            finally
            {
                this.Container.Release(userManager);
            }
        }

		/// <summary>
		/// Добавить фильтры источника обращения
		/// </summary>
		/// <param name="query">Запрос</param>
		/// <param name="loadParams">Параметры загрузки</param>
		/// <returns>Результат выполнения запроса</returns>
        public virtual IQueryable<T> ApplyAppealSourceFilter(IQueryable<T> query, LoadParam loadParams)
        {
            if (this.CheckFilters(loadParams))
            {
                return query;
            }

            var revenueSourceNamesFilter =
                loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "RevenueSourceNames");
            var revenueSourceNumbersFilter =
                loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "RevenueSourceNumbers");
            var revenueSourceDatesFilter =
                loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "RevenueSourceDates") != null
                    ? DateTime.Parse(
                        loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "RevenueSourceDates").Value)
                    : DateTime.MinValue;
            var filters = new[] {"RevenueSourceNames", "RevenueSourceNumbers", "RevenueSourceDates"};
            
            IQueryable<long> appealIdsFromSourceFilter = null;
            if (revenueSourceNamesFilter != null && revenueSourceNamesFilter.Value.IsNotEmpty() ||
                revenueSourceNumbersFilter != null && revenueSourceNumbersFilter.Value.IsNotEmpty() ||
                revenueSourceDatesFilter != DateTime.MinValue)
            {
                var appealCitsSourceService = this.Container.Resolve<IDomainService<AppealCitsSource>>();

                appealIdsFromSourceFilter = appealCitsSourceService.GetAll()
                    .WhereIf(revenueSourceNamesFilter != null && revenueSourceNamesFilter.Value.IsNotEmpty(),
                        s => (s.RevenueSource.Name.ToUpper().Contains(revenueSourceNamesFilter.Value.ToUpper())
                        || s.RevenueSource.Name.Replace(" ", "").ToUpper().Contains(revenueSourceNamesFilter.Value.Replace(" ", "").ToUpper())))
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

        protected virtual IQueryable<T> ApplyAddressFilter(IQueryable<T> query, LoadParam loadParams)
        {
            if (this.CheckFilters(loadParams)) return query;

            var realObjAddress = loadParams.DataFilter.Filters.FirstOrDefault(f => f.DataIndex == "RealObjAddresses");
            if (realObjAddress != null)
            {
                var appealCitsRealityObjectService = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();
                loadParams.DataFilter.Filters =
                    loadParams.DataFilter.Filters.Where(f => f.DataIndex != "RealObjAddresses");
                var appealIds = appealCitsRealityObjectService.GetAll()
                    .WhereIf(realObjAddress.Value.IsNotEmpty(),
                        ar => ar.RealityObject.Address.Contains(realObjAddress.Value)).Select(ar => ar.AppealCits.Id);
                return query.Where(x => appealIds.Contains(x.Id));
            }
            return query;
        }

        protected bool CheckFilters(LoadParam loadParams)
        {
            if (loadParams.DataFilter == null || loadParams.DataFilter.Filters == null || !loadParams.DataFilter.Filters.Any())
            {
                return true;
            }
            return false;
        }

        protected virtual IQueryable<T> ApplyFilters(IQueryable<T> query, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            query = this.ApplyAddressFilter(query, loadParams);
            query = this.ApplyAppealSourceFilter(query, loadParams);
            return query;
        }

		/// <summary>
		/// Получить список обращений из view
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <param name="totalCount">Общее количество</param>
		/// <param name="usePaging">Использовать пагинацию?</param>
		/// <returns>Результат выполнения запроса</returns>
        public virtual IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging)
        {
            var serviceAppealCitizensGji = this.Container.Resolve<IDomainService<T>>();
            var appealCitsSourceService = this.Container.Resolve<IDomainService<AppealCitsSource>>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var ids = baseParams.Params.ContainsKey("Id") ? baseParams.Params["Id"].ToStr() : string.Empty;

                var listIds = new List<long>();
                if (!string.IsNullOrEmpty(ids))
                {
                    listIds = ids.Split(',').Select(id => id.ToLong()).ToList();
                }

                var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
                var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");

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

                // Фильтрация по инспектору добавлена по задаче 33244

                var query = this.ApplyFilters(this.AddUserFilters(serviceAppealCitizensGji.GetAll()), baseParams)
                    .WhereIf(appealCitizensId > 0, x => x.Id != appealCitizensId)
                    .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id))
                    .WhereIf(
                        realityObjectId > 0,
                        x => x.RealityObjectIds.Contains($"/{realityObjectId}/"))
                    .WhereIf(dateFromStart != DateTime.MinValue, x => x.DateFrom > dateFromStart)
                    .WhereIf(dateFromEnd != DateTime.MinValue, x => x.DateFrom < dateFromEnd)
                    .WhereIf(checkTimeStart != DateTime.MinValue, x => x.CheckTime > checkTimeStart)
                    .WhereIf(checkTimeEnd != DateTime.MinValue, x => x.CheckTime < checkTimeEnd)
                    .WhereIf(!showCloseAppeals, x => x.State == null || !x.State.FinalState)
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.State,
                            Name = $"{x.Number} ({x.NumberGji})",
                            // Для отображения в строке масового выбора
                            ManagingOrganization = x.ContragentName,
                            x.Number,
                            x.NumberGji,
                            x.DateFrom,
                            x.CheckTime,
                            x.QuestionsCount,
                            x.Municipality,
                            x.MoSettlement,
                            x.PlaceName,
                            x.CountRealtyObj,
                            Executant = x.Executant.Fio?? "",
                            Tester = x.Tester.Fio,
                            SuretyResolve = x.SuretyResolve.Name?? "",
                            x.ExecuteDate,
                            x.ZonalInspection,
                            ZoneName = x.ZonalInspection != null ? x.ZonalInspection.ZoneName : "",
                            x.Correspondent,
                            x.RealObjAddresses,
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
                this.Container.Release(appealCitsSourceService);
            }
        }

        /// <inheritdoc />
        public IQueryable<TQuery> FilterByActiveAppealCits<TQuery>(IQueryable<TQuery> query, Expression<Func<TQuery, State>> stateSelector)
        {
            var nullRef = Expression.Constant(null, typeof(State));
            var pending = Expression.Constant("В ожидании", typeof(string));
            var needCancel = Expression.Constant("Требует отмены", typeof(string));
            var canceled = Expression.Constant("Отменено", typeof(string));

            var stateName = Expression.Property(stateSelector.Body, "Name");

            return query
                .Where(Expression.Lambda<Func<TQuery, bool>>(Expression.NotEqual(stateSelector.Body, nullRef), stateSelector.Parameters[0]))
                .Where(Expression.Lambda<Func<TQuery, bool>>(Expression.NotEqual(stateName, pending), stateSelector.Parameters[0]))
                .Where(Expression.Lambda<Func<TQuery, bool>>(Expression.NotEqual(stateName, needCancel), stateSelector.Parameters[0]))
                .Where(Expression.Lambda<Func<TQuery, bool>>(Expression.NotEqual(stateName, canceled), stateSelector.Parameters[0]));
        }

        /// <inheritdoc />
        public IDataResult WorkWithSoprAvailable(BaseParams baseParams)
        {
            var appealCitsId = baseParams.Params.GetAsId("appealCitizensId");

            var appealCitsStatSubjectDomain = this.Container.ResolveDomain<AppealCitsStatSubject>();

            using (this.Container.Using(appealCitsStatSubjectDomain))
            {
                var data = appealCitsStatSubjectDomain.GetAll()
                    .Any(x =>
                        x.AppealCits.Id == appealCitsId &&
                        x.AppealCits.IsIdentityVerified &&
                        (x.Subject.NeedInSopr || x.Subsubject.NeedInSopr));

                return new BaseDataResult(data);
            }
        }
    }
    
    public class GetInfoDto
    {
        public string relatedAppealIds { get; set; }

        public string relatedAppealNames { get; set; }
    }
}