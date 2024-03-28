using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.Utils;
using Castle.Windsor;
using GisGkhLibrary.HouseManagementAsync;
using GisGkhLibrary.Services;
using GisGkhLibrary.Utils;
using Sobits.GisGkh.Entities;
using Sobits.GisGkh.Enums;
using Sobits.GisGkh.Tasks.ProcessGisGkhAnswers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Sobits.GisGkh.DomainService
{
    public class ImportAccountDataService : IImportAccountDataService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<Room> RoomDomain { get; set; }
        public IRepository<BasePersonalAccount> BasePersonalAccountRepo { get; set; }
        public IRepository<IndividualAccountOwner> IndividualAccountOwnerRepo { get; set; }
        public IRepository<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Constructors

        public ImportAccountDataService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, string roId)
        {
            try
            {
                var house = RealityObjectDomain.Get(long.Parse(roId));

                // Румы с гуидом
                var RoomsWithGuid = RoomDomain.GetAll()
                    .Where(x => x.RealityObject == house)
                    .Where(x => x.GisGkhPremisesGUID != null && x.GisGkhPremisesGUID != "");

                // Все ЛС этих румов
                var persAccs = BasePersonalAccountRepo.GetAll()
                    .Where(x => RoomsWithGuid.Contains(x.Room))
                    // Только активные
                    .Where(x => x.State.Code == "1")
                    //// Только физлиц
                    //.Where(x => x.AccountOwner.OwnerType == Bars.Gkh.RegOperator.Enums.PersonalAccountOwnerType.Individual)
                    //// Только без гуида
                    //.Where(x => (x.GisGkhGuid == null || x.GisGkhGuid == ""))
                    .ToList();
                var reqNum = persAccs.Count() / 100;
                if (persAccs.Count() % 100 > 0)
                {
                    reqNum++;
                }
                for (int i = 0; i < reqNum; i++)
                {
                    var persAccsPart = persAccs.Skip(i * 100).Take(100);
                    if (i > 0)
                    {
                        var newReq = new GisGkhRequests();
                        newReq.TypeRequest = req.TypeRequest;
                        newReq.Operator = req.Operator;
                        newReq.RequestState = GisGkhRequestState.NotFormed;
                        req = newReq;
                        GisGkhRequestsDomain.Save(req);
                    }

                    List<importAccountRequestAccount> accs = new List<importAccountRequestAccount>();
                    foreach (var persAcc in persAccsPart)
                    {
                        persAcc.GisGkhTransportGuid = Guid.NewGuid().ToString();
                        BasePersonalAccountRepo.Update(persAcc);
                        if (persAcc.AccountOwner.OwnerType == Bars.Gkh.RegOperator.Enums.PersonalAccountOwnerType.Individual)
                        {
                            var indAccOwner = IndividualAccountOwnerRepo.GetAll()
                                .Where(x => x == persAcc.AccountOwner).FirstOrDefault();
                            if (indAccOwner != null && !string.IsNullOrWhiteSpace(indAccOwner.Surname))
                            {
                                accs.Add(new importAccountRequestAccount
                                {
                                    AccountGUID = (persAcc.GisGkhGuid != null && persAcc.GisGkhGuid != "") ? persAcc.GisGkhGuid : null,
                                    TransportGUID = persAcc.GisGkhTransportGuid,
                                    AccountNumber = persAcc.PersonalAccountNum,
                                    ItemElementName = ItemChoiceType18.isCRAccount,
                                    Item = true,
                                    Accommodation = new AccountTypeAccommodation[]
                                    {
                                    new AccountTypeAccommodation
                                    {
                                        ItemElementName = ItemChoiceType19.PremisesGUID,
                                        Item = persAcc.Room.GisGkhPremisesGUID,
                                        SharePercent = persAcc.AreaShare * 100,
                                        SharePercentSpecified = true
                                    }
                                    },
                                    TotalSquare = Math.Round(persAcc.AreaShare * persAcc.Room.Area, 2),
                                    TotalSquareSpecified = true,
                                    PayerInfo = new AccountTypePayerInfo
                                    {
                                        Item = string.IsNullOrWhiteSpace(indAccOwner.SecondName) ?
                                        new AccountIndType
                                        {
                                            Surname = indAccOwner.Surname.Trim(),
                                            FirstName = string.IsNullOrWhiteSpace(indAccOwner.FirstName) ?
                                            "-" : indAccOwner.FirstName.Trim()
                                        }
                                        :
                                        new AccountIndType
                                        {
                                            Surname = indAccOwner.Surname.Trim(),
                                            FirstName = string.IsNullOrWhiteSpace(indAccOwner.FirstName) ?
                                            "-" : indAccOwner.FirstName.Trim(),
                                            Patronymic = indAccOwner.SecondName.Trim()
                                        },
                                    }
                                    //PayerInfo = new AccountTypePayerInfo
                                    //{
                                    //    Item = persAcc.AccountOwner.Name.Split().Count() == 1 ?
                                    //    new AccountIndType
                                    //    {
                                    //        Surname = persAcc.AccountOwner.Name.Split()[0]
                                    //    }
                                    //    : persAcc.AccountOwner.Name.Split().Count() == 2 ?
                                    //    new AccountIndType
                                    //    {
                                    //        Surname = persAcc.AccountOwner.Name.Split()[0],
                                    //        FirstName = persAcc.AccountOwner.Name.Split()[1]
                                    //    }
                                    //    : persAcc.AccountOwner.Name.Split().Count() >= 3 ?
                                    //    new AccountIndType
                                    //    {
                                    //        Surname = persAcc.AccountOwner.Name.Split()[0],
                                    //        FirstName = persAcc.AccountOwner.Name.Split()[1],
                                    //        Patronymic = persAcc.AccountOwner.Name.Split()[2]
                                    //    } : null,
                                    //}
                                });
                            }
                        }
                        else {
                            var legalAccOwner = LegalAccountOwnerDomain.GetAll()
                                .Where(x => x == persAcc.AccountOwner).FirstOrDefault();
                            if (legalAccOwner != null)
                            {
                                if (legalAccOwner.Contragent != null)
                                {
                                    if (!string.IsNullOrEmpty(legalAccOwner.Contragent.GisGkhVersionGUID))
                                    {
                                        accs.Add(new importAccountRequestAccount
                                        {
                                            AccountGUID = (persAcc.GisGkhGuid != null && persAcc.GisGkhGuid != "") ? persAcc.GisGkhGuid : null,
                                            TransportGUID = persAcc.GisGkhTransportGuid,
                                            AccountNumber = persAcc.PersonalAccountNum,
                                            ItemElementName = ItemChoiceType18.isCRAccount,
                                            Item = true,
                                            Accommodation = new AccountTypeAccommodation[]
                                            {
                                    new AccountTypeAccommodation
                                    {
                                        ItemElementName = ItemChoiceType19.PremisesGUID,
                                        Item = persAcc.Room.GisGkhPremisesGUID,
                                        SharePercent = persAcc.AreaShare * 100,
                                        SharePercentSpecified = true
                                    }
                                            },
                                            TotalSquare = Math.Round(persAcc.AreaShare * persAcc.Room.Area, 2),
                                            TotalSquareSpecified = true,
                                            PayerInfo = new AccountTypePayerInfo
                                            {
                                                Item = new RegOrgVersionType
                                                {
                                                    orgVersionGUID = legalAccOwner.Contragent.GisGkhVersionGUID
                                                }
                                            }
                                        });
                                    }
                                    else
                                    {
                                        // Если нет ГУИДа организации
                                        if (!string.IsNullOrEmpty(req.Answer))
                                        {
                                            req.Answer += ". ";
                                        }
                                        req.Answer += $"Владелец счёта {persAcc.AccountOwner.Name}: отсутствует идентификатор контрагента ГИС ЖКХ";
                                    }
                                }
                                else
                                {
                                    // Если нет контрагента
                                    if (!string.IsNullOrEmpty(req.Answer))
                                    {
                                        req.Answer += ". ";
                                    }
                                    req.Answer += $"Владелец счёта {persAcc.AccountOwner.Name}: не привязан контрагент";
                                }
                            }
                            else
                            {
                                // Если нет LegalAccountOwner
                                if (!string.IsNullOrEmpty(req.Answer))
                                {
                                    req.Answer += ". ";
                                }
                                req.Answer += $"Владелец счёта {persAcc.AccountOwner.Name}: не привязан контрагент";
                            }
                        }
                    }

                    var request = HouseManagementAsyncService.importAccountDataReq(accs.ToArray());
                    var prefixer = new XmlNsPrefixer();
                    XmlDocument document = SerializeRequest(request);
                    prefixer.Process(document);
                    SaveFile(req, GisGkhFileType.request, document, "request.xml");
                    req.RequestState = GisGkhRequestState.Formed;
                    GisGkhRequestsDomain.Update(req);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Ошибка: " + e.Message);
            }
        }

        public void SaveRequest(GisGkhRequests req, List<long> lsIds)
        {
            try
            {
                var persAccs = BasePersonalAccountRepo.GetAll()
                     .Where(x => lsIds.Contains(x.Id))
                     .ToList();


                var accs = new List<importAccountRequestAccount>();
                foreach (var persAcc in persAccs)
                {
                    persAcc.GisGkhTransportGuid = Guid.NewGuid().ToString();
                    BasePersonalAccountRepo.Update(persAcc);
                    if (persAcc.AccountOwner.OwnerType == Bars.Gkh.RegOperator.Enums.PersonalAccountOwnerType.Individual)
                    {
                        var indAccOwner = IndividualAccountOwnerRepo.GetAll()
                            .Where(x => x == persAcc.AccountOwner).FirstOrDefault();
                        if (indAccOwner != null && !string.IsNullOrWhiteSpace(indAccOwner.Surname))
                        {
                            accs.Add(new importAccountRequestAccount
                            {
                                AccountGUID = (persAcc.GisGkhGuid != null && persAcc.GisGkhGuid != "") ? persAcc.GisGkhGuid : null,
                                TransportGUID = persAcc.GisGkhTransportGuid,
                                AccountNumber = persAcc.PersonalAccountNum,
                                ItemElementName = ItemChoiceType18.isCRAccount,
                                Item = true,
                                Accommodation = new AccountTypeAccommodation[]
                                {
                                   new AccountTypeAccommodation
                                   {
                                       ItemElementName = ItemChoiceType19.PremisesGUID,
                                       Item = persAcc.Room.GisGkhPremisesGUID,
                                       SharePercent = persAcc.AreaShare * 100,
                                       SharePercentSpecified = true
                                   }
                                },
                                TotalSquare = Math.Round(persAcc.AreaShare * persAcc.Room.Area, 2),
                                TotalSquareSpecified = true,
                                PayerInfo = new AccountTypePayerInfo
                                {
                                    Item = string.IsNullOrWhiteSpace(indAccOwner.SecondName)
                                        ? new AccountIndType
                                        {
                                            Surname = indAccOwner.Surname.Trim(),
                                            FirstName = string.IsNullOrWhiteSpace(indAccOwner.FirstName) ? "-" : indAccOwner.FirstName.Trim()
                                        }
                                        : new AccountIndType
                                        {
                                            Surname = indAccOwner.Surname.Trim(),
                                            FirstName = string.IsNullOrWhiteSpace(indAccOwner.FirstName) ? "-" : indAccOwner.FirstName.Trim(),
                                            Patronymic = indAccOwner.SecondName.Trim()
                                        },
                                }
                            });
                        }
                    }
                    else
                    {
                        var legalAccOwner = LegalAccountOwnerDomain
                            .GetAll()
                            .FirstOrDefault(x => x == persAcc.AccountOwner);
                        if (legalAccOwner != null)
                        {
                            if (legalAccOwner.Contragent != null)
                            {
                                if (!string.IsNullOrEmpty(legalAccOwner.Contragent.GisGkhVersionGUID))
                                {
                                    accs.Add(new importAccountRequestAccount
                                    {
                                        AccountGUID = (persAcc.GisGkhGuid != null && persAcc.GisGkhGuid != "") ? persAcc.GisGkhGuid : null,
                                        TransportGUID = persAcc.GisGkhTransportGuid,
                                        AccountNumber = persAcc.PersonalAccountNum,
                                        ItemElementName = ItemChoiceType18.isCRAccount,
                                        Item = true,
                                        Accommodation = new AccountTypeAccommodation[]
                                        {
                                           new AccountTypeAccommodation
                                           {
                                               ItemElementName = ItemChoiceType19.PremisesGUID,
                                               Item = persAcc.Room.GisGkhPremisesGUID,
                                               SharePercent = persAcc.AreaShare * 100,
                                               SharePercentSpecified = true
                                           }
                                        },
                                        TotalSquare = Math.Round(persAcc.AreaShare * persAcc.Room.Area, 2),
                                        TotalSquareSpecified = true,
                                        PayerInfo = new AccountTypePayerInfo
                                        {
                                            Item = new RegOrgVersionType
                                            {
                                                orgVersionGUID = legalAccOwner.Contragent.GisGkhVersionGUID
                                            }
                                        }
                                    });
                                }
                                //else
                                // {
                                continue;
                                // Если нет ГУИДа организации
                                //if (!string.IsNullOrEmpty(req.Answer))

                                //     req.Answer += ". ";
                                // }

                                // req.Answer += $"Владелец счёта {persAcc.AccountOwner.Name}: отсутствует идентификатор контрагента ГИС ЖКХ";
                                //}
                            }
                            //else
                            // {
                            //   // Если нет контрагента
                            //   if (!string.IsNullOrEmpty(req.Answer))
                            //   {
                            //       req.Answer += ". ";
                            //   }

                            //   req.Answer += $"Владелец счёта {persAcc.AccountOwner.Name}: не привязан контрагент";
                            // }
                        }
                        //else
                        // {
                        // Если нет LegalAccountOwner
                        //   if (!string.IsNullOrEmpty(req.Answer))
                        //   {
                        //       req.Answer += ". ";
                        //   }

                        //   req.Answer += $"Владелец счёта {persAcc.AccountOwner.Name}: не привязан контрагент";
                        // }

                    }
                }

                var request = HouseManagementAsyncService.importAccountDataReq(accs.ToArray());
                var prefixer = new XmlNsPrefixer();
                XmlDocument document = SerializeRequest(request);
                prefixer.Process(document);
                SaveFile(req, GisGkhFileType.request, document, "request.xml");
                req.RequestState = GisGkhRequestState.Formed;
                GisGkhRequestsDomain.Update(req);
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
                var response = HouseManagementAsyncService.GetState(req.MessageGUID, orgPPAGUID);
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
                            req.Answer = $"Задача на обработку ответа importAccountData поставлена в очередь с id {taskInfo.TaskId}";
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

        public void ProcessAnswer (GisGkhRequests req)
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
                        var errors = "";
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
                            else if (responseItem is getStateResultImportResult)
                            {
                                foreach (var obj in ((getStateResultImportResult)responseItem).Items)
                                {
                                    if (obj is getStateResultImportResultCommonResult)
                                    {
                                        var persAcc = BasePersonalAccountRepo.GetAll()
                                                .Where(x => x.GisGkhTransportGuid == ((getStateResultImportResultCommonResult)obj).TransportGUID).FirstOrDefault();
                                        if (((getStateResultImportResultCommonResult)obj).Items[0] is CommonResultTypeError)
                                        {
                                            // ошибка по ЛС
                                            if (!string.IsNullOrEmpty(errors))
                                            {
                                                errors += ": ";
                                            }
                                            errors += $"ЛС №{persAcc.PersonalAccountNum}: {((CommonResultTypeError)((getStateResultImportResultCommonResult)obj).Items[0]).Description}";
                                        }
                                        if (((getStateResultImportResultCommonResult)obj).Item is getStateResultImportResultCommonResultImportAccount)
                                        {
                                            if (persAcc != null)
                                            {
                                                persAcc.GisGkhGuid = ((getStateResultImportResultCommonResult)obj).GUID;
                                                persAcc.UnifiedAccountNumber = ((getStateResultImportResultCommonResultImportAccount)((getStateResultImportResultCommonResult)obj).Item).UnifiedAccountNumber;
                                                persAcc.ServiceId = ((getStateResultImportResultCommonResultImportAccount)((getStateResultImportResultCommonResult)obj).Item).ServiceID;
                                                BasePersonalAccountRepo.Update(persAcc);
                                            }
                                        }
                                    }
                                }
                                // todo тут обработка результата
                            }
                        }
                        if (!string.IsNullOrEmpty(errors))
                        {
                            req.Answer = $"Данные из ГИС ЖКХ обработаны. {errors}";
                        }
                        else
                        {
                            req.Answer = "Данные из ГИС ЖКХ обработаны";
                        }
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
                    //req.RequestState = GisGkhRequestState.Error;
                    //GisGkhRequestsDomain.Update(req);
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

        #endregion

    }
}
