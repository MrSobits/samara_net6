using System;
using System.Collections.Generic;
using System.Linq;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.Gkh.RegOperator.Domain;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.RegOperator.Entities.ValueObjects;
using Bars.Gkh.Utils;
using Bars.GkhCr.Entities;

namespace Bars.Gkh.RegOperator.Report
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.Extenstions;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    using NHibernate.Linq;

    internal class TurnoverBalanceByMkd : BasePrintForm
    {

        public IDomainService<RealityObjectChargeAccountOperation> RealityObjectChargeAccountOperationDomain { get; set; }

        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }
        public IDomainService<RealityObjectPaymentAccount> RoPaymentAccountDomain { get; set; }
        public IDomainService<PerformedWorkActPayment> ActPaymentDomain { get; set; }
        public IDomainService<Transfer> TransferDomain { get; set; }
        public IChargePeriodRepository ChargePeriodService { get; set; }

        public TurnoverBalanceByMkd()
            : base(new ReportTemplateBinary(Properties.Resources.TurnoverBalanceByMkd))
        {
        }

        private long[] municipalityIds;
        private DateTime dateStart = DateTime.MinValue;
        private DateTime dateEnd = DateTime.MaxValue;

        private MethodFormFundCr methodFormFund;

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Оборотно-сальдовая ведомость по счетам МКД"; }
        }

        public override string Desciption
        {
            get { return "Оборотно-сальдовая ведомость по счетам МКД"; }
        }

        public override string GroupName
        {
            get { return "Региональная программа"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.TurnoverBalanceByMkd";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.TurnoverBalanceByMkd";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                ? municipalityIdsList.Split(',').Select(id => id.To<long>()).ToArray()
                : new long[0];

            dateStart = baseParams.Params["dateStart"].ToDateTime();
            dateEnd = baseParams.Params["dateEnd"].ToDateTime();

            methodFormFund = baseParams.Params.GetAs<MethodFormFundCr>("methodForm");


        }

        public override string ReportGenerator { get; set; }

        private List<RobjectBalance> PrepareData()
        {
            var accounts =
                RoPaymentAccountDomain.GetAll()
                    .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .Fetch(x => x.RealityObject)
                    .ThenFetch(x => x.Municipality)
                    .FetchAllWallets();

            var firstPeriod = ChargePeriodDomain.GetAll().FirstOrDefault(x => x.StartDate.Year == dateStart.Year && x.StartDate.Month == dateStart.Month) ??
                              ChargePeriodService.GetFirstPeriod();

            var payments = ActPaymentDomain.GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.PerformedWorkAct.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => x.PerformedWorkAct.ObjectCr.RealityObject.MethodFormFundCr == methodFormFund)
                .GroupBy(x => x.PerformedWorkAct.ObjectCr.RealityObject.Id)
                .ToDictionary(x => x.Key, z => z.ToArray());

            var chargeAccountOperations = RealityObjectChargeAccountOperationDomain.GetAll()
                .Select(x => new
                                 {
                                     MuId = x.Account.RealityObject.Municipality.Id,
                                     x.Period,
                                     RoId = x.Account.RealityObject.Id,
                                     x.ChargedTotal,
                                     x.ChargedPenalty
                                 })
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.MuId))
                .Where(x => x.Period.StartDate >= dateStart)
                .Where(x => x.Period.EndDate <= dateEnd)
                .ToList()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, z => z.ToArray());

            var result = new List<RobjectBalance>();

            var transfers = TransferDomain.GetAll()
                   .Where(x => x.PaymentDate.Date <= dateEnd)
                   .Where(x => !x.IsLoan && !x.IsReturnLoan)
                   .ToArray();

            foreach (var account in accounts)
            {
                var walletGuids = account.GetWallets().Select(x => x.WalletGuid).ToList();

                var curTransfers = transfers
                    .Where(x => x.PaymentDate.Date <= dateEnd)
                    .Where(x => walletGuids.Contains(x.TargetGuid) || walletGuids.Contains(x.SourceGuid))
                    .Where(x => !x.IsLoan && !x.IsReturnLoan)
                    .ToArray();

                var lastOperationDate = curTransfers.SafeMax(x => x.PaymentDate);

                var record = new RobjectBalance(account, firstPeriod, payments.Get(account.RealityObject.Id), lastOperationDate);

                record.ApplyTransfers(curTransfers);
                var chargeAccountOperation = chargeAccountOperations.Get(account.RealityObject.Id);
                if (chargeAccountOperation != null)
                {
                    record.ApplyCharges(
                        chargeAccountOperation.Select(x => x.ChargedTotal).ToList(),
                        chargeAccountOperation.Select(x => x.ChargedPenalty).ToList());
                }

                result.Add(record);
            }
            
             return result;
        }

        public override void PrepareReport(ReportParams reportParams)
        {

            reportParams.SimpleReportParams["DateStart"] = dateStart.ToShortDateString();
            reportParams.SimpleReportParams["DateEnd"] = dateEnd.ToShortDateString();
            reportParams.SimpleReportParams["CurrentDate"] = DateTime.Today.ToShortDateString();

            var data = PrepareData();

            Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();

            int num = 0;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var item in data)
            {
                section.ДобавитьСтроку();

                section["num"] = ++num;
                section["Address"] = item.Address;
                section["Mun"] = item.Municipality;
                section["MkdNumSchet"] = item.AccountNumber;
                section["DateLastOper"] = item.LastOperDate != DateTime.MinValue ? item.LastOperDate.ToString("dd.MM.yyyy") : string.Empty;

                section["BalanceStartDate"] = item.SaldoIn;
                section["OfDebit"] = item.PaidTotal + item.PaidPenalty + item.Subsidy;
                section["OfCredit"] = item.ActPaid;
                section["BalanceEndDate"] = item.SaldoIn + (item.PaidTotal + item.PaidPenalty + item.Subsidy) -
                                            item.ActPaid;
            }

            var srp = reportParams.SimpleReportParams;

            var totalBalanceStartDate = data.SafeSum(x => x.SaldoIn);
            var totalOfDebit = data.SafeSum(x => x.PaidTotal + x.PaidPenalty + x.Subsidy);
            var totalOfCredit = data.SafeSum(x => x.ActPaid);

            srp["TotalBalanceStartDate"] = totalBalanceStartDate;
            srp["TotalOfDebit"] = totalOfDebit;
            srp["TotalOfCredit"] = totalOfCredit;

            srp["TotalBalanceEndDate"] = totalBalanceStartDate + totalOfDebit - totalOfCredit;
        }

        private class RobjectBalance
        {
            public RobjectBalance(RealityObjectPaymentAccount account, ChargePeriod startPeriod,
                IEnumerable<PerformedWorkActPayment> payments, DateTime lastOperationDate)
            {
                Account = account;
                Address = account.RealityObject.Address;
                Municipality = account.RealityObject.Municipality.Name;
                AccountNumber = account.AccountNumber;
                LastOperDate = lastOperationDate;

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

            public void ApplyCharges(IEnumerable<decimal> chargedTotal, IEnumerable<decimal> chargedPenalty)
            {
                foreach (var operation in chargedTotal)
                {
                    ChargedTotal += operation;
                }

                foreach (var operation in chargedPenalty)
                {
                    ChargedPenalty += operation;
                }
            }

            public string Address { get; private set; }

            public string Municipality { get; private set; }

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

            public DateTime LastOperDate { get; set; }

            public string AccountNumber { get; set; }

            private RealityObjectPaymentAccount Account { get; set; }

            private HashSet<string> PaymentGuids { get; set; }

            private DateTime PeriodStart { get; set; }
        }
    }
}
