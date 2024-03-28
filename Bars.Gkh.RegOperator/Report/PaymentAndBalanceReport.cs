namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    public class PaymentAndBalanceReport : BasePrintForm
    {
        public PaymentAndBalanceReport()
            : base(new ReportTemplateBinary(Properties.Resources.PaymentAndBalanceReport))
        {
        }

        #region Properties

        public IWindsorContainer Container { get; set; }

        private long roId;
        
        private DateTime startDate;

        private DateTime endDate;

        public override string Name
        {
            get { return "Сумма начислений и остаток средств"; }
        }

        public override string Desciption
        {
            get { return "Сумма начислений и остаток средств"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.PaymentAndBalanceReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.PaymentAndBalanceReport"; }
        }

        #endregion Properties

        public override void SetUserParams(BaseParams baseParams)
        {
            roId = baseParams.Params.GetAs<long>("roId");
            
            startDate = baseParams.Params.GetAs<DateTime>("startDate");
            endDate = baseParams.Params.GetAs<DateTime>("endDate");
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var roDomainService = Container.Resolve<IDomainService<RealityObject>>();
            var manOrgContractDomain = Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var accountOwnerDomain = Container.Resolve<IDomainService<AccountOwnerDecision>>();
            var roPaymentOpDomain = Container.Resolve<IDomainService<RealityObjectPaymentAccountOperation>>();
            var roPaymentAccDomain = Container.Resolve<IDomainService<RealityObjectPaymentAccount>>();

            try
            {
                var ro = roDomainService.Get(roId);

                if (ro == null)
                {
                    return;
                }

                reportParams.SimpleReportParams["DateStart"] = startDate.ToShortDateString();
                reportParams.SimpleReportParams["DateEnd"] = endDate.ToShortDateString();
                reportParams.SimpleReportParams["Mo"] = ro.Municipality.Name;
                reportParams.SimpleReportParams["Ms"] = ro.MoSettlement.Name;
                reportParams.SimpleReportParams["MethodFormFundCr"] = ro.MethodFormFundCr.GetEnumMeta().Display;
                reportParams.SimpleReportParams["Address"] = ro.Address;

                var realEndDate = endDate.AddDays(1).AddMilliseconds(-1);

                var contracts =
                    manOrgContractDomain.GetAll()
                        .Where(x => x.RealityObject.Id == ro.Id)
                        .Where(
                            x =>
                            x.ManOrgContract.EndDate.HasValue && x.ManOrgContract.EndDate <= realEndDate
                            && x.ManOrgContract.StartDate >= startDate)
                        .Select(x => x.ManOrgContract.TypeContractManOrgRealObj).ToArray();

                reportParams.SimpleReportParams["TypeContractManOrgRealObj"] = contracts.Any()
                                                                                   ? string.Join(
                                                                                       ", ",
                                                                                       contracts.Select(
                                                                                           x => x.GetEnumMeta().Display))
                                                                                   : string.Empty;
                var accountQuery =
                    accountOwnerDomain.GetAll()
                        .Where(
                            x =>
                            x.Protocol.RealityObject.Id == roId && x.Protocol.ProtocolDate <= realEndDate
                            && x.Protocol.State.FinalState)
                        .Select(x => new { x.Protocol.ProtocolDate, x.DecisionType });

                var accountOwner =
                    accountQuery.FirstOrDefault(x => x.ProtocolDate == accountQuery.Max(y => y.ProtocolDate));

                reportParams.SimpleReportParams["Owner"] = accountOwner != null
                                                               ? accountOwner.DecisionType.GetEnumMeta().Display
                                                               : string.Empty;

                var operations =
                    roPaymentOpDomain.GetAll()
                        .Where(x => x.Account.RealityObject.Id == roId && x.Date >= startDate && x.Date <= realEndDate)
                        .Select(x => new { x.OperationSum, x.OperationType })
                        .ToArray();

                var debitOperations =
                    operations.Where(
                        x =>
                        x.OperationType != PaymentOperationType.OutcomeAccountPayment
                        && x.OperationType != PaymentOperationType.ExpenseLoan
                        && x.OperationType != PaymentOperationType.OutcomeLoan
                        && x.OperationType != PaymentOperationType.CashService
                        && x.OperationType != PaymentOperationType.OpeningAcc
                        && x.OperationType != PaymentOperationType.CreditPayment
                        && x.OperationType != PaymentOperationType.CreditPercentPayment).ToArray();

                var creditOperations =
                    operations.Where(
                        x =>
                        x.OperationType == PaymentOperationType.OutcomeAccountPayment
                        || x.OperationType == PaymentOperationType.ExpenseLoan
                        || x.OperationType == PaymentOperationType.OutcomeLoan
                        || x.OperationType == PaymentOperationType.OpeningAcc
                        || x.OperationType == PaymentOperationType.CashService
                        || x.OperationType == PaymentOperationType.CreditPayment
                        || x.OperationType == PaymentOperationType.CreditPercentPayment).ToArray();

                var debit = debitOperations.Any() ? debitOperations.Sum(x => x.OperationSum) : 0m;
                var credit = creditOperations.Any() ? creditOperations.Sum(x => x.OperationSum) : 0m;

                reportParams.SimpleReportParams["debit"] = debit;

                reportParams.SimpleReportParams["saldo"] = debit - credit;
            }
            finally
            {
                Container.Release(roDomainService);
                Container.Release(manOrgContractDomain);
                Container.Release(accountOwnerDomain);
                Container.Release(roPaymentOpDomain);
                Container.Release(roPaymentAccDomain);
            }

        }
    }
}