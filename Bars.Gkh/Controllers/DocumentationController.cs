namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.DomainService.Documentation;

    /// <summary>
    /// Контроллер работы с документацией
    /// </summary>
    public class DocumentationController: BaseController
    {
        /// <summary>
        /// Вовзаращет url-адрес документации
        /// </summary>
        /// <returns>Url-адрес документации</returns>
        public ActionResult GetDocumentationUrl()
        {
            var result = Container.Resolve<IDocumentationService>().GetDocumentationUrl();
            return new JsonNetResult(result);
        }
    }
}
