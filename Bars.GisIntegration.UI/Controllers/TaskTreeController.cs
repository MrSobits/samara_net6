namespace Bars.GisIntegration.UI.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GisIntegration.UI.Service;
    using Bars.GisIntegration.UI.ViewModel;

    /// <summary>
    /// Контроллер дерева задач
    /// </summary>
    public class TaskTreeController: BaseController
    {
        /// <summary>
        /// Получить дочерние узлы
        /// </summary>
        /// <param name="baseParams">Параметры, в т.ч. параметры текущего узла</param>
        /// <returns>Дочерние узлы</returns>
        public ActionResult GetTaskTreeNodes(BaseParams baseParams)
        {
            var viewModel = this.Container.Resolve<ITreeViewModel>("TaskTreeViewModel");

            var result = viewModel.List(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Экспортировать в excel результаты валидации
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор триггера подготовки данных</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult ValidationResultForExport(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ValidationResultExport");
            try
            {
                return export?.ExportData(baseParams);
            }
            finally
            {
                this.Container.Release(export);
            }
        }

        /// <summary>
        /// Экспортировать в excel результаты загрузки вложений
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор триггера подготовки данных</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult UploadAttachmentsResultForExport(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("UploadAttachmentResultExport");
            try
            {
                return export?.ExportData(baseParams);
            }
            finally
            {
                this.Container.Release(export);
            }
        }

        /// <summary>
        /// Экспортировать в excel список пакетов
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор триггера подготовки данных</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult PackagesForExport(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("PackagesExport");
            try
            {
                return export?.ExportData(baseParams);
            }
            finally
            {
                this.Container.Release(export);
            }
        }

        /// <summary>
        /// Получить результат триггера подготовки данных
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор триггера или задачи</param>
        /// <returns>Результат подготовки данных</returns>
        public ActionResult GetPreparingDataTriggerResult(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ITaskTreeService>();

            try
            {
                var result = service.GetPreparingDataTriggerResult(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить протокол выполнения триггера
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификатор триггера</param>
        /// <returns>Результат выполнения операции, 
        /// содержащий протокол выполнения триггера</returns>
        public ActionResult GetTriggerProtocol(BaseParams baseParams)
        {
            var viewModel = this.Container.Resolve<ITriggerProtocolViewModel>();

            try
            {
                var result = viewModel.List(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                this.Container.Release(viewModel);
            }
        }

        /// <summary>
        /// Получить результаты обработки (отправки, получения результата) объектов
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// идентификатор триггера отправки данных,
        /// или
        /// идентификатор связки триггера и пакета</param>
        /// <returns>Результаты обработки объектов</returns>
        public ActionResult GetObjectProcessingResults(BaseParams baseParams)
        {
            var viewModel = this.Container.Resolve<IObjectProcessingResultViewModel>();

            try
            {
                var result = viewModel.List(baseParams);
                return result.Success ? new JsonNetResult(result) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(viewModel);
            }
        }
    }
}