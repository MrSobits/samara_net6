namespace Bars.GisIntegration.UI.ViewModel.GisRole
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Domain;
    using Bars.GisIntegration.Base.Entities.GisRole;

    /// <summary>
    /// View-модель GisRoleMethod
    /// </summary>
    public class GisRoleMethodViewModel : BaseViewModel<GisRoleMethod>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<GisRoleMethod> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var roleId = loadParams.Filter.GetAsId("roleId");
            var data = domainService.GetAll()
                .WhereIf(roleId != 0, x => x.Role.Id == roleId)
                .Select(x => new
                {
                    x.Id,
                    Role = x.Role.Name,
                    x.MethodId,
                    x.MethodName
                })
                .Filter(loadParams, this.Container);

            var count = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), count);
        }
    }
}

