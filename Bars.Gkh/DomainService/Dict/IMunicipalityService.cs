namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Интерфейс "Муниципальный район и Муниципальный образование"
    /// </summary>
    public interface IMunicipalityService
    {
        /// <summary>
        /// Вернуть МО без пагинации
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListWithoutPaging(BaseParams baseParams);

        /// <summary>
        /// Список по идентификаторам
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListByReg(BaseParams baseParams);

        /// <summary>
        /// Возвращает список МО в зависимости от оператора
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список МО</returns>
        IDataResult ListByOperator(BaseParams baseParams);

        /// <summary>
        /// Добавить МО по номеру ФИАС
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult AddMoFromFias(BaseParams baseParams);

        /// <summary>
        /// Вернуть дерево МО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListTree(BaseParams baseParams);

        /// <summary>
        /// Вернуть дерево по поисковому запросу
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListSelectTree(BaseParams baseParams);

        /// <summary>
        /// Вернуть МО в соответствии с настройками приложения
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListByParam(BaseParams baseParams);

        /// <summary>
        /// Вернуть список МР по настройкам с пагинацией
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListByParamPaging(BaseParams baseParams);

        /// <summary>
        /// Установить родителский МО для указанного
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult SetMoParent(BaseParams baseParams);

        /// <summary>
        /// Вернуть МО в соответствии с настройками приложения (с пагинацией)
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListMoArea(BaseParams baseParams);

        /// <summary>
        /// Вернуть Муниципальные районы (без пагинации)
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListMoAreaWithoutPaging(BaseParams baseParams);

        /// <summary>
        /// Вернуть муниципальные образования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListSettlement(BaseParams baseParams);

        /// <summary>
        /// Вернуть муниципальные образования с названием родительского мун. района и гор. округи
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListSettlementWithParentMo(BaseParams baseParams);

        /// <summary>
        /// Вернуть городские округи и муниципальные образования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListSettlementAndUrban(BaseParams baseParams);

        /// <summary>
        /// Вернуть городские округи и муниципальные образования
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListAllWithParent(BaseParams baseParams);

        /// <summary>
        /// Вернуть муниципальные образования (без пагинации)
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListSettlementWithoutPaging(BaseParams baseParams);

        /// <summary>
        /// Вернуть список МР по настройкам
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListByParamAndOperator(BaseParams baseParams);

        /// <summary>
        /// Вернуть список МР по настройкам
        /// </summary>
        /// <returns>Результат операции</returns>
        IQueryable<MunicipalityProxy> ListByParamAndOperatorQueryable();

        /// <summary>
        /// Вернуть по уровню МО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListByWorkpriceParam(BaseParams baseParams);

        /// <summary>
        /// Объединить МО
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult UnionMo(BaseParams baseParams);
    }

    /// <summary>
    /// Прокси-класс для Муниципальное образование
    /// </summary>
    public class MunicipalityProxy
    {
        public long Id { get; set; }

        public TypeMunicipality Level { get; set; }

        public string Name { get; set; }

        public string ParentMo { get; set; }
    }
}