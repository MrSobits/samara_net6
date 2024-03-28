namespace Bars.Gkh.Overhaul.Hmao.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Entities;
    using Gkh.Utils;

    public class DpkrCorrectionStage2Export : BaseDataExportService
    {
        public IDomainService<VersionRecord> versionRecord { get; set; }

        public IDomainService<DpkrCorrectionStage2> correctionDomain { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);

            var moId = 0M;
            var versionId = loadParam.Filter.GetAs<long>("versionId");
            if (versionId == 0)
            {
                moId = baseParams.Params.GetAs<long>("mo_id");
            }

            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;

            if (groupByRoPeriod == 0)
            {
                return correctionDomain.GetAll()
                         .WhereIf(moId > 0, x => x.Stage2.Stage3Version.ProgramVersion.IsMain && x.Stage2.Stage3Version.ProgramVersion.Municipality.Id == moId)
                         .WhereIf(versionId > 0, x => x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                         .Select(x => new
                         {
                             x.Stage2.Id,
                             Municipality = x.RealityObject.Municipality.Name,
                             RealityObject = x.RealityObject.Address,
                             CorrectionYear = x.PlanYear,
                             PlanYear = x.Stage2.Stage3Version.Year,
                             x.Stage2.Sum,
                             CommonEstateObjectName = x.Stage2.CommonEstateObject.Name,
                             x.Stage2.Stage3Version.IndexNumber
                         })
                         .OrderIf(loadParam.Order.Length == 0, true, x => x.CorrectionYear)
                         .Filter(loadParam, Container)
                         .Order(loadParam)
                         .ToList();

            }
            else
            {
                var dataCorrection =
                    correctionDomain.GetAll()
                                 .WhereIf(moId > 0, x => x.Stage2.Stage3Version.ProgramVersion.IsMain && x.Stage2.Stage3Version.ProgramVersion.Municipality.Id == moId)
                                 .WhereIf(versionId > 0, x => x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
                                 .Select(x => new { x.Stage2.Stage3Version.Id, x.PlanYear })
                                 .AsEnumerable()
                                 .GroupBy(x => x.Id)
                                 .ToDictionary(x => x.Key, y => y.Select(x => x.PlanYear).FirstOrDefault());

                var query =
                    versionRecord.GetAll()
                                 .Where(x => x.ProgramVersion.IsMain)
                                 .Where(x => x.ProgramVersion.Municipality.Id == moId)
                                 .Where(x => correctionDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                                 .Select(
                                     x =>
                                     new
                                     {
                                         x.Id,
                                         Municipality = x.RealityObject.Municipality.Name,
                                         RealityObject = x.RealityObject.Address,
                                         CorrectionYear = 0,
                                         PlanYear = x.Year,
                                         x.Sum,
                                         CommonEstateObjectName = x.CommonEstateObjects,
                                         x.IndexNumber
                                     })
                                 .AsEnumerable();

                return query.Select(
                        x =>
                            new
                            {
                                x.Id,
                                x.Municipality,
                                x.RealityObject,
                                CorrectionYear = dataCorrection.ContainsKey(x.Id) ? dataCorrection[x.Id] : 0,
                                x.PlanYear,
                                x.Sum,
                                x.CommonEstateObjectName,
                                x.IndexNumber
                            })
                           .AsQueryable()
                           .OrderIf(loadParam.Order.Length == 0, true, x => x.CorrectionYear)
                           .Filter(loadParam, Container)
                           .Order(loadParam)
                           .ToList();

            }
        }
    }
}