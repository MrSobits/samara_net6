namespace Bars.Gkh.Controllers.Administration
{
    using System.IO;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.SystemDataTransfer;
    using Bars.Gkh.SystemDataTransfer.Domain;
    using Bars.Gkh.SystemDataTransfer.Meta;
    using B4.Utils;

    using Bars.Gkh.Utils;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер для интеграции из внешней системы
    /// </summary>
    public class DataTransferIntegrationController : BaseController
    {
        public ISystemIntegrationService Service { get; set; }

        /// <summary>
        /// Запустить интеграцию
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        public ActionResult RunIntegration(BaseParams baseParams)
        {
            return this.Service.RunIntegration(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Выполнить экспорт
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var integrationService = this.Resolve<IDataTransferProvider>();
            using (this.Container.Using(integrationService))
            {
                var stream = integrationService.Export(
                    baseParams.Params.GetAs<string[]>("typeNames"), 
                    baseParams.Params.GetAs("exportDependencies", true));

                return this.File(stream, "application/zip");
            }
        }

        /// <summary>
        /// Выполнить импорт
        /// </summary>
        public ActionResult Import(BaseParams baseParams)
        {
            var integrationService = this.Resolve<IDataTransferProvider>();

            if (!baseParams.Files.Any())
            {
                return this.JsFailure("File missing");
            }

            using (var memoryStream = new MemoryStream(baseParams.Files.FirstOrDefault().Value.Data))
            using (this.Container.Using(integrationService))
            {
                integrationService.Import(memoryStream);
                return this.JsSuccess();
            }
        }

        /// <summary>
        /// Получить список экспортируемых типов
        /// </summary>
        public ActionResult GetExportableTypes(BaseParams baseParams)
        {
            var exportContainer = new TransferEntityContainer();
            this.Container.ResolveAll<ITransferEntityProvider>().ForEach(x => x.FillContainer(exportContainer));

            var data = exportContainer.Container.Values
                .Select(
                    x => new
                    {
                        Id = x.Type.FullName,
                        x.Description
                    })
                .AsQueryable()
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);

            return data.ToJsonResult();
        }
    }
}