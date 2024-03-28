namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.Entities;

    public class SetRealtyAccountNonActiveAction : BaseExecutionAction
    {
        public override string Description
            => "Изменение признака использования специального счета на \"Не активен\" у домов с отсутствующими протоколами решений (КАМЧАТКА)";

        public override string Name => "Изменение признака использования специального счета (КАМЧАТКА)";

        public override Func<IDataResult> Action => this.SetRealtyAccountNonActive;

        private BaseDataResult SetRealtyAccountNonActive()
        {
            var realityObjectDecisionProtocolDomain = this.Container.Resolve<IDomainService<RealityObjectDecisionProtocol>>();
            var accountDomain = this.Container.Resolve<IDomainService<SpecialCalcAccount>>();
            var realityObjectDomain = this.Container.Resolve<IDomainService<RealityObject>>();
            var calcaccroDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();

            using (this.Container.Using(realityObjectDecisionProtocolDomain, accountDomain, realityObjectDomain, calcaccroDomain))
            {
                var housesWithoutDecisionProtocolQuery = calcaccroDomain.GetAll()
                    .Where(x => !realityObjectDecisionProtocolDomain.GetAll().Any(y => y.RealityObject.Id == x.RealityObject.Id))
                    .Select(x => x.Account.Id);

                accountDomain.GetAll()
                    .Where(x => housesWithoutDecisionProtocolQuery.Contains(x.Id))
                    .Where(x => x.IsActive)
                    .ForEach(
                        x =>
                        {
                            x.IsActive = false;
                            accountDomain.Update(x);
                        });
            }

            return new BaseDataResult();
        }
    }
}