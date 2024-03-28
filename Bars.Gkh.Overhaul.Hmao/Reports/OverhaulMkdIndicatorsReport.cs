namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class OverhaulMkdIndicatorsReport : BasePrintForm
    {
        public IDomainService<PublishedProgramRecord> PublishedProgramRecordService { get; set; }
        public IRepository<Municipality> MunicipalityDomain { get; set; } 
        public IWindsorContainer Container { get; set; }
        public IGkhParams GkhParams { get; set; }
        public MoLevel MoLevel { get; set; }

        private long[] municipalityIds = new long[0];
        private int startYear;
        private int finishYear;



        public OverhaulMkdIndicatorsReport()
            : base(new ReportTemplateBinary(Properties.Resources.OverhaulMkdIndicators))
        {
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var moIds = baseParams.Params.GetAs<string>("muIds").ToLongArray();
            startYear = baseParams.Params.GetAs<string>("startDate").ToInt();
            finishYear = baseParams.Params.GetAs<string>("finishDate").ToInt();

            var appParams = GkhParams.GetParams();
            MoLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            municipalityIds = MunicipalityDomain.GetAll()
                    .WhereIf(moIds.Any(), x => moIds.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Level
                    })
                    .AsEnumerable()
                    .Where(x => x.Level.ToMoLevel(Container) == MoLevel)
                    .Select(x => x.Id)
                    .ToArray();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var dict2 = PublishedProgramRecordService.GetAll()
                .Where(x => x.RealityObject.MoSettlement != null)
                .WhereIf(
                    municipalityIds.Any(),
                    x => municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id))
                .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                .Where(x => x.PublishedYear >= startYear && x.PublishedYear <= finishYear)
                .Select(x => new
                {
                    x.RealityObject.NumberLiving,
                    x.RealityObject.AreaMkd,
                    x.RealityObject.Municipality,
                    x.RealityObject.MoSettlement,
                    RoId = x.RealityObject.Id,
                    x.PublishedYear,
                    Key = x.RealityObject.Id + "_" + x.PublishedYear
                })
                .AsEnumerable()
                .Distinct(x => x.Key);

            var dict = dict2
                .OrderBy(x => x.Municipality.Name)
                .ThenBy(x => x.MoSettlement.Name)
                .ThenBy(x => x.PublishedYear)
                .GroupBy(x => x.Municipality)
                .ToDictionary(x => x.Key, y => y.GroupBy(z => z.MoSettlement)
                    .ToDictionary(a => a.Key, b => b.GroupBy(a => a.PublishedYear).ToDictionary(g => g.Key, h =>
                        new
                        {
                            area = h.Sum(j => j.AreaMkd),
                            numberliving = h.Sum(j => j.NumberLiving)
                        })));

            var sectionMr = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMr");

            var i = 1;

            foreach (var mr in dict.Keys)
            {
                var mrValue = dict[mr];

                sectionMr.ДобавитьСтроку();
                sectionMr["НомерМР"] = i;
                sectionMr["НаименованиеМР"] = mr.Name;
                sectionMr["ИтогоПлощадьМР"] = mrValue.Sum(x => x.Value.Sum(y => y.Value.area));
                sectionMr["ИтогоЖителейМР"] = mrValue.Sum(x => x.Value.Sum(y => y.Value.numberliving));

                var sectionMo = sectionMr.ДобавитьСекцию("sectionMo");

                var j = 1;

                foreach (var mo in mrValue.Keys)
                {
                    var moValue = mrValue[mo];

                   sectionMo.ДобавитьСтроку();
                   sectionMo["НомерМО"] = String.Format("{0}.{1}", i, j);
                   sectionMo["НаименованиеМО"] = mo.Name;
                   sectionMo["ИтогоПлощадьМО"] = moValue.Sum(x => x.Value.area);
                   sectionMo["ИтогоЖителейМО"] = moValue.Sum(x => x.Value.numberliving);

                   var sectionYear = sectionMo.ДобавитьСекцию("sectionYear");

                   foreach (var year in moValue.Keys)
                   {
                       var yearValue = moValue[year];

                       sectionYear.ДобавитьСтроку();

                       sectionYear["Год"] = year;
                       sectionYear["Площадь"] = yearValue.area;
                       sectionYear["ИтогоЖителей"] = yearValue.numberliving;
                   }

                    j++;
                }

                i++;
            }
        }

        public override string Name
        {
            get
            {
                return "Форма 3. Планируемые показатели выполнения работ по КР МКД";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Форма 3. Планируемые показатели выполнения работ по КР МКД";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Региональная программа";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.OverhaulMkdIndicators";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.OverhaulMkdIndicators";
            }
        }
    }
}
