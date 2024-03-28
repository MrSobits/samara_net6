namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;
    using Gkh.Utils;

    public class BuildContractTerminationViewModel : BaseViewModel<BuildContractTermination>
    {
        public override IDataResult List(IDomainService<BuildContractTermination> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var buildContractId = loadParams.Filter.GetAsId("buildContractId");

            var data = domainService.GetAll()
                .Where(x => x.BuildContract.Id == buildContractId)
                .Select(x => new
                {
                    x.Id,
                    x.TerminationDate,
                    x.DocumentFile,
                    x.Reason,
                    x.DictReason,
                    x.DocumentNumber
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}