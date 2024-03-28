namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    
    using B4.Modules.Security;
    using B4.Modules.Reports;
    using B4.Utils;
    using Gkh.Entities;

    using Castle.Windsor;
    using Entities;

    /// <summary>
    /// 
    /// </summary>
    public class LoanReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public LoanReport()
            : base(new ReportTemplateBinary(Properties.Resources.LoanReport))
        {
        }

        private string _loanNum;
        private List<RealityObjectLoan> _loanList;

        public override string Name
        {
            get { return "LoanReport"; }
        }

        public override string Desciption
        {
            get { return string.Empty; }
        }

        public override string GroupName
        {
            get { return string.Empty; }
        }

        public override string ParamsController
        {
            get { return string.Empty; }
        }

        public override string RequiredPermission
        {
            get { return string.Empty; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            _loanNum = baseParams.Params.Get("loanCount", string.Empty);
            _loanList = baseParams.Params.GetAs<List<RealityObjectLoan>>("loanList");
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var date = DateTime.Today;
            var repoRo = Container.ResolveRepository<RealityObject>();

            reportParams.SimpleReportParams["НомерРаспоряжения"] = _loanNum;
            reportParams.SimpleReportParams["ТекущаяДата"] = date.ToString("dd.MM.yyyy");

            if (_loanList.Any())
            {
                reportParams.SimpleReportParams["АдресДомаЗанимателя"] =
                    repoRo.Load(_loanList.First().LoanTaker.RealityObject.Id).Address;
            }

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            
            foreach (var loan in _loanList)
            {
                section.ДобавитьСтроку();
                section["Сумма"] = loan.LoanSum;
                section["ДатаЗайма"] = date.ToString("dd.MM.yyyy");
                section["ДатаВозврата"] = loan.FactEndDate.HasValue 
                    ? loan.FactEndDate.Value.ToString("dd.MM.yyyy")
                    : string.Empty;
            }
            
            var userIdentity = Container.Resolve<IUserIdentity>();
            var user = Container.ResolveRepository<User>().GetAll()
                .FirstOrDefault(x => x.Id == userIdentity.UserId);

            reportParams.SimpleReportParams["ФиоОтветственного"] = user != null ? user.Name : string.Empty;
        }
    }
}