namespace Bars.Gkh.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.ImportExport;

    public class ImportExportController : BaseController
    {
        private readonly IImportExportProvider _provider;

        private readonly IViewModel<ImportExport> _logViewModel;

        private readonly IDomainService<ImportExport> _logDomain;

        public ImportExportController(IImportExportProvider provider, IViewModel<ImportExport> logViewModel, IDomainService<ImportExport> logDomain)
        {
            _provider = provider;
            _logViewModel = logViewModel;
            _logDomain = logDomain;
        }

        public ActionResult GetLog(BaseParams baseParams)
        {
            return new JsonNetResult(_logViewModel.List(_logDomain, baseParams));
        }

        public ActionResult GetExportableEntitites(BaseParams baseParams)
        {
            var entityNames = _provider.GetEntityNames().Select(x => new {TableName = x.Key, Description = x.Value}).ToList();

            var loadParams = baseParams.GetLoadParam();
            var data = entityNames.AsQueryable().Filter(loadParams, Container).Order(loadParams).Paging(loadParams);

            return new JsonListResult(data, entityNames.Count());
        }

        public ActionResult GetEntities(BaseParams baseParams)
        {
            var tableNames = baseParams.Params.GetAs("tableNames", string.Empty).Split(new [] {","}, StringSplitOptions.RemoveEmptyEntries);

            Stream stream = _provider.Export(tableNames);

            return new ReportStreamResult(stream, "export.gzip");
        }

        public ActionResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files.FirstOrDefault();

            if (file.Value == null)
            {
                return JsSuccess();
            }

            _provider.Import(new MemoryStream(file.Value.Data));

            return Content("{ success: true }", "text/html");
        }
    }
}