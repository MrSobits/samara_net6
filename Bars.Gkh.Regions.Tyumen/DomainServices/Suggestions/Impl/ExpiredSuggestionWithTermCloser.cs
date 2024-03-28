namespace Bars.Gkh.Regions.Tyumen.DomainServices.Suggestions.Impl
{
    using System;
    using System.Linq;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.States;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;
    using Entities.Suggestion;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Данный автозакрывальщик обращений реализует логику которая пришла для Тюмени.
    ///     А именно - в окне редактирования рубрики есть поле "Закрыть обращение через" и 
    ///     это количество дней через которое(+ контрольный срок) это обращение при отсутвии реакции(статус "Не подверждено")
    ///     должно автоматически закрываться
    /// </summary>
    public class ExpiredSuggestionWithTermCloser : IExpiredSuggestionWithTermCloser
    {
        private readonly IWindsorContainer container;
        private readonly ILogger logManager;

        public ExpiredSuggestionWithTermCloser(IWindsorContainer container,
            ILogger logManager)
        {
            this.container = container;
            this.logManager = logManager;
        }

        public void Close()
        {
            /* 
             * В Тюмени немного другая логика раьботы с обращениями, поэтому при вызове из базового модуля
             * задачи шедулера - подменяем реализацию на тюменскую с проверкой поля рубрики "Закрыть обращение через"
            */
            var stateHistoryDomain = this.container.ResolveDomain<StateHistory>();
            var suggDomain = this.container.ResolveDomain<CitizenSuggestion>();
            var stateProvider = this.container.Resolve<IStateProvider>();
            var stateRepo = this.container.Resolve<IRepository<State>>();
            var commentDomain = this.container.ResolveDomain<SuggestionComment>();
            try
            {
                using (container.BeginScope())
                {
                    using (this.container.Using(stateHistoryDomain, suggDomain, stateProvider, stateProvider, stateRepo))
                    {
                        var typeInfo = stateProvider.GetStatefulEntityInfo(typeof(CitizenSuggestion));
                        var waitingStates = this.GetWaitingStates(stateRepo, typeInfo);

                        var closeState = this.GetCloseState(stateRepo, typeInfo);
                        var now = DateTime.Now;

                        var getSuggestionsChangeTo = commentDomain.GetAll()
                           .Where(x => x.CitizenSuggestion.State != null && x.CitizenSuggestion.State == waitingStates)
                           .Where(x => x.CitizenSuggestion.Rubric.ExpireSuggestionTerm != null && x.AnswerDate != null)
                           .AsEnumerable()
                           .Select(x => new
                           {
                               x.CitizenSuggestion.Id,
                               AnswerDate = x.AnswerDate.Value,
                               ExpireDays = x.CitizenSuggestion.Rubric.ExpireSuggestionTerm.Value
                           });

                        foreach (var sugg in getSuggestionsChangeTo)
                        {
                            if (sugg.AnswerDate.AddDays(sugg.ExpireDays) < now)
                            {
                                try
                                {
                                    stateProvider.ChangeState(sugg.Id, typeInfo.TypeId, closeState,
                                        string.Format("Закрыто по истечении срока ожидания ({0} дней)", sugg.ExpireDays), false);
                                }
                                catch (Exception e)
                                {
                                    this.logManager.LogError(e, "При изменении статуса обращения произошла ошибка");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                this.logManager.LogError(e, "Ошибка при выполнении задачи ExpiredSuggestionWithTermCloser");
            }
            finally
            {
                this.container.Release(stateHistoryDomain);
                this.container.Release(suggDomain);
                this.container.Release(stateProvider);
                this.container.Release(stateRepo);
                this.container.Release(commentDomain);
            }

        }

        /// <summary>
        /// Если обращение находится в одном из этих статусов, считаем, что оно на обработке на портале.
        /// </summary>обращение с данным статусом будут подготавливаться к закрытию 
        /// <param name="stateRepo"></param>
        /// <param name="typeInfo"></param>
        /// <returns>Статус</returns> 
        private State GetWaitingStates(IRepository<State> stateRepo, StatefulEntityInfo typeInfo)
        {
            var getWaitingStates = stateRepo.GetAll()
                    .Where(x => x.TypeId == typeInfo.TypeId)
                    .FirstOrDefault(x => x.Name.ToLower() == "на подтверждении");
            return getWaitingStates;
        }

        /// <summary>
        /// Получение статуса-закрытия
        /// </summary>статус с который присвоят обращение после его закрытия
        /// <param name="stateRepo"></param>
        /// <param name="typeInfo"></param>
        /// <returns> Статус </returns> 
        private State GetCloseState(IRepository<State> stateRepo, StatefulEntityInfo typeInfo)
        {
            var getCloseState = stateRepo.GetAll()
                .Where(x => x.TypeId == typeInfo.TypeId)
                .FirstOrDefault(x => x.Name.ToLower() == "выполнено");
            return getCloseState;
        }
    }
}
