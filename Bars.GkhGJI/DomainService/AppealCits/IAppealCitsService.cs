namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public interface IAppealCitsService <T> where T : ViewAppealCitizens
    {
        IDataResult AddAppealCitizens(BaseParams baseParams);

        IDataResult RemoveRelated(long id, long parentId);

        IDataResult RemoveAllRelated(long parentId);

        IDataResult GetInfo(long? appealCitsId);

        IDataResult GetDefaultZji(BaseParams baseParams);

        IQueryable<T> AddUserFilters(IQueryable<T> query);

        IList GetViewModelList(BaseParams baseParams, out int totalCount, bool usePaging);

        IQueryable<T> ApplyAppealSourceFilter(IQueryable<T> query, LoadParam loadParams);

        /// <summary>
        /// Метод получения активных обращений граждан
        /// </summary>
        IQueryable<TQuery> FilterByActiveAppealCits<TQuery>(IQueryable<TQuery> query, Expression<Func<TQuery, State>> stateSelector);

        /// <summary>
        /// Доступность работы с СОПР для указанного обращения 
        /// </summary>
        IDataResult WorkWithSoprAvailable(BaseParams baseParams);
    }
}