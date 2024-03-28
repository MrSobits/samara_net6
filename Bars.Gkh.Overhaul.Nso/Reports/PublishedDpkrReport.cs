namespace Bars.Gkh.Overhaul.Nso.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;    
    using B4.Modules.Reports;
    
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Overhaul.Entities;

    public class PublishedDpkrReport : BasePrintForm
    {
        public PublishedDpkrReport()
            : base(new ReportTemplateBinary(Properties.Resources.PublishedDpkr))
        {

        }

        public IWindsorContainer Container { get; set; }

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
            var mainQuery = Container.Resolve<IDomainService<PublishedProgramRecord>>().GetAll()
          .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
          .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Stage2.Stage3Version.RealityObject.Municipality.Id))
          .Select(x => new
          {
              x.Id,
              x.Stage2.Stage3Version.CommonEstateObjects,
              MuName = x.Stage2.Stage3Version.RealityObject.Municipality.Name,
              x.Stage2.Stage3Version.RealityObject.Address,
              x.Stage2.Stage3Version.StoredCriteria,
              x.Stage2.Stage3Version.RealityObject.DateLastOverhaul,
              x.PublishedYear,
              Stage2Id = x.Stage2.Id,
              roId = x.Stage2.Stage3Version.RealityObject.Id
          });

            var data = mainQuery.OrderBy(x => x.MuName)
                         .ThenBy(x => x.Address)
                         .ThenBy(x => x.PublishedYear)
                         .AsEnumerable()
                         .GroupBy(x => new { x.roId, x.PublishedYear })
                         .ToDictionary(x => x.Key, y => y.ToList());

            var st1Data = Container.Resolve<IDomainService<VersionRecordStage1>>().GetAll()
                     .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                     .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                     .Select(x => new
                     {
                         Stage2Id = x.Stage2Version.Id,
                         StructId = x.StructuralElement.StructuralElement.Id
                     })
                     .ToList();

            var dictRo =
                Container.Resolve<IDomainService<RealityObject>>()
                         .GetAll()
                         .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Municipality.Id))
                         .Select(x => new { muName = x.Municipality.Name, roId = x.Id, x.Address, x.DateCommissioning })
                         .AsEnumerable()
                         .GroupBy(x => x.roId)
                         .ToDictionary(x => x.Key, v => v.FirstOrDefault());

            //словарь идентификатор оои - строка наименований работ
            var listWorks = Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
                    .Select(x => new
                    {
                        structId = x.StructuralElement.Id,
                        WorkName = x.Job.Name
                    })
                    .ToList(); 

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var minYear = config.ProgrammPeriodStart;
            var maxYear = config.ProgrammPeriodEnd;

            reportParams.SimpleReportParams["period"] = string.Format("{0}-{1}", minYear, maxYear);

            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("Section");
            var i = 1;

            foreach (var rec in data)
            {

                var strElemIds = st1Data.Where(x => rec.Value.Any(y => y.Stage2Id == x.Stage2Id)).Select(x => x.StructId).ToList();
                var workNames = listWorks.Where(x => strElemIds.Contains(x.structId)).Select(x => x.WorkName).Distinct().ToArray();

                sect.ДобавитьСтроку();

                sect["row"] = i++;
                sect["Mu"] = dictRo[rec.Key.roId].muName;
                sect["Address"] = dictRo[rec.Key.roId].Address;
                sect["StartDate"] = dictRo[rec.Key.roId].DateCommissioning;

                sect["OOI"] = rec.Value.Select(x => x.CommonEstateObjects).AggregateWithSeparator(", ");
                sect["Wear"] = rec.Value.Any(y => y.StoredCriteria.Any(x => x.Criterion == "StructuralElementWearout")) 
                    ? rec.Value.Select(y => y.StoredCriteria.Where(x => x.Criterion == "StructuralElementWearout").Select(x => x.Value.ToInt()).Average()).Average() : 0;
                sect["TypeWork"] = workNames.Any() ? workNames.AggregateWithSeparator(", ") : string.Empty;
                sect["Year"] = rec.Key.PublishedYear;
                var lastOverhaulYear =  rec.Value.Any(y => y.StoredCriteria.Any(x => x.Criterion == "LastOverhaulYearParam")) 
                    ? rec.Value.Select(y => y.StoredCriteria.Where(x => x.Criterion == "LastOverhaulYearParam").Select(x => x.Value.ToInt()).Max()).Max() : 0;
                sect["DateKr"] = lastOverhaulYear > 0 ? lastOverhaulYear.ToStr() : string.Empty;

            }
        }
    }
}