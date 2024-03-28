namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    /// <summary>
    /// Вьюха для <see cref="ManagingOrgMunicipality"/>
    /// </summary>
    public class ManagingOrgMunicipalityViewModel : BaseViewModel<ManagingOrgMunicipality>
    {
        /// <summary>
        /// Вернуть список сущностей
        /// </summary>
        /// <param name="domain">Домен-сервис "Связь управляющей организации с МО"</param>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>результат операции</returns>
        public override IDataResult List(IDomainService<ManagingOrgMunicipality> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var manorgId = baseParams.Params.GetAs<long>("manorgId");

            var data = domain.GetAll()
                .Where(x => x.ManOrg.Id == manorgId)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    ParentMo = x.Municipality.ParentMo.Name
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}