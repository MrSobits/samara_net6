namespace Bars.Gkh.Gis.ViewModel.ManOrg.Contract
{
    using B4;
    using Gkh.Domain;
    using Entities.ManOrg.Contract;
    using System.Linq;

    public class ContractOwnersWorkServiceViewModel : BaseViewModel<ContractOwnersWorkService>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<ContractOwnersWorkService> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var contractId = loadParams.Filter.GetAsId("contractId");

            var data = domainService.GetAll()
                .Where(x => x.Contract != null && x.Contract.Id == contractId)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.WorkService,
                        Name = x.WorkService.BilService.ServiceName,
                        x.WorkService.Type,
                        x.PaymentAmount
                    })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<ContractOwnersWorkService> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.Get(id);

            return obj != null ? new BaseDataResult(new
            {
                obj.Id,
                obj.Contract,
                WorkService = new
                {
                    obj.WorkService.BilService.ServiceName,
                    obj.WorkService.Id,
                    obj.WorkService.Type
                },
                obj.PaymentAmount
            }) : new BaseDataResult();
        }
    }
}
