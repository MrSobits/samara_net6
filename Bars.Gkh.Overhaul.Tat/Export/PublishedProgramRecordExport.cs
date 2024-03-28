namespace Bars.Gkh.Overhaul.Tat.Export
{
    using System.Collections;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;

    public class PublishedProgramRecordExport : BaseDataExportService
    {
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public IDomainService<PublishedProgramRecord> PublishedProgramRecordDomain { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);
            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;

            var moId = baseParams.Params.GetAs<long>("mo_id");

            if (groupByRoPeriod == 0)
            {
                // Поулчаем опубликованную программу по основной версии
                return PublishedProgramRecordDomain.GetAll()
                    .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                    .Where(x => x.PublishedProgram.ProgramVersion.Municipality.Id == moId)
                    .Select(x => new
                    {
                        x.Stage2.Id,
                        Municipality = x.Stage2.Stage3Version.RealityObject.Municipality.Name,
                        RealityObject = x.Stage2.Stage3Version.RealityObject.Address,
                        x.Stage2.Sum,
                        CommonEstateobject = x.Stage2.CommonEstateObject.Name,
                        x.PublishedYear,
                        x.Stage2.Stage3Version.IndexNumber
                    })
                    .OrderBy(x => x.IndexNumber)
                    .ThenBy(x => x.PublishedYear)
                    .Filter(loadParam, Container)
                    .Order(loadParam)
                    .ToList();
            }

            var dataPublished =
                this.PublishedProgramRecordDomain.GetAll()
                    .Where(
                        x =>
                        x.PublishedProgram.ProgramVersion.IsMain
                        && x.PublishedProgram.ProgramVersion.Municipality.Id == moId)
                    .Select(x => new { x.Stage2.Stage3Version.Id, x.PublishedYear })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.PublishedYear).FirstOrDefault());

            var query =
                this.VersionRecordDomain.GetAll()
                    .Where(x => x.ProgramVersion.IsMain)
                    .Where(x => x.ProgramVersion.Municipality.Id == moId)
                    .Where(x => this.PublishedProgramRecordDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                    .Select(
                        x =>
                        new
                            {
                                x.Id,
                                Municipality = x.RealityObject.Municipality.Name,
                                RealityObject = x.RealityObject.Address,
                                PublishedYear = 0,
                                x.Sum,
                                CommonEstateobject = x.CommonEstateObjects,
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
                        x.Sum,
                        x.CommonEstateobject,
                        PublishedYear = dataPublished.ContainsKey(x.Id) ? dataPublished[x.Id] : 0,
                        x.IndexNumber
                    })
                .AsQueryable()
                .OrderIf(loadParam.Order.Length == 0, true, x => x.PublishedYear)
                .Filter(loadParam, this.Container)
                .Order(loadParam)
                .ToList();
        }
    }
}