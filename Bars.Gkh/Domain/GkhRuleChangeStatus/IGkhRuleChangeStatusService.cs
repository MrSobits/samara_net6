namespace Bars.Gkh.Domain
{
    using System.Collections.Generic;
    using Bars.B4.Modules.States;

    public interface IGkhRuleChangeStatusService
    {
        /// <summary>
        /// Получить  правила перехода для статусов, которые реализуют обработку пользовательских данных
        /// </summary>
        /// <param name="typeId"> тип IStatefulEntity</param>
        /// <param name="newStateId"> идентификатор нового статуса</param>
        /// <param name="entity"> сущность, у которого меняется статус</param>
        /// <returns></returns>
        List<IGkhRuleChangeStatus> GetGkhRuleStatus(string typeId, long newStateId, IStatefulEntity entity);

        /// <summary>
        /// Получить сущность, у которого меняется статус
        /// </summary>
        /// <param name="typeId">тип IStatefulEntity</param>
        /// <param name="entityId"> идентификатор сущности, у которого меняется статус</param>
        /// <returns></returns>
        IStatefulEntity GetEntity(string typeId, long entityId);
    }
}