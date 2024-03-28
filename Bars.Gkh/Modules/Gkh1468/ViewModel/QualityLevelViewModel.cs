namespace Bars.Gkh.Modules.Gkh1468.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Представление для показателя качества
    /// </summary>
    public class PublicOrgServiceQualityLevelViewModel : BaseViewModel<PublicOrgServiceQualityLevel>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<PublicOrgServiceQualityLevel> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var serviceId = loadParams.Filter.GetAsId("serviceId");

            var data = domainService.GetAll()
                .Where(x => x.ServiceOrg.Id == serviceId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Value,
                    UnitMeasure = x.UnitMeasure.Name
                })
                .Filter(loadParams, this.Container);

            var count = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), count);
        }
    }
}
