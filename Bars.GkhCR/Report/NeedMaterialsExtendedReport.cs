namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class NeedMaterialsExtendedReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime ReportDate = DateTime.MinValue;
        private int programCrId;
        private int municipalityId;
        private List<long> typeWorkIds = new List<long>();
        private int condition;
        private decimal sum;


        public NeedMaterialsExtendedReport()
            : base(new ReportTemplateBinary(Properties.Resources.NeedMaterialsExtended))
        {
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.NeedMaterialsExtendedReport";
            }
        }

        public override string Name
        {
            get { return "Потребность в материалах (расширенная)"; }
        }

        public override string Desciption
        {
            get { return "Потребность в материалах (расширенная)"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.NeedMaterialsExtended"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCr"].ToInt();
            this.municipalityId = baseParams.Params["municipalityIds"].ToInt();

            this.condition = baseParams.Params["condition"].ToInt();
            this.sum = baseParams.Params["sum"].ToDecimal();
            this.ReportDate = baseParams.Params["reportDate"].ToDateTime();
            
            var typeWorkIdList = baseParams.Params.GetAs("typeWorkCr", string.Empty);
            this.typeWorkIds = !string.IsNullOrEmpty(typeWorkIdList) ? typeWorkIdList.Split(',').Select(id => id.ToLong()).ToList() : new List<long>();

        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var sectionRegion = reportParams.ComplexReportParams.ДобавитьСекцию("sectionregion");
            var sectionRealtyObject = sectionRegion.ДобавитьСекцию("sectionrealtyobject");
            var sectionMaterial = sectionRealtyObject.ДобавитьСекцию("sectionmaterial");

            var municipalityDict = this.Container.Resolve<IDomainService<Municipality>>().GetAll()
                .Where(x => this.municipalityId == x.Id)
                .ToDictionary(x => x.Id, x => x.Name);

            // дома по программе КР и по выбранным муницип.р-нам
            var realtyObjectsSubQuery = this.GetRealtyObjectSubset();
            
            // УО домов
            var manOrgByRoDict = this.GetManagementOrganisations(realtyObjectsSubQuery);

            // Подзапрос: по программе КР, мун.р-нам
            var typeWorkCrQuery = this.GetTypeWorkQuery(realtyObjectsSubQuery);
            var typeWorkCrIdQuery = typeWorkCrQuery.Select(x => x.Id);

            // Словарик: по Мун.р-н и дому - инфа о работе
            var worksByRoByMu = this.GetData(typeWorkCrQuery);

            // ресурсы по работам
            var objectCrWorksData = this.GetTypeWorkCrData(typeWorkCrIdQuery);

            var totals = new decimal[6];

            var typeWorksIdList = worksByRoByMu.SelectMany(x => x.Value.SelectMany(y => y.Value.Select(z => z.typeWorkCrId))).Distinct().ToList();
            var countRows = typeWorksIdList.Sum(x => objectCrWorksData.ContainsKey(x) ? objectCrWorksData[x].Count : 0);
            if (countRows > 65000)
            {
                throw new Exception("Получено слишком много данных, конкретизируйте отчет и повторите попытку");
            }

            int number = 0;
            foreach (var worksByRo in worksByRoByMu)
            {
                sectionRegion.ДобавитьСтроку();
                sectionRegion["MunicipalityName"] = municipalityDict[worksByRo.Key];
                var totalsMunicipality = new decimal[6];
                foreach (var works in worksByRo.Value)
                {
                    if (!works.Value.Any(x => objectCrWorksData.ContainsKey(x.typeWorkCrId) && objectCrWorksData[x.typeWorkCrId].Count > 0))
                    {
                        continue;
                    }

                    sectionRealtyObject.ДобавитьСтроку();
                    sectionRealtyObject["Address"] = works.Value.Select(x => x.roName).FirstOrDefault();
                    var totalsRealtyObject = new decimal[6];
                    foreach (var work in works.Value)
                    {
                        var objectCrWorkData = objectCrWorksData[work.typeWorkCrId];

                        foreach (var reportDataRow in objectCrWorkData.OrderBy(x => x.Key))
                        {
                            sectionMaterial.ДобавитьСтроку();
                            
                            var keys = reportDataRow.Key.Split("#####");
                            sectionMaterial["Number"] = ++number;
                            sectionMaterial["MunicipalityName"] = municipalityDict[worksByRo.Key];
                            sectionMaterial["Address"] = work.roName;
                            sectionMaterial["ManOrgName"] = manOrgByRoDict.ContainsKey(work.roId) ? manOrgByRoDict[work.roId] : string.Empty;
                            sectionMaterial["Work"] = work.workName;
                            sectionMaterial["FinSource"] = work.FinanceSourceName;
                            sectionMaterial["Material"] = keys.Any() ? keys.First() : string.Empty;
                            sectionMaterial["UnitMeasure"] = keys.Count() > 1 ? keys[1] : string.Empty;

                            sectionMaterial["CountOnAct"] = reportDataRow.Value.ActTotalCount;
                            totalsRealtyObject[0] += reportDataRow.Value.ActTotalCount ?? 0M;

                            sectionMaterial["PriceOnAct"] = reportDataRow.Value.ActOnUnitCost;
                            totalsRealtyObject[1] += reportDataRow.Value.ActOnUnitCost ?? 0M;

                            sectionMaterial["SumOnAct"] = reportDataRow.Value.ActTotalCost;
                            totalsRealtyObject[2] += reportDataRow.Value.ActTotalCost ?? 0M;

                            sectionMaterial["CountOnEstimate"] = reportDataRow.Value.RegistryTotalCount;
                            totalsRealtyObject[3] += reportDataRow.Value.RegistryTotalCount ?? 0M;

                            sectionMaterial["PriceOnEstimate"] = reportDataRow.Value.RegistryOnUnitCost;
                            totalsRealtyObject[4] += reportDataRow.Value.RegistryOnUnitCost ?? 0M;

                            sectionMaterial["SumOnEstimate"] = reportDataRow.Value.RegistryTotalCost;
                            totalsRealtyObject[5] += reportDataRow.Value.RegistryTotalCost ?? 0M;
                        }
                    }

                    for (int i = 0; i <= 5; i++)
                    {
                        totalsMunicipality[i] += totalsRealtyObject[i];
                        sectionRealtyObject[string.Format("TotalRo{0}", i)] = totalsRealtyObject[i];
                    }
                }

                for (int i = 0; i <= 5; i++)
                {
                    totals[i] += totalsMunicipality[i];
                    sectionRegion[string.Format("TotalMu{0}", i)] = totalsMunicipality[i];
                }
            }

            for (int i = 0; i <= 5; i++)
            {
                reportParams.SimpleReportParams[string.Format("Total{0}", i)] = totals[i];
            }
        }

        private IQueryable<TypeWorkCr> GetTypeWorkQuery(IQueryable<long> realtyObjectsSubQuery)
        {
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();
            var typeWorkCrIds = serviceTypeWorkCr.GetAll()
                                 .Where(x => realtyObjectsSubQuery.Contains(x.ObjectCr.RealityObject.Id))
                                 .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                                 .WhereIf(this.typeWorkIds.Count > 0, x => typeWorkIds.Contains(x.Work.Id));
            return typeWorkCrIds;
        }

        private IQueryable<long> GetRealtyObjectSubset()
        {
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();

            var realtyObjectSubQuery = serviceTypeWorkCr.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                .Where(x => municipalityId == x.ObjectCr.RealityObject.Municipality.Id)
                .Select(x => x.ObjectCr.RealityObject.Id);

            return realtyObjectSubQuery;
        }

        private Dictionary<long, string> GetManagementOrganisations(IQueryable<long> realObjIdsQuery)
        {
            var serviceManOrgContractRealityObject = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            var realityObjectManOrg = serviceManOrgContractRealityObject.GetAll()
                    .Where(x => realObjIdsQuery.Contains(x.RealityObject.Id))
                    .Where(x => x.ManOrgContract.StartDate <= this.ReportDate)
                    .Where(x => x.ManOrgContract.EndDate >= this.ReportDate || x.ManOrgContract.EndDate == null)
                    .Select(x => new
                    {
                        roId = x.RealityObject.Id,
                        ManOrgName = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                        Date = x.ManOrgContract.StartDate,
                        typeContract = x.ManOrgContract.TypeContractManOrgRealObj
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.roId)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        {
                            var manOrgByRoList = x.Select(y => new { y.ManOrgName, y.typeContract, y.Date }).ToList();
                            if (manOrgByRoList.Count == 1)
                            {
                                return manOrgByRoList.Select(y => y.ManOrgName).FirstOrDefault();
                            }

                            if (manOrgByRoList.Count > 1)
                            {
                                if (manOrgByRoList.Any(y => y.typeContract == TypeContractManOrg.JskTsj)
                                    && manOrgByRoList.Any(y => y.typeContract == TypeContractManOrg.ManagingOrgJskTsj))
                                {
                                    return manOrgByRoList
                                            .Where(y => y.typeContract == TypeContractManOrg.ManagingOrgJskTsj)
                                            .OrderByDescending(y => y.Date)
                                            .Select(y => y.ManOrgName)
                                            .FirstOrDefault();
                                }

                                return manOrgByRoList
                                    .OrderByDescending(y => y.Date)
                                    .Select(y => y.ManOrgName)
                                    .FirstOrDefault();
                            }

                            return string.Empty;
                        });

            return realityObjectManOrg;
        }

        // Словарик по муниц.р-ну и дому - работы 
        private Dictionary<long, Dictionary<long, List<TypeWkCrProxy>>> GetData(IQueryable<TypeWorkCr> typeWorkCrQuery)
        {
            var objectCrWorks = typeWorkCrQuery
                                 .OrderBy(x => x.ObjectCr.RealityObject.Municipality.Name)
                                 .ThenBy(x => x.ObjectCr.RealityObject.Address)
                                 .Select(
                                     x =>
                                     new TypeWkCrProxy
                                     {
                                         muId = x.ObjectCr.RealityObject.Municipality.Id,
                                         roId = x.ObjectCr.RealityObject.Id,
                                         roName = x.ObjectCr.RealityObject.Address,
                                         FinanceSourceName = x.FinanceSource.Name,
                                         typeWorkCrId = x.Id,
                                         workName = x.Work.Name
                                     })
                                  .AsEnumerable()
                                  .GroupBy(x => x.muId)
                                  .ToDictionary(x => x.Key,
                                                x => x.GroupBy(y => y.roId)
                                                      .ToDictionary(y => y.Key, y => y.ToList()));

            return objectCrWorks;
        }

        private Dictionary<long, Dictionary<string, ReportDataRow>> GetTypeWorkCrData(IQueryable<long> typeWorkCrIds)
        {
            var serviceResourceStatement = Container.Resolve<IDomainService<ResourceStatement>>();
            var servicePerformedWorkActRecord = Container.Resolve<IDomainService<PerformedWorkActRecord>>();

            var resourceStatements = serviceResourceStatement.GetAll()
                .Where(x => typeWorkCrIds.Contains(x.EstimateCalculation.TypeWorkCr.Id))
                .WhereIf(this.condition == 1 && this.sum > 0, x => x.TotalCost >= this.sum)
                .WhereIf(this.condition == 2 && this.sum > 0, x => x.TotalCost <= this.sum)
                .WhereIf(this.condition == 3 && this.sum > 0, x => x.TotalCost == this.sum)
                .GroupBy(x => new { x.EstimateCalculation.TypeWorkCr.Id, x.Name, x.UnitMeasure })
                .Select(x => new BaseEstimateProxy
                {
                    TypeWorkCrId = x.Key.Id,
                    Name = x.Key.Name,
                    MeasureUnit = x.Key.UnitMeasure,
                    OnUnitCost = x.Average(y => y.OnUnitCost),
                    TotalCost = x.Sum(y => y.TotalCost),
                    TotalCount = x.Sum(y => y.TotalCount)
                });

            var performedWorkActRecords = servicePerformedWorkActRecord.GetAll()
                .Where(x => typeWorkCrIds.Contains(x.PerformedWorkAct.TypeWorkCr.Id))
                .Where(x => x.PerformedWorkAct.DateFrom == null || x.PerformedWorkAct.DateFrom <= this.ReportDate)
                .GroupBy(x => new { x.PerformedWorkAct.TypeWorkCr.Id, x.Name, x.UnitMeasure })
                .Select(x => new BaseEstimateProxy
                {
                    TypeWorkCrId = x.Key.Id,
                    Name = x.Key.Name,
                    MeasureUnit = x.Key.UnitMeasure,
                    OnUnitCost = x.Average(y => y.OnUnitCost),
                    TotalCost = x.Sum(y => y.TotalCost),
                    TotalCount = x.Sum(y => y.TotalCount),
                });


            var resourceStatementsByTypeWorkDict = resourceStatements
                .AsEnumerable()
                .GroupBy(x => x.TypeWorkCrId)
                .ToDictionary(x => x.Key,
                              x => x.GroupBy(y => y.Name + "#####" + y.MeasureUnit)
                                    .ToDictionary(y => y.Key,
                                        y =>
                                        {
                                            var totalCost = y.Sum(z => z.TotalCost.HasValue ? z.TotalCost.Value : 0);
                                            var totalCount = y.Sum(z => z.TotalCount.HasValue ? z.TotalCount.Value : 0);
                                            var onUnitCost = y.Average(z => z.OnUnitCost);

                                            switch (this.condition)
                                            {
                                                case 2:
                                                    if (totalCost > this.sum)
                                                    {
                                                        return null;
                                                    }
                                                    break;
                                                case 3:
                                                    if (totalCost != this.sum)
                                                    {
                                                        return null;
                                                    }
                                                    break;
                                            }

                                            return new { totalCost, totalCount, onUnitCost };
                                        }));


            foreach (var resourceStatements1 in resourceStatementsByTypeWorkDict)
            {
                var nullValueKeys = resourceStatements1.Value.Where(x => x.Value == null).Select(x => x.Key).ToList();

                nullValueKeys.ForEach(x => resourceStatements1.Value.Remove(x));
            }

            var performedWorkActRecordsByTypeWorkDict = performedWorkActRecords
                .AsEnumerable()
                .GroupBy(x => x.TypeWorkCrId)
                .ToDictionary(x => x.Key,
                              x => x.GroupBy(y => y.Name + "#####" + y.MeasureUnit)
                                    .ToDictionary(y => y.Key,
                                        y =>
                                        {
                                            var totalCost = y.Sum(z => z.TotalCost.HasValue ? z.TotalCost.Value : 0);
                                            var totalCount = y.Sum(z => z.TotalCount.HasValue ? z.TotalCount.Value : 0);
                                            var onUnitCost = y.Average(z => z.OnUnitCost);
                                            return new { totalCost, totalCount, onUnitCost };
                                        }));

            var reportRows = typeWorkCrIds.ToDictionary(
                x => x,
                x =>
                {
                    var res = new Dictionary<string, ReportDataRow>();

                    if (resourceStatementsByTypeWorkDict.ContainsKey(x))
                    {
                        res = resourceStatementsByTypeWorkDict[x]
                            .ToDictionary(
                                y => y.Key,
                                y =>
                                new ReportDataRow
                                {
                                    RegistryOnUnitCost = y.Value.onUnitCost,
                                    RegistryTotalCost = y.Value.totalCost,
                                    RegistryTotalCount = y.Value.totalCount
                                });
                    }

                    if (performedWorkActRecordsByTypeWorkDict.ContainsKey(x))
                    {
                        var performedActRecords = performedWorkActRecordsByTypeWorkDict[x];
                        foreach (var performedActRecord in performedActRecords)
                        {
                            if (res.ContainsKey(performedActRecord.Key))
                            {
                                res[performedActRecord.Key].ActOnUnitCost = performedActRecord.Value.onUnitCost;
                                res[performedActRecord.Key].ActTotalCost = performedActRecord.Value.totalCost;
                                res[performedActRecord.Key].ActTotalCount = performedActRecord.Value.totalCount;
                            }
                            else
                            {
                                if (this.condition == 1)
                                {
                                    if (sum > 0)
                                    {
                                        continue;
                                    }
                                }
                                else if (this.condition == 2)
                                {
                                    if (sum < 0)
                                    {
                                        continue;
                                    }
                                }
                                else if (this.condition == 3)
                                {
                                    if (sum != 0)
                                    {
                                        continue;
                                    }
                                }

                                res[performedActRecord.Key] = new ReportDataRow
                                {
                                    ActOnUnitCost = performedActRecord.Value.onUnitCost,
                                    ActTotalCost = performedActRecord.Value.totalCost,
                                    ActTotalCount = performedActRecord.Value.totalCount
                                };
                            }
                        }
                    }

                    return res;
                });

            return reportRows;
        }

        public override string ReportGenerator
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }

    class BaseEstimateProxy
    {
        public long TypeWorkCrId;

        public string Name;

        public string MeasureUnit;

        public decimal? OnUnitCost;

        public decimal? TotalCost;

        public decimal? TotalCount;
    }

    class ReportDataRow
    {
        public decimal? ActOnUnitCost;

        public decimal? ActTotalCost;

        public decimal? ActTotalCount;

        public decimal? RegistryOnUnitCost;

        public decimal? RegistryTotalCost;

        public decimal? RegistryTotalCount;
    }

    internal class Resource
    {
        public string Name;

        public string MeasureUnit;
    }

    class TypeWkCrProxy
    {
        public long muId;

        public long roId;

        public string roName;

        public string FinanceSourceName;

        public long typeWorkCrId;

        public string workName;
    }
}