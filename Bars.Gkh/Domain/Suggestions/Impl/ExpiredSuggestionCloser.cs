namespace Bars.Gkh.Domain.Suggestions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities.Suggestion;
    using Castle.Windsor;

    /// <summary>
    /// Сервис закрытия обращений, по которым прошел срок ожидания.
    /// На ожидании считаются обращения, которые не закрыты (статус с кодом "end") и находятся в конечном статусе.
    /// </summary>
    public class ExpiredSuggestionCloser : IExpiredSuggestionCloser
    {
        private readonly IWindsorContainer _container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">IoC-Контейнер</param>
        public ExpiredSuggestionCloser(IWindsorContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Закрытие обращений, по которым прошел срок ожидания.
        /// На ожидании считаются обращения, которые не закрыты (статус с кодом "end") и находятся в конечном статусе.
        /// </summary>
        /// <param name="waitDays">Срок ожидания в днях</param>
        public void Close(int waitDays)
        {
            var stateHistoryDomain = _container.ResolveDomain<StateHistory>();
            var suggDomain = _container.ResolveDomain<CitizenSuggestion>();
            var stateProvider = _container.Resolve<IStateProvider>();
            var stateRepo = _container.Resolve<IRepository<State>>();

            using (_container.Using(stateHistoryDomain, suggDomain, stateProvider, stateProvider, stateRepo))
            {
                var typeInfo = stateProvider.GetStatefulEntityInfo(typeof(CitizenSuggestion));
                var waitingStatesIds = GetWaitingStates(stateRepo, typeInfo).Select(x => x.Id).ToArray();

                var sentDayStart = DateTime.Now.Date.AddDays(-1 - waitDays);
                var sentDayEnd = sentDayStart.AddHours(23).AddMinutes(59).AddSeconds(59);

                var suggestionsInWaitingState = suggDomain.GetAll()
                    .Where(x => x.State != null && waitingStatesIds.Contains(x.State.Id))
                    .Join(stateHistoryDomain.GetAll(),
                        suggestion => suggestion.Id,
                        history => history.EntityId,
                        (suggestion, history) => new
                        {
                            history.ChangeDate,
                            suggestion.Id
                        })
                    .Where(x => x.ChangeDate >= sentDayStart)
                    .Where(x => x.ChangeDate <= sentDayEnd).ToList();

                var closeState = GetCloseState(stateRepo, typeInfo);

                foreach (var sugg in suggestionsInWaitingState)
                {
                    stateProvider.ChangeState(sugg.Id, typeInfo.TypeId, closeState,
                        string.Format("Закрыто по истечении срока ожидания ({0} дней)", waitDays));
                }
            }
        }

        /// <summary>
        /// Если обращение находится в одном из этих статусов, считаем, что оно на обработке на портале.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<State> GetWaitingStates(IRepository<State> stateRepo, StatefulEntityInfo typeInfo)
        {
            return stateRepo.GetAll()
                .Where(x => x.TypeId == typeInfo.TypeId)
                .Where(x => x.FinalState).ToList()
                .Where(x => x.Code.ToLower() != "end");


        }

        /// <summary>
        /// Получение статуса-закрытия
        /// </summary>
        /// <param name="stateRepo"></param>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        private State GetCloseState(IRepository<State> stateRepo, StatefulEntityInfo typeInfo)
        {
            return stateRepo.GetAll()
                .Where(x => x.TypeId == typeInfo.TypeId)
                .FirstOrDefault(x => x.Code.ToLower() == "end");
        }
    }
}
