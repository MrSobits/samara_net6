namespace Bars.Gkh.Gis.ViewModel.ManOrg
{
    using System;
    using System.Linq;
    using B4;
    using B4.Utils;
    using B4.DataAccess;

    using Bars.Gkh.Domain;

    using Entities.Kp50;
    using Entities.ManOrg;

    /// <summary>
    /// ViewModel для коммунальной услуги управляющей организации
    /// </summary>
    public class ManOrgBilCommunalServiceViewModel : BaseViewModel<ManOrgBilCommunalService>
    {
        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<ManOrgBilCommunalService> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.Get(id);

            return obj != null
                ? new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.BilService,
                        MoId = obj.ManagingOrganization.Id,
                        obj.Name,
                        obj.Resource,
                        obj.BilService.OrderNumber,
                        obj.BilService.IsOdnService
                    })
                : new BaseDataResult();
        }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<ManOrgBilCommunalService> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var moId = loadParams.Filter.GetAsId("manorgId");

            var result = domainService.GetAll()
                .Where(x => x.ManagingOrganization != null && x.ManagingOrganization.Id == moId)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Resource,
                    x.BilService.ServiceName,
                    x.BilService.IsOdnService,
                    x.BilService.OrderNumber
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);

            return new ListDataResult(result.Order(loadParams).Paging(loadParams), result.Count());
        }
    }
}
