namespace Bars.GkhGji.Regions.BaseChelyabinsk.Controllers.Protocol197
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    // Пустышка на случай если от этого класса наледовались в регионах
    // Вообщем такая пустышка сделана для того чтобы перекрыт ьв другом регионе, наслучай если ктото будет расширять данный контроллер для Определенийй Протокола
    // Внимание от этого контроллера наследуется контроллерв в Томском Гжи
    public class Protocol197DefinitionController : FileStorageDataController<Protocol197Definition>
    {
        public ActionResult ListTypeDefinition(BaseParams baseParams)
        {
            var service = Container.Resolve<IProtocolDefinitionService>();
            try
            {
                var result = (ListDataResult)service.ListTypeDefinition(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }


}