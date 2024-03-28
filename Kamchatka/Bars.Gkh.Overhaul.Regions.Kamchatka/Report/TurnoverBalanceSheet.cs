namespace Bars.Gkh.Overhaul.Regions.Kamchatka.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Modules.Reports;
    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Extenstions;

    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using RegOperator.Entities;
    using Gkh.Utils;
    using RegOperator.Domain;
    using RegOperator.Entities.ValueObjects;

    /// <summary>
    /// Оборотно-сальдоый баланс (Камчатка)
    /// </summary>
    public class TurnoverBalanceSheet : BasePrintForm
    {
        private long _mrId;
        private long _moId;
        private string _locality;
        private long[] _roIds;
        private long _periodId;

        private readonly IDomainService<RealityObjectPaymentAccount> _ropayAccDomain;
        private readonly IDomainService<ChargePeriod> _chargePeriodDomain;
        private readonly IDomainService<Transfer> _transferDomain;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="ropayAccDomain"></param>
        /// <param name="chargePeriodDomain"></param>
        /// <param name="transferDomain"></param>
        public TurnoverBalanceSheet(
            IDomainService<RealityObjectPaymentAccount> ropayAccDomain,
            IDomainService<ChargePeriod> chargePeriodDomain,
            IDomainService<Transfer> transferDomain)
            : base(new ReportTemplateBinary(Properties.Resources.TurnoverBalanceSheet))
        {
            _ropayAccDomain = ropayAccDomain;
            _chargePeriodDomain = chargePeriodDomain;
            _transferDomain = transferDomain;
        }

        public override string Name { get { return "Оборотно-сальдовая ведомость (Камчатка)"; } }

        public override string Desciption { get { return "Оборотно-сальдовая ведомость (Камчатка)"; } }

        public override string GroupName { get { return "Региональная программа"; } }

        public override string ParamsController { get { return "B4.controller.report.TurnoverBalanceSheet"; } }

        public override string RequiredPermission { get { return "Reports.GkhRegOp.TurnoverBalanceSheet"; } }

        public override string ReportGenerator { get; set; }

        public override void SetUserParams(BaseParams baseParams)
        {
            _mrId = baseParams.Params["municipalityParent"].ToLong();
            _moId = baseParams.Params["municipality"].ToLong();
            _locality = baseParams.Params["locality"].ToStr();
            var roIdsList = baseParams.Params.GetAs("roIds", string.Empty);
            _roIds = !string.IsNullOrEmpty(roIdsList)
                                  ? roIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
            _periodId = baseParams.Params.GetAsId("period");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var period = _chargePeriodDomain.Get(_periodId);

            reportParams.SimpleReportParams["StartDate"] = period.StartDate.ToShortDateString();
            reportParams.SimpleReportParams["EndDate"] = period.GetEndDate().ToShortDateString();

            var data = PrepareData(period);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            int num = 0;

            foreach (var item in data)
            {
                section.ДобавитьСтроку();

                section["num"] = ++num;

                section["PaymentAccNum"] = item.AccountNumber;

                //"до" дебет
                section["BeforeDebetPaym"] = item.BeforeBalance.TariffDebet;
                section["BeforeDebetPenalty"] = item.BeforeBalance.PenaltyDebet;
                section["BeforeDebetSubsidy"] = item.BeforeBalance.SubsidyDebet;

                //"до" кредит
                section["BeforeCreditPaym"] = item.BeforeBalance.TariffCredit;
                section["BeforeCreditPenalty"] = item.BeforeBalance.PenaltyCredit;
                section["BeforeCreditSubsidy"] = item.BeforeBalance.SubsidyCredit;

                //"период" дебет
                section["PeriodDebetPaym"] = item.PeriodBalance.TariffDebet;
                section["PeriodDebetPenalty"] = item.PeriodBalance.PenaltyDebet;
                section["PeriodDebetSubsidy"] = item.PeriodBalance.SubsidyDebet;

                //"период" кредит
                section["PeriodCreditPaym"] = item.PeriodBalance.TariffCredit;
                section["PeriodCreditPenalty"] = item.PeriodBalance.PenaltyCredit;
                section["PeriodCreditSubsidy"] = item.PeriodBalance.SubsidyCredit;
                section["PeriodCreditAnother"] = item.PeriodBalance.OtherCredit;

                //итого по периоду
                section["PeriodDebetSum"] = item.PeriodBalance.DebetTotal;
                section["PeriodCreditSum"] = item.PeriodBalance.CreditTotal;

                //исходящее дебет
                section["AfterDebetPaym"] = item.AfterTariffDebet;
                section["AfterDebetPenalty"] = item.AfterPenaltyDebet;
                section["AfterDebetSubsidy"] = item.AfterSubsidyDebet;

                //исходящее кредит
                section["AfterCreditPaym"] = item.AfterTariffCredit;
                section["AfterCreditPenalty"] = item.AfterPenaltyCredit;
                section["AfterCreditSubsidy"] = item.AfterSubsidyCredit;
            }

            var srp = reportParams.SimpleReportParams;

            //"до" дебет
            srp["TotalBeforeDebetPaym"] = data.SafeSum(x => x.BeforeBalance.TariffDebet);
            srp["TotalBeforeDebetPenalty"] = data.SafeSum(x => x.BeforeBalance.PenaltyDebet);
            srp["TotalBeforeDebetSubsidy"] = data.SafeSum(x => x.BeforeBalance.SubsidyDebet);

            //"до" кредит
            srp["TotalBeforeCreditPaym"] = data.SafeSum(x => x.BeforeBalance.TariffCredit);
            srp["TotalBeforeCreditPenalty"] = data.SafeSum(x => x.BeforeBalance.PenaltyCredit);
            srp["TotalBeforeCreditSubsidy"] = data.SafeSum(x => x.BeforeBalance.SubsidyCredit);

            //"период" дебет
            srp["TotalPeriodDebetPaym"] = data.SafeSum(x => x.PeriodBalance.TariffDebet);
            srp["TotalPeriodDebetPenalty"] = data.SafeSum(x => x.PeriodBalance.PenaltyDebet);
            srp["TotalPeriodDebetSubsidy"] = data.SafeSum(x => x.PeriodBalance.SubsidyDebet);

            //"период" кредит
            srp["TotalPeriodCreditPaym"] = data.SafeSum(x => x.PeriodBalance.TariffCredit);
            srp["TotalPeriodCreditPenalty"] = data.SafeSum(x => x.PeriodBalance.PenaltyCredit);
            srp["TotalPeriodCreditSubsidy"] = data.SafeSum(x => x.PeriodBalance.SubsidyCredit);
            srp["TotalPeriodCreditAnother"] = data.SafeSum(x => x.PeriodBalance.OtherCredit);

            //итого по периоду
            srp["TotalPeriodDebetSum"] = data.SafeSum(x => x.PeriodBalance.DebetTotal);
            srp["TotalPeriodCreditSum"] = data.SafeSum(x => x.PeriodBalance.CreditTotal);

            //исходящее дебет
            srp["TotalAfterDebetPaym"] = data.SafeSum(x => x.AfterTariffDebet);
            srp["TotalAfterDebetPenalty"] = data.SafeSum(x => x.AfterPenaltyDebet);
            srp["TotalAfterDebetSubsidy"] = data.SafeSum(x => x.AfterSubsidyDebet);

            //исходящее кредит
            srp["TotalAfterCreditPaym"] = data.SafeSum(x => x.AfterTariffCredit);
            srp["TotalAfterCreditPenalty"] = data.SafeSum(x => x.AfterPenaltyCredit);
            srp["TotalAfterCreditSubsidy"] = data.SafeSum(x => x.AfterSubsidyCredit);
        }

        private List<RobjectBalances> PrepareData(ChargePeriod period)
        {
            var result = new List<RobjectBalances>();

            var accounts = _ropayAccDomain.GetAll()
                .WhereIf(_mrId != 0, x => x.RealityObject.Municipality.Id == _mrId)
                .WhereIf(_moId != 0, x => x.RealityObject.MoSettlement.Id == _moId)
                .WhereIf(_locality != string.Empty, x => x.RealityObject.FiasAddress.PlaceName == _locality)
                .WhereIf(_roIds.Any(), x => _roIds.Contains(x.RealityObject.Id))
                .OrderBy(x => x.RealityObject.Address);

            foreach (var account in accounts)
            {
                var record = new RobjectBalances(account, period);

                var walletGuids = account.GetWallets().Select(x => x.WalletGuid).ToList();

                var transfers = _transferDomain.GetAll()
                    .Where(x => walletGuids.Contains(x.TargetGuid) || walletGuids.Contains(x.SourceGuid))
                    .ToList();

                record.ApplyTransfers(transfers);

                result.Add(record);
            }

            return result;
        }

        private class RobjectBalances
        {
            public RobjectBalances(RealityObjectPaymentAccount account, ChargePeriod period)
            {
                _startDate = period.StartDate;
                _endDate = period.GetEndDate();
                AccountNumber = account.RealityObject.Address;

                var tariffWallets = new HashSet<string>
                {
                    account.BaseTariffPaymentWallet.WalletGuid,
                    account.DecisionPaymentWallet.WalletGuid
                };

                var penaltyWallets = new HashSet<string>
                {
                    account.PenaltyPaymentWallet.WalletGuid
                };

                var subsidyWallets = new HashSet<string>
                {
                    account.FundSubsidyWallet.WalletGuid,
                    account.RegionalSubsidyWallet.WalletGuid,
                    account.StimulateSubsidyWallet.WalletGuid,
                    account.TargetSubsidyWallet.WalletGuid
                };

                var otherWallets = account.GetWallets().Select(x => x.WalletGuid).ToList()
                    .Except(tariffWallets)
                    .Except(penaltyWallets)
                    .Except(subsidyWallets)
                    .ToHashSet();

                BeforeBalance = new Balance(tariffWallets, penaltyWallets, subsidyWallets, otherWallets);
                PeriodBalance = new Balance(tariffWallets, penaltyWallets, subsidyWallets, otherWallets);
            }

            public void ApplyTransfers(IEnumerable<Transfer> transfers)
            {
                foreach (var transfer in transfers)
                {
                    var date = transfer.PaymentDate.Date;

                    if (date > _endDate)
                    {
                        continue;
                    }

                    if (date < _startDate)
                        BeforeBalance.ApplyTransfer(transfer);
                    else
                        PeriodBalance.ApplyTransfer(transfer);
                }
            }

            public string AccountNumber { get; private set; }

            public readonly Balance BeforeBalance;

            public readonly Balance PeriodBalance;

            private readonly DateTime _startDate;
            private readonly DateTime _endDate;

            /// <summary>
            /// Исходящее, дебет, взносы
            /// </summary>
            public decimal AfterTariffDebet
            {
                get { return BeforeBalance.TariffDebet + PeriodBalance.TariffDebet; }
            }

            /// <summary>
            /// Исходящее, дебет, пени
            /// </summary>
            public decimal AfterPenaltyDebet
            {
                get { return BeforeBalance.PenaltyDebet + PeriodBalance.PenaltyDebet; }
            }

            /// <summary>
            /// Исходящее, дебет, субсидии
            /// </summary>
            public decimal AfterSubsidyDebet
            {
                get { return BeforeBalance.SubsidyDebet + PeriodBalance.SubsidyDebet; }
            }

            /// <summary>
            /// Исходящее, кредит, взносы
            /// </summary>
            public decimal AfterTariffCredit
            {
                get { return BeforeBalance.TariffCredit + PeriodBalance.TariffCredit; }
            }

            /// <summary>
            /// Исходящее, кредит, пени
            /// </summary>
            public decimal AfterPenaltyCredit
            {
                get { return BeforeBalance.PenaltyCredit + PeriodBalance.PenaltyCredit; }
            }

            /// <summary>
            /// Исходящее, кредит, субсидии
            /// </summary>
            public decimal AfterSubsidyCredit
            {
                get { return BeforeBalance.SubsidyCredit + PeriodBalance.SubsidyCredit; }
            }
        }

        private class Balance
        {
            public Balance(
                HashSet<string> tariffWallets,
                HashSet<string> penaltyWallets,
                HashSet<string> subsidyWallets,
                HashSet<string> otherWallets)
            {
                _tariffWallets = tariffWallets;
                _penaltyWallets = penaltyWallets;
                _subsidyWallets = subsidyWallets;
                _otherWallets = otherWallets;
            }

            private readonly HashSet<string> _tariffWallets;
            private readonly HashSet<string> _penaltyWallets;
            private readonly HashSet<string> _subsidyWallets;
            private readonly HashSet<string> _otherWallets;

            public void ApplyTransfer(Transfer transfer)
            {
                if (transfer.IsLoan || transfer.IsReturnLoan)
                {
                    //дом взял займ или дому вернули займ - учитываем на дебете
                    {
                        if (_tariffWallets.Contains(transfer.TargetGuid))
                            TariffDebet += transfer.Amount;

                        if (_penaltyWallets.Contains(transfer.TargetGuid))
                            PenaltyDebet += transfer.Amount;

                        if (_subsidyWallets.Contains(transfer.TargetGuid))
                            SubsidyDebet += transfer.Amount;

                        if (_otherWallets.Contains(transfer.TargetGuid))
                            OtherDebet += transfer.Amount;
                    }

                    //у дома взяли займ или дом вернул займ - учитываем на кредите
                    {
                        if (_tariffWallets.Contains(transfer.SourceGuid))
                            TariffCredit += transfer.Amount;

                        if (_penaltyWallets.Contains(transfer.SourceGuid))
                            PenaltyCredit += transfer.Amount;

                        if (_subsidyWallets.Contains(transfer.SourceGuid))
                            SubsidyCredit += transfer.Amount;

                        if (_otherWallets.Contains(transfer.SourceGuid))
                            OtherCredit += transfer.Amount;
                    }

                    return;
                }

                // операция не отменена
                // смотрим на TargetGuid и учитываем на дебете
                // или смотрим на SourceGuid и учитываем на кредите
                if (!transfer.Operation.IsCancelled)
                {
                    if (_tariffWallets.Contains(transfer.TargetGuid))
                        TariffDebet += transfer.Amount;

                    if (_penaltyWallets.Contains(transfer.TargetGuid))
                        PenaltyDebet += transfer.Amount;

                    if (_subsidyWallets.Contains(transfer.TargetGuid))
                        SubsidyDebet += transfer.Amount;

                    if (_otherWallets.Contains(transfer.TargetGuid))
                        OtherDebet += transfer.Amount;



                    if (_tariffWallets.Contains(transfer.SourceGuid))
                        TariffCredit += transfer.Amount;

                    if (_penaltyWallets.Contains(transfer.SourceGuid))
                        PenaltyCredit += transfer.Amount;

                    if (_subsidyWallets.Contains(transfer.SourceGuid))
                        SubsidyCredit += transfer.Amount;

                    if (_otherWallets.Contains(transfer.SourceGuid))
                        OtherCredit += transfer.Amount;
                }
                // операция отменена
                // смотрим на SourceGuid и учитываем на дебете с минусом
                // или смотрим на TargetGuid и учитываем на кредите с минусом
                else
                {
                    if (_tariffWallets.Contains(transfer.SourceGuid))
                        TariffDebet -= transfer.Amount;

                    if (_penaltyWallets.Contains(transfer.SourceGuid))
                        PenaltyDebet -= transfer.Amount;

                    if (_subsidyWallets.Contains(transfer.SourceGuid))
                        SubsidyDebet -= transfer.Amount;

                    if (_otherWallets.Contains(transfer.SourceGuid))
                        OtherDebet -= transfer.Amount;



                    if (_tariffWallets.Contains(transfer.TargetGuid))
                        TariffCredit -= transfer.Amount;

                    if (_penaltyWallets.Contains(transfer.TargetGuid))
                        PenaltyCredit -= transfer.Amount;

                    if (_subsidyWallets.Contains(transfer.TargetGuid))
                        SubsidyCredit -= transfer.Amount;

                    if (_otherWallets.Contains(transfer.TargetGuid))
                        OtherCredit -= transfer.Amount;
                }
            }

            public decimal TariffDebet { get; private set; }
            public decimal PenaltyDebet { get; private set; }
            public decimal SubsidyDebet { get; private set; }
            private decimal OtherDebet { get; set; }

            public decimal DebetTotal
            {
                get { return TariffDebet + PenaltyDebet + SubsidyDebet + OtherDebet; }
            }

            public decimal TariffCredit { get; private set; }
            public decimal PenaltyCredit { get; private set; }
            public decimal SubsidyCredit { get; private set; }
            public decimal OtherCredit { get; private set; }

            public decimal CreditTotal
            {
                get { return TariffCredit + PenaltyCredit + SubsidyCredit + OtherCredit; }
            }
        }
    }
}