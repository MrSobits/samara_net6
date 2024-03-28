namespace Bars.Gkh.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Dicts.ContractService;

    /// <summary>
    /// Вьюмодель для "CommunalContractService"
    /// </summary>
    public class CommunalContractServiceViewModel : BaseViewModel<CommunalContractService>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<CommunalContractService> domainService, BaseParams baseParams)
        {
            LoadParam loadParam = this.GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Name,
                        x.CommunalResource,
                        x.IsHouseNeeds                        
                    })
                .Filter(loadParam, this.Container)
                .Order(loadParam);

            var count = data.Count();

            return new ListDataResult(data.Paging(loadParam).ToList(), count); 
        }
    }
}