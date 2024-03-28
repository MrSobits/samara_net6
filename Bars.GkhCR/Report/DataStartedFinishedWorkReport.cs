namespace Bars.GkhCr.Report
{
    
    using B4.Modules.Reports;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;

    class DataStartedFinishedWorkReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime ReportDate = DateTime.MinValue;
        private long programmCrId;
        private List<long> worksIds;

        private Dictionary<long, string> municipalityDict;

        public DataStartedFinishedWorkReport()
            : base(new ReportTemplateBinary(Properties.Resources.DataStartedFinishedWorkReport))
        {
        }

        public override string Name
        {
            get
            {
                return "Сведения о начатых и завершенных работах";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Сведения о начатых и завершенных работах";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчет ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.DataStartedFinishedWorkReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.DataStartedFinishedWorkReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programmCrId = baseParams.Params["programCrId"].ToLong();
            this.ReportDate = baseParams.Params["reportDate"].ToDateTime();

            var worksStr = baseParams.Params["workId"].ToString();
            this.worksIds = !string.IsNullOrEmpty(worksStr)
                               ? worksStr.Split(',').Select(x => x.ToLong()).ToList()
                               : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams) 
        {
            var serviceArchiveSmr = this.Container.Resolve<IDomainService<ArchiveSmr>>();
            var serviceTypeWork = this.Container.Resolve<IDomainService<TypeWorkCr>>();

            var program = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programmCrId);

            var workType = this.Container.Resolve<IDomainService<Work>>().GetAll()
                .Where(x => worksIds.Contains(x.Id))
                .Select(x => x.Code)
                .ToList();
            
            var programYear = program != null ? program.Period.DateStart.Year : 0;

            var typeworkData = serviceTypeWork.GetAll()
                   .Where(x => x.ObjectCr.ProgramCr.Id == programmCrId)
                   .Where(x => worksIds.Contains(x.Work.Id))
                   .Where(x => x.DateEndWork != null && x.DateStartWork != null)
                   .Select(x => new
                   {
                       muId = x.ObjectCr.RealityObject.Municipality.Id,
                       roId = x.ObjectCr.RealityObject.Id,
                       workCode = x.Work.Code,
                       typeWork = x.Work.TypeWork,
                       x.DateStartWork,
                       x.DateEndWork,
                       x.PercentOfCompletion,
                       TypeWorkCrId = x.Id
                   })
                   .AsEnumerable()
                   .GroupBy(x => x.muId)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        x.GroupBy(y => y.roId)
                        .ToDictionary(
                            y => y.Key,
                            y => y.ToDictionary(z => z.TypeWorkCrId,
                                     z => new WorkReportProxy
                                                   {
                                                       PercentOfCompletion = z.PercentOfCompletion, 
                                                       DateStartWork = z.DateStartWork, 
                                                       DateEndWork = z.DateEndWork
                                                   })));

            var archiveWorkDataByCodeByRoByMuId = programYear >= 2013
               ? serviceArchiveSmr.GetAll()
                .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == program.Id)
                .Where(x => x.DateChangeRec <= this.ReportDate)
                .Where(x => worksIds.Contains(x.TypeWorkCr.Work.Id))
                .Where(x => x.TypeWorkCr.DateEndWork != null && x.TypeWorkCr.DateStartWork != null)
                .Select(x => new
                {
                    muId = x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id,
                    roId = x.TypeWorkCr.ObjectCr.RealityObject.Id,
                    x.TypeWorkCr.DateStartWork,
                    x.TypeWorkCr.DateEndWork,
                    TypeWorkId = x.TypeWorkCr.Id,
                    x.PercentOfCompletion,
                    x.Id,
                    x.DateChangeRec
                })
                .AsEnumerable()
                .GroupBy(x => x.muId)
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(y => y.roId)
                          .ToDictionary(
                            y => y.Key, 
                            y =>
                            {
                                var records = y.GroupBy(p => p.TypeWorkId)
                                               .ToDictionary(
                                                  p => p.Key,
                                                  p => p.OrderByDescending(z => z.DateChangeRec)
                                                        .ThenByDescending(z => z.Id)
                                                        .Select(z => new WorkReportProxy
                                                          {
                                                              PercentOfCompletion = z.PercentOfCompletion, 
                                                              DateStartWork = z.DateStartWork, 
                                                              DateEndWork = z.DateEndWork
                                                          })
                                                        .FirstOrDefault());

                                return records;
                            }))

                : typeworkData;

            Func<WorkReportProxy, bool> workStartedFact = x => x.PercentOfCompletion > 0 
                && (x.DateStartWork.HasValue && x.DateStartWork.Value > DateTime.MinValue);

            Func<WorkReportProxy, bool> workStartedInPlanLimits = x => x.PercentOfCompletion > 0 
                && (x.DateStartWork.HasValue && x.DateStartWork.Value > DateTime.MinValue && x.DateStartWork.Value < this.ReportDate);

            Func<WorkReportProxy, bool> workFinishedFact = x => x.PercentOfCompletion == 100 
                && (x.DateEndWork.HasValue && x.DateEndWork.Value > DateTime.MinValue);

            Func<WorkReportProxy, bool> workFinishedInPlanLimits = x => x.PercentOfCompletion == 100
                && (x.DateEndWork.HasValue && x.DateEndWork.Value > DateTime.MinValue && x.DateEndWork < this.ReportDate);

            Func<KeyValuePair<long, Dictionary<long, WorkReportProxy>>, Dictionary<long, Dictionary<long, WorkReportProxy>>, Func<WorkReportProxy, bool>, bool>
                processArchiveData = (robjectWorkData, archiveData, func) =>
                    {
                        var robjectId = robjectWorkData.Key;
                        if (!archiveData.ContainsKey(robjectId))
                        {
                            return false;
                        }

                        // Проходим по всем работам и проверяем наличие записей в архиве для соответствующей работы
                        // Если работа в архиве есть, то применяем к записи архива функцию func

                        return robjectWorkData.Value.All(typeWorkData => archiveData[robjectId].ContainsKey(typeWorkData.Key) && func(archiveData[robjectId][typeWorkData.Key]));
                    };

            municipalityDict = this.Container.Resolve<IDomainService<Municipality>>()
                .GetAll()
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, x => x.Name);

            var startedWorksByMuDict = typeworkData.ToDictionary(
                municipalityData => municipalityData.Key,
                municipalityData =>
                    {
                        var muArchiveData = archiveWorkDataByCodeByRoByMuId.ContainsKey(municipalityData.Key)
                                                ? archiveWorkDataByCodeByRoByMuId[municipalityData.Key]
                                                : null;

                        var plan = municipalityData.Value.Count(robjectData => robjectData.Value.All(typeWorkData => typeWorkData.Value.DateStartWork.HasValue && typeWorkData.Value.DateStartWork < ReportDate));
                        var total = muArchiveData == null ? 0 : municipalityData.Value.Count(robjectData => processArchiveData(robjectData, muArchiveData, workStartedFact));
                        var inPlanLimits = muArchiveData == null ? 0 : municipalityData.Value.Count(robjectData => processArchiveData(robjectData, muArchiveData, workStartedInPlanLimits));

                        return new ReportDataProxy
                        {
                            plan = plan,
                            total = total,
                            inPlanLimits = inPlanLimits
                        };
                    });

            var finishedWorksByMuDict = typeworkData.ToDictionary(
                municipalityData => municipalityData.Key,
                municipalityData =>
                {
                    var muArchiveData = archiveWorkDataByCodeByRoByMuId.ContainsKey(municipalityData.Key)
                                            ? archiveWorkDataByCodeByRoByMuId[municipalityData.Key]
                                            : null;

                    var plan = municipalityData.Value.Count(robjectData => robjectData.Value.All(typeWorkData => typeWorkData.Value.DateEndWork.HasValue && typeWorkData.Value.DateEndWork < ReportDate));
                    var total = muArchiveData == null ? 0 : municipalityData.Value.Count(robjectData => processArchiveData(robjectData, muArchiveData, workFinishedFact));
                    var inPlanLimits = muArchiveData == null ? 0 : municipalityData.Value.Count(robjectData => processArchiveData(robjectData, muArchiveData, workFinishedInPlanLimits));

                    return new ReportDataProxy
                    {
                        plan = plan,
                        total = total,
                        inPlanLimits = inPlanLimits
                    };
                });

            var sectionStarted = reportParams.ComplexReportParams.ДобавитьСекцию("section");
            var sectionFinished = reportParams.ComplexReportParams.ДобавитьСекцию("sectionF");
            var sectionTotalStarted = reportParams.ComplexReportParams.ДобавитьСекцию("sectionTotal");
            var sectionTotalFinished = reportParams.ComplexReportParams.ДобавитьСекцию("sectionTotalF");

            reportParams.SimpleReportParams["Programm"] = program.Name;
            reportParams.SimpleReportParams["Date"] = this.ReportDate.ToShortDateString();

            string codes = workType.Aggregate(string.Empty, (current, code) => current + GetWorkName(code) + ", ");
            reportParams.SimpleReportParams["Work"] = codes;

            this.FillSection(startedWorksByMuDict, reportParams, sectionStarted, sectionTotalStarted);
            this.FillSection(finishedWorksByMuDict, reportParams, sectionFinished, sectionTotalFinished);
        }

        public void FillSection(Dictionary<long, ReportDataProxy> municipalityData, ReportParams reportParams, Section section, Section sectionTotal)
        {
            var municipalityIdList = municipalityData
                .Where(x => x.Value.inPlanLimits > 0 || x.Value.total > 0 || x.Value.plan > 0)
                .Select(x => x.Key)
                .OrderBy(x => municipalityDict[x])
                .ToList();

            var halfCount = (municipalityIdList.Count / 2) + (municipalityIdList.Count % 2);

            Func<int, string> printableValue = x => x > 0 ? x.ToStr() : string.Empty;

            var totals = new int[4];

            for (var i = 0; i < halfCount; ++i)
            {
                var municipalityLeftData = municipalityData[municipalityIdList[i]];
                var municipalityRightData = new ReportDataProxy();

                section.ДобавитьСтроку();
                section["Num"] = i + 1;
                section["MO"] = municipalityDict[municipalityIdList[i]];

                var rightNumberer = i + halfCount;

                if (rightNumberer < municipalityIdList.Count)
                {
                    municipalityRightData = municipalityData[municipalityIdList[rightNumberer]];
                    section["Num2"] = rightNumberer + 1;
                    section["MO2"] = municipalityDict[municipalityIdList[rightNumberer]];
                }

                section["Plan"] = printableValue(municipalityLeftData.plan);
                section["FPredPlan"] = printableValue(municipalityLeftData.inPlanLimits);
                section["Vsego"] = printableValue(municipalityLeftData.total);
                section["FSverhPlan"] = printableValue(municipalityLeftData.total - municipalityLeftData.inPlanLimits);

                section["Plan2"] = printableValue(municipalityRightData.plan);
                section["FPredPlan2"] = printableValue(municipalityRightData.inPlanLimits);
                section["Vsego2"] = printableValue(municipalityRightData.total);
                section["FSverhPlan2"] = printableValue(municipalityRightData.total - municipalityRightData.inPlanLimits);

                totals[0] += municipalityLeftData.plan + municipalityRightData.plan;
                totals[1] += municipalityLeftData.inPlanLimits + municipalityRightData.inPlanLimits;
                totals[2] += (municipalityLeftData.total - municipalityLeftData.inPlanLimits) + (municipalityRightData.total - municipalityRightData.inPlanLimits);
                totals[3] += municipalityLeftData.total + municipalityRightData.total;
            }

            sectionTotal.ДобавитьСтроку();
            sectionTotal["IPlan"] = printableValue(totals[0]);
            sectionTotal["IFPredPlan"] = printableValue(totals[1]);
            sectionTotal["IFSverhPlan"] = printableValue(totals[2]);
            sectionTotal["IVsego"] = printableValue(totals[3]);
        }

        private static string GetWorkName(string code)
        {
            var result = string.Empty;

            switch (code)
            {
                case "24":
                result = "установке 2-х контурного котла";
                    break;
                case "25":
                    result = "замене подводящего газопровода";
                    break;
                case "26":
                    result = "устройству приточной вентиляции";
                    break;
                case "22":
                    result = "ремонту конструкций методом инъектирования";
                    break;
                case "23":
                    result = "монтажу и демонтажу балконов";
                    break;
                case "1020":
                    result = "технадзору";
                    break;
                case "18":
                    result = "усилению фундаментов";
                    break;
                case "19":
                    result = "устройству систем противопожарной автоматики и дымоудаления";
                    break;
                case "1019":
                    result = "ПСД экспертизе";
                    break;
                case "1018":
                    result = "ПСД разработке";
                    break;
                case "20":
                    result = "благоустройству дворовых территорий";
                    break;
                case "21":
                    result = "ремонту подъездов";
                    break;
                case "15":
                    result = "ремонту лифтовой шахты";
                    break;
                case "5":
                    result = "ремонту внутридомовой инж. системы газостабжения";
                    break;
                case "4":
                    result = "ремонту внутридомовой инж. системы водоотведения";
                    break;
                case "6":
                    result = "ремонту внутридомовой инж. системы электроснабжения";
                    break;
                case "7":
                    result = "установке приборов учета: тепла";
                    break;
                case "13":
                    result = "ремонту крыши";
                    break;
                case "16":
                    result = "ремонту фасада";
                    break;
                case "17":
                    result = "утеплению фасада";
                    break;
                case "2":
                    result = "ремонту внутридомовой инж. системы ГВС";
                    break;
                case "10":
                    result = "установке приборов учета: электроэнергии";
                    break;
                case "12":
                    result = "ремонту подвального помещения";
                    break;
                case "9":
                    result = "установке приборов учета: ХВС";
                    break;
                case "8":
                    result = "установке приборов учета: ГВС";
                    break;
                case "14":
                    result = "ремонту/замене лифтового оборудования";
                    break;
                case "1":
                    result = "ремонту внутридомовой инж. системы теплоснабжения";
                    break;
                case "3":
                    result = "ремонту внутридомовой инж. системы ХВС";
                    break;
                case "11":
                    result = "установке приборов учета: газа";
                    break;
                case "27":
                    result = "устройству естественной вытяжки";
                    break;
                case "28":
                    result = "замене внутриквартирной системы отопления";
                    break;
                case "999":
                    result = "общестроительным работам";
                    break;
                case "29":
                    result = "установке узлов регулирования";
                    break;
                case "30":
                    result = "энергообследованию";
                    break;
            }

            return result;
        }
    }

    internal sealed class ReportDataProxy
    {
        public int plan;

        public int total;

        public int inPlanLimits;
    }

    internal sealed class WorkReportProxy
    {
        public decimal? PercentOfCompletion;

        public DateTime? DateStartWork;

        public DateTime? DateEndWork;
    }
}
