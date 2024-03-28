namespace Bars.Gkh.Controllers.Suggestion
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.FileStorage;
    using B4.Utils;
    using Entities;
    using Entities.Suggestion;
    using Enums;
    using Ionic.Zip;
    using Ionic.Zlib;

    public class TransitionBusinessProcessController : BaseController
    {
        public ActionResult Validate(BaseParams baseParams)
        {
            var rubricDomain = this.Container.ResolveDomain<Rubric>();
            using (this.Container.Using(rubricDomain))
            {
                var rubricId = baseParams.Params.GetAs<long>("rubricId");
                var rubric = rubricDomain.FirstOrDefault(x => x.Id == rubricId);

                if (rubric == null)
                {
                    return this.JsFailure("Рубрика не найдена");
                }

                var validation = rubric.Validate();

                return new JsonNetResult(validation);
            }
        }

        public ActionResult Run(BaseParams baseParams)
        {
            var suggDomain = this.Container.ResolveDomain<CitizenSuggestion>();
            var rubricDomain = this.Container.ResolveDomain<Rubric>();
            var fileManager = this.Container.Resolve<IFileManager>();
            var logOperationRepo = this.Container.ResolveRepository<LogOperation>();

            const string OperationName = "Смена исполнителей обращений";

            using (this.Container.Using(rubricDomain, suggDomain, fileManager, logOperationRepo))
            {
                var rubricId = baseParams.Params.GetAs<long>("rubricId");
                var rubric = rubricDomain.FirstOrDefault(x => x.Id == rubricId);
                var log = new StringBuilder();

                if (rubric == null)
                {
                    return this.JsFailure("Рубрика не найдена");
                }

                var yesterdayEnd = DateTime.Today;
                var yesterdayStart = yesterdayEnd.AddDays(-1);
                var suggestions = suggDomain.GetAll()
                    .Where(x => x.Rubric.Id == rubricId)
                    .Where(x => x.Deadline != null)
                    .Where(x => x.Deadline.Value >= yesterdayStart)
                    .Where(x => x.Deadline.Value < yesterdayEnd)
                    .Where(x => x.State == null || !x.State.FinalState);
                var result = rubric.Run(suggestions, log);

                var message = string.Format("{0} обращений", suggestions.Count());

                log.AppendFormat(
                    "Смена исполнителей обращений. Рубрика \"{0}\" обработана. Результат: {1}. Сообщение: {2}",
                    rubric.Name,
                    result.Success ? "Успешно" : "Неудача",
                    result.Message ?? message);

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
                return new JsonNetResult(result);
            }
        }
    }
}