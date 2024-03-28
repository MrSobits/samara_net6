namespace Bars.Gkh.Modules.Gkh1468.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Представление для <see cref="PublicServiceOrgContractService"/>
    /// </summary>
    public class PublicServiceOrgContractServiceViewModel : BaseViewModel<PublicServiceOrgContractService>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<PublicServiceOrgContractService> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var publicServOrgId = loadParams.Filter.GetAsId("servOrgContractId");
            var data = domainService.GetAll()
                .Where(x => x.ResOrgContract.Id == publicServOrgId)
                .Select(x => new
                {
                    x.Id,
                    Service = x.Service.Name,
                    CommunalResource = x.CommunalResource.Name,
                    x.HeatingSystemType,
                    x.SchemeConnectionType,
                    x.StartDate,
                    x.EndDate
                })
                .Filter(loadParams, this.Container);

            var count = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), count);
        }
    }
}
