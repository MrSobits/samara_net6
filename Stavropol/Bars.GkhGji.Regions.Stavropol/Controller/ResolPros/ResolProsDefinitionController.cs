using System.Collections;
using Bars.B4;
using Bars.B4.Modules.FileStorage;
using Bars.GkhGji.Regions.Stavropol.DomainService.ResolPros;
using Bars.GkhGji.Regions.Stavropol.Entities;

namespace Bars.GkhGji.Regions.Stavropol.Controller.ResolPros
{
	using Microsoft.AspNetCore.Mvc;

	public class ResolProsDefinitionController : ResolProsDefinitionController<ResolProsDefinition>
    {
        // Внимание все методы писать в Generic
    }

    // Generic Класс потмочучто данная сущност ьв регионах расширяется через subclass 
    // Чтобы не дублирваоть функционал контроллеров все методы писать суда
    // FileStorageDataController делаю потомучто в регионах данный контроллер расширяется полями с Файлами
	public class ResolProsDefinitionController<T> : FileStorageDataController<T>
		where T : ResolProsDefinition
    {
        public ActionResult ListTypeDefinition(BaseParams baseParams)
        {
			var service = Container.Resolve<IResolProsDefinitionService>();
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