using System;
using System.Collections.Generic;
using System.Linq;

namespace Bars.GkhCr.Report
{
    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    class DataDevicesStatementReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private DateTime ReportDate = DateTime.MinValue;

        private long programCrId;

        private List<long> municipalityListId;

        private List<long> finSourceListId;

        private Dictionary<long, string> finSourceDict;

        public DataDevicesStatementReport()
            : base(new ReportTemplateBinary(Properties.Resources.DataDevicesStatementReport))
        {
        }

        public override string Name
        {
            get
            {
                return "Сведения по приборам Учета";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Сведения по приборам Учета";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.DataDevicesStatementReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.DataDevicesStatementReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToLong();
            var municipalityIdsString = baseParams.Params["municipalityIds"].ToString();
            this.municipalityListId = string.IsNullOrEmpty(municipalityIdsString)
                                     ? new List<long>()
                                     : municipalityIdsString.Split(',').Select(x => x.ToLong()).ToList();
            var finSourceIdsString = baseParams.Params["finSources"].ToString();
            this.finSourceListId = string.IsNullOrEmpty(finSourceIdsString)
                                  ? new List<long>()
                                  : finSourceIdsString.Split(',').Select(x => x.ToLong()).ToList();
            this.ReportDate = baseParams.Params["reportDate"].ToDateTime();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceArchiveSmr = this.Container.Resolve<IDomainService<ArchiveSmr>>();
            var serviceTypeWork = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();
            var servuceFinSource = this.Container.Resolve<IDomainService<FinanceSource>>();

            finSourceDict = servuceFinSource.GetAll().Select(x => new { x.Id, x.Name }).ToDictionary(x => x.Id, x => x.Name);
            
            var muList = serviceMunicipality.GetAll()
                .Select(x => new { x.Id, x.Name, Group = x.Group ?? string.Empty })
                .OrderBy(x => x.Group ?? x.Name)
                .ThenBy(x => x.Name)
                .ToList();

            var municipalityDict = muList.ToDictionary(x => x.Id, x => new { x.Name, x.Group });

            var alphabeticalGroups = new List<List<long>>();

            var lastGroup = "extraordinaryString";

            foreach (var municipality in muList)
            {
                if (municipality.Group != lastGroup)
                {
                    lastGroup = municipality.Group;
                    alphabeticalGroups.Add(new List<long>());
                }

                if (alphabeticalGroups.Any())
                {
                    alphabeticalGroups.Last().Add(municipality.Id);
                }
            }

            var program = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programCrId);
            var programYear = program != null ? program.Period.DateStart.Year : 0;
            var codeList = new List<string>() { "7", "8", "9", "10" };

            var typeWorks = serviceTypeWork
                            .GetAll()
                            .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId && x.Work.TypeWork == TypeWork.Work)
                            .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                            .WhereIf(this.finSourceListId.Count > 0, x => this.finSourceListId.Contains(x.FinanceSource.Id))
                            .Where(x => codeList.Contains(x.Work.Code))
                            .Select(x => new
                            {
                                muId = x.ObjectCr.RealityObject.Municipality.Id,
                                realObjId = x.ObjectCr.RealityObject.Id,
                                typeWorkId = x.Id,
                                percentOfCompletion = x.PercentOfCompletion,
                                dateStart = x.DateStartWork,
                                dateEnd = x.DateEndWork,
                                address = x.ObjectCr.RealityObject.Address,
                                x.Id,
                                x.Volume,
                                x.Sum,
                                x.Work.Code,
                                finSourceId = x.FinanceSource.Id
                            })
                            .AsEnumerable()
                            .GroupBy(x => x.muId)
                            .ToDictionary(x => x.Key,
                                          x => x.GroupBy(y => y.realObjId)
                                                .ToDictionary(y => y.Key, 
                                                y => y.GroupBy(z => z.finSourceId)
                                                .ToDictionary(z => z.Key, z => z.ToList())));

