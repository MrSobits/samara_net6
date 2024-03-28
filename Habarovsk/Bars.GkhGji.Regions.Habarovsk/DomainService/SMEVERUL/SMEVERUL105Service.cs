using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Erul105;
using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
using Bars.GkhGji.Regions.Habarovsk.Entities;
using SMEV3Library.Entities.GetResponseResponse;
using SMEV3Library.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using Bars.B4.Modules.States;

namespace Bars.GkhGji.Regions.Habarovsk.DomainService
{
    public class SMEVERUL105Service : ISMEVERULService
    {

        static XNamespace ns1Namespace = @"urn://rostelekom.ru/LicenseNumber/1.0.5";

        #region Properties

        public IDomainService<SMEVERULReqNumber> SMEVERULReqNumberDomain { get; set; }
        public IDomainService<SMEVERULReqNumberFile> SMEVERULReqNumberFileDomain { get; set; }
        public IDomainService<Contragent> ContragentDomain { get; set; }
        public IDomainService<ContragentContact> ContragentContactDomain { get; set; }
        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectDomain { get; set; }
        public IRepository<Contragent> ContragentRepository { get; set; }
        public IDomainService<ManOrgLicense> ManOrgLicenseDomain { get; set; }
        public IDomainService<State> StateDomain { get; set; }
        public IRepository<ManOrgLicense> ManOrgLicenseRepository { get; set; }

        #endregion

        #region Fields

        private ISMEV3Service _SMEV3Service;

        private IFileManager _fileManager;


        #endregion

        #region Constructors

