namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    public class SocialSupportReport : BasePrintForm
    {
        public SocialSupportReport()
            : base(new ReportTemplateBinary(Properties.Resources.SocialSupportReport))
        {
        }

        #region Properties

        public IWindsorContainer Container { get; set; }

        public IDomainService<PersonalAccountPayment> PersonalAccountPaymentDomain { get; set; }

        public IDomainService<IndividualAccountOwner> IndividualAccountOwnerDomain { get; set; }

        private DateTime startDate;

        private DateTime endDate;

        public override string Name
        {
            get { return "Отчет поступлений соц. поддержки"; }
        }

        public override string Desciption
        {
            get { return "Отчет поступлений соц. поддержки"; }
        }

        public override string GroupName
        {
            get { return "Региональный фонд"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.SocialSupportReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.SocialSupportReport"; }
        }

        #endregion Properties

        public override void SetUserParams(BaseParams baseParams)
        {          
            startDate = baseParams.Params.GetAs("startDate", DateTime.MinValue);
            endDate = baseParams.Params.GetAs("endDate", DateTime.MaxValue);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var persAccountPaymentQuery = PersonalAccountPaymentDomain
                .GetAll()
                .Where(x => x.PaymentDate >= startDate)
                .Where(x => x.PaymentDate <= endDate)
                .Where(x => x.Type == PaymentType.SocialSupport);

            var socSupportInfo = persAccountPaymentQuery
                .Select(x => new
                {
                    x.BasePersonalAccount.PersonalAccountNum,
                    OwnerId = x.BasePersonalAccount.AccountOwner.Id,
                    x.BasePersonalAccount.Room.RealityObject.Address,
                    Municipality = x.BasePersonalAccount.Room.RealityObject.Municipality.Name,
                    Settlement = x.BasePersonalAccount.Room.RealityObject.MoSettlement.Name,
                    x.Sum
                })
                .ToList();

            var owners = IndividualAccountOwnerDomain.GetAll()
                .Where(x => persAccountPaymentQuery.Any(y => y.BasePersonalAccount.AccountOwner.Id == x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Surname,
                    x.FirstName,
                    x.SecondName
                })
                .ToDictionary(x => x.Id);

            reportParams.SimpleReportParams["StartDate"] = startDate.ToShortDateString();
            reportParams.SimpleReportParams["EndDate"] = endDate.ToShortDateString();


            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var number = 1;
            foreach (var rec in socSupportInfo)
            {
                var owner = owners.Get(rec.OwnerId);

                section.ДобавитьСтроку();
                section["Number"] = number++;
                section["PersAccNumber"] = rec.PersonalAccountNum;
                section["Municipality"] = rec.Municipality;
                section["Settlement"] = rec.Settlement;
                section["Address"] = rec.Address;
                section["SocialSupport"] = rec.Sum;

                if (owner != null)
                {
                    section["Surname"] = owner.Surname;
                    section["Name"] = owner.FirstName;
                    section["Patronymic"] = owner.SecondName;
                }
            }

        }
    }
}