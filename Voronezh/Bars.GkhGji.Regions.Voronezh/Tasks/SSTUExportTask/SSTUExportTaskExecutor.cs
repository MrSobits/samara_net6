using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;

using Bars.B4;
using Bars.B4.Application;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Modules.Tasks.Common.Utils;
using Bars.Gkh.Domain;
using Bars.GkhGji.Controllers;
using Bars.GkhGji.DomainService;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Entities.Dict;
using Bars.GkhGji.Entities.SurveyPlan;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Castle.Windsor;
using Newtonsoft.Json;

namespace Bars.GkhGji.Regions.Voronezh.Tasks
{
    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class SSTUExportTaskExecutor : ITaskExecutor
    { 
        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;
        private ITaskManager _taskManager;
        private IWindsorContainer container;
        

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IFileManager _fileManager { get; set; }

        public IDomainService<AppealCitsStatSubject> AppealCitsStatSubjectDomain { get; set; }

        public IDomainService<AppealCitsAnswer> AppealCitsAnswerDomain { get; set; }

        public IDomainService<AppealCits> AppealCitsDomain { get; set; }

        public IDomainService<AppealCitsSource> AppealCitsSourceDomain { get; set; }

        public IDomainService<SSTUExportTaskAppeal> SSTUExportTaskAppealDomain { get; set; }

        public IDomainService<SSTUExportTask> SSTUExportTaskDomain { get; set; }

        public IDomainService<SSTUExportedAppeal> SSTUExportedAppealDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public IDataResult Execute(
            BaseParams @params,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            SSTUExportTask sSTUExportData = SSTUExportTaskDomain.Get(long.Parse((string)@params.Params["taskId"]));
            if (sSTUExportData == null)
                return new BaseDataResult(false, $"Запрос с ID {@params.Params["taskId"]} не найден");

            //отправка
            try
            {
                var taskId = sSTUExportData.Id;
                MemoryStream stream = new MemoryStream();
                StreamWriter sw = new StreamWriter(stream, Encoding.UTF8);
                sw.WriteLine(taskId);

                var appcitTask = SSTUExportTaskDomain.Get(taskId);

                bool isDirect = appcitTask.SSTUSource == SSTUSource.Direct;
                //   StreamWriter sw = new StreamWriter("C:\\FileStore\\exportlog" + taskId + ".txt");

                Dictionary<string, string> AppStateDict = new Dictionary<string, string>();
                AppStateDict.Add("Новое", "InWork");
                AppStateDict.Add("В работе", "InWork");
                AppStateDict.Add("СОПР вх", "InWork");
                AppStateDict.Add("СОПР исх", "InWork");
                AppStateDict.Add("Закрыто", "Explained");
                const string Url = "http://10.0.80.2/IPCP/HandlingReportPlugin/Api/Import";

                try
                {
                    sw.WriteLine(appcitTask.Operator.Login);
                    var appeals = SSTUExportTaskAppealDomain.GetAll()
                        .Where(x => x.SSTUExportTask.Id == taskId)
                        .Select(x => x.AppealCits.Id).ToList();

                    foreach (Int64 appId in appeals)
                    {
                        sw.WriteLine(appId.ToString());
                        var appealForExport = AppealCitsDomain.Get(appId);
                        if (appealForExport.QuestionStatus == QuestionStatus.Plizdu)
                        {
                            appealForExport.QuestionStatus = QuestionStatus.Supported;
                        }
                        sw.WriteLine("номер ГЖИ - " + appealForExport.DocumentNumber);
                        var appealForExportSource = AppealCitsSourceDomain.GetAll()
                            .Where(x => x.AppealCits.Id == appealForExport.Id).FirstOrDefault();
                        var statSubject = AppealCitsStatSubjectDomain.GetAll()
                       .Where(x => x.AppealCits.Id == appId).FirstOrDefault();
                        if (string.IsNullOrEmpty(statSubject.ExportCode))
                        {
                            statSubject.ExportCode = statSubject.Subject.SSTUCode;
                        }
                        
                        AppealCitsStatSubjectDomain.Update(statSubject);

                        if (appealForExport.QuestionStatus == QuestionStatus.Transferred)
                        {
                            List<SSTUQuestionsTransfer> questList = new List<SSTUQuestionsTransfer>();
                            sw.WriteLine("Трансфер");
                            string status = "Transferred";

                            var appCitAnswer = AppealCitsAnswerDomain.GetAll()
                           .Where(x => x.AppealCits.Id == appId && x.File != null && x.DocumentDate != null).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (appealForExport.SSTUTransferOrg == null || string.IsNullOrEmpty(appealForExport.SSTUTransferOrg.Guid))
                            {
                                sw.WriteLine("Не заполнена организация - получатель сообщения");
                                continue;
                            }
                            if (appCitAnswer == null)
                            {
                                sw.WriteLine("Не прикреплен ответ на обращение");
                                continue;
                            }
                            SSTUTransfer transfer = new SSTUTransfer()
                            {
                                departmentId = appealForExport.SSTUTransferOrg.Guid,
                                //      transferDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                transferDate = appCitAnswer.DocumentDate.HasValue ? appCitAnswer.DocumentDate.Value.ToString("yyyy-MM-dd") : appCitAnswer.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                // transferNumber = DocumentNumber по требованию ГЖИ номер исходящего берется из ответа
                                transferNumber = appCitAnswer.DocumentNumber != "" ? appCitAnswer.DocumentNumber : appealForExport.DocumentNumber
                            };

                            SSTUQuestionsTransfer question = new SSTUQuestionsTransfer()
                            {
                                code = string.IsNullOrEmpty(statSubject.ExportCode) ? statSubject.ExportCode : statSubject.Subject.SSTUCode,
                                status = status,
                                incomingDate = appealForExportSource.RevenueDate.HasValue ? appealForExportSource.RevenueDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                registrationDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                transfer = transfer,
                            };
                            questList.Add(question);
                            string json;
                            if (isDirect)
                            {
                                SSTUAppealTransfer sstuAppeal = new SSTUAppealTransfer()
                                {
                                    departmentId = "6bc82a49-6a97-43f4-9e42-a07d417b60b6",
                                    isDirect = isDirect,
                                    format = "Other",
                                    number = appealForExport.DocumentNumber,
                                    name = appealForExport.Correspondent,
                                    createDate = appealForExportSource.RevenueDate.HasValue ? appealForExportSource.RevenueDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                    address = appealForExport.CorrespondentAddress,
                                    questions = questList.ToArray()
                                };

                                json = JsonConvert.SerializeObject(sstuAppeal);
                            }
                            else
                            {
                                SSTUAppealTransferCD sstuAppeal = new SSTUAppealTransferCD()
                                {
                                    departmentId = "6bc82a49-6a97-43f4-9e42-a07d417b60b6",
                                    isDirect = isDirect,
                                    format = "Other",
                                    number = appealForExport.DocumentNumber,
                                    createDate = appealForExportSource.SSTUDate.HasValue ? appealForExportSource.SSTUDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                    name = appealForExport.Correspondent,
                                    address = appealForExport.CorrespondentAddress,
                                    questions = questList.ToArray()
                                };

                                json = JsonConvert.SerializeObject(sstuAppeal);
                            }

                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Method = "POST";

                            try
                            {

                                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                                {
                                    streamWriter.Write(json);
                                    streamWriter.Flush();
                                    streamWriter.Close();
                                }
                                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                appealForExport.SSTUExportState = SSTUExportState.Exported;
                                AppealCitsDomain.Update(appealForExport);
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var responseResult = streamReader.ReadToEnd();
                                    sw.Write("result" + responseResult);
                                    appcitTask.SSTUExportState = SSTUExportState.Exported;
                                    var appCitExported = AppealCitsDomain.Get(appId);
                                    var exportedAppeals = SSTUExportedAppealDomain.GetAll()
                                        .Where(x => x.AppealCits.Id == appId)
                                        .Select(x => x.Id).FirstOrDefault();
                                    if (exportedAppeals > 0)
                                    {
                                        SSTUExportedAppeal newApp = new SSTUExportedAppeal();
                                        newApp.AppealCits = appCitExported;
                                        newApp.ExportDate = DateTime.Now;
                                        newApp.ObjectVersion = 0;
                                        newApp.ObjectCreateDate = DateTime.Now;
                                        newApp.ObjectEditDate = DateTime.Now;
                                        SSTUExportedAppealDomain.Save(newApp);
                                    }

                                    //  sw.Close();
                                }

                            }
                            catch (Exception e)
                            {
                                sw.WriteLine(json);
                                var err = e.ToString();
                                sw.WriteLine(err);
                                appealForExport.SSTUExportState = SSTUExportState.Error;
                                AppealCitsDomain.Update(appealForExport);
                                appcitTask.SSTUExportState = SSTUExportState.Error;
                            }


                        }
                        else
                        {
                            List<SSTUQuestions> questList = new List<SSTUQuestions>();
                            List<SSTUQuestionsNoAnswer> questListNoAnswer = new List<SSTUQuestionsNoAnswer>();

                            //var statSubject = AppealCitsStatSubjectDomain.GetAll()
                            //    .Where(x => x.AppealCits.Id == appId).OrderByDescending(x => x.Id).FirstOrDefault();
                            sw.WriteLine("statSubject " + statSubject.Subject.SSTUCode);
                            var appCitAnswer = AppealCitsAnswerDomain.GetAll()
                                .Where(x => x.AppealCits.Id == appId && x.File != null && x.DocumentDate != null).OrderByDescending(x => x.Id).FirstOrDefault();

                            if (statSubject.Subject.SSTUCode == "")
                            {
                                sw.WriteLine("statSubject.Subject.SSTUCode == null");
                                appealForExport.SSTUExportState = SSTUExportState.Error;
                                AppealCitsDomain.Update(appealForExport);
                                continue;
                            }
                            var t = appealForExport.State.Name;
                            string status = "";
                            if (AppStateDict.ContainsKey(t))
                            {
                                if (t == "Закрыто" && appCitAnswer == null)
                                {
                                    status = "LeftWithoutAnswer";
                                }
                                else
                                {
                                    status = AppStateDict[t];
                                }
                            }
                            else
                            {
                                sw.WriteLine("!AppStateDict.ContainsKey(t)");
                                appealForExport.SSTUExportState = SSTUExportState.Error;
                                AppealCitsDomain.Update(appealForExport);
                                continue;
                            }


                            string fileName = "";
                            string fileContent = "";
                            if (appCitAnswer != null && appCitAnswer.File != null && appCitAnswer.File.Name != "")
                            {
                                fileName = appCitAnswer.File.Name + "." + appCitAnswer.File.Extention;
                                sw.WriteLine(fileName);
                                var fm = _fileManager;
                                fileContent = fm.GetBase64String(appCitAnswer.File);
                                sw.WriteLine("2 - " + appCitAnswer.File.FullName);
                                SSTUAttachment attachment = new SSTUAttachment()
                                {
                                    name = fileName,
                                    content = fileContent
                                };
                                sw.WriteLine("3");
                                sw.WriteLine("4" + statSubject.ExportCode);
                                sw.WriteLine("6" + (appealForExportSource.SSTUDate.HasValue ? appealForExportSource.SSTUDate.Value.ToShortDateString() : ""));
                                if (!string.IsNullOrEmpty(statSubject.ExportCode) && statSubject.ExportCode.Length > 1 && appealForExportSource.SSTUDate != null)
                                {
                                    sw.WriteLine("7");
                                    sw.WriteLine("Сопоставленное обращение, код - " + statSubject.ExportCode);
                                    string incDate = string.Empty;
                                    if (appealForExportSource.SSTUDate.HasValue)
                                    {
                                        if (appealForExportSource.RevenueDate.HasValue)
                                        {
                                            if (appealForExportSource.RevenueDate.Value < appealForExportSource.SSTUDate.Value)
                                            {
                                                incDate = appealForExportSource.SSTUDate.Value.ToString("yyyy-MM-dd");
                                            }
                                            else
                                            {
                                                incDate = appealForExportSource.RevenueDate.Value.ToString("yyyy-MM-dd");
                                            }
                                        }
                                        else
                                        {
                                            appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd");
                                        }
                                    }
                                    else
                                    {
                                        if (appealForExportSource.RevenueDate.HasValue)
                                        {
                                            incDate = appealForExportSource.RevenueDate.Value.ToString("yyyy-MM-dd");
                                        }
                                        else
                                        {
                                            appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd");
                                        }

                                    }
                                    if (appealForExport.QuestionStatus == QuestionStatus.Supported)
                                    {
                                        SSTUQuestions question = new SSTUQuestions()
                                        {
                                            code = statSubject.ExportCode,
                                            status = appealForExport.QuestionStatus != QuestionStatus.NotSet ? appealForExport.QuestionStatus.ToString() : status,
                                            incomingDate = incDate,
                                            registrationDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            responseDate = appCitAnswer.DocumentDate.HasValue ? appCitAnswer.DocumentDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectEditDate.ToString("yyyy-MM-dd"),
                                            //  actionsTaken = true,
                                            attachment = attachment,
                                        };
                                        questList.Add(question);
                                    }
                                    else
                                    {
                                        SSTUQuestions question = new SSTUQuestions()
                                        {
                                            code = statSubject.ExportCode,
                                            status = appealForExport.QuestionStatus != QuestionStatus.NotSet ? appealForExport.QuestionStatus.ToString() : status,
                                            incomingDate = incDate,
                                            registrationDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            responseDate = appCitAnswer.DocumentDate.HasValue ? appCitAnswer.DocumentDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectEditDate.ToString("yyyy-MM-dd"),
                                            attachment = attachment,
                                        };
                                        questList.Add(question);
                                    }

                                }
                                else
                                {
                                    sw.WriteLine("8");
                                    sw.WriteLine("Не сопоставленное обращение, код - " + statSubject.Subject.SSTUCode);
                                    string incDate = string.Empty;
                                    if (appealForExportSource.SSTUDate.HasValue)
                                    {
                                        if (appealForExportSource.RevenueDate.HasValue)
                                        {
                                            if (appealForExportSource.RevenueDate.Value < appealForExportSource.SSTUDate.Value)
                                            {
                                                incDate = appealForExportSource.SSTUDate.Value.ToString("yyyy-MM-dd");
                                            }
                                            else
                                            {
                                                incDate = appealForExportSource.RevenueDate.Value.ToString("yyyy-MM-dd");
                                            }
                                        }
                                        else
                                        {
                                            incDate = appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd");
                                        }
                                    }
                                    else
                                    {
                                        if (appealForExportSource.RevenueDate.HasValue)
                                        {
                                            incDate = appealForExportSource.RevenueDate.Value.ToString("yyyy-MM-dd");
                                        }
                                        else
                                        {
                                            incDate = appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd");
                                        }

                                    }
                                    if (appealForExport.QuestionStatus == QuestionStatus.Supported)
                                    {
                                        SSTUQuestions question = new SSTUQuestions()
                                        {
                                            code = string.IsNullOrEmpty(statSubject.ExportCode) ? statSubject.ExportCode : statSubject.Subject.SSTUCode,
                                            status = appealForExport.QuestionStatus != QuestionStatus.NotSet ? appealForExport.QuestionStatus.ToString() : status,
                                            incomingDate = incDate,
                                          //  incomingDate = appealForExportSource.RevenueDate.HasValue ? appealForExportSource.RevenueDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            registrationDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            responseDate = appCitAnswer.DocumentDate.HasValue ? appCitAnswer.DocumentDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectEditDate.ToString("yyyy-MM-dd"),
                                            //  actionsTaken = true,
                                            attachment = attachment,
                                        };
                                        questList.Add(question);
                                    }
                                    else
                                    {
                                        SSTUQuestions question = new SSTUQuestions()
                                        {
                                            code = string.IsNullOrEmpty(statSubject.ExportCode) ? statSubject.ExportCode : statSubject.Subject.SSTUCode,
                                            status = appealForExport.QuestionStatus != QuestionStatus.NotSet ? appealForExport.QuestionStatus.ToString() : status,
                                            incomingDate = incDate,
                                          //  incomingDate = appealForExportSource.RevenueDate.HasValue ? appealForExportSource.RevenueDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            registrationDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            responseDate = appCitAnswer.DocumentDate.HasValue ? appCitAnswer.DocumentDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectEditDate.ToString("yyyy-MM-dd"),
                                            attachment = attachment,
                                        };
                                        questList.Add(question);
                                    }
                                }

                                string json;
                                if (isDirect)
                                {
                                    SSTUAppeal sstuAppeal = new SSTUAppeal()
                                    {
                                        departmentId = "6bc82a49-6a97-43f4-9e42-a07d417b60b6",
                                        isDirect = isDirect,
                                        format = "Other",
                                        number = isDirect ? appealForExport.DocumentNumber : appealForExportSource.RevenueSourceNumber,
                                        createDate = appealForExportSource.SSTUDate.HasValue ? appealForExportSource.SSTUDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                        name = appealForExport.Correspondent,
                                        address = appealForExport.CorrespondentAddress,
                                        questions = questList.ToArray()
                                    };
                                    json = JsonConvert.SerializeObject(sstuAppeal);
                                }
                                else
                                {
                                    SSTUAppealCD sstuAppeal = new SSTUAppealCD()
                                    {
                                        departmentId = "6bc82a49-6a97-43f4-9e42-a07d417b60b6",
                                        isDirect = isDirect,
                                        format = "Other",
                                        number = isDirect ? appealForExport.DocumentNumber : appealForExportSource.RevenueSourceNumber,
                                        createDate = appealForExportSource.SSTUDate.HasValue ? appealForExportSource.SSTUDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                        name = appealForExport.Correspondent,
                                        address = appealForExport.CorrespondentAddress,
                                        questions = questList.ToArray()
                                    };
                                    json = JsonConvert.SerializeObject(sstuAppeal);
                                }

                                var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                                httpWebRequest.ContentType = "application/json";
                                httpWebRequest.Method = "POST";

                                try
                                {

                                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                                    {
                                        streamWriter.Write(json);
                                        streamWriter.Flush();
                                        streamWriter.Close();
                                    }
                                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                    appealForExport.SSTUExportState = SSTUExportState.Exported;
                                    AppealCitsDomain.Update(appealForExport);
                                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                    {
                                        var responseResult = streamReader.ReadToEnd();
                                        sw.Write("result" + responseResult);
                                        appcitTask.SSTUExportState = SSTUExportState.Exported;

                                        var appCitExported = AppealCitsDomain.Get(appId);
                                        var exportedAppeals = SSTUExportedAppealDomain.GetAll()
                                            .Where(x => x.AppealCits.Id == appId)
                                            .Select(x => x.Id).FirstOrDefault();
                                        if (exportedAppeals != null)
                                        {
                                            SSTUExportedAppeal newApp = new SSTUExportedAppeal();
                                            newApp.AppealCits = appCitExported;
                                            newApp.ExportDate = DateTime.Now;
                                            newApp.ObjectVersion = 0;
                                            newApp.ObjectCreateDate = DateTime.Now;
                                            newApp.ObjectEditDate = DateTime.Now;
                                            SSTUExportedAppealDomain.Save(newApp);
                                        }

                                        //  sw.Close();
                                    }

                                    SSTUExportTaskDomain.Update(appcitTask);

                                }
                                catch (Exception e)
                                {
                                    sw.WriteLine(json);
                                    var err = e.ToString();
                                    sw.WriteLine(err);
                                    appealForExport.SSTUExportState = SSTUExportState.Error;
                                    AppealCitsDomain.Update(appealForExport);
                                    appcitTask.SSTUExportState = SSTUExportState.Error;
                                }
                            }
                            else
                            {
                                string json = string.Empty;                               

                                if (isDirect)
                                {
                                    if (appealForExport.QuestionStatus == QuestionStatus.Supported)
                                    {
                                        SSTUQuestionsNoAnswer question = new SSTUQuestionsNoAnswer()
                                        {
                                            code = string.IsNullOrEmpty(statSubject.ExportCode) ? statSubject.ExportCode : statSubject.Subject.SSTUCode,
                                            status = appealForExport.QuestionStatus != QuestionStatus.NotSet ? appealForExport.QuestionStatus.ToString() : status,
                                            incomingDate = appealForExportSource.RevenueDate.HasValue ? appealForExportSource.RevenueDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            //     incomingDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            registrationDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                        };
                                        questListNoAnswer.Add(question);
                                    }
                                    else
                                    {
                                        SSTUQuestionsNoAnswer question = new SSTUQuestionsNoAnswer()
                                        {
                                            code = string.IsNullOrEmpty(statSubject.ExportCode) ? statSubject.ExportCode : statSubject.Subject.SSTUCode,
                                            status = appealForExport.QuestionStatus != QuestionStatus.NotSet ? appealForExport.QuestionStatus.ToString() : status,
                                            incomingDate = appealForExportSource.RevenueDate.HasValue ? appealForExportSource.RevenueDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            //     incomingDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            registrationDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                        };
                                        questListNoAnswer.Add(question);
                                    }

                                    SSTUAppealNoAnswer sstuAppealNoAnswer = new SSTUAppealNoAnswer()
                                    {
                                        departmentId = "6bc82a49-6a97-43f4-9e42-a07d417b60b6",
                                        isDirect = isDirect,
                                        format = "Other",
                                        number = isDirect ? appealForExport.DocumentNumber : appealForExportSource.RevenueSourceNumber,
                                        createDate = appealForExportSource.SSTUDate.HasValue ? appealForExportSource.SSTUDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                        name = appealForExport.Correspondent,
                                        address = appealForExport.CorrespondentAddress,
                                        questions = questListNoAnswer.ToArray()
                                    };
                                    json = JsonConvert.SerializeObject(sstuAppealNoAnswer);
                                }
                                else
                                {
                                    if (statSubject.ExportCode != "" && statSubject.ExportCode.Length > 1 && appealForExportSource.SSTUDate != null)
                                    {

                                        sw.WriteLine("Сопоставленное обращение, код - " + statSubject.ExportCode);
                                        string incDate = string.Empty;
                                        if (appealForExportSource.SSTUDate.HasValue)
                                        {
                                            if (appealForExportSource.RevenueDate.HasValue)
                                            {
                                                if (appealForExportSource.RevenueDate.Value < appealForExportSource.SSTUDate.Value)
                                                {
                                                    incDate = appealForExportSource.SSTUDate.Value.ToString("yyyy-MM-dd");
                                                }
                                                else
                                                {
                                                    incDate = appealForExportSource.RevenueDate.Value.ToString("yyyy-MM-dd");
                                                }
                                            }
                                            else
                                            {
                                                appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd");
                                            }
                                        }
                                        else
                                        {
                                            if (appealForExportSource.RevenueDate.HasValue)
                                            {
                                                incDate = appealForExportSource.RevenueDate.Value.ToString("yyyy-MM-dd");
                                            }
                                            else
                                            {
                                                appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd");
                                            }

                                        }

                                        SSTUQuestionsNoAnswer question = new SSTUQuestionsNoAnswer()
                                        {
                                            code = statSubject.ExportCode,
                                            status = appealForExport.QuestionStatus != QuestionStatus.NotSet ? appealForExport.QuestionStatus.ToString() : status,
                                            incomingDate = incDate,
                                            registrationDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                        };
                                        questListNoAnswer.Add(question);

                                        SSTUAppealNoAnswer sstuAppealNoAnswer = new SSTUAppealNoAnswer()
                                        {
                                            departmentId = "6bc82a49-6a97-43f4-9e42-a07d417b60b6",
                                            isDirect = isDirect,
                                            format = "Other",
                                            number = isDirect ? appealForExport.DocumentNumber : appealForExportSource.RevenueSourceNumber,
                                            createDate = appealForExportSource.SSTUDate.HasValue ? appealForExportSource.SSTUDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            name = appealForExport.Correspondent,
                                            address = appealForExport.CorrespondentAddress,
                                            questions = questListNoAnswer.ToArray()
                                        };
                                        json = JsonConvert.SerializeObject(sstuAppealNoAnswer);
                                    }
                                    else
                                    {
                                        sw.WriteLine("Не сопоставленное обращение, код - " + statSubject.Subject.SSTUCode);
                                        SSTUQuestionsNoAnswer question = new SSTUQuestionsNoAnswer()
                                        {
                                            code = statSubject.Subject.SSTUCode,
                                            status = appealForExport.QuestionStatus != QuestionStatus.NotSet ? appealForExport.QuestionStatus.ToString() : status,
                                            incomingDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            registrationDate = appealForExport.DateFrom.HasValue ? appealForExport.DateFrom.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                        };
                                        questListNoAnswer.Add(question);

                                        SSTUAppealNoAnswer sstuAppealNoAnswer = new SSTUAppealNoAnswer()
                                        {
                                            departmentId = "6bc82a49-6a97-43f4-9e42-a07d417b60b6",
                                            isDirect = isDirect,
                                            format = "Other",
                                            number = isDirect ? appealForExport.DocumentNumber : appealForExportSource.RevenueSourceNumber,
                                            createDate = appealForExportSource.SSTUDate.HasValue ? appealForExportSource.SSTUDate.Value.ToString("yyyy-MM-dd") : appealForExport.ObjectCreateDate.ToString("yyyy-MM-dd"),
                                            name = appealForExport.Correspondent,
                                            address = appealForExport.CorrespondentAddress,
                                            questions = questListNoAnswer.ToArray()
                                        };
                                        json = JsonConvert.SerializeObject(sstuAppealNoAnswer);
                                    }

                                }
                                var httpWebRequest = (HttpWebRequest)WebRequest.Create(Url);
                                httpWebRequest.ContentType = "application/json";
                                httpWebRequest.Method = "POST";

                                try
                                {

                                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                                    {
                                        streamWriter.Write(json);
                                        streamWriter.Flush();
                                        streamWriter.Close();
                                    }
                                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                    appealForExport.SSTUExportState = SSTUExportState.Exported;
                                    AppealCitsDomain.Update(appealForExport);
                                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                    {
                                        var responseResult = streamReader.ReadToEnd();
                                        sw.Write("result" + responseResult);
                                        appcitTask.SSTUExportState = SSTUExportState.Exported;
                                        var appCitExported = AppealCitsDomain.Get(appId);
                                        var exportedAppeals = SSTUExportedAppealDomain.GetAll()
                                            .Where(x => x.AppealCits.Id == appId)
                                            .Select(x => x.Id).FirstOrDefault();
                                        if (exportedAppeals != null)
                                        {
                                            SSTUExportedAppeal newApp = new SSTUExportedAppeal();
                                            newApp.AppealCits = appCitExported;
                                            newApp.ExportDate = DateTime.Now;
                                            newApp.ObjectVersion = 0;
                                            newApp.ObjectCreateDate = DateTime.Now;
                                            newApp.ObjectEditDate = DateTime.Now;
                                            SSTUExportedAppealDomain.Save(newApp);
                                        }
                                    }
                                    SSTUExportTaskDomain.Update(appcitTask);
                                }
                                catch (Exception e)
                                {
                                    sw.WriteLine(json);
                                    var err = e.ToString();
                                    sw.WriteLine(err);
                                    appealForExport.SSTUExportState = SSTUExportState.Error;
                                    AppealCitsDomain.Update(appealForExport);
                                    appcitTask.SSTUExportState = SSTUExportState.Error;
                                }
                            }


                        }
                    }
                    sw.Flush();
                    stream.Position = 0;
                    appcitTask.FileInfo = _fileManager.SaveFile(stream, "result.txt");
                    if (appcitTask.SSTUExportState != SSTUExportState.Error)
                    {
                        appcitTask.SSTUExportState = SSTUExportState.Exported;
                    }
                    SSTUExportTaskDomain.Update(appcitTask);
                    //    sw.Close();

                    return new BaseDataResult(true, "Успешно");
                }
                catch (Exception e)
                {
                    sw.WriteLine(e.ToString());
                    sw.Flush();
                    stream.Position = 0;
                    appcitTask.FileInfo = _fileManager.SaveFile(stream, "result.txt");
                    appcitTask.SSTUExportState = SSTUExportState.Error;
                    SSTUExportTaskDomain.Update(appcitTask);
                    return new BaseDataResult(false, $"Ошибка: {e.Message}");
                }
            }
            catch (HttpRequestException e)
            {
                return new BaseDataResult(false, $"Ошибка связи: {e.InnerException}");
            }
        }
    }
}
