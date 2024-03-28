namespace Bars.GkhGji.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Сервис для акта без взаимодействия
    /// </summary>
    public interface IActIsolatedService
    {
        /// <summary>
        /// Получить информацию (для карточки Акта)
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult GetInfo(BaseParams baseParams);

        /// <summary>
        /// Получить список из вьюхи
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult ListView(BaseParams baseParams);

        /// <summary>
        /// Получить список из вьюхи
        /// </summary>
        /// <returns>Результат выполнения</returns>
        IQueryable<ViewActIsolated> GetViewList();

        /// <summary>
        /// Получить список актов без взаимодействия для этапа
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        IDataResult ListForStage(BaseParams baseParams);
    }
}