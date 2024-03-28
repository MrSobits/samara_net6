namespace Bars.Gkh.Overhaul.Regions.Saratov.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;

    using Bars.Gkh.Overhaul.Regions.Saratov.Properties;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;

    public class RealtyObjectCertificationControl : BasePrintForm
    {
        public RealtyObjectCertificationControl() : 
            base(new ReportTemplateBinary(Resources.RealtyObjectCertificationControl))
        {
        
        }

        private long[] municipalityIds;

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Контроль паспортизации домов (Саратов)"; }
        }

        public override string Desciption
        {
            get { return "Контроль паспортизации домов (Саратов)"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.RealtyObjectCertificationControl";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.Saratov.GkhOverhaul.RealtyObjectCertificationControl";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var commonEstateObjectsQuery = CreateVerticalColums(reportParams);
            var ceoIds = commonEstateObjectsQuery.ToList();

            var realtyObjectIndicatorsCount = 6;

            var realtyObjByMuDict = Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .Where(x => x.TypeHouse == TypeHouse.ManyApartments)
                .Where(x => x.ConditionHouse == ConditionHouse.Dilapidated || x.ConditionHouse == ConditionHouse.Serviceable)
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                 .Select(x => new
                 {
                     x.Municipality.Name,
                     roId = x.Id,
                     x.Address,
                     x.BuildYear,
                     x.PrivatizationDateFirstApartment,
                     x.NecessaryConductCr,
                     x.AreaMkd,
                     x.AreaLiving,
                     x.NumberLiving
                 })
                .AsEnumerable()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.ToList());

            var realityObjectStructuralElement = GetData(commonEstateObjectsQuery);

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");
            var num = 1;
            var sumPercent = 0M;
            var sumPercentOccup = 0M;

            foreach (var realtyObjectsByMu in realtyObjByMuDict.OrderBy(x => x.Key))
            {
                sectionMu.ДобавитьСтроку();
                sectionMu["MuName"] = realtyObjectsByMu.Key;
                var sumPercentByMu = 0M;
                var sumPercentOccupByMu = 0M;

                foreach (var realtyObjectInfo in realtyObjectsByMu.Value.OrderBy(x => x.Address))
                {
                    sectionRo.ДобавитьСтроку();
                    sectionRo["Num"] = num++;
                    sectionRo["MO"] = realtyObjectsByMu.Key;
                    sectionRo["Address"] = realtyObjectInfo.Address;

                    sectionRo["BuildYear"] = realtyObjectInfo.BuildYear;
                    sectionRo["PrivatizationDateFirstApartment"] = realtyObjectInfo.PrivatizationDateFirstApartment.HasValue 
                        ? realtyObjectInfo.PrivatizationDateFirstApartment.Value.ToString("dd.MM.yyyy") : string.Empty;
                    sectionRo["NecessaryConductCr"] = realtyObjectInfo.NecessaryConductCr.GetEnumMeta().Display;
                    sectionRo["AreaMkd"] = realtyObjectInfo.AreaMkd;
                    sectionRo["AreaLiving"] = realtyObjectInfo.AreaLiving;
                    sectionRo["NumberLiving"] = realtyObjectInfo.NumberLiving;

                    var currentRoIndicatorsCount = (realtyObjectInfo.BuildYear.HasValue && realtyObjectInfo.BuildYear.Value > 0 ? 1 : 0)
                        + (realtyObjectInfo.PrivatizationDateFirstApartment.HasValue && realtyObjectInfo.PrivatizationDateFirstApartment.Value != DateTime.MinValue ? 1 : 0)
                        + (realtyObjectInfo.NecessaryConductCr != YesNoNotSet.NotSet ? 1 : 0)
                        + (realtyObjectInfo.AreaMkd.HasValue && realtyObjectInfo.AreaMkd.Value > 0 ? 1 : 0)
                        + (realtyObjectInfo.AreaLiving.HasValue && realtyObjectInfo.AreaLiving.Value > 0 ? 1 : 0)
                        + (realtyObjectInfo.NumberLiving.HasValue && realtyObjectInfo.NumberLiving.Value > 0 ? 1 : 0);

                    var currentRoCeoIdsList = realityObjectStructuralElement.ContainsKey(realtyObjectInfo.roId) 
                        ? realityObjectStructuralElement[realtyObjectInfo.roId] 
                        : new List<long>();

                    var currentPercentOccup = ceoIds.Count != 0 ? decimal.Divide(currentRoCeoIdsList.Count, ceoIds.Count) : 0;
                    var currentRoPercent = decimal.Divide(currentRoCeoIdsList.Count + currentRoIndicatorsCount, ceoIds.Count + realtyObjectIndicatorsCount);
                    sumPercentByMu += currentRoPercent;
                    sumPercentOccupByMu += currentPercentOccup;
                    sectionRo["Percent"] = currentRoPercent;
                    sectionRo["PercentOccup"] = currentPercentOccup;

                    foreach (var ceoId in ceoIds)
                    {
                        var haveConElem = currentRoCeoIdsList.Contains(ceoId) ? 1 : 0;
                        sectionRo[string.Format("haveElement{0}", ceoId)] = haveConElem;
                    }
                }

                var currentMuPercent = sumPercentByMu / realtyObjectsByMu.Value.Count;
                var currentMuPercentOccup = sumPercentOccupByMu / realtyObjectsByMu.Value.Count;
                sumPercent += sumPercentByMu;
                sumPercentOccup += sumPercentOccupByMu;
                sectionMu["AverageMunOccup"] = currentMuPercentOccup;
                sectionMu["AverageMun"] = currentMuPercent;
            }

            if (realtyObjByMuDict.Any())
            {
                var roCount = realtyObjByMuDict.Sum(x => x.Value.Count);
                reportParams.SimpleReportParams["AverageAllMunOccup"] = sumPercentOccup / roCount;
                reportParams.SimpleReportParams["AverageAllMun"] = sumPercent / roCount;
            }
            
        }

        // получение групп конструктивных элементов по домам
        public Dictionary<long, List<long>> GetData(IQueryable<long> commonEstateObjectsQuery)
        {
            return this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => commonEstateObjectsQuery.Contains(x.StructuralElement.Group.CommonEstateObject.Id))
                .Select(x => new
                {
                    roId = x.RealityObject.Id,
                    ceoId = x.StructuralElement.Group.CommonEstateObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                x => x.Key,
                x => x.Select(y => y.ceoId).Distinct().ToList());
        }

        // заполнение вертикальной секции
        public IQueryable<long> CreateVerticalColums(ReportParams reportParams)
        {
            var commonEstateObjectsQuery =
                this.Container.Resolve<IDomainService<CommonEstateObject>>()
                    .GetAll()
                    .Where(x => x.IncludedInSubjectProgramm);

            var commonEstateObjects =
                commonEstateObjectsQuery
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => x.Name)
                .ToList();
            
            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("GroupCost");

            foreach (var commonEstateObject in commonEstateObjects)
            {
                verticalSection.ДобавитьСтроку();
                verticalSection["GroupName"] = commonEstateObject.Name;
                verticalSection["haveElement"] = string.Format("$haveElement{0}$", commonEstateObject.Id);
            }

            return commonEstateObjectsQuery.Select(x => x.Id);
        }
    }
}