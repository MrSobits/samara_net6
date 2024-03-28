namespace Bars.GkhCr.Regions.Tatarstan.ViewModel.RealityObjectOutdoorProgram
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram;

    public class RealityObjectOutdoorProgramViewModel : BaseViewModel<RealityObjectOutdoorProgram>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<RealityObjectOutdoorProgram> domainService, BaseParams baseParams)
        {
            var needOnlyWithFullVisibility = baseParams.Params.GetAs<bool>("needOnlyWithFullVisibility");
            var selectedIds = baseParams.Params.GetAs<string>("Id")
                .ToLongArray()
                .Where(x => x != 0)
                .ToList();

            return domainService.GetAll()
                .WhereIf(needOnlyWithFullVisibility, x => x.TypeVisibilityProgram == TypeVisibilityProgramCr.Full)
                .WhereIf(selectedIds.Any(), x => selectedIds.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    PeriodName = x.Period.Name,
                    x.TypeVisibilityProgram,
                    x.TypeProgramState
                }).ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
