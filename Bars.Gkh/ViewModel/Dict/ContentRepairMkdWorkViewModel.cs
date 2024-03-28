namespace Bars.Gkh.ViewModel.Dict
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Dicts;
    using System.Linq;

    /// <summary>
    /// View-модель для справочника "Работы по содержанию и ремонту МКД"
    /// </summary>
    class ContentRepairMkdWorkViewModel : BaseViewModel<ContentRepairMkdWork>
    {
        /// <summary>
        /// Получить список записей справочника "Работы по содержанию и ремонту МКД"
        /// </summary>
        /// <param name="domain">Доменный сервис сущности ContentRepairMkdWork</param>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции со списком записей</returns>
        public override IDataResult List(IDomainService<ContentRepairMkdWork> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var organizationWorkId = baseParams.Params.GetAs<long>("organizationWorkId");

            var excludeOrganizationWorkId = baseParams.Params.GetAs<long>("excludeOrganizationWorkId");

            var data = domain.GetAll()
                .WhereIf(organizationWorkId > 0, x => x.Work != null && x.Work.Id == organizationWorkId)
                .WhereIf(excludeOrganizationWorkId > 0, x => x.Work == null || x.Work.Id != excludeOrganizationWorkId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.Description
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
