namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    // Пустышка на случай если от этого класса наледовались в регионах
    // Вообщем такая пустышка сделана для того чтобы перекрыт ьв другом регионе, наслучай если ктото будет расширять данный контроллер для Определенийй Протокола
    // Внимание от этого контроллера наследуется контроллерв в Томском Гжи
    public class ProtocolDefinitionController : ProtocolDefinitionController<ProtocolDefinition>
    {
        // Внимание все методы писать в Generic
    }

    // Generic Класс потмочучто данная сущност ьв регионах расширяется через subclass 
    // Чтобы не дублирваоть функционал контроллеров все методы писать суда
    // FileStorageDataController делаю потомучто в регионах данный контроллер расширяется полями с Файлами
    public class ProtocolDefinitionController<T> : FileStorageDataController<T>
        where T : ProtocolDefinition
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