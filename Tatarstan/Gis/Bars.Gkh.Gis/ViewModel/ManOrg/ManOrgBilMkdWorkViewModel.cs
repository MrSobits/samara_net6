namespace Bars.Gkh.Gis.ViewModel.ManOrg
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Gkh.Domain;
    using Entities.ManOrg;

    /// <summary>
    ///  ViewModel для работ и услуг управляющей организации по МКД
    /// </summary>
    public class ManOrgBilMkdWorkViewModel : BaseViewModel<ManOrgBilMkdWork>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<ManOrgBilMkdWork> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var serviceWorkId = loadParams.Filter.GetAsId("serviceWorkId");

            var data = domainService.GetAll()
                .Where(x => x.WorkService.Id == serviceWorkId)
                .Select(x => new
                {
                    x.Id,
                    x.MkdWork.Name,
                    x.MkdWork.Code,
                    x.MkdWork.Description
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}
