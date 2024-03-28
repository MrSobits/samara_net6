namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Utils;
    using Overhaul.Entities;

    /// <summary>
    /// Отчет по ДПКР
    /// </summary>
    public class PublishedDpkrExtendedReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<VersionRecordStage1> Stage1Domain { get; set; }

        public IDomainService<RealityObjectStructuralElement> RoStructElementDomain { get; set; }

        public IDomainService<DpkrCorrectionStage2> CorrectionDomain { get; set; }

        public IDomainService<StructuralElement> StructuralElementDomain { get; set; }

        #endregion

        /// <summary>
        /// .ctor
        /// </summary>
        public PublishedDpkrExtendedReport()
            : base(new ReportTemplateBinary(Properties.Resources.PublishedDpkrExtended))
        {

        }

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сервис модификации коллекции
        /// </summary>
        public IModifyEnumerableService ModifyEnumerableService { get; set; }

        /// <inheritdoc />
        public override string Name
        {
            get
            {
                return "Отчет по ДПКР";
            }
        }

        /// <inheritdoc />
        public override string Desciption
        {
            get
            {
                return "Отчет по ДПКР";
            }
        }

        /// <inheritdoc />
        public override string GroupName
        {
            get
            {
                return "Долгосрочная программа";
            }
        }

        /// <inheritdoc />
        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.PublishedDpkrExtended";
            }
        }

        /// <inheritdoc />
        public override string RequiredPermission
        {
            get
            {
                return "Ovrhl.PublishedDpkrExtended";
            }
        }

        private long[] municipalityIds;

        private int groupPeriod;

        /// <inheritdoc />
        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            this.groupPeriod = baseParams.Params.GetAs("groupPeriod", 5);

            this.municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToArray()
                : new long[0];
        }

        /// <inheritdoc />
        public override string ReportGenerator { get; set; }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var minYear = config.ProgrammPeriodStart;
            var maxYear = config.ProgrammPeriodEnd;

            if (minYear == 0 || maxYear == 0 || minYear > maxYear || this.groupPeriod < 1 || this.groupPeriod > 10)
            {
                throw new ReportParamsException("Проверьте правильность параметров расчета дпкр");
            }

            // получаем года когда в ДПКР ожидается ремонт данного структурного элемента
            var dictCorrectionYears = this.CorrectionDomain.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                .Select(x => new
                {
                    St2Id = x.Stage2.Id,
                    x.PlanYear
                })
                .AsEnumerable()
                .GroupBy(x => x.St2Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.PlanYear).First());

            // получаем  связи Структурного элемента с записями 2 этапа ДПКР
            var dictForRoStructel = this.Stage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.Stage2Version.Stage3Version.ProgramVersion.Municipality.Id))
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
            var recordList = this.RoStructElementDomain.GetAll()
                .WhereIf(this.municipalityIds.Length > 0,
                    x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id)
                        || this.municipalityIds.Contains(x.RealityObject.MoSettlement.Id))
                .Where(x => x.State != null && x.State.StartState)
                .Select(x => new RoStructElementProxy
                {
                    Id = x.Id,
                    MuName = x.RealityObject.Municipality.Name,
                    Locality = x.RealityObject.FiasAddress.PlaceName,
                    Street = x.RealityObject.FiasAddress.StreetName,
                    RoId = x.RealityObject.Id,
                    House = x.RealityObject.FiasAddress.House,
                    Letter = x.RealityObject.FiasAddress.Letter,
                    Housing = x.RealityObject.FiasAddress.Housing,
                    BuildYear = x.RealityObject.BuildYear,
                    AreaLiving = x.RealityObject.AreaLiving,
                    Volume = x.Volume,
                    LastOverhaulYear = x.LastOverhaulYear,
                    CeoName = x.StructuralElement.Group.CommonEstateObject.Name,
                    SeName = x.StructuralElement.Name,
                    SeId = x.StructuralElement.Id
                })
                .OrderBy(x => x.MuName)
                .ThenBy(x => x.Locality)
                .ThenBy(x => x.Street)
                .ThenBy(x => x.House)
                .ThenBy(x => x.Letter)
                .ThenBy(x => x.Housing)
                .ThenBy(x => x.RoId)
                .ThenBy(x => x.CeoName)
                .ThenBy(x => x.SeName)
                .ToList();

            if (this.ModifyEnumerableService != null)
            {
                recordList = this.ModifyEnumerableService.ReplaceProperty(recordList, ".", x => x.MuName, x => x.Locality, x => x.Street).ToList();
            }

            reportParams.SimpleReportParams["reportdate"] = DateTime.Now.Date.ToShortDateString();

            if (!recordList.Any())
            {
                return;
            }

            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            // Раскрываем вертикальную секцию
            this.CreateVerticalColums(reportParams, minYear, maxYear);

            // Раскрываем горизонтальную секцию
            var i = 0;
            long currentRoId = 0;
            foreach (var rec in recordList)
            {
                sect.ДобавитьСтроку();

                if (rec.RoId != currentRoId)
                {
                    currentRoId = rec.RoId;
                    i++;
                }

                sect["row"] = i;
                sect["mu"] = rec.MuName;
                sect["locality"] = rec.Locality;
                sect["street"] = rec.Street;
                sect["house"] = "д. " + rec.House;
                sect["letter"] = "лит. " + rec.Letter;
                sect["housing"] = "корп. " + rec.Housing;
                sect["buildyear"] = rec.BuildYear;
                sect["arealiving"] = rec.AreaLiving;
                sect["ceo"] = rec.CeoName;
                sect["structel"] = rec.SeName;
                sect["volume"] = rec.Volume;
                sect["lastoverhaulyear"] = rec.LastOverhaulYear;

                if (dictForRoStructel.ContainsKey(rec.Id))
                {
                    var currentYear = minYear;

                    while (currentYear <= maxYear)
                    {
                        var tmpEndYear = currentYear + this.groupPeriod - 1;
                        int endYearPeriod =
                            tmpEndYear > maxYear
                                ? maxYear
                                : tmpEndYear;

                        //если есть хотя бы одна запись за указанный период, то ставим крестик
                        if (dictForRoStructel[rec.Id].Any(x => x >= currentYear && x <= endYearPeriod))
                        {
                            sect[$"haveelement_{currentYear}"] = "X";
                        }

                        currentYear = endYearPeriod + 1;
                    }
                }
            }
        }

        /// <summary>
        /// Заполнение вертикальной секции
        /// </summary>
        public void CreateVerticalColums(ReportParams reportParams, int minYear, int maxYear)
        {
            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("groupperiod");

            int currentYear = minYear;

            while (currentYear <= maxYear)
            {
                var tmpEndYear = currentYear + this.groupPeriod - 1;
                int endYearPeriod =
                    tmpEndYear > maxYear
                        ? maxYear
                        : tmpEndYear;

                verticalSection.ДобавитьСтроку();
                verticalSection["ceoperioditeration"] =
                    endYearPeriod - currentYear >= 1
                        ? $"{currentYear}-{endYearPeriod}"
                        : currentYear.ToString("0000");
                verticalSection["haveelement"] = $"$haveelement_{currentYear}$";

                currentYear = endYearPeriod + 1;
            }
        }

        private class RoStructElementProxy
        {
            public long Id { get; set; }
            public string MuName { get; set; }
            public string Locality { get; set; }
            public string Street { get; set; }
            public long RoId { get; set; }
            public string House { get; set; }
            public string Letter { get; set; }
            public string Housing { get; set; }
            public int? BuildYear { get; set; }
            public decimal? AreaLiving { get; set; }
            public decimal Volume { get; set; }
            public int LastOverhaulYear { get; set; }
            public string CeoName { get; set; }
            public string SeName { get; set; }
            public long SeId { get; set; }
        }
    }
}