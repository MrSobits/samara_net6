using Bars.B4;
using Bars.B4.Application;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.FileManager;
using Castle.Windsor;
using GisGkhLibrary.LicenseServiceAsync;
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
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Sobits.GisGkh.DomainService
{
    public class ExportLicenseService : IExportLicenseService
    {
        #region Constants


        #endregion

        #region Properties              

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IRepository<Contragent> ContragentRepo { get; set; }
        public IRepository<RealityObject> RealityObjectRepo { get; set; }
        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }
        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }
        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Constructors

        public ExportLicenseService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, List<long> contragentIds)
        {
            string log = string.Empty;
            if (req == null)
            {
                req = new GisGkhRequests();
                req.TypeRequest = GisGkhTypeRequest.exportLicense;
                //req.ReqDate = DateTime.Now;
                req.RequestState = GisGkhRequestState.NotFormed;
                GisGkhRequestsDomain.Save(req);
            }
            if (req.LogFile != null)
            {
                StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                log = reader.ReadToEnd();
                log += "\r\n";
            }
            log += $"{DateTime.Now} Формирование запроса на получение лицензий из ГИС ЖКХ\r\n";
            List<string> ogrns = new List<string>();
            try
            {
                foreach (var contragentId in contragentIds)
                {
                    var contragent = ContragentRepo.Get(contragentId);
                    log += $"ОГРН: {contragent.Ogrn}, \"{contragent.Name}\" ";
                    var ogrn = Regex.Replace(contragent.Ogrn, @"\D+", string.Empty);
                    if (ogrn.Length == 13 || ogrn.Length == 15)
                    {
                        ogrns.Add(ogrn);
                        log += "добавлено в запрос\r\n";
                    }
                    else
                    {
                        //что-то не то с ОГРН
                        log += "не добавлено в запрос - проверьте правильность ОГРН\r\n";
                    }
                }

                var request = LicenseServiceAsync.exportLicenseReq(ogrns);

                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
                log += $"{DateTime.Now} Запрос сформирован\r\n";
                SaveLog(ref req, ref log);
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
                string log = string.Empty;
                if (req.LogFile != null)
                {
                    StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                    log = reader.ReadToEnd();
                    log += "\r\n";
                }
                log += $"{DateTime.Now} Запрос ответа из ГИС ЖКХ\r\n";
                var response = LicenseServiceAsync.GetState(req.MessageGUID, orgPPAGUID);
                if (response.RequestState == 3)
                {
                    //log += $"{DateTime.Now} Ответ получен\r\n";
                    // Удаляем старые файлы ответов, если они, вдруг, имеются
                    GisGkhRequestsFileDomain.GetAll()
                        .Where(x => x.GisGkhRequests == req)
                        .Where(x => x.GisGkhFileType == GisGkhFileType.answer)
                        .ToList().ForEach(x => GisGkhRequestsFileDomain.Delete(x.Id));
                    SaveFile(req, GisGkhFileType.answer, SerializeRequest(response), "response.xml");
                    req.Answer = "Ответ получен";
                    req.RequestState = GisGkhRequestState.ResponseReceived;
                    log += $"{DateTime.Now} Ответ из ГИС ЖКХ получен. Ставим задачу на обработку ответа\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                    BaseParams baseParams = new BaseParams();
                    baseParams.Params.Add("reqId", req.Id.ToString());
                    try
                    {
                        var taskInfo = _taskManager.CreateTasks(new ProcessGisGkhAnswersTaskProvider(_container), baseParams).Data.Descriptors.FirstOrDefault();
                        if (taskInfo == null)
                        {
                            req.Answer = "Сбой создания задачи обработки ответа";
                            log += $"{DateTime.Now} Сбой создания задачи обработки ответа\r\n";
                            SaveLog(ref req, ref log);
                            //GisGkhRequestsDomain.Update(req);
                            //req.RequestState = GisGkhRequestState.У; ("Сбой создания задачи");
                        }
                        else
                        {
                            req.Answer = $"Задача на обработку ответа exportLicense поставлена в очередь с id {taskInfo.TaskId}";
                            //GisGkhRequestsDomain.Update(req);
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
                    log += $"{DateTime.Now} Запрос ещё в очереди\r\n";
                    SaveLog(ref req, ref log);
                }
                GisGkhRequestsDomain.Update(req);
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        public void ProcessAnswer(GisGkhRequests req)
        {
            string log = string.Empty;
            if (req.LogFile != null)
            {
                StreamReader reader = new StreamReader(_fileManager.GetFile(req.LogFile));
                log = reader.ReadToEnd();
                log += "\r\n";
            }
            log += $"{DateTime.Now} Обработка ответа\r\n";
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
                        Dictionary<LicenseTypeLicenseStatus, string> licStateDict = new Dictionary<LicenseTypeLicenseStatus, string>
                        {
                            {LicenseTypeLicenseStatus.A, "активна" },
                            {LicenseTypeLicenseStatus.C, "аннулирована" },
                            {LicenseTypeLicenseStatus.F, "включена в реестр, действие прекращено в зависимости от даты окончания действия лицензии" },
                            {LicenseTypeLicenseStatus.I, "не включена в реестр, не действующая" },
                            {LicenseTypeLicenseStatus.R, "отменена в зависимости от основания" }
                        };
                        foreach (var responseItem in response.Items)
                        {
                            if (responseItem is GisGkhLibrary.LicenseServiceAsync.ErrorMessageType)
                            {
                                req.RequestState = GisGkhRequestState.Error;
                                var error = (GisGkhLibrary.LicenseServiceAsync.ErrorMessageType)responseItem;
                                req.Answer = $"{error.ErrorCode}:{error.Description}";
                                log += $"{DateTime.Now} ГИС ЖКХ вернула ошибку: {error.ErrorCode}:{error.Description}\r\n";
                            }
                            else if (responseItem is exportLicenseResultType)
                            {
                                var licResult = (exportLicenseResultType)responseItem;
                                    string ogrn = null;
                                if (licResult.LicenseOrganization.Item is LicenseOrganizationTypeEntrp) // ИП
                                {
                                    var ip = (LicenseOrganizationTypeEntrp)(licResult.LicenseOrganization.Item);
                                    ogrn = ip.OGRNIP;
                                    log += $"{DateTime.Now} Лицензия ИП {ip.Surname} {ip.FirstName} {ip.Patronymic} (ОГРНИП {ogrn})\r\n";
                                }
                                else if (licResult.LicenseOrganization.Item is LicenseOrganizationTypeLegal) // организация
                                {
                                    var org = (LicenseOrganizationTypeLegal)(licResult.LicenseOrganization.Item);
                                    ogrn = org.OGRN;
                                    log += $"{DateTime.Now} Лицензия организации {org.FullName} (ОГРН {ogrn})\r\n";
                                }
                                log += $"Номер лицензии в ГИС ЖКХ: {licResult.LicenseNumber}\r\n";
                                log += $"Дата регистрации лицензии в ГИС ЖКХ: {licResult.LicenseRegDate}\r\n";
                                log += $"Лицензируемый вид дейтельности в ГИС ЖКХ: {licResult.LicensableTypeOfActivity}\r\n";
                                log += $"Статус лицензии в ГИС ЖКХ: {licStateDict[licResult.LicenseStatus]}\r\n";
                                var contragent = ContragentRepo.GetAll()
                                    .Where(x => x.Ogrn == ogrn).FirstOrDefault();
                                if (contragent != null)
                                {
                                    var contractsRO = ManOrgContractRealityObjectDomain.GetAll()
                                        .Where(x => x.ManOrgContract.ManagingOrganization.Contragent == contragent)
                                        .Where(x => (!x.ManOrgContract.StartDate.HasValue
                                           || x.ManOrgContract.StartDate <= DateTime.Now.Date)
                                           && (!x.ManOrgContract.EndDate.HasValue
                                           || x.ManOrgContract.EndDate >= DateTime.Now.Date));
                                    var manOrgLics = ManOrgLicenseDomain.GetAll()
                                        .Where(x => x.Contragent == contragent);
                                    log += $"Количество лицензий по данному контрагенту в системе: {manOrgLics.Count()}\r\n";
                                    foreach (var manOrgLic in manOrgLics)
                                    {
                                        log += $"Номер лицензии: {manOrgLic.LicNumber}\r\n";
                                        log += $"Дата внесения в реестр: {manOrgLic.DateRegister}\r\n";
                                        log += $"Статус лицензии: {manOrgLic.State.Name}\r\n";
                                        // сопоставляем
                                        if (licResult.LicenseNumber.Contains(manOrgLic.LicNumber))
                                        {
                                            log += $"Лицензия ГИС ЖКХ {licResult.LicenseNumber} соответствует лицензии {manOrgLic.LicNumber} в системе\r\n";
                                        }
                                    }
                                    // проверяем контракты и дома в активной гисовой лицензии
                                    if (licResult.LicenseStatus == LicenseTypeLicenseStatus.A)
                                    {
                                        log += $"Договоры управления домами активной лицензии {licResult.LicenseNumber} из ГИС ЖКХ:\r\n";
                                        foreach (var house in licResult.House)
                                        {
                                            log += $"{house.HouseAddress}\r\n";
                                            log += $"Договор управления {house.Contract.DocNum}, " +
                                                $"дата начала управления {(house.Contract.StartDateSpecified ? house.Contract.StartDate.ToShortDateString() : "не указана")}, " +
                                                $"дата окончания управления {(house.Contract.EndDateSpecified ? house.Contract.EndDate.ToShortDateString() : "не указана")}\r\n";
                                            var ro = RealityObjectRepo.GetAll().Where(x => x.HouseGuid == house.FIASHouseGUID
                                                || (x.FiasAddress.HouseGuid.HasValue && x.FiasAddress.HouseGuid.ToString() == house.FIASHouseGUID));
                                            if (ro == null)
                                            {
                                                log += $"В системе не найден дом с ФИАС ГУИД {house.FIASHouseGUID}\r\n";
                                            }
                                            else
                                            {
                                                var contractsSelectedRO = contractsRO.Where(x => x.RealityObject == ro);
                                                switch (contractsSelectedRO.Count())
                                                {
                                                    case 0:
                                                        log += $"В системе не найден соответствующий договор управления\r\n";
                                                        break;
                                                    case 1:
                                                        var contractSelectedRO = contractsSelectedRO.First();
                                                        log += $"По данному дому в системе имеется договор управления {contractSelectedRO.ManOrgContract.DocumentNumber}, " +
                                                            $"дата начала управления {(contractSelectedRO.ManOrgContract.StartDate.HasValue ? contractSelectedRO.ManOrgContract.StartDate.Value.ToShortDateString() : "не указана")}, " +
                                                            $"дата окончания управления {(contractSelectedRO.ManOrgContract.EndDate.HasValue ? contractSelectedRO.ManOrgContract.EndDate.Value.ToShortDateString() : "не указана")}\r\n";
                                                        break;
                                                    default:
                                                        log += $"По данному дому в системе имеется несколько {contractsSelectedRO.Count()} договоров управления\r\n";
                                                        foreach (var contractSelectedROMany in contractsSelectedRO)
                                                        {
                                                            log += $"Договор управления {contractSelectedROMany.ManOrgContract.DocumentNumber}, " +
                                                                $"дата начала управления {(contractSelectedROMany.ManOrgContract.StartDate.HasValue ? contractSelectedROMany.ManOrgContract.StartDate.Value.ToShortDateString() : "не указана")}, " +
                                                                $"дата окончания управления {(contractSelectedROMany.ManOrgContract.EndDate.HasValue ? contractSelectedROMany.ManOrgContract.EndDate.Value.ToShortDateString() : "не указана")}\r\n";
                                                        }
                                                        break;
                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    log += $"В системе не найден контрагент с указанным ОГРН\r\n";
                                }
                                //if (orgResult.OrgVersion.Item is LegalType)
                                //{
                                //    ogrn = ((LegalType)orgResult.OrgVersion.Item).OGRN;
                                //}
                                //else if (orgResult.OrgVersion.Item is exportOrgRegistryResultTypeOrgVersionSubsidiary)
                                //{
                                //    ogrn = ((exportOrgRegistryResultTypeOrgVersionSubsidiary)orgResult.OrgVersion.Item).OGRN;
                                //}
                                //else if (orgResult.OrgVersion.Item is EntpsType)
                                //{
                                //    ogrn = ((EntpsType)orgResult.OrgVersion.Item).OGRNIP;
                                //}

                                //if (ogrn != null)
                                //{
                                //    var contragent = ContragentRepo.GetAll()
                                //        .Where(x => x.Ogrn.Trim() == ogrn)
                                //        .FirstOrDefault();
                                //    if (contragent != null)
                                //    {
                                //        contragent.GisGkhGUID = orgResult.orgRootEntityGUID;
                                //        contragent.GisGkhVersionGUID = orgResult.OrgVersion.orgVersionGUID;
                                //        contragent.GisGkhOrgPPAGUID = orgResult.orgPPAGUID;
                                //        ContragentRepo.Update(contragent);
                                //    }

                                //    var creditOrg = CreditOrgRepo.GetAll()
                                //        .Where(x => x.Ogrn.Trim() == ogrn)
                                //        .FirstOrDefault();
                                //    if (creditOrg != null)
                                //    {
                                //        creditOrg.GisGkhOrgRootEntityGUID = orgResult.orgRootEntityGUID;
                                //        CreditOrgRepo.Update(creditOrg);
                                //    }
                                //}
                            }
                        }
                        req.Answer = "Данные из ГИС ЖКХ обработаны";
                        req.RequestState = GisGkhRequestState.ResponseProcessed;
                        log += $"{DateTime.Now} Обработка ответа завершена\r\n";
                        SaveLog(ref req, ref log);
                        GisGkhRequestsDomain.Update(req);
                    }
                    else
                    {
                        log += $"{DateTime.Now} Не найден файл с ответом из ГИС ЖКХ\r\n";
                        SaveLog(ref req, ref log);
                        GisGkhRequestsDomain.Update(req);
                        throw new Exception("Не найден файл с ответом из ГИС ЖКХ");
                    }
                }
                catch (Exception e)
                {
                    //req.RequestState = GisGkhRequestState.Error;
                    log += $"{DateTime.Now} Ошибка: {e.Message}\r\n";
                    SaveLog(ref req, ref log);
                    GisGkhRequestsDomain.Update(req);
                    throw new Exception("Ошибка: " + e.Message);
                }
            }
        }

        #endregion

        #region Private methods

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

        private void SaveLog(ref GisGkhRequests req, ref string log)
        {
            if (req.LogFile != null)
            {
                var FileInfo = req.LogFile;
                req.LogFile = null;
                GisGkhRequestsDomain.Update(req);
                _fileManager.Delete(FileInfo);
                var fullPath = $"{((FileSystemFileManager)_fileManager).FilesDirectory.FullName}\\{FileInfo.ObjectCreateDate.Year}\\{FileInfo.ObjectCreateDate.Month}\\{FileInfo.Id}.{FileInfo.Extention}";
                //var fullPath = $"{FtpPath}\\{FileInfo.ObjectCreateDate.Year}\\{FileInfo.ObjectCreateDate.Month}\\{FileInfo.Id}.{FileInfo.Extention}";
                try
                {
                    System.IO.File.Delete(fullPath);
                }
                catch
                {
                    log += $"{DateTime.Now} Не удалось удалить старый лог-файл\r\n";
                }
            }
            req.LogFile = _fileManager.SaveFile("log.txt", Encoding.UTF8.GetBytes(log));
            //GisGkhRequestsDomain.Update(req);
        }

        #endregion
    }
}
