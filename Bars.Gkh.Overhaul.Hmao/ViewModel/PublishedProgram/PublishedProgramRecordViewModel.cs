namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Gkh.Utils;

    /// <summary>
    /// Представление для сущности Запись Опубликованной программы
    /// </summary>
    public class PublishedProgramRecordViewModel : BaseViewModel<PublishedProgramRecord>
    {
        /// <summary>
        /// Домен-сервис для сущности Запись в версии программы
        /// </summary>
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        /// <inheritdoc/>
        public override IDataResult List(IDomainService<PublishedProgramRecord> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);

            var moId = 0M;
            var versionId = loadParam.Filter.GetAs<long>("versionId");
            if (versionId == 0)
            {
                moId = baseParams.Params.GetAs<long>("mo_id");
            }

            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;

            decimal summary;

            var baseQuery = domainService.GetAll()
                .WhereIf(moId > 0, x => x.PublishedProgram.ProgramVersion.IsMain && x.PublishedProgram.ProgramVersion.Municipality.Id == moId)
                .WhereIf(versionId > 0, x => x.PublishedProgram.ProgramVersion.Id == versionId);
            
            var versionProgramDict = this.VersionRecordDomain.GetAll()
                .WhereIf(moId > 0, x => x.ProgramVersion.IsMain && x.ProgramVersion.Municipality.Id == moId)
                .WhereIf(versionId > 0, x => x.ProgramVersion.Id == versionId)
                .Where(x => baseQuery.Any(y => y.Stage2.Stage3Version.Id == x.Id))
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObject = x.RealityObject.Address,
                    CommonEstateobject = x.CommonEstateObjects,
                    x.WorkCode
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.First());
            
            if (groupByRoPeriod == 0)
            {
                var data = baseQuery
                    .Select(x => new
                    {
                        x.Id,
                        StageId = x.Stage2.Stage3Version.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        RealityObject = x.RealityObject.Address,
                        x.PublishedYear,
                        x.Sum,
                        x.CommonEstateobject,
                        x.IndexNumber,
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.Municipality,
                        x.RealityObject,
                        x.PublishedYear,
                        x.Sum,
                        x.CommonEstateobject,
                        x.IndexNumber,
                        WorkCode = versionProgramDict.Get(x.StageId)?.WorkCode ?? string.Empty
                    })
                    .AsQueryable()
                    .Filter(loadParam, this.Container);

                summary = data.Select(x => x.Sum).Sum();
            
                return new ListSummaryResult(data.OrderBy(x => x.IndexNumber)
                    .ThenBy(x => x.PublishedYear)
                    .Order(loadParam)
                    .Paging(loadParam), data.Count(), new { Sum = summary });
            }

            var query = baseQuery
                .Where(x => x.Stage2 != null)
                .Select(x => new
                {
                    x.Id,
                    StageId = x.Stage2.Stage3Version.Id,
                    x.Sum,
                    x.IndexNumber,
                    x.PublishedYear
                })
                .AsEnumerable()
                .Select(x =>
                {
                    var versionRecord = versionProgramDict.Get(x.StageId);
                    return new
                    {
                        x.Id,
                        Municipality = versionRecord?.Municipality ?? string.Empty,
                        RealityObject = versionRecord?.RealityObject ?? string.Empty,
                        x.PublishedYear,
                        x.Sum,
                        CommonEstateobject = versionRecord?.CommonEstateobject ?? string.Empty,
                        x.IndexNumber,
                        WorkCode = versionRecord?.WorkCode ?? string.Empty
                    };
                })
                .AsQueryable()
                .Filter(loadParam, this.Container);

            summary = query.Select(x => x.Sum).Sum();
                
            return new ListSummaryResult(query
                .OrderIf(loadParam.Order.Length == 0, true, x => x.PublishedYear)
                .Order(loadParam)
                .Paging(loadParam), query.Count(), new { Sum = summary });
        }
    }
}