namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using Entities.ValueObjects;
    using Enums;
    using Gkh.Domain;

    /// <summary>
    /// Представление для <see cref="DistributionDetail"/>
    /// </summary>
    public class DistributionDetailViewModel : BaseViewModel<DistributionDetail>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<DistributionDetail> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var entityId = baseParams.Params.GetAsId("entityId");
            var source = baseParams.Params.GetAs<DistributionSource>("source");

            var data = domainService.GetAll()
                .Where(x => x.EntityId == entityId)
                .Where(x => x.Source == source)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}