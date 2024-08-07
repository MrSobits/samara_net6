﻿namespace Bars.Gkh.Gis.ViewModel.ManOrg.Contract
{
    using B4;
    using Gkh.Domain;
    using Entities.ManOrg.Contract;
    using System.Linq;

    public class ContractOwnersCommServiceViewModel : BaseViewModel<ContractOwnersCommService>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<ContractOwnersCommService> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var contractId = loadParams.Filter.GetAsId("contractId");

            var data = domainService.GetAll()
                .Where(x => x.Contract != null && x.Contract.Id == contractId)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.CommunalService.Name,
                        x.StartDate,
                        x.EndDate
                    })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}
