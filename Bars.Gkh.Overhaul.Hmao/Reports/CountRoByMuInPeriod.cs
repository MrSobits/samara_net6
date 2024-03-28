namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;
    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Castle.Windsor;

    /// <summary>
    /// Количество МКД по муниципальным образованиям за период
    /// </summary>
    public class CountRoByMuInPeriod: BasePrintForm
    {
        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Сервис модификации коллекции
        /// </summary>
        public IModifyEnumerableService ModifyEnumerableService { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        public CountRoByMuInPeriod()
            : base(new ReportTemplateBinary(Properties.Resources.CountRoByMuInPeriod))
        {
        }

        #region ReportProperties
        /// <inheritdoc />
        public override string Name
        {
            get { return "Количество МКД по муниципальным образованиям за период"; }
        }

        /// <inheritdoc />
        public override string Desciption
        {
            get { return "Количество МКД по муниципальным образованиям за период"; }
        }

        /// <inheritdoc />
        public override string GroupName
        {
            get { return "Долгосрочная программа"; }
        }

        /// <inheritdoc />
        public override string ParamsController
        {
            get { return "B4.controller.report.CountRoByMuInPeriod"; }
        }

        /// <inheritdoc />
        public override string RequiredPermission
        {
            get { return "Ovrhl.CountRoByMuInPeriod"; }
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
            var data = this.Container.Resolve<IDomainService<DpkrCorrectionStage2>>().GetAll()
                .WhereIf(this.municipalityIds.Any(), x => this.municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.Municipality != null)
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .Where(x => x.PlanYear >= this.startYear)
                .Where(x => x.PlanYear <= this.endYear)
                .Select(x => new DpkrCorrectionStage2Proxy
                {
                    RoId = x.RealityObject.Id,
                    MuName = x.RealityObject.Municipality.Name,
                    MuId = x.RealityObject.Municipality.Id,
                    StreetName = x.RealityObject.FiasAddress.StreetName,
                    House = x.RealityObject.FiasAddress.House,
                    Housing = x.RealityObject.FiasAddress.Housing,
                    PlaceName = x.RealityObject.FiasAddress.PlaceName
                })
                .OrderBy(x => x.MuName)
                .ThenBy(x => x.StreetName)
                .ThenBy(x => x.House)
                .ThenBy(x => x.Housing)
                .AsEnumerable()
                .Distinct(x => x.RoId);

            if (this.ModifyEnumerableService != null)
            {
                data = this.ModifyEnumerableService.ReplaceProperty(data, ".", x => x.MuName, x => x.PlaceName, x => x.StreetName);
            }

            var correction = data.GroupBy(x => new { x.MuId, x.MuName });

            reportParams.SimpleReportParams["startPeriod"] = this.startYear;
            reportParams.SimpleReportParams["endPeriod"] = this.endYear;

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            var totalCount = 0;

            foreach (var mu in correction)
            {
                section.ДобавитьСтроку();
                var muName = mu.Key.MuName;

                var sectionMu = section.ДобавитьСекцию("sectionMu");

                var countMu = 0;

                foreach (var ro in mu)
                {
                    sectionMu.ДобавитьСтроку();
                    sectionMu["MuName"] = muName;
                    sectionMu["PlaceName"] = ro.PlaceName;
                    sectionMu["Street"] = ro.StreetName;
                    sectionMu["House"] = ro.House;
                    sectionMu["Housing"] = ro.Housing;
                    sectionMu["Count"] = 1;
                    countMu++;
                }

                section["CountMu"] = countMu;
                totalCount += countMu;
            }

            reportParams.SimpleReportParams["CountTotal"] = totalCount;
        }

        private class DpkrCorrectionStage2Proxy
        {
            public long RoId { get; set; }
            public string MuName { get; set; }
            public long MuId { get; set; }
            public string StreetName { get; set; }
            public string House { get; set; }
            public string Housing { get; set; }
            public string PlaceName { get; set; }
        }
    }
}
