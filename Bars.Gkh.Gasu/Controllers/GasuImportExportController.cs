namespace Bars.Gkh.Gasu.Controller
{
    using System.Web.Mvc;
    using Bars.B4;
    using Bars.Gkh.Gasu.DomainService;

    public class GasuImportExportController : BaseController
    {
        /// <summary>
        ///     Возваращается даные сервиса ГАСУ.
        /// </summary>
        public ActionResult GetServiceData(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IGasuImportExportService>();
            try
            {
                baseParams.Params.Add("gasuAddress", Server.MapPath("~/gasu.config"));

                var result = service.GetServiceData(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }

		/// <summary>
		///     Сохраняются даные сервиса ГАСУ.
		/// </summary>
		public ActionResult SetServiceData(BaseParams baseParams)
		{
			var service = this.Container.Resolve<IGasuImportExportService>();
			try
			{
				baseParams.Params.Add("gasuAddress", Server.MapPath("~/gasu.config"));

				var result = service.SetServiceData(baseParams);
				return result.Success ? new JsonNetResult(result.Data) : JsFailure(result.Message);
			}
			finally
			{
				Container.Release(service);
			}
		}

        /// <summary>
        ///     Сформировать и отправить данные для ГАСУ на указанный веб-сервис
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Успешность операции</returns>
        public ActionResult SendGasu(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IGasuImportExportService>();
            try
            {
                baseParams.Params.Add("gasuAddress", Server.MapPath("~/gasu.config"));

                var result = service.SendGasu(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsFailure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}