namespace Bars.Gkh.Overhaul.Nso.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;
    using Bars.Gkh.Overhaul.Nso.Entities;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Utils;
    using Overhaul.Entities;

    public class PublishedDpkrExtendedReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<VersionRecordStage1> Stage1Domain { get; set; }

        public IDomainService<RealityObjectStructuralElement> RoStructElementDomain { get; set; }

        public IDomainService<DpkrCorrectionStage2> CorrectionDomain { get; set; }

        public IDomainService<StructuralElement> StructuralElementDomain { get; set; }

        #endregion

        public PublishedDpkrExtendedReport()
            : base(new ReportTemplateBinary(Properties.Resources.PublishedDpkrExtended))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Отчет по ДПКР";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по ДПКР";
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
                return "B4.controller.report.PublishedDpkrExtended";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.PublishedDpkrExtended";
            }
        }

        private List<long> municipalityIds;

        private int groupPeriod = 5;

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            groupPeriod = baseParams.Params.GetAs("groupPeriod", 5);

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var minYear = config.ProgrammPeriodStart;
            var maxYear = config.ProgrammPeriodEnd;

            // Если период проведения работ будет браться из параметров то раскомментировать.
            //  На момент выполнения задачи период сказали(Потемкина Алина, 29.11.2013 9:48) сделать пять лет
            //var ceoPeriod = parameters.ContainsKey("GroupByCeoPeriod") ? parameters["GroupByCeoPeriod"].ToInt() : 0;
            //int ceoPeriod = 5;
            if (minYear == 0 || maxYear == 0 || minYear > maxYear || groupPeriod < 1 || groupPeriod > 10)
            {
                throw new ReportParamsException("Проверьте правильность параметров расчета дпкр");
            }

            // получаем года когда в ДПКР ожидается ремонт данного структурного элемента 
            var dictCorrectionYears = CorrectionDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Stage2.Stage3Version.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    St2Id = x.Stage2.Id,
                    x.PlanYear
                })
                .AsEnumerable()
                .GroupBy(x => x.St2Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.PlanYear).First());

            // получаем  связи Структурного элемента с записями 2 этапа ДПКР
            var dictForRoStructel = Stage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                .WhereIf(municipalityIds.Count > 0,
                    x => municipalityIds.Contains(x.Stage2Version.Stage3Version.RealityObject.Municipality.Id))
                .Select(x => new
                {
                    St2Id = x.Stage2Version.Id,
                    RoSeId = x.StructuralElement.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.RoSeId)
                .ToDictionary(x => x.Key, group =>
                {
                    //собираем все года, в которые конструктивный элемент попадал в дпкр
                    var result = new List<int>();

                    foreach (var st2Id in group.Select(x => x.St2Id))
                    {
                        if (dictCorrectionYears.ContainsKey(st2Id))
                        {
                            result.Add(dictCorrectionYears[st2Id]);
                        }
                    }

                    return result;
                });

            // получаем список КЭ в доме , это будет основной список
            var recordList =
                RoStructElementDomain.GetAll()
                    .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .Where(x => x.State != null && x.State.StartState)
                    .Select(x => new
                    {
                        x.Id,
                        MuName = x.RealityObject.Municipality.Name,
                        Locality = x.RealityObject.FiasAddress.PlaceName,
                        Street = x.RealityObject.FiasAddress.StreetName,
                        RoId = x.RealityObject.Id,
                        x.RealityObject.FiasAddress.House,
                        x.RealityObject.FiasAddress.Housing,
                        x.RealityObject.BuildYear,
                        x.RealityObject.AreaLiving,
                        x.Volume,
                        x.LastOverhaulYear,
                        CeoName = x.StructuralElement.Group.CommonEstateObject.Name,
                        SeName = x.StructuralElement.Name,
                        SeId = x.StructuralElement.Id
                    })
                    .OrderBy(x => x.MuName)
                    .ThenBy(x => x.Locality)
                    .ThenBy(x => x.Street)
                    .ThenBy(x => x.House)
                    .ThenBy(x => x.Housing)
                    .ThenBy(x => x.RoId)
                    .ThenBy(x => x.CeoName)
                    .ThenBy(x => x.SeName)
                    .ToArray();

            reportParams.SimpleReportParams["reportDate"] = DateTime.Now.Date.ToShortDateString();

            if (!recordList.Any())
            {
                return;
            }

            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("Section");

            // Раскрываем вертикальную секцию
            CreateVerticalColums(reportParams, minYear, maxYear);

            // Раскрываем горизонтальную секцию
            var i = 0;
            var currentRoId = 0L;
            foreach (var rec in recordList)
            {
                sect.ДобавитьСтроку();

                if (rec.RoId != currentRoId)
                {
                    currentRoId = rec.RoId;
                    i++;
                }

                sect["row"] = i;
                sect["Mu"] = rec.MuName;
                sect["Locality"] = rec.Locality;
                sect["Street"] = rec.Street;
                sect["House"] = rec.House;
                sect["Housing"] = rec.Housing;
                sect["BuildYear"] = rec.BuildYear;
                sect["AreaLiving"] = rec.AreaLiving;
                sect["Ceo"] = rec.CeoName;
                sect["StructEl"] = rec.SeName;
                sect["Volume"] = rec.Volume;
                sect["LastOverhaulYear"] = rec.LastOverhaulYear;

                if (dictForRoStructel.ContainsKey(rec.Id))
                {
                    var currentYear = minYear;

                    while (currentYear <= maxYear)
                    {
                        var tmpEndYear = currentYear + groupPeriod - 1;
                        int endYearPeriod =
                            tmpEndYear > maxYear
                                ? maxYear
                                : tmpEndYear;

                        //если есть хотя бы одна запись за указанный период, то ставим крестик
                        if (dictForRoStructel[rec.Id].Any(x => x >= currentYear && x <= endYearPeriod))
                        {
                            sect[string.Format("haveElement_{0}", currentYear)] = "X";
                        }

                        currentYear = endYearPeriod + 1;
                    }
                }
            }
        }

        // заполнение вертикальной секции
        public void CreateVerticalColums(ReportParams reportParams, int minYear, int maxYear)
        {
            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("GroupPeriod");

            int currentYear = minYear;

            while (currentYear <= maxYear)
            {
                var tmpEndYear = currentYear + groupPeriod - 1;
                int endYearPeriod =
                    tmpEndYear > maxYear
                        ? maxYear
                        : tmpEndYear;

                verticalSection.ДобавитьСтроку();
                verticalSection["ceoPeriodIteration"] =
                    endYearPeriod - currentYear > 1
                        ? string.Format("{0}-{1}", currentYear, endYearPeriod)
                        : currentYear.ToString("0000");
                verticalSection["haveElement"] = string.Format("$haveElement_{0}$", currentYear);

                currentYear = endYearPeriod + 1;
            }
        }
    }
}
