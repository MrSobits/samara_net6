namespace Bars.GkhCr.Report
{
    using System;
    using System.Linq;
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Castle.Windsor;

    public class ProgramCrProgressOperativeReport : BasePrintForm
    {
        #region Свойства

        private long programCrId;
        private long[] municipalityIds;
        private DateTime reportDate;
        public IWindsorContainer Container { get; set; }

        public ProgramCrProgressOperativeReport()
            : base(new ReportTemplateBinary(Properties.Resources.ProgramCrProgressOperativeReport))
        {
        }

        public override string RequiredPermission
        {
            get { return "Reports.CR.ProgramCrProgressOperativeReport"; }
        }

        public override string Name
        {
            get { return "Оперативный отчет о ходе реализации программ КР МКД"; }
        }

        public override string Desciption
        {
            get { return "Оперативный отчет о ходе реализации программ КР МКД"; }
        }

        public override string GroupName
        {
            get { return "Формы для фонда"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ProgramCrProgressOperativeReport"; }
        }

        #endregion

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params["programCrId"].ToInt();

            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            reportDate = baseParams.Params["reportDate"].ToDateTime();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var program = this.Container.Resolve<IDomainService<ProgramCr>>().Load(programCrId);
            var programYear = program != null ? program.Period.DateStart.Year : 0;

            var yearStartDate = new DateTime(reportDate.Year, 1, 1);

            var serviceFinanceSource = Container.Resolve<IDomainService<FinanceSource>>();
            var serviceFinanceSourceResource = Container.Resolve<IDomainService<FinanceSourceResource>>();
            var serviceObjectCr = Container.Resolve<IDomainService<ObjectCr>>();
            var serviceMunicipality = Container.Resolve<IDomainService<Municipality>>();
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceBuildContract = Container.Resolve<IDomainService<BuildContract>>();
            var serviceBasePaymentOrder = Container.Resolve<IDomainService<BasePaymentOrder>>();
            var serviceArchiveSmr = Container.Resolve<IDomainService<ArchiveSmr>>();
            var servicePerformedWorkAct = Container.Resolve<IDomainService<PerformedWorkAct>>();

            // Финансирование по 185-ФЗ
            var financeSourceId = serviceFinanceSource.GetAll().Where(x => x.Code == "1").Select(x => x.Id).FirstOrDefault();

            var municipalityDict = serviceMunicipality.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, x => x.Name);

            // Данные для полей 3-4
            var finSrcResourceByMuDict = serviceFinanceSourceResource.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                .Where(x => x.FinanceSource.Id == financeSourceId)
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .GroupBy(x => x.ObjectCr.RealityObject.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    total = x.Sum(y => y.BudgetMu + y.BudgetSubject + y.OwnerResource + y.FundResource),
                    fundResource = x.Sum(y => y.FundResource)
                })
                .ToDictionary(x => x.Key);

            var objectsWith185FzSrcWorksQuery = serviceTypeWorkCr.GetAll()
                .Where(x => x.FinanceSource.Id == financeSourceId)
                .Select(x => x.ObjectCr.Id);

            var objectsCrIdsQuery = serviceObjectCr.GetAll()
                .Where(x => x.ProgramCr.Id == programCrId)
                .Where(x => objectsWith185FzSrcWorksQuery.Contains(x.Id))
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => x.Id);

            // Данные для полей 5-7
            var quantitiveIndicatorsByMuDict = serviceObjectCr.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ProgramCr.Id == programCrId)
                .Where(x => objectsWith185FzSrcWorksQuery.Contains(x.Id))
                .GroupBy(x => x.RealityObject.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    countRo = x.Count(),
                    totalArea = x.Sum(y => y.RealityObject.AreaMkd),
                    numberLiving = x.Sum(y => y.RealityObject.NumberLiving)
                })
                .ToDictionary(x => x.Key);

            // Данные для полей 8-9
            var buildContractDataByMuDict = serviceBuildContract.GetAll()
                .Where(x => objectsCrIdsQuery.Contains(x.ObjectCr.Id))
                .GroupBy(x => x.ObjectCr.RealityObject.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    CountRo = x.Select(y => y.ObjectCr.RealityObject.Id).Distinct().Count(),
                    Sum = x.Sum(y => y.Sum)
                })
                .ToDictionary(x => x.Key);

            // Данные для поля 10
            var paymentsByMuDict = serviceBasePaymentOrder.GetAll()
                .Where(x => x.DocumentDate >= yearStartDate)
               .Where(x => x.DocumentDate <= reportDate)
               .Where(x => x.TypePaymentOrder == TypePaymentOrder.Out)
                .Where(x => x.BankStatement.TypeFinanceGroup == TypeFinanceGroup.ProgramCr)
                .Where(x => objectsCrIdsQuery.Contains(x.BankStatement.ObjectCr.Id))
                .GroupBy(x => x.BankStatement.ObjectCr.RealityObject.Municipality.Id)
                .Select(x => new { x.Key, Sum = x.Sum(y => y.Sum) })
                .ToDictionary(x => x.Key, x => x.Sum);

            // Подготовка данных для полей 11-14
            var typeWorks = serviceTypeWorkCr.GetAll()
                   .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                   .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                   .Where(x => x.FinanceSource.Id == financeSourceId)
                   .Where(x => x.Work.TypeWork == TypeWork.Work)
                   .Select(x => new
                   {
                       muId = x.ObjectCr.RealityObject.Municipality.Id,
                       roId = x.ObjectCr.RealityObject.Id,
                       roArea = x.ObjectCr.RealityObject.AreaMkd,
                       roNumberLiving = x.ObjectCr.RealityObject.NumberLiving,
                       x.PercentOfCompletion,
                       TypeWorkCrId = x.Id
                   })
                   .AsEnumerable()
                   .ToDictionary(x => x.TypeWorkCrId);

            var archiveRecords = serviceArchiveSmr.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == program.Id)
                .Where(x => x.TypeWorkCr.FinanceSource.Id == financeSourceId)
                .Where(x => x.TypeWorkCr.Work.TypeWork == TypeWork.Work)
                .Where(x => x.DateChangeRec <= this.reportDate)
                .Select(x => new
                {
                    muId = x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id,
                    roId = x.TypeWorkCr.ObjectCr.RealityObject.Id,
                    x.PercentOfCompletion,
                    x.Id,
                    x.DateChangeRec,
                    TypeWorkCrId = x.TypeWorkCr.Id
                })
                .AsEnumerable()
                .GroupBy(z => z.TypeWorkCrId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var archiveRec = x.OrderByDescending(p => p.DateChangeRec)
                                          .ThenByDescending(p => p.Id)
                                          .First();

                        return archiveRec.PercentOfCompletion;
                    });

            // Данные для полей 11
            var worksInProgressObjectsCountByMuDict = typeWorks
                .GroupBy(x => x.Value.muId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.Value.roId)
                          .Count(
                              z =>
                              z.Any(r => programYear >= 2013 ?
                                                (r.Value.PercentOfCompletion <= 99 && r.Value.PercentOfCompletion >= 1) : 
                                                (archiveRecords.ContainsKey(r.Value.TypeWorkCrId)
                                                && archiveRecords[r.Value.TypeWorkCrId] < 100
                                                && archiveRecords[r.Value.TypeWorkCrId] > 0))));
            // Данные для полей 12-14
            var allworksFinishedRobjectsListByMuDict = typeWorks
                .GroupBy(x => x.Value.muId)
                .ToDictionary(
                    x => x.Key, 
                    x => x.GroupBy(y => y.Value.roId)
                          .Where(
                              z =>
                              z.All(r => programYear >= 2013
                                             ? (r.Value.PercentOfCompletion == 100) 
                                             : (archiveRecords.ContainsKey(r.Value.TypeWorkCrId)
                                                && archiveRecords[r.Value.TypeWorkCrId] == 100)))
                          .Select(y => new
                                           {
                                               roId = y.Key, 
                                               y.First().Value.roArea, 
                                               y.First().Value.roNumberLiving
                                           })
                          .ToList());

            // Данные для поля 15
            var actsDataByMuDict = servicePerformedWorkAct.GetAll()
                .Where(x => !x.State.StartState)
                .Where(x => x.TypeWorkCr.FinanceSource.Id == financeSourceId)
                .Where(x => objectsCrIdsQuery.Contains(x.ObjectCr.Id))
                .Select(x => new
                {
                    muId = x.ObjectCr.RealityObject.Municipality.Id,
                    roId = x.ObjectCr.RealityObject.Id,
                    x.Sum
                })
                .AsEnumerable()
                .GroupBy(x => x.muId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        if (allworksFinishedRobjectsListByMuDict.ContainsKey(x.Key))
                        {
                            var roIds = allworksFinishedRobjectsListByMuDict[x.Key].Select(y => y.roId).ToList();
                            var sumList = x.Where(y => roIds.Contains(y.roId)).Select(y => y.Sum).ToList();
                            return sumList.Any() ? sumList.Sum() : 0;
                        }

                        return 0;
                    });

            reportParams.SimpleReportParams["day"] = this.reportDate.Day;
            reportParams.SimpleReportParams["monthYear"] = this.reportDate.ToString("MMMM yyyy");

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            
            var fieldNames = Enumerable.Range(3, 13).Select(x => x.ToStr()).ToList();
            
            var totalsDict = fieldNames.ToDictionary(x => "column" + x + "Total", _ => 0m);
            var divider = fieldNames.ToDictionary(x => "column" + x, _ => 1);

            divider["column3"] = 1000000;
            divider["column4"] = 1000000;
            divider["column6"] = 1000;
            divider["column7"] = 1000;
            divider["column9"] = 1000000;
            divider["column10"] = 1000000;
            divider["column13"] = 1000;
            divider["column14"] = 1000;
            divider["column15"] = 1000000;
            
            var count = 0;

            foreach (var municipality in municipalityDict.OrderBy(x => x.Value))
            {
                var rowDict = fieldNames.ToDictionary(x => "column" + x, _ => 0m);

                section.ДобавитьСтроку();
                section["column1"] = ++count;
                section["column2"] = municipality.Value;

                if (finSrcResourceByMuDict.ContainsKey(municipality.Key))
                {
                    var finSrcResource = finSrcResourceByMuDict[municipality.Key];
                    rowDict["column3"] = finSrcResource.total.HasValue ? finSrcResource.total.Value : 0;
                    rowDict["column4"] = finSrcResource.fundResource.HasValue ? finSrcResource.fundResource.Value : 0;
                }

                if (quantitiveIndicatorsByMuDict.ContainsKey(municipality.Key))
                {
                    var quantitativeIndicators = quantitiveIndicatorsByMuDict[municipality.Key];
                    rowDict["column5"] = quantitativeIndicators.countRo;
                    rowDict["column6"] = quantitativeIndicators.totalArea.HasValue ? quantitativeIndicators.totalArea.Value : 0; 
                    rowDict["column7"] = quantitativeIndicators.numberLiving.HasValue ? quantitativeIndicators.numberLiving.Value : 0; 
                }

                if (buildContractDataByMuDict.ContainsKey(municipality.Key))
                {
                    var buildContract = buildContractDataByMuDict[municipality.Key];
                    rowDict["column8"] = buildContract.CountRo;
                    rowDict["column9"] = buildContract.Sum.HasValue ? buildContract.Sum.Value : 0;
                }

                if (paymentsByMuDict.ContainsKey(municipality.Key))
                {
                    var payments = paymentsByMuDict[municipality.Key];
                    rowDict["column10"] = payments.HasValue ? payments.Value : 0;
                }

                if (worksInProgressObjectsCountByMuDict.ContainsKey(municipality.Key))
                {
                    rowDict["column11"] = worksInProgressObjectsCountByMuDict[municipality.Key];
                }

                if (allworksFinishedRobjectsListByMuDict.ContainsKey(municipality.Key))
                {
                    var allworksFinishedRobjectsList = allworksFinishedRobjectsListByMuDict[municipality.Key];

                    rowDict["column12"] = allworksFinishedRobjectsList.Count;
                    rowDict["column13"] = allworksFinishedRobjectsList.Sum(x => x.roArea.HasValue ? x.roArea.Value : 0);
                    rowDict["column14"] = allworksFinishedRobjectsList.Sum(x => x.roNumberLiving.HasValue ? x.roNumberLiving.Value : 0);
                }

                if (actsDataByMuDict.ContainsKey(municipality.Key))
                {
                    var actsData = actsDataByMuDict[municipality.Key];
                    rowDict["column15"] = actsData.HasValue ? actsData.Value : 0;
                }
                    rowDict.ForEach(x => section[x.Key] = x.Value / divider[x.Key]);
                    rowDict.ForEach(x => totalsDict[x.Key + "Total"] += x.Value / divider[x.Key]);
            }

            totalsDict.ForEach(x => reportParams.SimpleReportParams[x.Key] = x.Value);
        }  
    }
}
