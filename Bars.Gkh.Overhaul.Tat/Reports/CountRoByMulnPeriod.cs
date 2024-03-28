namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Castle.Windsor;
    public class CountRoByMuInPeriod : BasePrintForm
    {
        public CountRoByMuInPeriod()
            : base(new ReportTemplateBinary(Properties.Resources.CountRoByMuInPeriod))
        {
        }

        public IWindsorContainer Container { get; set; }

        #region ReportProperties

        public override string Name
        {
            get { return "Количество МКД по муниципальным образованиям за период"; }
        }

        public override string Desciption
        {
            get { return "Количество МКД по муниципальным образованиям за период"; }
        }

        public override string GroupName
        {
            get { return "Долгосрочная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CountRoByMuInPeriod"; }
        }

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

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMuIds = baseParams.Params.GetAs<string>("municipalityIds");

            municipalityIds = !string.IsNullOrEmpty(strMuIds)
                ? strMuIds.Split(',').Select(x => x.ToLong()).ToArray()
                : new long[0];

            startYear = baseParams.Params.GetAs<int>("startYear");
            endYear = baseParams.Params.GetAs<int>("endYear");

            if (startYear > endYear)
            {
                throw new ReportParamsException("Год окончания должен быть больше года начала");
            }
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var correction = Container.Resolve<IDomainService<VersionRecordStage1>>().GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Stage2Version.Stage3Version.ProgramVersion.Municipality.Id))

                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.Municipality != null)
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                .Where(x => x.Stage2Version.Stage3Version.CorrectYear >= startYear)
                .Where(x => x.Stage2Version.Stage3Version.CorrectYear <= endYear)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    MuName = x.RealityObject.Municipality.Name,
                    MuId = x.RealityObject.Municipality.Id,
                    x.RealityObject.FiasAddress.StreetName,
                    x.RealityObject.FiasAddress.House,
                    x.RealityObject.FiasAddress.Housing,
                    x.RealityObject.FiasAddress.PlaceName
                })
                .OrderBy(x => x.MuName)
                .ThenBy(x => x.StreetName)
                .ThenBy(x => x.House)
                .ThenBy(x => x.Housing)
                .AsEnumerable()
                .Distinct(x => x.RoId)
                .GroupBy(x => new { x.MuId, x.MuName });

            reportParams.SimpleReportParams["startPeriod"] = startYear;
            reportParams.SimpleReportParams["endPeriod"] = endYear;

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
    }
}