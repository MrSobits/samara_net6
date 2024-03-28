namespace Bars.Gkh.Repair.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Repair.Enums;

    public class RepairProgramViewModel : BaseViewModel<RepairProgram>
    {
        public override IDataResult List(IDomainService<RepairProgram> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var ids = baseParams.Params.GetAs("Id", string.Empty);
            var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var forRepairObject = baseParams.Params.GetAs("forRepairObject", false);

            var data = domainService.GetAll()
                .WhereIf(listIds.Length > 0, x => listIds.Contains(x.Id))
                .WhereIf(
                    forRepairObject,
                    x => x.TypeVisibilityProgramRepair != TypeVisibilityProgramRepair.Hidden
                    && x.TypeVisibilityProgramRepair != TypeVisibilityProgramRepair.Print)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    PeriodName = x.Period.Name,
                    x.TypeProgramRepairState,
                    x.TypeVisibilityProgramRepair
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}
