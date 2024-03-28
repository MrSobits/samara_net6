namespace Bars.Gkh.ClaimWork.Controllers
{
    using System.ComponentModel;

    using Bars.B4;
    using Bars.B4.IoC;

    using Microsoft.AspNetCore.Mvc;
    using DomainService.DocumentRegister;

    /// <summary>
    /// Контроллер реестра документов ПИР
    /// </summary>
    public class DocumentRegisterController : BaseController
    {
        /// <summary>
        /// Список типов документов
        /// </summary>
        /// <returns></returns>
        public ActionResult ListTypeDocument()
        {
            var service = Resolve<IDocumentRegisterService>();

            using (Container.Using(service))
            {
                var result = service.ListTypeDocument();

                return new JsonListResult(result, result.Count);
            }
        }
    }
}