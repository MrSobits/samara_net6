namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Loan;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    public class RepairPaymentAmountReport : BasePrintForm
    {
        public RepairPaymentAmountReport()
            : base(new ReportTemplateBinary(Properties.Resources.RepairPaymentAmountReport))
        {
        }

        #region Properties

        public IWindsorContainer Container { get; set; }

        private long roId;
        
        private DateTime startDate;

        private DateTime endDate;

        public override string Name
        {
            get { return "Размер средств, направленныx на КР"; }
        }

        public override string Desciption
        {
            get { return "Размер средств, направленныx на КР"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.RepairPaymentAmountReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.RepairPaymentAmountReport"; }
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
            var ro = Container.Resolve<IDomainService<RealityObject>>().Get(roId);

            if (ro == null)
            {
                return;
            }

            reportParams.SimpleReportParams["DateStart"] = startDate.ToShortDateString();
            reportParams.SimpleReportParams["DateEnd"] = endDate.ToShortDateString();
            reportParams.SimpleReportParams["Mo"] = ro.Municipality.Name;
            reportParams.SimpleReportParams["Ms"] = ro.MoSettlement.Name;
            var query =
                Container.Resolve<IDomainService<CrFundFormationDecision>>()
                    .GetAll()
                    .Where(
                        x =>
                        x.Protocol.RealityObject.Id == roId && x.Protocol.ProtocolDate <= endDate
                        && x.Protocol.State.FinalState);

            var crFundDecision = query.FirstOrDefault(x => x.Protocol.ProtocolDate == query.Max(y => y.Protocol.ProtocolDate));

            reportParams.SimpleReportParams["MethodFormFundCr"] = crFundDecision != null
                                                                               ? crFundDecision.Decision.GetEnumMeta()
                                                                                     .Display
                                                                               : string.Empty;

            reportParams.SimpleReportParams["Address"] = ro.Address;

            var contract = Container.Resolve<IManagingOrgRealityObjectService>().GetCurrentManOrg(ro);

            reportParams.SimpleReportParams["TypeContractManOrgRealObj"] = contract != null
                                                           ? contract.TypeContractManOrgRealObj.GetEnumMeta().Display
                                                           : string.Empty;

            var accountQuery =
                Container.Resolve<IDomainService<AccountOwnerDecision>>()
                    .GetAll().Where(
                    x =>
                    x.Protocol.RealityObject.Id == roId && x.Protocol.ProtocolDate <= endDate
                    && x.Protocol.State.FinalState).Select(x => new { x.Protocol.ProtocolDate, x.DecisionType });

            var accountOwner =
                accountQuery.FirstOrDefault(x => x.ProtocolDate == accountQuery.Max(y => y.ProtocolDate));

            reportParams.SimpleReportParams["Owner"] = accountOwner != null
                                                           ? accountOwner.DecisionType.GetEnumMeta().Display
                                                           : string.Empty;

            var creditList =
                Container.Resolve<IDomainService<RealityObjectPaymentAccountOperation>>()
                    .GetAll()
                    .Where(
                        x =>
                        x.Account.RealityObject.Id == roId
                        && x.OperationType == PaymentOperationType.OutcomeAccountPayment && x.Date <= endDate
                        && x.Date >= startDate);
            
            reportParams.SimpleReportParams["credit"] = creditList.Any() ? creditList.Sum(x => x.OperationSum) : 0;

            var loanList =
                Container.Resolve<IDomainService<RealityObjectLoan>>().GetAll().Where(x => x.LoanTaker.RealityObject.Id == roId);

            reportParams.SimpleReportParams["loan"] = loanList.Any() ? loanList.Sum(x => x.LoanSum - x.LoanReturnedSum) : 0;

            var suppOperations =
                Container.Resolve<IDomainService<RealityObjectSupplierAccountOperation>>()
                    .GetAll()
                    .Where(x => x.Account.RealityObject.Id == roId && x.Date <= endDate && x.Date >= startDate)
                    .Select(x => new { x.Credit, x.Debt })
                    .ToList();

            reportParams.SimpleReportParams["saldo"] = suppOperations.SafeSum(x => x.Debt) - suppOperations.SafeSum(x => x.Credit);
        }
    }
}