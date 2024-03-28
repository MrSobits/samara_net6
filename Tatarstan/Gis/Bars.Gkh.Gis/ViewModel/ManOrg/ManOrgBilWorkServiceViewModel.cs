namespace Bars.Gkh.Gis.ViewModel.ManOrg
{
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.Domain;

    using Entities.ManOrg;

    /// <summary>
    /// ViewModel для работ и услуг управляющей организации
    /// </summary>
    public class ManOrgBilWorkServiceViewModel : BaseViewModel<ManOrgBilWorkService>
    {
        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<ManOrgBilWorkService> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.Get(id);

            return obj != null ? new BaseDataResult(new
            {
                obj.Id,
                MoId = obj.ManagingOrganization.Id,
                obj.BilService,
                obj.BilService.MeasureName,
                obj.BilService.ServiceCode,
                obj.BilService.ServiceName,
                obj.Purpose,
                obj.Type,
                obj.Description
            }) : new BaseDataResult();
        }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<ManOrgBilWorkService> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var moId = loadParams.Filter.GetAsId("manorgId");

            var data = domainService.GetAll()
                .Where(x => x.ManagingOrganization != null && x.ManagingOrganization.Id == moId)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.BilService.ServiceName,
                        x.BilService.MeasureName,
                        x.BilService.ServiceCode,
                        Purpose = x.Purpose.ToString(),
                        Type = x.Type.ToString(),
                        x.Description
                    })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}