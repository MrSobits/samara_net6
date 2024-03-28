namespace Bars.GisIntegration.UI.ViewModel.Delegacy
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Domain;
    using Bars.GisIntegration.Base.Entities.Delegacy;

    /// <summary>
    /// View-модель делегирования
    /// </summary>
    public class DelegacyViewModel : BaseViewModel<Delegacy>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<Delegacy> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var operatorIsId = loadParams.Filter.GetAsId("operatorIsId");
            var data = domainService.GetAll()
                .WhereIf(operatorIsId != 0, x => x.OperatorIS.Id == operatorIsId)
                .Select(x => new
                {
                    x.Id,
                    OperatorIS = x.OperatorIS.FullName,
                    InformationProvider = x.InformationProvider.FullName,
                    x.StartDate,
                    x.EndDate
                })
                .Filter(loadParams, this.Container);

            var count = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), count);
        }
    }
}

