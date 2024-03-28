namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Hmao.DomainService.Version;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Controller for <see cref="DpkrDocumentRealityObject"/>
    /// </summary>
    public class DpkrDocumentRealityObjectController : B4.Alt.DataController<DpkrDocumentRealityObject>
    {
        /// <summary>
        /// Добавление домов
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult AddRealityObjects(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDpkrDocumentRealityObjectService>();

            try
            {
                return service.AddRealityObjects(baseParams).ToJsonResult();
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}