using System.Collections.Generic;
using System.Linq;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Enums;
using Bars.GkhCr.Entities;
using Castle.Windsor;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.Entities;
using Bars.GkhRf.Entities;

namespace Bars.GkhRf.Report
{
    using B4.Modules.Reports;

    /// <summary>
    /// Отчет "Анализ программы капремонта"
    /// </summary>
    public class AnalisysProgramCr : BasePrintForm
    {
        private long programCrId;
        //private DateTime reportDate;
        private List<long> municipalityIds = new List<long>();
        //дополнительные программы
        private List<long> additionalProgrammCrIds = new List<long>();

        private readonly List<string> allRows = new List<string>();

        private readonly List<string> staticRows = new List<string>
            {
                "countMkd",
                "areaMkd",
                "countExceptAdditional",
                "areaExceptAdditional",
                "countMeterDevice",
                "areaMeterDevice",
                "countProgram",
                "areaProgram",
                "countUk",
                "countTsj",
                "count1Flat",
                "count2Flat",
                "count3Flat",
                "areaLiving",
                "areaLivingCits",
                "percentPrivatized",
                "countBefore1910",
                "countBefore1995",
                "countBefore2010",
                "countAfter2010",
                "count10Wear",
                "count20Wear",
                "count30Wear",
                "count40Wear",
                "count70Wear",
                "countMore70Wear",
                "countContractGisu",
                "percentContractGisu",
                "countEmergency",
                "percentEmergency"
            };

        //динамические строки
        private readonly List<string> dynamicRows = new List<string>
            {
                "countCapGroup",
                "countWall",
                "countRoof",
                "countFloor",
                "count1AddProgram",
                "count2AddProgram",
                "areaAddProgram",
                "percentAddProgram",
                "countWork",
                "averageCostWork"
            };

        private readonly List<string> averageRows = new List<string>();

        public IWindsorContainer Container { get; set; }

        public AnalisysProgramCr() : base(new ReportTemplateBinary(Properties.Resources.AnalisysProgramCr))
        {
        }

        #region Свойства

        public override string Name
        {
            get { return "Анализ программы капремонта"; }
        }

        public override string Desciption
        {
            get { return "Анализ программы капремонта"; }
        }

        public override string GroupName
        {
            get { return "Ход капремонта"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.AnalisysProgramCr"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.RF.AnalisysProgramCr"; }
        }

        #endregion Свойства

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params.ContainsKey("programCrId") ? baseParams.Params["programCrId"].ToLong() : 0L;
            //reportDate = baseParams.Params.ContainsKey("reportDate") ? baseParams.Params["reportDate"].ToDateTime() : DateTime.MinValue;

            var muIds = baseParams.Params.ContainsKey("municipalityIds") ? baseParams.Params["municipalityIds"].ToStr() : "";

            if (!string.IsNullOrEmpty(muIds))
            {
                municipalityIds = muIds.Split(',').Select(x => x.ToLong()).ToList();
            }

            var programIds = baseParams.Params.ContainsKey("additionalProgrammCrIds") ? baseParams.Params["additionalProgrammCrIds"].ToStr() : "";

