namespace Bars.Gkh.Entities.Suggestion
{
    using System;
    using System.Linq;
    using B4.Application;
    using B4.DataAccess;
    using B4.Utils;
    using B4.Utils.Annotations;

    using Bars.Gkh.Utils.EntityExtensions;

    using Castle.Windsor;
    using Domain.Suggestions;
    using DomainService;
    using Enums;

    /// <summary>
    /// Класс методов расширения сущности Обращения граждан
    /// </summary>
    public partial class CitizenSuggestion
    {
        private IWindsorContainer Container
        {
            get { return ApplicationContext.Current.Container; }
        }

        /// <summary>
        /// Получить тип текущего исполнителя
        /// </summary>
        /// <returns>Текущий исполнитель ображения</returns>
        public virtual ExecutorType GetCurrentExecutorType()
        {
            return ExecutorCrFund != null
                ? ExecutorType.CrFund
                : ExecutorManagingOrganization != null
                    ? ExecutorType.Mo
                    : ExecutorMunicipality != null
                        ? ExecutorType.Mu
                        : ExecutorZonalInspection != null ? ExecutorType.Gji : Rubric.FirstExecutorType;
        }

        /// <summary>
        /// Применить правило перехода
        /// </summary>
        /// <param name="transition">Переход, который необходимо провести</param>
        /// <param name="rubric">Текущая рубрика обращения</param>
        public virtual void ApplyTransition(Transition transition, Rubric rubric = null)
        {
            ArgumentChecker.NotNull(transition, "transition");

            ApplyExecutor(transition);

            if (this.GetExecutor(transition.TargetExecutorType) == null)
            {
                if (rubric != null)
                {
                    var exceptionThrown = false;
                    var nullExecutor = true;

                    while (!exceptionThrown && nullExecutor)
                    {
                        try
                        {
                            transition = rubric.NextTransition(transition);
                            ApplyExecutor(transition);
                            nullExecutor = this.GetExecutor(transition.TargetExecutorType) == null;
                        }
                        catch (Exception)
                        {
                            exceptionThrown = true;
                        }
                        
                    }
                }
            }

            if (transition != null)
            {
                ApplyDeadline(transition);

                SuggestionChangeHandler.ApplyChange(this, transition);
            }
        }

        /// <summary>
        /// Установка исполнителя по целевому исполнителю
        /// </summary>
        /// <param name="transition">Переход обращения</param>
        protected virtual void ApplyExecutor(Transition transition)
        {
            switch (transition.TargetExecutorType)
            {
                case ExecutorType.Gji:
                    ExecutorCrFund = null;
                    ExecutorManagingOrganization = null;
                    ExecutorMunicipality = null;
                    ExecutorZonalInspection = GetExecutorZonalInspection();
                    break;
                case ExecutorType.Mo:
                    ExecutorCrFund = null;
                    ExecutorMunicipality = null;
                    ExecutorZonalInspection = null;
                    ExecutorManagingOrganization = GetExecutorManagingOrganization();
                    break;
                case ExecutorType.Mu:
                    ExecutorCrFund = null;
                    ExecutorManagingOrganization = null;
                    ExecutorZonalInspection = null;
                    ExecutorMunicipality = RealityObject.Municipality;
                    break;
                case ExecutorType.CrFund:
                    ExecutorManagingOrganization = null;
                    ExecutorZonalInspection = null;
                    ExecutorMunicipality = null;
                    ExecutorCrFund = GetExecutorExecutorCrFund();
                    break;
            }
        }

        /// <summary>
        /// Установить срок исполнения
        /// </summary>
        /// <param name="transition">Переход обращения граждан с одного состояния в другое</param>
        protected virtual void ApplyDeadline(Transition transition)
        {
            var nextTransition = NextTransition(transition);

            if (nextTransition != null)
            {
                Deadline = DateTime.Now.AddDays(nextTransition.Return(x => x.ExecutionDeadline));
            }
        }

        /// <summary>
        /// Получить следующий шаг
        /// </summary>
        /// <param name="transition">Переход обращения</param>
        /// <returns>Новый переход обращения</returns>
        protected virtual Transition NextTransition(Transition transition)
        {
            return Container.ResolveDomain<Transition>().GetAll()
                .Where(x => x.Rubric.Id == transition.Rubric.Id)
                .FirstOrDefault(x => x.InitialExecutorType == transition.TargetExecutorType);
        }

        /// <summary>
        /// Получить исполнителя "Управляющая компания"
        /// </summary>
        /// <returns>Исполняющая управляющая организация</returns>
        protected virtual ManagingOrganization GetExecutorManagingOrganization()
        {
            var manorgReality = Container.Resolve<IManagingOrgRealityObjectService>().GetCurrentManOrg(RealityObject);
            return manorgReality != null ? manorgReality.ManagingOrganization : null;
        }

        /// <summary>
        /// Получить исполнителя "Зональная жилищная инспекция"
        /// </summary>
        /// <returns>Исполняющая зональная жилиьщная испекция</returns>
        protected virtual ZonalInspection GetExecutorZonalInspection()
        {
            var zonalInsp = Container.ResolveDomain<ZonalInspectionMunicipality>().GetAll()
                .FirstOrDefault(x => x.Municipality.Id == RealityObject.Municipality.Id && x.ZonalInspection != null);
            return zonalInsp.Return(x => x.ZonalInspection);
        }

        /// <summary>
        /// Получить исполнителя "Фонд КР"
        /// </summary>
        /// <returns>Исполняющий контрагент фонда КР</returns>
        protected virtual ContragentContact GetExecutorExecutorCrFund()
        {
            //Ищем исполнителя, чья должность имеет код 1, чаще всего это Ген. Директор
            return Container.ResolveDomain<ContragentContact>().GetAll().FirstOrDefault(x => x.Position.Code == "1");
        }
    }
}