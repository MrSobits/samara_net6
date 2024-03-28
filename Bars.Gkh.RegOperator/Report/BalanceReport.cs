namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using B4;
    using B4.Modules.Reports;
    using B4.Utils;
    using DomainModelServices;
    using Bars.Gkh.Utils;

    using Entities;
    
    using Entities.ValueObjects;
    using Gkh.Domain;
    using NHibernate.Linq;

    public class BalanceReport : BasePrintForm
    {
        #region Properties

        private long[] _municipalityIds;
        private long[] _settlementIds;
        private long[] _addressIds;
        private DateTime _startDate;
        private DateTime _endDate;

        #endregion

        #region Injection

        public IWalletBalanceService TransferCombine { get; set; }
        public IDomainService<Transfer> TransferDomain { get; set; }
        public IDomainService<RealityObjectPaymentAccount> RoPaymentAccountDomain { get; set; }

        #endregion

        public BalanceReport() : base(new ReportTemplateBinary(Properties.Resources.BalanceReport))
        {
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["ДатаС"] = _startDate.ToShortDateString();
            reportParams.SimpleReportParams["ДатаПо"] = _endDate.ToShortDateString();

            var accounts = RoPaymentAccountDomain.GetAll()
                .WhereIf(_municipalityIds.IsNotEmpty(), x => _municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(_settlementIds.IsNotEmpty(), x => _settlementIds.Contains(x.RealityObject.MoSettlement.Id))
                .WhereIf(_addressIds.IsNotEmpty(), x => _addressIds.Contains(x.RealityObject.Id))

                .OrderBy(x => x.RealityObject.Municipality.Name)
                .ThenBy(x => x.RealityObject.MoSettlement.Name)
                .ThenBy(x => x.RealityObject.Address)

                .Fetch(x => x.RealityObject)
                .ThenFetch(x => x.Municipality)

                .Fetch(x => x.RealityObject)
                .ThenFetch(x => x.MoSettlement)

                .Fetch(x => x.RealityObject)
                .ThenFetch(x => x.FiasAddress)
                
                .ToArray();

            var page1Data = FillPage1(accounts, reportParams);
            var page2Data = FillPage2(accounts, reportParams);
            var page3Data = FillPage3(accounts, reportParams);
            var page4Data = FillPage4(accounts, reportParams, page1Data, page2Data, page3Data);

            FillPage5(accounts, reportParams, page1Data, page2Data, page3Data, page4Data);
        }

        private Dictionary<long, RealtyBalance> FillPage1(RealityObjectPaymentAccount[] accounts, ReportParams reportParams)
        {
            var balances =
                TransferCombine.GetDebetBalance(
                    accounts,
                    x => !x.IsLoan,
                    x => !x.IsReturnLoan,
                    x => x.PaymentDate <= _startDate);

            var loanTakeBalances =
                TransferCombine.GetDebetBalance(
                    accounts,
                    x => x.IsLoan,
                    x => !x.IsReturnLoan,
                    x => x.PaymentDate <= _startDate);

            var loanReturnBalances =
                TransferCombine.GetDebetBalance(
                    accounts,
                    x => !x.IsLoan,
                    x => x.IsReturnLoan,
                    x => x.PaymentDate <= _startDate);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRow");

            var result = new Dictionary<long, RealtyBalance>();

            int index = 0;

            foreach (var account in accounts)
            {
                section.ДобавитьСтроку();
                section["index"] = ++index;
                section["mr"] = account.RealityObject.Municipality.Name;
                section["mo"] = account.RealityObject.MoSettlement.Return(x => x.Name);
                section["place"] = account.RealityObject.FiasAddress.PlaceName;
                section["address"] = account.RealityObject.Address;
                result[account.Id] = new RealtyBalance(account, balances, loanTakeBalances, loanReturnBalances){Id = account.Id};
                FillSection(section, result[account.Id]);
            }

            return result;
        }

        private Dictionary<long, RealtyBalance> FillPage2(RealityObjectPaymentAccount[] accounts, ReportParams reportParams)
        {
            var balances =
                TransferCombine.GetDebetBalance(
                    accounts,
                    x => !x.IsLoan,
                    x => !x.IsReturnLoan,
                    x => x.PaymentDate >= _startDate && x.PaymentDate <= _endDate);

            var loanTakeBalances =
                TransferCombine.GetDebetBalance(
                    accounts,
                    x => x.IsLoan,
                    x => !x.IsReturnLoan,
                    x => x.PaymentDate >= _startDate && x.PaymentDate <= _endDate);

            var loanReturnBalances =
                TransferCombine.GetDebetBalance(
                    accounts,
                    x => !x.IsLoan,
                    x => x.IsReturnLoan,
                    x => x.PaymentDate >= _startDate && x.PaymentDate <= _endDate);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRow2");

            var result = new Dictionary<long, RealtyBalance>();

            int index = 0;

            foreach (var account in accounts)
            {
                section.ДобавитьСтроку();
                section["index"] = ++index;
                section["mr"] = account.RealityObject.Municipality.Name;
                section["mo"] = account.RealityObject.MoSettlement.Return(x => x.Name);
                section["place"] = account.RealityObject.FiasAddress.PlaceName;
                section["address"] = account.RealityObject.Address;
                result[account.Id] = new RealtyBalance(account, balances, loanTakeBalances, loanReturnBalances) { Id = account.Id };
                FillSection(section, result[account.Id]);
            }

            return result;
        }

        private Dictionary<long, RealtyBalance> FillPage3(RealityObjectPaymentAccount[] accounts, ReportParams reportParams)
        {
            var balances =
                TransferCombine.GetCreditBalance(
                    accounts,
                    x => !x.IsLoan,
                    x => !x.IsReturnLoan,
                    x => x.PaymentDate >= _startDate && x.PaymentDate <= _endDate);

            var loanTakeBalances =
                TransferCombine.GetCreditBalance(
                    accounts,
                    x => x.IsLoan,
                    x => !x.IsReturnLoan,
                    x => x.PaymentDate >= _startDate && x.PaymentDate <= _endDate);

            var loanReturnBalances =
                TransferCombine.GetCreditBalance(
                    accounts,
                    x => !x.IsLoan,
                    x => x.IsReturnLoan,
                    x => x.PaymentDate >= _startDate && x.PaymentDate <= _endDate);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRow3");

            var result = new Dictionary<long, RealtyBalance>();

            int index = 0;

            foreach (var account in accounts)
            {
                section.ДобавитьСтроку();
                section["index"] = ++index;
                section["mr"] = account.RealityObject.Municipality.Name;
                section["mo"] = account.RealityObject.MoSettlement.Return(x => x.Name);
                section["place"] = account.RealityObject.FiasAddress.PlaceName;
                section["address"] = account.RealityObject.Address;
                result[account.Id] = new RealtyBalance(account, balances, loanTakeBalances, loanReturnBalances) { Id = account.Id };
                FillSection(section, result[account.Id]);
            }

            return result;
        }

        private Dictionary<long, RealtyBalance> FillPage4(
            RealityObjectPaymentAccount[] accounts,
            ReportParams reportParams,
            Dictionary<long, RealtyBalance> page1Data,
            Dictionary<long, RealtyBalance> page2Data,
            Dictionary<long, RealtyBalance> page3Data)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRow4");

            var result = new Dictionary<long, RealtyBalance>();

            int index = 0;

            foreach (var account in accounts)
            {
                var p1 = page1Data.Get(account.Id);
                var p2 = page2Data.Get(account.Id);
                var p3 = page3Data.Get(account.Id);

                var rb = result[account.Id] = new RealtyBalance { Id = account.Id };

                rb.Add(p1);
                rb.Add(p2);
                rb.Substruct(p3);

                section.ДобавитьСтроку();

                section["index"] = ++index;
                section["mr"] = account.RealityObject.Municipality.Name;
                section["mo"] = account.RealityObject.MoSettlement.Return(x => x.Name);
                section["place"] = account.RealityObject.FiasAddress.PlaceName;
                section["address"] = account.RealityObject.Address;
                FillSection(section, rb);
            }

            return result;
        }

        private void FillPage5(
            RealityObjectPaymentAccount[] accounts,
            ReportParams reportParams,
            Dictionary<long, RealtyBalance> page1Data,
            Dictionary<long, RealtyBalance> page2Data,
            Dictionary<long, RealtyBalance> page3Data,
            Dictionary<long, RealtyBalance> page4Data)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRow5");

            int index = 0;

            foreach (var account in accounts)
            {
                section.ДобавитьСтроку();

                section["index"] = ++index;
                section["mr"] = account.RealityObject.Municipality.Name;
                section["mo"] = account.RealityObject.MoSettlement.Return(x => x.Name);
                section["place"] = account.RealityObject.FiasAddress.PlaceName;
                section["address"] = account.RealityObject.Address;

                section["Остаток"] = page1Data.Get(account.Id).Return(x => x.Total.RegopRoundDecimal(2));
                section["ПоступилоЗаПериод"] = page2Data.Get(account.Id).Return(x => x.Total.RegopRoundDecimal(2));
                section["РасходЗаПериод"] = page3Data.Get(account.Id).Return(x => x.Total.RegopRoundDecimal(2));
                section["Остаток2"] = page4Data.Get(account.Id).Return(x => x.Total.RegopRoundDecimal(2));

                page1Data.Remove(account.Id);
                page2Data.Remove(account.Id);
                page3Data.Remove(account.Id);
                page4Data.Remove(account.Id);
            }
        }

        private void FillSection(Section section, RealtyBalance rec)
        {
            section["МинимальныйВзнос"] = rec.BaseTariff.RegopRoundDecimal(2);
            section["СверхМинимальногоВзноса"] = rec.DecisionTariff.RegopRoundDecimal(2);
            section["Пени"] = rec.Penalty.RegopRoundDecimal(2);
            section["ПроцентНачисленныйБанком"] = rec.BankPercent.RegopRoundDecimal(2);
            section["ИтогоСредствФондаКапитальногоРемонта"] = rec.CrFundTotal.RegopRoundDecimal(2);

            //субсидии

            section["СубсидияФонда"] = rec.FundSubsidy.RegopRoundDecimal(2);
            section["Целевой"] = rec.TargetSubsidy.RegopRoundDecimal(2);
            section["Региональной"] = rec.RegionalSubsidy.RegopRoundDecimal(2);
            section["Стимулирующей"] = rec.StimulateSubsidy.RegopRoundDecimal(2);
            section["ИтогоСубсидий"] = rec.SubsidyTotal.RegopRoundDecimal(2);

            //остальные

            section["ОтОплатыАренды"] = rec.Rent.RegopRoundDecimal(2);
            section["РанееНакопленныеСредства"] = rec.AccumulatedFunds.RegopRoundDecimal(2);
            section["Прочие"] = rec.OtherSources.RegopRoundDecimal(2);
            section["ИтогоПрочихПоступлений"] = rec.OtherSourcesTotal.RegopRoundDecimal(2);

            //Займы

            section["ЗаСчётМинимальныхВзносов"] = rec.BaseTariffTakeLoan.RegopRoundDecimal(2);
            section["ЗаСчётВзносовСверхМинимальных"] = rec.DecisionTariffTakeLoan.RegopRoundDecimal(2);
            section["ЗаСчётПени"] = rec.PenaltyTakeLoan.RegopRoundDecimal(2);
            section["ИтогоЗаСчётСредствНаКр"] = rec.CrFundTakeLoanTotal.RegopRoundDecimal(2);

            section["ФондыОтЗаймодателя"] = rec.FundSubsidyTakeLoan.RegopRoundDecimal(2);
            section["ЦелевойОтЗаймодателя"] = rec.TargetSubsidyTakeLoan.RegopRoundDecimal(2);
            section["РегиональнойОтЗаймодателя"] = rec.RegionalSubsidyTakeLoan.RegopRoundDecimal(2);
            section["СтимулирующейОтЗаймодателя"] = rec.StimulateSubsidyTakeLoan.RegopRoundDecimal(2);

            section["ИтогоЗаСчётСубсидий"] = rec.SubsidyTakeLoanTotal.RegopRoundDecimal(2);

            section["РанееНакопленныеСредстваЗайм"] = rec.AccumFundsTakeLoan.RegopRoundDecimal(2);

            section["ИтогоПоступленийОтЗаймодателя"] = rec.TakeLoanTotal.RegopRoundDecimal(2);

            //Возврат займов

            section["ЗаCчётМинимальныхВзносовЗаймов"] = rec.BaseTariffReturnLoan.RegopRoundDecimal(2);
            section["ЗаCчётВзносовСверхМинимальныхЗаймов"] = rec.DecisionTariffReturnLoan.RegopRoundDecimal(2);
            section["ПениЗаймов"] = rec.PenaltyReturnLoan.RegopRoundDecimal(2);
            section["ИтогоЗаСчётСредствФондаНаКапитальныйРемонт"] = rec.CrFundReturnLoanTotal.RegopRoundDecimal(2);

            section["ВозвратФонды"] = rec.FundSubsidyReturnLoan.RegopRoundDecimal(2);
            section["ВозвратЦелевой"] = rec.TargetSubsidyReturnLoan.RegopRoundDecimal(2);
            section["ВозвратРегиональной"] = rec.RegionalSubsidyReturnLoan.RegopRoundDecimal(2);
            section["ВозвратСтимулирующей"] = rec.StimulateSubsidyReturnLoan.RegopRoundDecimal(2);
            section["ИтогоЗаСчётСубсидийЗаймов"] = rec.SubsidyReturnLoanTotal.RegopRoundDecimal(2);

            section["РанееНакопленныеСредстваВозврат"] = rec.AccumFundsReturnLoan.RegopRoundDecimal(2);
            section["ИтогоВозвратаОтЗанимателя"] = rec.ReturnLoanTotal.RegopRoundDecimal(2);

            section["ИтогоЗаймов"] = rec.LoanTotal.RegopRoundDecimal(2);

            section["ИтогоОстаток"] = rec.Total.RegopRoundDecimal(2);
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            _municipalityIds = baseParams.Params.GetAs("municipalityIds", string.Empty).ToLongArray();
            _settlementIds = baseParams.Params.GetAs("settlementIds", string.Empty).ToLongArray();
            _addressIds = baseParams.Params.GetAs("addressIds", string.Empty).ToLongArray();
            _startDate = baseParams.Params["startDate"].ToDateTime();
            _endDate = baseParams.Params["endDate"].ToDateTime();
        }

        public override string Name
        {
            get { return "Отчет об остатках"; }
        }

        public override string Desciption
        {
            get { return "Отчет об остатках"; }
        }

        public override string GroupName
        {
            get { return "Региональный фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.BalanceReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.BalanceReport"; }
        }

        protected class RealtyBalance
        {
            public RealtyBalance()
            {
                
            }

            public RealtyBalance(
                RealityObjectPaymentAccount acc,
                Dictionary<string, WalletBalance> balances,
                Dictionary<string, WalletBalance> takeLoan,
                Dictionary<string, WalletBalance> returnLoan)
            {
                BaseTariff = balances.Get(acc.BaseTariffPaymentWallet.WalletGuid).Return(x => x.Amount);
                DecisionTariff = balances.Get(acc.DecisionPaymentWallet.WalletGuid).Return(x => x.Amount);
                Penalty = balances.Get(acc.PenaltyPaymentWallet.WalletGuid).Return(x => x.Amount);
                BankPercent = balances.Get(acc.BankPercentWallet.WalletGuid).Return(x => x.Amount);

                //субсидии

                FundSubsidy = balances.Get(acc.FundSubsidyWallet.WalletGuid).Return(x => x.Amount);
                TargetSubsidy = balances.Get(acc.TargetSubsidyWallet.WalletGuid).Return(x => x.Amount);
                RegionalSubsidy = balances.Get(acc.RegionalSubsidyWallet.WalletGuid).Return(x => x.Amount);
                StimulateSubsidy = balances.Get(acc.StimulateSubsidyWallet.WalletGuid).Return(x => x.Amount);

                //остальные

                Rent = balances.Get(acc.RentWallet.WalletGuid).Return(x => x.Amount);
                AccumulatedFunds = balances.Get(acc.AccumulatedFundWallet.WalletGuid).Return(x => x.Amount);

                PrevWorksPayment = balances.Get(acc.PreviosWorkPaymentWallet.WalletGuid).Return(x => x.Amount);
                OtherSources = balances.Get(acc.OtherSourcesWallet.WalletGuid).Return(x => x.Amount);
                SocialSupport = balances.Get(acc.SocialSupportWallet.WalletGuid).Return(x => x.Amount);

                //Займы

                BaseTariffTakeLoan = takeLoan.Get(acc.BaseTariffPaymentWallet.WalletGuid).Return(x => x.Amount);
                DecisionTariffTakeLoan = takeLoan.Get(acc.DecisionPaymentWallet.WalletGuid).Return(x => x.Amount);
                PenaltyTakeLoan = takeLoan.Get(acc.PenaltyPaymentWallet.WalletGuid).Return(x => x.Amount);

                FundSubsidyTakeLoan = takeLoan.Get(acc.FundSubsidyWallet.WalletGuid).Return(x => x.Amount);
                TargetSubsidyTakeLoan = takeLoan.Get(acc.TargetSubsidyWallet.WalletGuid).Return(x => x.Amount);
                RegionalSubsidyTakeLoan = takeLoan.Get(acc.RegionalSubsidyWallet.WalletGuid).Return(x => x.Amount);
                StimulateSubsidyTakeLoan = takeLoan.Get(acc.StimulateSubsidyWallet.WalletGuid).Return(x => x.Amount);

                AccumFundsTakeLoan = takeLoan.Get(acc.AccumulatedFundWallet.WalletGuid).Return(x => x.Amount);

                //Возврат займов

                BaseTariffReturnLoan = returnLoan.Get(acc.BaseTariffPaymentWallet.WalletGuid).Return(x => x.Amount);
                DecisionTariffReturnLoan = returnLoan.Get(acc.DecisionPaymentWallet.WalletGuid).Return(x => x.Amount);
                PenaltyReturnLoan = returnLoan.Get(acc.PenaltyPaymentWallet.WalletGuid).Return(x => x.Amount);

                FundSubsidyReturnLoan = returnLoan.Get(acc.FundSubsidyWallet.WalletGuid).Return(x => x.Amount);
                TargetSubsidyReturnLoan = returnLoan.Get(acc.TargetSubsidyWallet.WalletGuid).Return(x => x.Amount);
                RegionalSubsidyReturnLoan = returnLoan.Get(acc.RegionalSubsidyWallet.WalletGuid).Return(x => x.Amount);
                StimulateSubsidyReturnLoan = returnLoan.Get(acc.StimulateSubsidyWallet.WalletGuid).Return(x => x.Amount);

                AccumFundsReturnLoan = returnLoan.Get(acc.AccumulatedFundWallet.WalletGuid).Return(x => x.Amount);
            }

            public long Id { get; set; }

            //балансы

            public decimal BaseTariff { get; set; }
            public decimal DecisionTariff { get; set; }
            public decimal Penalty { get; set; }
            public decimal BankPercent { get; set; }

            public decimal CrFundTotal
            {
                get { return BaseTariff + DecisionTariff + Penalty + BankPercent; }
            }

            public decimal FundSubsidy { get; set; }
            public decimal TargetSubsidy { get; set; }
            public decimal RegionalSubsidy { get; set; }
            public decimal StimulateSubsidy { get; set; }

            public decimal SubsidyTotal
            {
                get { return FundSubsidy + TargetSubsidy + RegionalSubsidy + StimulateSubsidy; }
            }

            public decimal AccumulatedFunds { get; set; }
            public decimal Rent { get; set; }
            public decimal OtherSources { get; set; }
            public decimal PrevWorksPayment { get; set; }
            public decimal SocialSupport { get; set; }

            public decimal OtherSourcesTotal
            {
                get { return AccumulatedFunds + Rent + OtherSources + PrevWorksPayment + SocialSupport; }
            }

            //(займы) поступления от займодателя

            public decimal BaseTariffTakeLoan { get; set; }
            public decimal DecisionTariffTakeLoan { get; set; }
            public decimal PenaltyTakeLoan { get; set; }

            public decimal CrFundTakeLoanTotal
            {
                get { return BaseTariffTakeLoan + DecisionTariffTakeLoan + PenaltyTakeLoan; }
            }

            public decimal FundSubsidyTakeLoan { get; set; }
            public decimal TargetSubsidyTakeLoan { get; set; }
            public decimal RegionalSubsidyTakeLoan { get; set; }
            public decimal StimulateSubsidyTakeLoan { get; set; }

            public decimal SubsidyTakeLoanTotal
            {
                get
                {
                    return FundSubsidyTakeLoan
                           + TargetSubsidyTakeLoan
                           + RegionalSubsidyTakeLoan
                           + StimulateSubsidyTakeLoan;
                }
            }

            public decimal AccumFundsTakeLoan { get; set; }

            public decimal TakeLoanTotal
            {
                get { return CrFundTakeLoanTotal + SubsidyTakeLoanTotal + AccumFundsTakeLoan; }
            }

            //(займы) возвраты от занимателя

            public decimal BaseTariffReturnLoan { get; set; }
            public decimal DecisionTariffReturnLoan { get; set; }
            public decimal PenaltyReturnLoan { get; set; }

            public decimal CrFundReturnLoanTotal
            {
                get { return BaseTariffReturnLoan + DecisionTariffReturnLoan + PenaltyReturnLoan; }
            }

            public decimal FundSubsidyReturnLoan { get; set; }
            public decimal TargetSubsidyReturnLoan { get; set; }
            public decimal RegionalSubsidyReturnLoan { get; set; }
            public decimal StimulateSubsidyReturnLoan { get; set; }

            public decimal SubsidyReturnLoanTotal
            {
                get
                {
                    return FundSubsidyReturnLoan
                           + TargetSubsidyReturnLoan
                           + RegionalSubsidyReturnLoan
                           + StimulateSubsidyReturnLoan;
                }
            }

            public decimal AccumFundsReturnLoan { get; set; }

            public decimal ReturnLoanTotal
            {
                get { return CrFundReturnLoanTotal + SubsidyReturnLoanTotal + AccumFundsReturnLoan; }
            }

            public decimal LoanTotal
            {
                get { return TakeLoanTotal + ReturnLoanTotal; }
            }

            public decimal Total
            {
                get { return CrFundTotal + SubsidyTotal + OtherSourcesTotal + LoanTotal; }
            }

            public void Add(RealtyBalance rb)
            {
                if (rb == null || Id != rb.Id)
                {
                    return;
                }

                foreach (var property in Properties)
                {
                    var valueToAdd = (decimal) property.Get.Invoke(rb, null);

                    var oldValue = (decimal) property.Get.Invoke(this, null);

                    var newValue = oldValue + valueToAdd;

                    property.Set.Invoke(this, new object[] {newValue});
                }
            }

            public void Substruct(RealtyBalance rb)
            {
                if (rb == null || Id != rb.Id)
                {
                    return;
                }

                foreach (var property in Properties)
                {
                    var valueToAdd = (decimal) property.Get.Invoke(rb, null);

                    var oldValue = (decimal) property.Get.Invoke(this, null);

                    var newValue = oldValue - valueToAdd;

                    property.Set.Invoke(this, new object[] {newValue});
                }
            }

            private GetSetMethods[] Properties
            {
                get
                {
                    return _properties ?? (_properties = typeof (RealtyBalance)
                        .GetProperties()
                        .Where(x => x.CanWrite && x.CanRead)
                        .Where(x => x.PropertyType == typeof (decimal))
                        .Select(x => new GetSetMethods
                        {
                            Get = x.GetGetMethod(),
                            Set = x.GetSetMethod()
                        })
                        .ToArray());
                }
            }

            private GetSetMethods[] _properties;

            private class GetSetMethods
            {
                public MethodInfo Get { get; set; }

                public MethodInfo Set { get; set; }
            }
        }
    }
}