            if (!string.IsNullOrEmpty(programIds))
            {
                additionalProgrammCrIds = programIds.Split(',').Select(x => x.ToLong()).ToList();
            }
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var dictMu = Container.Resolve<IRepository<Municipality>>().GetAll()
                .Where(x => municipalityIds.Contains(x.Id))
                .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Group
                    })
                .AsEnumerable()
                .GroupBy(x => x.Group.ToStr().Replace(" ", ""))
                .ToDictionary(x => x.Key, y => y.ToDictionary(x => x.Id.ToStr(), x => x.Name));

            //заполнение секций
            FillSections(reportParams, dictMu);

            //все жилые дома по МО
            var realityObjectsMo = Container.Resolve<IRepository<RealityObject>>().GetAll()
                .Where(x => municipalityIds.Contains(x.Municipality.Id))
                .Select(x => new
                    {
                        MuId = x.Municipality.Id,
                        x.AreaMkd
                    })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key.ToStr(), y => new { Count = y.Count(), SumArea = y.Sum(x => x.AreaMkd).ToDecimal() });

            var serviceObjectCr = Container.Resolve<IRepository<ObjectCr>>();

            //данные из жилых домов по главной программе
            var robjectInfoMainProgram = serviceObjectCr.GetAll()
                .Where(x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                    && x.ProgramCr.Id == programCrId)
                .Select(x => new
                    {
                        MuId = x.RealityObject.Municipality.Id,
                        RoId = x.RealityObject.Id,
                        CapGroupId = x.RealityObject.CapitalGroup.Id,
                        x.RealityObject.AreaMkd,
                        WallId = x.RealityObject.WallMaterial.Id,
                        RoofId = x.RealityObject.RoofingMaterial.Id,
                        x.RealityObject.MaximumFloors,
                        x.RealityObject.DateCommissioning,
                        x.RealityObject.PhysicalWear,
                        x.RealityObject.AreaLiving,
                        x.RealityObject.AreaLivingOwned,
                        x.RealityObject.NumberApartments
                    })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key.ToStr(), y => y.ToList());

            var dictEmergency = serviceObjectCr.GetAll()
                .Where(x => municipalityIds.Contains(x.RealityObject.Municipality.Id) && x.ProgramCr.Id == programCrId)
                .Where(y => Container.Resolve<IRepository<EmergencyObject>>().GetAll()
                    .Any(x => municipalityIds.Contains(x.RealityObject.Municipality.Id) 
                        && x.RealityObject.Id == y.RealityObject.Id))
                .Select(x => new
                {
                    MuId = x.RealityObject.Municipality.Id,
                    RoId = x.RealityObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key.ToStr(), x => x.Count());

            var dictGisu = serviceObjectCr.GetAll()
                .Where(x => municipalityIds.Contains(x.RealityObject.Municipality.Id) && x.ProgramCr.Id == programCrId)
                .Where(y => Container.Resolve<IRepository<ContractRfObject>>().GetAll()
                    .Any(x => municipalityIds.Contains(x.RealityObject.Municipality.Id) 
                        && x.RealityObject.Id == y.RealityObject.Id))
                .Select(x => new
                    {
                        MuId = x.RealityObject.Municipality.Id,
                        RoId = x.RealityObject.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key.ToStr(), x => x.Count());

            var dictTypeContract =
                Container.Resolve<IRepository<ManOrgContractRealityObject>>().GetAll()
                    .Where(x => x.ManOrgContract.ManagingOrganization != null)
                    .Where(y => Container.Resolve<IRepository<ObjectCr>>().GetAll()
                        .Any(x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                            && x.ProgramCr.Id == programCrId
                            && y.RealityObject.Id == x.RealityObject.Id))
                    .Select(x => new
                        {
                            MuId = x.RealityObject.Municipality.Id,
                            TypeOrg = x.ManOrgContract.ManagingOrganization.TypeManagement
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.MuId)
                    .ToDictionary(x => x.Key.ToStr(), y => new
                                    {
                                        CountUk = y.Count(x => x.TypeOrg == TypeManagementManOrg.UK),
                                        CountTsj = y.Count(x =>
                                                        x.TypeOrg == TypeManagementManOrg.TSJ 
                                                        || x.TypeOrg == TypeManagementManOrg.JSK)
                                    });

            // средняя стоимость по видам работ
            var dictTypeWorks = Container.Resolve<IRepository<TypeWorkCr>>().GetAll()
                .Where(x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id)
                    && x.ObjectCr.ProgramCr.Id == programCrId
                    && x.Work.TypeWork != TypeWork.Service)
                .Select(x => new
                {
                    MuId = x.ObjectCr.RealityObject.Municipality.Id,
                    x.Work.Code,
                    x.Sum,
                    x.Volume
                })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key.ToStr(), y => y.Select(x => new { x.Code, x.Sum, x.Volume })
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, f => f.Select(x => new { AvrCost = x.Volume > 0 ? x.Sum / x.Volume : null })));

            var dictAddProgram = Container.Resolve<IRepository<ObjectCr>>().GetAll()
                .Where(x => additionalProgrammCrIds.Contains(x.ProgramCr.Id)
                    && municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new
                    {
                        MuId = x.RealityObject.Municipality.Id,
                        ProgramId = x.ProgramCr.Id,
                        x.RealityObject.AreaMkd
                    })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key.ToStr(), y => y.GroupBy(x => x.ProgramId).ToDictionary(x => x.Key, x => x.Select(z => z.AreaMkd)));

            var robjectAreaMainProgram = serviceObjectCr.GetAll()
                .Where(x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                    && x.ProgramCr.Id == programCrId)
                .Select(x => new { MuId = x.RealityObject.Municipality.Id, RoId = x.RealityObject.Id, x.RealityObject.AreaMkd })
                .AsEnumerable()
                .Distinct(x => x.RoId)
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key.ToStr(), y => y.Select(x => new {x.RoId, x.AreaMkd}).AsEnumerable());

            var robjectIdsAddProgram = serviceObjectCr.GetAll()
                .Where(x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                    && additionalProgrammCrIds.Contains(x.ProgramCr.Id))
                .Select(x => x.RealityObject.Id)
                .Distinct()
                .ToList();

            var meterDeviceCodes = new List<string> {"7", "8", "9", "10", "11"};

            var dictMeterDevice = Container.Resolve<IRepository<ObjectCr>>().GetAll()
                .Where(x => x.ProgramCr.Id == programCrId && municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(y => Container.Resolve<IRepository<TypeWorkCr>>().GetAll()
                    .Where(x => x.ObjectCr.Id == y.Id)
                    .All(x => meterDeviceCodes.Contains(x.Work.Code)))
                .Select(x => new
                    {
                        MuId = x.RealityObject.Municipality.Id,
                        RoId = x.RealityObject.Id,
                        x.RealityObject.AreaMkd
                    })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key.ToStr(), y => new { Count = y.Distinct(x => x.RoId).Count(), SumArea = (decimal) y.Distinct(x => x.RoId).Sum(x => x.AreaMkd) });

            var dictAddProgramIntersect = serviceObjectCr.GetAll()
                .Where(x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                    && additionalProgrammCrIds.Contains(x.ProgramCr.Id))
                .Where(y => serviceObjectCr.GetAll()
                    .Any(x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                        && x.ProgramCr.Id == programCrId
                        && x.RealityObject.Id == y.RealityObject.Id))
                .Select(x => new
                    {
                        MuId = x.RealityObject.Municipality.Id,
                        ProgramId = x.ProgramCr.Id,
                        RoId = x.RealityObject.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key.ToStr(), y => y.Select(x => new { x.RoId, x.ProgramId })
                    .GroupBy(x => x.ProgramId)
                    .ToDictionary(x => x.Key, x => x.Count()));

            //заполнение отчета
            var dictAverageGrandTotals = averageRows.ToDictionary(x => x, x => 0);
            var dictGrandTotals = allRows.ToDictionary(x => x, x => 0m);
            foreach (var group in dictMu)
            {
                var dictGroupTotals = allRows.ToDictionary(x => x, x => 0m);
                var dictAverageGroupTotals = averageRows.ToDictionary(x => x, x => 0);

                foreach (var mu in group.Value)
                {
                    int countInMainProgram = 0;

                    var dictMuTotals = allRows.ToDictionary(x => x, x => 0m);
                    var dictAverageTotals = averageRows.ToDictionary(x => x, x => 0);

                    if (realityObjectsMo.ContainsKey(mu.Key))
                    {
                        dictMuTotals["countMkd"] = realityObjectsMo[mu.Key].Count;
                        dictMuTotals["areaMkd"] = realityObjectsMo[mu.Key].SumArea;
                    }

                    if (robjectInfoMainProgram.ContainsKey(mu.Key))
                    {
                        var currMu = robjectInfoMainProgram[mu.Key];

                        var dictByCapGroup = currMu.Select(x => x.CapGroupId).GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
                        foreach (var item in dictByCapGroup)
                            dictMuTotals[string.Format("countCapGroup_{0}", item.Key)] = item.Value;

                        var dictByWall = currMu.Select(x => x.WallId).GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
                        foreach (var item in dictByWall)
                            dictMuTotals[string.Format("countWall_{0}", item.Key)] = item.Value;

                        var dictByRoof = currMu.Select(x => x.RoofId).GroupBy(x => x).ToDictionary(x => x.Key, y => y.Count());
                        foreach (var item in dictByRoof)
                            dictMuTotals[string.Format("countRoof_{0}", item.Key)] = item.Value;

                        var dictByFloors = currMu.Select(x => x.MaximumFloors).GroupBy(x => x).ToDictionary(x => x.Key.ToStr(), y => y.Count());
                        foreach (var item in dictByFloors)
                            dictMuTotals[string.Format("countFloor_{0}", item.Key)] = item.Value;

                        dictMuTotals["countBefore1910"] = currMu.Count(x => x.DateCommissioning.HasValue && x.DateCommissioning.Value.Year < 1910);
                        dictMuTotals["countBefore1995"] = currMu.Count(x => x.DateCommissioning.HasValue && x.DateCommissioning.Value.Year >= 1910 && x.DateCommissioning.Value.Year < 1995);
                        dictMuTotals["countBefore2010"] = currMu.Count(x => x.DateCommissioning.HasValue && x.DateCommissioning.Value.Year >= 1995 && x.DateCommissioning.Value.Year < 2010);
                        dictMuTotals["countAfter2010"] = currMu.Count(x => x.DateCommissioning.HasValue && x.DateCommissioning.Value.Year >= 2010);

                        dictMuTotals["count10Wear"] = currMu.Count(x => x.PhysicalWear.HasValue && x.PhysicalWear < 10);
                        dictMuTotals["count20Wear"] = currMu.Count(x => x.PhysicalWear.HasValue && x.PhysicalWear >= 10 && x.PhysicalWear < 20);
                        dictMuTotals["count30Wear"] = currMu.Count(x => x.PhysicalWear.HasValue && x.PhysicalWear >= 20 && x.PhysicalWear < 30);
                        dictMuTotals["count40Wear"] = currMu.Count(x => x.PhysicalWear.HasValue && x.PhysicalWear >= 30 && x.PhysicalWear < 40);
                        dictMuTotals["count70Wear"] = currMu.Count(x => x.PhysicalWear.HasValue && x.PhysicalWear >= 40 && x.PhysicalWear < 70);
                        dictMuTotals["countMore70Wear"] = currMu.Count(x => x.PhysicalWear.HasValue && x.PhysicalWear >= 70);

                        dictMuTotals["count1Flat"] = currMu.Count(x => x.NumberApartments == 1);
                        dictMuTotals["count2Flat"] = currMu.Count(x => x.NumberApartments == 2);
                        dictMuTotals["count3Flat"] = currMu.Count(x => x.NumberApartments >= 3);

                        var areaMkd = currMu.Sum(x => x.AreaMkd).ToDecimal();
                        dictMuTotals["areaProgram"] = areaMkd;
                        dictMuTotals["countProgram"] = currMu.Count();
                        dictMuTotals["areaLiving"] = currMu.Sum(x => x.AreaLiving).ToDecimal();
                        dictMuTotals["areaLivingCits"] = currMu.Sum(x => x.AreaLivingOwned).ToDecimal();

                        countInMainProgram = currMu.Count();
                    }

                    if (dictEmergency.ContainsKey(mu.Key))
                    {
                        dictMuTotals["countEmergency"] = dictEmergency[mu.Key];
                        dictMuTotals["percentEmergency"] = countInMainProgram > 0 ? dictEmergency[mu.Key] * 100 / countInMainProgram : 0;
                    }

                    if (dictGisu.ContainsKey(mu.Key))
                    {
                        dictMuTotals["countContractGisu"] = dictGisu[mu.Key];
                        dictMuTotals["percentContractGisu"] = countInMainProgram > 0 ? dictGisu[mu.Key] * 100 / countInMainProgram : 0;
                    }

                    if (dictTypeContract.ContainsKey(mu.Key))
                    {
                        dictMuTotals["countUk"] = dictTypeContract[mu.Key].CountUk;
                        dictMuTotals["countTsj"] = dictTypeContract[mu.Key].CountTsj;
                    }

                    if (dictTypeWorks.ContainsKey(mu.Key))
                    {
                        foreach (var item in dictTypeWorks[mu.Key])
                        {
                            var workCode = "countWork_" + item.Key;
                            var workAvr = "averageCostWork_" + item.Key;

                            int count = 0;

                            if (dictMuTotals.ContainsKey(workCode))
                            {
                                count = dictTypeWorks[mu.Key][item.Key].Count(x => x.AvrCost > 0);
                                dictMuTotals[workCode] = dictTypeWorks[mu.Key][item.Key].Count();
                            }

                            if (dictMuTotals.ContainsKey(workAvr))
                            {
                                var avr = count > 0 ? dictTypeWorks[mu.Key][item.Key].Sum(x => x.AvrCost).ToDecimal() / count : 0m;

                                dictMuTotals[workAvr] = avr;
                                dictAverageTotals[workAvr] += avr > 0 ? 1 : 0;
                            }
                        }
                    }

                    if (dictAddProgram.ContainsKey(mu.Key))
                    {
                        foreach (var program in dictAddProgram[mu.Key])
                        {
                            dictMuTotals["count1AddProgram_" + program.Key] = program.Value.Count();
                            dictMuTotals["areaAddProgram_" + program.Key] = program.Value.Sum().ToDecimal();
                        }
                    }

                    if (dictAddProgramIntersect.ContainsKey(mu.Key))
                    {
                        foreach (var program in dictAddProgramIntersect[mu.Key])
                        {
                            dictMuTotals["count2AddProgram_" + program.Key] = program.Value;
                            dictMuTotals["percentAddProgram_" + program.Key] = countInMainProgram > 0 ? ((decimal)program.Value * 100 / countInMainProgram).RoundDecimal(2) : 0;
                        }
                    }

                    if (dictMeterDevice.ContainsKey(mu.Key))
                    {
                        dictMuTotals["countMeterDevice"] = dictMeterDevice[mu.Key].Count;
                        dictMuTotals["areaMeterDevice"] = dictMeterDevice[mu.Key].SumArea;
                    }

                    if (robjectAreaMainProgram.ContainsKey(mu.Key))
                    {
                        int count = 0;
                        decimal area = 0m;

                        foreach (var item in robjectAreaMainProgram[mu.Key])
                        {
                            if (!robjectIdsAddProgram.Contains(item.RoId))
                            {
                                count++;
                                area += item.AreaMkd.ToDecimal();
                            }
                        }

                        dictMuTotals["countExceptAdditional"] = count;
                        dictMuTotals["areaExceptAdditional"] = area;
                    }

                    foreach (var item in dictMuTotals)
                    {
                        dictGroupTotals[item.Key] += item.Value;
                        reportParams.SimpleReportParams[string.Format("{0}_{1}", item.Key, mu.Key)] = item.Value.RoundDecimal(2);
                    }

                    foreach (var item in dictAverageTotals)
                    {
                        dictAverageGroupTotals[item.Key] += item.Value;
                    }
                }

                foreach (var item in dictAverageGroupTotals)
                {
                    dictAverageGrandTotals[item.Key] += item.Value;
                }

                foreach (var item in dictGroupTotals)
                {
                    dictGrandTotals[item.Key] += item.Value;
                    if (!string.IsNullOrEmpty(group.Key))
                    {
                        var format = string.Format("{0}_{1}", item.Key, group.Key);
                        if (item.Key.Contains("average"))
                            reportParams.SimpleReportParams[format] = dictAverageGroupTotals[item.Key] > 0 ? (item.Value / dictAverageGroupTotals[item.Key]).RoundDecimal(2) : 0;
                        else
                            reportParams.SimpleReportParams[format] = item.Value.RoundDecimal(2);
                    }
                }
            }

            foreach (var item in dictGrandTotals)
            {
                var format = item.Key + "Total";

                if(item.Key.Contains("average"))
                    reportParams.SimpleReportParams[format] = dictAverageGrandTotals[item.Key] > 0 ? (item.Value / dictAverageGrandTotals[item.Key]).RoundDecimal(2) : 0;
                else
                    reportParams.SimpleReportParams[format] = item.Value.RoundDecimal(2);
            }
        }

        private void FillSections(ReportParams reportParams, Dictionary<string, Dictionary<string,string>> dictMu)
        {
            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("verticalSection");

            allRows.AddRange(staticRows);

            //заполнение вертикальных секций
            foreach (var group in dictMu.OrderBy(x => x.Key))
            {
                foreach (var mu in group.Value.OrderBy(x => x.Value))
                {
                    verticalSection.ДобавитьСтроку();
                    verticalSection["groupMuName"] = group.Key;
                    verticalSection["muName"] = mu.Value;

                    foreach (var row in staticRows)
                        verticalSection[row] = string.Format("${0}_{1}$", row, mu.Key);

                    foreach (var row in dynamicRows)
                        verticalSection[row] = string.Format("${0}_{1}$", row, mu.Key);
                }

                //итоги по группе
                if (!string.IsNullOrEmpty(group.Key))
                {
                    verticalSection.ДобавитьСтроку();
                    verticalSection["name"] = "Итого по группе";
                    verticalSection["groupMuName"] = group.Key;

                    foreach (var row in staticRows)
                        verticalSection[row] = string.Format("${0}_{1}$", row, group.Key);

                    foreach (var row in dynamicRows)
                        verticalSection[row] = string.Format("${0}_{1}$", row, group.Key);
                }
            }

            var sectionAddProgram = reportParams.ComplexReportParams.ДобавитьСекцию("sectionAddProgram");
            var sectionAddProgramIntersect = reportParams.ComplexReportParams.ДобавитьСекцию("sectionAddProgramIntersect");
            var sectionCapGroup = reportParams.ComplexReportParams.ДобавитьСекцию("sectionCapGroup");
            var sectionWall = reportParams.ComplexReportParams.ДобавитьСекцию("sectionWall");
            var sectionRoof = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRoof");
            var sectionFloor = reportParams.ComplexReportParams.ДобавитьСекцию("sectionFloor");
            var sectionCountWork = reportParams.ComplexReportParams.ДобавитьСекцию("sectionCountWork");
            var sectionAvrCostWork = reportParams.ComplexReportParams.ДобавитьСекцию("sectionAvrCostWork");

            var dictAddProgram =
                Container.Resolve<IRepository<ProgramCr>>().GetAll()
                    .Where(x => additionalProgrammCrIds.Contains(x.Id))
                    .Select(x => new { x.Id, x.Name })
                    .AsEnumerable()
                    .ToDictionary(x => x.Id.ToStr(), x => x.Name);

            var dictCapitalGroup = Container.Resolve<IRepository<CapitalGroup>>().GetAll()
                .Where(y => Container.Resolve<IRepository<ObjectCr>>().GetAll()
                    .Any(x => municipalityIds.Contains(x.RealityObject.Municipality.Id) 
                        && x.ProgramCr.Id == programCrId 
                        && x.RealityObject.CapitalGroup.Id == y.Id))
                .ToDictionary(x => x.Id.ToStr(), x => x.Name);

            var dictWall = Container.Resolve<IRepository<WallMaterial>>().GetAll()
                .Where(y => Container.Resolve<IRepository<ObjectCr>>().GetAll()
                    .Any(x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                        && x.ProgramCr.Id == programCrId
                        && x.RealityObject.WallMaterial.Id == y.Id))
                .ToDictionary(x => x.Id.ToStr(), x => x.Name);

            var dictRoof = Container.Resolve<IRepository<RoofingMaterial>>().GetAll()
                .Where(y => Container.Resolve<IRepository<ObjectCr>>().GetAll()
                    .Any(x => municipalityIds.Contains(x.RealityObject.Municipality.Id)
                        && x.ProgramCr.Id == programCrId
                        && x.RealityObject.RoofingMaterial.Id == y.Id))
                .ToDictionary(x => x.Id.ToStr(), x => x.Name);

            var dictFloor =
                Container.Resolve<IRepository<ObjectCr>>().GetAll()
                    .Where(x => municipalityIds.Contains(x.RealityObject.Municipality.Id) 
                        && x.ProgramCr.Id == programCrId)
                    .Select(x => x.RealityObject.MaximumFloors)
                    .Distinct()
                    .AsEnumerable()
                    .OrderBy(x => x.ToDecimal())
                    .Select(x => x > 0 ? x.ToStr() : "")
                    .ToDictionary(x => x);

            var dictWorks = Container.Resolve<IRepository<Work>>().GetAll()
                .Where(y => Container.Resolve<IRepository<TypeWorkCr>>().GetAll()
                    .Any(x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id) 
                        && x.ObjectCr.ProgramCr.Id == programCrId
                        && x.Work.TypeWork != TypeWork.Service
                        && x.Work.Id == y.Id))
                .ToDictionary(x => x.Code, y => y.Name);

            //заполнение горизонтальных секций
            //count1 - общее количество домов в доп программе,
            //count2 - количество домов, которые есть в доп программе и основной программе
            FillDynamicRows(sectionAddProgram, dictAddProgram, new[] { "count1AddProgram", "areaAddProgram" }, dictMu);
            FillDynamicRows(sectionCapGroup, dictCapitalGroup, new[] { "countCapGroup" }, dictMu);
            FillDynamicRows(sectionWall, dictWall, new[] { "countWall" }, dictMu);
            FillDynamicRows(sectionRoof, dictRoof, new[] { "countRoof" }, dictMu);
            FillDynamicRows(sectionFloor, dictFloor, new[] { "countFloor" }, dictMu);
            FillDynamicRows(sectionCountWork, dictWorks, new[] { "countWork" }, dictMu);
            FillDynamicRows(sectionAvrCostWork, dictWorks, new[] { "averageCostWork" }, dictMu);
            FillDynamicRows(sectionAddProgramIntersect, dictAddProgram, new[] { "count2AddProgram", "percentAddProgram" }, dictMu);
        }

        private void FillDynamicRows(Section section, Dictionary<string, string> values, string[] rowNames, Dictionary<string, Dictionary<string, string>> dictMu)
        {
            foreach (var value in values.OrderBy(x => x.Value))
            {
                section.ДобавитьСтроку();

                if(!string.IsNullOrEmpty(value.Value))
                    section["Name"] = value.Value;
                else
                    section["Name"] = "Не задано";

                foreach (var rowName in rowNames)
                {
                    var row = string.Format("{0}_{1}", rowName, value.Key);

                    if (rowName.Contains("average"))
                        averageRows.Add(row);

                    allRows.Add(row);
                    section[rowName + "Total"] = string.Format("${0}Total$", row);

                    foreach (var group in dictMu)
                    {
                        foreach (var mu in group.Value.OrderBy(x => x.Value))
                            section[string.Format("{0}_{1}", rowName, mu.Key)] = string.Format("${0}_{1}_{2}$", rowName, value.Key, mu.Key);

                        if (!string.IsNullOrEmpty(group.Key))
                            section[string.Format("{0}_{1}", rowName, group.Key)] = string.Format("${0}_{1}_{2}$", rowName, value.Key, group.Key);
                    }
                }
            }
        }
    }
}