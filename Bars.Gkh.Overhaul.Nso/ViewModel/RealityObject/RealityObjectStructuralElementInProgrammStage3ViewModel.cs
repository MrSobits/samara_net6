namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class RealityObjectStructuralElementInProgrammStage3ViewModel : BaseViewModel<RealityObjectStructuralElementInProgrammStage3>
    {
        public override IDataResult List(IDomainService<RealityObjectStructuralElementInProgrammStage3> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObject = x.RealityObject.Address,
                    x.CommonEstateObjects,
                    x.Year,
                    x.IndexNumber,
                    x.Point,
                    x.Sum
                })
                .Filter(loadParams, Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.IndexNumber);

            var summary = data.AsEnumerable().Sum(x => x.Sum);

            return new ListSummaryResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count(), new { Sum = summary });
        }
    }
}