namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActCheck
{
    using Bars.B4;
    using System.Collections;

    /// <summary>
    /// Интерфейс сервиса действия акта проверки 
    /// </summary>
    public interface IActCheckActionService
    {
        /// <summary>
        /// Добавить выполненные мероприятия действия акта проверки
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult AddCarriedOutEvents(BaseParams baseParams);

        /// <summary>
        /// Добавить инспекторов действия акта проверки
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult AddInspectors(BaseParams baseParams);

        /// <summary>
        /// Получение видов действий из справочника "Вид действия"
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IList GetActionTypes(BaseParams baseParams);
    }
}