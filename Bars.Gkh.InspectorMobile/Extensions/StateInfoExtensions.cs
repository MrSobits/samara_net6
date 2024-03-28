namespace Bars.Gkh.InspectorMobile.Extensions
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;

    /// <summary>
    /// Расширения, используемые для удобства работы с <see cref="BaseStateInfo"/> и его наследниками
    /// </summary>
    public static class StateInfoExtensions
    {
        /// <summary>
        /// Сменить статус сущности
        /// </summary>
        /// <param name="stateProvider"></param>
        /// <param name="stateDomain"></param>
        /// <param name="processedStatesDict"></param>
        /// <param name="entity">Сущность для смены статуса</param>
        /// <param name="newStateInfo">Информация о новом статусе</param>
        /// <param name="notAvailableMessagePostFix">Постфикс стандартного сообщения о невозможности смены статуса</param>
        /// <param name="notAvailableMessage">Сообщение для подмены стандартного сообщения о невозможности смены статуса</param>
        /// <typeparam name="TEntity">Тип сущности для смены статуса</typeparam>
        /// <typeparam name="TStateInfo">Тип модели с информацией о новом статусе</typeparam>
        /// <exception cref="ApiServiceException">Ошибка при попытке смены на статус, который не относится к указанной сущности</exception>
        public static void ChangeState<TEntity, TStateInfo>(
            IStateProvider stateProvider,
            IDomainService<State> stateDomain,
            IDictionary<long, State> processedStatesDict,
            TEntity entity,
            TStateInfo newStateInfo,
            string notAvailableMessagePostFix = null,
            string notAvailableMessage = null)
            where TEntity : PersistentObject, IStatefulEntity
            where TStateInfo : BaseStateInfo
        {
            var newStateId = (long)newStateInfo.Id;

            if (entity.State.Id != newStateId)
            {
                if (!processedStatesDict.TryGetValue(newStateId, out var newState))
                {
                    newState = stateDomain.Get(newStateId);

                    if (entity.State.TypeId != newState.TypeId)
                    {
                        notAvailableMessage = notAvailableMessage ?? $"Статус с идентификатором {newState.Id} " +
                            $"не может быть присвоен для {notAvailableMessagePostFix}";

                        throw new ApiServiceException(notAvailableMessage);
                    }

                    processedStatesDict.Add(newStateId, newState);
                }

                stateProvider.ChangeState(entity.Id, newState.TypeId, newState);
            }
        }

        /// <summary>
        /// Получить для выборки информацию о статусе сущности
        /// </summary>
        /// <param name="state">Экземпляр статуса <see cref="State"/></param>
        public static StateInfoGet GetStateInfo(this State state) => new StateInfoGet(state);
    }
}