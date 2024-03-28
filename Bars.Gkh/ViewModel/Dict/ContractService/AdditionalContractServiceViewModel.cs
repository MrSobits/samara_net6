namespace Bars.Gkh.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Dicts.ContractService;

    /// <summary>
    /// Вьюмодель для "AdditionalContractService"
    /// </summary>
    public class AdditionalContractServiceViewModel : BaseViewModel<AdditionalContractService>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<AdditionalContractService> domainService, BaseParams baseParams)
        {
            LoadParam loadParam = this.GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Name,
                        UnitMeasureName = x.UnitMeasure.Name
                    })
                .Filter(loadParam, this.Container)
                .Order(loadParam);

            var count = data.Count();

            return new ListDataResult(data.Paging(loadParam).ToList(), count); 
        }
    }
}