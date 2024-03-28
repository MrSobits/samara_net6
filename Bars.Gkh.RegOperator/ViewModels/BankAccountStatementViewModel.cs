namespace Bars.Gkh.RegOperator.ViewModels
{
    using System;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Enums;
    using Gkh.Domain;

    public class BankAccountStatementViewModel : BaseViewModel<BankAccountStatement>
    {
        /// <summary>
        /// Домен-сервис <see cref="CalcAccount"/>
        /// </summary>
        public IDomainService<CalcAccount> CalcAccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="CalcAccountRealityObject"/>
        /// </summary>
        public IDomainService<CalcAccountRealityObject> CalcAccountRoDomain { get; set; }

        public override IDataResult List(IDomainService<BankAccountStatement> domainService, BaseParams baseParams)
        {
            var groupId = baseParams.Params.GetAsId("groupId");
            var showDistributed = baseParams.Params.GetAs<bool>("showDistributed");
            var showCancelled = baseParams.Params.GetAs<bool>("showDeleted");
            var accountNumbersString = baseParams.Params.GetAs("specAccountIds", string.Empty);
            var dateReceiptFrom = baseParams.Params.GetAs<DateTime>("dateReceiptFrom");
            var dateReceiptBy = baseParams.Params.GetAs<DateTime>("dateReceiptBy");
            var addAnApostrophe = baseParams.Params.GetAs<bool>("addAnApostrophe", ignoreCase: true);
            long[] specAccountIds = null;
            var allSpecAccounts = false;

            if (accountNumbersString.ToUpper() != "ALL")
            {
                specAccountIds = accountNumbersString.ToLongArray();
            }
            else
            {
                allSpecAccounts = true;
            }

            var loadParam = this.GetLoadParam(baseParams);

            IQueryable<string> accountQuery = null;
            if (specAccountIds.IsNotEmpty() || allSpecAccounts)
            {
                accountQuery = this.CalcAccountDomain.GetAll()
                    .WhereIf(specAccountIds.IsNotEmpty(), x => specAccountIds.Contains(x.Id))
                    .Where(x => x.AccountNumber != null && x.AccountNumber != string.Empty && x.TypeAccount == TypeCalcAccount.Special)
                    .Where(x => this.CalcAccountRoDomain.GetAll().Any(y => y.Account == x))
                    .Select(x => x.AccountNumber);
            }

            var query = domainService.GetAll();

            if (groupId == 0)
            {
                query = query
                    .WhereIf(!showDistributed, x => x.DistributeState != DistributionState.Distributed)
                    .WhereIf(!showCancelled, x => x.DistributeState != DistributionState.Deleted);
            }

            var data = query
                .WhereIf(groupId > 0, x => x.Group.Id == groupId)
                .WhereIf(accountQuery != null, x => accountQuery.Any(y => y == x.RecipientAccountNum))
                .WhereIf(dateReceiptFrom != DateTime.MinValue, x=>x.DateReceipt >= dateReceiptFrom)
                .WhereIf(dateReceiptBy != DateTime.MinValue, x=>x.DateReceipt <= dateReceiptBy)
                .Select(x => new
                {
                    x.Id,
                    x.LinkedDocuments,
                    x.MoneyDirection,
                    x.OperationDate,
                    x.PayerFull,
                    PayerName = x.Payer != null ? x.Payer.Name : x.PayerName,
                    x.PayerInn,
                    RecipientName = x.Recipient != null ? x.Recipient.Name : x.RecipientName,
                    PayerAccountNum = addAnApostrophe ? "'" + x.PayerAccountNum : x.PayerAccountNum,
                    RecipientAccountNum = addAnApostrophe ? "'" + x.RecipientAccountNum : x.RecipientAccountNum,
                    x.PaymentDetails,
                    x.Sum,
                    RemainSum = x.RemainSum == 0 && x.DistributeState == DistributionState.NotDistributed ? x.Sum : x.RemainSum,
                    x.UserLogin,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.DistributeState,
                    DistributionDate = x.DistributionDate ?? DateTime.MinValue,
                    x.DistributionCode,
                    x.IsDistributable,
                    x.DateReceipt,
                    FileName = x.File.Name,
                    x.File,
                    x.Document,
                    x.SuspenseAccount,
                    x.IsROSP
                })
                .Filter(loadParam, Container);

            var debugList = data.ToList();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToArray(), data.Count());
        }
    }
}