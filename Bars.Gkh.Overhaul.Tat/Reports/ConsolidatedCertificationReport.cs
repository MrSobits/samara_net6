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
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Properties;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    class ConsolidatedCertificationReport : BasePrintForm
    {
        public ConsolidatedCertificationReport()
            : base(new ReportTemplateBinary(Resources.ConsolidatedCertificationReport))
        {
        }

        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }
        public override string Name
        {
            get { return "Cводный отчет по паспортизации домов"; }
        }

        public override string Desciption
        {
            get { return "Cводный отчет по паспортизации домов"; }
        }

        public override string GroupName
        {
            get { return "Жилые дома"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.ConsolidatedCertificationReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.ConsolidatedCertificationReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.To<long>()).ToArray()
                                  : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["reportDate"] = DateTime.Now.ToString("dd.MM.yyyy");

            var commonEstateObjectsQuery = this.Container.Resolve<IDomainService<CommonEstateObject>>().GetAll()
                .Where(x => x.IncludedInSubjectProgramm).Select(x => x.Id);
            var ceoCount = this.Container.Resolve<IDomainService<CommonEstateObject>>().GetAll().Count(x => x.IncludedInSubjectProgramm);

            var roMissingStructElemGroupDict = GetMissingCeo(commonEstateObjectsQuery);
            var roStructElemGroupDict = GetData(commonEstateObjectsQuery);

            var queryRo = this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .Where(x => x.TypeHouse == TypeHouse.ManyApartments)
                .Where(x => x.ConditionHouse == ConditionHouse.Dilapidated || x.ConditionHouse == ConditionHouse.Serviceable)
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id));

            var municipalityData = queryRo
                .Select(x => new
                    {
                        x.Municipality.Name,
                        muId = x.Municipality.Id,
                        x.Id
                    })
                .AsEnumerable()
                .Select(x =>
                    {
                        var currentRoCeoIdsList = roStructElemGroupDict.ContainsKey(x.Id) ? roStructElemGroupDict[x.Id] : new List<long>();
                        var currentRoMissingCeoCount = roMissingStructElemGroupDict.ContainsKey(x.Id) 
                            ? roMissingStructElemGroupDict[x.Id].Count(y => !currentRoCeoIdsList.Contains(y))
                            : 0;

                        var tempCeoCount = ceoCount - currentRoMissingCeoCount;
                        var percent = tempCeoCount != 0 ? currentRoCeoIdsList.Count * 100.0 / tempCeoCount : 0d;

                        return new { x.muId, x.Name, percent };
                    })
                .GroupBy(x => new { x.Name, x.muId })
                .Select(x => new {
                    Municipality = x.Key,
                    Count = x.Count(),
                    Percent0 = x.Count(y => y.percent == 0),
                    PercentLess50 = x.Count(y => y.percent > 0 && y.percent < 50),
                    Percent50To70 = x.Count(y => y.percent >= 50 && y.percent <= 70),
                    Percent70To100 = x.Count(y => y.percent > 70 && y.percent < 100),
                    Percent100 = x.Count(y => y.percent == 100)
                 })
                 .ToList();

            var dictMoPercent = this.GetMoPercent(commonEstateObjectsQuery, queryRo);

            var emergencyRoCountByMunicipality = this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                .Where(x => x.TypeHouse == TypeHouse.ManyApartments)
                .Where(x => x.ConditionHouse == ConditionHouse.Emergency)
                .GroupBy(x => x.Municipality.Id)
                .Select(x => new { x.Key, Count = x.Count() })
                .ToDictionary(x => x.Key, x => x.Count);

            var socialRoCountByMunicipality = this.Container.Resolve<IDomainService<RealityObject>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Municipality.Id))
                .Where(x => x.TypeHouse == TypeHouse.SocialBehavior)
                .GroupBy(x => x.Municipality.Id)
                .Select(x => new { x.Key, Count = x.Count()})
                .ToDictionary(x => x.Key, x => x.Count);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var num = 0;

            foreach (var data in municipalityData.OrderBy(x => x.Municipality.Name))
            {
                section.ДобавитьСтроку();
                section["Num"] = ++num;
                section["MuName"] = data.Municipality.Name;
                section["Count"] = data.Count;
                section["Emergency"] = emergencyRoCountByMunicipality.ContainsKey(data.Municipality.muId) ? emergencyRoCountByMunicipality[data.Municipality.muId]: 0;
                section["Social"] = socialRoCountByMunicipality.ContainsKey(data.Municipality.muId) ? socialRoCountByMunicipality[data.Municipality.muId]: 0;

                if (data.Percent0 > 0)
                {
                    section["Zero"] = data.Percent0;
                    section["ZeroPart"] = decimal.Divide(data.Percent0, data.Count).RoundDecimal(4);
                }

                if (data.PercentLess50 > 0)
                {
                    section["Less50"] = data.PercentLess50;
                    section["Less50Part"] = decimal.Divide(data.PercentLess50, data.Count).RoundDecimal(4);
                }

                if (data.Percent50To70 > 0)
                {
                    section["From50To70"] = data.Percent50To70;
                    section["From50To70Part"] = decimal.Divide(data.Percent50To70, data.Count).RoundDecimal(4);
                }

                if (data.Percent70To100 > 0)
                {
                    section["From70To100"] = data.Percent70To100;
                    section["From70To100Part"] = decimal.Divide(data.Percent70To100, data.Count).RoundDecimal(4);
                }

                if (data.Percent100 > 0)
                {
                    section["Is100"] = data.Percent100;
                    section["Is100Part"] = decimal.Divide(data.Percent100, data.Count).RoundDecimal(4);
                }

                if (dictMoPercent.ContainsKey(data.Municipality.muId))
                {
                    section["Percent"] = Math.Round(dictMoPercent[data.Municipality.muId], 2);
                }                
            }

            reportParams.SimpleReportParams["TotalEmergency"] = emergencyRoCountByMunicipality.Any() ? emergencyRoCountByMunicipality.Sum(x => x.Value) : 0;
            reportParams.SimpleReportParams["TotalSocial"] = socialRoCountByMunicipality.Any() ? socialRoCountByMunicipality.Sum(x => x.Value) : 0;

            var totalCount = municipalityData.Any() ? municipalityData.Sum(x => x.Count) : 0;
            reportParams.SimpleReportParams["TotalCount"] = totalCount;

            if (totalCount <= 0)
            {
                return;
            }

            var totalPercent0 = municipalityData.Sum(x => x.Percent0);
            if (totalPercent0 > 0)
            {
                reportParams.SimpleReportParams["TotalZero"] = totalPercent0;
                reportParams.SimpleReportParams["TotalZeroPart"] = decimal.Divide(totalPercent0, totalCount).RoundDecimal(4);
            }

            var totalLess50 = municipalityData.Sum(x => x.PercentLess50);
            if (totalLess50 > 0)
            {
                reportParams.SimpleReportParams["TotalLess50"] = totalLess50;
                reportParams.SimpleReportParams["TotalLess50Part"] = decimal.Divide(totalLess50, totalCount).RoundDecimal(4);
            }

            var totalFrom50To70 = municipalityData.Sum(x => x.Percent50To70);
            if (totalFrom50To70 > 0)
            {
                reportParams.SimpleReportParams["TotalFrom50To70"] = totalFrom50To70;
                reportParams.SimpleReportParams["TotalFrom50To70Part"] = decimal.Divide(totalFrom50To70, totalCount).RoundDecimal(4);    
            }

            var totalPercent70To100 = municipalityData.Sum(x => x.Percent70To100);
            if (totalPercent70To100 > 0)
            {
                reportParams.SimpleReportParams["TotalFrom70To100"] = totalPercent70To100;
                reportParams.SimpleReportParams["TotalFrom70To100Part"] = decimal.Divide(totalPercent70To100, totalCount).RoundDecimal(4);
            }

            var totalPercent100 = municipalityData.Sum(x => x.Percent100);
            if (totalPercent100 > 0)
            {
                reportParams.SimpleReportParams["TotalIs100"] = totalPercent100;
                reportParams.SimpleReportParams["TotalIs100Part"] = decimal.Divide(totalPercent100, totalCount).RoundDecimal(4);
            }

            reportParams.SimpleReportParams["AvgPercent"] = Math.Round(dictMoPercent.Values.Average(), 2);
        }

        // получение групп конструктивных элементов по домам
        public Dictionary<long, List<long>> GetData(IQueryable<long> commonEstateObjectsQuery)
        {
            return Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
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

        // получение отсутствующих объектов общего имущества по домам
        public Dictionary<long, List<long>> GetMissingCeo(IQueryable<long> commonEstateObjectsQuery)
        {
            return this.Container.Resolve<IDomainService<RealityObjectMissingCeo>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => commonEstateObjectsQuery.Contains(x.MissingCommonEstateObject.Id))
                .Select(x => new 
                {
                    roId = x.RealityObject.Id,
                    ceoId = x.MissingCommonEstateObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => y.ceoId).Distinct().ToList());
        }

        private Dictionary<long, decimal> GetMoPercent(IQueryable<long> commonEstateObjectsQuery, IQueryable<RealityObject> queryRo)
        {

            var dictRoStrEl = Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                .Where(x => commonEstateObjectsQuery.Contains(x.StructuralElement.Group.CommonEstateObject.Id))
                .Select(x => new
                    {
                        muId = x.RealityObject.Municipality.Id,
                        roId = x.RealityObject.Id,
                        x.Volume
                    })
                .AsEnumerable()
                .GroupBy(x => x.roId)
                .ToDictionary(
                    x => x.Key, 
                    v => v.Count() == v.Count(y => y.Volume != 0));

            return queryRo.Select(x => new { muId = x.Municipality.Id, roId = x.Id })
                       .ToList()
                       .GroupBy(x => x.muId)
                       .ToDictionary(
                           x => x.Key,
                           v =>
                               {
                                   var listRoId = v.Select(y => y.roId).Distinct().ToList();
                                   var countRo = v.Count();

                                   var countRoRes = listRoId.Where(dictRoStrEl.ContainsKey).Count(id => dictRoStrEl[id]);

                                   return (decimal.Divide(countRoRes, countRo)*100.0M);
                               });
        }
    }
}