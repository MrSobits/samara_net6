namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    public class UpdateRoSpecialAccountsAfterProtocolDeletionAction : BaseExecutionAction
    {
        public override string Description => "Исправление статуса специального счета дома после удаления протокола решения";

        public override string Name => this.Description;

        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var calcAccRoDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();
            var specAccDomain = this.Container.ResolveDomain<SpecialCalcAccount>();
            var decisionProtocolDomain = this.Container.ResolveDomain<RealityObjectDecisionProtocol>();
            var roDomain = this.Container.ResolveDomain<RealityObject>();
            var decisionService = this.Container.Resolve<IRealityObjectDecisionsService>();

            using (this.Container.Using(calcAccRoDomain, decisionProtocolDomain, decisionService))
            {
                // Дома с решением о спец счете
                var rosWithSpecAccDecision = decisionService
                    .GetActualDecisionForCollection<CrFundFormationDecision>(roDomain.GetAll(), true)
                    .Where(x => x.Value.Decision == CrFundFormationDecisionType.SpecialAccount)
                    .Select(x => x.Key.Id)
                    .ToList();

                // Получаем дома без актуальных протоколов решений
                var rosWithoutProtocol = roDomain.GetAll()
                    .Where(x => !rosWithSpecAccDecision.Contains(x.Id));

                // Обновляем статус спец счета
                calcAccRoDomain.GetAll()
                    .Where(x => x.Account.TypeAccount == TypeCalcAccount.Special)
                    .Where(x => rosWithoutProtocol.Any(r => x.RealityObject == r))
                    .Select(x => x.Account)
                    .ForEach(
                        x =>
                        {
                            var acc = x as SpecialCalcAccount;
                            if (acc != null)
                            {
                                acc.IsActive = false;
                                specAccDomain.Update(acc);
                            }
                        });
            }

            return new BaseDataResult();
        }
    }
}