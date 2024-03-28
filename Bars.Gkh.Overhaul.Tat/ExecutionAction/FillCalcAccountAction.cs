namespace Bars.Gkh.Overhaul.Tat.ExecutionAction
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;

    /// <summary>
    /// Заполнение расчетных счетов
    /// </summary>
    [Repeatable]
    public class FillCalcAccountAction : BaseMandatoryExecutionAction
    {
        /// <inheritdoc />
        public override string Name => "Заполнение расчетных счетов";

        /// <inheritdoc />
        public override string Description => "Действие заполняет расчетные счета из решений о способе формирования фонда";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        private IDataResult Execute()
        {
            var service = this.Container.Resolve<ICalcAccountOwnerDecisionService>();
            var specialRepos = this.Container.ResolveRepository<SpecialAccountDecision>();
            var regopRepos = this.Container.ResolveRepository<RegOpAccountDecision>();
            using (this.Container.Using(service, specialRepos, regopRepos))
            {
                var specResult = service.SaveSpecialCalcAccount(specialRepos.GetAll());
                var regopResult = service.SaveRegopCalcAccount(regopRepos.GetAll());
                var total = specResult.Data.ToInt() + regopResult.Data.ToInt();
                var message = $"{specResult.Message}{Environment.NewLine}{regopResult.Message}";
                return new BaseDataResult
                {
                    Success = specResult.Success && regopResult.Success,
                    Message = message,
                    Data = total
                };
            }
        }
    }
}