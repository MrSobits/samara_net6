namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.Utils;

    public class SpecialAccountDecisionReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<SpecialAccountDecision> SpecialAccountDecisionDomain { get; set; }

        public IDomainService<BasePropertyOwnerDecision> BasePropertyOwnerDecisionDomain { get; set; }

        public IRealityObjectsProgramVersion RealObjProgramVersion { get; set; }

        #endregion

        private List<long> municipalityIds;

        private DateTime dateTimeReport;

        private YesNoNotSet hasInDpkr;

        private bool byActualHolders;

        public SpecialAccountDecisionReport()
            : base(new ReportTemplateBinary(Properties.Resources.SpecialAccountDecision))
        {
        }

        public override string Name => "Реестр специальных счетов";

        public override string Desciption => "Реестр специальных счетов";

        public override string GroupName => "Региональная программа";

        public override string ParamsController => "B4.controller.report.SpecialAccountDecision";

        public override string RequiredPermission => "Ovrhl.SpecialAccountDecisionReport";

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            this.municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();

            var date = baseParams.Params.GetAs<DateTime?>("dateTimeReport");

            this.dateTimeReport = date ?? DateTime.Now.Date;

            this.hasInDpkr = baseParams.Params.GetAs("hasInDpkr", YesNoNotSet.NotSet);

            this.byActualHolders = baseParams.Params.GetAs<bool>("byActualHolders");
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var roInDpkrQuery = this.RealObjProgramVersion.GetMainVersionRealityObjects();

            var records = this.SpecialAccountDecisionDomain.GetAll()
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.PropertyOwnerProtocol != null && x.PropertyOwnerProtocol.DocumentDate != null)
                .Where(x => x.RealityObject.TypeHouse == TypeHouse.ManyApartments)
                .Where(x => x.RealityObject.ConditionHouse == ConditionHouse.Serviceable || x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated)
                .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming)
                .Where(x => x.MethodFormFund.HasValue && x.MethodFormFund.Value == MethodFormFundCr.SpecialAccount)
                .WhereIf(this.hasInDpkr == YesNoNotSet.Yes, x => roInDpkrQuery.Any(y => y.Id == x.RealityObject.Id))
                .WhereIf(this.hasInDpkr == YesNoNotSet.No, x => !roInDpkrQuery.Any(y => y.Id == x.RealityObject.Id)
                         && (x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated || x.RealityObject.ConditionHouse == ConditionHouse.Serviceable))
                .WhereIf(this.hasInDpkr == YesNoNotSet.NotSet, x =>
                     x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated || x.RealityObject.ConditionHouse == ConditionHouse.Serviceable)
                .Select(x => new
                {
                    Municipality = x.RealityObject.Municipality.Name,
                    RoId = x.RealityObject.Id,
                    x.RealityObject.Address,
                    x.TypeOrganization,
                    ManOrgName = x.ManagingOrganization.Contragent.Name,
                    ManOrgInn = x.ManagingOrganization.Contragent.Inn,
                    RegOpName = x.RegOperator.Contragent.Name,
                    RegOpInn = x.RegOperator.Contragent.Inn,
                    x.AccountNumber,
                    x.PropertyOwnerProtocol.DocumentDate,
                    x.OpenDate,
                    x.CloseDate,
                    BankName = x.CreditOrg.Name,
                    MailingAddress = x.MailingAddress.AddressName,
                    x.Inn,
                    x.Kpp,
                    x.Ogrn,
                    x.Bik,
                    x.Okpo,
                    x.CorrAccount
                })
                .OrderBy(x => x.Municipality)
                .ThenBy(x => x.Address)
                .ToList();

            reportParams.SimpleReportParams["DateTimeReport"] = this.dateTimeReport.Date.ToShortDateString();

            if (!records.Any())
            {
                return;
            }

            if (this.byActualHolders)
            {
                var roIds = records
                    .Select(y => y.RoId)
                    .ToArray();

                // Выбираем дома, у которых по последнему решению
                // метод формирования фонда = "На счете регионального оператора"
                var outRoIds = this.BasePropertyOwnerDecisionDomain.GetAll()
                    .Where(x => roIds.Contains(x.RealityObject.Id))
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        PropOwnerProtoсolId = x.PropertyOwnerProtocol.Id,
                        PropOwnerProtoсolDate = x.PropertyOwnerProtocol.DocumentDate,
                        DecisionCreateDate = x.ObjectCreateDate,
                        x.MethodFormFund
                    })
                    .AsEnumerable()
                    .GroupBy(x => new { x.RoId, x.PropOwnerProtoсolId, x.PropOwnerProtoсolDate })
                    .GroupBy(x => x.Key.RoId)
                    .Where(x => x
                        .OrderBy(y => y.Key.PropOwnerProtoсolDate)
                        .Last()
                        .OrderBy(z => z.DecisionCreateDate)
                        .Last().MethodFormFund == MethodFormFundCr.RegOperAccount)
                    .Select(z => z.Key)
                    .ToHashSet();

                records = records
                    .Where(x => !outRoIds.Contains(x.RoId))
                    .ToList();
            }

            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
            var i = 1;
            foreach (var record in records)
            {
                sect.ДобавитьСтроку();

                var ownerAccountName = "Непосредственное управление";
                var ownerAccountInn = "";
                switch (record.TypeOrganization)
                {
                    case TypeOrganization.TSJ:
                    case TypeOrganization.JSK:
                    case TypeOrganization.ManOrg:
                        ownerAccountName = record.ManOrgName;
                        ownerAccountInn = record.ManOrgInn;
                        break;
                    case TypeOrganization.RegOperator:
                        ownerAccountName = record.RegOpName;
                        ownerAccountInn = record.RegOpInn;
                        break;
                }

                sect["row"] = i++;
                sect["Mu"] = record.Municipality;
                sect["Address"] = record.Address;
                sect["OwnerAccountName"] = ownerAccountName;
                sect["OwnerAccountInn"] = ownerAccountInn;
                sect["AccountNumber"] = record.AccountNumber;
                sect["OpenDate"] = record.OpenDate;
                sect["CloseDate"] = record.CloseDate;
                sect["BankName"] = record.BankName;
                sect["MailingAddress"] = record.MailingAddress;
                sect["Inn"] = record.Inn;
                sect["Kpp"] = record.Kpp;
                sect["Ogrn"] = record.Ogrn;
                sect["Bik"] = record.Bik;
                sect["Okpo"] = record.Okpo;
                sect["CorrAccount"] = record.CorrAccount;
            }
        }
    }
}