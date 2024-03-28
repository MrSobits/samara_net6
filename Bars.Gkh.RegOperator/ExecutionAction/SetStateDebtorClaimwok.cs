namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Modules.ClaimWork.Contracts;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories;

    using Castle.Windsor;

    /// <summary>
    /// Проставить статус ПИР - "Начато исполнительное производство
    /// </summary>
    public class SetStateDebtorClaimwok : BaseExecutionAction
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис "Основание претензионно исковой работы для неплательщиков"
        /// </summary>
        public IDomainService<Lawsuit> LawsuitDomain { get; set; }

        /// <summary>
        /// Домен-сервис "Основание претензионно исковой работы"
        /// </summary>
        public IDomainService<BaseClaimWork> ClaimWorkDomain { get; set; }

        /// <summary>
        /// Провадер для работы со статусами
        /// </summary>
        public IStateProvider StateProvider { get; set; }

        /// <summary>
        /// Репозиторий для получения различных данных по статусам
        /// </summary>
        public IStateRepository StateRepository { get; set; }


        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description =>
            "Действие проставляет статус \"Начато исполнительное производство\" для претензионно-исковых работ, если значения поля \"Факт возбуждения\" = \"Возбуждено\""
            ;

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Проставить статус ПИР - \"Начато исполнительное производство\"";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;


        private BaseDataResult Execute()
        {
            var startedEnforcementState =
                this.StateRepository.GetAllStates<DebtorClaimWork>().FirstOrDefault(x => x.Name == ClaimWorkStates.StartedEnforcement);

            if (startedEnforcementState.IsNull())
            {
                return BaseDataResult.Error("Не найден статус \"Начато исполнительное производство\".");
            }

            this.Container.InTransaction(
                () =>
                {
                    this.LawsuitDomain.GetAll()
                        .Where(x => x.CbFactInitiated == LawsuitFactInitiationType.Initiated)
                        .ForEach(
                            lawsuit =>
                            {
                                lawsuit.ClaimWork.State = startedEnforcementState;
                                this.ClaimWorkDomain.Update(lawsuit.ClaimWork);
                            });
                });

            return new BaseDataResult();
        }
    }
}