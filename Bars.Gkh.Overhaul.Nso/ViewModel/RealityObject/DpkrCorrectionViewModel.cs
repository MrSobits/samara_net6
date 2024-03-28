namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DataResult;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class DpkrCorrectionViewModel : BaseViewModel<DpkrCorrectionStage2>
    {
        public override IDataResult List(IDomainService<DpkrCorrectionStage2> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            
            var data = domainService.GetAll()
                .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                .Select(x => new
                {
                    Municipality = x.RealityObject.Municipality.Name,
                    RealityObject = x.RealityObject.Address,
                    CorrectionYear = x.PlanYear,
                    PlanYear = x.Stage2.Stage3Version.Year,
                    x.Stage2.Sum,
                    CommonEstateObjectName = x.Stage2.CommonEstateObject.Name,
                    x.Stage2.Stage3Version.IndexNumber
                })
                .OrderIf(loadParam.Order.Length == 0, true, x => x.CorrectionYear)
                .Filter(loadParam, Container);

            var summary = data.AsEnumerable().Sum(x => x.Sum);

            return new ListSummaryResult(data.Order(loadParam).Paging(loadParam), data.Count(), new { Sum = summary });
        }
    }
}