namespace Bars.GkhGji.Regions.Tomsk.Controller
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;

    using Bars.GkhGji.Regions.Tomsk.DomainService;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class ProtocolDefinitionController : Bars.GkhGji.Controllers.ProtocolDefinitionController<TomskProtocolDefinition>
    {

        // Метод получает значения по умолчанию
        // Предположим при создании Определения получают Дата и время из родительского протокола
        public ActionResult GetDefaultParams(BaseParams baseParams)
        {
            var service = Container.Resolve<IProtocolDefinitionDefaultParamsService>();

            try
            {
                var result = service.GetDefaultParams(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}
