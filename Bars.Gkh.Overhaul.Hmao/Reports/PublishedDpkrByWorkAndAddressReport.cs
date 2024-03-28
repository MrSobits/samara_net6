namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Config;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Overhaul.Entities;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Castle.Core.Internal;

    public class PublishedDpkrByWorkAndAddressReport : BasePrintForm
    {
        #region Dependency injection members

        public IDomainService<PublishedProgramRecord> PublishedProgRecDomain { get; set; }

        public IDomainService<VersionRecordStage1> Stage1VersionDomain { get; set; }

        public IRepository<Municipality> MunicipalityDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        #endregion

        public PublishedDpkrByWorkAndAddressReport()
            : base(new ReportTemplateBinary(Properties.Resources.PublishProgramByWorkAndAddress))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Отчет по опубликованию (в разрезе видов работ и уровней адреса)";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Отчет по опубликованию (в разрезе видов работ и уровней адреса)";
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
                return "B4.controller.report.PublishedDpkrByWork";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.PublishedDpkrByWorkAndAddressReport";
            }
        }

        private long[] municipalityIds;

        private int groupPeriod;

        // Используется для перевода арабских чисел в римские
        private readonly Dictionary<int, string> romanDict = new Dictionary<int, string>
        {
            { 1000, "M" },
            { 900, "CM" }, { 500, "D" }, { 400, "CD" }, { 100, "C" },
            { 90, "XC" }, { 50, "L" }, { 40, "XL" }, { 10, "X" },
            { 9, "IX" }, { 5, "V" }, { 4, "IV" }, { 1, "I" }
        };

        // Приоритет сортировки для МО
        private readonly Dictionary<string, int> sortPriorityDict = new Dictionary<string, int>()
        {
            {"муниципальный округ", 0 },
            {"городской округ", 1 },
            {"город ", 2 },
            {"город-курорт", 3 }
        };

        /// <summary>
        /// Перевести арабское число в римское
        /// </summary>
        /// <param name="number">Арабское число</param>
        /// <returns>Римское число</returns>
        private string ConvertArabicNumberToRoman(int number) => romanDict
            .Where(d => number >= d.Key)
            .Select(d => d.Value + ConvertArabicNumberToRoman(number - d.Key))
            .FirstOrDefault();

        /// <summary>
        /// Получить приоритет сортировки для МО по его наименованию
        /// </summary>
        /// <remarks>Без учета наименования образования</remarks>
        /// <param name="municipalityName">Наименование МО</param>
        /// <returns>Приоритет сортировки</returns>
        private int GetSortPriority(string municipalityName)
        {
            foreach (var key in sortPriorityDict.Keys)
                if (!municipalityName.IsNullOrEmpty() && municipalityName.ToLower().Contains(key))
                    return sortPriorityDict[key];

            // Для остальных наименований
            return sortPriorityDict.Keys.Count;
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            this.groupPeriod = baseParams.Params.GetAs("groupPeriod", 5);

            this.municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToArray()
                : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var energyEfficiencyClassesService = this.Container.ResolveDomain<EnergyEfficiencyClasses>();
            var tehPassportValueService = this.Container.ResolveDomain<TehPassportValue>();
            var structuralElementWorkService = this.Container.ResolveDomain<StructuralElementWork>();
            var appParamsService = this.Container.Resolve<IGkhParams>();

            using (this.Container.Using(energyEfficiencyClassesService, tehPassportValueService, structuralElementWorkService, appParamsService))
            {
                var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
                var minYear = config.ProgrammPeriodStart;
                var maxYear = config.ProgrammPeriodEnd;

                if (minYear == 0 || maxYear == 0 || minYear > maxYear)
                {
                    throw new ReportParamsException("Проверьте правильность параметров расчета дпкр");
                }

                CreateVerticalColumns(reportParams, minYear, maxYear);

                var appParamsDict = appParamsService.GetParams();

                var moLevel = appParamsDict.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParamsDict["MoLevel"].To<string>())
                    ? appParamsDict["MoLevel"].To<MoLevel>()
                    : MoLevel.MunicipalUnion;

                var showUrbanAreaHigh = appParamsDict.ContainsKey("ShowUrbanAreaHigh") && appParamsDict["ShowUrbanAreaHigh"].ToBool();

                var muNameById = this.MunicipalityDomain.GetAll()
                    .Where(x => this.municipalityIds.Any()
                        ? this.municipalityIds.Contains(x.Id)
                        // Приведение TypeMunicipality к MoLevel в зависимости от единых настроек приложения
                        : ((x.Level == TypeMunicipality.MunicipalArea ||
                            (x.Level == TypeMunicipality.UrbanArea && showUrbanAreaHigh && moLevel == MoLevel.MunicipalUnion))
                            ? MoLevel.MunicipalUnion
                            : MoLevel.Settlement) == moLevel)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        ParentName = x.ParentMo.Name
                    })
                    .ToDictionary(x => x.Id, y => $"{y.Name} {y.ParentName}");

                var realityObjectQueryable = this.RealityObjectDomain.GetAll()
                    .WhereIf(this.municipalityIds.Any() && moLevel == MoLevel.MunicipalUnion,
                        x => this.municipalityIds.Contains(x.Municipality.Id))
                    .WhereIf(this.municipalityIds.Any() && moLevel == MoLevel.Settlement,
                        x => this.municipalityIds.Contains(x.MoSettlement.Id));

                var roAddressById = realityObjectQueryable
                    .ToDictionary(x => x.Id, y => y.Address);

                var energyEfficiencyClassesDict = energyEfficiencyClassesService
                    .GetAll()
                    .ToDictionary(x => x.Code,
                        y => y.Designation);

                var roEnergyEfficiencyClass = tehPassportValueService
                    .GetAll()
                    .Where(x => realityObjectQueryable.Any(y => x.TehPassport.RealityObject.Id == y.Id) &&
                        // Класс энергоэффективности здания
                        x.FormCode == "Form_6_1_1" && x.CellCode == "1:1" && x.Value != null)
                    .ToDictionary(x => x.TehPassport.RealityObject.Id,
                        y => energyEfficiencyClassesDict.ContainsKey(y.Value)
                            ? energyEfficiencyClassesDict[y.Value]
                            : "");

                var publishRecQuery = this.PublishedProgRecDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .WhereIf(this.municipalityIds.Any(),
                        x => this.municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id));

                var worksDict = structuralElementWorkService
                    .GetAll()
                    .Select(x => new
                    {
                        structId = x.StructuralElement.Id,
                        WorkName = x.Job.Work.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.structId)
                    .ToDictionary(x => x.Key, y => y.First().WorkName);

                var yearByStage2Dict = publishRecQuery
                    .Where(x => x.Stage2 != null)
                    .Select(x => new { x.Stage2.Id, x.PublishedYear })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.PublishedYear).FirstOrDefault());

                var data = this.Stage1VersionDomain.GetAll()
                    .Where(x => publishRecQuery.Any(y => y.Stage2.Id == x.Stage2Version.Id))
                    .WhereIf(this.municipalityIds.Any() && moLevel == MoLevel.MunicipalUnion,
                        x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .WhereIf(this.municipalityIds.Any() && moLevel == MoLevel.Settlement,
                        x => this.municipalityIds.Contains(x.RealityObject.MoSettlement.Id))
                    .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.RealityObject.Address,
                        MuId = moLevel == MoLevel.MunicipalUnion
                            ? (long?) x.RealityObject.Municipality.Id
                            : (long?) x.RealityObject.MoSettlement.Id,
                        StructElId = x.StructuralElement.StructuralElement.Id,
                        Stage2Id = x.Stage2Version.Id,
                        SortPriority = GetSortPriority(x.RealityObject.Municipality.Name),
                        MuName = moLevel == MoLevel.MunicipalUnion
                            ? x.RealityObject.Municipality.Name
                            : x.RealityObject.MoSettlement.Name,
                        ParentMuName = moLevel == MoLevel.MunicipalUnion
                            ? x.RealityObject.Municipality.ParentMo.Name
                            : x.RealityObject.MoSettlement.ParentMo.Name,
                        PlaceName = x.RealityObject.FiasAddress.PlaceName ?? "",
                        StreetName = x.RealityObject.FiasAddress.StreetName ?? "",
                        x.RealityObject.FiasAddress.House
                    })
                    .AsEnumerable()
                    .Where(x => x.MuId.HasValue && muNameById.ContainsKey(x.MuId.Value))
                    .OrderBy(x => x.SortPriority)
                    .ThenBy(x => x.MuName)
                    .ThenBy(x => x.ParentMuName)
                    .ThenBy(x => x.PlaceName.Substring(x.PlaceName.IndexOf(". ") + 1))
                    .ThenBy(x => x.StreetName.Substring(x.StreetName.IndexOf(". ") + 1))
                    .ThenBy(x => x.House)
                    .Select(x => new
                    {
                        MuId = x.MuId.Value,
                        x.RoId,
                        WorkName = worksDict.ContainsKey(x.StructElId) ? worksDict[x.StructElId] : string.Empty,
                        Year = yearByStage2Dict.ContainsKey(x.Stage2Id) ? yearByStage2Dict[x.Stage2Id] : 0
                    })
                    .GroupBy(x => x.MuId)
                    .ToDictionary(x => x.Key,
                        y => y.GroupBy(x => x.RoId)
                            .ToDictionary(x => x.Key,
                                z => z.Select(x => new
                                {
                                    x.Year,
                                    x.WorkName
                                })));

                var sectMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMunicipality");

                var muNumber = 1;

                foreach (var muData in data)
                {
                    var roNumber = 1;
                    sectMunicipality.ДобавитьСтроку();
                    sectMunicipality["Municipality"] = muNameById.ContainsKey(muData.Key)
                        ? ConvertArabicNumberToRoman(muNumber) + ". " + muNameById[muData.Key]
                        : string.Empty;

                    var sectRealObj = sectMunicipality.ДобавитьСекцию("sectionRealObj");

                    foreach (var roData in muData.Value)
                    {
                        sectRealObj.ДобавитьСтроку();

                        sectRealObj["Number"] = roNumber + ".";
                        sectRealObj["Address"] = roAddressById.ContainsKey(roData.Key)
                            ? roAddressById[roData.Key]
                            : string.Empty;
                        sectRealObj["EnergyEfficiencyClass"] = roEnergyEfficiencyClass.ContainsKey(roData.Key)
                            ? roEnergyEfficiencyClass[roData.Key]
                            : string.Empty;

                        var currentYear = minYear;

                        while (currentYear <= maxYear)
                        {
                            var tmpEndYear = currentYear + this.groupPeriod - 1;
                            int endYearPeriod =
                                tmpEndYear > maxYear
                                    ? maxYear
                                    : tmpEndYear;

                            var works = roData.Value.Where(x => x.Year >= currentYear && x.Year <= endYearPeriod)
                                .Select(x => x.WorkName)
                                .Distinct()
                                .ToList();

                            sectRealObj[$"TypeWork{currentYear}"] = works.Any() ? works.AggregateWithSeparator(", ") : " _____ \n";

                            currentYear = endYearPeriod + 1;
                        }

                        roNumber++;
                    }

                    muNumber++;
                } 
            }
        }

        // заполнение вертикальной секции
        public void CreateVerticalColumns(ReportParams reportParams, int minYear, int maxYear)
        {
            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("vertsection");

            var currentYear = minYear;
            var columnNum = 3;
            while (currentYear <= maxYear)
            {
                var tmpEndYear = currentYear + this.groupPeriod - 1;
                var endYearPeriod =
                    tmpEndYear > maxYear
                        ? maxYear
                        : tmpEndYear;

                verticalSection.ДобавитьСтроку();
                verticalSection["PeriodYears"] =
                    endYearPeriod - currentYear > 1
                        ? string.Format("{0}-{1}", currentYear, endYearPeriod)
                        : currentYear.ToString("0000");
                verticalSection["TypeWork"] = string.Format("$TypeWork{0}$", currentYear);
                verticalSection["ColumnNum"] = columnNum++;

                currentYear = endYearPeriod + 1;
            }

            reportParams.SimpleReportParams["effColumnNum"] = columnNum;
        }
    }
}