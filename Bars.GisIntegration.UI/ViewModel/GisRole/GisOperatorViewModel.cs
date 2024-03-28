namespace Bars.GisIntegration.UI.ViewModel.GisRole
{
    using System.Linq;

    using Bars.B4;
    using Bars.GisIntegration.Base.Entities.GisRole;

    /// <summary>
    /// View-модель GisOperator
    /// </summary>
    public class GisOperatorViewModel : BaseViewModel<GisOperator>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<GisOperator> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Contragent = x.Contragent.FullName
                })
                .Filter(loadParams, this.Container);

            var count = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), count);
        }
    }
}

