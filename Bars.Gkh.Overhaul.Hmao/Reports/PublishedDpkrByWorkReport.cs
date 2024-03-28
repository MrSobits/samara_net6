namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Linq;
    
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Properties;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Отчет по опубликованию ДПКР (по видам работ)
    /// </summary>
    public class PublishedDpkrByWorkReport : BasePrintForm
    {
        private long[] municipalityIds;

        private int groupPeriod;

        /// <summary>
        /// .ctor
        /// </summary>
        public PublishedDpkrByWorkReport()
            : base(new ReportTemplateBinary(Resources.PublishProgramByWork))
        {
        }

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Отчет по опубликованию ДПКР (по видам работ)";

        /// <summary>
        /// Описание
        /// </summary>
        public override string Desciption => "Отчет по опубликованию ДПКР (по видам работ)";

        /// <summary>
        /// Наименовние группы
        /// </summary>
        public override string GroupName => "Долгосрочная программа";

        /// <summary>
        /// Контроллер
        /// </summary>
        public override string ParamsController => "B4.controller.report.PublishedDpkrByWork";

        /// <summary>
        /// Права доступа
        /// </summary>
        public override string RequiredPermission => "Reports.GkhOverhaul.PublishedDpkrByWorkReport";

        /// <summary>
        /// Домен-сервис <see cref="PublishedProgramRecord"/>
        /// </summary>
        public IDomainService<PublishedProgramRecord> PublishedProgRecDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="VersionRecordStage1"/>
        /// </summary>
        public IDomainService<VersionRecordStage1> Stage1VersionDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="Municipality"/>
        /// </summary>
        public IRepository<Municipality> MunicipalityDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObject"/>
        /// </summary>
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }


        /// <summary>
        /// Домен-сервис <see cref="Municipality"/>
        /// </summary>
        public IRepository<CommonEstateObject> CommonEstateObjectDomain { get; set; }

        /// <summary>
        /// Задать пользовательские параметры
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            this.groupPeriod = baseParams.Params.GetAs("groupPeriod", 5);

            this.municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => Convert.ToInt64(x)).ToArray()
                : new long[0];
        }

        /// <summary>
        /// Подготовить отчет
        /// </summary>
        /// <param name="reportParams">Параметры отчета</param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var minYear = config.ProgrammPeriodStart;
            var maxYear = config.ProgrammPeriodEnd;

            var commonEstateObjectDict = this.CommonEstateObjectDomain
                .GetAll()
                .AsEnumerable()
                .GroupBy(x => x.Code)
                .ToDictionary( x => x.Key, y => y.Select(z => z.GroupType.Name).FirstOrDefault());

            commonEstateObjectDict[""] = "";

            if (minYear == 0 || maxYear == 0 || minYear > maxYear)
            {
                throw new ReportParamsException("Проверьте правильность параметров расчета дпкр");
            }

            this.CreateVerticalColums(reportParams, minYear, maxYear);

            var muNameById = this.MunicipalityDomain.GetAll()
                .WhereIf(this.municipalityIds.Any(), x => this.municipalityIds.Contains(x.Id))
                .ToDictionary(x => x.Id, y => y.Name);

            var roAddressById = this.RealityObjectDomain.GetAll()
                .WhereIf(
                    this.municipalityIds.Any(),
                    x => this.municipalityIds.Contains(x.Municipality.Id)
                        || this.municipalityIds.Contains(x.MoSettlement.Id))
                .ToDictionary(x => x.Id, y => y.FiasAddress.AddressName);

            var roGkhCodeById = this.RealityObjectDomain.GetAll()
                .WhereIf(
                    this.municipalityIds.Any(),
                    x => this.municipalityIds.Contains(x.Municipality.Id)
                        || this.municipalityIds.Contains(x.MoSettlement.Id))
                .ToDictionary(x => x.Id, y => y.GkhCode);

            var publishRecQuery = this.PublishedProgRecDomain.GetAll()
                .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                .WhereIf(
                    this.municipalityIds.Any(),
                    x => this.municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id));

            var worksDict = this.Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
                .Select(
                    x => new
                    {
                        structId = x.StructuralElement.Id,
                        WorkName = x.Job.Work.Name
                    })
                .AsEnumerable()
                .GroupBy(x => x.structId)
                .ToDictionary(x => x.Key, y => y.First().WorkName);

            var yearByStage2Dict = publishRecQuery
                .Where(x => x.Stage2 != null)
                .Select(x => new {x.Stage2.Id, x.PublishedYear})
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.PublishedYear).FirstOrDefault());

            var appParams = this.Container.Resolve<IGkhParams>().GetParams();

            var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            var query = this.Stage1VersionDomain.GetAll()
                .Where(x => publishRecQuery.Any(y => y.Stage2.Id == x.Stage2Version.Id))
                .WhereIf(moLevel == MoLevel.Settlement, x => x.RealityObject.MoSettlement != null)
                .Select(
                    x => new
                    {
                        RoId = x.RealityObject.Id,
                        x.RealityObject.Address,
                        MuId = moLevel == MoLevel.MunicipalUnion ? x.RealityObject.Municipality.Id : x.RealityObject.MoSettlement.Id,
                        StructElId = x.StructuralElement.StructuralElement.Id,
                        Stage2Id = x.Stage2Version.Id,
                        StructElCode = x.StructuralElement.StructuralElement.Group.CommonEstateObject.Code
                    })
                .AsEnumerable();

            var data =
                query
                    .OrderBy(x => x.Address)
                    .Select(
                        x => new
                        {
                            x.MuId,
                            x.RoId,
                            WorkName = worksDict.ContainsKey(x.StructElId) ? worksDict[x.StructElId] : string.Empty,
                            Year = yearByStage2Dict.ContainsKey(x.Stage2Id) ? yearByStage2Dict[x.Stage2Id] : 0,
                            Code = commonEstateObjectDict.ContainsKey(x.StructElCode) ? x.StructElCode : "",
                          })
                    .GroupBy(x => x.MuId)
                    .ToDictionary(
                        x => x.Key,
                        y => y.GroupBy(x => x.RoId)
                            .ToDictionary(
                                x => x.Key,
                                z => z.Select(
                                    x => new
                                    {
                                        x.Year,
                                        x.WorkName,
                                        x.Code
                                    })));

            var sectMunicipality = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMunicipality");

            foreach (var muData in data)
            {
                sectMunicipality.ДобавитьСтроку();
                sectMunicipality["Municipality"] = muNameById.ContainsKey(muData.Key)
                    ? muNameById[muData.Key]
                    : string.Empty;

                var sectRealObj = sectMunicipality.ДобавитьСекцию("sectionRealObj");
                var number = 1;
                foreach (var roData in muData.Value)
                {
                    sectRealObj.ДобавитьСтроку();

                    sectRealObj["GkhCode"] = roGkhCodeById.ContainsKey(roData.Key) 
                        ? roGkhCodeById[roData.Key]
                        : string.Empty;

                    sectRealObj["Address"] = roAddressById.ContainsKey(roData.Key)
                        ? roAddressById[roData.Key]
                        : string.Empty;
                    sectRealObj["Number"] = number++;
                    var currentYear = minYear;

                    while (currentYear <= maxYear)
                    {
                        var tmpEndYear = currentYear + this.groupPeriod - 1;
                        int endYearPeriod =
                            tmpEndYear > maxYear
                                ? maxYear
                                : tmpEndYear;

                        var works =
                            roData.Value.Where(x => x.Year >= currentYear && x.Year <= endYearPeriod)
                                .Select(x => commonEstateObjectDict.Get(x.Code))
                                .Distinct()
                                .ToList();
                        
                        sectRealObj[$"TypeWork{currentYear}"] = works.Any() ? works.AggregateWithSeparator(", ") : string.Empty;

                        currentYear = endYearPeriod + 1;
                    }
                }
            }
        }

        /// <summary>
        /// Заполнение вертикальной секции
        /// </summary>
        /// <param name="reportParams">Параметры отчета</param>
        /// <param name="minYear">Год начала</param>
        /// <param name="maxYear">Год окончания</param>
        public void CreateVerticalColums(ReportParams reportParams, int minYear, int maxYear)
        {
            var verticalSection = reportParams.ComplexReportParams.ДобавитьСекцию("GroupPeriod");

            var currentYear = minYear;
            var colNumber = 3; // № п/п; Код дома в АИС МЖФ; Адрес многоквартирного дома

            while (currentYear <= maxYear)
            {
                var tmpEndYear = currentYear + this.groupPeriod - 1;
                var endYearPeriod =
                    tmpEndYear > maxYear
                        ? maxYear
                        : tmpEndYear;

                verticalSection.ДобавитьСтроку();
                verticalSection["ColNumber"] = ++colNumber;
                verticalSection["PeriodYears"] =
                    endYearPeriod - currentYear > 1
                        ? $"{currentYear}-{endYearPeriod}"
                        : currentYear.ToString("0000");
                verticalSection["TypeWork"] = $"$TypeWork{currentYear}$";

                currentYear = endYearPeriod + 1;
            }
        }
    }
}