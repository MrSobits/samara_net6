namespace Bars.Gkh.Overhaul.Tat.ViewModel.PublishedProgram
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;

    public class PublishedProgramRecordViewModel : BaseViewModel<PublishedProgramRecord>
    {
        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public override IDataResult List(IDomainService<PublishedProgramRecord> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var moId = baseParams.Params.GetAs<long>("mo_id");
            var versionId = loadParam.Filter.GetAs<long>("versionId");

            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            var groupByRoPeriod = config.GroupByRoPeriod;

            decimal summary;
            int cnt;

            if (groupByRoPeriod == 0)
            {
                // Поулчаем опубликованную программу по основной версии
                var newData =
                    domainService.GetAll()
                        .WhereIf(moId > 0, x => x.PublishedProgram.ProgramVersion.IsMain && x.PublishedProgram.ProgramVersion.Municipality.Id == moId)
                        .WhereIf(versionId > 0, x => x.PublishedProgram.ProgramVersion.Id == versionId)
                        .Select(x => new
                        {
                            x.Stage2.Id,
                            Municipality = x.Stage2.Stage3Version.RealityObject.Municipality.Name,
                            RealityObject = x.Stage2.Stage3Version.RealityObject.Address,
                            x.PublishedYear,
                            x.Stage2.Sum,
                            CommonEstateobject = x.Stage2.CommonEstateObject.Name,
                            x.Stage2.Stage3Version.IndexNumber
                        })
                        .Filter(loadParam, Container);

                summary = newData.Select(x => x.Sum).AsEnumerable().Sum();

                cnt = newData.Count();

                newData = newData
                    .OrderBy(x => x.Municipality)
                    .ThenBy(x => x.IndexNumber)
                    .ThenBy(x => x.PublishedYear)
                    .Order(loadParam)
                    .Paging(loadParam);

                return new ListSummaryResult(newData, cnt, new { Sum = summary });
            }
            else
            {
                var dataPublished =
                    domainService.GetAll()
                        .WhereIf(moId > 0, x => x.PublishedProgram.ProgramVersion.IsMain && x.PublishedProgram.ProgramVersion.Municipality.Id == moId)
                        .WhereIf(versionId > 0, x => x.PublishedProgram.ProgramVersion.Id == versionId)
                        .Select(x => new { x.Stage2.Stage3Version.Id, x.PublishedYear })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.PublishedYear).FirstOrDefault());

                var query =
                    VersionRecordDomain.GetAll()
                        .WhereIf(moId > 0, x => x.ProgramVersion.IsMain && x.ProgramVersion.Municipality.Id == moId)
                        .WhereIf(versionId > 0, x => x.ProgramVersion.Id == versionId)
                        .Where(x => domainService.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                        .Select(x => new
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

                var newData =
                    query
                        .Select(x => new
                        {
                            x.Id,
                            x.Municipality,
                            x.RealityObject,
                            PublishedYear = dataPublished.ContainsKey(x.Id) ? dataPublished[x.Id] : 0,
                            x.Sum,
                            x.CommonEstateobject,
                            x.IndexNumber
                        })
                        .AsQueryable()
                        .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                        .OrderThenIf(loadParam.Order.Length == 0, true, x => x.IndexNumber)
                        .OrderThenIf(loadParam.Order.Length == 0, true, x => x.PublishedYear)
                        .Filter(loadParam, Container);

                summary = newData.Sum(x => x.Sum);
                cnt = newData.Count();

                return new ListSummaryResult(newData.Order(loadParam).Paging(loadParam), cnt, new { Sum = summary });
            }
        }
    }
}