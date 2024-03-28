namespace Bars.Gkh.RegOperator.DataExport
{
    using System;
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Entities;
    using System.Collections.Generic;
    using GkhGji.Enums;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Enums;

    public class BankAccountStatementDataExport : BaseDataExportService
    {
        /// <summary>
        /// Домен-сервис <see cref="CalcAccount"/>
        /// </summary>
        public IDomainService<CalcAccount> CalcAccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="CalcAccountRealityObject"/>
        /// </summary>
        public IDomainService<CalcAccountRealityObject> CalcAccountRoDomain { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var statementDomain = this.Container.ResolveDomain<BankAccountStatement>();
            var groupId = baseParams.Params.GetAsId("groupId");
            var showDistributed = baseParams.Params.GetAs<bool>("showDistributed");
            var showCancelled = baseParams.Params.GetAs<bool>("showDeleted");
            var accountNumbersString = baseParams.Params.GetAs("specAccountIds", string.Empty);
            var dateReceiptFrom = baseParams.Params.GetAs<DateTime>("dateReceiptFrom");
            var alerdate = baseParams.Params.Get("dateReceiptFrom");
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

            var query = statementDomain.GetAll();

            if (groupId == 0)
            {
                query = query
                    .WhereIf(!showDistributed, x => x.DistributeState != DistributionState.Distributed)
                    .WhereIf(!showCancelled, x => x.DistributeState != DistributionState.Deleted);
            }

            var data = query
                .WhereIf(groupId > 0, x => x.Group.Id == groupId)
                .WhereIf(accountQuery != null, x => accountQuery.Any(y => y == x.RecipientAccountNum))
                .WhereIf(dateReceiptFrom != DateTime.MinValue, x => x.DateReceipt >= dateReceiptFrom)
                .WhereIf(dateReceiptBy != DateTime.MinValue, x => x.DateReceipt <= dateReceiptBy)
                .Select(x => new
                {
                    x.Id,
                    x.LinkedDocuments,
                    x.MoneyDirection,
                    x.OperationDate,
                    PayerName = x.Payer != null ? x.Payer.Name : x.PayerName,
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
                    x.SuspenseAccount
                })
                .Filter(loadParam, Container);

            int countData = data.Count();

            return data.ToList();
        }
    }
}