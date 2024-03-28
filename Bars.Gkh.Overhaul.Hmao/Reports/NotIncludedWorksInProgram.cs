using System.Collections.Generic;
using Bars.Gkh.Config;
using Bars.Gkh.Domain;
using Bars.Gkh.Entities.Dicts;
using Bars.Gkh.Enums;
using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.Utils;

namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;

    public class NotIncludedWorksInProgram : BasePrintForm
    {
        public IRepository<Municipality> MunicipalityDomain { get; set; }

        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionDomain { get; set; }

        public IDomainService<VersionRecordStage2> VersionRecSt2Domain { get; set; }

        public IDomainService<VersionRecordStage1> VersionRecSt1Domain { get; set; }

        public IDomainService<StructuralElementWork> StructElWorkDomain { get; set; }

        public IDomainService<Work> WorkDomain { get; set; }

        public IGkhParams GkhParams { get; set; }

        public NotIncludedWorksInProgram()
            : base(new ReportTemplateBinary(Properties.Resources.NotIncludedWorksInProgram))
        {

        }

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get
            {
                return "Работы, исключенные из Долгосрочной программы капитального ремонта";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Работы, исключенные из Долгосрочной программы капитального ремонта";
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
                return "B4.controller.report.NotIncludedWorksInProgram";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.NotIncludedWorksInProgram";
            }
        }

        private long[] municipalityIds;

        public override void SetUserParams(BaseParams baseParams)
        {
             municipalityIds = baseParams.Params.GetAs("municipalityIds", string.Empty).ToLongArray();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var appParams = GkhParams.GetParams();

            var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                    ? appParams["MoLevel"].To<MoLevel>()
                    : MoLevel.MunicipalUnion;

            municipalityIds = MunicipalityDomain.GetAll()
                     .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Id))
                     .Select(x => new { x.Id, x.Level })
                     .AsEnumerable()
                     .Where(x => x.Level.ToMoLevel(Container) == moLevel)
                     .Select(x => x.Id)
                     .ToArray();

            var works = StructElWorkDomain.GetAll()
                                .Select(x => new
                                {
                                    StructId = x.StructuralElement.Id,
                                    WorkId = x.Job.Work.Id
                                })
                                .ToList();

            var workNames = WorkDomain.GetAll()
                    .Select(x => new
                    {
                        x.Id,
                        x.Name
                    })
                    .ToList();

            var data =
                VersionRecSt2Domain.GetAll()
                    .Where(x => x.Stage3Version.ProgramVersion.IsMain)
                    .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Stage3Version.ProgramVersion.Municipality.Id))
                    .Where(x => !DpkrCorrectionDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Stage3Version.Id))
                    .Select(x => new
                    {
                        x.Id,
                        MuId = x.Stage3Version.RealityObject.Municipality.Id,
                        Municipality = x.Stage3Version.RealityObject.Municipality.Name,
                        x.Stage3Version.RealityObject.Address,
                        RoId = x.Stage3Version.RealityObject.Id,
                        PlanYear = x.Stage3Version.Year,
                        x.Sum,
                        CommonEstateObject = x.CommonEstateObject.Name,
                        x.Stage3Version.IndexNumber
                    })
                    .OrderBy(x => x.Municipality)
                    .ThenBy(x => x.PlanYear)
                    .ThenBy(x => x.IndexNumber)
                    .AsEnumerable()
                    .GroupBy(x => x.MuId)
                    .ToDictionary(x => x.Key);

            // работы которые выпали
            var worksBySt2 = VersionRecSt1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                .WhereIf(municipalityIds.Length > 0,
                    x => municipalityIds.Contains(x.Stage2Version.Stage3Version.ProgramVersion.Municipality.Id))
                .Where(x => !DpkrCorrectionDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Stage2Version.Stage3Version.Id))
                .Select(x => new
                {
                    St2Id = x.Stage2Version.Id,
                    StElId = x.StructuralElement.StructuralElement.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.St2Id)
                .Select(x => new
                {
                    x.Key,
                    StrElIds = x.Select(y => y.StElId)
                })
                .ToDictionary(x => x.Key,
                    y =>
                        works.Where(x => y.StrElIds.Contains(x.StructId))
                            .Select(x => x.WorkId).Distinct().ToList());

            // работы которые не выпали
            var worksByRo = VersionRecSt1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                .WhereIf(municipalityIds.Length > 0,
                    x => municipalityIds.Contains(x.Stage2Version.Stage3Version.ProgramVersion.Municipality.Id))
                .Where(x => DpkrCorrectionDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Stage2Version.Stage3Version.Id))
                .Select(x => new
                {
                    RoId = x.Stage2Version.Stage3Version.RealityObject.Id,
                    StElId = x.StructuralElement.StructuralElement.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .Select(x => new
                {
                    x.Key,
                    StrElIds = x.Select(y => y.StElId)
                })
                .ToDictionary(x => x.Key,
                    y =>
                        works.Where(x => y.StrElIds.Contains(x.StructId))
                            .Select(x => x.WorkId).Distinct().ToList());

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");

            var number = 1;
            foreach (var dataByMu in data)
            {
                sectionMu.ДобавитьСтроку();

                sectionMu["Municipality"] = dataByMu.Value.First().Municipality;

                var section = sectionMu.ДобавитьСекцию("section");

                foreach (var rec in dataByMu.Value)
                {
                    var existWorks = worksByRo.Get(rec.RoId) ?? new List<long>();
                    var noExistWorks = worksBySt2.Get(rec.Id) ?? new List<long>();
                    noExistWorks = noExistWorks.Where(x => !existWorks.Contains(x)).ToList();

                    if (!noExistWorks.Any())
                    {
                        continue;
                    }

                    if (worksByRo.ContainsKey(rec.RoId))
                    {
                        worksByRo[rec.RoId].AddRange(noExistWorks);
                    }
                    else
                    {
                        worksByRo.Add(rec.RoId, noExistWorks);
                    }

                    section.ДобавитьСтроку();

                    section["Number"] = number++;
                    section["IndexNumber"] = rec.IndexNumber;
                    section["Address"] = rec.Address;
                    section["Ceo"] = rec.CommonEstateObject;
                    section["PlanYear"] = rec.PlanYear;
                    section["Sum"] = rec.Sum;
                    section["Works"] = workNames.Where(x => noExistWorks.Contains(x.Id)).Select(x => x.Name).AggregateWithSeparator("; ");
                }
            }

        }
    }
}