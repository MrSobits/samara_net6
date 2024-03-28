namespace Bars.GisIntegration.UI.ViewModel.GisRole
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Domain;
    using Bars.GisIntegration.Base.Entities.GisRole;

    /// <summary>
    /// View-модель RisContragentRole
    /// </summary>
    public class RisContragentRoleViewModel : BaseViewModel<RisContragentRole>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<RisContragentRole> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var operatorId = loadParams.Filter.GetAsId("operatorId");
            var data = domainService.GetAll()
                .WhereIf(operatorId != 0, x => x.GisOperator.Id == operatorId)
                .Select(x => new
                {
                    x.Id,
                    Role = x.Role.Name,
                    Contragent = x.GisOperator.Contragent.FullName
                })
                .Filter(loadParams, this.Container);

            var count = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), count);
        }
    }
}

