namespace Bars.GisIntegration.UI.Controllers
{
    using System.Collections;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Controllers;
    using Bars.GisIntegration.UI.Service;

    public class GisIntegrationController : BaseDataSupplierController
    {
        /// <summary>
        /// Получить список контрагентов привязанных к текущему пользователю
        /// или делегировавших ему полномочия
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult ListContragents(BaseParams baseParams)
        {
            var gisIntegrService = this.Container.Resolve<IGisIntegrationService>();

            try
            {
                var result = (ListDataResult)gisIntegrService.ListContragents(baseParams);
                return new JsonListResult((IEnumerable)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(gisIntegrService);
            }
        }

        /// <summary>
        /// Получить список методов
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult MethodList(BaseParams baseParams)
        {
            var gisIntegrService = this.Container.Resolve<IGisIntegrationService>();

            try
            {
                var result = (ListDataResult)gisIntegrService.GetMethodList(baseParams);
                return new JsonListResult((IEnumerable)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(gisIntegrService);
            }
        }


        /// <summary>
        /// Проверить выполнимость метода
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор метода</param>
        /// <returns>Результат проверки</returns>
        public ActionResult CheckMethodFeasibility(BaseParams baseParams)
        {
            var gisIntegrService = this.Container.Resolve<IGisIntegrationService>();

            try
            {
                var result = gisIntegrService.CheckMethodFeasibility(baseParams);

                 return result.Success ? new JsonGetResult(result) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(gisIntegrService);
            }
        }

        /// <summary>
        ///  Запланировать подготовку данных: извлечение, валидация, формирование пакетов
        /// </summary>
        /// <param name="baseParams">Параметры подготовки данных, содержащие фильтры, идентификатор метода</param>
        /// <returns>Результат планирования</returns>
        public ActionResult SchedulePrepareData(BaseParams baseParams)
        {
            var gisIntegrService = this.Container.Resolve<IGisIntegrationService>();

            try
            {
                var result = gisIntegrService.SchedulePrepareData(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(gisIntegrService);
            }
        }

        /// <summary>
        ///  Запланировать отправку данных
        /// </summary>
        /// <param name="baseParams">Параметры отправки данных, содержащие
        /// идентификатор задачи,
        /// идентификаторы пакетов к отправке</param>
        /// <returns>Результат планирования</returns>
        public ActionResult ScheduleSendData(BaseParams baseParams)
        {
            var gisIntegrService = this.Container.Resolve<IGisIntegrationService>();

            try
            {
                var result = gisIntegrService.ScheduleSendData(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(gisIntegrService);
            }
        }

        /// <summary>
        /// Получить параметры выполнения подписывания и отправки данных
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор задачи</param>
        /// <returns>Параметры выполнения подписывания и отправки данных</returns>
        public ActionResult GetSignAndSendDataParams(BaseParams baseParams)
        {
            var gisIntegrService = this.Container.Resolve<IGisIntegrationService>();

            try
            {
                var result = gisIntegrService.GetSignAndSendDataParams(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(gisIntegrService);
            }
        }

        public ActionResult LoadGisContragents()
        {
            var gisIntegrService = this.Container.Resolve<IGisIntegrationService>();

            try
            {
                var par = new BaseParams();
                par.Params.SetValue("exporter_Id", "OrgRegistryExporter");
                par.Params.SetValue("selectedList", "ALL");

                var result = gisIntegrService.SchedulePrepareData(par);

                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(gisIntegrService);
            }
        }
    }
}