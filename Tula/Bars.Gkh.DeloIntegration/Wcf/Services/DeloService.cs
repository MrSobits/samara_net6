namespace Bars.Gkh.DeloIntegration.Wcf.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.ServiceModel.Activation;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.DeloIntegration.Wcf.Contracts;
    using Bars.Gkh.Import;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tula.Entities;
    using Castle.Windsor;
    using Entities;
    using Bars.Gkh.DeloIntegration.DomainService;
    using Bars.GkhGji.Regions.Tula.Import;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DeloService : IDeloService
    {
        public IWindsorContainer Container { get; set; }

        private ILogImportManager logManager;

        private ILogImport logImport;

        private const string FileName = "Вебсервис для Дело АСЭД";

        private string Key = "DeloServiceImport";

        private int errorCount = 0;

        public DeloResult[] SetAppeals(DeloAppealList appeals)
        {
            this.InitLog(FileName);
            var countSuccess = 0;
            errorCount = 0;
            var counter = 0;
            var result = new DeloResult[appeals.Count];
            foreach (var item in appeals)
            {
                try
                {
                    this.SaveAppealRequest(item);
                    countSuccess++;
                    result[counter] = new DeloResult {Success = true, ExtId = item.ExtId, Message = ""};
                }
                catch (Exception e)
                {
                    logImport.Error("Ошибка вебсервиса Дело АСЭД", e.ToString());
                    errorCount++;
                    result[counter] = new DeloResult {Success = false, ExtId = item.ExtId, Message = e.ToString()};
                }
                counter++;
            }
            this.FinishLog(countSuccess, errorCount);
            return result;
        }

        public DeloResult SetAppeal(DeloAppeal appeal)
        {
            this.InitLog(FileName);
            errorCount = 0;
            try
            {
                this.SaveAppealRequest(appeal);
                this.FinishLog(1, errorCount);
                return new DeloResult {Success = true, ExtId = appeal.ExtId, Message = ""};
            }
            catch (Exception e)
            {
                logImport.Error("Ошибка вебсервиса Дело АСЭД", e.ToString());
                this.FinishLog(0, errorCount + 1, false);
                return new DeloResult {Success = false, Message = e.ToString()};
            }
        }
        
        public GetAnswerResponse GetAnswers()
        {
            var deloService = Container.Resolve<IDeloIntegrationService>();
            var result = new GetAnswerResponse();
            try
            {
                var answers = deloService.GetAnswers();

                result.Records = answers;
                result.Success = true;
                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
                return result;

            }
            finally
            {
                Container.Release(deloService);
            }
        }

        public UpdateAnswerResponse UpdateAnswers(AnswerList answers)
        {
            var deloService = Container.Resolve<IDeloIntegrationService>();
            var result = new UpdateAnswerResponse();

            try
            {
                if (answers == null)
                    throw new Exception("Необходимо передать список ответов");

                if (!answers.Any())
                    throw new Exception("Необходимо передать список ответов");

                var res = deloService.UpdateAnswers(answers);
                result.Success = res.Success;
                
                if(res.Success)
                {
                    result.Message = res.Message;
                }

                return result;
            }
            catch (Exception e)
            {
                result.Success = false;
                result.Message = e.Message;
                return result;
            }
            finally
            {
                Container.Release(deloService);
            }
        }
        
        /// <summary> Обработка записи обращение из Дело АСЭД </summary>
        protected void SaveAppealRequest(DeloAppeal appeal)
        {
            // Обращение граждан
            var appealCits = CreateOrUdateAppealCits(appeal);
            // Источники поступления
            CreateAppealCitsSource(appealCits, appeal);
            // Рассмотрение
            CreateAppealCitsExecutant(appealCits, appeal);
            // Ответы
            CreateAppealCitsAnswer(appealCits, appeal);
        }

        protected AppealCits CreateOrUdateAppealCits(DeloAppeal appeal)
        {
            var domain = Container.Resolve<IDomainService<AppealCits>>();
            var entity =
                domain.GetAll().FirstOrDefault(x => x.NumberGji.ToLower().Trim() == appeal.ExtId.ToLower().Trim());
            if (entity != null)
            {
                entity.DateFrom = appeal.StartDate;
                entity.CheckTime = appeal.TargetDate;
                entity.Correspondent = appeal.Citizen;
                entity.CorrespondentAddress = appeal.Address;
                entity.Description = appeal.Description;
                entity.CitizenId = appeal.CitizenId;
                entity.IsImported = true;
                domain.Update(entity);
            }
            else
            {
                entity = new AppealCits()
                {
                    NumberGji = appeal.ExtId,
                    DateFrom = appeal.StartDate,
                    CheckTime = appeal.TargetDate,
                    Correspondent = appeal.Citizen,
                    CorrespondentAddress = appeal.Address,
                    Description = appeal.Description,
                    CitizenId = appeal.CitizenId,
                    IsImported = true
                };
                domain.Save(entity);
            }
            return entity;
        }

        #region appealcits mutators

        protected void CreateAppealCitsSource(AppealCits entity, DeloAppeal appeal)
        {
            var domainSource = Container.Resolve<IDomainService<AppealCitsSource>>();
            var entitySource = new AppealCitsSource
            {
                AppealCits = entity,
                RevenueSource = this.GetOrCreateRevenueSource(appeal.Source),
                RevenueSourceNumber = appeal.Num,
                RevenueForm = this.GetOrCreateRevenueForm(appeal.Form),
                RevenueDate = appeal.ReciveDate
            };
            domainSource.Save(entitySource);
        }

        protected void CreateAppealCitsExecutant(AppealCits entity, DeloAppeal appeal)
        {
            try
            {
                var domainExecutant = Container.Resolve<IDomainService<AppealCitsExecutant>>();
                var entityExecutant = new AppealCitsExecutant
                {
                    AppealCits = entity,
                    Author = GetOrCreateInspector(appeal.Guarantor, appeal.Post)
                };

                if (appeal.Performer.IsNotEmpty() && appeal.PerformersPost.IsNotEmpty())
                {
                    entityExecutant.Executant = GetOrCreateInspector(appeal.Performer, appeal.PerformersPost);
                }

                domainExecutant.Save(entityExecutant);
            }
            catch (Exception e)
            {
                logImport.Error(
                    string.Format("Ошибка вебсервиса Дело АСЭД. Номер обращения {0}", appeal.ExtId),
                    e.ToString());
                errorCount++;
            }
        }

        protected void CreateAppealCitsAnswer(AppealCits entity, DeloAppeal appeal)
        {
            var domainAnswer = Container.Resolve<IDomainService<AppealCitsAnswer>>();
            var entityAnswer = new AppealCitsAnswer
            {
                AppealCits = entity,
                Addressee = GetOrCreateRevenueSource(appeal.Addressee),
                DocumentNumber = appeal.DocNumber,
                DocumentDate = appeal.DocDate,
                File =
                    appeal.File != null
                        ? Container.Resolve<IFileManager>().SaveFile(new MemoryStream(appeal.File), appeal.FileName)
                        : null
            };
            domainAnswer.Save(entityAnswer);
        }

        #endregion

        protected RevenueSourceGji GetOrCreateRevenueSource(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }
            var domain = Container.Resolve<IDomainService<RevenueSourceGji>>();
            var entity = domain.GetAll().FirstOrDefault(x => x.Name.ToLower() == source.ToLower());
            if (entity == null)
            {
                entity = new RevenueSourceGji {Name = source, Code = source};
                domain.Save(entity);
            }
            return entity;
        }

        protected RevenueFormGji GetOrCreateRevenueForm(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }
            var domain = Container.Resolve<IDomainService<RevenueFormGji>>();
            var entity = domain.GetAll().FirstOrDefault(x => x.Name.ToLower() == source.ToLower());
            if (entity == null)
            {
                entity = new RevenueFormGji {Name = source, Code = source};
                domain.Save(entity);
            }
            return entity;
        }

        protected Inspector GetOrCreateInspector(string fio, string position)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(fio))
            {
                error += "Поле \"Поручитель\" пусто; ";
            }
            if (string.IsNullOrEmpty(position))
            {
                error += "Поле \"Должность\" пусто";
            }
            if (!string.IsNullOrEmpty(error))
            {
                throw new Exception(error);
            }

            var domainInspector = Container.Resolve<IDomainService<Inspector>>();

            var inspector =
                domainInspector.GetAll()
                    .FirstOrDefault(
                        x => x.Fio.ToLower() == fio.ToLower() && x.Position.ToLower() == position.ToLower());
            var lastCodeStr = domainInspector.GetAll().OrderByDescending(x => x.Id).FirstOrDefault();
            var lastCode = lastCodeStr != null ? Convert.ToInt32(lastCodeStr.Code) + 1 : 0;
            if (inspector == null)
            {
                inspector = new Inspector
                {
                    Fio = fio,
                    Position = position,
                    Code = (lastCode).ToString()
                };
                domainInspector.Save(inspector);
            }

            return inspector;
        }

        private void InitLog(string fileName)
        {
            this.logManager = Container.Resolve<ILogImportManager>();
            if (this.logManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.logManager.FileNameWithoutExtention = fileName;
            this.logManager.UploadDate = DateTime.Now;

            var logImports = Container.ResolveAll<ILogImport>();

            logImport = logImports.FirstOrDefault(x => x.Key == MainLogImportInfo.Key);
            if (logImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }
            logImport.ImportKey = Key;
            logImport.SetFileName(fileName);
        }

        private void FinishLog(int success, int errors, bool isImported = true)
        {
            logImport.CountAddedRows = success;
            logImport.CountError = errors;
            logManager.Add(new MemoryStream(), "SetAppeals", logImport);
            logImport.IsImported = isImported;
            logManager.UploadDate = DateTime.Now;
            logManager.Save();
        }
    }
}