using Bars.B4;
using Bars.B4.Config;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Authentification;
using Bars.Gkh.Decisions.Nso.Domain;
using Bars.Gkh.Decisions.Nso.Entities;
using Bars.Gkh.Decisions.Nso.Entities.Decisions;
using Bars.Gkh.Decisions.Nso.Entities.Proxies;
using Bars.Gkh.Domain;
using Bars.Gkh.DomainService;
using Bars.Gkh.Entities;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.RegOperator.Entities.Decisions;
using Bars.Gkh.RegOperator.Enums;
using Bars.Gkh.Utils;
using Castle.Windsor;
using GisGkhLibrary.HcsCapitalRepairAsync;
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
//using CryptoPro.Sharpei;
using Sobits.GisGkh.File;

namespace Sobits.GisGkh.DomainService
{
    public class ImportDecisionsFormingFundService : IImportDecisionsFormingFundService
    {
        #region Constants



        #endregion

        #region Properties              

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }
        public IDomainService<Room> RoomDomain { get; set; }
        public IRepository<BasePersonalAccount> BasePersonalAccountRepo { get; set; }
        public IRepository<RealityObjectDecisionProtocol> RealityObjectDecisionProtocolRepo { get; set; }
        public IRepository<UltimateDecision> UltimateDecisionRepo { get; set; }
        public IRepository<CrFundFormationDecision> CrFundFormationDecisionRepo { get; set; }
        public IRepository<CoreDecision> CoreDecisionRepo { get; set; }
        public IRepository<GovDecision> GovDecisionRepo { get; set; }
        public IRepository<CreditOrgDecision> CreditOrgDecisionRepo { get; set; }
        public IRepository<DecisionNotification> DecisionNotificationRepo { get; set; }

        public IDomainService<GisGkhRequestsFile> GisGkhRequestsFileDomain { get; set; }

        public IDomainService<GisGkhRequests> GisGkhRequestsDomain { get; set; }

        public IWindsorContainer Container { get; set; }

        private IExportOrgRegistryService _ExportOrgRegistryService;

        private IFileService _fileService;

        #endregion

        #region Fields

        private IFileManager _fileManager;
        private readonly ITaskManager _taskManager;
        private readonly IWindsorContainer _container;
        public IGkhUserManager UserManager { get; set; }

        #endregion

        #region Constructors

        public ImportDecisionsFormingFundService(IWindsorContainer container, IFileManager fileManager, ITaskManager taskManager,
            IExportOrgRegistryService ExportOrgRegistryService, IFileService FileService)
        {
            _container = container;
            _fileManager = fileManager;
            _taskManager = taskManager;
            _ExportOrgRegistryService = ExportOrgRegistryService;
            _fileService = FileService;
        }

        #endregion

        #region Public methods

