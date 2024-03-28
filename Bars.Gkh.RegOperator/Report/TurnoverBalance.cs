namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Modules.Analytics.Utils;
    using B4;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Extenstions;

    using Domain;
    using Entities.ValueObjects;
    using Gkh.Domain.CollectionExtensions;
    using Entities;
    using Gkh.Utils;
    using GkhCr.Entities;

    public class TurnoverBalance : BasePrintForm
    {
        private long[] _municipalityIds;

        private long _startPeriodId;

        private long _endPeriodId;

        public IDomainService<RealityObjectChargeAccountOperation> RealityObjectChargeAccountOperationDomain { get; set; }
        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }
        public IDomainService<RealityObjectPaymentAccount> RoPaymentAccountDomain { get; set; }
        public IDomainService<PerformedWorkActPayment> ActPaymentDomain { get; set; }
        public IDomainService<Transfer> TransferDomain { get; set; }

        public TurnoverBalance() : base(new ReportTemplateBinary(Properties.Resources.TurnoverBalance))
        {
        }
        
        public override string Name { get { return "Оборотно-сальдовая ведомость"; } }

        public override string Desciption { get { return "Оборотно-сальдовая ведомость"; } }

        public override string GroupName { get { return "Региональная программа"; } }

        public override string ParamsController { get { return "B4.controller.report.TurnoverBalance"; } }

        public override string RequiredPermission { get { return "Reports.GkhRegOp.TurnoverBalance"; } }

        public override void SetUserParams(BaseParams baseParams)
        {
            _municipalityIds = baseParams.Params.GetAs<string>("municipalityIds").ToLongArray();
            _startPeriodId = baseParams.Params.GetAs("startPeriod", 0L);
            _endPeriodId = baseParams.Params.GetAs("endPeriod", 0L);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var data = PrepareData();

            int num = 0;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var item in data)
            {
                section.ДобавитьСтроку();

                section["num"] = ++num;
                section["Address"] = item.Address;

                section["SaldoIn"] = item.SaldoIn;
                section["ChargedTotal"] = item.ChargedTotal;
                section["PaidTotal"] = item.PaidTotal;

                section["ChargedPenalty"] = item.ChargedPenalty;
                section["PaidPenalty"] = item.PaidPenalty;
                
                section["Subsidy"] = item.Subsidy;
                section["ActPaid"] = item.ActPaid;
                section["SaldoOwners"] = item.SaldoOwners;
                section["SaldoMkd"] = item.SaldoMkd;
            }

            var srp = reportParams.SimpleReportParams;

            srp["TotalChargedPenalty"] = data.SafeSum(x => x.ChargedPenalty);
            srp["TotalChargedTotal"] = data.SafeSum(x => x.ChargedTotal);
            srp["TotalPaidPenalty"] = data.SafeSum(x => x.PaidPenalty);
            srp["TotalPaidTotal"] = data.SafeSum(x => x.PaidTotal);
            srp["TotalIncomeSubsidy"] = data.SafeSum(x => x.Subsidy);
            srp["TotalLoanIn"] = data.SafeSum(x => x.LoanIn);
            srp["TotalLoanOut"] = data.SafeSum(x => x.LoanOut);
            srp["TotalActPaid"] = data.SafeSum(x => x.ActPaid);
            srp["TotalSaldoOwners"] = data.SafeSum(x => x.SaldoOwners);
            srp["TotalSaldoMkd"] = data.SafeSum(x => x.SaldoMkd);
        }

        private List<RobjectBalance> PrepareData()
        {
            var accounts = RoPaymentAccountDomain.GetAll()
                .Where(x => _municipalityIds.Contains(x.RealityObject.Municipality.Id));

            var firstPeriod = ChargePeriodDomain.Get(_startPeriodId);
            var lastPeriod = ChargePeriodDomain.Get(_endPeriodId);

            var dateStart = firstPeriod.StartDate;
            var dateEnd = lastPeriod.GetEndDate();

            var payments = ActPaymentDomain.GetAll()
                .Where(x => _municipalityIds.Contains(x.PerformedWorkAct.ObjectCr.RealityObject.Municipality.Id))
                .GroupBy(x => x.PerformedWorkAct.ObjectCr.RealityObject.Id)
                .ToDictionary(x => x.Key, z => z.ToArray());

            var chargeAccountOperations = RealityObjectChargeAccountOperationDomain.GetAll()
                .Where(x => _municipalityIds.Contains(x.Account.RealityObject.Municipality.Id))
                .Where(x => x.Period.StartDate >= dateStart)
                .Where(x => x.Period.EndDate <= dateEnd)
                .GroupBy(x => x.Account.RealityObject.Id)
                .ToDictionary(x => x.Key, z => z.ToArray());

            var result = new List<RobjectBalance>();

            var transferQuery = TransferDomain.GetAll();

            foreach (var account in accounts)
            {
                var walletGuids = account.GetWallets().Select(x => x.WalletGuid).ToList();

                var transfers = transferQuery
                    .Where(x => x.PaymentDate.Date <= dateEnd)
                    .Where(x => walletGuids.Contains(x.TargetGuid) || walletGuids.Contains(x.SourceGuid))
                    .Where(x => !x.IsLoan && !x.IsReturnLoan)
                    .ToArray();

                var record = new RobjectBalance(account, firstPeriod, payments.Get(account.RealityObject.Id));

                record.ApplyTransfers(transfers);
                record.ApplyCharges(chargeAccountOperations.Get(account.RealityObject.Id));

                result.Add(record);
            }

            return result;
        }

        private class RobjectBalance
        {
            public RobjectBalance(RealityObjectPaymentAccount account, ChargePeriod startPeriod, IEnumerable<PerformedWorkActPayment> payments)
            {
                Account = account;
                Address = account.RealityObject.Address;

                PeriodStart = startPeriod.StartDate;

                PaymentGuids =
                    payments != null
                        ? payments
                            .Where(x => !x.TransferGuid.IsEmpty())
                            .Select(x => x.TransferGuid)
                            .ToHashSet()
                        : new HashSet<string>();
            }

            public void ApplyTransfers(IEnumerable<Transfer> transfers)
            {
                var walletGuids = Account.GetWallets().Select(x => x.WalletGuid).ToHashSet();
                var subsidyGuids = Account.GetSubsidyWalletGuids().ToHashSet();
                var tariffWalletGuids = new HashSet<string>
                {
                    Account.BaseTariffPaymentWallet.WalletGuid,
                    Account.DecisionPaymentWallet.WalletGuid
                };

                var penaltyWalletGuids = new HashSet<string>
                {
                    Account.PenaltyPaymentWallet.WalletGuid
                };

                foreach (var transfer in transfers)
                {
                    if (transfer.PaymentDate.Date < PeriodStart)
                    {
                        SaldoIn += transfer.Amount;
                        continue;
                    }

                    //исходящий трансфер
                    if (walletGuids.Contains(transfer.SourceGuid))
                    {
                        if (subsidyGuids.Contains(transfer.SourceGuid))
                            Subsidy -= transfer.Amount;

                        //трансфер является оплатой акта
                        if (PaymentGuids.Contains(transfer.TargetGuid))
                            ActPaid += transfer.Amount;

                        continue;
                    }

                    //входящий трансфер
                    if (walletGuids.Contains(transfer.TargetGuid))
                    {
                        if (subsidyGuids.Contains(transfer.TargetGuid))
                            Subsidy += transfer.Amount;

                        if (tariffWalletGuids.Contains(transfer.TargetGuid))
                            PaidTotal += transfer.Amount;

                        if (penaltyWalletGuids.Contains(transfer.TargetGuid))
                            PaidPenalty += transfer.Amount;
                    }
                }
            }

            public void ApplyCharges(IEnumerable<RealityObjectChargeAccountOperation> operations)
            {
                if (operations == null)
                {
                    return;
                }

                foreach (var operation in operations)
                {
                    ChargedTotal += operation.ChargedTotal;
                    ChargedPenalty += operation.ChargedPenalty;
                }
            }

            public string Address { get; private set; }

            /// <summary>
            /// Входящее сальдо (по оплатам)
            /// </summary>
            public decimal SaldoIn { get; private set; }

            /// <summary>
            /// Начислено
            /// </summary>
            public decimal ChargedTotal { get; private set; }

            /// <summary>
            /// Начислено пени
            /// </summary>
            public decimal ChargedPenalty { get; private set; }

            /// <summary>
            /// Уплачено
            /// </summary>
            public decimal PaidTotal { get; private set; }

            /// <summary>
            /// Оплачено пени
            /// </summary>
            public decimal PaidPenalty { get; private set; }

            /// <summary>
            /// Субсидии
            /// </summary>
            public decimal Subsidy { get; private set; }

            /// <summary>
            /// Поступление займов
            /// </summary>
            public decimal LoanIn { get; private set; }

            /// <summary>
            /// Оплата займа
            /// </summary>
            public decimal LoanOut { get; private set; }

            /// <summary>
            /// Оплаты работ
            /// </summary>
            public decimal ActPaid { get; private set; }

            /// <summary>
            /// Сальдо по абонентам
            /// </summary>
            public decimal SaldoOwners
            {
                get { return ChargedTotal + ChargedPenalty - PaidTotal - PaidPenalty; }
            }

            /// <summary>
            /// Сальдо МКД
            /// </summary>
            public decimal SaldoMkd
            {
                get { return SaldoIn + PaidTotal + PaidPenalty + Subsidy + LoanIn - LoanOut - ActPaid; }
            }

            private RealityObjectPaymentAccount Account { get; set; }

            private HashSet<string> PaymentGuids { get; set; }

            private DateTime PeriodStart { get; set; }
        }
    }
}