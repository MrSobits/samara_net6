namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Enums.Decisions;

    using DataResult;
    using DomainService;
    using Entities;
    using Enums;
    using Gkh.Domain;

    public class SpecialCalcAccountViewModel : BaseViewModel<SpecialCalcAccount>
    {
        public override IDataResult List(IDomainService<SpecialCalcAccount> domainService, BaseParams baseParams)
        {
            var calcaccService = this.Container.Resolve<ICalcAccountService>();
            var accountroDomain = this.Container.ResolveDomain<CalcAccountRealityObject>();
            var crFundDecisDomain = this.Container.ResolveDomain<CrFundFormationDecision>();
            var accOwnDecisDomain = this.Container.ResolveDomain<AccountOwnerDecision>();
            var roDecisionProtocolDomainService = this.Container.ResolveDomain<RealityObjectDecisionProtocol>();
            var creditOrgDecisionDomainService = this.Container.ResolveDomain<CreditOrgDecision>();

            try
            {
                var loadParams = baseParams.GetLoadParam();

                var typeOwner = baseParams.Params.GetAs<TypeOwnerCalcAccount>("typeOwner");
                var ownerId = baseParams.Params.GetAsId("ownerId");

                var crFundQuery = crFundDecisDomain.GetAll()
                    .Where(x => x.Decision == CrFundFormationDecisionType.SpecialAccount);
                var accOwnQuery = accOwnDecisDomain.GetAll()
                    .Where(x => x.DecisionType == AccountOwnerDecisionType.RegOp);

                var realiesWithDecisions =
                    roDecisionProtocolDomainService.GetAll()
                    .Join(crFundDecisDomain.GetAll(),
                        protocol => protocol.Id,
                        decision => decision.Protocol.Id,
                        (protocol, decision) => new
                        {
                            Protocol = protocol,
                            Decision = decision
                        })
                    .ToList()
                    .GroupBy(x => x.Protocol)
                    .Select(group => new
                    {
                        Protocol = group.Key,
                        LatestDecision =
                            group
                                .OrderByDescending(y => y.Decision.StartDate)
                                .First()
                                .Decision
                    })
                    .Where(x => x.LatestDecision.Decision == CrFundFormationDecisionType.SpecialAccount)
                    .Join(creditOrgDecisionDomainService.GetAll(),
                        join => join.Protocol.Id,
                        decision => decision.Protocol.Id,
                        (join, decision) => new
                        {
                            join.Protocol.RealityObject,
                            Decision = decision
                        })
                    .ToList()
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.Decision.StartDate)
                            .First()
                            .Decision
                            .Decision);

                var accroQuery = accountroDomain.GetAll()
                    .WhereIf(ownerId > 0, x => x.Account.AccountOwner.Id == ownerId)
                    .WhereIf(typeOwner != 0, x => x.Account.TypeOwner == typeOwner)
                    .Where(x => x.Account.TypeAccount == TypeCalcAccount.Special)
                    .Where(x => crFundQuery.Any(y => y.Protocol.RealityObject.Id == x.RealityObject.Id))
                    .Where(x => accOwnQuery.Any(y => y.Protocol.RealityObject.Id == x.RealityObject.Id));

                var dictOwners = accroQuery
                    .Select(x => new
                    {
                        x.Account.Id,
                        x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name,
                        RoId = x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => new { x.Address, x.Municipality, x.RoId }).First());

                var dictMoney = calcaccService.GetAccountSummary(accroQuery);

                var data = domainService.GetAll()
                    .Where(x => x.IsActive)
                    .WhereIf(ownerId > 0, x => x.AccountOwner.Id == ownerId)
                    .WhereIf(typeOwner != 0, x => x.TypeOwner == typeOwner)
                    .Select(x => new
                    {
                        x.Id,
                        x.AccountNumber,
                        x.TotalIn,
                        x.TotalOut,
                        CreditOrg = x.CreditOrg.Name,
                        x.DateOpen,
                        x.DateClose
                    })
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var roId = dictOwners.Get(x.Id)?.RoId ?? 0;
                        return new
                        {
                            x.Id,
                            AccountNumber = x.AccountNumber ?? string.Empty,
                            x.TotalIn,
                            x.TotalOut,
                            ContragentCreditOrg = (realiesWithDecisions.ContainsKey(roId)
                                ? realiesWithDecisions[roId]?.Name
                                : x.CreditOrg)
                            ?? string.Empty,
                            x.DateOpen,
                            x.DateClose,
                            Debt = dictMoney.Get(x.Id).Return(y => y.Debt),
                            Credit = dictMoney.Get(x.Id).Return(y => y.Credit),
                            Saldo = dictMoney.Get(x.Id).Return(y => y.Saldo),
                            Address = dictOwners.Get(x.Id).Return(y => y.Address, string.Empty),
                            Municipality = dictOwners.Get(x.Id).Return(y => y.Municipality)
                        };
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListSummaryResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(calcaccService);
                this.Container.Release(accountroDomain);
                this.Container.Release(crFundDecisDomain);
                this.Container.Release(accOwnDecisDomain);
                this.Container.Release(roDecisionProtocolDomainService);
                this.Container.Release(creditOrgDecisionDomainService);
            }
        }
    }
}