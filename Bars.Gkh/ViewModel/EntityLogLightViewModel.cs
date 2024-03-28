namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.Entities;

    public class EntityLogLightViewModel : BaseViewModel<EntityLogLight>
    {
        public override IDataResult List(IDomainService<EntityLogLight> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var entityId = baseParams.Params.GetAs("entityId", 0L);
            var className = baseParams.Params.GetAs("className", string.Empty);
            var parameter = baseParams.Params.GetAs("parameter", string.Empty);
            var parametersString = baseParams.Params.GetAs("parameters", string.Empty);

            var parameters = parametersString.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            var data =
                domainService.GetAll()
                             .Where(x => x.EntityId == entityId)
                             .Where(x => x.ParameterName == parameter || parameters.Contains(x.ParameterName))
                             .WhereIf(!string.IsNullOrEmpty(className), x => x.ClassName == className)
                             .Select(
                                 x =>
                                 new
                                     {
                                         x.Id,
                                         x.ParameterName,
                                         x.PropertyDescription,
                                         x.PropertyValue,
                                         x.DateActualChange,
                                         x.User,
                                         x.DateApplied,
                                         x.Document
                                     });

            var filteredData =
                data.Paging(loadParams)
                    .AsEnumerable()
                    .Select(
                        x =>
                        new
                            {
                                x.Id,
                                ParameterName = VersionedEntityHelper.GetFriendlyName(x.ParameterName),
                                PropertyDescription = x.PropertyDescription ?? string.Empty,
                                PropertyValue = VersionedEntityHelper.RenderValue(x.ParameterName, x.PropertyValue),
                                x.DateActualChange,
                                x.User,
                                DateApplied = x.DateApplied.GetValueOrDefault(),
                                x.Document
                            })
                    .AsQueryable()
                    .Filter(loadParams, Container) //Фильтрация в памяти из-за VersionedEntityHelper.GetFriendlyName
                    .Order(loadParams);

            var total = filteredData.Count();

            var result = filteredData.Paging(loadParams);

            return new ListDataResult(result, total);
        }
    }
}