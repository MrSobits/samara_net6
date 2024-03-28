namespace Bars.Gkh.RegOperator.DomainService.RegOperator
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Domain;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Decisions;
    using Bars.Gkh.RegOperator.DomainService.RealityObjectAccount.Impl;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    public class BankAccountDataProvider : EmptyBankDataProvider
    {
        private readonly IRealityObjectDecisionsService _decisionService;
        private readonly IDomainService<DecisionNotification> _decNotifDomain;
        private readonly IDomainService<CalcAccountRealityObject> _calcaccDomain;
        private readonly ISpecialCalcAccountService _specaccService;
        private readonly IRegopCalcAccountService _regopaccService;
        private readonly IRepository<RealityObject> _realObjRepo;

        public BankAccountDataProvider(
            IRealityObjectDecisionsService decisionService,
            IDomainService<DecisionNotification> decNotifDomain,
            IDomainService<CalcAccountRealityObject> calcaccDomain,
            ISpecialCalcAccountService specaccService,
            IRegopCalcAccountService regopaccService,
            IRepository<RealityObject> realObjRepo)
        {
            _decisionService = decisionService;
            _decNotifDomain = decNotifDomain;
            _calcaccDomain = calcaccDomain;
            _specaccService = specaccService;
            _regopaccService = regopaccService;
            _realObjRepo = realObjRepo;
        }

        public override string GetBankAccountNumber(RealityObject ro)
        {
            //var specAccount = _specaccService.GetRobjectActiveSpecialAccount(ro);
            var specAccount = _specaccService.GetSpecialAccount(ro);

            string accnum;
            if (specAccount != null)
            {
                accnum = specAccount.Return(x => x.AccountNumber);
            }
            else
            {
                accnum =
                    _regopaccService.GetRegopAccount(ro)
                        .Return(x => x.ContragentCreditOrg)
                        .Return(x => x.SettlementAccount);
            }

            // делаем так иначе не будут работать фильтры
            return accnum ?? string.Empty;
        }

        public override AccInfoProxy GetBankAccountInfo(long roId)
        {
            var ro = _realObjRepo.Load(roId);
            var specAccount = _specaccService.GetSpecialAccount(ro);

            AccInfoProxy accInfo;
            if (specAccount != null)
            {
                accInfo = new AccInfoProxy
                {
                    CalcAccId = specAccount.Return(x => x.Id),
                    BankAccountNum = specAccount.Return(x => x.AccountNumber),
                    OpenDate = specAccount.Return(x => x.DateOpen),
                    CloseDate = specAccount.Return(x => x.DateClose)
                };
            }
            else
            {
                var regopAccount = _regopaccService.GetRegopAccount(ro);
                accInfo = new AccInfoProxy
                {
                    CalcAccId = regopAccount.Return(x => x.Id),
                    BankAccountNum = regopAccount.Return(x => x.ContragentCreditOrg).Return(x => x.SettlementAccount),
                    OpenDate = regopAccount.Return(x => x.DateOpen),
                    CloseDate = regopAccount.Return(x => x.DateClose)
                };
            }

            return accInfo;
        }

        public override Dictionary<long, string> GetBankNumbersForCollection(IEnumerable<RealityObject> ros)
        {
            var roids = ros.Select(x => x.Id).ToList();

            var decisions = _decisionService.GetActualDecisionForCollection<CrFundFormationDecision>(ros, true);

            var onSpecAcc =
                decisions
                    .Where(x => x.Value.Decision == CrFundFormationDecisionType.SpecialAccount)
                    .Select(x => x.Key.Id).ToHashSet();

            var protocolIds = decisions.Values.Select(x => x.Protocol.Id).ToList();

            var notifs = _decNotifDomain.GetAll().Where(x => protocolIds.Contains(x.Protocol.Id))
                .GroupBy(x => x.Protocol.RealityObject.Id)
                .ToList()
                .Select(x => new
                {
                    Ro = x.Key,
                    Notif = x.OrderByDescending(n => n.Protocol.ProtocolDate).FirstOrDefault(n => n.AccountNum.IsNotEmpty())
                })
                .ToList()
                .ToDictionary(x => x.Ro, x => x.Notif);

            var calcAccounts =
                _calcaccDomain.GetAll()
                              .Where(x => roids.Contains(x.RealityObject.Id))
                              .Where(x => x.Account is RegopCalcAccount)
                              .Select(
                                  x =>
                                  new
                                  {
                                      x.RealityObject.Id,
                                      ((RegopCalcAccount)x.Account).ContragentCreditOrg.SettlementAccount
                                  })
                              .AsEnumerable()
                              .GroupBy(x => x.Id)
                              .ToDictionary(x => x.Key, x => x.First().SettlementAccount);

            var result = new Dictionary<long, string>();

            foreach (var ro in ros.ToList())
            {
                if (onSpecAcc.Contains(ro.Id))
                {
                    DecisionNotification notif;
                    notifs.TryGetValue(ro.Id, out notif);

                    result.Add(ro.Id, notif.Return(x => x.AccountNum));
                }
                else
                {
                    var acc = calcAccounts.Get(ro.Id);

                    result.Add(ro.Id, acc);
                }
            }

            return result;
        }
    }
}