        public SMEVERUL105Service(IFileManager fileManager, ISMEV3Service SMEV3Service)
        {
            _fileManager = fileManager;
            _SMEV3Service = SMEV3Service;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Отправка запроса выписки ЕГРЮЛ
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool SendInformationRequest(SMEVERULReqNumber requestData, IProgressIndicator indicator = null)
        {
            try
            {
                //Очищаем список файлов
                indicator?.Report(null, 0, "Очистка старых файлов");
                SMEVERULReqNumberFileDomain.GetAll().Where(x => x.SMEVERULReqNumber == requestData).ToList().ForEach(x => SMEVERULReqNumberFileDomain.Delete(x.Id));

                //формируем отправляемую xml
                indicator?.Report(null, 10, "Формирование запроса");
                XElement request = GetInformationRequestXML(requestData);
                ChangeState(requestData, RequestState.Formed);

                //
                indicator?.Report(null, 20, "Отправка запроса");
                var requestResult = _SMEV3Service.SendRequestAsync(request, null, true).GetAwaiter().GetResult();
                requestData.MessageId = requestResult.MessageId;
                SMEVERULReqNumberDomain.Update(requestData);

                //
                indicator?.Report(null, 80, "Сохранение данных для отладки");
                SaveFile(requestData, requestResult.SendedData, "SendRequestRequest.dat");
                SaveFile(requestData, requestResult.ReceivedData, "SendRequestResponse.dat");

                //
                indicator?.Report(null, 90, "Обработка результата");
                if (requestResult.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {requestResult.Error}");
                    SaveException(requestData, requestResult.Error.Exception);
                }
                else if (requestResult.FaultXML != null)
                {
                    SaveFile(requestData, requestResult.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в ГИС ГМП, подробности в файле Fault.xml");
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
                    SMEVERULReqNumberDomain.Update(requestData);
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
                SetErrorState(requestData, "SendInformationRequest exception: " + e.Message);
            }

            return false;
        }

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="requestData"></param>
        /// <param name="response"></param>
        /// <param name="indicator"></param>
        /// <returns></returns>
        public bool TryProcessResponse(SMEVERULReqNumber requestData, GetResponseResponse response, IProgressIndicator indicator = null)
        {
            try
            {
                //
                indicator?.Report(null, 40, "Сохранение данных для отладки");
                SaveFile(requestData, response.SendedData, "GetResponseRequest.dat");
                SaveFile(requestData, response.ReceivedData, "GetResponseResponse.dat");
                //сохраняем все файлы, которые прислал сервер
                response.Attachments.ForEach(x => SaveFile(requestData, x.FileData, x.FileName));

                indicator?.Report(null, 70, "Обработка результата");
                if (response.Error != null)
                {
                    SetErrorState(requestData, $"Ошибка при отправке: {response.Error}");
                    SaveException(requestData, response.Error.Exception);
                    return false;
                }

                //ACK - ставим вдумчиво
                _SMEV3Service.GetAckAsync(response.MessageId, true).GetAwaiter().GetResult();

                if (response.FaultXML != null)
                {
                    SaveFile(requestData, response.FaultXML, "Fault.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в ГИС ГМП, подробности в файле Fault.xml");
                }
                //сервер вернул ошибку?
                else if (response.AsyncProcessingStatus != null)
                {
                    SaveFile(requestData, response.AsyncProcessingStatus, "AsyncProcessingStatus.xml");
                    SetErrorState(requestData, "Ошибка при обработке сообщения в ГИС ГМП, подробности в приаттаченом файле AsyncProcessingStatus.xml");
                }
                //сервер отклонил запрос?
                else if (response.RequestRejected != null)
                {
                    SaveFile(requestData, response.RequestRejected, "RequestRejected.xml");
                    SetErrorState(requestData, "Сервер отклонил запрос, подробности в приаттаченом файле RequestRejected.xml");
                }
                //ответ пустой?
                else if (response.MessagePrimaryContent == null)
                {
                    SetErrorState(requestData, "Сервер прислал ответ, в котором нет ни результата, ни ошибки обработки");
                }
                else
                {
                    //разбираем xml, которую прислал сервер
                    indicator?.Report(null, 80, "Разбор содержимого");
                    string erulNumber = string.Empty;
                    DateTime? erulDate = null;
                    try
                    {
                        var licenseNumberResponse = response.MessagePrimaryContent.Element(ns1Namespace + "permitNumberResponse");
                        if (licenseNumberResponse != null)
                        {
                            responseType newReaponce = Deserialize<responseType>(licenseNumberResponse);
                            if (newReaponce != null)
                            {
                                int i = 0;
                                foreach (var item in newReaponce.Items)
                                {
                                    var itemElement = newReaponce.ItemsElementName[i];

                                    switch (itemElement)
                                    {
                                        case ItemsChoiceType.permitNumber:
                                        {
                                            erulNumber = item as string;
                                            break;
                                        }
                                        case ItemsChoiceType.issueDate:
                                        {
                                            erulDate = item as DateTime?;
                                            break;
                                        }                                        
                                    }
                                    i++;

                                }
                                if (erulDate.HasValue && !string.IsNullOrEmpty(erulNumber))
                                {
                                    var license = ManOrgLicenseDomain.GetAll().FirstOrDefault(x => x.Id == requestData.ManOrgLicense.Id);
                                    license.ERULDate = erulDate;
                                    license.ERULNumber = erulNumber;
                                    requestData.Answer = erulNumber;
                                    ManOrgLicenseDomain.Update(license);
                                }
                                else
                                {
                                    requestData.Answer = "Ошибка распознавания " + erulNumber;
                                }
                            }
                        }

                    }
                    catch(Exception ex)
                    {
                        requestData.Answer = ex.Message;
                    }


                    requestData.RequestState = RequestState.ResponseReceived;
                    
                    SMEVERULReqNumberDomain.Update(requestData);

                    return true;
                }
            }
            catch (Exception e)
            {
                SaveException(requestData, e);
                SetErrorState(requestData, "SendInformationRequest exception: " + e.Message);
            }

            return false;
        }
        #endregion

        #region Private methods


        private XElement GetInformationRequestXML(SMEVERULReqNumber requestData)
        {
            string messageId = GuidGenerator.GenerateTimeBasedGuid(DateTime.Now).ToString();
            if (requestData.ERULRequestType == ERULRequestType.GetLicNumber)
            {
                permitNumberRequestType newRequest = new permitNumberRequestType
                {
                    permitType = new dictionaryRecordType
                    {
                        code = "001",
                        title = "Лицензирование"
                    },
                    permitInfo = new permitInfoType
                    {
                        application = new applicationType
                        {
                            applicationMethod = new dictionaryRecordType
                            {
                                code = "03",
                                title = "С использованием ВИС"
                            },
                             applicationNumberInfo = GetApplication(requestData.ManOrgLicense)
                        },
                        permissionOrganInfo = new permissionOrganInfoType
                        {
                             permissionOrganization = new dictionaryRecordType
                             {
                                 code = "01096",
                                 title = "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ"
                             },
                             region = new dictionaryRecordType
                             {
                                 code = "36",
                                 title = "Воронежская область"
                             }
                        },
                        permissionActivity = new dictionaryRecordType
                        {
                            code = "045",
                            title = "Предпринимательская деятельность по управлению многоквартирными домами"
                        },
                        validityType = new dictionaryRecordType
                        {
                            code = "01",
                            title = "Срочная"
                        },
                        recipient = new recipientPermitType
                        {
                            Item = GetOrgInfoItem(requestData.ManOrgLicense)
                        },
                        workAddresses = GetWorkAddressList(requestData.ManOrgLicense.Contragent),
                        idCard = "651aa6e9813b9f7f98e686dd"
                    }



                };

               return ToXElement<permitNumberRequestType>(newRequest);
            }
            else if (requestData.ERULRequestType == ERULRequestType.Changes)
            {
                BaseChelyabinsk.Entities.ERULchange.changePermitRequestType newRequest = new BaseChelyabinsk.Entities.ERULchange.changePermitRequestType
                {
                    permitNumber = requestData.ManOrgLicense.ERULNumber,
                    permitType = new BaseChelyabinsk.Entities.ERULchange.dictionaryRecordType
                    {
                        code = "001",
                        title = "Лицензирование"
                    },
                    changes = new BaseChelyabinsk.Entities.ERULchange.changesType
                    {
                        Item = new BaseChelyabinsk.Entities.ERULchange.permitStatusType
                        {
                            newStatus = GetStatus(requestData),
                            decision = new BaseChelyabinsk.Entities.ERULchange.decisionInfoType
                            {
                                decisionType = new BaseChelyabinsk.Entities.ERULchange.dictionaryRecordType
                                {
                                    code = "01",
                                    title = "Приказ/распоряжение"
                                },
                                decisionNumberInfo = new BaseChelyabinsk.Entities.ERULchange.numberInfoType//ToDo получать основание из доработанной карточки лицензии
                                {
                                    date = requestData.ManOrgLicense.DateTermination.HasValue ? requestData.ManOrgLicense.DateTermination.Value : requestData.ManOrgLicense.DateDisposal.Value,
                                    number = requestData.ManOrgLicense.DateTermination.HasValue ? $"63-01206-{requestData.ManOrgLicense.Id}" : requestData.ManOrgLicense.DisposalNumber
                                }
                            },
                            reason = "Смена статуса карточки лицензии в ВИС",
                            decisionOrganization = "ГОСУДАРСТВЕННАЯ ЖИЛИЩНАЯ ИНСПЕКЦИЯ ВОРОНЕЖСКОЙ ОБЛАСТИ",
                            newStatusStartDate = requestData.ManOrgLicense.DateTermination.HasValue ? requestData.ManOrgLicense.DateTermination.Value : requestData.ManOrgLicense.DateDisposal.Value,
                        }
                    }
                };

                return ToXElement<BaseChelyabinsk.Entities.ERULchange.changePermitRequestType>(newRequest);
              


            }
            else if (requestData.ERULRequestType == ERULRequestType.AdditionalInfo)
            {
                BaseChelyabinsk.Entities.ErulState102.additionalPermitRequestType newRequest = new BaseChelyabinsk.Entities.ErulState102.additionalPermitRequestType
                {
                    permitNumber = requestData.ManOrgLicense.ERULNumber,
                    permitType = new BaseChelyabinsk.Entities.ErulState102.dictionaryRecordType
                    {
                        code = "001",
                        title = "Лицензирование"
                    },
                    permitDate = requestData.ManOrgLicense.ERULDate.HasValue ? requestData.ManOrgLicense.ERULDate.Value : requestData.ManOrgLicense.DateIssued.Value,
                    validity = new BaseChelyabinsk.Entities.ErulState102.validityInfoType
                    {
                        validityType = new BaseChelyabinsk.Entities.ErulState102.dictionaryRecordType
                        {
                            code = "01",
                            title = "Срочная"
                        },
                        startDate = requestData.ManOrgLicense.DateIssued.Value,
                        expirationDate = requestData.ManOrgLicense.DateValidity.HasValue ? requestData.ManOrgLicense.DateValidity.Value : requestData.ManOrgLicense.DateIssued.Value.AddYears(5),
                        expirationDateSpecified = true,
                    },
                    decision = new BaseChelyabinsk.Entities.ErulState102.decisionInfoType
                    {
                        decisionType = new BaseChelyabinsk.Entities.ErulState102.dictionaryRecordType
                        {
                            code = "01",
                            title = "Приказ/распоряжение"
                        },
                        decisionNumberInfo = new BaseChelyabinsk.Entities.ErulState102.numberInfoType//ToDo получать основание из доработанной карточки лицензии
                        {
                            date = requestData.ManOrgLicense.DateTermination.HasValue ? requestData.ManOrgLicense.DateTermination.Value : requestData.ManOrgLicense.DateDisposal.Value,
                            number = requestData.ManOrgLicense.DateTermination.HasValue ? $"63-01206-{requestData.ManOrgLicense.Id}" : requestData.ManOrgLicense.DisposalNumber
                        },
                    },
                    decisionMaker = new BaseChelyabinsk.Entities.ErulState102.decisionMakerType
                    {
                        name = "Попова Ольга Владимировна",
                        position = new BaseChelyabinsk.Entities.ErulState102.dictionaryRecordType
                        {
                            code = "3650",
                            title = "Заместитель руководителя государственной жилищной инспекции Воронежской области - начальник отдела правового регулирования"
                        }
                    }
                };

                return ToXElement<BaseChelyabinsk.Entities.ErulState102.additionalPermitRequestType>(newRequest);
            }
            else
            {
                return null;
            }
        }

        private BaseChelyabinsk.Entities.ERULchange.workAddressType[] GetWorkAddressChangesList(Contragent contragent)
        {
            long ctId = contragent.Id;
            List<BaseChelyabinsk.Entities.ERULchange.workAddressType> wrkaddrList = new List<BaseChelyabinsk.Entities.ERULchange.workAddressType>();
            var RoList = ManOrgContractRealityObjectDomain.GetAll()
                .Where(x => x.RealityObject != null && x.ManOrgContract != null && x.ManOrgContract.ManagingOrganization != null && x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners)
                .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id == ctId && x.ManOrgContract.StartDate <= DateTime.Now && (!x.ManOrgContract.EndDate.HasValue || (x.ManOrgContract.EndDate.HasValue && x.ManOrgContract.EndDate.Value > DateTime.Now)))
                .Select(x => new
                {
                    x.RealityObject.Municipality.Name,
                    x.RealityObject.Address
                }).ToList();
            if (RoList.Count > 0)
            {
                RoList.ForEach(x =>
                {
                    wrkaddrList.Add(new BaseChelyabinsk.Entities.ERULchange.workAddressType
                    {
                        address = $"{x.Name}, {x.Address}",
                        works = GetManagingWorkChanges()
                    });
                });

            }
            else
            {
                wrkaddrList.Add(new BaseChelyabinsk.Entities.ERULchange.workAddressType
                {
                    address = !string.IsNullOrEmpty(contragent.JuridicalAddress) ? contragent.JuridicalAddress : contragent.AddressOutsideSubject,
                    works = GetManagingWorkChanges()
                });
            }
            return wrkaddrList.ToArray();

        }

        private BaseChelyabinsk.Entities.ERULchange.dictionaryRecordType[] GetManagingWorkChanges()
        {
            List<BaseChelyabinsk.Entities.ERULchange.dictionaryRecordType> wrkaddrList = new List<BaseChelyabinsk.Entities.ERULchange.dictionaryRecordType>();
            wrkaddrList.Add(new BaseChelyabinsk.Entities.ERULchange.dictionaryRecordType
            {
                code = "862",
                title = "Управление многоквартирными домами"
            });
            return wrkaddrList.ToArray();
        }

        private object GetOrgInfoChangesItem(ManOrgLicense manOrgLicense)
        {
            var opf = manOrgLicense.Contragent.OrganizationForm != null ? manOrgLicense.Contragent.OrganizationForm : null;
            List<string> opfcodesUL = new List<string> { "47", "65" };
            List<string> opfcodesIP = new List<string> { "91" };
            if (opf != null)
            {
                if (!opfcodesIP.Contains(opf.Code))
                {
                    return new ulType
                    {
                        address = !string.IsNullOrEmpty(manOrgLicense.Contragent.JuridicalAddress) ? manOrgLicense.Contragent.JuridicalAddress : manOrgLicense.Contragent.AddressOutsideSubject,
                        shortName = manOrgLicense.Contragent.ShortName,
                        email = manOrgLicense.Contragent.Email,
                        fullName = manOrgLicense.Contragent.Name,
                        inn = manOrgLicense.Contragent.Inn,
                        ogrn = manOrgLicense.Contragent.Ogrn,
                        ogrnDate = manOrgLicense.Contragent.DateRegistration.Value,
                        organizationalForm = new dictionaryRecordType
                        {
                            code = opf.OkopfCode.Replace(" ", ""),
                            title = opf.Name
                        },
                        phone = manOrgLicense.Contragent.Phone
                    };
                }
                else if (opfcodesIP.Contains(opf.Code))
                {
                    var contact = ContragentContactDomain.GetAll().FirstOrDefault(x => x.Contragent == manOrgLicense.Contragent && x.FLDocIssuedDate.HasValue);
                    if (contact == null)
                    {
                        return null;
                    }
                    return new ipType
                    {
                        address = manOrgLicense.Contragent.JuridicalAddress,
                        firstName = contact.Name,
                        lastName = contact.Surname,
                        middleName = contact.Patronymic,
                        ogrnip = manOrgLicense.Contragent.Ogrn,
                        document = new documentType
                        {
                            documentNumberInfo = new numberInfoType
                            {
                                date = contact.FLDocIssuedDate.Value,
                                number = $"{contact.FLDocSeries} {contact.FLDocNumber}"
                            },
                            issuedBy = contact.FLDocIssuedBy

                        },
                        phone = manOrgLicense.Contragent.Phone,
                        email = manOrgLicense.Contragent.Email,
                        inn = manOrgLicense.Contragent.Inn
                    };
                }
            }
            return null;

        }

        private workAddressType[] GetWorkAddressList(Contragent contragent)
        {
            long ctId = contragent.Id;
            List<workAddressType> wrkaddrList = new List<workAddressType>();
            var RoList = ManOrgContractRealityObjectDomain.GetAll()
                .Where(x => x.RealityObject != null && x.ManOrgContract != null && x.ManOrgContract.ManagingOrganization != null && x.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.ManagingOrgOwners)
                .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id == ctId && x.ManOrgContract.StartDate <= DateTime.Now && (!x.ManOrgContract.EndDate.HasValue || (x.ManOrgContract.EndDate.HasValue && x.ManOrgContract.EndDate.Value > DateTime.Now)))
                .Select(x => new
                {
                    x.RealityObject.Municipality.Name,
                    x.RealityObject.Address
                }).ToList();
            if (RoList.Count > 0)
            {
                RoList.ForEach(x =>
                {
                    wrkaddrList.Add(new workAddressType
                    {
                        address = $"{x.Name}, {x.Address}",
                        works = GetManagingWork()
                    });
                });

            }
            else if(!string.IsNullOrEmpty(contragent.JuridicalAddress))
            {
                wrkaddrList.Add(new workAddressType
                {
                    address = contragent.JuridicalAddress,
                    works = GetManagingWork()
                }) ;
            }
            else
            {
                wrkaddrList.Add(new workAddressType
                {
                    address = "Не указан",
                    works = GetManagingWork()
                });
            }
            return wrkaddrList.ToArray();

        }

        private dictionaryRecordType[] GetManagingWork()
        {
            List<dictionaryRecordType> wrkaddrList = new List<dictionaryRecordType>();
            wrkaddrList.Add(new dictionaryRecordType
            {
                code = "862",
                title = "Управление многоквартирными домами"
            });
            return wrkaddrList.ToArray();
        }

        private numberInfoType GetApplication(ManOrgLicense manOrgLicense)
        {
            return new numberInfoType
            {
                date = manOrgLicense.Request.DateRequest.Value,
                number = "36-01096-" + manOrgLicense.Request.RegisterNumber
            };
           
        }

        private object GetOrgInfoItem(ManOrgLicense manOrgLicense)
        {
            var opf = manOrgLicense.Contragent.OrganizationForm != null ? manOrgLicense.Contragent.OrganizationForm : null;
            List<string> opfcodesUL = new List<string> { "47", "65" };
            List<string> opfcodesIP= new List<string> { "91"};
            if (opf != null)
            {
                if (!opfcodesIP.Contains(opf.Code))
                {
                    return new ulType
                    {
                        address = !string.IsNullOrEmpty(manOrgLicense.Contragent.JuridicalAddress) ? manOrgLicense.Contragent.JuridicalAddress : manOrgLicense.Contragent.AddressOutsideSubject,
                        shortName = manOrgLicense.Contragent.ShortName,
                        email = manOrgLicense.Contragent.Email,
                        fullName = manOrgLicense.Contragent.Name,
                        inn = manOrgLicense.Contragent.Inn,
                        ogrn = manOrgLicense.Contragent.Ogrn,
                        ogrnDateSpecified = false,
                        organizationalForm = new dictionaryRecordType
                        {
                            code = opf.OkopfCode.Replace(" ", ""),
                            title = opf.Name
                        },
                        phone = manOrgLicense.Contragent.Phone
                    };
                }
                else if (opfcodesIP.Contains(opf.Code))
                {
                    var contact = ContragentContactDomain.GetAll().FirstOrDefault(x => x.Contragent == manOrgLicense.Contragent && x.FLDocIssuedDate.HasValue);
                    if (contact == null)
                    {
                        return null;
                    }
                    return new ipType
                    {
                        address = manOrgLicense.Contragent.JuridicalAddress,
                        firstName = contact.Name,
                        lastName = contact.Surname,
                        middleName = contact.Patronymic,
                        ogrnip = manOrgLicense.Contragent.Ogrn,
                        document = new documentType
                        {
                            documentNumberInfo = new numberInfoType
                            {
                                date = contact.FLDocIssuedDate.Value,
                                number = $"{contact.FLDocSeries} {contact.FLDocNumber}"
                            },
                            issuedBy = contact.FLDocIssuedBy
                        },
                        phone = manOrgLicense.Contragent.Phone,
                        email = manOrgLicense.Contragent.Email,
                        inn = manOrgLicense.Contragent.Inn
                    };
                }
            }
            return null;

        }




        private void ChangeState(SMEVERULReqNumber requestData, RequestState state)
        {
            requestData.RequestState = state;
            SMEVERULReqNumberDomain.Update(requestData);
        }

        private void SetErrorState(SMEVERULReqNumber requestData, string error)
        {
            requestData.RequestState = RequestState.Error;
            requestData.Answer = error;
            SMEVERULReqNumberDomain.Update(requestData);
        }

        private void SaveFile(SMEVERULReqNumber request, Stream data, string fileName)
        {
            //сохраняем ошибку
            SMEVERULReqNumberFileDomain.Save(new SMEVERULReqNumberFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVERULReqNumber = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(data, fileName)
            });
        }

