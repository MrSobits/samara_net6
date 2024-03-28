namespace Bars.Gkh.RegOperator.DomainModelServices
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Интерфейс фильтрации ЛС
    /// </summary>
    public interface IPersonalAccountFilterService
    {
        /// <summary>
        /// Отфильтровать запрос по параметрам запроса
        /// </summary>
        /// <param name="query">Запрос</param>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Отфильтрованный запрос</returns>
        IQueryable<TModel> FilterByBaseParams<TModel>(IQueryable<TModel> query, BaseParams baseParams) where TModel : PersonalAccountDto;

        /// <summary>
        /// Отфильтровать запрос для расчетов
        /// </summary>
        /// <param name="data">Запрос</param>
        /// <param name="period">Период</param>
        /// <returns>Отфильтрованный запрос</returns>
        IQueryable<TModel> FilterCalculable<TModel>(IQueryable<TModel> data, IPeriod period) where TModel : PersonalAccountDto;

        /// <summary>
        /// Фильтрация в соответствии с параметрами из настроек РФ
        /// </summary>
        /// <param name="data">Запрос</param>
        /// <returns>Отфильтрованный запрос</returns>
        IQueryable<TModel> FilterByRegFondSetting<TModel>(IQueryable<TModel> data) where TModel : PersonalAccountDto;

        /// <summary>
        /// получение списка идентификаторов домов
        /// по способу формирования счёта для капитального ремонта
        /// </summary>
        /// <param name="crFundType">Способ формирования фонда</param>
        /// <returns>Отфильтрованный запрос</returns>
        List<long> GetDecisionFilteredRoCrFundType(CrFundFormationType crFundType);
        
        /// <summary>
        /// Данный метод забирает всю фильтрацию которая делается в реестре Фильтры в компонентах, и получает IQueryable
        /// </summary>
        IQueryable<Dto.BasePersonalAccountDto> GetQueryableByFilters(BaseParams baseParams, IQueryable<BasePersonalAccount> queryable);

        /// <summary>
        /// Данный метод забирает всю фильтрацию которая делается в реестре Фильтры в компонентах,
        /// с учетом настроект адреса для получения платежек и получает IQueryable
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <param name="queryable">Запрос</param>
        /// <returns>Отфильтрованный запрос</returns>
        IQueryable<BillingPersonalAccountDto> GetQueryableByFiltersAndBillingAddress(BaseParams baseParams, IQueryable<BasePersonalAccount> queryable);
    }
}