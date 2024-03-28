using Bars.Gkh.Config;
using Bars.Gkh.Overhaul.Entities;
using NHibernate.Hql.Ast.ANTLR;

namespace Bars.Gkh.Overhaul.Nso.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;    
    using B4.Modules.Reports;
    
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    
    using Castle.Windsor;
    using Entities;

    public class FormFundNotSetMkdInfoReport : BasePrintForm
    {
        #region Dependency injection members

        public IWindsorContainer Container { get; set; }

        public IDomainService<BasePropertyOwnerDecision> BasePropertyOwnerDecisionDomain { get; set; }
        public IDomainService<LongTermPrObject> LongTermPrObjectDomain { get; set; }

        #endregion

        private List<long> municipalityIds;

        private DateTime dateTimeReport;

        private List<TypeHouse> houseTypes;
        private List<ConditionHouse> houseCondition;

        private Boolean WithoutPhysicalWear;

        public IGkhParams GkhParams { get; set; }

        public FormFundNotSetMkdInfoReport()
            : base(new ReportTemplateBinary(Properties.Resources.FormFundNotSetMkdInfo))
        {

        }

        public override string Name
        {
            get
            {
                return "Сведения о МКД, собственники помещений в которых не выбрали способ формирования фонда капитального ремонта";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Сведения о МКД, собственники помещений в которых не выбрали способ формирования фонда капитального ремонта";
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
            var strMunicpalIds = baseParams.Params.GetAs<string>("municipalityIds");

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();

            var date = baseParams.Params.GetAs<DateTime?>("dateTimeReport");

            WithoutPhysicalWear = baseParams.Params.GetAs<Boolean>("withoutPhysicalWear");

            dateTimeReport = date.HasValue ? date.Value : DateTime.Now.Date;

            var houseTypesList = baseParams.Params.GetAs("houseTypes", string.Empty);
            houseTypes = !string.IsNullOrEmpty(houseTypesList)
                ? houseTypesList.Split(',').Select(id => (TypeHouse) (id.ToInt())).ToList()
                : new List<TypeHouse>();

            var houseConditionList = baseParams.Params.GetAs("houseCondition", string.Empty);
            houseCondition = !string.IsNullOrEmpty(houseConditionList)
                ? houseConditionList.Split(',').Select(id => (ConditionHouse) (id.ToInt())).ToList()
                : new List<ConditionHouse>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var ownDecMuIds =
                BasePropertyOwnerDecisionDomain.GetAll()
                    .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .Where(x => x.MethodFormFund.HasValue && x.MethodFormFund != MethodFormFundCr.NotSet)
                    .Where(x => x.RealityObject.TypeHouse == TypeHouse.ManyApartments)
                    .Select(x => x.RealityObject.Id);

            var roIds =
                LongTermPrObjectDomain.GetAll()
                    .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .WhereIf(houseTypes.Any(), x => houseTypes.Contains(x.RealityObject.TypeHouse))
                    .WhereIf(houseCondition.Any(), x => houseCondition.Contains(x.RealityObject.ConditionHouse))
                    .WhereIf(WithoutPhysicalWear, x => !x.RealityObject.PhysicalWear.HasValue || x.RealityObject.PhysicalWear.Value < 70)
                    .Select(x => new {x.RealityObject.ConditionHouse, RoId = x.RealityObject.Id, x.RealityObject.TypeHouse})
                    .Where(x => !ownDecMuIds.Any(y => y == x.RoId))
                   // .Where(x => x.ConditionHouse == ConditionHouse.Dilapidated || x.ConditionHouse == ConditionHouse.Serviceable)
                    .Select(x => x.RoId);

            var dictManOrgRo =
                Container.Resolve<IDomainService<ManOrgContractRealityObject>>().GetAll()
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.EndDate,
                        TypeManagement = (TypeManagementManOrg?)x.ManOrgContract.ManagingOrganization.TypeManagement,
                        ManOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name
                    })
                    .Where(x => roIds.Any(y => y == x.RoId))
                    .Where(x => x.StartDate <= dateTimeReport && (!x.EndDate.HasValue || x.EndDate >= dateTimeReport))
                    .OrderByDescending(x => x.EndDate ?? DateTime.MaxValue.Date)
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.RoId,
                        TypeManagement = x.TypeManagement.HasValue ? x.TypeManagement.Value.GetEnumMeta().Display : "",
                        x.ManOrgName
                    })
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            var records =
                LongTermPrObjectDomain.GetAll()
                    .Where(y => roIds.Any(x => x == y.RealityObject.Id))
                    .Select(x => new
                    {
                        MuName = x.RealityObject.Municipality.Name,
                        MoName = x.RealityObject.MoSettlement.Name,
                        x.RealityObject.Address,
                        x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.MuName,
                        x.MoName,
                        x.Address,
                        TypeOrganization = dictManOrgRo.ContainsKey(x.Id)
                            ? dictManOrgRo[x.Id].TypeManagement
                            : "",
                        OrganizationName =
                            dictManOrgRo.ContainsKey(x.Id)
                                ? dictManOrgRo[x.Id].ManOrgName
                                : ""
                    })
                    .OrderBy(x => x.MuName)
                    .ThenBy(x => x.Address);

            reportParams.SimpleReportParams["DateReport"] = dateTimeReport.ToShortDateString();

            var appParams = GkhParams.GetParams();

            var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            var moOption = moLevel == MoLevel.Settlement;


            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("vertSection");

            verticalSection.ДобавитьСтроку();
            verticalSection["header"] = "Муниципальный район";
            verticalSection["num"] = "2";
            verticalSection["value"] = "$Mu$";

            if (moOption)
            {
                verticalSection.ДобавитьСтроку();
                verticalSection["header"] = "Муниципальное образование";
                verticalSection["num"] = "2";
                verticalSection["value"] = "$Mo$";
            }
            
            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
            var i = 1;
            foreach (var record in records)
            {
                sect.ДобавитьСтроку();

                sect["row"] = i++;
                sect["Mu"] = record.MuName;
                if (moOption)
                {
                    sect["Mo"] = record.MoName;
                }
                sect["Address"] = record.Address;
                sect["TypeOrganization"] = record.TypeOrganization;
                sect["OrganizationName"] = record.OrganizationName;
            }
        }
    }
}