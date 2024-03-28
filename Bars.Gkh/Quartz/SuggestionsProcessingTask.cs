namespace Bars.Gkh.Quartz
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4.Application;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.FileStorage;
    using B4.Modules.Quartz;
    using B4.Utils;

    using Castle.MicroKernel.Lifestyle;
    using Castle.Windsor;
    using Entities;
    using Entities.Suggestion;
    using Enums;

    using Ionic.Zip;
    using Ionic.Zlib;

    /// <summary>
    ///     Таска для обработки обращений по рубрикам
    /// </summary>
    public class SuggestionsProcessingTask : ITask
    {
        private readonly IWindsorContainer _container = ApplicationContext.Current.Container;

        public void Execute(DynamicDictionary @params)
        {
            const string OperationName = "Смена исполнителей обращений";
            var log = new StringBuilder();

            var suggDomain = this._container.ResolveDomain<CitizenSuggestion>();
            var rubricDomain = this._container.ResolveDomain<Rubric>();
            var logOperationRepo = this._container.ResolveRepository<LogOperation>();
            var fileManager = this._container.Resolve<IFileManager>();

            try
            {
                using (this._container.Using(suggDomain, rubricDomain, logOperationRepo, fileManager))
                {
                    using (this._container.BeginScope())
                    {
                        var rubrics = rubricDomain.GetAll().Where(x => x.IsActual).ToList();

                        var yesterdayEnd = DateTime.Today;
                        var yesterdayStart = yesterdayEnd.AddDays(-1);

                        foreach (var rubric in rubrics)
                        {
                            var rubricId = rubric.Id;
                            var suggestions = suggDomain.GetAll()
                                .Where(x => x.Rubric.Id == rubricId)
                                .Where(x => x.Deadline != null)
                                .Where(x => x.Deadline.Value >= yesterdayStart)
                                .Where(x => x.Deadline.Value < yesterdayEnd)
                                .Where(x => x.State == null || !x.State.FinalState);
                            var result = rubric.Run(suggestions, log);

                            var message = string.Format("{0} обращений", suggestions.Count());

                            log.AppendFormat(
                                "{0}. Рубрика \"{1}\" обработана. Результат: {2}. Сообщение: {3}",
                                OperationName,
                                rubric.Name,
                                result.Success ? "Успешно" : "Неудача",
                                result.Message ?? message);
                        }

                        var logOperation = new LogOperation
                        {
                            StartDate = yesterdayEnd,
                            Comment = OperationName,
                            OperationType = LogOperationType.RunCitizenSuggestion,
                            EndDate = DateTime.UtcNow
                        };

                        var logsZip = new ZipFile(Encoding.UTF8)
                        {
                            CompressionLevel = CompressionLevel.Level3,
                            AlternateEncoding = Encoding.GetEncoding("cp866")
                        };

                        using (var logFile = new MemoryStream())
                        {
                            var filelog = Encoding.GetEncoding(1251).GetBytes(log.ToString());

                            logsZip.AddEntry(
                                string.Format("{0}.csv", logOperation.OperationType.GetEnumMeta().Display), filelog);

                            logsZip.Save(logFile);

                            var logFileInfo = fileManager.SaveFile(logFile,
                                string.Format("{0}.zip", logOperation.OperationType.GetEnumMeta().Display));

                            logOperation.LogFile = logFileInfo;
                        }

                        logOperationRepo.Save(logOperation);

                        suggDomain.GetAll()
                        .Where(x => x.TestSuggestion)
                        .Select(x => x.Id)
                        .ForEach(x => suggDomain.Delete(x));
                    }
                }
            }
            catch (Exception e)
            {
                log.AppendFormat(
                    "{0}. Ошибка при обработке SuggestionsProcessingTask. ExceptionMessage: {1}. StackTrace: {2}",
                    OperationName,
                    e.Message,
                    e.StackTrace);
            }
        }
    }
}