            var archiveRecords = serviceArchiveSmr
                            .GetAll()
                            .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == this.programCrId && x.TypeWorkCr.Work.TypeWork == TypeWork.Work)
                            .WhereIf(this.municipalityListId.Count > 0, x => this.municipalityListId.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                            .WhereIf(this.finSourceListId.Count > 0, x => this.finSourceListId.Contains(x.TypeWorkCr.FinanceSource.Id))
                            .Where(x => x.DateChangeRec <= this.ReportDate)
                            .Where(x => codeList.Contains(x.TypeWorkCr.Work.Code))
                            .Select(x => new
                            {
                                x.Id,
                                x.DateChangeRec,
                                typeWorkId = x.TypeWorkCr.Id,
                                percentOfCompletion = x.PercentOfCompletion
                            })
                            .AsEnumerable()
                            .GroupBy(x => x.typeWorkId)
                            .ToDictionary(x => x.Key, x =>
                            {
                                var archiveRec = x.OrderByDescending(p => p.DateChangeRec)
                                                  .ThenByDescending(p => p.Id)
                                                  .FirstOrDefault();
                                return (archiveRec != null)
                                           ? archiveRec.percentOfCompletion
                                           : 0;
                            });

            var resultDict = typeWorks.ToDictionary(
                x => x.Key,
                x => x.Value.ToDictionary(
                    y => y.Key,
                    y => y.Value.ToDictionary(
                        z => z.Key,
                        z =>
                            {
                                return z.Value.GroupBy(j => j.Code).ToDictionary(
                                    j => j.Key,
                                    j =>
                                        {
                                            var typeWork = j.FirstOrDefault();
                                            var dateStart = typeWork.dateStart.ToDateTime();
                                            var dateEnd = typeWork.dateEnd.ToDateTime();

                                            var percentGraphic = dateStart != DateTime.MinValue
                                                                 && dateEnd != DateTime.MinValue && dateStart != dateEnd
                                                                 && dateStart.Date < ReportDate.Date
                                                                     ? ((ReportDate.Date - dateStart.Date).TotalDays
                                                                        / ((dateEnd.Date - dateStart.Date).TotalDays + 1))
                                                                           .ToDecimal()
                                                                     : 0m;

                                            percentGraphic = percentGraphic > 1 ? 1 : percentGraphic;

                                            var archRec = programYear >= 2013
                                                              ? (archiveRecords.ContainsKey(typeWork.typeWorkId)
                                                                     ? archiveRecords[typeWork.typeWorkId]
                                                                     : null)
                                                              : typeWork.percentOfCompletion;

                                            var percentFact = archRec.HasValue ? archRec.Value / 100 : 0;

                                            percentFact = percentFact > 1 ? 1 : percentFact;

                                            var percentLagg = percentFact == 1 || percentFact > percentGraphic
                                                                  ? 0
                                                                  : ((percentGraphic - percentFact) * 100).RoundDecimal(
                                                                      2);
                                            return
                                                new
                                                    {
                                                        typeWork.finSourceId,
                                                        typeWork.address,
                                                        typeWork.Code,
                                                        typeWork.Sum,
                                                        typeWork.Volume,
                                                        archRec,
                                                        percentLagg
                                                    };
                                        }).ToList();
                            })));

            var sectionGroup = reportParams.ComplexReportParams.ДобавитьСекцию("sectionGroup");
            var sectionTotals = reportParams.ComplexReportParams.ДобавитьСекцию("sectionTotals");
            var sectionGroupName = sectionGroup.ДобавитьСекцию("sectionGroupName");
            var sectionGroupTotals = sectionGroup.ДобавитьСекцию("sectionGroupTotals");
            var sectionMu = sectionGroup.ДобавитьСекцию("sectionMu");
            var sectionRo = sectionMu.ДобавитьСекцию("sectionRo");
            var sectionMuTotals = sectionMu.ДобавитьСекцию("sectionMuTotals");

            var totals = this.finSourceListId.ToDictionary(x => x, x => codeList.ToDictionary(y => y, y => new TotalsValuesDevicesStatement()));

            var counter = 1;
            foreach (var group in alphabeticalGroups)
            {
                if (!resultDict.Keys.Intersect(group).Any())
                {
                    continue;
                }

                var inGroup = false;
                sectionGroup.ДобавитьСтроку();
                var groupName = municipalityDict[group.First()].Group;
                if (groupName != string.Empty)
                {
                    sectionGroupName.ДобавитьСтроку();
                    sectionGroupName["GroupName"] = groupName;
                    inGroup = true;
                }

                var groupTotals = this.finSourceListId.ToDictionary(x => x, x => codeList.ToDictionary(y => y, y => new TotalsValuesDevicesStatement()));

                foreach (var municipalityId in group)
                {
                    if (!resultDict.ContainsKey(municipalityId))
                    {
                        continue;
                    }

                    sectionMu.ДобавитьСтроку();
                    sectionMu["MuName"] = municipalityDict[municipalityId].Name;
                    var resultDictByRo = resultDict[municipalityId];

                    var finSourceIdsByMu = resultDictByRo.Values.SelectMany(x => x.Keys).Distinct().ToList();

                    foreach (var roDict in resultDictByRo)
                    {
                        foreach (var resultDictByTypeWork in roDict.Value)
                        {
                            sectionRo.ДобавитьСтроку();
                            sectionRo["Num"] = counter++;
                            sectionRo["MON"] = municipalityDict[municipalityId].Name;
                            sectionRo["Address"] = resultDictByTypeWork.Value.Select(x => x.Value.address).FirstOrDefault();
                            sectionRo["FinanceSource"] = this.finSourceDict[resultDictByTypeWork.Key];

                            foreach (var workData in resultDictByTypeWork.Value)
                            {
                                var code = workData.Key;
                                sectionRo[string.Format("WorkCount{0}", code)] = workData.Value.Volume;
                                sectionRo[string.Format("WorkSum{0}", code)] = workData.Value.Sum;
                                sectionRo[string.Format("Proc{0}", code)] = workData.Value.archRec; 
                                sectionRo[string.Format("ProcOts{0}", code)] = workData.Value.percentLagg;

                                // заполнение словарей с итогами всего и по группе
                                groupTotals[resultDictByTypeWork.Key][workData.Key].countRo += 1;
                                totals[resultDictByTypeWork.Key][workData.Key].countRo += 1;
                                groupTotals[resultDictByTypeWork.Key][workData.Key].sum += workData.Value.Sum.ToDecimal();
                                totals[resultDictByTypeWork.Key][workData.Key].sum  += workData.Value.Sum.ToDecimal();
                                groupTotals[resultDictByTypeWork.Key][workData.Key].volume += workData.Value.Volume.ToDecimal();
                                totals[resultDictByTypeWork.Key][workData.Key].volume += workData.Value.Volume.ToDecimal();
                                groupTotals[resultDictByTypeWork.Key][workData.Key].percent += workData.Value.archRec.ToDecimal();
                                totals[resultDictByTypeWork.Key][workData.Key].percent += workData.Value.archRec.ToDecimal();
                                groupTotals[resultDictByTypeWork.Key][workData.Key].percentLagg += workData.Value.percentLagg;
                                totals[resultDictByTypeWork.Key][workData.Key].percentLagg += workData.Value.percentLagg;
                            }
                        }
                    }

                    // заполнение итогов по мун.образованию
                    foreach (var finSourceId in finSourceIdsByMu)
                    {
                        sectionMuTotals.ДобавитьСтроку();
                        sectionMuTotals["MuName"] = municipalityDict[municipalityId].Name;
                        sectionMuTotals["MuFinanceSource"] = this.finSourceDict[finSourceId];

                        var worksByFinSource =
                            resultDictByRo.Where(x => x.Value.Keys.Contains(finSourceId))
                                          .SelectMany(x => x.Value.Values.SelectMany(y => y))
                                          .GroupBy(x => x.Key)
                                          .ToDictionary(
                                              x => x.Key,
                                              x => new TotalsValuesDevicesStatement
                                                {
                                                    sum = x.Sum(y => y.Value.Sum).ToDecimal(),
                                                    volume = x.Sum(y => y.Value.Volume).ToDecimal(),
                                                    percent = x.Sum(y => y.Value.archRec).ToDecimal() / x.Count(),
                                                    percentLagg = x.Sum(y => y.Value.percentLagg) / x.Count(),
                                                    countRo = resultDictByRo.Where(p => p.Value.Keys.Contains(finSourceId)).Count(p => p.Value.Values.Any(y => y.Any(z => z.Key == x.Key)))
                                                });

                        this.FillDataCells(sectionMuTotals, worksByFinSource, "Mu");
                    }

                    sectionMuTotals.ДобавитьСтроку();
                    sectionMuTotals["MuName"] = municipalityDict[municipalityId].Name + " Всего";
                    sectionMuTotals["MuRoCount"] = "Всего домов: " + resultDictByRo.Count();

                    var works = resultDictByRo.SelectMany(x => x.Value.Values.SelectMany(y => y))
                                          .GroupBy(x => x.Key)
                                          .ToDictionary(
                                              x => x.Key,
                                              x => new TotalsValuesDevicesStatement
                                              {
                                                  sum = x.Sum(y => y.Value.Sum).ToDecimal(),
                                                  volume = x.Sum(y => y.Value.Volume).ToDecimal(),
                                                  percent = x.Sum(y => y.Value.archRec).ToDecimal() / x.Count(),
                                                  percentLagg = x.Sum(y => y.Value.percentLagg) / x.Count(),
                                                  countRo = resultDictByRo.Count(p => p.Value.Values.Any(y => y.Any(z => z.Key == x.Key)))
                                              });

                    this.FillDataCells(sectionMuTotals, works, "Mu");
                }

                // заполнение итогов по группе
                if (inGroup)
                {
                    var countRo = resultDict.Where(x => group.Contains(x.Key)).SelectMany(x => x.Value).Count();
                    this.FillTotalCells(sectionGroupTotals, groupTotals, countRo, "Gr", groupName);
                }
            }

            // заполнение итогов по всем мун. образованиям
            var allRoCount = resultDict.SelectMany(x => x.Value).Count();
            this.FillTotalCells(sectionTotals, totals, allRoCount, "Total");
        }

        private void FillTotalCells(Section section, Dictionary<long, Dictionary<string, TotalsValuesDevicesStatement>> totals, int roCount, string prefix, string groupName = "")
        {
            foreach (var total in totals)
            {
                if (total.Value.Values.Any(x => !x.isEmpty()))
                {
                    section.ДобавитьСтроку();
                    section["GroupName"] = groupName;
                    section[string.Format("{0}FinanceSource", prefix)] = this.finSourceDict[total.Key];

                    var tmp = total.Value.ToDictionary(
                        x => x.Key,
                        x =>
                        {
                            if (x.Value.countRo > 0)
                            {
                                return new TotalsValuesDevicesStatement()
                                {
                                    sum = x.Value.sum,
                                    volume = x.Value.volume,
                                    countRo = x.Value.countRo,
                                    percent =
                                        x.Value.percent
                                        / x.Value.countRo,
                                    percentLagg =
                                        x.Value.percentLagg
                                        / x.Value.countRo
                                };
                            }
                            else
                            {
                                return new TotalsValuesDevicesStatement();
                            }
                        });

                    this.FillDataCells(section, tmp, prefix);
                }
            }

            section.ДобавитьСтроку();
            section["GroupName"] = groupName + " Всего";
            section[string.Format("{0}RoCount", prefix)] = "Всего домов: " + roCount;
            var allTotals = totals
                .SelectMany(x => x.Value)
                .ToList().GroupBy(x => x.Key)
                .ToDictionary(
                x => x.Key, x =>
                {
                    if (x.Sum(y => y.Value.countRo) > 0)
                    {
                        return new TotalsValuesDevicesStatement()
                        {
                            countRo = x.Sum(y => y.Value.countRo),
                            volume = x.Sum(y => y.Value.volume),
                            sum = x.Sum(y => y.Value.sum),
                            percent =
                                x.Sum(y => y.Value.percent)
                                / x.Sum(y => y.Value.countRo),
                            percentLagg =
                                x.Sum(y => y.Value.percentLagg)
                                / x.Sum(y => y.Value.countRo)
                        };
                    }
                    else
                    {
                        return new TotalsValuesDevicesStatement();
                    }
                });

            this.FillDataCells(section, allTotals, prefix);
        }

        private void FillDataCells(Section section, Dictionary<string, TotalsValuesDevicesStatement> works, string prefix)
        {
            foreach (var workTotalByMu in works)
            {
                section[string.Format("{0}HouseCount{1}", prefix, workTotalByMu.Key)] = workTotalByMu.Value.countRo;
                section[string.Format("{0}WorkCount{1}", prefix, workTotalByMu.Key)] = workTotalByMu.Value.volume;
                section[string.Format("{0}WorkSum{1}", prefix, workTotalByMu.Key)] = workTotalByMu.Value.sum;
                section[string.Format("{0}Proc{1}", prefix, workTotalByMu.Key)] = workTotalByMu.Value.percent;
                section[string.Format("{0}ProcOts{1}", prefix, workTotalByMu.Key)] = workTotalByMu.Value.percentLagg;
            }
        }
    }

    internal class TotalsValuesDevicesStatement
    {
        public decimal sum;

        public decimal volume;

        public decimal percent;

        public decimal percentLagg;

        public int countRo;

        public bool isEmpty()
        {
            if (sum > 0 || volume > 0 || percent > 0 || percentLagg > 0 || countRo > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
