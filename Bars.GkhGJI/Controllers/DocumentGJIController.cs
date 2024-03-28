namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class DocumentGjiController: DocumentGjiController<DocumentGji>
    {
    }

    public class DocumentGjiController<T> : B4.Alt.DataController<T>
        where T: DocumentGji
    {
        /// <summary>
        /// Метод перевода статуса документа ГЖИ
        /// </summary>
        public ActionResult StateChange(BaseParams baseParams)
        {
            var service = Container.Resolve<IDocumentGjiService>();
            try
            {
                var result = service.StateChange(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
            
        }
    }
}