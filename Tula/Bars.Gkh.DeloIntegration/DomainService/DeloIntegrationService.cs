namespace Bars.Gkh.DeloIntegration.DomainService
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
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tula.Entities;
    using Castle.Windsor;
    using Entities;
    using Bars.Gkh.DeloIntegration.DomainService;
    using System.Collections.Generic;
    using Bars.GkhGji.Regions.Tula.Import;

    /// <summary>
    /// Интерфейс для интеграции с системой Дело
    /// </summary>
    public class DeloIntegrationService : IDeloIntegrationService
    {
        public IWindsorContainer Container { get; set; }

        public IFileManager FileManager { get; set; }

        private ILogImportManager logManager;

        private ILogImport logImport;

        private string FileName = "Вебсервис для Дело АСЭД";

        // Метод получения ответов которые небходимо отправить в Дело
        // Берутся только те ответы, которые не отправлены, и которые находятся на конечном статуса и обращение которых загружено из Дело
        // параметр ids для того, чтобы только по нужным получать ответы 
        public virtual AnswerRecord[] GetAnswers(long[] ids = null)
        {
            var answerDomain = Container.Resolve<IDomainService<AppealCitsAnswer>>();
            try
            {
                // получаем нужные на отправку ответы 
                var answers =  answerDomain.GetAll().Where(x => !x.IsMoved && x.State.FinalState && x.AppealCits.IsImported)
                    .WhereIf(ids != null, x => ids.Contains(x.Id))
                    .Select(x => new
                    {
                        x.AppealCits.NumberGji,
                        x.Id,
                        x.DocumentNumber,
                        x.File
                    })
                    .ToList();

                var res = new AnswerRecord[answers.Count()];
                int i = 0;
                // формируем результат и проставляем Фалы там где нужно 
                foreach(var a in answers)
                {
                    var answer = new AnswerRecord(){
                        AppealNumber = a.NumberGji,
                        AnswerId = a.Id,
                        AnswerNumber = a.DocumentNumber,
                        FileId = a.File != null ? a.File.Id : 0,
                        FileName = a.File != null ? a.File.FullName : null
                    };

                    if( answer.FileId > 0 ) 
                    {
                        var file = FileManager.GetFile(a.File);
                        
                        var bytes = new byte[file.Length];
                        file.Seek(0, SeekOrigin.Begin);
                        file.Read(bytes, 0, (int)file.Length);
                        file.Seek(0, SeekOrigin.Begin);
                         
                        answer.File = bytes;
                    }

                    res[i] = answer;
                    i++;
                }

                return res;
            }
            finally
            {
                Container.Release(answerDomain);
            }
        }

        // Метод сохранения полученных ответов 
        public virtual IDataResult UpdateAnswers(List<AnswerRecord> answers)
        {
            var answerDomain = Container.Resolve<IDomainService<AppealCitsAnswer>>();

            try
            {
                InitLog(FileName, "DeloServiceAnswersImport");
                var countSuccess = 0;
                var errorCount = 0;
                var counter = 0;

                var ids = answers.Where(x => x.AnswerId > 0).Select(x => x.AnswerId).ToList();

                var currentDict = answerDomain.GetAll()
                                    .Where(x => ids.Contains(x.Id))
                                    .AsEnumerable()
                                    .GroupBy(x => x.Id)
                                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                var listToSave = new List<AppealCitsAnswer>();

                foreach(var a in answers)
                {
                    var obj = currentDict.Get(a.AnswerId);

                    if (obj == null)
                    {
                        errorCount++;
                        logImport.Error("Ошибка вебсервиса Дело АСЭД", string.Format("Нет записи с ответом по Id:{0}. Ответ был удален в системе БАРС", a.AnswerId));
                    }
                    else if ( string.IsNullOrEmpty(obj.DocumentNumber))
                    {
                        countSuccess++;
                        logImport.Info("Ответ интегрирован успешно", string.Format("Номер ответа: {0}, Id ответа: {1}", a.AnswerNumber, a.AnswerId));
                        obj.DocumentNumber = a.AnswerNumber;
                        obj.DocumentDate = DateTime.Now;
                        obj.IsMoved = true; // помечаем чтобы в следующий раз этот ответ не брался для интеграции
                        listToSave.Add(obj);
                    }
                    else
                    {
                        errorCount++;
                        logImport.Error("Ошибка вебсервиса Дело АСЭД", string.Format("Ответ по Id:{0} уже содержит номер", a.AnswerId));
                    }
                }

                if (listToSave.Any())
                {
                    using (var tr = Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            foreach (var item in listToSave)
                            {
                                answerDomain.Update(item);
                            }

                            tr.Commit();

                            FinishLog(countSuccess, errorCount, "UpdateAnswers");
                        }
                        catch (Exception e)
                        {
                            tr.Rollback();
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch(Exception exc)
            {
                return new BaseDataResult(false, exc.Message);
            }
            finally
            {
                Container.Release(answerDomain);
            }
        }
        public void InitLog(string fileName, string key)
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
            logImport.ImportKey = key;
            logImport.SetFileName(fileName);
        }

        public void FinishLog(int success, int errors, string fileName, bool isImported = true)
        {
            logImport.CountAddedRows = success;
            logImport.CountError = errors;
            logManager.Add(new MemoryStream(), fileName, logImport);
            logImport.IsImported = isImported;
            logManager.UploadDate = DateTime.Now;
            logManager.Save();
        }
    }
}
