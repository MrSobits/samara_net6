namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhRf.Entities;

    using Castle.Windsor;

    public class FormFundNotSetMkdInfoReport : BasePrintForm
    {
        #region Dependency injection members

        public IWindsorContainer Container { get; set; }

        public IDomainService<BasePropertyOwnerDecision> BasePropertyOwnerDecisionDomain { get; set; }
        public IDomainService<LongTermPrObject> LongTermPrObjectDomain { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<SpecialAccountDecision> SpecialAccountDecisionDomain { get; set; }

        public IDomainService<ContractRfObject> ContractRoDomain { get; set; }

        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        public IDomainService<ManOrgContractRealityObject> ManOrgContractRoDomain { get; set; }

        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        #endregion

        private List<long> municipalityIds;

        private DateTime dateTimeReport;

        private List<TypeHouse> houseTypes;
        private List<ConditionHouse> houseCondition;

        private TypeCollectRealObj typeCollectRealObj;

        private long ProgramCrId;

        public FormFundNotSetMkdInfoReport()
            : base(new ReportTemplateBinary(Properties.Resources.FormFundNotSetMkdInfo))
        {
        }

        public override string Name
        {
            get { return "Сведения о МКД, собственники помещений в которых не выбрали способ формирования фонда капитального ремонта"; }
        }

        public override string Desciption
        {
            get { return "Сведения о МКД, собственники помещений в которых не выбрали способ формирования фонда капитального ремонта"; }
        }

        public override string GroupName
        {
            get { return "Региональная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.FormFundNotSetMkdInfo"; }
        }

        public override string RequiredPermission
        {
            get { return "Ovrhl.FormFundNotSetMkdInfoReport"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs<string>("municipalityIds");

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();

            var date = baseParams.Params.GetAs<DateTime?>("dateTimeReport");

            dateTimeReport = date.HasValue ? date.Value : DateTime.Now.Date;

            var houseTypesList = baseParams.Params.GetAs("houseTypes", string.Empty);
            houseTypes = !string.IsNullOrEmpty(houseTypesList)
                ? houseTypesList.Split(',').Select(id => (TypeHouse) (id.ToInt())).ToList()
                : new List<TypeHouse>();
            var houseConditionList = baseParams.Params.GetAs("houseCondition", string.Empty);
            houseCondition = !string.IsNullOrEmpty(houseConditionList)
                ? houseConditionList.Split(',').Select(id => (ConditionHouse) (id.ToInt())).ToList()
                : new List<ConditionHouse>();

            typeCollectRealObj = baseParams.Params.GetAs<TypeCollectRealObj>("typeCollectRealObj");

            ProgramCrId = baseParams.Params.GetAs<long>("programCrId");
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var ownDecMuQuery =
                BasePropertyOwnerDecisionDomain.GetAll()
                    .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .WhereIf(houseTypes.Any(), x => houseTypes.Contains(x.RealityObject.TypeHouse))
                    .WhereIf(houseCondition.Any(), x => houseCondition.Contains(x.RealityObject.ConditionHouse))
                    .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming);

            var ownDecByRo = BasePropertyOwnerDecisionDomain.GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(houseTypes.Any(), x => houseTypes.Contains(x.RealityObject.TypeHouse))
                .WhereIf(houseCondition.Any(), x => houseCondition.Contains(x.RealityObject.ConditionHouse))
                .Where(x => x.PropertyOwnerDecisionType == PropertyOwnerDecisionType.SelectMethodForming)
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.PropertyOwnerProtocol.DocumentDate,
                    x.ObjectCreateDate,
                    x.MethodFormFund
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.OrderBy(x => x.DocumentDate).ThenBy(x => x.ObjectCreateDate).Last());

            var specAccOwnerByRo = SpecialAccountDecisionDomain.GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(houseTypes.Any(), x => houseTypes.Contains(x.RealityObject.TypeHouse))
                .WhereIf(houseCondition.Any(), x => houseCondition.Contains(x.RealityObject.ConditionHouse))
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.ObjectCreateDate,
                    Owner = x.RegOperator.Contragent.Name ?? x.ManagingOrganization.Contragent.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.OrderBy(x => x.ObjectCreateDate).Last());

            var roProgramCrIds = new List<long>();
            if (ProgramCrId != 0)
            {
                roProgramCrIds = ObjectCrDomain.GetAll()
                    .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .WhereIf(houseTypes.Any(), x => houseTypes.Contains(x.RealityObject.TypeHouse))
                    .WhereIf(houseCondition.Any(), x => houseCondition.Contains(x.RealityObject.ConditionHouse))
                    .WhereIf(ProgramCrId != 0, x => x.ProgramCr.Id == ProgramCrId)
                    .Select(x => x.RealityObject.Id)
                    .AsEnumerable()
                    .Distinct()
                    .ToList();
            }

            var roQuery = RealityObjectDomain.GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Municipality.Id))
                .WhereIf(houseTypes.Any(), x => houseTypes.Contains(x.TypeHouse))
                .WhereIf(houseCondition.Any(), x => houseCondition.Contains(x.ConditionHouse))
                .WhereIf(roProgramCrIds.Any(), x => roProgramCrIds.Contains(x.Id))
                .Select(x => new
                {
                    x.ConditionHouse,
                    x.Id,
                    x.TypeHouse,
                    MuId = x.Municipality.Id,
                    MuName = x.Municipality.Name,
                    x.Address
                })
                .WhereIf(typeCollectRealObj == TypeCollectRealObj.UnspecifiedMethodFormFumd, x => !ownDecMuQuery.Any(y => y.RealityObject.Id == x.Id));

            var contractsByRo = ContractRoDomain.GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(houseTypes.Any(), x => houseTypes.Contains(x.RealityObject.TypeHouse))
                .WhereIf(houseCondition.Any(), x => houseCondition.Contains(x.RealityObject.ConditionHouse))
                .WhereIf(roProgramCrIds.Any(), x => roProgramCrIds.Contains(x.RealityObject.Id))
                .Where(x => (!x.ContractRf.DateBegin.HasValue || x.ContractRf.DateBegin.Value <= dateTimeReport) && (!x.ContractRf.DateEnd.HasValue || dateTimeReport <= x.ContractRf.DateEnd.Value))
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.ContractRf.DocumentNum,
                    DocumentDate = x.ContractRf.DateBegin
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, x => x.First());

            var dictManOrgRo = ManOrgContractRoDomain.GetAll()
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.ManOrgContract.StartDate,
                    x.ManOrgContract.EndDate,
                    TypeManagement = (TypeManagementManOrg?) x.ManOrgContract.ManagingOrganization.TypeManagement,
                    ManOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                    ManOrgContractNum = x.ManOrgContract.DocumentNumber
                })
                .Where(x => roQuery.Select(y => y.Id).Any(y => y == x.RoId))
                .Where(x => x.StartDate <= dateTimeReport && (!x.EndDate.HasValue || x.EndDate >= dateTimeReport))
                .OrderByDescending(x => x.EndDate ?? DateTime.MaxValue.Date)
                .AsEnumerable()
                .Select(x => new
                {
                    x.RoId,
                    TypeManagement = x.TypeManagement.HasValue ? x.TypeManagement.Value.GetEnumMeta().Display : "",
                    x.ManOrgName,
                    x.ManOrgContractNum,
                    x.StartDate
                })
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var records = roQuery
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.MuName,
                    x.Address,
                    x.TypeHouse,
                    x.MuId,
                    x.ConditionHouse,
                    TypeOrganization = dictManOrgRo.ContainsKey(x.Id)
                        ? dictManOrgRo[x.Id].TypeManagement
                        : "",
                    OrganizationName =
                        dictManOrgRo.ContainsKey(x.Id)
                            ? dictManOrgRo[x.Id].ManOrgName
                            : "",
                    ContractNum = contractsByRo.ContainsKey(x.Id)
                        ? contractsByRo[x.Id].DocumentNum
                        : "",
                    ContractStartDate = contractsByRo.ContainsKey(x.Id)
                        ? contractsByRo[x.Id].DocumentDate.ToDateString()
                        : ""
                })
                .OrderBy(x => x.MuName)
                .ThenBy(x => x.Address).ToList();

            var includedList = VersionRecordDomain.GetAll()
                .Where(x => x.ProgramVersion.IsProgramPublished == true)
                .Select(x => x.RealityObject.Id).ToHashSet();

            reportParams.SimpleReportParams["DateReport"] = dateTimeReport.ToShortDateString();

            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
            var i = 1;
            foreach (var record in records)
            {
                sect.ДобавитьСтроку();

                sect["row"] = i++;
                sect["Mu"] = record.MuName;
                sect["Address"] = record.Address;
                sect["TypeOrganization"] = record.TypeOrganization;
                sect["OrganizationName"] = record.OrganizationName;
                sect["TypeHouse"] = record.TypeHouse.GetEnumMeta().Display;
                sect["ConditionHouse"] = record.ConditionHouse.GetEnumMeta().Display;
                sect["Included"] = includedList.Contains(record.Id) ? "да" : "нет";
                sect["UOAggrNum"] = record.ContractNum;
                sect["StartDate"] = record.ContractStartDate;

                if (ownDecByRo.ContainsKey(record.Id))
                {
                    var methodFormFund = ownDecByRo[record.Id].MethodFormFund.To<MethodFormFundCr>();
                    sect["MethodFormFund"] = methodFormFund.GetEnumMeta().Display;
                    sect["AccountOwner"] = methodFormFund == MethodFormFundCr.SpecialAccount && specAccOwnerByRo.ContainsKey(record.Id)
                        ? specAccOwnerByRo[record.Id].Owner
                        : string.Empty;
                }
            }
        }
    }
}