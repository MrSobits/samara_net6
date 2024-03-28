namespace Bars.GisIntegration.UI.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Entities.Delegacy;
    using Bars.GisIntegration.UI.Service;

    /// <summary>
    /// Контроллер делегироваия
    /// </summary>
    public class DelegacyController : B4.Alt.DataController<Delegacy>
    {
        /// <summary>
        /// Добавить поставщиков информации
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult AddInformationProviders(BaseParams baseParams)
        {
            var delegacyService = this.Container.Resolve<IDelegacyService>();

            try
            {
                return new JsonNetResult(delegacyService.AddInformationProviders(baseParams));
            }
            finally
            {
                this.Container.Release(delegacyService);
            }
        }
    }
}
