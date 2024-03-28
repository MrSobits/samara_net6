namespace Bars.Gkh.RegOperator.Distribution
{
    using B4;

    using Bars.Gkh.RegOperator.Enums;

    using DomainModelServices;
    using Entities.ValueObjects;

    /// <summary>
    /// Распределение со счета НВС
    /// </summary>
    public interface IDistribution
    {
        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Роут клиентского контроллера
        /// </summary>
        string Route { get; }

        /// <summary>
        /// Код распределения
        /// </summary>
        DistributionCode DistributionCode { get; }

        /// <summary>
        /// Маркер распределяемости объекта автоматически
        /// </summary>
        bool DistributableAutomatically { get; }

        /// <summary>
        /// Код
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Идентификатор права доступа
        /// </summary>
        string PermissionId { get; }

        /// <summary>
        /// Проверка применимости распределения к счету НВС
        /// </summary>
        /// <param name="distributable"></param>
        /// <returns></returns>
        bool CanApply(IDistributable distributable);

        /// <summary>
        /// Отменить распределение
        /// </summary>
        /// <param name="distributable">Счет НВС</param>
        /// <param name="operation">Отменяемая операция</param>
        void Undo(IDistributable distributable, MoneyOperation operation);

        /// <summary>
        /// Применить распределение
        /// </summary>
        /// <param name="args"></param>
        IDataResult Apply(IDistributionArgs args);
        
        /// <summary>
        /// Вытащить аргументы распределения
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDistributionArgs ExtractArgsFrom(BaseParams baseParams);

        /// <summary>
        /// Вытащить аргументы из множества распределений
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDistributionArgs ExtractArgsFromMany(BaseParams baseParams, int counter, decimal thisOneDistribSum);

        /// <summary>
        /// Получить объекты распределения
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListDistributionObjs(BaseParams baseParams);

        /// <summary>
        /// Получить наименование плательщика
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Плательщик</returns>
        IDataResult GetOriginatorName(BaseParams baseParams);
    }
}