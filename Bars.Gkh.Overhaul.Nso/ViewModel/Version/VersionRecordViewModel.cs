namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;

    using Entities;
    using Gkh.Utils;

    public class VersionRecordViewModel : BaseViewModel<VersionRecord>
    {
        public override IDataResult List(IDomainService<VersionRecord> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var version = loadParams.Filter.GetAs<long>("version");

            if (loadParams.Order == null || loadParams.Order.Length == 0)
            {
                loadParams.Order = new[] { new OrderField { Asc = true, Name = "IndexNumber" } };
            }

            var data = domainService.GetAll()
                .Where(x => x.ProgramVersion.Id == version)
                .Select(x => new
                    {
                        x.Id,
                        Municipality = x.RealityObject.Municipality.Name,
                        RealityObject = x.RealityObject.Address,
                        x.CommonEstateObjects,
                        x.Year,
                        x.IndexNumber,
                        x.Point,
                        x.Sum,
                        x.Changes,
                        x.IsChangedYear
                    })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<VersionRecord> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var periodStart = config.ProgrammPeriodStart;
            var periodEnd = config.ProgrammPeriodEnd;

            return new BaseDataResult(
                domainService.GetAll()
                    .Where(x => x.Id == id)
                    .Select(x => new
                    {
                        x.ProgramVersion,
                        x.RealityObject,
                        x.Id,
                        x.Year,
                        x.Sum,
                        x.CommonEstateObjects,
                        x.Point,
                        x.IndexNumber,
                        x.StoredCriteria,
                        x.StoredPointParams,
                        x.IsChangedYear,
                        x.File,
                        x.DocumentName,
                        x.DocumentNum,
                        x.DocumentDate,
                        PeriodStart = periodStart,
                        PeriodEnd = periodEnd
                    })
                    .FirstOrDefault()
                );
        }
    }
}