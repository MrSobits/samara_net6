using Bars.B4;

namespace Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction
{
    /// <summary>
    /// Сервис для <see cref="PreventiveActionTask"/>
    /// </summary>
    public interface IPreventiveActionTaskService
    {
        /// <summary>
        /// Получить список документов "Задание профилактического мероприятия"
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список документов</returns>
        IDataResult List(BaseParams baseParams);
    }
}