        public void SaveRequest(GisGkhRequests req, string roId, string OrgPPAGUID)
        {
            try
            {
                var house = RealityObjectDomain.Get(long.Parse(roId));
                string GUID = "";
                if (house.HouseGuid != null && house.HouseGuid != "")
                {
                    GUID = house.HouseGuid;
                }
                else
                {
                    GUID = house.FiasAddress.HouseGuid.ToString();
                }

                var protocol = this.RealityObjectDecisionProtocolRepo.GetAll()
                .Where(x => x.RealityObject.Id == long.Parse(roId))
                .Where(x => x.State.FinalState)
                .Select(x => new
                {
                    Type = CoreDecisionType.Owners,
                    x.Id,
                    x.ProtocolDate,
                    DocNum = x.DocumentNum,
                    x.DateStart,
                    x.File,
                    AttachmentGUID = x.GisGkhAttachmentGuid,
                    TransportGUID = x.GisGkhTransportGuid,
                    GUID = x.GisGkhGuid
                })
                .AsEnumerable()
                .Union(this.GovDecisionRepo.GetAll()
                        .Where(x => x.RealityObject.Id == long.Parse(roId))
                        .Where(x => x.State.FinalState && x.FundFormationByRegop)
                        .Select(x => new
                        {
                            Type = CoreDecisionType.Government,
                            x.Id,
                            x.ProtocolDate,
                            DocNum = x.ProtocolNumber,
                            x.DateStart,
                            File = x.ProtocolFile,
                            AttachmentGUID = x.GisGkhAttachmentGuid,
                            TransportGUID = x.GisGkhTransportGuid,
                            GUID = x.GisGkhGuid
                        })
                        .AsEnumerable()
                        )
                .OrderByDescending(x => x.ProtocolDate)
                .FirstOrDefault();

                if (protocol != null && protocol.ProtocolDate != null && protocol.DateStart != null && protocol.ProtocolDate != DateTime.MinValue && protocol.DateStart != DateTime.MinValue)
                {
                    List<importDecisionsFormingFundRequestImportDecision> importDecision = new List<importDecisionsFormingFundRequestImportDecision>();

                    // Если это протокол собрания собственников
                    //if (ownerDecision != null)
                    if (protocol.Type == CoreDecisionType.Owners)
                    {
                        var ownerDecision = RealityObjectDecisionProtocolRepo.GetAll()
                            .Where(x => x.Id == protocol.Id).FirstOrDefault();
                        
                        //var ultDecision = UltimateDecisionRepo.GetAll()
                        //    .Where(x => x.Protocol == ownerDecision).FirstOrDefault();

                        //if (ultDecision != null)
                        //{
                        //var coreDecision = CoreDecisionRepo.GetAll()
                        //.Where(x => x.UltimateDecision == ultDecision).FirstOrDefault();

                        //if (coreDecision != null)
                        //{
                        //var protocolType = coreDecision.DecisionType;
                        var baseParams = new BaseParams();
                        baseParams.Params["protocolT"] = protocol.Type.ToString();
                        baseParams.Params["protocolId"] = ownerDecision.Id.ToString();
                        baseParams.Params["roId"] = ownerDecision.RealityObject.Id.ToString();

                        var robjectDecisionService = this.Container.Resolve<IRobjectDecisionService>();
                        try
                        {
                            UltimateDecisionProxy robjectDecision = (UltimateDecisionProxy)robjectDecisionService.Get(baseParams).Data;
                            ownerDecision.GisGkhTransportGuid = Guid.NewGuid().ToString();

                            if (robjectDecision.AccountOwnerDecision.DecisionType == Bars.Gkh.Enums.Decisions.AccountOwnerDecisionType.Custom)
                            {
                                throw new Exception("Региональный оператор не является владельцем специального счёта данного дома");
                            }
                            if (ownerDecision.File != null && (ownerDecision.GisGkhAttachmentGuid == null || ownerDecision.GisGkhAttachmentGuid == ""))
                            {
                                var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.funddecisions, ownerDecision.File, OrgPPAGUID);
                                ////5 МБ = 5242880 Б 
                                //var uploaderName = ownerDecision.File.Size <= 5242880 ? "SimpleFileUploader" : "FileUploader";

                                //var uploader = Container.Resolve<IFileUploader>(uploaderName);

                                //var uploadResult = uploader.UploadFile(GisGkhLibrary.Enums.GisFileRepository.funddecisions, ownerDecision.File, OrgPPAGUID);

                                if (uploadResult.Success)
                                {
                                    ownerDecision.GisGkhAttachmentGuid = uploadResult.FileGuid;
                                    RealityObjectDecisionProtocolRepo.Update(ownerDecision);
                                }
                                else
                                {
                                    throw new Exception(uploadResult.Message);
                                }
                            }
                            if (robjectDecision.CrFundFormationDecision.Decision == Bars.Gkh.Enums.Decisions.CrFundFormationDecisionType.SpecialAccount)
                            {
                                var notification = DecisionNotificationRepo.GetAll()
                                    .Where(x => x.Protocol == ownerDecision).FirstOrDefault();
                                if (notification == null)
                                {
                                    throw new Exception("Отсутстует уведомление об открытии специального счёта");
                                }
                                if (notification.BankDoc == null)
                                {
                                    throw new Exception("Отсутстует справка об открытии специального счёта");
                                }
                                else if (notification.GisGkhAttachmentGuid == null || notification.GisGkhAttachmentGuid == "")
                                {
                                    var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.funddecisions, notification.BankDoc, OrgPPAGUID);
                                    ////5 МБ = 5242880 Б 
                                    //var uploaderName = notification.BankDoc.Size <= 5242880 ? "SimpleFileUploader" : "FileUploader";

                                    //var uploader = Container.Resolve<IFileUploader>(uploaderName);

                                    //var uploadResult = uploader.UploadFile(GisGkhLibrary.Enums.GisFileRepository.funddecisions, notification.BankDoc, OrgPPAGUID);

                                    if (uploadResult.Success)
                                    {
                                        notification.GisGkhAttachmentGuid = uploadResult.FileGuid;
                                        DecisionNotificationRepo.Update(notification);
                                    }
                                    else
                                    {
                                        throw new Exception(uploadResult.Message);
                                    }
                                }

                                // Запрос информации о кредитной организации
                                var creditOrgDecision = robjectDecision.CreditOrgDecision;
                                if ((creditOrgDecision.Decision.GisGkhOrgRootEntityGUID == null) || (creditOrgDecision.Decision.GisGkhOrgRootEntityGUID == ""))
                                {
                                    try
                                    {
                                        _ExportOrgRegistryService.SaveRequest(null, creditOrgDecision.Decision);
                                        throw new Exception("Сформирован запрос информации об организации, необходимо его отправить");
                                    }
                                    catch (Exception e)
                                    {
                                        throw new Exception("Ошибка при запросе информации об организации: " + e.Message);
                                    }
                                }

                                if (ownerDecision.File != null)
                                {
                                    importDecision.Add(new importDecisionsFormingFundRequestImportDecision
                                    {
                                        TransportGuid = ownerDecision.GisGkhTransportGuid,
                                        Item = new importDecisionsFormingFundRequestImportDecisionLoadDecision
                                        {
                                            FIASHouseGuid = GUID,
                                            DateEffective = ownerDecision.DateStart,
                                            Item = new DecisionTypeProtocol
                                            {
                                                ItemsElementName = new ItemsChoiceType3[]
                                                {
                                                    ItemsChoiceType3.Number,
                                                    ItemsChoiceType3.Date,
                                                    ItemsChoiceType3.Attachment
                                                },
                                                Items = new object[]
                                                {
                                                    ownerDecision.DocumentNum,
                                                    ownerDecision.ProtocolDate,
                                                    new AttachmentType
                                                    {
                                                        Attachment = new Attachment
                                                        {
                                                            AttachmentGUID = ownerDecision.GisGkhAttachmentGuid
                                                        },
                                                        Description = $"Протокол собрания собственников от {ownerDecision.ProtocolDate.ToShortDateString()} №{ownerDecision.DocumentNum}",
                                                        Name = ownerDecision.File.FullName,
                                                        AttachmentHASH = GetGhostHash(_fileManager.GetFile(ownerDecision.File))
                                                    }
                                                }
                                            },
                                            Item1 = new DecisionTypeFormationFundInSpecialAccount
                                            {
                                                AccountCreationDate = notification.OpenDate,
                                                AccountNumber = notification.AccountNum,
                                                BIKCredOrg = creditOrgDecision.Decision.Bik,
                                                CreditOrganization = new RegOrgType
                                                {
                                                    orgRootEntityGUID = creditOrgDecision.Decision.GisGkhOrgRootEntityGUID
                                                },
                                                AccountOpeningDocument = new AttachmentType[]
                                                {
                                                    new AttachmentType
                                                    {
                                                        Attachment = new Attachment
                                                        {
                                                            AttachmentGUID = notification.GisGkhAttachmentGuid
                                                        },
                                                        Description = notification.BankDoc.Name,
                                                        Name = notification.BankDoc.FullName,
                                                        AttachmentHASH = GetGhostHash(_fileManager.GetFile(notification.BankDoc))
                                                    }
                                                }
                                            }
                                        }
                                    });
                                }
                                else
                                {
                                    importDecision.Add(new importDecisionsFormingFundRequestImportDecision
                                    {
                                        TransportGuid = ownerDecision.GisGkhTransportGuid,
                                        Item = new importDecisionsFormingFundRequestImportDecisionLoadDecision
                                        {
                                            FIASHouseGuid = GUID,
                                            DateEffective = ownerDecision.DateStart,
                                            Item = new DecisionTypeProtocol
                                            {
                                                ItemsElementName = new ItemsChoiceType3[]
                                            {
                                                ItemsChoiceType3.Number,
                                                ItemsChoiceType3.Date,
                                                ItemsChoiceType3.DocumentIsNotAvailable
                                            },
                                                Items = new object[]
                                            {
                                                ownerDecision.DocumentNum,
                                                ownerDecision.ProtocolDate,
                                                true
                                            }
                                            },
                                            Item1 = new DecisionTypeFormationFundInSpecialAccount
                                            {
                                                AccountCreationDate = notification.OpenDate,
                                                AccountNumber = notification.AccountNum,
                                                BIKCredOrg = creditOrgDecision.Decision.Bik,
                                                CreditOrganization = new RegOrgType
                                                {
                                                    orgRootEntityGUID = creditOrgDecision.Decision.GisGkhOrgRootEntityGUID
                                                },
                                                AccountOpeningDocument = new AttachmentType[]
                                                {
                                                    new AttachmentType
                                                    {
                                                        Attachment = new Attachment
                                                        {
                                                            AttachmentGUID = notification.GisGkhAttachmentGuid
                                                        },
                                                        Description = notification.BankDoc.Name,
                                                        Name = notification.BankDoc.FullName,
                                                        AttachmentHASH = GetGhostHash(_fileManager.GetFile(notification.BankDoc))
                                                    }
                                                }
                                            }
                                        }
                                    });
                                }
                            }
                            else if (robjectDecision.CrFundFormationDecision.Decision == Bars.Gkh.Enums.Decisions.CrFundFormationDecisionType.RegOpAccount)
                            {
                                if (ownerDecision.File != null)
                                {
                                    importDecision.Add(new importDecisionsFormingFundRequestImportDecision
                                    {
                                        TransportGuid = ownerDecision.GisGkhTransportGuid,
                                        Item = new importDecisionsFormingFundRequestImportDecisionLoadDecision
                                        {
                                            FIASHouseGuid = GUID,
                                            DateEffective = ownerDecision.DateStart,
                                            Item = new DecisionTypeProtocol
                                            {
                                                ItemsElementName = new ItemsChoiceType3[]
                                                {
                                                    ItemsChoiceType3.Number,
                                                    ItemsChoiceType3.Date,
                                                    ItemsChoiceType3.Attachment
                                                },
                                                Items = new object[]
                                                {
                                                    ownerDecision.DocumentNum,
                                                    ownerDecision.ProtocolDate,
                                                    new AttachmentType
                                                    {
                                                        Attachment = new Attachment
                                                        {
                                                            AttachmentGUID = ownerDecision.GisGkhAttachmentGuid
                                                        },
                                                        Description = $"Протокол собрания собственников от {ownerDecision.ProtocolDate.ToShortDateString()} №{ownerDecision.DocumentNum}",
                                                        Name = ownerDecision.File.FullName,
                                                        AttachmentHASH = GetGhostHash(_fileManager.GetFile(ownerDecision.File))
                                                    }
                                                }
                                            },
                                            Item1 = true
                                        }
                                    });
                                }
                                else
                                {
                                    importDecision.Add(new importDecisionsFormingFundRequestImportDecision
                                    {
                                        TransportGuid = ownerDecision.GisGkhTransportGuid,
                                        Item = new importDecisionsFormingFundRequestImportDecisionLoadDecision
                                        {
                                            FIASHouseGuid = GUID,
                                            DateEffective = ownerDecision.DateStart,
                                            Item = new DecisionTypeProtocol
                                            {
                                                ItemsElementName = new ItemsChoiceType3[]
                                                {
                                                    ItemsChoiceType3.Number,
                                                    ItemsChoiceType3.Date,
                                                    ItemsChoiceType3.DocumentIsNotAvailable
                                                },
                                                Items = new object[]
                                                {
                                                    ownerDecision.DocumentNum,
                                                    ownerDecision.ProtocolDate,
                                                    true
                                                }
                                            },
                                            Item1 = true
                                        }
                                    });
                                }
                            }
                        }
                        finally
                        {
                            this.Container.Release(robjectDecisionService);
                        }
                    }

                    // Протокол решения органа государственной власти
                    else if (protocol.Type == CoreDecisionType.Government)
                    {
                        var govDecision = GovDecisionRepo.GetAll()
                            .Where(x => x.Id == protocol.Id).FirstOrDefault();
                        if (govDecision.ProtocolFile != null && (govDecision.GisGkhAttachmentGuid == null || govDecision.GisGkhAttachmentGuid == ""))
                        {
                            var uploadResult = _fileService.UploadFile(GisGkhLibrary.Enums.GisFileRepository.funddecisions, govDecision.ProtocolFile, OrgPPAGUID);
                            ////5 МБ = 5242880 Б 
                            //var uploaderName = govDecision.ProtocolFile.Size <= 5242880 ? "SimpleFileUploader" : "FileUploader";

                            //var uploader = Container.Resolve<IFileUploader>(uploaderName);

                            //var uploadResult = uploader.UploadFile(GisGkhLibrary.Enums.GisFileRepository.funddecisions, govDecision.ProtocolFile, OrgPPAGUID);

                            if (uploadResult.Success)
                            {
                                govDecision.GisGkhAttachmentGuid = uploadResult.FileGuid;
                                GovDecisionRepo.Update(govDecision);
                            }
                            else
                            {
                                throw new Exception(uploadResult.Message);
                            }
                        }
                        govDecision.GisGkhTransportGuid = Guid.NewGuid().ToString();
                        importDecision.Add(new importDecisionsFormingFundRequestImportDecision
                        {
                            TransportGuid = govDecision.GisGkhTransportGuid,
                            Item = new importDecisionsFormingFundRequestImportDecisionLoadDecision
                            {
                                FIASHouseGuid = GUID,
                                DateEffective = govDecision.DateStart,
                                Item = new DocumentDecisionType
                                {
                                    FullName = $"Протокол решения органа государственной власти",
                                    Number = govDecision.ProtocolNumber,
                                    Date = govDecision.ProtocolDate,
                                    Kind = "Протокол решения органа государственной власти",
                                    Attachment = govDecision.ProtocolFile != null ?
                                    new AttachmentType[] {
                                        new AttachmentType
                                        {
                                            Attachment = new Attachment
                                            {
                                                AttachmentGUID = govDecision.GisGkhAttachmentGuid
                                            },
                                            Description = $"Протокол решения органа государственной власти от {govDecision.ProtocolDate.ToShortDateString()} №{govDecision.ProtocolNumber}",
                                            Name = govDecision.ProtocolFile.FullName,
                                            AttachmentHASH = GetGhostHash(_fileManager.GetFile(govDecision.ProtocolFile))
                                        }
                                    } : null
                                },
                                Item1 = true
                            }
                        });
                    }
                    var request = HcsCapitalRepairAsync.importDecisionsFormingFundReq(importDecision.ToArray());

                    var prefixer = new XmlNsPrefixer();
                    XmlDocument document = SerializeRequest(request);
                    prefixer.Process(document);
                    SaveFile(req, GisGkhFileType.request, document, "request.xml");
                    req.RequestState = GisGkhRequestState.Formed;
                    GisGkhRequestsDomain.Update(req);
                }
                else
                {
                    throw new Exception("Отсутствует утверждённый протокол или не заполнена информация по нему");
                }
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
                var response = HcsCapitalRepairAsync.GetState(req.MessageGUID, orgPPAGUID);
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
                            req.Answer = $"Задача на обработку ответа importDecisionsFormingFund поставлена в очередь с id {taskInfo.TaskId}";
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
                            else if (responseItem is CapRemCommonResultType)
                            {
                                var ownerDecision = RealityObjectDecisionProtocolRepo.GetAll()
                                    .Where(x => x.GisGkhTransportGuid == ((CapRemCommonResultType)responseItem).TransportGUID).FirstOrDefault();
                                if (ownerDecision != null)
                                {
                                    ownerDecision.GisGkhGuid = ((CapRemCommonResultType)responseItem).GUID;
                                    RealityObjectDecisionProtocolRepo.Update(ownerDecision);
                                }
                                var govDecision = GovDecisionRepo.GetAll()
                                    .Where(x => x.GisGkhTransportGuid == ((CapRemCommonResultType)responseItem).TransportGUID).FirstOrDefault();
                                if (govDecision != null)
                                {
                                    govDecision.GisGkhGuid = ((CapRemCommonResultType)responseItem).GUID;
                                    GovDecisionRepo.Update(govDecision);
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
                    //req.RequestState = GisGkhRequestState.Error;
                    //GisGkhRequestsDomain.Update(req);
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
            return "";
            /*using (var gost = Gost3411.Create())
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

        #endregion

    }
}
