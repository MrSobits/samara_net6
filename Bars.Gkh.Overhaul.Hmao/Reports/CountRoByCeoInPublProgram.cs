namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;
    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;

    public class CountRoByCeoInPublProgram : BasePrintForm
    {
        public CountRoByCeoInPublProgram()
            : base(new ReportTemplateBinary(Properties.Resources.CountRoByCeoInPublProgram))
        {
        }

        public IWindsorContainer Container { get; set; }

        #region ReportProperties

        public override string Name
        {
            get { return "Количество объектов в опубликованной программе"; }
        }

        public override string Desciption
        {
            get { return "Количество объектов в опубликованной программе"; }
        }

        public override string GroupName
        {
            get { return "Долгосрочная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.CountRoByCeoInPublProgram"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhOverhaul.CountRoByCeoInPublProgram"; }
        }

        #endregion ReportProperties

        #region ReportFields

        private long[] municipalityIds;

        private int startYear;

        private int endYear;

        #endregion ReportFields

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
                throw new ReportParamsException("Год окончания должен быть больше либо равен году начала");
            }
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var data = Container.Resolve<IDomainService<PublishedProgramRecord>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id))
                .Where(x => x.Stage2 != null)
                //на всякий случай
                .Where(x => x.PublishedProgram.ProgramVersion.Municipality != null)
                .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                .Where(x => x.PublishedYear >= startYear)
                .Where(x => x.PublishedYear <= endYear)
                .OrderBy(x => x.CommonEstateobject)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    Year = x.PublishedYear,
                    CeoId = x.Stage2.CommonEstateObject.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.CeoId)
                .ToDictionary(x => x.Key,
                    y => y
                        .GroupBy(x => x.Year)
                        .ToDictionary(x => x.Key, z => z.Count()));

            FillVerticalSection(reportParams);

            var ceoDict =
                Container.Resolve<IDomainService<CommonEstateObject>>().GetAll()
                    .ToDictionary(x => x.Id, x => x.Name);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var item in data)
            {
                section.ДобавитьСтроку();

                section["ceoName"] = ceoDict[item.Key];

                for (int year = startYear; year <= endYear; year++)
                {
                    var key = "count_" + year;

                    if (!item.Value.ContainsKey(year))
                    {
                        section[key] = 0;
                        continue;
                    }

                    section[key] = item.Value[year];
                }
            }
        }

        private void FillVerticalSection(ReportParams reportParams)
        {
            var sectionYears = reportParams.ComplexReportParams.ДобавитьСекцию("sectionYears");

            for (int year = startYear; year <= endYear; year++)
            {
                sectionYears.ДобавитьСтроку();

                sectionYears["Year"] = year.ToString("0000");
                sectionYears["count"] = string.Format("$count_{0}$", year.ToString("0000"));
            }
        }
    }
}