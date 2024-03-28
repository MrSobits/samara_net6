namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;

    public class AnalysOfRealizByProgram : BasePrintForm
    {
        #region Входящие параметры
        private long programCrId;
        private DateTime reportDate;
        #endregion

        public IWindsorContainer Container { get; set; }

        public AnalysOfRealizByProgram()
            : base(new ReportTemplateBinary(Properties.Resources.AnalysOfRealizByProgram))
        {
        }

        public override string Name
        {
            get
            {
                return "Анализ реализации программы";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Анализ реализации программы";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Ход капремонта";
            }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.AnalysOfRealizByProgram"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.CR.AnalysOfRealizByProgram"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params.GetAs<long>("programCrId", 0);

            reportDate = baseParams.Params.GetAs("reportDate", DateTime.MinValue);
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var program = this.Container.Resolve<IDomainService<ProgramCr>>().Load(programCrId);
            var programYear = program != null ? program.Period.DateStart.Year : 0;

            reportParams.SimpleReportParams["ProgramCrName"] = program.Name;
            reportParams.SimpleReportParams["reportDate"] = reportDate.ToShortDateString();

            var serviceObjectCr = Container.Resolve<IDomainService<ObjectCr>>();
            var serviceBuildContract = Container.Resolve<IDomainService<BuildContract>>();
            var serviceTypeWorkCr = Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceArchiveSmr = Container.Resolve<IDomainService<ArchiveSmr>>();
            var servicePerformedWorkAct = this.Container.Resolve<IDomainService<PerformedWorkAct>>();
            var serviceBasePaymentOrder = Container.Resolve<IDomainService<BasePaymentOrder>>();

            var objectCrCountByMuDict = serviceObjectCr.GetAll()
                .Where(x => x.ProgramCr.Id == programCrId)
                .GroupBy(x => new { x.RealityObject.Municipality.Id, x.RealityObject.Municipality.Name })
                .Select(x => new { x.Key, objectCrCount = x.Count() })
                .AsEnumerable()
                .ToDictionary(x => x.Key, x => x.objectCrCount);

            var objectCrIdsQuery = serviceObjectCr.GetAll()
                .Where(x => x.ProgramCr.Id == programCrId)
                .Select(x => x.Id);

            var typeWorks = serviceTypeWorkCr.GetAll()
                .Where(x => objectCrIdsQuery.Contains(x.ObjectCr.Id))
                .Select(x => new
                {
                    muId = x.ObjectCr.RealityObject.Municipality.Id,
                    objectCrId = x.ObjectCr.Id,
                    x.ObjectCr.DateAcceptCrGji,
                    x.PercentOfCompletion,
                    x.Work.Code,
                    x.Work.TypeWork,
                    x.DateEndWork,
                    x.Sum,
                    x.CostSum,
                    TypeWorkCrId = x.Id
                })
                .ToList();

            var installationOfMeterCodes = new List<string> { "7", "8", "9", "10" };

            var typedObjectListByMuDict = typeWorks.GroupBy(x => x.muId).ToDictionary(
                x => x.Key,
                x =>
                {
                    var objectsDataList =
                        x.Where(y => y.TypeWork == TypeWork.Work)
                         .GroupBy(y => y.objectCrId)
                         .Select(
                             y =>
                                 {
                                     var workCodes = y.Select(z => z.Code).ToList();
                                     //Считаем, что комплексный ремонт обязательно должен включать все установки приборов учета + любую другую работу
                                     var complexRepair = installationOfMeterCodes.All(workCodes.Contains) && y.Any(z => !installationOfMeterCodes.Contains(z.Code));
                                     var onlyInstallationWorks = workCodes.All(installationOfMeterCodes.Contains);

                                     return new { y.Key, complexRepair, onlyInstallationWorks };
                             })
                         .ToList();

                    return
                        new
                        {
                            complexRepairObjectsList = objectsDataList.Where(y => y.complexRepair).Select(y => y.Key).ToList(),
                            onlyInstallationObjectsList = objectsDataList.Where(y => y.onlyInstallationWorks).Select(y => y.Key).ToList(),
                        };
                });

            var buildersCountByMuDict = serviceBuildContract.GetAll()
                .Where(x => objectCrIdsQuery.Contains(x.ObjectCr.Id))
                .GroupBy(x => x.ObjectCr.RealityObject.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    CountBuilders = x.Select(y => y.Builder.Id).Distinct().Count()
                })
                .ToDictionary(x => x.Key, x => x.CountBuilders);

            var archiveRecords = serviceArchiveSmr.GetAll()
                .Where(x => objectCrIdsQuery.Contains(x.TypeWorkCr.ObjectCr.Id))
                .Where(x => x.DateChangeRec <= this.reportDate)
                .Select(x => new
                {
                    muId = x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id,
                    objectCrId = x.TypeWorkCr.ObjectCr.Id,
                    x.PercentOfCompletion,
                    x.Id,
                    x.DateChangeRec,
                    x.CostSum,
                    TypeWorkCrId = x.TypeWorkCr.Id
                })
                .AsEnumerable()
                .GroupBy(z => z.TypeWorkCrId)
                .Select(x => x.OrderByDescending(p => p.DateChangeRec)
                                          .ThenByDescending(p => p.Id)
                                          .First())
                .ToList();

            var typeWorkArchivePercentDict = archiveRecords.ToDictionary(x => x.TypeWorkCrId, x => x.PercentOfCompletion);

            var typeWorkDataByMuDict = typeWorks
                .GroupBy(x => x.muId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var completionDate = x.Max(y => y.DateEndWork);
                        var estimatedCost = x.Sum(y => y.Sum.HasValue ? y.Sum.Value : 0) / 1000;
                        var complexRepairEstimatedCost = 0m;
                        var installationEstimatedCost = 0m;

                        var workInProgressObjectsCount = 0;
                        var workInProgressComplexRepairObjectsCount = 0;
                        var workInProgressInstallationObjectsCount = 0;

                        var repairCompleteObjectsCount = 0;
                        var complexRepairCompleteObjectsCount = 0;
                        var repairCompleteInstallationObjectsCount = 0;

                        var gjiAcceptedObjectsCount = 0;
                        var gjiAcceptedComplexRepairObjectsCount = 0;
                        var gjiAcceptedInstallationObjectsCount = 0;

                        if (typedObjectListByMuDict.ContainsKey(x.Key))
                        {
                            var objectLists = typedObjectListByMuDict[x.Key];
                            var complexRepairObjectsList = objectLists.complexRepairObjectsList;
                            var onlyInstallationObjectsList = objectLists.onlyInstallationObjectsList;

                            var objectsDataList = x
                                .GroupBy(y => new { y.objectCrId, y.DateAcceptCrGji })
                                .Select(y => new
                                {
                                    y.Key,
                                    workInProgress = y.Any(z => programYear >= 2013
                                            ? (typeWorkArchivePercentDict.ContainsKey(z.TypeWorkCrId)
                                            && typeWorkArchivePercentDict[z.TypeWorkCrId] < 100
                                            && typeWorkArchivePercentDict[z.TypeWorkCrId] > 0)
                                            : (z.PercentOfCompletion < 100 && z.PercentOfCompletion > 0)),

                                    allWorksFinished = y.All(z => programYear >= 2013
                                            ? (typeWorkArchivePercentDict.ContainsKey(z.TypeWorkCrId)
                                            && typeWorkArchivePercentDict[z.TypeWorkCrId] == 100)
                                            : (z.PercentOfCompletion == 100)),

                                    complexRepair = complexRepairObjectsList.Contains(y.Key.objectCrId),
                                    onlyInstallation = onlyInstallationObjectsList.Contains(y.Key.objectCrId),
                                    Sum = y.Sum(z => z.Sum.HasValue ? z.Sum.Value : 0) / 1000
                                })
                                .ToList();

                            var complexRepairObjects = objectsDataList.Where(y => y.complexRepair).ToList();
                            var onlyInstallationObjects = objectsDataList.Where(y => y.onlyInstallation).ToList();

                            complexRepairEstimatedCost = complexRepairObjects.Sum(y => y.Sum);
                            installationEstimatedCost = onlyInstallationObjects.Sum(y => y.Sum);

                            workInProgressComplexRepairObjectsCount = complexRepairObjects.Count(y => y.workInProgress);
                            workInProgressInstallationObjectsCount = onlyInstallationObjects.Count(y => y.workInProgress);
                            workInProgressObjectsCount = workInProgressComplexRepairObjectsCount + workInProgressInstallationObjectsCount;

                            var complexRepairCompleteObjects = complexRepairObjects.Where(y => y.allWorksFinished).ToList();
                            var repairCompleteInstallationObjects = onlyInstallationObjects.Where(y => y.allWorksFinished).ToList();

                            complexRepairCompleteObjectsCount = complexRepairCompleteObjects.Count;
                            repairCompleteInstallationObjectsCount = repairCompleteInstallationObjects.Count;
                            repairCompleteObjectsCount = complexRepairCompleteObjectsCount + repairCompleteInstallationObjectsCount;

                            gjiAcceptedComplexRepairObjectsCount = complexRepairCompleteObjects.Count(y => y.Key.DateAcceptCrGji != null && y.Key.DateAcceptCrGji != DateTime.MinValue);
                            gjiAcceptedInstallationObjectsCount = repairCompleteInstallationObjects.Count(y => y.Key.DateAcceptCrGji != null && y.Key.DateAcceptCrGji != DateTime.MinValue);
                            gjiAcceptedObjectsCount = gjiAcceptedComplexRepairObjectsCount + gjiAcceptedInstallationObjectsCount;
                        }

                        return new
                                   {
                                       completionDate, 
                                       estimatedCost, 
                                       complexRepairEstimatedCost, 
                                       installationEstimatedCost,
                                       workInProgressObjectsCount,
                                       workInProgressComplexRepairObjectsCount,
                                       workInProgressInstallationObjectsCount,
                                       repairCompleteObjectsCount,
                                       complexRepairCompleteObjectsCount,
                                       repairCompleteInstallationObjectsCount,
                                       gjiAcceptedObjectsCount,
                                       gjiAcceptedComplexRepairObjectsCount,
                                       gjiAcceptedInstallationObjectsCount
                                   };
                    });

            var receivedFundsByMuDict = serviceBasePaymentOrder.GetAll()
                .Where(x => x.TypePaymentOrder == TypePaymentOrder.In)
                .Where(x => x.BankStatement.TypeFinanceGroup == TypeFinanceGroup.ProgramCr)
                .Where(x => x.BankStatement.ObjectCr.ProgramCr.Id == programCrId)
                .GroupBy(x => new { muId = x.BankStatement.ObjectCr.RealityObject.Municipality.Id, x.TypeFinanceSource })
                .Select(x => new
                {
                    x.Key,
                    Sum = x.Sum(y => y.Sum)
                })
                .AsEnumerable()
                .GroupBy(x => x.Key.muId)
                .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var dictOfsums = x.GroupBy(y => y.Key.TypeFinanceSource)
                                          .ToDictionary(y => y.Key, y => y.Sum(z => z.Sum.HasValue ? z.Sum.Value : 0) / 1000);

                        var receivedFundsFederal = dictOfsums.ContainsKey(TypeFinanceSource.FederalFunds) ? dictOfsums[TypeFinanceSource.FederalFunds] : 0;
                        var receivedFundsSubject = dictOfsums.ContainsKey(TypeFinanceSource.SubjectBudgetFunds) ? dictOfsums[TypeFinanceSource.SubjectBudgetFunds] : 0;
                        var receivedFundsLocal = dictOfsums.ContainsKey(TypeFinanceSource.PlaceBudgetFunds) ? dictOfsums[TypeFinanceSource.PlaceBudgetFunds] : 0;
                        var receivedFundsOwner = dictOfsums.ContainsKey(TypeFinanceSource.OccupantFunds) ? dictOfsums[TypeFinanceSource.OccupantFunds] : 0;
                        var receivedFunds = receivedFundsFederal + receivedFundsSubject + receivedFundsLocal + receivedFundsOwner;

                        return new { receivedFunds, receivedFundsFederal, receivedFundsSubject, receivedFundsLocal, receivedFundsOwner };
                    });

            var paidToBuildersByMuDict = serviceBasePaymentOrder.GetAll()
                .Where(x => x.TypePaymentOrder == TypePaymentOrder.Out)
                .Where(x => x.BankStatement.TypeFinanceGroup == TypeFinanceGroup.ProgramCr)
                .Where(x => x.BankStatement.ObjectCr.ProgramCr.Id == programCrId)
                .GroupBy(x => x.BankStatement.ObjectCr.RealityObject.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    Sum = x.Sum(y => y.Sum)
                })
                .ToDictionary(x => x.Key, x => (x.Sum.HasValue ? x.Sum.Value : 0) / 1000);

            var performedWorkData = programYear >= 2013
                                        ? archiveRecords.Select(x => new { x.muId, x.CostSum })
                                        : typeWorks.Select(x => new { x.muId, x.CostSum });

            var performedWorkDataByMuDict = performedWorkData
                  .GroupBy(x => x.muId)
                  .ToDictionary(
                    x => x.Key,
                    x =>
                    {
                        var cost = x.Sum(y => y.CostSum.HasValue ? y.CostSum.Value : 0) / 1000;
                        var costOfLimit = 1m;

                        if (typeWorkDataByMuDict.ContainsKey(x.Key))
                        {
                            var limit = typeWorkDataByMuDict[x.Key].estimatedCost;

                            costOfLimit = limit > 0 ? cost / limit : 1;
                        }

                        return new { cost, costOfLimit };
                    });
            

            var actDataByMuDict = servicePerformedWorkAct.GetAll()
                    .Where(x => objectCrIdsQuery.Contains(x.ObjectCr.Id))
                    .Where(x => x.State.Name == "Принято ГЖИ")
                    .GroupBy(x => x.ObjectCr.RealityObject.Municipality.Id)
                    .Select(x => new
                    {
                        x.Key,
                        count = x.Count(),
                        sum = x.Sum(y => y.Sum) / 1000
                    })
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        {
                            var costOfLimit = 1m;

                            if (typeWorkDataByMuDict.ContainsKey(x.Key) && x.sum.HasValue)
                            {
                                var limit = typeWorkDataByMuDict[x.Key].estimatedCost;

                                costOfLimit = limit > 0 ? x.sum.Value / limit : 1;
                            }

                            return new { x.count, sum = x.sum.HasValue ? x.sum.Value : 0, costOfLimit };
                        });

            var count = 0;
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("sectionData");
            var totalsDict = new Dictionary<string, decimal>();

            foreach (var municipality in objectCrCountByMuDict.OrderBy(x => x.Key.Name))
            {
                var municipalityId = municipality.Key.Id;

                section.ДобавитьСтроку();
                section["num"] = ++count;
                section["municipality"] = municipality.Key.Name;

                Action<string, decimal> set = (field, value) =>
                    {
                        section[field] = value;
                        var tatalKey = "Total" + field;
                        if (totalsDict.ContainsKey(tatalKey))
                        {
                            totalsDict[tatalKey] += value;
                        }
                        else
                        {
                            totalsDict[tatalKey] = value;
                        }
                    };

                set("ObjectsCountByProgram", municipality.Value);

                if (typedObjectListByMuDict.ContainsKey(municipalityId))
                {
                    var data = typedObjectListByMuDict[municipalityId];
                    set("ComplexRepairObjectsCount", data.complexRepairObjectsList.Count);
                    set("InstallationObjectsCount", data.onlyInstallationObjectsList.Count);
                }

                if (buildersCountByMuDict.ContainsKey(municipalityId))
                {
                    set("BuildersCount", buildersCountByMuDict[municipalityId]);
                }

                if (typeWorkDataByMuDict.ContainsKey(municipalityId))
                {
                    var data = typeWorkDataByMuDict[municipalityId];
                    section["CompletionDate"] = data.completionDate.HasValue ? data.completionDate.Value.ToShortDateString() : string.Empty;

                    set("EstimatedCost", data.estimatedCost);
                    set("ComplexRepairEstimatedCost", data.complexRepairEstimatedCost);
                    set("InstallationEstimatedCost", data.installationEstimatedCost);

                    set("WorkInProgressObjectsCount", data.workInProgressObjectsCount);
                    set("WorkInProgressComplexRepairObjectsCount", data.workInProgressComplexRepairObjectsCount);
                    set("WorkInProgressInstallationObjectsCount", data.workInProgressInstallationObjectsCount);

                    set("RepairCompleteObjectsCount", data.repairCompleteObjectsCount);
                    set("ComplexRepairCompleteObjectsCount", data.complexRepairCompleteObjectsCount);
                    set("RepairCompleteInstallationObjectsCount", data.repairCompleteObjectsCount);

                    set("GjiAcceptedObjectsCount", data.gjiAcceptedObjectsCount);
                    set("GjiAcceptedComplexRepairObjectsCount", data.gjiAcceptedComplexRepairObjectsCount);
                    set("GjiAcceptedInstallationObjectsCount", data.gjiAcceptedInstallationObjectsCount);
                }

                if (receivedFundsByMuDict.ContainsKey(municipalityId))
                {
                    var data = receivedFundsByMuDict[municipalityId];
                    set("ReceivedFunds", data.receivedFunds);
                    set("ReceivedFundsFederal", data.receivedFundsFederal);
                    set("ReceivedFundsSubject", data.receivedFundsSubject);
                    set("ReceivedFundsLocal", data.receivedFundsLocal);
                    set("ReceivedFundsOwner", data.receivedFundsOwner);
                }

                if (paidToBuildersByMuDict.ContainsKey(municipalityId))
                {
                    set("PaidToBuilder", paidToBuildersByMuDict[municipalityId]);
                }

                if (performedWorkDataByMuDict.ContainsKey(municipalityId))
                {
                    var data = performedWorkDataByMuDict[municipalityId];
                    set("PerformedWorkCost", data.cost);
                    set("PerformedWorkCostOfLimit", data.costOfLimit);
                }

                if (actDataByMuDict.ContainsKey(municipalityId))
                {
                    var data = actDataByMuDict[municipalityId];
                    set("ActsCount", data.count);
                    set("ActsCost", data.sum);
                    set("ActsCostOfLimit", data.costOfLimit);
                }
            }

            var averageFields = new List<string> { "TotalPerformedWorkCostOfLimit", "TotalActsCostOfLimit" };

            totalsDict.ForEach(x => reportParams.SimpleReportParams[x.Key] = averageFields.Contains(x.Key) ? x.Value / objectCrCountByMuDict.Count : x.Value);
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
}