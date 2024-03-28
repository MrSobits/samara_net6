using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Castle.Windsor;
// TOD: Замена
//using CryptoPro.Sharpei;
using GisGkhLibrary.InspectionServiceAsync;
using GisGkhLibrary.Services;
using GisGkhLibrary.Utils;
using Sobits.GisGkh.Entities;
using Sobits.GisGkh.Enums;
using Sobits.GisGkh.Tasks.ProcessGisGkhAnswers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Sobits.GisGkh.DomainService
{
    public class ExportExaminationsService : IExportExaminationsService
    {
        #region Constants



        #endregion

        #region Properties   
        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }
        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<Prescription> PrescriptionDomain { get; set; }
        public IDomainService<DisposalTypeSurvey> DisposalTypeSurveyDomain { get; set; }
        public IDomainService<TypeSurveyGoalInspGji> TypeSurveyGoalInspGjiDomain { get; set; }

        public IRepository<Disposal> DisposalRepo { get; set; }
        public IRepository<Contragent> ContragentRepo { get; set; }
        public IRepository<DocumentGji> DocumentGjiRepo { get; set; }

        public IWindsorContainer Container { get; set; }

        private IFileService _fileService;

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Constructors

        public ExportExaminationsService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, string[] reqParams)
        {
            try
            {
                DateTime from = DateTime.Parse(reqParams[0]);
                DateTime to = DateTime.Parse(reqParams[1]);
                var request = InspectionServiceAsync.exportExaminationsReq(from, to);
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
                GisGkhRequestsDomain.Update(req);
                //return true;
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        public void CheckAnswer(GisGkhRequests req, string orgPPAGUID)
        {
            try
            {
                var response = InspectionServiceAsync.GetState(req.MessageGUID, orgPPAGUID);
                if (response.RequestState == 3)
                {
                    // Удаляем старые файлы ответов, если они, вдруг, имеются
                    GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .ToList().ForEach(x => GisGkhRequestsFileDomain.Delete(x.Id));
                    SaveFile(req, GisGkhFileType.answer, SerializeRequest(response), "response.xml");
                    req.Answer = "Ответ получен";
                    req.RequestState = GisGkhRequestState.ResponseReceived;
                    GisGkhRequestsDomain.Update(req);
                    BaseParams baseParams = new BaseParams();
                    baseParams.Params.Add("reqId", req.Id.ToString());
                    try
                    {
                        var taskInfo = _taskManager.CreateTasks(new ProcessGisGkhAnswersTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                        if (taskInfo == null)
                        {
                            req.Answer = "Сбой создания задачи обработки ответа";
                            GisGkhRequestsDomain.Update(req);
                            //req.RequestState = GisGkhRequestState.У; ("Сбой создания задачи");
                        }
                        else
                        {
                            req.Answer = $"Задача на обработку ответа exportRegionalProgram поставлена в очередь с id {taskInfo.TaskId}";
                            GisGkhRequestsDomain.Update(req);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Ошибка: " + e.Message);
                    }
                }
                else if ((response.RequestState == 1) || (response.RequestState == 2))
                {
                    req.Answer = "Запрос ещё в очереди";
                }
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                req.RequestState = GisGkhRequestState.Error;
                req.Answer = e.Message;
                GisGkhRequestsDomain.Update(req);
            }
        }

        public void ProcessAnswer(GisGkhRequests req)
        {
            if (req.RequestState == GisGkhRequestState.ResponseReceived)
            {
                try
                {
                    var fileInfo = GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .FirstOrDefault();
                    if (fileInfo != null)
                    {
                        var fileStream = _fileManager.GetFile(fileInfo.FileInfo);
                        var data = fileStream.ReadAllBytes();
                        //return Encoding.UTF8.GetString(data);
                        var response = DeserializeData<getStateResult>(Encoding.UTF8.GetString(data));
                        foreach (var responseItem in response.Items)
                        {
                            if (responseItem is ErrorMessageType)
                            {
                                req.RequestState = GisGkhRequestState.Error;
                                var error = (ErrorMessageType)responseItem;
                                req.Answer = $"{error.ErrorCode}:{error.Description}";
                            }
                            else if (responseItem is exportExaminationResultType)
                            {
                                var examination = (exportExaminationResultType)responseItem;
                                Disposal disposal = DisposalRepo.GetAll()
                                    .Where(x => x.GisGkhGuid == examination.ExaminationGuid)
                                    .FirstOrDefault();
                                if (disposal == null)
                                {
                                    try
                                    {
                                        var exNum = examination.Examination.ExaminationOverview.OrderNumber.Replace(".", "").Replace(" ", "").Replace("-", "").Replace("_", "").Replace("№", "").ToLower();
                                        var disposalTemp = DisposalRepo.GetAll()
                                            .Select(x => new
                                            {
                                                x.Id,
                                                DocumentNumber = x.DocumentNumber.Replace(".", "").Replace(" ", "").Replace("-", "").Replace("_", "").ToLower()
                                            }).AsEnumerable().FirstOrDefault(x => x.DocumentNumber == exNum);

                                        if (disposalTemp != null)
                                        {
                                            disposal = DisposalRepo.Get(disposalTemp.Id);
                                        }
                                        if (disposal != null)
                                        {
                                            disposal.GisGkhGuid = examination.ExaminationGuid;
                                            DisposalRepo.Update(disposal);
                                        }
                                    }
                                    catch (Exception e)
                                    {

                                    }
                                }
                                if (disposal != null)
                                {
                                    // проставляем гуиды контрагентов, если их нету
                                    var contragent = disposal.Inspection.Contragent;
                                    if (contragent == null)
                                    {
                                        // значит физлицо
                                    }
                                    else if (string.IsNullOrEmpty(contragent.GisGkhGUID))
                                    {
                                        if (examination.Examination.ExaminationOverview.ExaminationTypeType.Item is exportExaminationTypeExaminationOverviewExaminationTypeTypeScheduled)
                                        {
                                            var exTypeType = (exportExaminationTypeExaminationOverviewExaminationTypeTypeScheduled)examination.Examination.ExaminationOverview.ExaminationTypeType.Item;
                                            if (exTypeType.Subject.Item is ScheduledExaminationSubjectInfoTypeIndividual)
                                            {
                                                contragent.GisGkhGUID = ((ScheduledExaminationSubjectInfoTypeIndividual)exTypeType.Subject.Item).orgRootEntityGUID;
                                            }
                                            else if (exTypeType.Subject.Item is ScheduledExaminationSubjectInfoTypeOrganization)
                                            {
                                                contragent.GisGkhGUID = ((ScheduledExaminationSubjectInfoTypeOrganization)exTypeType.Subject.Item).orgRootEntityGUID;
                                            }
                                        }
                                        else if (examination.Examination.ExaminationOverview.ExaminationTypeType.Item is exportExaminationTypeExaminationOverviewExaminationTypeTypeUnscheduled)
                                        {
                                            var exTypeType = (exportExaminationTypeExaminationOverviewExaminationTypeTypeUnscheduled)examination.Examination.ExaminationOverview.ExaminationTypeType.Item;
                                            if (exTypeType.Subject.Item is ExportUnscheduledExaminationSubjectInfoTypeIndividual)
                                            {
                                                contragent.GisGkhGUID = ((ExportUnscheduledExaminationSubjectInfoTypeIndividual)exTypeType.Subject.Item).orgRootEntityGUID;
                                            }
                                            else if (exTypeType.Subject.Item is ExportUnscheduledExaminationSubjectInfoTypeOrganization)
                                            {
                                                contragent.GisGkhGUID = ((ExportUnscheduledExaminationSubjectInfoTypeOrganization)exTypeType.Subject.Item).orgRootEntityGUID;
                                            }
                                        }
                                        ContragentRepo.Update(contragent);
                                    }
                                    // акт
                                    DocumentGji docdAct = DocumentGjiChildrenDomain.GetAll()
                                                    .Where(x => x.Parent == disposal)
                                                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck// ||
                                                                                                                      //x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval ||
                                                                                                                      //x.Children.TypeDocumentGji == TypeDocumentGji.ActSurvey
                                                        )
                                                        .Select(x => x.Children).FirstOrDefault();
                                    if (docdAct != null)
                                    {
                                        // акт проверки предписания
                                        DocumentGji docdActRemoval = DocumentGjiChildrenDomain.GetAll()
                                                    .Where(x => x.Parent == docdAct)
                                                    .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                                                            .Select(x => x.Children).FirstOrDefault();
                                        // предписания
                                        List<DocumentGji> docPrescr = new List<DocumentGji>();
                                        if (examination.ExportPrecept != null)
                                        {
                                            docPrescr = DocumentGjiChildrenDomain.GetAll()
                                                .Where(x => x.Parent == docdAct || x.Parent == docdActRemoval)
                                                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                                                .Where(x => x.Children.DocumentNumber != null) // номер документа непустой
                                                .Select(x => x.Children)
                                                .ToList();
                                            if (docPrescr.Count() > 0)
                                            {
                                                var precepts = examination.ExportPrecept.ToList();
                                                foreach (var precept in precepts)
                                                {
                                                    try
                                                    {
                                                        DocumentGji docPrescription = docPrescr.Where(x => x.DocumentNumber.Replace(".", "").Replace(" ", "").Replace("-", "").Replace("_", "").ToLower() ==
                                                            precept.Precept.Number.Replace(".", "").Replace(" ", "").Replace("-", "").Replace("_", "").Replace("№", "").ToLower() ||
                                                            x.DocumentNum.ToString() == precept.Precept.Number).FirstOrDefault();
                                                        if (docPrescription != null)
                                                        {
                                                            docPrescription.GisGkhGuid = precept.PreceptGuid;
                                                            DocumentGjiRepo.Update(docPrescription);
                                                        }
                                                    }
                                                    catch (Exception e)
                                                    {

                                                    }
                                                }
                                            }
                                        }

                                        // протоколы
                                        List<DocumentGji> docProtoc = DocumentGjiChildrenDomain.GetAll()
                                                .Where(x => x.Parent == docdAct || x.Parent == docdActRemoval || docPrescr.Contains(x.Parent))
                                                .Where(x => x.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                                                .Where(x => x.Children.DocumentNumber != null) // номер документа непустой
                                                .Select(x => x.Children)
                                                .ToList();
                                        if (docProtoc.Count() > 0 && examination.ExportOffence != null)
                                        {
                                            var offences = examination.ExportOffence.ToList();
                                            foreach (var offence in offences)
                                            {
                                                try
                                                {
                                                    DocumentGji docProtocol = docProtoc.Where(x => x.DocumentNumber.Replace(".", "").Replace(" ", "").Replace("-", "").Replace("_", "").ToLower() ==
                                                        offence.Offence.Number.Replace(".", "").Replace(" ", "").Replace("-", "").Replace("_", "").Replace("№", "").ToLower() ||
                                                        x.DocumentNum.ToString() == offence.Offence.Number).FirstOrDefault();
                                                    if (docProtocol != null)
                                                    {
                                                        docProtocol.GisGkhGuid = offence.OffenceGuid;
                                                        DocumentGjiRepo.Update(docProtocol);
                                                    }
                                                }
                                                catch (Exception e)
                                                {

                                                }
                                            }
                                        }

                                        // результаты проверки
                                        if (examination.Examination.ResultsInfo != null)
                                        {
                                            var result = examination.Examination.ResultsInfo;
                                            var resultDoc = result.FinishedInfo.Result;
                                            switch (resultDoc.DocumentType.Code)
                                            {
                                                // акт
                                                case "1":
                                                    try
                                                    {
                                                        // если акт не сопоставлен (приписываем актам ГУИД самой проверки из-за отсутствия в ГИС ГУИДа актов)
                                                        if (docdAct.GisGkhGuid == null || docdAct.GisGkhGuid == "")
                                                        {
                                                            docdAct.GisGkhGuid = examination.ExaminationGuid;
                                                            DocumentGjiRepo.Update(docdAct);
                                                            //switch (docChildAct.TypeDocumentGji)
                                                            //{
                                                            //    // акт проверки
                                                            //    case TypeDocumentGji.ActCheck:
                                                            //        ActCheck actCheck = ActCheckRepo.Get(docChildAct.Id);
                                                            //        actCheck.GisGkhGuid = examination.ExaminationGuid;
                                                            //        ActCheckRepo.Update(actCheck);
                                                            //        break;
                                                            //    // акт устранения
                                                            //    case TypeDocumentGji.ActRemoval:
                                                            //        ActRemoval actRemoval = ActRemovalRepo.Get(docChildAct.Id);
                                                            //        actRemoval.GisGkhGuid = examination.ExaminationGuid;
                                                            //        ActRemovalRepo.Update(actRemoval);
                                                            //        break;
                                                            //    // акт обследования
                                                            //    case TypeDocumentGji.ActSurvey:
                                                            //        ActSurvey actSurvey = ActSurveyRepo.Get(docChildAct.Id);
                                                            //        actSurvey.GisGkhGuid = examination.ExaminationGuid;
                                                            //        ActSurveyRepo.Update(actSurvey);
                                                            //        break;
                                                            //    default:
                                                            //        break;
                                                            //}
                                                        }
                                                    }
                                                    catch (Exception e)
                                                    {

                                                    }
                                                    break;
                                                // протокол
                                                case "2":
                                                    DocumentGji docProtocol = docProtoc.Where(x => x.DocumentNumber == resultDoc.Number).FirstOrDefault();
                                                    if (docProtocol != null)
                                                    {
                                                        // ну нашли.. И что с ним делать?
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        req.Answer = "Данные из ГИС ЖКХ обработаны";
                        req.RequestState = GisGkhRequestState.ResponseProcessed;
                        GisGkhRequestsDomain.Update(req);
                    }
                    else
                    {
                        throw new Exception("Не найден файл с ответом из ГИС ЖКХ");
                    }
                }
                catch (Exception e)
                {
                    req.RequestState = GisGkhRequestState.Error;
                    req.Answer = "Ошибка при обработке данных из ГИС ЖКХ";
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("Ошибка: " + e.Message);
                }
            }
        }

        #endregion

        #region Private methods
        /// <summary>
        /// Получить хэш файла по алгоритму ГОСТ Р 34.11-94
        /// </summary>
        public static string GetGhostHash(Stream content)
        {
            return ";";
            // TODO: Заменить криптопро
            /*sing (var gost = Gost3411.Create())
            {
                if (gost == null)
                {
                    throw new ApplicationException("Не удалось получть хэш вложения по алгоритму ГОСТ-3411");
                }
                var hash = gost.ComputeHash(content);
                var hex = new StringBuilder(hash.Length * 2);
                foreach (var b in hash)
                {
                    hex.AppendFormat("{0:x2}", b);
                }
                return hex.ToString();
            }*/
        }

        /// <summary>
        /// Сериаилазация запроса
        /// </summary>
        /// <param name="data">Запрос</param>
        /// <returns>Xml-документ</returns>
        protected XmlDocument SerializeRequest(object data)
        {
            var type = data.GetType();
            XmlDocument result;

            var attr = (XmlTypeAttribute)type.GetCustomAttribute(typeof(XmlTypeAttribute));
            var xmlSerializer = new XmlSerializer(type, attr.Namespace);

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
                {
                    xmlSerializer.Serialize(xmlWriter, data);

                    result = new XmlDocument();
                    result.LoadXml(stringWriter.ToString());
                }
            }

            //var prefixer = new XmlNsPrefixer();
            //prefixer.Process(result);

            return result;
        }

        protected TDataType DeserializeData<TDataType>(string data)
        {
            TDataType result;

            var attr = (XmlTypeAttribute)typeof(TDataType).GetCustomAttribute(typeof(XmlTypeAttribute));
            var xmlSerializer = new XmlSerializer(typeof(TDataType), attr.Namespace);

            using (var reader = XmlReader.Create(new StringReader(data)))
            {
                result = (TDataType)xmlSerializer.Deserialize(reader);
            }

            return result;
        }

        private Bars.B4.Modules.FileStorage.FileInfo SaveFile(byte[] data, string fileName)
        {
            if (data == null)
                return null;

            try
            {
                //сохраняем пакет
                return _fileManager.SaveFile(fileName, data);
            }
            catch (Exception eeeeeeee)
            {
                return null;
            }
        }

        private void SaveFile(GisGkhRequests req, GisGkhFileType fileType, XmlDocument data, string fileName)
        {
            if (data == null)
                throw new Exception("Пустой документ для сохранения");

            MemoryStream stream = new MemoryStream();
            data.PreserveWhitespace = true;
            data.Save(stream);
            try
            {
                //сохраняем
                GisGkhRequestsFileDomain.Save(new GisGkhRequestsFile
                {
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now,
                    ObjectVersion = 1,
                    GisGkhRequests = req,
                    GisGkhFileType = fileType,
                    FileInfo = _fileManager.SaveFile(stream, fileName)
                });
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка при сохранении файла: " + e.Message);
            }
        }

        private string GetPrescriptionGuid(Disposal disposal)
        {
            if (disposal != null)
            {
                var parent = DocumentGjiChildrenDomain.GetAll()
                               .Where(x => x.Children == disposal)
                               .Select(x => new
                               {
                                   x.Parent
                               })
                               .FirstOrDefault();
                var prescription = PrescriptionDomain.Get(parent.Parent.Id);
                if (prescription != null)
                {
                    return prescription.GisGkhGuid;
                }
                else return null;
            }
            else return null;
        }

        private string GetObjective(Disposal disposal)
        {
            string result = null;
            if (disposal != null)
            {
                var parent = DocumentGjiChildrenDomain.GetAll()
                               .Where(x => x.Children == disposal)
                               .Select(x => new
                               {
                                   x.Parent
                               })
                               .FirstOrDefault();
                var disposalTypes = DisposalTypeSurveyDomain.GetAll()
                    .Where(x => x.Disposal == disposal).ToList();
                foreach (var disposalType in disposalTypes)
                {
                    var goals = TypeSurveyGoalInspGjiDomain.GetAll()
                        .Where(x => x.TypeSurvey == disposalType.TypeSurvey).ToList();
                    foreach (var goal in goals)
                    {
                        var goalstr = goal.SurveyPurpose.Name;
                        if (goalstr.Contains("предписан"))
                        {
                            var prescription = PrescriptionDomain.Get(parent.Parent.Id);
                            goalstr += string.Format(" № {0} от {1}",
                                prescription.DocumentNumber,
                                prescription.DocumentDate.HasValue ? prescription.DocumentDate.Value.ToString("D") : string.Empty);
                        }
                        //else if (goalstr.Contains())
                    }
                }
                return result;
                //var prescription = PrescriptionDomain.Get(parent.Parent.Id);
                //if (prescription != null)
                //{
                //    return string.Format("проверка исполнения предписания № {0} от {1}",
                //            prescription.DocumentNumber,
                //            prescription.DocumentDate.HasValue? prescription.DocumentDate.Value.ToString("D") : string.Empty);
                //}
                //else return null;
            }
            else return null;
        }

        private string GetTasks(Disposal disposal)
        {
            if (disposal != null)
            {
                var parent = DocumentGjiChildrenDomain.GetAll()
                               .Where(x => x.Children == disposal)
                               .Select(x => new
                               {
                                   x.Parent
                               })
                               .FirstOrDefault();
                var prescription = PrescriptionDomain.Get(parent.Parent.Id);
                if (prescription != null)
                {
                    return string.Format("проверка исполнения предписания № {0} от {1}",
                            prescription.DocumentNumber,
                            prescription.DocumentDate.HasValue ? prescription.DocumentDate.Value.ToString("D") : string.Empty);
                }
                else return null;
            }
            else return null;
        }

        #endregion

    }
}
