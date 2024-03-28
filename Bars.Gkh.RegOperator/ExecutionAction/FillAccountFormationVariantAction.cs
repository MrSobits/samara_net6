namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.RegOperator.DomainModelServices;

    public class FillAccountFormationVariantAction : BaseExecutionAction
    {
        public override string Description => "Заполнение колонки вариантов формирования счета в таблице домов";

        public override string Name => "Заполнение вариантов формирования счета для домов";

        public override Func<IDataResult> Action => this.FillAccountFormationVariant;

        public BaseDataResult FillAccountFormationVariant()
        {
            var personalAccountFilterService = this.Container.Resolve<IPersonalAccountFilterService>();
            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            var results = new List<BaseDataResult>();

            var roIdsWithoutProtocol = personalAccountFilterService.GetDecisionFilteredRoCrFundType(
                CrFundFormationType.NotSelected);
            results.Add(
                this.FillRoAccountFormationVariantColumn(
                    sessionProvider,
                    roIdsWithoutProtocol,
                    CrFundFormationType.NotSelected));

            var roIdsCrNotRegOp = personalAccountFilterService.GetDecisionFilteredRoCrFundType(
                CrFundFormationType.SpecialAccount);
            results.Add(
                this.FillRoAccountFormationVariantColumn(
                    sessionProvider,
                    roIdsCrNotRegOp,
                    CrFundFormationType.SpecialAccount));

            var roIdsCrFundRegOpSpecialAcc = personalAccountFilterService.GetDecisionFilteredRoCrFundType(
                CrFundFormationType.SpecialRegOpAccount);
            results.Add(
                this.FillRoAccountFormationVariantColumn(
                    sessionProvider,
                    roIdsCrFundRegOpSpecialAcc,
                    CrFundFormationType.SpecialRegOpAccount));

            var roIdsCrFundRegOpAcc = personalAccountFilterService.GetDecisionFilteredRoCrFundType(
                CrFundFormationType.RegOpAccount);
            results.Add(
                this.FillRoAccountFormationVariantColumn(
                    sessionProvider,
                    roIdsCrFundRegOpAcc,
                    CrFundFormationType.RegOpAccount));

            sessionProvider.Dispose();
            foreach (var result in results.Where(result => !result.Success))
            {
                return result;
            }

            return new BaseDataResult();
        }

        private BaseDataResult FillRoAccountFormationVariantColumn(
            ISessionProvider sessionProvider,
            List<long> roIdList,
            CrFundFormationType crFundFormation)

        {
            string errorMsg = null;
            if (roIdList.Any())
            {
                try
                {
                    using (var session = sessionProvider.OpenStatelessSession())
                    {
                        using (var tr = session.BeginTransaction())
                        {
                            session.CreateQuery(
                                "update RealityObject set AccountFormationVariant =:val where Id in (:ids)")
                                .SetInt32("val", (int) crFundFormation)
                                .SetParameterList("ids", roIdList)
                                .ExecuteUpdate();

                            tr.Commit();
                        }
                    }
                }
                catch (Exception exc)
                {
                    errorMsg = exc.Message;
                }
            }
            return new BaseDataResult(errorMsg == null, errorMsg);
        }
    }
}