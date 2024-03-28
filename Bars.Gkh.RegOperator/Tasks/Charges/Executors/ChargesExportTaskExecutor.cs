namespace Bars.Gkh.RegOperator.Tasks.Charges.Executors
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.FileStorage;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;
    using Castle.Windsor;
    using Domain.ImportExport.DataProviders.Export;
    using Domain.ImportExport.Mapping;
    using Microsoft.Extensions.Logging;
    using NHibernate;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class ChargesExportTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IWindsorContainer Container { get; set; }

        public ILogger Logger { get; set; }

        #region Implementation of ITaskExecutor

        public IDataResult Execute(BaseParams @params, ExecutionContext ctx, IProgressIndicator indicator, CancellationToken ct)
        {
            try
            {
                var providerCode = @params.Params.GetAs<string>("providerCode", ignoreCase: true);

                var exporters = Container.ResolveAll<IExportDataProvider>();
                var fileManager = Container.Resolve<IFileManager>();
                var sessionProvider = Container.Resolve<ISessionProvider>();

                var session = sessionProvider.GetCurrentSession();

                var oldFlushMode = session.FlushMode;
                session.FlushMode = FlushMode.Never;

                IDataResult<ExportOutput> result;

                try
                {
                    using (Container.Using((object)exporters))
                    {
                        var exporter =
                            exporters.FirstOrDefault(x => x.Mapper.GetKey() == providerCode);

                        if (exporter == null)
                        {
                            Logger.LogWarning(
                                "Не найдена реализация для IExportDataProvider по коду {0}"
                                    .FormatUsing(providerCode));
                            return new BaseDataResult(false,
                                "Невозможно осуществить экспорт с кодом {0}".FormatUsing(
                                    providerCode));
                        }

                        result = exporter.GetData(@params);

                        if (!result.Success)
                        {
                            Logger.LogWarning(
                                "Сообщение при экспорте ({0}): {1}".FormatUsing(providerCode,
                                    result.Message));
                            return new BaseDataResult(false, result.Message);
                        }
                    }
                }
                finally
                {
                    session.Clear();
                    session.FlushMode = oldFlushMode;
                }

                using (Container.Using(fileManager))
                {
                    var file = fileManager.SaveFile(result.Data.Data, result.Data.OutputName);
                    return new BaseDataResult(file.Id);
                }
            }
            catch (Exception e)
            {
                return BaseDataResult.Error(" message: {0} \r\n stacktrace: {1}".FormatUsing(e.Message, e.StackTrace));
            }
        }

        public string ExecutorCode { get; private set; }

        #endregion
    }
}