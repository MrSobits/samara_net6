namespace Bars.Gkh.DomainService
{
	using System.Collections.Generic;
	using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

	public interface IRealityObjectService
    {
        /// <summary>
        /// Возвращает то же самое что и метод List, но данные берет из ViewRealityObject. 
        /// Т.к. нужны наименования управляющих организаций и договоров с жилыми домами
        /// </summary>
        IDataResult ListView(BaseParams baseParams);
        
        /// <summary>
        /// Список лифтов
        /// </summary>
        /// <returns></returns>
        IDataResult ListLiftsRegistry(BaseParams baseParams);
        
        List<long> GetAvaliableRoIds(List<long> contragentList);

        IQueryable<ViewRealityObject> GetViewList();

        /// <summary>
        /// Жилые дома (RealityObject) с фильтрацией по оператору
        /// </summary>
        /// <returns></returns>
        IQueryable<RealityObject> GetFilteredByOperator();

        /// <summary>
        /// Список жилых домов, у которых нет текущих договоров непосредственного управления
        /// с фильтрацией по муниципальному образованию оператора
        /// </summary>
        /// <returns></returns>
        IDataResult ListExceptDirectManag(BaseParams baseParams);

        /// <summary>
        /// Список жилых домов, которые указаны во вкладке "жилые дома" в форме редактирования упр.орг
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListByManOrg(BaseParams baseParams);

        /// <summary>
        /// Список не снесенных жилых домов, которые указаны во вкладке "жилые дома" в форме редактирования упр.орг для Тюмени
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListByManOrgTumen(BaseParams baseParams);

        /// <summary>
        /// Список жилых домов, которые указаны во вкладке "жилые дома" в форме редактирования орг. жил. услуг
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListByServOrg(BaseParams baseParams);

        /// <summary>
        /// Список жилых домов по постащику жилищных услуг 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListRoByServOrg(BaseParams baseParams);

        /// <summary>
        /// Список жилых домов, у которых на текущий момент есть договор с управляющей организацией
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListByTypeOrg(BaseParams baseParams);

        /// <summary>
        /// Список жилых домов, для сведений о ЖКУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListGkuInfo(BaseParams baseParams);

        /// <summary>
        /// Сведения о ЖКУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult GetGkuInfo(BaseParams baseParams);
 
        /// <summary>
        /// Тарифы ЖКУ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListGkuInfoTarifs(BaseParams baseParams);

        /// <summary>
        /// Список МО по контрагенту
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListMoByContragentId(BaseParams baseParams);

		/// <summary>
		/// Список жилых домов по постащику
		/// </summary>
		/// <param name="baseParams"></param>
		/// <returns></returns>
		IDataResult ListRoBySupplyResorg(BaseParams baseParams);

        /// <summary>
        /// Список Жилых домов по поставщику ком.услуг
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListBySupplySerOrg(BaseParams baseParams);

        /// <summary>
        /// Список Жилых домов по guid населенного пункта
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListByLocalityGuid(BaseParams baseParams);


        /// <summary>
        /// Список жилых домов по муниципальному образованию образованием(поселению)
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListByMoSettlement(BaseParams baseParams);

        IDataResult ListWoPagingCapitalGroup(BaseParams baseParams);

        IDataResult GetPassportReport(BaseParams baseParams);

        /// <summary>
        /// Возвращает информацию о доме, где адрес дома сформирован в необходимом для поиска виде
        /// </summary>
        /// <returns></returns>
        IDataResult GetForMap(BaseParams baseParams);

		/// <summary>
		/// Получение запроса жилых домов 
		/// </summary>
		/// <param name="baseParams">Базовые параметры </param>
		/// <returns>Запрос жилых домов</returns>
		IQueryable<RealityObjectProxy> GetRealityObjectsQuery(BaseParams baseParams);

        /// <summary>
        /// Получение изменений по текущему жилому дому 
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Список изменений</returns>
        IDataResult GetHistory(BaseParams baseParams);
        
        /// <summary>
        /// Детализации по изменениям
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Список изменений</returns>
        IDataResult GetHistoryDetail(BaseParams baseParams);

    }
}