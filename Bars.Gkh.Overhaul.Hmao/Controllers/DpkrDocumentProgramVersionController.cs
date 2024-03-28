namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Hmao.DomainService.Version;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Controller for <see cref="DpkrDocumentProgramVersion"/>
    /// </summary>
    public class DpkrDocumentProgramVersionController : B4.Alt.DataController<DpkrDocumentProgramVersion>
    {
        /// <summary>
        /// Список версий программ
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetProgramVersionList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDpkrDocumentProgramVersionService>();

            try
            {
                return service.GetProgramVersionList(baseParams).ToJsonResult();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Добавление ссылок на версии
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult AddProgramVersions(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDpkrDocumentProgramVersionService>();

            try
            {
                return service.AddProgramVersions(baseParams).ToJsonResult();
            }
            finally
            {
                this.Container.Release(service);
            }
        }
    }
}