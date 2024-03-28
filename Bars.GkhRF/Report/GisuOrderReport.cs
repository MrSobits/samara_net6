namespace Bars.GkhRf.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;
    using Bars.GkhRf.Properties;

    using Castle.Windsor;

    class GisuOrderReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private List<long> municipalities;

        private DateTime reportDate = DateTime.Now;

        public GisuOrderReport()
            : base(new ReportTemplateBinary(Resources.GisuOrdersReport))
        {
        }

        public override string Desciption
        {
            get
            {
                return "Информация о заключенных договорах между УК и ГКУ Главинвестстрой РТ";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Региональный фонд";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.GisuOrderReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.GisuOrdersReport";
            }
        }

        public override string Name
        {
            get
            {
                return "Информация о заключенных договорах между УК и ГКУ Главинвестстрой РТ";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalitiesParam = baseParams.Params["municipalityIds"].ToString();
            municipalities = string.IsNullOrEmpty(municipalitiesParam) ? new List<long>() : municipalitiesParam.Split(',').Select(x => x.ToLong()).ToList();
            reportDate = baseParams.Params["dateReport"].ToDateTime();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();
            var serviceContractRfObject = Container.Resolve<IDomainService<ContractRfObject>>();
            var serviceContractRf = Container.Resolve<IDomainService<ContractRf>>();
            var serviceRealityObject = Container.Resolve<IDomainService<RealityObject>>();

            var muNameById = serviceMunicipality
                .GetAll()
                .WhereIf(this.municipalities.Count > 0, x => this.municipalities.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => x.Name)
                .ToDictionary(x => x.Id, x => x.Name);

            var queryRegFondManOrgByMuId = serviceContractRf.GetAll()
                .WhereIf(this.municipalities.Count > 0, x => municipalities.Contains(x.ManagingOrganization.Contragent.Municipality.Id))
                .Where(x => x.DateBegin == null || x.DateBegin <= reportDate)
                .Where(x => x.DateEnd == null || x.DateEnd >= reportDate);

            var queryRegFondManOrgContractIdByMuId = queryRegFondManOrgByMuId.Select(y => y.Id);

            var regFondRoCountByMuId = serviceContractRfObject.GetAll()
                .Where(x => queryRegFondManOrgContractIdByMuId.Contains(x.ContractRf.Id))
                .Where(x => x.IncludeDate == null || x.IncludeDate <= reportDate.Date)
                .Where(x => x.TypeCondition == TypeCondition.Include)
                .GroupBy(x => x.ContractRf.ManagingOrganization.Contragent.Municipality.Id)
                .Select(x => new
                                 {
                                     Key = (long?)x.Key ?? -1, 
                                     CountRo = x.Count(),
                                     TotalAreaMkd = x.Sum(y => y.RealityObject.AreaMkd),
                                     TotalAreaLivingOwned = x.Sum(y => y.RealityObject.AreaLivingOwned),
                                     TotalAreaLiving = x.Sum(y => y.RealityObject.AreaLiving)
                                 })
                .ToDictionary(x => x.Key, x => new {x.CountRo, x.TotalAreaLiving, x.TotalAreaLivingOwned, x.TotalAreaMkd});

            var regFondUniqueOrgCountByMuId = queryRegFondManOrgByMuId
                .GroupBy(x => x.ManagingOrganization.Contragent.Municipality.Id)
                .Select(x => new
                {
                    Key = (long?)x.Key ?? -1,
                    UniqueMO = x.Select(y => y.ManagingOrganization.Id).Distinct().Count()
                })
                .ToDictionary(x => x.Key, x => x.UniqueMO);

            var roDataByMuId = serviceRealityObject.GetAll()
                .WhereIf(this.municipalities.Count > 0, x => this.municipalities.Contains(x.Municipality.Id))
                .Where(x => x.ConditionHouse != ConditionHouse.Razed)
                .GroupBy(x => x.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    CountRo = x.Count()                    
                })
                .ToDictionary(x => x.Key, x => x.CountRo);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var num = 0;
            foreach (var mu in muNameById)
            {
                section.ДобавитьСтроку();
                var muId = mu.Key;

                section["number"] = ++num;
                section["MO"] = muNameById[muId];

                if (roDataByMuId.ContainsKey(muId))
                {
                    section["CountMkdByMoInfo"] = roDataByMuId[muId];                    
                }
                else
                {
                    section["CountMkdByMoInfo"] = string.Empty;                    
                }

                if (regFondRoCountByMuId.ContainsKey(muId))
                {
                    section["CountMkdContracted"] = regFondRoCountByMuId[muId].CountRo;
                    section["AreaTotal"] = regFondRoCountByMuId[muId].TotalAreaMkd;
                    section["TotalAreaLivingOwned"] = regFondRoCountByMuId[muId].TotalAreaLivingOwned;
                    section["TotalAreaLiving"] = regFondRoCountByMuId[muId].TotalAreaLiving;
                }
                else
                {
                    section["CountMkdContracted"] = string.Empty;
                    section["AreaTotal"] = string.Empty;
                    section["TotalAreaLivingOwned"] = string.Empty;
                    section["TotalAreaLiving"] = string.Empty;
                }

                if (regFondUniqueOrgCountByMuId.ContainsKey(muId))
                {
                    section["CountControlOrganization"] = regFondUniqueOrgCountByMuId[muId];
                }
                else
                {
                    section["CountControlOrganization"] = string.Empty;                    
                }
            }

            reportParams.SimpleReportParams["ReportDate"] = reportDate.Date;
            reportParams.SimpleReportParams["CountMkdByMoInfo"] = roDataByMuId.Values.Sum();
            var valuesRegFondRoCountByMuId = regFondRoCountByMuId.Values;
            reportParams.SimpleReportParams["CountMkdContracted"] = valuesRegFondRoCountByMuId.Sum(x => x.CountRo);
            reportParams.SimpleReportParams["CountControlOrganization"] = regFondUniqueOrgCountByMuId.Values.Sum();
            reportParams.SimpleReportParams["AreaTotal"] = valuesRegFondRoCountByMuId.Sum(x => x.TotalAreaMkd).ToDecimal();
            reportParams.SimpleReportParams["TotalAreaLiving"] = valuesRegFondRoCountByMuId.Sum(x => x.TotalAreaLiving).ToDecimal();
            reportParams.SimpleReportParams["TotalAreaLivingOwned"] = valuesRegFondRoCountByMuId.Sum(x => x.TotalAreaLivingOwned).ToDecimal();
        }
    }
}
