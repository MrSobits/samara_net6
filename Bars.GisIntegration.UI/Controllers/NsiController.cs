namespace Bars.GisIntegration.UI.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Controllers;
    using Bars.GisIntegration.UI.Service;

    /// <summary>
    /// Контроллер для получения объектов при выполнении импорта/экспорта данных через сервис Nsi
    /// </summary>
    public class NsiController : BaseDataSupplierController
    {
        /// <summary>
        /// Получить список дополнительных услуг
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetAdditionalServices(BaseParams baseParams)
        {
            var nsiService = this.Container.Resolve<INsiService>();

            try
            {
                var result = (ListDataResult)nsiService.GetAdditionalServices(baseParams);
                return new JsonListResult((IEnumerable)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(nsiService);
            }
        }

        /// <summary>
        /// Получить список коммунальных услуг
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetMunicipalServices(BaseParams baseParams)
        {
            var nsiService = this.Container.Resolve<INsiService>();

            try
            {
                var result = (ListDataResult)nsiService.GetMunicipalServices(baseParams);
                return new JsonListResult((IEnumerable)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(nsiService);
            }
        }

        /// <summary>
        /// Получить список работ и услуг
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult GetOrganizationWorks(BaseParams baseParams)
        {
            var nsiService = this.Container.Resolve<INsiService>();

            try
            {
                var result = (ListDataResult)nsiService.GetOrganizationWorks(baseParams);
                return new JsonListResult((IEnumerable)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(nsiService);
            }
        }
    }
}