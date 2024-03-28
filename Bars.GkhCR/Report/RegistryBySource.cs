namespace Bars.GkhCr.Report
{
    using System;
    using System.Linq;
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    public class RegistryBySource : BasePrintForm
    {
        private long programCrId;

        private long[] municipalityIds;
        private DateTime reportDate;

        public RegistryBySource()
            : base(new ReportTemplateBinary(Properties.Resources.RegistryBySource))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string Desciption
        {
            get { return "Отчет Реестр по источникам"; }
        }

        public override string GroupName
        {
            get { return "Формы программы"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.RegistryBySource"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.CR.RegistryBySource"; }
        }

        public override string Name
        {
            get { return "Отчет Реестр по источникам"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();
            var mus = baseParams.Params["municipalityIds"].ToString().Split(',');
            reportDate = baseParams.Params["reportDate"].ToDateTime();
            this.municipalityIds = mus.Length != 1 || mus[0] != string.Empty
                                       ? mus.Select(x => x.ToLong()).ToArray()
                                       : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var municipalities = Container.Resolve<IDomainService<ObjectCr>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ProgramCr.Id == programCrId)
                .Select(x =>
                    new
                    {
                        x.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        x.RealityObject.Address,
                        x.RealityObject.DateCommissioning,
                        x.RealityObject.DateLastOverhaul,
                        WallMaterial = x.RealityObject.WallMaterial.Name,
                        x.RealityObject.Floors,
                        x.RealityObject.NumberEntrances,
                        x.RealityObject.AreaMkd,
                        x.RealityObject.AreaLivingNotLivingMkd,
                        x.RealityObject.AreaLivingOwned,
                        x.RealityObject.NumberLiving
                    })
                       .AsEnumerable()
                       .GroupBy(x => x.Municipality)
                       .ToDictionary(
                       x => x.Key,
                       x => new
                       {
                           objectsCr = x.ToList().OrderBy(y => y.Address),
                           TotalMoAreaMkd = x.Where(y => y.AreaMkd.HasValue).Sum(y => y.AreaMkd.Value),
                           TotalMoAreaLivingNotLivingMk = x.Where(y => y.AreaLivingNotLivingMkd.HasValue).Sum(y => y.AreaLivingNotLivingMkd.Value),
                           TotalMoAreaLivingOwned = x.Where(y => y.AreaLivingOwned.HasValue).Sum(y => y.AreaLivingOwned.Value),
                           TotalMoNumberLiving = x.Where(y => y.NumberLiving.HasValue).Sum(y => y.NumberLiving.Value)
                       });

            var works185Fz = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.FinanceSource.Code == "1")
                .Select(x => new { Municipality = x.ObjectCr.RealityObject.Municipality.Name, ObjectCrId = x.ObjectCr.Id, x.Work.Name, x.Sum, DateEndWork = x.DateEndWork.HasValue ? x.DateEndWork.Value : DateTime.MinValue })
                .AsEnumerable()
                .GroupBy(x => x.Municipality)
                .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        Cost = x.Where(y => y.Sum.HasValue).Sum(y => y.Sum.Value.RoundDecimal(2)),
                        objectsCr = x.Select(y => new { y.ObjectCrId, y.Sum, y.Name, y.DateEndWork }).ToArray()
                        .GroupBy(y => y.ObjectCrId)
                        .ToDictionary(
                        c => c.Key,
                        c => new
                        {
                            Cost = c.Where(z => z.Sum.HasValue).Sum(z => z.Sum.Value.RoundDecimal(2)),
                            Names = c.Select(z => z.Name).Distinct().Aggregate((curr, next) => string.Format("{0}, {1}", curr, next)),
                            DateEndWork = c.Max(z => z.DateEndWork)
                        })
                    });

            var resource185Fz = Container.Resolve<IDomainService<FinanceSourceResource>>().GetAll()
                  .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                  .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && x.FinanceSource.Code == "1")
                  .Select(x => new
                  {
                      Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                      ObjectCrId = x.ObjectCr.Id,
                      x.FundResource,
                      x.BudgetSubject,
                      x.BudgetMu,
                      x.OwnerResource
                  })
                  .AsEnumerable()
                  .GroupBy(x => x.Municipality)
                  .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        FundResource = x.Where(y => y.FundResource.HasValue).Sum(y => y.FundResource.Value),
                        BudgetSubject = x.Where(y => y.BudgetSubject.HasValue).Sum(y => y.BudgetSubject.Value),
                        BudgetMu = x.Where(y => y.BudgetMu.HasValue).Sum(y => y.BudgetMu.Value),
                        OwnerResource = x.Where(y => y.OwnerResource.HasValue).Sum(y => y.OwnerResource.Value),
                        objectsCr = x.GroupBy(y => y.ObjectCrId).ToDictionary(
                        y => y.Key,
                        y => new
                        {
                            FundResource = y.Where(z => z.FundResource.HasValue).Sum(z => z.FundResource.Value),
                            BudgetSubject = y.Where(z => z.BudgetSubject.HasValue).Sum(z => z.BudgetSubject.Value),
                            BudgetMu = y.Where(z => z.BudgetMu.HasValue).Sum(z => z.BudgetMu.Value),
                            OwnerResource = y.Where(z => z.OwnerResource.HasValue).Sum(z => z.OwnerResource.Value)
                        })
                    });

            var works = Container.Resolve<IDomainService<TypeWorkCr>>().GetAll()
               .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
               .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && (x.FinanceSource.Code == "3" || x.FinanceSource.Code == "2" || x.FinanceSource.Code == "4"))
               .Select(
                   x =>
                   new
                   {
                       Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                       ObjectCrId = x.ObjectCr.Id,
                       x.Work.Name,
                       x.Sum,
                       DateEndWork = x.DateEndWork.HasValue ? x.DateEndWork.Value : DateTime.MinValue
                   })
                .AsEnumerable()
                .GroupBy(x => x.Municipality)
                .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        Cost = x.Where(y => y.Sum.HasValue).Sum(y => y.Sum.Value),
                        objectsCr = x.GroupBy(y => y.ObjectCrId).ToDictionary(
                        y => y.Key,
                        y => new
                        {
                            Cost = y.Where(z => z.Sum.HasValue).Sum(z => z.Sum.Value),
                            Names = y.Select(z => z.Name).Distinct().Aggregate((curr, next) => string.Format("{0}, {1}", curr, next)),
                            DateEndWork = y.Max(z => z.DateEndWork)
                        })
                    });

            var resource = Container.Resolve<IDomainService<FinanceSourceResource>>().GetAll()
                  .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                  .Where(x => x.ObjectCr.ProgramCr.Id == programCrId && (x.FinanceSource.Code == "3" || x.FinanceSource.Code == "2" || x.FinanceSource.Code == "4"))
                  .Select(x => new
                  {
                      Municipality = x.ObjectCr.RealityObject.Municipality.Name,
                      ObjectCrId = x.ObjectCr.Id,
                      x.FundResource,
                      x.BudgetSubject,
                      x.BudgetMu,
                      x.OwnerResource
                  })
                  .AsEnumerable()
                  .GroupBy(x => x.Municipality)
                  .ToDictionary(
                    x => x.Key,
                    x => new
                    {
                        FundResource = x.Where(y => y.FundResource.HasValue).Sum(y => y.FundResource.Value),
                        BudgetSubject = x.Where(y => y.BudgetSubject.HasValue).Sum(y => y.BudgetSubject.Value),
                        BudgetMu = x.Where(y => y.BudgetMu.HasValue).Sum(y => y.BudgetMu.Value),
                        OwnerResource = x.Where(y => y.OwnerResource.HasValue).Sum(y => y.OwnerResource.Value),
                        objectsCr = x.GroupBy(y => y.ObjectCrId).ToDictionary(
                        y => y.Key,
                        y => new
                        {
                            FundResource = y.Where(z => z.FundResource.HasValue).Sum(z => z.FundResource.Value),
                            BudgetSubject = y.Where(z => z.BudgetSubject.HasValue).Sum(z => z.BudgetSubject.Value),
                            BudgetMu = y.Where(z => z.BudgetMu.HasValue).Sum(z => z.BudgetMu.Value),
                            OwnerResource = y.Where(z => z.OwnerResource.HasValue).Sum(z => z.OwnerResource.Value)
                        })
                    });

            var sectionMo = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMO");

            var totalAreaMkd = 0M;
            var totalAreaLivingNotLivingMkd = 0M;
            var totalAreaLivingOwned = 0M;
            var totalNumberLiving = 0M;
            var totalCostWorks185Fz = 0M;
            var totalFundResource185Fz = 0M;
            var totalBudgetSubject185Fz = 0M;
            var totalBudgetMu185Fz = 0M;
            var totalOwnerResource185Fz = 0M;
            var totalSpecificCos185Fz = 0M;
            var totalCostWorks = 0M;
            var totalCostCr = 0M;

            foreach (var mu in municipalities.OrderBy(x => x.Key))
            {
                var num = 0;
                var totalMoCostWorks185Fz = works185Fz.ContainsKey(mu.Key) ? works185Fz[mu.Key].Cost : 0M;
                var totalMoCostWorks = works.ContainsKey(mu.Key) ? works[mu.Key].Cost : 0M;
                var totalMoCostCr = totalMoCostWorks185Fz + totalMoCostWorks;
                var totalMoSpecificCos185Fz = totalMoCostWorks185Fz / mu.Value.TotalMoAreaMkd;
                var totalMoFundResource185Fz = resource185Fz.ContainsKey(mu.Key) ? resource185Fz[mu.Key].FundResource : 0M;
                var totalMoBudgetSubject185Fz = resource185Fz.ContainsKey(mu.Key) ? resource185Fz[mu.Key].BudgetSubject : 0M;
                var totalMoBudgetMu185Fz = resource185Fz.ContainsKey(mu.Key) ? resource185Fz[mu.Key].BudgetMu : 0M;
                var totalMoOwnerResource185Fz = resource185Fz.ContainsKey(mu.Key) ? resource185Fz[mu.Key].OwnerResource : 0M;

                totalAreaMkd += mu.Value.TotalMoAreaMkd;
                totalAreaLivingNotLivingMkd += mu.Value.TotalMoAreaLivingNotLivingMk;
                totalAreaLivingOwned += mu.Value.TotalMoAreaLivingOwned;
                totalNumberLiving += mu.Value.TotalMoNumberLiving;
                totalCostWorks185Fz += totalMoCostWorks185Fz;
                totalFundResource185Fz += totalMoFundResource185Fz;
                totalBudgetSubject185Fz += totalMoBudgetSubject185Fz;
                totalBudgetMu185Fz += totalMoBudgetMu185Fz;
                totalOwnerResource185Fz += totalMoOwnerResource185Fz;
                totalSpecificCos185Fz += totalMoSpecificCos185Fz;
                totalCostWorks += totalMoCostWorks;
                totalCostCr += totalMoCostCr;

                sectionMo.ДобавитьСтроку();
                sectionMo["mo"] = mu.Key;
                sectionMo["totalMoAreaMkd"] = mu.Value.TotalMoAreaMkd;
                sectionMo["totalMoAreaLivingNotLivingMkd"] = mu.Value.TotalMoAreaLivingNotLivingMk;
                sectionMo["totalMoAreaLivingOwned"] = mu.Value.TotalMoAreaLivingOwned;
                sectionMo["totalMoNumberLiving"] = mu.Value.TotalMoNumberLiving;
                sectionMo["totalMoCostWorks185Fz"] = totalMoCostWorks185Fz;
                sectionMo["totalMoFundResource185Fz"] = totalMoFundResource185Fz;
                sectionMo["totalMoBudgetSubject185Fz"] = totalMoBudgetSubject185Fz;
                sectionMo["totalMoBudgetMu185Fz"] = totalMoBudgetMu185Fz;
                sectionMo["totalMoOwnerResource185Fz"] = totalMoOwnerResource185Fz;
                sectionMo["totalMoSpecificCos185Fz"] = totalMoSpecificCos185Fz;
                sectionMo["totalMoCostWorks"] = totalMoCostWorks;
                sectionMo["totalMoCostCr"] = totalMoCostCr;

                var sectionHome = sectionMo.ДобавитьСекцию("sectionHome");
                foreach (var objectCr in mu.Value.objectsCr)
                {
                    num++;

                    var objectCrWorks185Fz = works185Fz.ContainsKey(mu.Key)
                                                 ? works185Fz[mu.Key].objectsCr.ContainsKey(objectCr.Id) ? works185Fz[mu.Key].objectsCr[objectCr.Id] : null
                                                 : null;
                    var objectCrWorks = works.ContainsKey(mu.Key)
                                                 ? works[mu.Key].objectsCr.ContainsKey(objectCr.Id) ? works[mu.Key].objectsCr[objectCr.Id] : null
                                                 : null;
                    var objectCrResource185Fz = resource185Fz.ContainsKey(mu.Key)
                                                 ? resource185Fz[mu.Key].objectsCr.ContainsKey(objectCr.Id) ? resource185Fz[mu.Key].objectsCr[objectCr.Id] : null
                                                 : null;
                    var objectCrResource = resource.ContainsKey(mu.Key)
                                                ? resource[mu.Key].objectsCr.ContainsKey(objectCr.Id) ? resource[mu.Key].objectsCr[objectCr.Id] : null
                                                : null;
                    var costWorks185Fz = objectCrWorks185Fz != null ? objectCrWorks185Fz.Cost : 0M;
                    var costWorks = objectCrWorks != null ? objectCrWorks.Cost : 0M;
                    var specificCos185Fz = costWorks185Fz > 0 && objectCr.AreaMkd > 0
                                               ? costWorks185Fz / objectCr.AreaMkd
                                               : 0m;

                    sectionHome.ДобавитьСтроку();
                    sectionHome["num"] = num;
                    sectionHome["address"] = objectCr.Address;
                    sectionHome["dateCommissioning"] = objectCr.DateCommissioning.HasValue ? objectCr.DateCommissioning.Value.Year.ToStr() : string.Empty;
                    sectionHome["dateLastOverhaul"] = objectCr.DateLastOverhaul.HasValue ? objectCr.DateLastOverhaul.Value.Year.ToStr() : string.Empty;
                    sectionHome["wallMaterial"] = objectCr.WallMaterial;
                    sectionHome["floors"] = objectCr.Floors;
                    sectionHome["numberEntrances"] = objectCr.NumberEntrances;
                    sectionHome["areaMkd"] = objectCr.AreaMkd;
                    sectionHome["areaLivingNotLivingMkd"] = objectCr.AreaLivingNotLivingMkd;
                    sectionHome["areaLivingOwned"] = objectCr.AreaLivingOwned;
                    sectionHome["numberLiving"] = objectCr.NumberLiving;
                    sectionHome["works185Fz"] = objectCrWorks185Fz != null ? objectCrWorks185Fz.Names : string.Empty;
                    sectionHome["costWorks185Fz"] = costWorks185Fz;
                    sectionHome["fundResource185Fz"] = objectCrResource185Fz != null ? objectCrResource185Fz.FundResource : 0M;
                    sectionHome["budgetSubject185Fz"] = objectCrResource185Fz != null ? objectCrResource185Fz.BudgetSubject : 0M;
                    sectionHome["budgetMu185Fz"] = objectCrResource185Fz != null ? objectCrResource185Fz.BudgetMu : 0M;
                    sectionHome["ownerResource185Fz"] = objectCrResource185Fz != null ? objectCrResource185Fz.OwnerResource : 0M;
                    sectionHome["specificCos185Fz"] = specificCos185Fz;
                    sectionHome["limitCost185Fz"] = 9000;
                    sectionHome["plannedCompletionDate185Fz"] = objectCrWorks185Fz != null && objectCrWorks185Fz.DateEndWork > DateTime.MinValue ? objectCrWorks185Fz.DateEndWork.ToShortDateString() : string.Empty;
                    sectionHome["works"] = objectCrWorks != null ? objectCrWorks.Names : string.Empty;
                    sectionHome["costWorks"] = costWorks;

                    sectionHome["budgetSubject"] = objectCrResource != null ? objectCrResource.BudgetSubject : 0M;
                    sectionHome["budgetMu"] = objectCrResource != null ? objectCrResource.BudgetMu : 0M;
                    sectionHome["ownerResource"] = objectCrResource != null ? objectCrResource.OwnerResource : 0M;
                    sectionHome["costCr"] = costWorks185Fz + costWorks;
                }
            }

            reportParams.SimpleReportParams["totalAreaMkd"] = totalAreaMkd;
            reportParams.SimpleReportParams["totalAreaLivingNotLivingMkd"] = totalAreaLivingNotLivingMkd;
            reportParams.SimpleReportParams["totalAreaLivingOwned"] = totalAreaLivingOwned;
            reportParams.SimpleReportParams["totalNumberLiving"] = totalNumberLiving;
            reportParams.SimpleReportParams["totalCostWorks185Fz"] = totalCostWorks185Fz;
            reportParams.SimpleReportParams["totalFundResource185Fz"] = totalFundResource185Fz;
            reportParams.SimpleReportParams["totalBudgetSubject185Fz"] = totalBudgetSubject185Fz;
            reportParams.SimpleReportParams["totalBudgetMu185Fz"] = totalBudgetMu185Fz;
            reportParams.SimpleReportParams["totalOwnerResource185Fz"] = totalOwnerResource185Fz;
            reportParams.SimpleReportParams["totalSpecificCos185Fz"] = totalSpecificCos185Fz;
            reportParams.SimpleReportParams["totalCostWorks"] = totalCostWorks;
            reportParams.SimpleReportParams["totalCostCr"] = totalCostCr;
        }
    }
}
