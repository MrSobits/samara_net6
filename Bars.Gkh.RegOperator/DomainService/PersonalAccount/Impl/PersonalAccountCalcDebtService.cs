namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using Bars.B4.Modules.Analytics.Reports.Utils;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;
    using B4.IoC;

    using Bars.B4.Modules.NH.Extentions;
    using Bars.Gkh.RegOperator.CodedReports;
    using Bars.Gkh.RegOperator.Domain;

    /// <inheritdoc />
    public class PersonalAccountCalcDebtService : IPersonalAccountCalcDebtService
    {
        public IDomainService<PersonalAccountPeriodSummary> PeriodSummaryDomain { get; set; }

        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }

        public IDomainService<PersonalAccountPaymentTransfer> TransferDomain { get; set; }

        public IDomainService<PersonalAccountCalcDebt> PersonalAccountCalcDebtDomain { get; set; }

        public IDomainService<CalcDebtDetail> CalcDebtDetailDomain { get; set; }

        public IChargePeriodRepository PeriodRepository { get; set; }

        public IFileManager FileManager { get; set; }

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult CalcDebtTransfer(BaseParams baseParams)
        {
            var persAccId = baseParams.Params.GetAs<long>("persAccId");
            var prevOwnerId = baseParams.Params.GetAs<long>("ownerId");
            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");

            List<object> result = new List<object>();

            var account = this.BasePersonalAccountDomain.Get(persAccId);
            var previousOwner = this.LegalAccountOwnerDomain.Get(prevOwnerId);

            var startPeriod = this.PeriodRepository.GetPeriodByDate(dateStart);
            var endPeriod = this.PeriodRepository.GetPeriodByDate(dateEnd);

            var wallets = account.GetMainWallets();
            var walletGuids = wallets.Select(x => x.WalletGuid);

            var baseTariffWallet = wallets.FirstOrDefault(x => x.WalletType == WalletType.BaseTariffWallet).WalletGuid;
            var decTariffWallet = wallets.FirstOrDefault(x => x.WalletType == WalletType.DecisionTariffWallet).WalletGuid;
            var penaltyWallet = wallets.FirstOrDefault(x => x.WalletType == WalletType.PenaltyWallet).WalletGuid;

            var charge = this.PeriodSummaryDomain.GetAll()
                .Where(x => x.PersonalAccount.Id == persAccId)
                .Where(x => x.Period.StartDate >= startPeriod.StartDate && x.Period.EndDate <= (endPeriod == null ? endPeriod.StartDate : endPeriod.GetEndDate()))
                .AsEnumerable()
                .GroupBy(x => x.PersonalAccount.Id)
                .Select(
                    x => new
                    {
                        ChargeBaseTariff = x.Sum(y => y.ChargedByBaseTariff) + x.Sum(y => y.RecalcByBaseTariff) + x.Sum(y => y.BaseTariffChange),
                        ChargeDecTariff = x.Sum(y => y.ChargeTariff) - x.Sum(y => y.ChargedByBaseTariff) + x.Sum(y => y.RecalcByDecisionTariff)
                        + x.Sum(y => y.DecisionTariffChange),
                        ChargePenalty = x.Sum(y => y.Penalty) + x.Sum(y => y.RecalcByPenalty) + x.Sum(y => y.PenaltyChange)
                    })
                .FirstOrDefault();

            var transfers = this.TransferDomain.GetAll()
                .Where(x => x.Owner.Id == persAccId)
                .Where(x => x.IsAffect && !x.Operation.IsCancelled)
                .Where(x => walletGuids.Contains(x.TargetGuid))
                .Where(x => x.PaymentDate >= dateStart)
                .Where(x => x.PaymentDate <= dateEnd)
                .Select(
                    x => new
                    {
                        Amount = (long?) (x.Operation.CanceledOperation.Id) != null
                            ? -1 * x.Amount
                            : x.Amount,
                        x.TargetGuid

                    })
                .ToList();

            var baseTariffPayments = transfers
                .Where(x =>
                x.TargetGuid == baseTariffWallet)
                .SafeSum(x => x.Amount);

            var decTariffPayments = transfers
                .Where(x =>
                    x.TargetGuid == decTariffWallet)
                .SafeSum(x => x.Amount);

            var penaltyPayments = transfers
                .Where(x =>
                    x.TargetGuid == penaltyWallet)
                .SafeSum(x => x.Amount);

            var currentOwner = new 
            {
                OwnerId = account.AccountOwner.Id,
                account.AccountOwner,
                AccountOwnerName = account.AccountOwner.Name,
                account.AccountOwner.OwnerType,
                account.AreaShare,
                Type = DebtDetailType.Current,
                charge.ChargeBaseTariff,
                charge.ChargeDecTariff,
                charge.ChargePenalty,
                DistributionDebtBaseTariff = -charge.ChargeBaseTariff,
                DistributionDebtDecTariff = -charge.ChargeDecTariff,
                DistributionDebtPenalty = -charge.ChargePenalty,
                PaymentBaseTariff = baseTariffPayments,
                PaymentDecTariff = decTariffPayments,
                PaymentPenalty = penaltyPayments

            };

            result.Add(currentOwner);

            var prevOwner = new
            {
                OwnerId = prevOwnerId,
                AccountOwner = previousOwner,
                AccountOwnerName = previousOwner.Name,
                previousOwner.OwnerType,
                Type = DebtDetailType.Previous,
                DistributionDebtBaseTariff = charge.ChargeBaseTariff,
                DistributionDebtDecTariff = charge.ChargeDecTariff,
                DistributionDebtPenalty = charge.ChargePenalty,
            };

            result.Add(prevOwner);

            return new ListDataResult(result, result.Count);
        }

        /// <inheritdoc />
        public IDataResult SaveDebtTransfer(BaseParams baseParams)
        {
            var persAccId = baseParams.Params.GetAs<long>("persAccId");
            var prevOwnerId = baseParams.Params.GetAs<long>("ownerId");
            var dateStart = baseParams.Params.GetAs<DateTime>("DateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("DateEnd");
            var agreementNumber = baseParams.Params.GetAs<string>("AgreementNumber");
            var document = baseParams.Files.Get("Document");
            PersonalAccountCalcDebt debtOperation = null;

            this.Container.InTransaction(
                () =>
                {
                    var fileInfo = document != null ? this.FileManager.SaveFile(document) : null;

                    this.DeletePrevOperation(persAccId);

                    debtOperation = new PersonalAccountCalcDebt
                    {
                        PersonalAccount = new BasePersonalAccount { Id = persAccId },
                        PreviousOwner = new LegalAccountOwner { Id = prevOwnerId },
                        DateStart = dateStart,
                        DateEnd = dateEnd,
                        AgreementNumber = agreementNumber,
                        Document = fileInfo
                    };

                    this.PersonalAccountCalcDebtDomain.Save(debtOperation);
                });

            return new BaseDataResult(debtOperation);
        }

        /// <inheritdoc />
        public IDataResult Export(BaseParams baseParams)
        {
            var calcDebtId = baseParams.Params.GetAs<string>("calcDebtId");
            var generator = this.Container.Resolve<ICodedReportManager>();
            using (this.Container.Using(generator))
            {
                var report = new CalcDebtExportReport
                {
                    CalcDebtId = calcDebtId
                };

                return new ReportResult
                {
                    ReportStream = generator.GenerateReport(report, baseParams, ReportPrintFormat.csv),
                    FileName = $"Приложение 2.3_{DateTime.Now.ToShortDateString()}.csv"
                };
            }
        }

        /// <inheritdoc />
        public IDataResult Print(BaseParams baseParams)
        {
            var calcDebtId = baseParams.Params.GetAs<string>("calcDebtId");
            var generator = this.Container.Resolve<ICodedReportManager>();
            using (this.Container.Using(generator))
            {
                var report = new CalcDebtReport
                {
                    CalcDebtId = calcDebtId
                };

                var name = $"Сопроводительное письмо в ЧЭС по переносу долга_{DateTime.Now.ToShortDateString()}.docx";

                return new ReportResult
                {
                    ReportStream = generator.GenerateReport(report, baseParams, ReportPrintFormat.docx),
                    FileName = name
                };
            }
        }

        /// <inheritdoc />
        public IDataResult GetPeriodInfo()
        {
            var firstPeriod = this.PeriodRepository.GetFirstPeriod();
            var curPeriod = this.PeriodRepository.GetCurrentPeriod();

            var result = new
            {
                StartDate = firstPeriod.StartDate,
                EndDate = curPeriod.GetEndDate()
            };

            return new BaseDataResult(result);
        }

        private void DeletePrevOperation(long accountId)
        {
            var calcDdebtDetail = this.CalcDebtDetailDomain.GetAll()
                .Where(x => x.CalcDebt.PersonalAccount.Id == accountId)
                .ToList();

            var calcDebts = calcDdebtDetail.Select(x => x.CalcDebt).Distinct().ToList();

            calcDdebtDetail.ForEach(x => this.CalcDebtDetailDomain.Delete(x.Id));
            calcDebts.ForEach(x => this.PersonalAccountCalcDebtDomain.Delete(x.Id));
        }
    }
}