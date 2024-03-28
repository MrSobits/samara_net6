using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Entities.PosAppeal;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
using Bars.GkhGji.Regions.Voronezh.Entities;
using Castle.Windsor;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Helpers;
using SMEV3Library.Namespaces;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Ionic.Zip;
using Ionic.Zlib;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using Bars.Gkh.Enums;
using Bars.Gkh.Services.DataContracts.Suggestion;
using Bars.B4.Modules.Security;
using SMEV3Library.Entities;
using Bars.GkhGji.Regions.Voronezh.Services.DataContracts.SyncAppealCit;

namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    public class ComplaintsService : IComplaintsService
    { 

        #region Properties              

        public IDomainService<SMEVComplaintsRequestFile> SMEVComplaintsRequestFileDomain { get; set; }
        public IDomainService<SMEVComplaintsLongText> SMEVComplaintsLongTextDomain { get; set; }
        public IDomainService<SMEVComplaintsRequest> SMEVComplaintsRequestDomain { get; set; }
        public IDomainService<SMEVComplaintsExecutant> SMEVComplaintsExecutantDomain { get; set; }
        public IDomainService<SMEVComplaints> SMEVComplaintsDomain { get; set; }
        public IRepository<Contragent> ContragentDomain { get; set; }
        public IRepository<LogOperation> LogOperationRepository { get; set; }
        public IWindsorContainer Container { get; set; }

        private string tokenPOS { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;

        #endregion

        #region Constructors

        public ComplaintsService(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

        public IDataResult ListComplaintFiles(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var complaintId = baseParams.Params.GetAs<long>("complaintId");
            var SMEVComplaintsRequestFileServ = this.Container.Resolve<IDomainService<SMEVComplaintsRequestFile>>();
            if (complaintId > 0)
            {
                var data = SMEVComplaintsRequestFileServ.GetAll()
               .Where(x => x.SMEVComplaintsRequest.ComplaintId.HasValue && x.SMEVComplaintsRequest.ComplaintId == complaintId)
               .Where(x => x.SMEVFileType == SMEVFileType.ResponseAttachment)
               .Select(x => new
               {
                   x.Id,
                   x.FileInfo,
                   x.FileInfo.Name,
                   x.SMEVFileType
               }).Filter(loadParams, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);

            }
            else
            {
                return null;
            }


        }

        /// <summary>
        /// Запрос информации о платежах
        /// </summary>
        public bool SendRequest(SMEVComplaintsRequest requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVComplaintsRequestFileDomain.GetAll().Where(x => x.SMEVComplaintsRequest == requestData && x.SMEVFileType != SMEVFileType.RequestAttachment).ToList().ForEach(x => SMEVComplaintsRequestFileDomain.Delete(x.Id));

                //PayRegDomain.GetAll().Where(x => x.PayReg == requestData).ToList().ForEach(x => PayRegDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = XElement.Parse(requestData.TextReq);

                ChangeState(requestData, RequestState.Formed);
                var attachment = SMEVComplaintsRequestFileDomain.GetAll().OrderByDescending(x => x.Id).FirstOrDefault(x => x.SMEVComplaintsRequest == requestData && x.SMEVFileType == SMEVFileType.RequestAttachment);
                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, GetFileAttachment(attachment), true).GetAwaiter().GetResult();
                //requestData.BillFor = "Запрос оплат";
                requestData.MessageId = requestResult.MessageId;
                SMEVComplaintsRequestDomain.Update(requestData);

                //
                indicator?.Report(null, 80, "Сохранение данных для отладки");
                SaveFile(requestData, requestResult.SendedData, "SendRequestRequest.dat");
                SaveFile(requestData, requestResult.ReceivedData, "SendRequestResponse.dat");

                indicator?.Report(null, 90, "Обработка результата");
                if (requestResult.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {requestResult.Error}");
                    SaveException(requestData, requestResult.Error.Exception);
                }
                else if (requestResult.FaultXML != null)
                {
                    SaveFile(requestData, requestResult.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в СМЭВ3, подробности в файле Fault.xml");
                }
                else if (requestResult.Status != "requestIsQueued")
                {
                    SetErrorState(requestData, "Ошибка при отправке: cервер вернул статус " + requestResult.Status);
                }
                else
                {
                    //изменяем статус
                    //TODO: Domain.Update не работает из колбека авайта. Дать пендаль казани
                    requestData.RequestState = RequestState.Queued;
                    requestData.Answer = "Поставлено в очередь";
                    SMEVComplaintsRequestDomain.Update(requestData);
                    return true;
                }
            }
            catch (HttpRequestException)
            {
                //ошибки связи прокидываем в контроллер
                throw;
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "SendPaymentRequest exception: " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData">Запрос</param>
        /// <param name="response">Ответ</param>
        /// <param name="indicator">Индикатор прогресса для таски</param>
        public bool TryProcessResponse(SMEVComplaintsRequest requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //сохранение данных
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponceRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponceResponse.dat");
                if (response.Attachments != null)
                {
                    response.Attachments.ForEach(x => SaveAttachmentFile(requestData, x.FileData, x.FileName));
                }
                if (response.FSAttachmentsList != null)
                {
                    response.FSAttachmentsList.ForEach(x => SaveFSAttachmentFile(requestData, x));
                }                

                indicator?.Report(null, 70, "Обработка результата");

                //ошибки наши
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveException(requestData, response.Error.Exception);
                    return false;
                }

                //ACK - ставим вдумчиво - чтобы можнор было считать повторно ,если это наш косяк
                //_SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

                //Ошибки присланные
                if (response.FaultXML != null)
                {
                    SaveFile(requestData, response.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения с оплатами из ГИС ГМП, подробности в файле Fault.xml");
                }
                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения с оплатами из ГИС ГМП, подробности в приаттаченом файле AsyncProcessingStatus.xml");
                }
                //сервер отклонил запрос?
                //else if (response.RequestRejected != null)
                //{
                //    requestData.RequestState = RequestState.ResponseReceived;
                //    requestData.Answer = response.RequestRejected.Element(SMEVNamespaces12.TypesNamespace + "RejectionReasonDescription")?.Value.Cut(500);
                //    SMEVComplaintsRequestDomain.Update(requestData);
                //    return true;
                //}
                else
                {
                    //перехватываем и получаем нужный контент для тестирования
                    // response.MessagePrimaryContent = GetMPC();
                    //ответ пустой?
                    if (response.MessagePrimaryContent == null)
                    {
                        SetErrorState(requestData, "Сервер прислал ответ, в котором нет ни результата, ни ошибки обработки");
                        return false;
                    }

                }
                //Парсим ответ если это запрос жалоб
                if (requestData.TypeComplainsRequest == TypeComplainsRequest.ComplaintsRequest)
                {
                    KndResponse newResponce = Deserialize<KndResponse>(XElement.Parse(response.MessagePrimaryContent.FirstNode.ToString()));
                    if (newResponce != null)
                    {
                        if (newResponce.Item is getComplaintsResultType)
                        {
                            getComplaintsResultType resType = newResponce.Item as getComplaintsResultType;
                            foreach (var complaint in resType.complaint.ToList())
                            {
                                if (SMEVComplaintsDomain.GetAll().FirstOrDefault(x => x.ComplaintId == complaint.id) != null)
                                {
                                    // пришло событие по жалобе
                                    var existsComplaint = SMEVComplaintsDomain.GetAll().FirstOrDefault(x => x.ComplaintId == complaint.id);
                                    existsComplaint.LifeEvent = GetStringFromcadeNameTypeArray(complaint.lifeEvent);
                                    if (complaint.pauseResolutionPetition != null && !string.IsNullOrEmpty(complaint.pauseResolutionPetition.content))
                                    {
                                        CreateLongText(complaint.pauseResolutionPetition, "pause", existsComplaint.Id);
                                    }
                                    if (complaint.renewTermPetition != null && !string.IsNullOrEmpty(complaint.renewTermPetition.content))
                                    {
                                        CreateLongText(complaint.renewTermPetition, "renew", existsComplaint.Id);
                                    }
                                    existsComplaint.ComplaintState = complaint.status;
                                    existsComplaint.Number = complaint.number;
                                    SMEVComplaintsDomain.Update(existsComplaint);
                                    requestData.RequestState = RequestState.ResponseReceived;
                                    requestData.Answer = "Успешно";
                                    SMEVComplaintsRequestDomain.Update(requestData);
                                    return true;
                                }
                                SMEVComplaints newComplaint = new SMEVComplaints
                                {
                                    ComplaintId = complaint.id,
                                    CommentInfo = $"Наименование жалобы: {complaint.name}. Пояснительный комментарий: {complaint.commentInfo}",
                                    ComplaintState = complaint.status,
                                    Number = complaint.number,
                                    ComplaintDate = complaint.epguData != null ? complaint.epguData.complaintDate.ToString("dd.MM.yyyy") : DateTime.Now.ToString("dd.MM.yyyy"),
                                    TypeAppealDecision = GetStringFromcadeNameTypeArray(complaint.typeAppealDecision),
                                    LifeEvent = GetStringFromcadeNameTypeArray(complaint.lifeEvent),
                                    AppealNumber = complaint.epguData != null ? complaint.epguData.appealNumber : "",
                                    Okato = complaint.epguData != null ? complaint.epguData.okato : "",
                                    OrderId = complaint.epguData != null ? complaint.epguData.orderId : "",
                                    RequesterRole = GetReqesterRole(complaint.applicantData != null ? complaint.applicantData.requesterRole : requesterRoleType.PERSON),
                                    EsiaOid = complaint.applicantData != null ? complaint.applicantData.esiaOid : ""
                                };
                                if (!string.IsNullOrEmpty(newComplaint.CommentInfo) && newComplaint.CommentInfo.Length > 9999)
                                {
                                    newComplaint.CommentInfo = newComplaint.CommentInfo.Substring(0, 9999);
                                }
                                if (complaint.applicantData != null)
                                {
                                    GetApplicantInfo(complaint.applicantData.Item, ref newComplaint);
                                }
                                SMEVComplaintsDomain.Save(newComplaint);
                                if (complaint.pauseResolutionPetition != null && !string.IsNullOrEmpty(complaint.pauseResolutionPetition.content))
                                {
                                    CreateLongText(complaint.pauseResolutionPetition, "pause", newComplaint.Id);
                                }
                                if (complaint.renewTermPetition != null && !string.IsNullOrEmpty(complaint.renewTermPetition.content))
                                {
                                    CreateLongText(complaint.renewTermPetition, "renew", newComplaint.Id);
                                }
                            }
                        }
                    }
                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVComplaintsRequestDomain.Update(requestData);

                }
                else
                {
                    requestData.RequestState = RequestState.ResponseReceived;
                    requestData.Answer = "Успешно";
                    SMEVComplaintsRequestDomain.Update(requestData);
                }

                return true;
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "TryProcessResponse exception: " + e.Message);
                return false;
            }
        }

        public bool CreateAppeals(Rootobject rootobject, string token, IProgressIndicator indicator = null)
        {
            var appcitDomain = this.Container.Resolve<IDomainService<AppealCits>>();
            var placeProblemDomain = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var zonalDomain = this.Container.Resolve<IDomainService<ZonalInspection>>();
            var RevenueSourceGjiDomain = this.Container.Resolve<IDomainService<RevenueSourceGji>>();
            var RevenueFormGjiDomain = this.Container.Resolve<IDomainService<RevenueFormGji>>();
            var AppealCitsSourceDomain = this.Container.Resolve<IDomainService<AppealCitsSource>>();
            var AppealCitsAttachmentDomain = this.Container.Resolve<IDomainService<AppealCitsAttachment>>();
            List<string> idsList = new List<string>();
            string t1 = "";
            foreach (var rec in rootobject.content.ToList())
            {
                t1 = $@"ПОС id{rec.id}";
                if (rec.applicant != null)
                {
                    t1 += $" {rec.applicant.surname} {rec.applicant.name} {rec.applicant.patronymic}";
                } 
                if (!CheckExists(rec))
                {
                    t1 += " новое";
                    var newControlDate = rec.answerAt.AddDays(-3);
                    if (rec.createdAt != null && rec.createdAt != DateTime.MinValue)
                    {
                        var prodCalendarContainer = this.Container.Resolve<IDomainService<ProdCalendar>>().GetAll()
                                               .Where(x => x.ProdDate >= rec.createdAt && x.ProdDate <= rec.createdAt.AddDays(38)).Select(x => x.ProdDate).ToList();

                        newControlDate = rec.answerAt.AddDays(-3);
                        if (prodCalendarContainer.Contains(newControlDate))
                        {
                            for (int i = 0; i <= prodCalendarContainer.Count; i++)
                            {
                                if (prodCalendarContainer.Contains(newControlDate))
                                {
                                    newControlDate = newControlDate.AddDays(-1);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }

                        if (newControlDate.DayOfWeek == DayOfWeek.Saturday)
                        {
                            newControlDate = newControlDate.AddDays(-1);
                        }
                        else if (newControlDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            newControlDate = newControlDate.AddDays(-2);
                        }
                        if (prodCalendarContainer.Contains(newControlDate))
                        {
                            for (int i = 0; i <= prodCalendarContainer.Count; i++)
                            {
                                if (prodCalendarContainer.Contains(newControlDate))
                                {
                                    newControlDate = newControlDate.AddDays(-1);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }

                    AppealCits newAppeal = new AppealCits
                    {
                        ArchiveNumber = rec.id.ToString(),
                        AppealStatus = GkhGji.Enums.AppealStatus.Control,
                        Description = rec.description,
                        DateFrom = rec.createdAt,
                        FastTrack = rec.fastTrack,
                        // ZonalInspection = zonalDomain.GetAll().FirstOrDefault(x => x.Name == "Государственная жилищная инспекция Воронежской области"),
                        ZonalInspection = zonalDomain.GetAll().FirstOrDefault(x => x.Name == "Отдел организации работы с обращениями граждан и документационного обеспечения"),
                        IsImported = true,
                        CheckTime = newControlDate,
                        Email = rec.applicant?.email,
                        Phone = rec.applicant?.phone,
                        CorrespondentAddress = GetCorrAddress(rec),
                        Correspondent = rec.applicant != null ? $"{rec.applicant.surname} {rec.applicant.name} {rec.applicant.patronymic}" : "",
                        TypeCorrespondent = GkhGji.Enums.TypeCorrespondent.CitizenHe,
                        Year = rec.createdAt.Year
                    };
                    try
                    {
                        #region Сохранение
                        using (var tr = Container.Resolve<IDataTransaction>())
                        {
                            try
                            {
                                appcitDomain.Save(newAppeal);
                                tr.Commit();
                            }
                            catch
                            {
                                tr.Rollback();                               
                            }
                        }
                        #endregion
                       
                        //файлы
                        if (rec.attachments.Length > 0)
                        {
                            try
                            {
                                foreach (string url in rec.attachments.ToList())
                                {
                                    B4.Modules.FileStorage.FileInfo appealFile = null;

                                    try
                                    {
                                        Task<FileResponce> task = Task.Run<FileResponce>(async () => await GetFile(token, url));
                                        FileResponce res = task.Result;
                                        if (res != null)
                                        {
                                            appealFile = _fileManager.SaveFile(res.filename, res.bytearray);
                                            AppealCitsAttachment newatach = new AppealCitsAttachment
                                            {
                                                AppealCits = newAppeal,
                                                FileInfo = appealFile,
                                                Description = "Приложение к обращению ПОС",
                                                Name = res.filename
                                            };
                                            #region Сохранение
                                            using (var tr = Container.Resolve<IDataTransaction>())
                                            {
                                                try
                                                {
                                                    AppealCitsAttachmentDomain.Save(newatach);
                                                    tr.Commit();
                                                }
                                                catch
                                                {
                                                    tr.Rollback();
                                                }
                                            }
                                            #endregion
                                           
                                        }
                                    }
                                    catch (Exception e)
                                    {

                                    }

                                    //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);

                                    //string filename = "";
                                    //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                                    //{
                                    //    var fn = response.Headers["Content-Disposition"].Split(new string[] { "=" }, StringSplitOptions.None)[1];
                                    //    filename = fn.Replace("\"", "");
                                    //    byte[] b = null;
                                    //    using (Stream stream = response.GetResponseStream())
                                    //    using (MemoryStream ms = new MemoryStream())
                                    //    {
                                    //        int count = 0;
                                    //        do
                                    //        {
                                    //            byte[] buf = new byte[1024];
                                    //            count = stream.Read(buf, 0, 1024);
                                    //            ms.Write(buf, 0, count);
                                    //        } while (stream.CanRead && count > 0);
                                    //        b = ms.ToArray();
                                    //    }
                                    //    //  var streamBytes = ReadFully(responseStream);

                                    //    appealFile = _fileManager.SaveFile(filename, b);
                                    //}

                                }
                                GetAttachmentArchive(newAppeal.Id);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                        //Место возникновения проблемы
                        var realityObj = GetRealObj(rec.locationAddress);
                        if (realityObj != null)
                        {
                            AppealCitsRealityObject pl = new AppealCitsRealityObject
                            {
                                AppealCits = newAppeal,
                                RealityObject = realityObj
                            };
                            placeProblemDomain.Save(pl);
                        }
                        //Источник
                        var source = RevenueSourceGjiDomain.GetAll().FirstOrDefault(x => x.Code == "12");
                        AppealCitsSource newsource = new AppealCitsSource
                        {
                            AppealCits = newAppeal,
                            RevenueDate = DateTime.Now,
                            RevenueSourceNumber = rec.id.ToString(),
                            RevenueForm = RevenueFormGjiDomain.GetAll().FirstOrDefault(x => x.Code == "09"),
                            RevenueSource = source,
                            SSTUDate = rec.createdAt
                        };
                        #region Сохранение
                        using (var tr = Container.Resolve<IDataTransaction>())
                        {
                            try
                            {
                                AppealCitsSourceDomain.Save(newsource);
                                tr.Commit();
                            }
                            catch
                            {
                                tr.Rollback();
                            }
                        }
                        #endregion
                        
                        //Тематики
                        //SetSubject(rec, newAppeal.Id);
                        idsList.Add(rec.id.ToString());
                        //отправить ответ о получении

                        t1 += " обработано";

                    }
                    catch (Exception e)
                    {
                        t1 += $" ошибка {e.Message}";
                    }

                }
                else
                {
                    t1 += " старое";
                    idsList.Add(rec.id.ToString());
                }
                try
                {
                    var userrepo = this.Container.Resolve<IRepository<User>>();
                    var user = userrepo.GetAll()
                        .FirstOrDefault();

                    var logOperation = new LogOperation
                    {
                        StartDate = DateTime.Now,
                        OperationType = LogOperationType.AppealPosReg,
                        User = user,
                        EndDate = DateTime.Now,
                        Comment = t1
                    };
                    this.LogOperationRepository.Save(logOperation);
                }
                catch
                { }
            }
            try
            {              
              
                foreach (string recId in idsList)
                {
                    Task<bool> acc = Task.Run<bool>(async () => await SendAccepted(recId, token));
                    var result = acc.Result;
                }
               
            }
            catch
            { }
            return true;
        }
        #endregion

        #region Private methods

        private void CreateLongText(petitionType pet, string pt, long id)
        {
            var existsLT = SMEVComplaintsLongTextDomain.GetAll().FirstOrDefault(x => x.SMEVComplaints.Id == id);
            if (existsLT != null)
            {
                if (pt == "pause")
                {
                    existsLT.PauseResolutionPetition = Encoding.UTF8.GetBytes(pet.content);
                }
                if (pt == "renew")
                {
                    existsLT.RenewTermPetition = Encoding.UTF8.GetBytes(pet.content);
                }
            }
            else
            {
                if (pt == "pause")
                {
                    SMEVComplaintsLongTextDomain.Save(new SMEVComplaintsLongText
                    {
                        SMEVComplaints = new SMEVComplaints { Id = id },
                        PauseResolutionPetition = Encoding.UTF8.GetBytes(pet.content)
                    });
                }
                if (pt == "renew")
                {
                    SMEVComplaintsLongTextDomain.Save(new SMEVComplaintsLongText
                    {
                        SMEVComplaints = new SMEVComplaints { Id = id },
                        RenewTermPetition = Encoding.UTF8.GetBytes(pet.content)
                    });
                }
            }
        }

        private async Task<FileResponce> GetFile(string token, string url)
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseRest = await client.GetAsync("");
            if (responseRest.IsSuccessStatusCode)
            {
              
                var headers = responseRest.Headers;
                //var dataObjects = await responseRest.Content.ReadAsStringAsync();

                var filename = responseRest.Content.Headers.ContentDisposition.FileName;
                filename = filename.Replace("\"", "");

                using (MemoryStream ms = new MemoryStream())
                {
                    await responseRest.Content.CopyToAsync(ms);
                    try
                    {
                        ms.Position = 0;

                       // var appealFile = _fileManager.SaveFile(ms, filename);
                        return new FileResponce
                        {
                            filename = filename,
                            bytearray = ms.ToArray()
                        };
                    }
                    catch (Exception e)
                    {

                    }
                }
                //  var streamBytes = ReadFully(responseStream);
              
                
            }
            return null;
        }

        private XNamespace Schema = "http://schemas.xmlsoap.org/soap/envelope/";

        private class FileResponce
        {
            public string filename { get; set; }
            public byte[] bytearray { get; set; }
        }

        private class ResponceJson
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
            public string scope { get; set; }
            public string patronymic { get; set; }
            public string system { get; set; }
            public string surname { get; set; }
            public string name { get; set; }
            public string rsId { get; set; }
            public string userId { get; set; }
            public string email { get; set; }
            public string superUser { get; set; }
            public string jti { get; set; }
        }

        private async Task<bool> SendAccepted(string id, string token)
        {
            string url = "https://pos.gosuslugi.ru/appeal-service/edms/" + id + "/mark-accepted";
            using (var myclient = new HttpClient())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                myclient.DefaultRequestHeaders.Add("User-Agent", "CBS Brightcove API Service");
                myclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
               var req = new HttpRequestMessage(HttpMethod.Post, url);

                HttpResponseMessage response = await myclient.SendAsync(req);
                HttpContent content = response.Content;
                string result = await content.ReadAsStringAsync();
            }
            return true;
        }      

        private void GetAttachmentArchive(long appealCitsId)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            //var fileInfoDomainService = this.Container.ResolveDomain<B4.Modules.FileStorage.FileInfo>();
            var AppealCitsAttachmentDomain = this.Container.ResolveDomain<AppealCitsAttachment>();
            var AppealCitsDomain = this.Container.Resolve<IRepository<AppealCits>>();

            try
            {
                var appealCits = AppealCitsDomain.Get(appealCitsId);

                var archive = new ZipFile(Encoding.UTF8)
                {
                    CompressionLevel = CompressionLevel.Level9,
                    AlternateEncoding = Encoding.GetEncoding("cp866"),
                    AlternateEncodingUsage = ZipOption.AsNecessary
                };

                var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
                bool skip = false;
                var appealCitsAttachmentFiles = AppealCitsAttachmentDomain.GetAll().Where(x => x.AppealCits.Id == appealCitsId).ToList();

                foreach (var file in appealCitsAttachmentFiles)
                {
                    System.IO.File.WriteAllBytes(
                        Path.Combine(tempDir.FullName, $"{file.FileInfo.Name}.{file.FileInfo.Extention}"),
                        fileManager.GetFile(file.FileInfo).ReadAllBytes());
                }

                archive.AddDirectory(tempDir.FullName);

                using (var ms = new MemoryStream())
                {
                    archive.Save(ms);

                    var file = fileManager.SaveFile(ms, $"Обращение{(appealCits.DateFrom.HasValue ? $" от {appealCits.DateFrom.Value.ToShortDateString()}" : "")}.zip");
                    appealCits.File = file;
                    AppealCitsDomain.Update(appealCits);
                }
            }
            finally
            {
                this.Container.Release(fileManager);
                //this.Container.Release(fileInfoDomainService);
                this.Container.Release(AppealCitsAttachmentDomain);
                this.Container.Release(AppealCitsDomain);
            }
        }

        private void SetSubject(Content rec, long appId)
        {
            var StatSubjectGjiDomain = this.Container.Resolve<IDomainService<StatSubjectGji>>();
            var StatSubjectSubsubjectGjiDomain = this.Container.Resolve<IDomainService<StatSubjectSubsubjectGji>>();
            var StatSubsubjectGjiGjiDomain = this.Container.Resolve<IDomainService<StatSubsubjectGji>>();
            var AppealCitsStatSubjectDomain = this.Container.Resolve<IDomainService<AppealCitsStatSubject>>();
            try
            {
                if (rec.subjectId > 0 && !string.IsNullOrEmpty(rec.subjectName))
                {
                    AppealCitsStatSubject newsubj = new AppealCitsStatSubject
                    {
                        AppealCits = new AppealCits { Id = appId },
                    };
                    var subDict = StatSubjectGjiDomain.GetAll()
                 .Where(x => x.Code == rec.subjectId.ToString() || x.Name == rec.subjectName)
                 .FirstOrDefault();
                    if (subDict != null)
                    {
                        newsubj.Subject = subDict;
                    }
                    else
                    {
                        StatSubjectGji newSubjInDict = new StatSubjectGji
                        {
                            Code = rec.subjectId.ToString(),
                            Name = rec.subjectName
                        };
                        StatSubjectGjiDomain.Save(newSubjInDict);
                        newsubj.Subject = newSubjInDict;
                    }
                    if (rec.subsubjectId > 0 && !string.IsNullOrEmpty(rec.subsubjectName))
                    {
                        var sub = StatSubjectSubsubjectGjiDomain.GetAll()
                            .Where(x => x.Subject == newsubj.Subject)
                            .Where(x => x.Subsubject.Code == rec.subsubjectId.ToString() || x.Subsubject.Name == rec.subsubjectName).FirstOrDefault();
                        if (sub != null)
                        {
                            newsubj.Subsubject = sub.Subsubject;
                        }
                        else
                        {
                            var subsub = StatSubsubjectGjiGjiDomain.GetAll()
                                .Where(x => x.Code == rec.subsubjectId.ToString() || x.Name == rec.subsubjectName).FirstOrDefault();
                            if (subsub != null)
                            {
                                StatSubjectSubsubjectGji newssubdict = new StatSubjectSubsubjectGji
                                {
                                    Subject = newsubj.Subject,
                                    Subsubject = subsub
                                };
                                StatSubjectSubsubjectGjiDomain.Save(newssubdict);
                                newsubj.Subsubject = subsub;
                            }
                            else
                            {
                                StatSubsubjectGji newsubsub = new StatSubsubjectGji
                                {
                                    Code = rec.subsubjectId.ToString(),
                                    Name = rec.subsubjectName
                                };
                                StatSubsubjectGjiGjiDomain.Save(newsubsub);
                                StatSubjectSubsubjectGji newssubdict = new StatSubjectSubsubjectGji
                                {
                                    Subject = newsubj.Subject,
                                    Subsubject = newsubsub
                                };
                                StatSubjectSubsubjectGjiDomain.Save(newssubdict);
                                newsubj.Subsubject = newsubsub;
                            }
                        }
                    }
                    AppealCitsStatSubjectDomain.Save(newsubj);


                }
            }
            finally
            {
                Container.Release(StatSubjectGjiDomain);
                Container.Release(StatSubjectSubsubjectGjiDomain);
                Container.Release(StatSubsubjectGjiGjiDomain);
                Container.Release(AppealCitsStatSubjectDomain);
            }
        }

        private RealityObject GetRealObj(Locationaddress addr)
        {
            if (addr == null)
            {
                return null;
            }
            var roDomain = this.Container.Resolve<IDomainService<RealityObject>>();
            try
            {
                if (!string.IsNullOrEmpty(addr.houseFiasId))
                {
                    var ro = roDomain.GetAll().FirstOrDefault(x => x.FiasAddress.HouseGuid.HasValue && x.FiasAddress.HouseGuid.Value.ToString() == addr.houseFiasId);
                    if (ro != null)
                    {
                        return ro;
                    }
                }
                if (!string.IsNullOrEmpty(addr.streetFiasId))
                {
                    RealityObject ro = null;
                    var roList = roDomain.GetAll().Where(x => x.FiasAddress.StreetGuidId == addr.streetFiasId && x.FiasAddress.House == addr.house).ToList();
                    roList.ForEach(x =>
                    {
                        if (!string.IsNullOrEmpty(addr.block))
                        {
                            if (!string.IsNullOrEmpty(x.FiasAddress.Housing))
                            {
                                if (x.FiasAddress.Housing == addr.block)
                                {
                                    ro = x;
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(x.FiasAddress.Housing) && string.IsNullOrEmpty(x.FiasAddress.Building))
                            {
                                ro = x;
                            }
                        }
                    });
                    return ro;
                }
            }
            finally
            {
                Container.Release(roDomain);
            }
            return null;
        }

        private string GetCorrAddress(Content rec)
        {
            if (!string.IsNullOrEmpty(rec.applicant.postAddress))
            {
                return rec.applicant.postAddress;
            }
            string aaplAddress = string.Empty;
            if (rec.locationAddress == null)
            {
                return "";
            }
            if (!string.IsNullOrEmpty(rec.locationAddress.postalCode))
            {
                aaplAddress += rec.locationAddress.postalCode + ", ";
            }
            if (!string.IsNullOrEmpty(rec.locationAddress.regionWithType))
            {
                aaplAddress += rec.locationAddress.regionWithType + ", ";
            }
            if (!string.IsNullOrEmpty(rec.locationAddress.areaWithType))
            {
                aaplAddress += rec.locationAddress.areaWithType + ", ";
            }
            if (!string.IsNullOrEmpty(rec.locationAddress.cityWithType))
            {
                aaplAddress += rec.locationAddress.cityWithType + ", ";
            }
            if (!string.IsNullOrEmpty(rec.locationAddress.streetWithType))
            {
                aaplAddress += rec.locationAddress.streetWithType + ", ";
            }
            if (!string.IsNullOrEmpty(rec.locationAddress.house))
            {
                aaplAddress += "д. " + rec.locationAddress.house;
            }
            if (!string.IsNullOrEmpty(rec.locationAddress.blockTypeFull))
            {
                aaplAddress += ", " + rec.locationAddress.blockTypeFull;
            }
            return aaplAddress;
        }

        private bool CheckExists(Content rec)
        {
            var appealDomain = this.Container.Resolve<IDomainService<AppealCits>>();
            var existAppeal = appealDomain.GetAll()
                .Where(x => x.ArchiveNumber == rec.id.ToString()).FirstOrDefault();
            if (existAppeal != null)
            {
                return true;
            }
            return false;

        }

        private void GetApplicantInfo(object item, ref SMEVComplaints newComplaint)
        {
            if (item != null)
            {
                if (item is applicantBusinessmanType)
                {
                    applicantBusinessmanType buisnessman = item as applicantBusinessmanType;
                    newComplaint.BirthAddress = buisnessman.applicantPerson.birthAddress;
                    newComplaint.RequesterFIO = $"{buisnessman.applicantPerson.lastName} {buisnessman.applicantPerson.firstName} {buisnessman.applicantPerson.middleName}";
                    newComplaint.Gender = buisnessman.applicantPerson.gender == genderType.Male ? Gkh.Enums.Gender.Male : Gkh.Enums.Gender.Female;
                    if (buisnessman.applicantPerson.identityDocument.Item is passportRfType)
                    {
                        var passportRF = buisnessman.applicantPerson.identityDocument.Item as passportRfType;
                        newComplaint.IdentityDocumentType = IdentityDocumentType.passportRf;
                        newComplaint.DocSeries = passportRF.series;
                        newComplaint.DocNumber = passportRF.number;
                    }
                    else if (buisnessman.applicantPerson.identityDocument.Item is internationalPassportRfType)
                    {
                        var passportRF = buisnessman.applicantPerson.identityDocument.Item as internationalPassportRfType;
                        newComplaint.IdentityDocumentType = IdentityDocumentType.internationalPassportRf;
                        newComplaint.DocSeries = passportRF.series;
                        newComplaint.DocNumber = passportRF.number;
                    }
                    else if (buisnessman.applicantPerson.identityDocument.Item is notRestrictedDocumentType)
                    {
                        var passportRF = buisnessman.applicantPerson.identityDocument.Item as notRestrictedDocumentType;
                        newComplaint.IdentityDocumentType = IdentityDocumentType.internationalPassportRf;
                        newComplaint.DocSeries = passportRF.series;
                        newComplaint.DocNumber = passportRF.number;
                    }
                    if (buisnessman.applicantPerson.registrationAddress != null)
                    {
                        newComplaint.RegAddess = $"{buisnessman.applicantPerson.registrationAddress.index} {buisnessman.applicantPerson.registrationAddress.fullAddress}";
                    }
                    newComplaint.INNFiz = buisnessman.applicantPerson.inn;
                    newComplaint.Nationality = buisnessman.applicantPerson.nationality;
                    newComplaint.BirthDate = buisnessman.applicantPerson.birthDate;
                    newComplaint.Email = buisnessman.applicantPerson.email;
                    newComplaint.MobilePhone = buisnessman.applicantPerson.mobilePhone;
                    newComplaint.SNILS = buisnessman.applicantPerson.snils;
                }
                else if (item is applicantLegalType)
                {                  
                    applicantLegalType buisnessman = item as applicantLegalType;
                    Contragent contragent = ContragentDomain.GetAll()
                      .Where(x => x.Ogrn == buisnessman.ogrn && x.Inn == buisnessman.inn).FirstOrDefault();
                    if (contragent == null)
                    {
                        contragent = new Contragent
                        {
                            Ogrn = buisnessman.ogrn,
                            Inn = buisnessman.inn,
                            Kpp = buisnessman.kpp,
                            Name = buisnessman.legalFullName,
                            ShortName = buisnessman.legalShortName,
                            Email = buisnessman.email,
                            ContragentState = Gkh.Enums.ContragentState.Active,
                            Description = "Создан из запроса по досудебному обжалованию",
                            ActivityGroundsTermination = Gkh.Enums.GroundsTermination.NotSet,
                            JuridicalAddress = buisnessman.legalAddress.fullAddress,
                            Phone = buisnessman.mobilePhone,
                            ObjectCreateDate = DateTime.Now,
                            ObjectEditDate = DateTime.Now,
                            ObjectVersion = 1
                        };
                        ContragentDomain.Save(contragent);
                    }
                    newComplaint.RequesterFIO = $"{buisnessman.lastName} {buisnessman.firstName} {buisnessman.middleName}";
                    newComplaint.Gender = Gkh.Enums.Gender.NotSet;
                    newComplaint.WorkingPosition = buisnessman.workingPosition;
                    newComplaint.Email = buisnessman.email;
                    newComplaint.MobilePhone = buisnessman.mobilePhone;
                }
                else if (item is applicantPersonType)
                {
                    applicantPersonType buisnessman = item as applicantPersonType;
                    newComplaint.BirthAddress = buisnessman.birthAddress;
                    newComplaint.RequesterFIO = $"{buisnessman.lastName} {buisnessman.firstName} {buisnessman.middleName}";
                    newComplaint.Gender = buisnessman.gender == genderType.Male ? Gkh.Enums.Gender.Male : Gkh.Enums.Gender.Female;
                    if (buisnessman.identityDocument.Item is passportRfType)
                    {
                        var passportRF = buisnessman.identityDocument.Item as passportRfType;
                        newComplaint.IdentityDocumentType = IdentityDocumentType.passportRf;
                        newComplaint.DocSeries = passportRF.series;
                        newComplaint.DocNumber = passportRF.number;
                    }
                    else if (buisnessman.identityDocument.Item is internationalPassportRfType)
                    {
                        var passportRF = buisnessman.identityDocument.Item as internationalPassportRfType;
                        newComplaint.IdentityDocumentType = IdentityDocumentType.internationalPassportRf;
                        newComplaint.DocSeries = passportRF.series;
                        newComplaint.DocNumber = passportRF.number;
                    }
                    else if (buisnessman.identityDocument.Item is notRestrictedDocumentType)
                    {
                        var passportRF = buisnessman.identityDocument.Item as notRestrictedDocumentType;
                        newComplaint.IdentityDocumentType = IdentityDocumentType.internationalPassportRf;
                        newComplaint.DocSeries = passportRF.series;
                        newComplaint.DocNumber = passportRF.number;
                    }
                    if (buisnessman.registrationAddress != null)
                    {
                        newComplaint.RegAddess = $"{buisnessman.registrationAddress.index} {buisnessman.registrationAddress.fullAddress}";
                    }
                    newComplaint.INNFiz = buisnessman.inn;
                    newComplaint.Nationality = buisnessman.nationality;
                    newComplaint.BirthDate = buisnessman.birthDate;
                    newComplaint.Email = buisnessman.email;
                    newComplaint.MobilePhone = buisnessman.mobilePhone;
                    newComplaint.SNILS = buisnessman.snils;
                }
            }
        }

        private RequesterRole GetReqesterRole(requesterRoleType role)
        {
            switch (role)
            {
                case requesterRoleType.PERSON: return RequesterRole.PERSON;
                case requesterRoleType.BUSINESSMAN: return RequesterRole.BUSINESSMAN;
                case requesterRoleType.EMPLOYEE: return RequesterRole.EMPLOYEE;
                default:
                    return RequesterRole.PERSON;

            }
        }

        private string GetStringFromcadeNameTypeArray(codeNameType[] typeArray)
        {
            string str = string.Empty;
            foreach (codeNameType type in typeArray.ToList())
            {
                str += $"{type.code} {type.Value}; ";
            }
            return str;
        }

        private XElement GetMPC()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\or\\newfilesRequest.xml");
            XElement mpc = XElement.Parse(doc.InnerXml);
            var soapXML = GetBody(mpc);
            var GetResponseResponseElement = soapXML.Element(SMEVNamespaces12.TypesNamespace + "GetResponseResponse");
            var ResponseElement = GetResponseResponseElement.Element(SMEVNamespaces12.TypesNamespace + "ResponseMessage")?.Element(SMEVNamespaces12.TypesNamespace + "Response");
            XElement SenderProvidedResponseDataElement = ResponseElement.Element(SMEVNamespaces12.TypesNamespace + "SenderProvidedResponseData");
            return SenderProvidedResponseDataElement.Element(SMEVNamespaces12.BasicNamespace + "MessagePrimaryContent");
        }

        private XElement GetBody(XElement SoapXml)
        {
            var body = SoapXml.Element(Schema + "Body");
            return body;
        }

        private T Deserialize<T>(XElement element)
        where T : class
        {
            XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(element.ToString()))
                return (T)ser.Deserialize(sr);
        }


        private DateTime? NullableDateParse(string value)
        {
            if (value == null)
                return null;

            DateTime result;

            return (DateTime.TryParse(value, out result) ? result : (DateTime?)null);
        }

        private byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }



        private void SaveFile(SMEVComplaintsRequest request, byte[] data, string fileName)
        {
            if (data == null)
                return;

            //сохраняем отправленный пакет
            SMEVComplaintsRequestFileDomain.Save(new SMEVComplaintsRequestFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVComplaintsRequest = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVComplaintsRequest request, XElement data, string fileName)
        {
            if (data == null)
                return;

            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVComplaintsRequestFile faultRec = new SMEVComplaintsRequestFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVComplaintsRequest = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            };

            SMEVComplaintsRequestFileDomain.Save(faultRec);
        }

        private void SaveException(SMEVComplaintsRequest request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVComplaintsRequestFileDomain.Save(new SMEVComplaintsRequestFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVComplaintsRequest = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }

        private void ChangeState(SMEVComplaintsRequest requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVComplaintsRequestDomain.Update(requestData);
        }

        private void SetErrorState(SMEVComplaintsRequest requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVComplaintsRequestDomain.Update(requestData);
        }
        private List<FileAttachment> GetFileAttachment(SMEVComplaintsRequestFile att)
        {
            if (att != null && att.FileInfo != null)
            {
                return new List<FileAttachment> { new FileAttachment { FileData = _fileManager.GetFile(att.FileInfo).ReadAllBytes(), FileName = $"_{att.FileInfo.Id}.{att.FileInfo.Extention}" } };
            }
            return null;
        }
        private void SaveFSAttachmentFile(SMEVComplaintsRequest request, FsAttachmentProxy ftpData)
        {
            //сохраняем отправленный пакет
            try
            {
                FtpWebRequest reqFTP;
                if (!string.IsNullOrEmpty(ftpData.FileName) && ftpData.FileName.StartsWith("/"))
                {
                    ftpData.FileName = ftpData.FileName.Substring(1);
                }
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new
                //Uri($"ftp://{ftpData.UserName}:{ftpData.Password}@172.20.3.12/{ftpData.FileName}"));
                //Uri($"ftp://{ftpData.UserName}:{ftpData.Password}@10.0.43.61/{ftpData.FileName}"));
                Uri($"ftp://{ftpData.UserName}:{ftpData.Password}@smev3-n0.test.gosuslugi.ru/{ftpData.FileName}"));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                //   reqFTP.Credentials = new NetworkCredential(ftpData.UserName, ftpData.Password);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                Stream filestream = new MemoryStream();
                while (readCount > 0)
                {
                    filestream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }
                var fileInfo = _fileManager.SaveFile(filestream, ftpData.FileName);

                SMEVComplaintsRequestFileDomain.Save(new SMEVComplaintsRequestFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    SMEVComplaintsRequest = request,
                    SMEVFileType = SMEVFileType.ResponseAttachmentFTP,
                    FileInfo = fileInfo
                });
            }
            catch (Exception e)
            {
                try
                {
                    SMEVComplaintsRequestFileDomain.Save(new SMEVComplaintsRequestFile
                    {
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        ObjectVersion = 1,
                        SMEVComplaintsRequest = request,
                        SMEVFileType = SMEVFileType.Error,
                        FileInfo = _fileManager.SaveFile("Exception.txt", ($"{e.GetType().ToString()}\n{e.Message}\n{e.StackTrace}").GetBytes())
                    });
                }
                catch
                {
                }
            }
        }
        private void SaveAttachmentFile(SMEVComplaintsRequest request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            try
            {
                SMEVComplaintsRequestFileDomain.Save(new SMEVComplaintsRequestFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    SMEVComplaintsRequest = request,
                    SMEVFileType = SMEVFileType.ResponseAttachment,
                    FileInfo = _fileManager.SaveFile(fileName, data)
                });
            }
            catch
            {
                try
                {
                    SMEVComplaintsRequestFileDomain.Save(new SMEVComplaintsRequestFile
                    {
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        ObjectVersion = 1,
                        SMEVComplaintsRequest = request,
                        SMEVFileType = SMEVFileType.ResponseAttachment,
                        FileInfo = _fileManager.SaveFile("Answer.zip", data)
                    });
                }
                catch
                {
                    throw new Exception($"некорректное имя файла ответа {fileName}");
                }
            }
        }
        #endregion

        #region Nested classes
        internal class Identifiers
        {
            internal string SenderIdentifier;
            internal string SenderRole;
            internal string OriginatorID;
        }

        #endregion

    }
}
