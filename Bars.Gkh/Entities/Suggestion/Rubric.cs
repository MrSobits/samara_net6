namespace Bars.Gkh.Entities.Suggestion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4;
    using B4.Application;
    using B4.DataAccess;
    using B4.Utils;
    using B4.Utils.Annotations;
    using Castle.Windsor;
    using Domain;
    using Enums;
    using Exceptions;
    using Newtonsoft.Json;

    /// <summary>
    ///     Рубрика обращения граждан. Служит для группировки обращений и определяет процесс, по правилам
    ///     которого будут обрабатываться обращения.
    /// </summary>
    public class Rubric : BaseGkhEntity
    {
        private readonly List<Transition> _transitions;

        private IDomainService<SuggestionComment> _commentDomain;

        private IWindsorContainer Container
        {
            get { return ApplicationContext.Current.Container; }
        }

        private IDomainService<SuggestionComment> CommentDomain
        {
            get
            {
                return this._commentDomain ??
                       (this._commentDomain = ApplicationContext.Current.Container.ResolveDomain<SuggestionComment>());
            }
        }

        /// <summary>
        ///     Переходы, определяющие правила обработки обращений.
        /// </summary>
        [JsonIgnore]
        public virtual IEnumerable<Transition> Transitions
        {
            get { return this._transitions; }
            set
            {
                this._transitions.Clear();
                if (value != null)
                {
                    this._transitions.AddRange(value);
                }
            }
        }

        /// <summary>
        ///     Код
        /// </summary>
        public virtual int Code { get; protected set; }

        /// <summary>
        ///     Наименование
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        ///     Тип первого исполнителя
        /// </summary>
        public virtual ExecutorType FirstExecutorType { get; protected set; }

        /// <summary>
        ///     Активна
        /// </summary>
        public virtual bool IsActual { get; protected set; }

        /// <summary>
        ///     Срок(дни) автоматического перевода обращения в статус "Выполнено"
        /// </summary>
        public virtual int? ExpireSuggestionTerm { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Rubric;

            if (other == null) return false;
            if (ReferenceEquals(this, obj)) return true;

            return this.Id == other.Id;
        }

        /// <summary>
        ///     Сделать неактивной.
        /// </summary>
        /// <param name="nonactual">Неактивна.</param>
        public virtual void Deactualize(bool nonactual = true)
        {
            this.IsActual = !nonactual;
        }

        /// <summary>
        ///     Валидация процесса обработки обращений.
        /// </summary>
        public virtual IDataResult Validate()
        {
            ArgumentChecker.NotNull(this._transitions, "transitions");

            if (!this._transitions.Any())
            {
                return BaseDataResult.Error("В процессе должен присутсвовать хотя бы один переход.");
            }

            if (!this._transitions.Any(x => x.IsFirst))
            {
                return BaseDataResult.Error("Отсутствует начальный шаг процесса.");
            }

            if (this._transitions.Count(x => x.IsFirst) > 1)
            {
                return BaseDataResult.Error("Количество шагов с признаком Начальный больше 1.");
            }

            foreach (var transition in this._transitions)
            {
                var validation = transition.Validate();
                if (!validation.Success)
                {
                    return validation;
                }
            }

            var iterated = new HashSet<Transition>();
            var firstTransition = this._transitions.Single(x => x.IsFirst);
            iterated.Add(firstTransition);

            try
            {
                var next = this.NextTransition(firstTransition);
                while (next != null)
                {
                    if (iterated.Contains(next))
                    {
                        break;
                    }
                    iterated.Add(next);
                    next = this.NextTransition(next);
                }
            }
            catch (TransitionIterationException e)
            {
                return BaseDataResult.Error(e.Message);
            }

            if (!this._transitions.All(iterated.Contains))
            {
                return BaseDataResult.Error("В процессе присутсвуют недостижимые переходы.");
            }

            return new BaseDataResult();
        }

        /// <summary>
        ///     Обработка обращений из рубрики
        /// </summary>
        /// <param name="suggestions">Обращения</param>
        /// <param name="logManager">Лог менеджер</param>
        /// <returns>Успешность обработки</returns>
        public virtual IDataResult Run(IQueryable<CitizenSuggestion> suggestions, StringBuilder logManager)
        {
            var validation = this.Validate();

            if (!validation.Success)
            {
                return validation;
            }


            try
            {
                var transitions = this.Transitions.ToArray().ToDictionary(x => x.InitialExecutorType);

                foreach (var suggestion in suggestions)
                {
                    var comment = this.CommentDomain.GetAll()
                        .Where(x => x.CitizenSuggestion.Id == suggestion.Id)
                        .OrderByDescending(x => x.Id)
                        .FirstOrDefault();

                    var transition = transitions.Get(comment.GetCurrentExecutorType());

                    if (transition != null)
                    {
                        if (this.NextTransition(transition) != null)
                        {
                            var comment1 = comment;
                            ApplicationContext.Current.Container.InTransaction(() =>
                            {
                                comment1.ApplyTransition(transition);
                                this.CommentDomain.Update(comment1);
                            });

                            logManager.AppendFormat("Обращение: {0} успешно обработано с помощью перехода: {1}",
                                suggestion.Number, transition.Name);
                        }
                        else
                        {
                            logManager.AppendFormat(
                                "Обращение: {0} пропущено, т.к. уже находится на конечном переходе: {1}",
                                suggestion.Number, transition.Name);
                        }
                    }
                    else
                    {
                        logManager.AppendFormat("Обращение: {0} пропущено, т.к. не найден переход", suggestion.Number);
                    }
                }
            }
            catch (TransitionException e)
            {
                BaseDataResult.Error(e.Message);
            }

            return new BaseDataResult();
        }

        /// <summary>
        ///     Получение следующего шага в процессе обработки обращений.
        /// </summary>
        /// <param name="current">Текущий шаг обработки</param>
        /// <returns>Следующий шаг</returns>
        /// <exception cref="TransitionIterationException"></exception>
        public virtual Transition NextTransition(Transition current)
        {
            var domain = this.Container.ResolveDomain<Transition>();

            try
            {
                // В set свойства Transitions приходит пустой value иногда, поэтому получаем обработки из базы
                var next = this.Container.ResolveDomain<Transition>().GetAll()
                    .Where(x => x.Rubric.Id == current.Rubric.Id)
                    .Where(x => x.InitialExecutorType == current.TargetExecutorType).ToArray();

                if (next.Length == 0)
                {
                    return null;
                }

                if (next.Length > 1)
                {
                    throw new TransitionIterationException("В процессе присутствует ветвление.");
                }

                return next[0];
            }
            finally
            {
                this.Container.Release(domain);
            }
        }

        /// <summary>
        ///     Найти для обращения шаг обработки, в котором он находится
        /// </summary>
        protected virtual Transition FindCurrentTransitionFor(CitizenSuggestion suggestion,
            ExecutorType executorType = 0)
        {
            var execType = executorType > 0 ? executorType : suggestion.GetCurrentExecutorType();
            return this.Transitions.SingleOrDefault(x => x.InitialExecutorType == execType);
        }

        /// <summary>
        ///     Получить нулевой переход (начальный)
        /// </summary>
        public virtual Transition GetZeroTransition()
        {
            return new ZeroTransition(this);
        }

        /// <summary>
        ///     Начальный переход
        /// </summary>
        private class ZeroTransition : Transition
        {
            public ZeroTransition(Rubric rubric)
            {
                this.TargetExecutorType = rubric.FirstExecutorType;
                var firstTransition = rubric.Transitions.SingleOrDefault(x => x.IsFirst);

                this.ExecutionDeadline = firstTransition.Return(x => x.ExecutionDeadline);
                this.EmailSubject = firstTransition.Return(x => x.EmailSubject);
                this.EmailTemplate = firstTransition.Return(x => x.EmailTemplate);

                this.Rubric = rubric;
            }
        }

        public virtual Transition GetTestTransition()
        {
            return new TestTransition(this);
        }

        /// <summary>
        ///     Начальный переход
        /// </summary>
        private class TestTransition : Transition
        {
            public TestTransition(Rubric rubric)
            {
                var firstTransition = rubric.Transitions.SingleOrDefault(x => x.IsFirst);
                this.TargetExecutorType = firstTransition.InitialExecutorType;

                this.ExecutionDeadline = firstTransition.Return(x => x.ExecutionDeadline);
                this.EmailSubject = firstTransition.Return(x => x.EmailSubject);
                this.EmailTemplate = firstTransition.Return(x => x.EmailTemplate);

                this.Rubric = rubric;
            }
        }

        #region .ctor

        [Obsolete(
            "Используйте для создания конструктор с параметрами. Данный конструктор используется только NHibernate.")]
        public Rubric()
        {
            this._transitions = new List<Transition>();
        }

        public Rubric(int code, string name, ExecutorType firstExecutorType)
        {
            this.Code = code;
            this.Name = name;
            this.FirstExecutorType = firstExecutorType;
            this.IsActual = true;
            this._transitions = new List<Transition>();
        }

        #endregion
    }
}