        private XElement ToXElement<T>(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
                }
            }
        }

        private T Deserialize<T>(XElement element)
       where T : class
        {
            XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using (StringReader sr = new StringReader(element.ToString()))
                return (T)ser.Deserialize(sr);
        }

        private BaseChelyabinsk.Entities.ERULchange.dictionaryRecordType GetStatus(SMEVERULReqNumber req)
        {
            var state = StateDomain.Get(req.ManOrgLicense.State.Id);
            switch (state.Name)
            {
                case "Действующая":
                    return new BaseChelyabinsk.Entities.ERULchange.dictionaryRecordType
                    {
                        code = "01",
                        title = "Действующая"
                    };
                case "Прекращена":
                    return new BaseChelyabinsk.Entities.ERULchange.dictionaryRecordType
                    {
                        code = "04",
                        title = "Прекращена"
                    };

                case "Аннулирована":
                    return new BaseChelyabinsk.Entities.ERULchange.dictionaryRecordType
                    {
                        code = "05",
                        title = "Аннулирована"
                    };
                case "Дубликат":
                    return new BaseChelyabinsk.Entities.ERULchange.dictionaryRecordType
                    {
                        code = "01",
                        title = "Действующая"
                    };
                default:
                    return new BaseChelyabinsk.Entities.ERULchange.dictionaryRecordType
                    {
                        code = "01",
                        title = "Действующая"
                    };
            }
        }

        private void SaveFile(SMEVERULReqNumber request, byte[] data, string fileName)
        {
            //сохраняем отправленный пакет
            SMEVERULReqNumberFileDomain.Save(new SMEVERULReqNumberFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVERULReqNumber = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(fileName, data)
            });
        }

        private void SaveFile(SMEVERULReqNumber request, XElement data, string fileName)
        {
            MemoryStream stream = new MemoryStream();
            data.Save(stream);

            //сохраняем ошибку
            SMEVERULReqNumberFileDomain.Save(new SMEVERULReqNumberFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVERULReqNumber = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile(stream, fileName)
            });
        }

        private void SaveException(SMEVERULReqNumber request, Exception exception)
        {
            if (exception == null)
                return;

            SMEVERULReqNumberFileDomain.Save(new SMEVERULReqNumberFile
            {
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,
                ObjectVersion = 1,
                SMEVERULReqNumber = request,
                SMEVFileType = SMEVFileType.Request,
                FileInfo = _fileManager.SaveFile("Exception.txt", ($"{exception.GetType().ToString()}\n{exception.Message}\n{exception.StackTrace}").GetBytes())
            });
        }
        #endregion
    }
}
