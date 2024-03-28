using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;
using System.Linq;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.ViewModel
{
    class AgentPIRDebtorViewModel : BaseViewModel<AgentPIRDebtor>
    {
        public override IDataResult List(IDomainService<AgentPIRDebtor> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var agentPIRId = loadParams.Filter.GetAs("agentPIRId", 0L);

            var creditDict = this.Container.Resolve<IDomainService<AgentPIRDebtorCredit>>().GetAll()
                .Where(x => x.Debtor.AgentPIR.Id == agentPIRId)
                .GroupBy(x => x.Debtor.Id)
                .ToDictionary(x => x.Key, x => x.Sum(y => y.Credit));

            var data = domain.GetAll()
                .Where(x => x.AgentPIR.Id == agentPIRId)
                .Select(x => new
                {
                    x.Id,
                    x.Status,
                    BasePersonalAccount = x.BasePersonalAccount.AccountOwner.OwnerType == Enums.PersonalAccountOwnerType.Legal ? x.BasePersonalAccount.AccountOwner.Name : "* * *",
                    Address = x.BasePersonalAccount.Room.RealityObject.Address + ", кв." + x.BasePersonalAccount.Room.RoomNum,
                    x.BasePersonalAccount.PersonalAccountNum,
                    x.BasePersonalAccount.UnifiedAccountNumber,
                    x.DebtStartDate,
                    x.DebtEndDate,
                    x.DebtBaseTariff,
                    x.PenaltyDebt,
                    x.Ordering,
                    Credit = creditDict.ContainsKey(x.Id) ? creditDict[x.Id] : 0,
                    State = x.BasePersonalAccount.State.Name,
                    x.BasePersonalAccount.AreaShare
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Status,
                    x.BasePersonalAccount,
                    x.Address,
                    x.PersonalAccountNum,
                    x.UnifiedAccountNumber,
                    x.DebtStartDate,
                    x.DebtEndDate,
                    x.DebtBaseTariff,
                    x.PenaltyDebt,
                    x.Ordering,
                    x.Credit,
                    x.State,
                    x.AreaShare
                })
                .AsQueryable()
                .Filter(loadParams, Container);



            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<AgentPIRDebtor> domain, BaseParams baseParams)
        {
            var debtor = domain.Get(baseParams.Params["id"].To<long>());

            var credit = this.Container.Resolve<IDomainService<AgentPIRDebtorCredit>>().GetAll()
                .Where(x => x.Debtor.Id == debtor.Id)
                .Select(x => x.Credit)
                .Sum();

            var debFio = debtor.BasePersonalAccount.AccountOwner.Name.Trim();

            return new BaseDataResult(new
            {
                debtor.Id,
                debtor.Status,
                BasePersonalAccount = debtor.BasePersonalAccount.AccountOwner.OwnerType == Enums.PersonalAccountOwnerType.Legal ? debtor.BasePersonalAccount.AccountOwner.Name : "* * *",
                BasePersonalAccountId = debtor.BasePersonalAccount.Id,
                Address = debtor.BasePersonalAccount.Room.RealityObject.Address + ", кв." + debtor.BasePersonalAccount.Room.RoomNum,
                debtor.BasePersonalAccount.PersonalAccountNum,
                debtor.BasePersonalAccount.UnifiedAccountNumber,
                debtor.DebtStartDate,
                debtor.DebtEndDate,
                debtor.DebtBaseTariff,
                debtor.PenaltyDebt,
                debtor.Ordering,
                debtor.DebtCalc,
                debtor.PenaltyCharge,
                Credit = credit == null ? 0 : credit,
                Fio = debFio.Split(' ').Length > 3 ? debFio : debFio.Substring(0, 1) + " " + debFio.Substring(debFio.Length - 3, 1) + " " + debFio.Substring(debFio.Length - 1, 1)
            });
        }
    }
}
