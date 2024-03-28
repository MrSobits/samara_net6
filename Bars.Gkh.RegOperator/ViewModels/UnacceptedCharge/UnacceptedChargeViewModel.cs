namespace Bars.Gkh.RegOperator.ViewModels.UnacceptedCharge
{
    using System.Linq;
    using B4;
    using DataResult;
    using Entities;
    using Gkh.Domain;

    public class UnacceptedChargeViewModel : BaseViewModel<UnacceptedCharge>
    {
        public override IDataResult List(IDomainService<UnacceptedCharge> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);
            
            var packetId = loadParam.Filter.GetAsId("packetId");

            if (packetId == 0)
            {
                packetId = baseParams.Params.GetAsId("packetId");
            }

            var dataQuery = domainService.GetAll()
                .Where(x => x.Packet.Id == packetId)
                .Select(x => new
                {
                    x.Id,
                    AccountNum = x.PersonalAccount.PersonalAccountNum,
                    x.ContragentAccountNumber,
                    x.AccountState,
                    x.CrFundFormationDecisionType,
                    AccountId = x.PersonalAccount.Id,
                    x.Charge,
                    ChargeTariff = x.ChargeTariff - x.TariffOverplus,
                    ChargeDt = x.TariffOverplus,
                    x.Penalty,
                    x.RecalcPenalty,
                    Recalc = x.RecalcByBaseTariff,
                    x.RecalcByDecision,
                    x.Description
                })
                .Filter(loadParam, this.Container);

	        var totalCount = dataQuery.Count();
            var data = dataQuery.Order(loadParam).Paging(loadParam).ToArray();

            var sumChargeTariff = data.Sum(x => x.ChargeTariff);
            var sumChargeDt = data.Sum(x => x.ChargeDt);
            var sumRecalc = data.Sum(x => x.Recalc);
            var sumRecalcDecision = data.Sum(x => x.RecalcByDecision);
            var sumPenalty = data.Sum(x => x.Penalty);
            var sumRecalcPenalty = data.Sum(x => x.RecalcPenalty);
            var chargeSum = data.Sum(x => x.Charge);

            return new ListSummaryResult(data, totalCount, 
                new
                {
                    ChargeTariff = sumChargeTariff,
                    ChargeDt = sumChargeDt,
                    Recalc = sumRecalc,
                    Penalty = sumPenalty,
                    Charge = chargeSum,
                    RecalcByDecision = sumRecalcDecision,
                    RecalcPenalty = sumRecalcPenalty
                });
        }
    }
}