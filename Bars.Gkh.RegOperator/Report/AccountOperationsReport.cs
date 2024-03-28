namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    public class AccountOperationsReport : BasePrintForm
    {
        public AccountOperationsReport()
            : base(new ReportTemplateBinary(Properties.Resources.AccountOperationsReport))
        {
        }

        #region Properties

        public IWindsorContainer Container { get; set; }

        public IRealtyObjectMoneyRepository RoMoneyRepository { get; set; }

        private long roId;
        
        private DateTime startDate;

        private DateTime endDate;

        public override string Name
        {
            get { return "Операции по счету"; }
        }

        public override string Desciption
        {
            get { return "Операции по счету"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.AccountOperationsReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.AccountOperationsReport"; }
        }

        #endregion Properties

        public override void SetUserParams(BaseParams baseParams)
        {
            this.roId = baseParams.Params.GetAs<long>("roId");
            
            this.startDate = baseParams.Params.GetAs<DateTime>("startDate");
            this.endDate = baseParams.Params.GetAs<DateTime>("endDate");
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var ro = this.Container.Resolve<IDomainService<RealityObject>>().Get(this.roId);
            var manOrgContractDomain = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();
            var roPaymentAccDomain = this.Container.Resolve<IDomainService<RealityObjectPaymentAccount>>();

            if (ro == null)
            {
                return;
            }

            reportParams.SimpleReportParams["DateStart"] = this.startDate.ToShortDateString();
            reportParams.SimpleReportParams["DateEnd"] = this.endDate.ToShortDateString();
            reportParams.SimpleReportParams["Mo"] = ro.Municipality.Name;
            reportParams.SimpleReportParams["Ms"] = ro.MoSettlement.Name;

            var query = this.Container.Resolve<IDomainService<CrFundFormationDecision>>()
                .GetAll()
                .Where(x => x.Protocol.RealityObject.Id == this.roId && 
                            x.Protocol.ProtocolDate <= this.endDate &&
                            x.Protocol.State.FinalState);

            var crFundDecision = query.FirstOrDefault(x => x.Protocol.ProtocolDate == query.Max(y => y.Protocol.ProtocolDate));

            reportParams.SimpleReportParams["MethodFormFundCr"] = crFundDecision != null
                                                                      ? crFundDecision.Decision.GetEnumMeta().Display
                                                                      : string.Empty;

            reportParams.SimpleReportParams["Address"] = ro.Address;

            var contracts = manOrgContractDomain.GetAll()
                .Where(x => x.RealityObject.Id == ro.Id)
                .Where(x => ((x.ManOrgContract.EndDate == null && x.ManOrgContract.StartDate <= this.endDate) || x.ManOrgContract.EndDate >= this.startDate) ||
                            x.ManOrgContract.StartDate <= this.endDate)
                .Select(x => x.ManOrgContract.TypeContractManOrgRealObj).ToArray();

            reportParams.SimpleReportParams["TypeContractManOrgRealObj"] = contracts.Any()
                                                                               ? string.Join(", ", contracts.Select(x => x.GetEnumMeta().Display))
                                                                               : string.Empty;

            var accountQuery = this.Container.Resolve<IDomainService<AccountOwnerDecision>>().GetAll()
                .Where(x => x.Protocol.RealityObject.Id == this.roId && 
                            x.Protocol.ProtocolDate <= this.endDate &&
                            x.Protocol.State.FinalState)
                .Select(x => new { x.Protocol.ProtocolDate, x.DecisionType });

            var accountOwners = accountQuery.Select(x => x.DecisionType).ToArray();

            reportParams.SimpleReportParams["Owner"] = accountOwners.Any()
                                                           ? string.Join(", ", accountOwners.Select(x => x.GetEnumMeta().Display))
                                                           : string.Empty;

            var roPaymentAccounts = roPaymentAccDomain.GetAll().Where(x => x.RealityObject.Id == this.roId);

            var creditList = this.RoMoneyRepository.GetCreditTransfers(roPaymentAccounts)
                .Where(x => x.OperationDate >= this.startDate && x.OperationDate <= this.endDate);

            var creditSection = reportParams.ComplexReportParams.ДобавитьСекцию("creditRow");

            foreach (var row in creditList.OrderBy(x => x.OperationDate))
            {
                creditSection.ДобавитьСтроку();
                creditSection["Date"] = row.OperationDate;
                creditSection["Type"] = row.Reason;
                creditSection["Sum"] = row.Amount;
            }

            var debetList = this.RoMoneyRepository.GetDebtTransfers(roPaymentAccounts)
                .Where(x => x.OperationDate >= this.startDate && x.OperationDate <= this.endDate);

            var debetSection = reportParams.ComplexReportParams.ДобавитьСекцию("debetRow");

            foreach (var row in debetList.OrderBy(x => x.OperationDate))
            {
                debetSection.ДобавитьСтроку();
                debetSection["Date"] = row.OperationDate;
                debetSection["Type"] = row.Reason;
                debetSection["Sum"] = row.Amount;
            }
        }
    }
}