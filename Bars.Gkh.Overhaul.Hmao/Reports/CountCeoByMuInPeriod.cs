namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;

    /// <summary>
    /// Количество конструктивных элементов по муниципальным образованиям за период
    /// </summary>
    public class CountCeoByMuInPeriod : BasePrintForm
    {
        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// ООИ
        /// </summary>
        public IDomainService<CommonEstateObject> CommonEstateObjectDomain { get; set; }

        /// <summary>
        /// Корректировка ДПКР
        /// </summary>
        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionStage2Domain { get; set; }

        /// <summary>
        /// Запись версии
        /// </summary>
        public IDomainService<VersionRecordStage1> VersionRecordStage1Domain { get; set; }

        /// <summary>
        /// МО
        /// </summary>
        public IRepository<Municipality> MunicipalityRepository { get; set; }

        /// <summary>
        /// Сервис модификации коллекции
        /// </summary>
        public IModifyEnumerableService ModifyEnumerableService { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public CountCeoByMuInPeriod()
            : base(new ReportTemplateBinary(Properties.Resources.CountCeoByMuInPeriod))
        {
        }

        #region ReportProperties
        /// <inheritdoc />
        public override string Name
        {
            get { return "Количество конструктивных элементов по муниципальным образованиям за период"; }
        }

        /// <inheritdoc />
        public override string Desciption
        {
            get { return "Количество конструктивных элементов по муниципальным образованиям за период"; }
        }

        /// <inheritdoc />
        public override string GroupName
        {
            get { return "Долгосрочная программа"; }
        }

        /// <inheritdoc />
        public override string ParamsController
        {
            get { return "B4.controller.report.CountCeoByMuInPeriod"; }
        }

        /// <inheritdoc />
        public override string RequiredPermission
        {
            get { return "Ovrhl.CountCeoByMuInPeriod"; }
        }

        #endregion ReportProperties

        #region ReportParams

        private long[] municipalityIds;

        private int startYear;

        private int endYear;

        #endregion ReportParams

        /// <inheritdoc />
        public override void SetUserParams(BaseParams baseParams)
        {
            var strMuIds = baseParams.Params.GetAs<string>("municipalityIds");

            this.municipalityIds = !string.IsNullOrEmpty(strMuIds)
                ? strMuIds.Split(',').Select(x => x.ToLong()).ToArray()
                : new long[0];

            this.startYear = baseParams.Params.GetAs<int>("startYear");
            this.endYear = baseParams.Params.GetAs<int>("endYear");

            if (this.startYear > this.endYear)
            {
                throw new ReportParamsException("Год окончания должен быть больше года начала");
            }
        }

        /// <inheritdoc />
        public override string ReportGenerator { get; set; }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            var correctionQuery = this.DpkrCorrectionStage2Domain.GetAll()
                .WhereIf(this.municipalityIds.Any(), x => this.municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.Municipality != null)
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .Where(x => x.PlanYear >= this.startYear)
                .Where(x => x.PlanYear <= this.endYear);

            var dictStage1 = this.VersionRecordStage1Domain.GetAll()
                .Where(x => correctionQuery.Any(y => y.Stage2.Id == x.Stage2Version.Id))
                .Select(x => new
                {
                    MuId = x.RealityObject.Municipality.Id,
                    CeoId = x.Stage2Version.CommonEstateObject.Id,
                    x.StructuralElement.Volume,
                    RoSeId = x.StructuralElement.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key,
                    y => y
                        .GroupBy(x => x.CeoId)
                        .ToDictionary(x => x.Key, z => z.Distinct(x => x.RoSeId).Sum(x => x.Volume)));

            var correctionData = correctionQuery
                .OrderBy(x => x.RealityObject.Municipality.Name)
                .ThenBy(x => x.Stage2.CommonEstateObject.Name)
                .Select(x => new
                {
                    MuId = x.RealityObject.Municipality.Id,
                    CeoId = x.Stage2.CommonEstateObject.Id,
                    x.Stage2.Sum
                })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key,
                    y => y
                        .GroupBy(x => x.CeoId)
                        .ToDictionary(x => x.Key,
                            z =>
                                new
                                {
                                    Sum = z.Sum(x => x.Sum),
                                    Count = z.Count()
                                }));

            var muList = this.MunicipalityRepository.GetAll()
                .Select(x => new MunicipalityProxy
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToList();

            if (this.ModifyEnumerableService != null)
            {
                muList = this.ModifyEnumerableService.ReplaceProperty(muList, ".", x => x.Name).ToList();
            }

            var dictMu = muList.ToDictionary(x => x.Id, x => x.Name);

            var dictCeo = this.CommonEstateObjectDomain.GetAll().ToDictionary(x => x.Id, y => y.Name);

            reportParams.SimpleReportParams["startPeriod"] = this.startYear;
            reportParams.SimpleReportParams["endPeriod"] = this.endYear;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var totalCountCeo = 0;
            var totalSum = 0m;
            var totalVolume = 0m;

            foreach (var mu in correctionData)
            {
                section.ДобавитьСтроку();
                var sectionMu = section.ДобавитьСекцию("sectionMu");

                var muName = dictMu[mu.Key];

                var muCountCeo = 0;
                var muSum = 0m;
                var muVolume = 0m;

                foreach (var ceo in mu.Value)
                {
                    sectionMu.ДобавитьСтроку();

                    sectionMu["Mu"] = muName;
                    sectionMu["CeoName"] = dictCeo[ceo.Key];
                    sectionMu["CountCeo"] = ceo.Value.Count;
                    sectionMu["Sum"] = ceo.Value.Sum;

                    muCountCeo += ceo.Value.Count;
                    muSum += ceo.Value.Sum;

                    if (dictStage1.ContainsKey(mu.Key) && dictStage1[mu.Key].ContainsKey(ceo.Key))
                    {
                        sectionMu["Volume"] = dictStage1[mu.Key][ceo.Key];
                        muVolume += dictStage1[mu.Key][ceo.Key];
                    }
                }

                section["CountCeoMu"] = muCountCeo;
                section["SumMu"] = muSum;
                section["VolumeMu"] = muVolume;

                totalCountCeo += muCountCeo;
                totalSum += muSum;
                totalVolume += muVolume;
            }

            reportParams.SimpleReportParams["CountCeoTotal"] = totalCountCeo;
            reportParams.SimpleReportParams["SumTotal"] = totalSum;
            reportParams.SimpleReportParams["VolumeTotal"] = totalVolume;
        }

        private class MunicipalityProxy
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }
    }
}