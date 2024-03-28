namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using DataResult;
    using Decisions.Nso.Entities;
    using DomainService.RealityObjectAccount;
    using Entities;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using Gkh.Enums.Decisions;

    public class RealityObjectSpecialOrRegOperatorAccountViewModel : BaseViewModel<RealityObjectSpecialOrRegOperatorAccount>
    {
        private readonly IDomainService<CreditOrgDecision> _crDecDomain;
        private readonly IBankAccountDataProvider _bankProvider;

        public RealityObjectSpecialOrRegOperatorAccountViewModel(IDomainService<CreditOrgDecision> crDecDomain,
            IBankAccountDataProvider bankProvider)
        {
            _crDecDomain = crDecDomain;
            _bankProvider = bankProvider;
        }

        public override IDataResult List(IDomainService<RealityObjectSpecialOrRegOperatorAccount> domainService, BaseParams baseParams)
        {
            var showAll = baseParams.Params.GetAs<bool>("showall", ignoreCase: true);
            var accountType = baseParams.Params.GetAs<CrFundFormationDecisionType>("accountType", ignoreCase: true);

            var loadParams = baseParams.GetLoadParam();

            var paymentAccountDomain = Container.ResolveDomain<RealityObjectPaymentAccount>();

            var data = domainService.GetAll()
                .Where(x => x.IsActive)
                .Where(x => x.AccountType == accountType)
                .WhereIf(!showAll && accountType == CrFundFormationDecisionType.SpecialAccount, x => x.RegOperator != null)
                .WhereIf(showAll && accountType == CrFundFormationDecisionType.SpecialAccount, x => true)
                .Join(paymentAccountDomain.GetAll(), account => account.RealityObjectChargeAccount.RealityObject, account => account.RealityObject, (x, y) => new
                {
                    x.Id,
                    x.IsActive,
                    x.RealityObjectChargeAccount.RealityObject.Address,
                    x.RealityObjectChargeAccount.AccountNumber,
                    Municipality = x.RealityObjectChargeAccount.RealityObject.Municipality.Name,
                    x.RealityObjectChargeAccount.ChargeTotal,
                    x.RealityObjectChargeAccount.PaidTotal,
                    Debt = x.RealityObjectChargeAccount.ChargeTotal - x.RealityObjectChargeAccount.PaidTotal,
                    AccountOwner = x.Contragent != null ? x.Contragent.Name : x.RegOperator,
                    Saldo = y.DebtTotal - y.CreditTotal,
                    PaymentAccountId = y.Id,
                    x.RealityObjectChargeAccount.RealityObject,
                    RoId = x.RealityObjectChargeAccount.RealityObject.Id,
                    x.RegOperator,
                    x.AccountType
                })
                .Filter(loadParams, Container)
                .Order(loadParams);

            var result = data.Paging(loadParams).ToList();

            var ids = result.Select(x => new RealityObject{Id = x.RoId});
            var numbers = _bankProvider.GetBankNumbersForCollection(ids);//GetDecisionCreditOrgNumber(ids);

            var resData = result.Select(x => new
            {
                x.Id,
                x.IsActive,
                x.Address,
                x.Municipality,
                x.ChargeTotal,
                x.PaidTotal,
                x.Debt,
                x.AccountOwner,
                x.Saldo,
                x.PaymentAccountId,
                x.RealityObject,
                AccountNumber = numbers.ContainsKey(x.RoId) ? numbers[x.RoId] : string.Empty
            });

            //TODO: переделать на обычный Sum
            var chargeSum = data.SafeSum(x => x.ChargeTotal);
            var paidSum = data.SafeSum(x => x.PaidTotal);
            var debtSum = data.SafeSum(x => x.Debt);
            var saldoSum = data.SafeSum(x => x.Saldo);

            return new ListSummaryResult(resData, data.Count(),
                new
                {
                    ChargeTotal = chargeSum,
                    PaidTotal = paidSum,
                    Debt = debtSum,
                    Saldo = saldoSum
                });
        }

        private Dictionary<long, string> GetDecisionCreditOrgNumber(IEnumerable<RealityObject> ros)
        {
            var ids = ros.Select(x => x.Id).ToList();

            return _crDecDomain.GetAll()
                .Where(x => ids.Contains(x.Protocol.RealityObject.Id))
                .Where(x => x.BankAccountNumber != null && x.BankAccountNumber != string.Empty)
                .OrderByDescending(x => x.StartDate)
                .Select(x => new
                {
                    RoId = x.Protocol.RealityObject.Id,
                    x.BankAccountNumber,
                    x.StartDate
                })
                .ToList()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => x.FirstOrDefault().Return(n => n.BankAccountNumber));
        } 
    }
}