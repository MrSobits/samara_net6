namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Chelyabinsk.EaisCitizensAppealService;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Enums;

    using Castle.Core;
    using Castle.Windsor;

    using NHibernate.Linq;

    public partial class CitizensAppealServiceClient : ICitizensAppealServiceClient, IInitializable
    {
        private CitizensAppealServiceSoapClient client;
        private readonly MessageContainer messageContainer = new MessageContainer();
        private bool isDisposed = false;

        public IWindsorContainer Container { get; set; }
        public IDomainService<AppealCitsTransferResult> AppealCitsTransferResultDomain { get; set; }
        public IUserIdentity UserIdentity { get; set; }
        public IFileManager FileManager { get; set; }

        /// <inheritdoc />
        public IDataResult ExportInfoAcceptWorkResult(AppealCits appealCits, bool isAccept)
        {
            if (!appealCits.ExecutantTakeDate.HasValue)
            {
                return BaseDataResult.Error("Не заполнено поле 'Дата приема в работу исполнителем'");
            }
            if (appealCits.Executant == null)
            {
                return BaseDataResult.Error("Не заполнено поле 'Исполнитель'");
            }

            var exportParams = new ExportInfoAcceptWorkResult
            {
                АppealUID = appealCits.AppealUid.ToStr(),
                IsAcceptWork = isAccept,
                DateAcceptWork = appealCits.ExecutantTakeDate.Value,
                ExecutantFIO = appealCits.Executant.Fio
            };

            var baseParams = new BaseParams();
            baseParams.Params.Add("exportParams", exportParams);

            var transfer = this.StartExport(appealCits.Id, AppealCitsTransferType.ExportInfoAcceptWork, baseParams);

            return this.InternalExportInfoAcceptWorkResult(exportParams, transfer);
        }

        /// <inheritdoc />
        public IDataResult ExportInfoCompletionOfWorkResult(AppealCits appealCits, bool isCompletion)
        {
            var answers = this.GetAnswers(appealCits);
            var answerIds = answers.Select(x => x.Id).ToArray();
            var attachments = this.GetAttachments(answerIds);
            var questions = this.GetQuestions(answerIds);

            Func<AppealCitsAnswer, ExportInfoCompletionOfWorkResult> selector;

            if (isCompletion)
            {
                selector = x => new ExportInfoCompletionOfWorkResult
                {
                    AppealUID = x.AppealCits.AppealUid.ToStr(),
                    IsCompletionOfWork = isCompletion,
                    SendingDateToDeclarant = x.ExecDate ?? DateTime.Now,
                    AnswerNumber = x.DocumentNumber,
                    AnswerContent = x.Description,
                    AnswerAttachments = attachments.Get(x.Id),
                    DateOfExtension = x.ExtendDate,
                    KindOfFactCheck = x.FactCheckingType?.Code.ToIntNullable(),
                    FinalReply = x.TypeAppealFinalAnswer.ToInt(),
                    ConsiderationResult = x.ConcederationResult?.Code.ToInt() ?? 0,
                    AppealQuestions = questions.Get(x.Id),
                    AnswerUID = x.Id.ToString(),
                    ExecutantFIO = x.Executor.Fio
                };
            }
            else
            {
                selector = x => new ExportInfoCompletionOfWorkResult
                {
                    AppealUID = x.AppealCits.AppealUid.ToStr(),
                    IsCompletionOfWork = isCompletion,
                    SendingDateToDeclarant = x.ExecDate ?? DateTime.Now,
                    DateOfExtension = x.ExtendDate,
                    FinalReply = GetFinalReply(x.TypeAppealFinalAnswer),
                    KindOfFactCheck = x.FactCheckingType?.Code.ToIntNullable(),
                    AnswerUID = x.Id.ToString(),
                    ExecutantFIO = x.Executor.Fio
                };
            }

            var exportParams = answers.Select(selector)
                .ToArray();

            var storedParams = new ExportInfoCompletionOfWorkParams
            {
                ExportParams = exportParams,
                AppealCitsId = appealCits.Id,
                AnswerIds = answerIds
            };

            var baseParams = new BaseParams();
            baseParams.Params.Add("exportParams", storedParams);

            var transfer = this.StartExport(appealCits.Id, AppealCitsTransferType.ExportInfoCompletionOfWork, baseParams);

            return this.InternalExportInfoCompletionOfWorkResult(storedParams, transfer);
        }

        /// <inheritdoc />
        public IDataResult ExportInfoCitizensAppealCancelResult(AppealCits appealCits, bool isCancel)
        {
            var exportParams = new ExportInfoCitizensAppealCancelResult
            {
                АppealUID = appealCits.AppealUid.ToStr(),
                IsАppealСancel = isCancel
            };

            var baseParams = new BaseParams();
            baseParams.Params.Add("exportParams", exportParams);

            var transfer = this.StartExport(appealCits.Id, AppealCitsTransferType.ExportInfoCitizensAppealCancel, baseParams);

            return this.InternalExportInfoCitizensAppealCancelResult(exportParams, transfer);
        }

        /// <inheritdoc />
        public IDataResult RestartAppealCitsTransfer(long appealCitsTransferResultId)
        {
            var transferResult = this.ReStartExport(appealCitsTransferResultId);

            if (transferResult == null)
            {
                return BaseDataResult.Error($"Не найдена запись передачи данных с идентификатором {appealCitsTransferResultId}");
            }

            try
            {
                switch (transferResult.Type)
                {
                    case AppealCitsTransferType.ExportInfoAcceptWork:
                        this.InternalExportInfoAcceptWorkResult(transferResult.ExportParams.Params
                                .GetAs<ExportInfoAcceptWorkResult>("exportParams"),
                            transferResult);
                        break;
                    case AppealCitsTransferType.ExportInfoCompletionOfWork:
                        this.InternalExportInfoCompletionOfWorkResult(transferResult.ExportParams.Params
                                .GetAs<ExportInfoCompletionOfWorkParams>("exportParams"),
                            transferResult);
                        break;
                    case AppealCitsTransferType.ExportInfoCitizensAppealCancel:
                        this.InternalExportInfoCitizensAppealCancelResult(transferResult.ExportParams.Params
                                .GetAs<ExportInfoCitizensAppealCancelResult>("exportParams"),
                            transferResult);
                        break;
                    default:
                        return BaseDataResult.Error("Не удалось определить тип операции");
                }
            }
            catch(Exception e)
            {
                this.ErrorExport(transferResult,
                    e.GetInnerExceptions()
                        .Select(x => x.Message)
                        .AggregateWithSeparator(Environment.NewLine));
                return BaseDataResult.Error(e.Message);
            }

            return new BaseDataResult();
        }

        private int GetFinalReply(TypeAppealFinalAnswer tfa)
        {
            switch (tfa)
            {
                case TypeAppealFinalAnswer.Project:
                    return 0;
                case TypeAppealFinalAnswer.Answer: return 1;
                    default : return 0;
            }
        }

        private IDataResult InternalExportInfoAcceptWorkResult(ExportInfoAcceptWorkResult exportParams, AppealCitsTransferResult transfer)
        {          
            ArgumentChecker.NotNull(exportParams, "Не удалось получить информацию о передаваемых данных");
            try
            {
                this.client.ExportInfoAcceptWorkResult(exportParams);

                this.EndExport(transfer);
            }
            catch (Exception e)
            {
                this.ErrorExport(transfer,
                    e.GetInnerExceptions()
                        .Select(x => x.Message)
                        .AggregateWithSeparator(Environment.NewLine));
                return BaseDataResult.Error(e.Message);
            }

            return new BaseDataResult();
        }

        private IDataResult InternalExportInfoCompletionOfWorkResult(ExportInfoCompletionOfWorkParams exportParams, AppealCitsTransferResult transfer)
        {
            ArgumentChecker.NotNull(exportParams, "Не удалось получить информацию о передаваемых данных");
            var result = new List<ImportInfoDataTransferResult>();
            var resultDict = new Dictionary<long, ImportInfoDataTransferResult>();

            if (exportParams.ExportParams.IsNotEmpty())
            {
                var errorMsg = "Не удалось загрузить все ответы на обращения";
                try
                {
                    foreach (var exportParam in exportParams.ExportParams)
                    {
                        var response = this.client.ExportInfoCompletionOfWorkResult(exportParam);

                        if (response != null)
                        {
                            resultDict[response.AnswerUID.ToLong()] = response;
                        }
                        result.Add(response);
                    }

                    if (resultDict.IsNotEmpty())
                    {
                        var answerDomain = this.Container.ResolveDomain<AppealCitsAnswer>();
                        using (this.Container.Using(answerDomain))
                        {
                            var answers = answerDomain.GetAll()
                                .Where(x => x.AppealCits.Id == exportParams.AppealCitsId)
                                .ToList();

                            answers.ForEach(x =>
                            {
                                x.IsUploaded = resultDict.Get(x.Id)?.IsUploaded;
                                x.AdditionalInfo = resultDict.Get(x.Id)?.AdditionalInformation;
                            });

                            TransactionHelper.InsertInManyTransactions(this.Container, answers, useStatelessSession: true);
                        }
                    }

                    if (result.Any(x => !x.IsUploaded))
                    {
                        this.ErrorExport(transfer, errorMsg);
                        return BaseDataResult.Error(errorMsg);
                    }
                }
                catch (Exception e)
                {
                    this.ErrorExport(transfer,
                        e.GetInnerExceptions()
                            .Select(x => x.Message)
                            .AggregateWithSeparator(Environment.NewLine));
                    return BaseDataResult.Error(errorMsg);
                }
            }

            this.EndExport(transfer);

            return new ListDataResult(result, result.Count);
        }

        private IDataResult InternalExportInfoCitizensAppealCancelResult(ExportInfoCitizensAppealCancelResult exportParams, AppealCitsTransferResult transfer)
        {
            ArgumentChecker.NotNull(exportParams, "Не удалось получить информацию о передаваемых данных");
            try
            {
                this.client.ExportInfoCitizensAppealCancelResult(exportParams);
                this.EndExport(transfer);
            }
            catch (Exception e)
            {
                this.ErrorExport(transfer,
                    e.GetInnerExceptions()
                        .Select(x => x.Message)
                        .AggregateWithSeparator(Environment.NewLine));
                return BaseDataResult.Error(e.Message);
            }

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.client.Close();
                this.isDisposed = true;
            }
        }

        /// <inheritdoc />
        public void Initialize()
        {
            var configProvider = this.Container.Resolve<IConfigProvider>();

            using (this.Container.Using(configProvider))
            {
                var config = configProvider.GetConfig().GetModuleConfig("Bars.GkhGji.Regions.Chelyabinsk");
                var endpointAddress = config.GetAs("EndpointAddress", default(string), true);

                this.client = new CitizensAppealServiceSoapClient(new BasicHttpBinding(), new EndpointAddress(endpointAddress));
                this.client.Endpoint.EndpointBehaviors.Add(new MessageInspectorBehavior(this.messageContainer));
            }
        }

        private AppealCitsAnswer[] GetAnswers(AppealCits appealCits)
        {
            var result = new AppealCitsAnswer[0];
            this.Container.UsingForResolved<IDomainService<AppealCitsAnswer>>((ioc, domain) =>
            {
                result = domain.GetAll()
                    .Where(x => x.AppealCits.Id == appealCits.Id)
                    .Where(x=> x.State.Name == "Готов ответ")
                    .Where(x => x.IsUploaded != true)
                    .Fetch(x => x.FactCheckingType)
                    .Fetch(x => x.ConcederationResult)
                    .ToArray();
            });

            return result;
        }

        private IDictionary<long, AnswerAttachment[]> GetAttachments(long[] appealCitsAnswerIds)
        {
            var result = new Dictionary<long, AnswerAttachment[]>();
            this.Container.UsingForResolved<IDomainService<AppealCitsAnswerAttachment>>((ioc, domain) =>
            {
                result = domain.GetAll()
                    .WhereNotNull(x => x.FileInfo)
                    .WhereContainsBulked(x => x.AppealCitsAnswer.Id, appealCitsAnswerIds)
                    .Select(x => new
                    {
                        x.AppealCitsAnswer.Id,
                        x.UniqueName,
                        Name = $"{x.Name}.{x.FileInfo.Extention}",
                        x.Description
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id,
                        x => new AnswerAttachment
                        {

                            UniqueName = x.UniqueName,
                            Name = x.Name,
                            Description = x.Description
                        })
                    .ToDictionary(x => x.Key, x => x.ToArray());
            });

            return result;
        }

        private IDictionary<long, AppealQuestion[]> GetQuestions(long[] appealCitsAnswerIds)
        {
            var result = new Dictionary<long, AppealQuestion[]>();
            this.Container.UsingForResolved<IDomainService<AppealCitsAnswerStatSubject>>((ioc, domain) =>
            {
                result = domain.GetAll()
                    .WhereContainsBulked(x => x.AppealCitsAnswer.Id, appealCitsAnswerIds)
                    .WhereNotNull(x => x.StatSubject)
                    .Select(x => new
                    {
                        x.AppealCitsAnswer.Id,
                        // ReSharper disable once UseStringInterpolation
                        //Code = string.Format("0005.{0}.{1}.{2}",
                        //    x.StatSubject.Subject.QuestionCode,
                        //    x.StatSubject.Subsubject.QuestionCode,
                        //    x.StatSubject.Feature.QuestionCode),
                        Code = x.StatSubject.Subsubject.SSTUCodeSub,
                        StatementSubjectName = x.StatSubject.Subject.Name,
                        SubsubjectName = x.StatSubject.Subsubject.Name,
                        QuestionName = x.StatSubject.Feature.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => new AppealQuestion
                    {
                        Code = x.Code,
                        SectionName = "Жилищно-коммунальная сфера",
                        StatementSubjectName = x.StatementSubjectName,
                        SubsubjectName = x.SubsubjectName,
                        QuestionName = x.QuestionName
                    })
                    .ToDictionary(x => x.Key, x => x.ToArray());
            });

            return result;
        }

        private AppealCitsTransferResult StartExport(long appealId, AppealCitsTransferType transferType, BaseParams baseParams)
        {
            var result = new AppealCitsTransferResult
            {
                AppealCits = new AppealCits { Id = appealId },
                Type = transferType,
                User = new User { Id = this.UserIdentity.UserId },
                ExportParams = baseParams,
                StartDate = DateTime.Now,
                Status = AppealCitsTransferStatus.Running
            };

            this.AppealCitsTransferResultDomain.Save(result);
            return result;
        }

        private AppealCitsTransferResult ReStartExport(long transferResultId)
        {
            var result = this.AppealCitsTransferResultDomain.Load(transferResultId);
            if (result == null)
            {
                return null;
            }
            var logFile = result.LogFile;

            result.StartDate = DateTime.Now;
            result.EndDate = null;
            result.LogFile = null;

            result.Status = AppealCitsTransferStatus.Running;
            result.User = new User { Id = this.UserIdentity.UserId };

            this.AppealCitsTransferResultDomain.Update(result);
            if (logFile != null)
            {
                this.FileManager.Delete(logFile);
            }
            return result;
        }

        private void EndExport(AppealCitsTransferResult result)
        {
            this.Container.InTransaction(() =>
            {
                var response = this.messageContainer.ToString();
                if (!string.IsNullOrWhiteSpace(response))
                {
                    var fileName = result.Type.GetDisplayName();
                    var logFile = this.FileManager.SaveFile($"{fileName}.log", Encoding.UTF8.GetBytes(response));
                    result.LogFile = logFile;
                }

                result.EndDate = DateTime.Now;
                result.Status = AppealCitsTransferStatus.Success;

                this.AppealCitsTransferResultDomain.Update(result);
            });
        }

        private void ErrorExport(AppealCitsTransferResult result, string error)
        {
            this.Container.InTransaction(() =>
            {
                var message = $"-- ERROR --\r\n{error}\r\n----\r\n{this.messageContainer}";
                if (!string.IsNullOrWhiteSpace(message))
                {
                    var fileName = result.Type.GetDisplayName();
                    var logFile = this.FileManager.SaveFile($"{fileName}.log", Encoding.UTF8.GetBytes(message));
                    result.LogFile = logFile;
                }

                result.EndDate = DateTime.Now;
                result.Status = AppealCitsTransferStatus.Failure;

                this.AppealCitsTransferResultDomain.Update(result);
            });
        }

        internal class CompletionOfWorkResult
        {
            public long AnswerUid { get; set; }
            public bool IsUploaded { get; set; }
            public string AdditionalInformation { get; set; }
        }

        internal class ExportInfoCompletionOfWorkParams
        {
            public ExportInfoCompletionOfWorkResult[] ExportParams { get; set; }
            public long AppealCitsId { get; set; }
            public long[] AnswerIds { get; set; }
        }
    }
}