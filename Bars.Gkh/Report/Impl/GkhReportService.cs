namespace Bars.Gkh.Report
{
    using System.Collections;
    using System.IO;
    using System.Linq;
        // TODO: Расскоментировать
        //using Bars.Gkh.DocIoGenerator;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using Bars.Gkh.Authentification;
    using B4.Modules.Reports;
    using B4.Utils;
    using Castle.Windsor;
    using StimulReport;

    /// <inheritdoc />
    public class GkhReportService : IGkhReportService
    {
        /// <summary>
        /// Контейнер IoC
        /// </summary>
        public virtual IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IList GetReportList(BaseParams baseParams)
        {
            var reports = this.Container.ResolveAll<IGkhBaseReport>();
            var authService = this.Container.Resolve<IAuthorizationService>();
            var userIdentity = this.Container.Resolve<IUserIdentity>();

            var codeForm = baseParams.Params["codeForm"].ToString();

            try
            {
                return reports
                    .Where(x => !x.CodeForm.IsEmpty() && codeForm.Split(',').Contains(x.CodeForm))
                    .Where(x => x.PrintingAllowed)
                    .Where(x => x.Permission.IsEmpty() || authService.Grant(userIdentity, x.Permission))
                    .Select(x => new { x.Id, x.Name, x.Description })
                    .ToList();
            }
            finally
            {
                foreach (var report in reports)
                {
                    this.Container.Release(report);
                    this.Container.Release(authService);
                    this.Container.Release(userIdentity);
                }
            }
        }

        /// <inheritdoc />
        public FileStreamResult GetReport(BaseParams baseParams)
        {
            var reportProvider = this.Container.Resolve<IGkhReportProvider>();

            MemoryStream stream;
            
            var report = this.Container.ResolveAll<IGkhBaseReport>().First(x => x.Id == baseParams.Params.GetAs<string>("reportId"));
            var generatorName = report.GetReportGenerator();

            var userParam = new UserParamsValues();

            if (baseParams.Params.ContainsKey("userParams") && baseParams.Params["userParams"] is DynamicDictionary)
            {
                userParam.Values = (DynamicDictionary)baseParams.Params["userParams"];
            }

            // Проставляем пользовательские параметры
            report.SetUserParams(userParam);

            if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
            {
                //Вот такой вот костыльный этот метод. Все надо переделывать
                stream = (report as StimulReport).GetGeneratedReport();

            }
            else
            {
                var reportParams = new ReportParams();
                report.PrepareReport(reportParams);

                stream = new MemoryStream();
                var generator = this.Container.Resolve<IReportGenerator>(generatorName);
                reportProvider.GenerateReport(report, stream, generator, reportParams);
            }

            // потому что не понятно, как предоставлять результирующее расширение файла, поскольку у СтимулРепорт шаблон в формате mrt, а результат может быть doc, pdf, xls
            var fileName = string.Empty;

            if (report is IReportGeneratorFileName repFn)
            {
                fileName = repFn.GetFileName();
            }
            else
            {
                // сначала берем расширение которое указали в отчете
                var extension = report.Extention;

                // Поскольку у отчета может быть много шаблонов 
                // и к тому же каждый шаблон может быть своего формата - один doc, другой xls, третий mrt
                // то тогда считаем более приоритетным тот, шаблон который заменили
                var replaceExtension = report.GetFileExtension();

                if (!string.IsNullOrEmpty(replaceExtension))
                {
                    extension = replaceExtension;
                }

                // если совсем не удалось определить расширение то тогда ставим по Генератору
                if (string.IsNullOrEmpty(extension))
                {
                    // Если программист забыл заполнить поле Extension при указании шаблона, то остается только по генератору определять что он хотел
                    switch (report.GetReportGenerator())
                    {
                        case "XlsIoGenerator": extension = "xls"; break;
                        case "DocIoGenerator": extension = "doc"; break;
                        case "StimulReportGenerator": extension = "mrt"; break;
                    }
                }

                fileName = $"{report.Name}.{extension}";
            }

            if (string.IsNullOrEmpty(Path.GetExtension(fileName)))
            {
                fileName += ".xls";
            }
            if (Path.GetExtension(fileName) == ".docx")
            {
                var userManager = this.Container.Resolve<IGkhUserManager>();

                try
                {
                    var oper = userManager.GetActiveOperator();
                    if (oper != null)
                    {
                        var ext = oper.ExportFormat;
                        if (ext == Enums.OperatorExportFormat.doc)
                        {
                            // TODO: Расскоментировать
                            //var docIO = Container.Resolve<IDocIo>();
                            MemoryStream docstream = new MemoryStream();
                           // docIO.ConvertToDoc(stream).CopyTo(docstream);
                            fileName = "report.doc";
                            docstream.Position = 0;
                            var mimeTypedoc = MimeTypeHelper.GetMimeType(Path.GetExtension(fileName));
                            return new FileStreamResult(docstream, mimeTypedoc) { FileDownloadName = fileName };
                        }
                    }                 

                }
                finally
                {
                    this.Container.Release(userManager);
                }
            }

            var mimeType = MimeTypeHelper.GetMimeType(Path.GetExtension(fileName));

            return new FileStreamResult(stream, mimeType) { FileDownloadName = fileName };
        }
    }
}