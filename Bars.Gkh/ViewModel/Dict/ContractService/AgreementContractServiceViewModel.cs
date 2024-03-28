namespace Bars.Gkh.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities.Dicts.ContractService;

    /// <summary>
    /// Вьюмодель для "AgreementContractService"
    /// </summary>
    public class AgreementContractServiceViewModel : BaseViewModel<AgreementContractService>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<AgreementContractService> domainService, BaseParams baseParams)
        {
            LoadParam loadParam = this.GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(
                    x => new
                    {
                        x.Id,
                        x.Code,
                        x.Name,
                        x.TypeWork,
                        x.WorkAssignment,
                        UnitMeasureName = x.UnitMeasure.Name
                    })
                .Filter(loadParam, this.Container)
                .Order(loadParam);

            var count = data.Count();

            return new ListDataResult(data.Paging(loadParam).ToList(), count); 
        }
    }
}