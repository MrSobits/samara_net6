namespace Bars.Gkh.RegOperator.Distribution
{
    using B4;

    using Bars.Gkh.RegOperator.DomainModelServices;

    /// <summary>
    /// Провайдер распределений со счетов НВС
    /// </summary>
    public interface IDistributionProvider : ICancellableSourceProvider
    {
        /// <summary>
        /// Получить список всех распределений
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult List(BaseParams baseParams);

        /// <summary>
        /// Получить список  распределений субсидий
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListSubsidyDistribution(BaseParams baseParams);

        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult Validate(BaseParams baseParams);

        /// <summary>
        /// Валидация, которая предусматривает продолжение работы.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult SoftValidate(BaseParams baseParams);

        /// <summary>
        /// Валидация при распределении, которая предусматривает продолжение работы.
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult SoftApplyValidate(BaseParams baseParams);

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult Apply(BaseParams baseParams);

        /// <summary>
        /// Отменить распределение
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult Undo(BaseParams baseParams);

        /// <summary>
        /// Отменить распределение
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат операции</returns>
        IDataResult UndoPartially(BaseParams baseParams);

        /// <summary>
        /// Отменить зачисление
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult UndoCheckin(BaseParams baseParams);

        /// <summary>
        /// Отменить либо удалить распределения
        /// </summary>
        IDataResult UndoOperationOrUndoCheckin(BaseParams baseParams);

        /// <summary>
        /// Получить список объектов с распределенными суммами
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListDistributionObjs(BaseParams baseParams);

        /// <summary>
        /// Вернуть список объектов распределения для автораспределений
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат операции</returns>
        IDataResult ListAutoDistributionObjs(BaseParams baseParams);

        /// <summary>
        /// Получить наименование плательщика
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Плательщик</returns>
        IDataResult GetOriginatorName(BaseParams baseParams);
    }
}