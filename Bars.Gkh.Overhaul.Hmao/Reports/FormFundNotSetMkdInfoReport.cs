namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;

    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;
    using Config;

    public class FormFundNotSetMkdInfoReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<BasePropertyOwnerDecision> BasePropertyOwnerDecisionDomain { get; set; }
        public IDomainService<LongTermPrObject> LongTermPrObjectDomain { get; set; }
        public IGkhParams GkhParams { get; set; }

        #endregion

        private List<long> municipalityIds;
        private DateTime dateTimeReport;
        private List<TypeHouse> houseTypes;
        private List<ConditionHouse> houseCondition;
        private MoLevel moLevel;

        public FormFundNotSetMkdInfoReport()
            : base(new ReportTemplateBinary(Properties.Resources.FormFundNotSetMkdInfo))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return
                    "Сведения о МКД, собственники помещений в которых не выбрали способ формирования фонда капитального ремонта";
            }
        }

        public override string Desciption
        {
            get
            {
                return
                    "Сведения о МКД, собственники помещений в которых не выбрали способ формирования фонда капитального ремонта";
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
                return "B4.controller.report.FormFundNotSetMkdInfo";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.FormFundNotSetMkdInfoReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();

            var appParams = GkhParams.GetParams();
            moLevel = appParams.ContainsKey("MoLevel")
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            var houseTypesList = baseParams.Params.GetAs("houseTypes", string.Empty);
            houseTypes = !string.IsNullOrEmpty(houseTypesList)
                ? houseTypesList.Split(',').Select(id => (TypeHouse)(id.ToInt())).ToList()
                : new List<TypeHouse>();
            var houseConditionList = baseParams.Params.GetAs("houseCondition", string.Empty);
            houseCondition = !string.IsNullOrEmpty(houseConditionList)
                ? houseConditionList.Split(',').Select(id => (ConditionHouse)(id.ToInt())).ToList()
                : new List<ConditionHouse>();

            dateTimeReport = baseParams.Params.GetAs("dateTimeReport", DateTime.Now);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var ownDecMuIds =
                BasePropertyOwnerDecisionDomain.GetAll()
                    .Where(x => x.MethodFormFund.HasValue && x.MethodFormFund != MethodFormFundCr.NotSet)
                    .Select(x => x.RealityObject.Id);

            var roIds = LongTermPrObjectDomain.GetAll()
                .Where(x => !ownDecMuIds.Any(y => y == x.RealityObject.Id))
                .WhereIf(municipalityIds.Any(), 
                    x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                    || municipalityIds.Contains(x.RealityObject.MoSettlement.Id))
                .WhereIf(houseTypes.Any(), x => houseTypes.Contains(x.RealityObject.TypeHouse))
                .WhereIf(houseCondition.Any(), x => houseCondition.Contains(x.RealityObject.ConditionHouse))
                //.Where(x =>
                //    x.RealityObject.ConditionHouse == ConditionHouse.Dilapidated
                //    || x.RealityObject.ConditionHouse == ConditionHouse.Serviceable)
                .Select(x => x.RealityObject.Id);

            var nowDate = DateTime.Now.Date;

            var dictManOrgRo =
                Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                    .Where(x => roIds.Any(y => y == x.RealityObject.Id))
                    .Where(x => x.ManOrgContract != null)
                    .Where(x => x.ManOrgContract.StartDate <= nowDate
                                && (x.ManOrgContract.EndDate.HasValue && x.ManOrgContract.EndDate >= nowDate
                                    || !x.ManOrgContract.EndDate.HasValue))
                    .OrderByDescending(x => x.ManOrgContract.EndDate)
                    .Select(x => new
                    {
                        TypeManagement = (TypeManagementManOrg?) x.ManOrgContract.ManagingOrganization.TypeManagement,
                        x.ManOrgContract.ManagingOrganization.Contragent.Name,
                        RoId = x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.RoId,
                        TypeManagement = x.TypeManagement.HasValue ? x.TypeManagement.Value.GetEnumMeta().Display : "",
                        ManOrgName = x.Name ?? ""
                    })
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var records =
                LongTermPrObjectDomain.GetAll()
                    .Where(y => roIds.Any(x => x == y.RealityObject.Id))
                    .Select(x => new
                            {
                                x.Id,
                                Municipality = moLevel == MoLevel.MunicipalUnion
                                    ? x.RealityObject.Municipality.Name
                                    : x.RealityObject.MoSettlement.Name,
                                x.RealityObject.Address,
                                TypeOrganization =
                                    dictManOrgRo.ContainsKey(x.RealityObject.Id)
                                        ? dictManOrgRo[x.RealityObject.Id].TypeManagement
                                        : "",
                                OrganizationName =
                                    dictManOrgRo.ContainsKey(x.RealityObject.Id)
                                        ? dictManOrgRo[x.RealityObject.Id].ManOrgName
                                        : "",
                                x.RealityObject.ConditionHouse,
                                x.RealityObject.TypeHouse,
                                roId = x.RealityObject.Id
                            })
                    .OrderBy(x => x.Municipality).ToList();

            reportParams.SimpleReportParams["DateTimeReport"] = dateTimeReport.Date.ToShortDateString();

            if (!records.Any())
            {
                return;
            }

            var includedList =
               Container.Resolve<IDomainService<VersionRecord>>().GetAll()
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .Where(x => x.ProgramVersion.IsMain)
                .Select(x => x.RealityObject.Id)
                .ToList();

            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
            var i = 1;
            foreach (var record in records)
            {
                sect.ДобавитьСтроку();

                sect["row"] = i++;
                sect["Mu"] = record.Municipality;
                sect["Address"] = record.Address;
                sect["TypeOrganization"] = record.TypeOrganization;
                sect["OrganizationName"] = record.OrganizationName;
                sect["TypeHouse"] = record.TypeHouse.GetEnumMeta().Display;
                sect["ConditionHouse"] = record.ConditionHouse.GetEnumMeta().Display;
                sect["Included"] = includedList.Contains(record.roId) ? "да" : "нет";
            }
        }
    }
}