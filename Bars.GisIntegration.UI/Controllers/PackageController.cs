namespace Bars.GisIntegration.UI.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.UI.Service;

    /// <summary>
    /// Контроллер пакетов
    /// </summary>
    public class PackageController : BaseController
    {
        /// <summary>
        /// Получить xml данные пакета форматированные для просмотра
        /// либо неформатированные для подписи
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие 
        /// идентификатор пакета, 
        /// тип пакета,
        /// признак: для предпросмотра
        /// признак: подписанные/неподписанные данные</param>
        /// <returns>xml данные пакета</returns>
        public ActionResult GetPackageXmlData(BaseParams baseParams)
        {
            var packageService = this.Container.Resolve<IPackageService>();

            try
            {
                var result = packageService.GetPackageXmlData(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(packageService);
            }
        }

        /// <summary>
        /// Сохранить подписанную xml
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// идентификатор пакета, 
        /// тип пакета,
        /// подписанные данные
        /// </param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult SaveSignedData(BaseParams baseParams)
        {
            var packageService = this.Container.Resolve<IPackageService>();

            try
            {
                var result = packageService.SaveSignedData(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(packageService);
            }
        }
    }
}
