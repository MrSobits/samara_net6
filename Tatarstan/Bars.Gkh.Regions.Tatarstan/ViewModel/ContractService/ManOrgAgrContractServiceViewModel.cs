namespace Bars.Gkh.Regions.Tatarstan.ViewModel.ContractService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts.ContractService;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Вьюмодель для "ManOrgAgrContractService"
    /// </summary>
    public class ManOrgAgrContractServiceViewModel : BaseViewModel<ManOrgAgrContractService>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ManOrgAgrContractService> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var contractId = loadParams.Filter.GetAsId("contractId");

            return domainService.GetAll()
                .Where(x => x.Contract.Id == contractId)
                .Select(x => new
                {
                    x.Id,
                    x.Service.Name,
                    Type = (TypeWork?)(x.Service as AgreementContractService).TypeWork ?? TypeWork.NotSet,
                    x.PaymentAmount,
                    x.StartDate,
                    x.EndDate
                })
                .ToListDataResult(loadParams);
        }
    }
}