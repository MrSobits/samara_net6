
using Bars.Gkh.Config;

namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using System.Data;
    using System.IO;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Enum;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.Overhaul.Tat.PriorityParams;
    using Bars.Gkh.Overhaul.Tat.PriorityParams.Impl;
    using Bars.Gkh.Overhaul.Tat.Reports;
    using Bars.Gkh.Utils;

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;
    using Overhaul.DomainService;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Сервис для актуализации версии
    /// </summary>
    public class ActualizeVersionLogService : IActualizeVersionLogService
    {
        public IWindsorContainer Container { get; set; }

        public IFileManager FileManagerService { get; set; }

        public IDomainService<FileInfo> FileInfoDomain{ get; set; }

        /// <summary>
        /// Запускае
        /// </summary>
        /// <returns></returns>
        public FileInfo CreateLogFile(IEnumerable<ActualizeVersionLogRecord> LogRecords, BaseParams baseParams)
        {
            IPrintForm printForm;

            if (Container.Kernel.HasComponent("ActualizeVersionLogReport"))
            {
                printForm = Container.Resolve<IPrintForm>("ActualizeVersionLogReport");
            }
            else
            {
                throw new Exception("Реализация ActualizeVersionLogReport не зарегестрирована");
            }

            baseParams.Params.Add("Data", LogRecords);

            var rp = new ReportParams();
            printForm.SetUserParams(baseParams);
            printForm.PrepareReport(rp);
            var template = printForm.GetTemplate();

            IReportGenerator generator;
            if (printForm is IGeneratedPrintForm)
            {
                generator = printForm as IGeneratedPrintForm;
            }
            else
            {
                generator = Container.Resolve<IReportGenerator>("XlsIoGenerator");
            }

            using (var result = new MemoryStream())
            {
                generator.Open(template);
                generator.Generate(result, rp);
                result.Seek(0, SeekOrigin.Begin);

                var fileInfo = FileManagerService.SaveFile(result, "ActualizeVersionLogReport.xlsx");

                return fileInfo;
            }
        }
    }
}