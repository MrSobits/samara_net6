namespace Bars.Gkh.Gis.ViewModel.Dict.Service
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Entities.Kp50;

    /// <summary>
    /// Модель представления для справочника услуг биллинга
    /// </summary>
    public class BilServiceDictionaryViewModel : BaseViewModel<BilServiceDictionary>
    {
        /// <summary>
        /// Получить список справочника услуг биллинга
        /// </summary>
        /// <param name="domainService">Домен-сервис</param>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>список справочника услуг биллинга</returns>
        public override IDataResult List(IDomainService<BilServiceDictionary> domainService, BaseParams baseParams)
        {
            var baseParamsServiceId = baseParams.Params.GetAs<long?>("serviceId");
            var loadParams = this.GetLoadParam(baseParams);
            var loadParamsServiceId = loadParams.Filter.GetAs<long?>("serviceId");

            var data = domainService.GetAll()
                .WhereIf(baseParamsServiceId != null, x => x.Service.Id == baseParamsServiceId)
                .WhereIf(loadParamsServiceId != null, x => x.Service.Id == loadParamsServiceId)
                .Select(
                    x => new
                    {
                        x.Id,
                        DataBank = x.Schema.Description,
                        Organization = x.Schema.LocalSchemaPrefix == null || x.Schema.LocalSchemaPrefix == ""
                            ? x.Schema.Description
                            : x.Schema.Description == null || x.Schema.Description == ""
                                ? "(" + x.Schema.LocalSchemaPrefix + ")"
                                : x.Schema.Description + " (" + x.Schema.LocalSchemaPrefix + ")",
                        ServiceName = x.ServiceName ?? string.Empty,
                        x.ServiceCode,
                        x.ServiceTypeName,
                        RelatedService = x.Service == null
                            ? string.Empty
                            : x.Service.Name ?? string.Empty
                    })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Organization)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.ServiceName)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}