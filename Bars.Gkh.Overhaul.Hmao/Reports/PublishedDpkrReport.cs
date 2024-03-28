namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;    

    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Domain;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Overhaul.DomainService;
    using Overhaul.Entities;

    public class PublishedDpkrReport : BasePrintForm
    {

        public PublishedDpkrReport()
            : base(new ReportTemplateBinary(Properties.Resources.PublishedDpkr))
        {

        }

        public IWindsorContainer Container { get; set; }

        public IVersionDateCalcService VersionDateCalcService { get; set; }

        public override string Name
        {
            get
            {
                return "Отчет по опубликованию ДПКР";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по опубликованию ДПКР";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Долгосрочная программа";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.PublishedDpkr";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.PublishedDpkr";
            }
        }

        private List<long> municipalityIds;

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;

            var mainQuery = Container.Resolve<IDomainService<PublishedProgramRecord>>().GetAll()
                .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                .Where(x => x.PublishedProgram.ProgramVersion.Municipality != null)
                .Where(x => x.Stage2 != null)
                .WhereIf(municipalityIds.Count > 0,
                    x => municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                .Select(x => new
                {
                    x.Id,
                    CommonEstateObjects = x.Stage2.CommonEstateObject.Name,
                    MuName = x.Stage2.Stage3Version.RealityObject.Municipality.Name,
                    x.Stage2.Stage3Version.RealityObject.Address,
                    x.Stage2.Stage3Version.StoredCriteria,
                    x.Stage2.Stage3Version.RealityObject.DateLastOverhaul,
                    x.PublishedYear,
                    Stage2Id = x.Stage2.Id,
                    roId = x.Stage2.Stage3Version.RealityObject.Id,
                    x.Sum
                });

            var data = mainQuery
                .OrderBy(x => x.MuName)
                .ThenBy(x => x.Address)
                .ThenBy(x => x.PublishedYear)
                .AsEnumerable()
                .GroupBy(x => new {x.roId, x.PublishedYear})
                .ToDictionary(x => x.Key, y => y.ToList());

            var st1Data = Container.Resolve<IDomainService<VersionRecordStage1>>().GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Municipality != null)
                .WhereIf(municipalityIds.Count > 0,
                    x => municipalityIds.Contains(x.Stage2Version.Stage3Version.ProgramVersion.Municipality.Id))
                .Select(x => new
                {
                    Stage2Id = x.Stage2Version.Id,
                    StructId = x.StructuralElement.StructuralElement.Id
                })
                .ToList();


            var appParams = Container.Resolve<IGkhParams>().GetParams();

            var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;


            var dictRo =
                Container.Resolve<IDomainService<RealityObject>>()
                    .GetAll()
                    .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Municipality.Id) || municipalityIds.Contains(x.MoSettlement.Id))
                    .Select(x => new
                    {
                        muName = moLevel == MoLevel.MunicipalUnion ? x.Municipality.Name : x.MoSettlement.Name,
                        moName = x.MoSettlement.Name,
                        roId = x.Id,
                        x.Address,
                        x.DateCommissioning
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.roId)
                    .ToDictionary(x => x.Key, v => v.FirstOrDefault());

            //словарь идентификатор оои - строка наименований работ
            var listWorks =
                Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
                    .Select(x => new
                    {
                        structId = x.StructuralElement.Id,
                        WorkName = x.Job.Name
                    })
                    .ToList();

            var minYear = config.ProgrammPeriodStart;
            var maxYear = config.ProgrammPeriodEnd;

            var dateCalcPublished = VersionDateCalcService.GetDateCalcPublished(municipalityIds);

            reportParams.SimpleReportParams["dateCalcPublished"] = dateCalcPublished.ToString("dd.MM.yyyy г. HH:mm");
            reportParams.SimpleReportParams["period"] = string.Format("{0}-{1}", minYear, maxYear);

            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
            var i = 1;

            // Получаем словарь по St2Id для быстрого доступа к списку StructId
            var dictSt2 = st1Data.GroupBy(x => x.Stage2Id).ToDictionary(x => x.Key, y => y.Select(x => x.StructId).ToList());

            // Формируем словарь по Структурному элементу соответствия его работе
            var dictWorks = listWorks.GroupBy(x => x.structId)
                                     .ToDictionary(x => x.Key, y => y.Select(x => x.WorkName).Distinct().ToList());

            var dictSt2Works = new Dictionary<long, List<string>>();

            // Заранее подготавливаем словарь по St2Id список Работ
            foreach (var kvp in dictSt2)
            {
                var works = new List<string>();
                foreach (var structId in kvp.Value)
                {
                    if (dictWorks.ContainsKey(structId))
                    {
                        works.AddRange(dictWorks[structId]);
                    }
                }

                dictSt2Works.Add(kvp.Key, works);
            }

            foreach (var rec in data)
            {
                if (groupByRoPeriod == 0)
                {
                    foreach (var publProgRec in rec.Value)
                    {
                        // Здесь работы получаем только для 2 этапа 
                        var works = new List<string>();
                        if (dictSt2Works.ContainsKey(publProgRec.Stage2Id))
                        {
                            works.AddRange(dictSt2Works[publProgRec.Stage2Id]);
                        }
                        var workNames = works.Any() ? works.Distinct().AggregateWithSeparator(", ") : string.Empty;

                        sect.ДобавитьСтроку();

                        sect["row"] = i++;
                        sect["Mu"] = dictRo[rec.Key.roId].muName;
                        sect["Mo"] = dictRo[rec.Key.roId].moName;
                        sect["Address"] = dictRo[rec.Key.roId].Address;
                        sect["StartDate"] = dictRo[rec.Key.roId].DateCommissioning;

                        sect["OOI"] = publProgRec.CommonEstateObjects;

                        sect["TypeWork"] = workNames;
                        sect["Year"] = rec.Key.PublishedYear;
                        var lastOverhaulYear = publProgRec.StoredCriteria.Any(x => x.Criterion == "LastOverhaulYearParam")
                            ? publProgRec.StoredCriteria.Where(x => x.Criterion == "LastOverhaulYearParam").Select(x => x.Value.ToInt()).Max() : 0;
                        sect["DateKr"] = lastOverhaulYear > 0 ? lastOverhaulYear.ToStr() : string.Empty;
                        sect["SumWorks"] = publProgRec.Sum;
                    }
                }
                else
                {
                    var works = new List<string>();
                    var listSt2 = rec.Value.Select(x => x.Stage2Id).ToList();
                    foreach (var st2 in listSt2)
                    {
                        if (dictSt2Works.ContainsKey(st2))
                        {
                            works.AddRange(dictSt2Works[st2]);
                        }
                    }

                    var workNames = works.Any() ? works.Distinct().AggregateWithSeparator(", ") : string.Empty;

                    sect.ДобавитьСтроку();

                    sect["row"] = i++;
                    sect["Mu"] = dictRo[rec.Key.roId].muName;
                    sect["Mo"] = dictRo[rec.Key.roId].moName;
                    sect["Address"] = dictRo[rec.Key.roId].Address;
                    sect["StartDate"] = dictRo[rec.Key.roId].DateCommissioning;

                    sect["OOI"] = rec.Value.Select(x => x.CommonEstateObjects).AggregateWithSeparator(", ");
                    sect["Wear"] = rec.Value.Any(y => y.StoredCriteria.Any(x => x.Criterion == "StructuralElementWearout"))
                        ? ((decimal)rec.Value.Select(y => y.StoredCriteria.Where(x => x.Criterion == "StructuralElementWearout").Select(x => x.Value.ToInt()).Average()).Average()).RoundDecimal(2) : 0;
                    sect["TypeWork"] = workNames;
                    sect["Year"] = rec.Key.PublishedYear;
                    var lastOverhaulYear = rec.Value.Any(y => y.StoredCriteria.Any(x => x.Criterion == "LastOverhaulYearParam"))
                        ? rec.Value.Select(y => y.StoredCriteria.Where(x => x.Criterion == "LastOverhaulYearParam").Select(x => x.Value.ToInt()).Max()).Max() : 0;
                    sect["DateKr"] = lastOverhaulYear > 0 ? lastOverhaulYear.ToStr() : string.Empty;
                    sect["SumWorks"] = rec.Value.Sum(x => x.Sum);
                }
            }
        }
    }
}