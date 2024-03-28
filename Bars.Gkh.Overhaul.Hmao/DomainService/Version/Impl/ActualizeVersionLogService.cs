
namespace Bars.Gkh.Overhaul.Hmao.DomainService.Version.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using B4;
    using B4.Modules.FileStorage;
    using B4.Modules.Reports;
    using Castle.Windsor;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис для актуализации версии
    /// </summary>
    public class ActualizeVersionLogService<T, TForm> : IActualizeVersionLogService<T, TForm>
        where T : class
        where TForm : class
    {
        public IWindsorContainer Container { get; set; }

        public IFileManager FileManagerService { get; set; }

        /// <summary>
        /// Создание файла лога
        /// </summary>
        /// <returns></returns>
        public FileInfo CreateLogFile(IEnumerable<T> logRecords, BaseParams baseParams)
        {
            var printReportName = typeof(TForm).Name;
            var printForm = this.Container.Kernel.HasComponent(printReportName)
                ? this.Container.Resolve<IPrintForm>(printReportName)
                : throw new Exception($"Реализация {printReportName} не зарегистрирована");

            baseParams.Params.Add("Data", logRecords);

            var rp = new ReportParams();
            printForm.SetUserParams(baseParams);
            printForm.PrepareReport(rp);
            var template = printForm.GetTemplate();

            var generator = printForm is IGeneratedPrintForm form 
                ? form 
                : this.Container.Resolve<IReportGenerator>("XlsIoGenerator");

            using (var result = new MemoryStream())
            {
                generator.Open(template);
                generator.Generate(result, rp);
                result.Seek(0, SeekOrigin.Begin);

                var fileInfo = this.FileManagerService.SaveFile(result, $"{printReportName}.xlsx");

                return fileInfo;
            }
        }
    }
}