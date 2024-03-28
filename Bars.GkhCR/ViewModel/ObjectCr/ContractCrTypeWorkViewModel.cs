namespace Bars.GkhCr.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.DataResult;
    using Bars.GkhCr.Entities;

    public class ContractCrTypeWorkViewModel : BaseViewModel<ContractCrTypeWork>
    {
        public override IDataResult List(IDomainService<ContractCrTypeWork> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var contractCrId = loadParams.Filter.GetAs<long>("contractCrId");

            var data = domainService.GetAll()
                .Where(x => x.ContractCr.Id == contractCrId)
                .Select(
                    x => new
                    {
                        x.Id,
                        TypeWork = x.TypeWork.Work.Name,
                        x.Sum
                    })
                .Filter(loadParams, this.Container);

            var summary = data.Sum(x => x.Sum);
            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListSummaryResult(data.ToList(), totalCount, new {Sum = summary});
        }
    }
}