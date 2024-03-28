namespace Bars.Gkh.Decisions.Nso.Domain.Impl
{
    using System.Collections.Generic;

    using B4;
    using Domain;
    using Entities;
    using Authentification;

    public class MonthlyFeeAmountDecisionService : IMonthlyFeeAmountDecisionService
    {
        public IDomainService<MonthlyFeeAmountDecHistory> MonthlyFeeHistoryDomain { get; set; }
        public IDomainService<RealityObjectDecisionProtocol> RoDecProtocolDomain { get; set; }
        public IGkhUserManager UserManager { get; set; }

        public IDataResult SaveHistory(BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAs<long>("protocolId");
            var records = baseParams.Params.GetAs<List<PeriodMonthlyFee>>("recs");

            var user = UserManager.GetActiveOperator();

            MonthlyFeeHistoryDomain.Save(new MonthlyFeeAmountDecHistory
            {
                Decision = records,
                UserName = user != null ? user.Name : "Администратор",
                Protocol = RoDecProtocolDomain.Load(protocolId)
            });

            return new BaseDataResult();
        }
    }
}