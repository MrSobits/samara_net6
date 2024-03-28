namespace Bars.Gkh.RegOperator.Tasks.PaymentDocuments.V2
{
    using Authentification;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Modules.Tasks.Common.Contracts.Result;
    using B4.Modules.Tasks.Common.Service;
    using B4.Utils;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.Enums.Administration;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Domain.ValueObjects;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using Ionic.Zip;
    using Ionic.Zlib;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using ExecutionContext = B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    /// <summary>
    /// Обработка логов
    /// </summary>
    public class LogFileMergeCallback : ITaskCallback
    {
        /// <summary>
        /// Идентификатор обратного вызова задачи
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private readonly IWindsorContainer container;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        public LogFileMergeCallback(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Вызов обработки
        /// </summary>
        /// <param name="taskId">Идентификатор задачи</param>
        /// <param name="baseParams">Входные параметры</param>
        /// <param name="executionContext">Контекст выполения</param>
        /// <param name="indicator">Индикатор прогресса</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns></returns>
        public CallbackResult Call(long taskId,
            BaseParams baseParams,
            ExecutionContext executionContext,
            IProgressIndicator indicator,
            CancellationToken cancellationToken)
        {
            var logFilesPath = baseParams.Params.GetAs<string>("logFilesPath");
            var startDate = baseParams.Params.GetAs<DateTime>("startDate");
            var period = baseParams.Params.GetAs<PeriodDto>("periodDto");
            var logLevel = baseParams.Params.GetAs<PaymentDocumentsLogLevel>("logLevel");
            if (logLevel == PaymentDocumentsLogLevel.None)
            {
                return new CallbackResult(true);
            }

            var logOperationRepo = this.container.ResolveRepository<LogOperation>();
            var fileManager = this.container.Resolve<IFileManager>();
            var userManager = this.container.Resolve<IGkhUserManager>();

            try
            {
                string[] dirs = Directory.GetFiles(logFilesPath);
              
                var logOperation = new LogOperation
                {
                    StartDate = startDate,
                    Comment = "Выгрузка документов на оплату",
                    OperationType = LogOperationType.PrintPaymentDocument,
                    EndDate = DateTime.UtcNow,
                    User = userManager.GetActiveUser()
                };

                var finalLog = new StringBuilder();

                var paymentDocumentConfig = this.container.GetGkhConfig<RegOperatorConfig>().PaymentDocumentConfigContainer;
                var groupByDeliveryAgent = paymentDocumentConfig.PaymentDocumentConfigIndividual.GroupingOptions.GroupingElements
                    .Any(g => g.GroupingType == GroupingType.DeliveryAgent && g.IsUsed == YesNo.Yes);
                var deliveryAgentColumn = groupByDeliveryAgent ? ";Агент доставки" : "";

                finalLog.AppendLine(@"Номер ЛС;Тип абонента;Статус;ФИО/Наименование абонента;Индекс;Адрес абонента" + deliveryAgentColumn + ";Мун. район;Мун. образование;Получатель;ИНН получателя;КПП получателя;Адрес получателя;Расчетный счет получателя;Тариф, руб;Общая площадь, м. кв.;Всего к оплате;К оплате за месяц, руб;Сообщение об ошибке");

                var persAccCnt = 0;
                var errorCnt = 0;

                foreach (var path in dirs)
                {
                    if (File.Exists(path))
                    {
                        var tempLog = File.ReadLines(path, Encoding.GetEncoding(1251));
                        if (tempLog != null)
                        {
                            tempLog.ForEach(x =>
                            {
                                persAccCnt++;
                                var error = false;
                                if (x.Last() != ';')
                                {
                                    errorCnt++;
                                    error = true;
                                }

                                if (error || logLevel == PaymentDocumentsLogLevel.All)
                                {
                                    finalLog.AppendLine(x);
                                }
                            });
                            File.Delete(path);
                        }
                    }
                }

                Directory.Delete(logFilesPath);

                finalLog.Insert(0, "{0};{1};{2};{3};{4};{5};{6}\n".FormatUsing(
                    period.Name, 
                    logOperation.StartDate, 
                    logOperation.EndDate, 
                    errorCnt > 0 ? "Завершено с ошибками" : "Успешно завершено", 
                    persAccCnt, 
                    persAccCnt - errorCnt, 
                    errorCnt));

                finalLog.Insert(0, "Отчетный период;Дата начала формирования выгрузки;Дата окончания формирования выгрузки;Статус;Всего обработано ЛС;в т. ч. кол-во успешно обработанных ЛС;в т. ч. кол-во обработанных ЛС с ошибками\n");

                var logsZip = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level3,
                    AlternateEncoding = Encoding.GetEncoding("cp866")
                };

                using (var logFile = new MemoryStream())
                {
                    var log = Encoding.GetEncoding(1251).GetBytes(finalLog.ToString());

                    logsZip.AddEntry(string.Format("{0}.csv", logOperation.OperationType.GetEnumMeta().Display), log);

                    logsZip.Save(logFile);

                    var logFileInfo = fileManager.SaveFile(logFile, string.Format("{0}.zip", logOperation.OperationType.GetEnumMeta().Display));

                    logOperation.LogFile = logFileInfo;
                }

                logOperationRepo.Save(logOperation);

                return new CallbackResult(true);
            }
            finally
            {
                this.container.Release(logOperationRepo);
                this.container.Release(fileManager);
                this.container.Release(userManager);
            }
        }
    }
}