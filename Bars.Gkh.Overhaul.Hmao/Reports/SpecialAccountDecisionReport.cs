namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;

    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Enum;

    using Castle.Windsor;

    public class SpecialAccountDecisionReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<SpecialAccountDecision> SpecialAccountDecisionDomain { get; set; }

        #endregion

        private List<long> municipalityIds;

        private DateTime dateTimeReport;

        public SpecialAccountDecisionReport()
            : base(new ReportTemplateBinary(Properties.Resources.SpecialAccountDecision))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Реестр специальных счетов";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Реестр специальных счетов";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Региональная программа";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.SpecialAccountDecision";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.SpecialAccountDecisionReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();

            dateTimeReport = baseParams.Params.GetAs("dateTimeReport", DateTime.Now.Date);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var records =
                SpecialAccountDecisionDomain.GetAll()
                    .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming)
                    .Where(x => x.MethodFormFund == MethodFormFundCr.SpecialAccount)
                    .WhereIf(municipalityIds.Count > 0,
                        x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                            || municipalityIds.Contains(x.RealityObject.MoSettlement.Id))
                    .Select(x => new
                    {
                        Municipality = x.RealityObject.Municipality.Name,
                        x.RealityObject.Address,
                        x.TypeOrganization,
                        ManOrgName = x.ManagingOrganization.Contragent.Name,
                        //RegOpName = x.RegOperator.Contragent.Name,
                        x.AccountNumber,
                        x.OpenDate,
                        x.CloseDate,
                        BankName = x.CreditOrg.Name,
                        x.CreditOrg.MailingAddress,
                        x.CreditOrg.Inn,
                        x.CreditOrg.Kpp,
                        x.CreditOrg.Ogrn,
                        x.CreditOrg.Bik,
                        x.CreditOrg.CorrAccount
                    })
                    .OrderBy(x => x.Municipality);

            reportParams.SimpleReportParams["DateTimeReport"] = this.dateTimeReport.Date.ToShortDateString();

            if (!records.Any())
            {
                return;
            }

            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
            var i = 1;
            foreach (var record in records)
            {
                sect.ДобавитьСтроку();

                var ownerAccountName = TypeOrganization.DirectManag.GetEnumMeta().Display;
                switch (record.TypeOrganization)
                {
                    case TypeOrganization.TSJ:
                    case TypeOrganization.JSK:
                        ownerAccountName = record.ManOrgName;
                        break;
                    case TypeOrganization.RegOperator:
                       // ownerAccountName = record.RegOpName;
                        break;
                }

                sect["row"]                 = i++;
                sect["Mu"]                  = record.Municipality;
                sect["Address"]             = record.Address;
                sect["OwnerAccountName"]    = ownerAccountName;
                sect["AccountNumber"]       = record.AccountNumber;
                sect["OpenDate"]            = record.OpenDate;
                sect["CloseDate"]           = record.CloseDate;
                sect["BankName"]            = record.BankName;
                sect["MailingAddress"]      = record.MailingAddress;
                sect["Inn"]                 = record.Inn;
                sect["Kpp"]                 = record.Kpp;
                sect["Ogrn"]                = record.Ogrn;
                sect["Bik"]                 = record.Bik;
                sect["CorrAccount"]         = record.CorrAccount;
            }
        }
    }
}