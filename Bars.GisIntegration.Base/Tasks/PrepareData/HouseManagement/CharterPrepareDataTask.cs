namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Castle.Core.Internal;
    using Entities.HouseManagement;
    using HouseManagementAsync;
    using Attachment = HouseManagementAsync.Attachment;
    using Gkh.Quartz.Scheduler.Log;

    /// <summary>
    /// Задача подготовки данных по договорам
    /// </summary>
    public class CharterPrepareDataTask : BasePrepareDataTask<importCharterRequest>
    {
        private List<Charter> charters;
        private List<ProtocolMeetingOwner> protocolsMeetingOwners;
        private List<RisContractAttachment> charterAttachments;
        private List<ContractObject> contractObjects;


        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 1;
        private object exception;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var charterExtractor = this.Container.Resolve<IDataExtractor<Charter>>("CharterExtractor");
            var contractAttachmentExtractor = this.Container.Resolve<IDataExtractor<RisContractAttachment>>("ContractAttachmentExtractor");
            var contractObjectDataExtractor = this.Container.Resolve<IDataExtractor<ContractObject>>("ContractObjectDataExtractor");
            var protocolMeetingOwnersExtractor = this.Container.Resolve<IDataExtractor<ProtocolMeetingOwner>>("JskTsjProtocolMeetingOwnerExtractor");

            try
            {
                this.charters = this.RunExtractor(charterExtractor, parameters);
                parameters.Add("selectedCharters", this.charters);

                this.protocolsMeetingOwners = this.RunExtractor(protocolMeetingOwnersExtractor, parameters);
                this.charterAttachments = this.RunExtractor(contractAttachmentExtractor, parameters);
                this.contractObjects = this.RunExtractor(contractObjectDataExtractor, parameters);

                parameters.Add("contractObjects", this.contractObjects);
            }
            finally
            {
                this.Container.Release(charterExtractor);
                this.Container.Release(protocolMeetingOwnersExtractor);
                this.Container.Release(contractAttachmentExtractor);
                this.Container.Release(contractObjectDataExtractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            result.AddRange(this.ValidateObjectList(this.charters, this.CheckCharter));
            result.AddRange(this.ValidateObjectList(this.contractObjects, this.CheckContractObject));
            result.AddRange(this.ValidateObjectList(this.charterAttachments, this.CheckAttachment));
            result.AddRange(this.ValidateObjectList(this.protocolsMeetingOwners, this.CheckProtocolMeetingOwner));

            return result;
        }

        protected override List<ValidateObjectResult> ValidateAttachments()
        {
            var result = new List<ValidateObjectResult>();

            result.AddRange(this.ValidateObjectList(this.charterAttachments, this.CheckAttachmentUploaded));
            result.AddRange(this.ValidateObjectList(this.protocolsMeetingOwners, this.CheckProtocolMeetingOwnerAttachments));

            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importCharterRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importCharterRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);
                if (request != null)
                {
                    request.Id = Guid.NewGuid().ToString();
                    result.Add(request, transportGuidDictionary);
                }
            }
            return result;
        }

        /// <summary>
        /// Проверка дополнительной услуги перед импортом
        /// </summary>
        /// <param name="addService">Дополнительная услуга</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckAddService(AddService addService)
        {
            StringBuilder messages = new StringBuilder();

            if (addService.ServiceTypeCode.IsEmpty())
            {
                messages.Append("ServiceType/Code");
            }

            if (addService.ServiceTypeGuid.IsEmpty())
            {
                messages.Append("ServiceType/GUID");
            }

            return new ValidateObjectResult
            {
                Id = addService.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Дополнительная услуга"
            };
        }

        /// <summary>
        /// Проверка коммунальной услуги перед импортом
        /// </summary>
        /// <param name="houseManService">Коммунальная услуга</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckHouseManService(HouseManService houseManService)
        {
            StringBuilder messages = new StringBuilder();

            if (houseManService.ServiceTypeCode.IsEmpty())
            {
                messages.Append("ServiceType/Code");
            }

            if (houseManService.ServiceTypeGuid.IsEmpty())
            {
                messages.Append("ServiceType/GUID");
            }

            return new ValidateObjectResult
            {
                Id = houseManService.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Коммунальная услуга"
            };
        }

        /// <summary>
        /// Проверка протокола собрания собственников перед импортом
        /// </summary>
        /// <param name="protocolMeetingOwner">Протокол собрания собственников</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckProtocolMeetingOwner(ProtocolMeetingOwner protocolMeetingOwner)
        {
            StringBuilder messages = new StringBuilder();

            if (protocolMeetingOwner.Attachment == null || !protocolMeetingOwner.Attachment.IsValid())
            {
                messages.Append("Attachment");
            }

            return new ValidateObjectResult
            {
                Id = protocolMeetingOwner.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Протокол собрания собственников"
            };
        }

        /// <summary>
        /// Проверка прикреплений протокола собрания собственников перед импортом
        /// </summary>
        /// <param name="protocolMeetingOwner">Протокол собрания собственников</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckProtocolMeetingOwnerAttachments(ProtocolMeetingOwner protocolMeetingOwner)
        {
            StringBuilder messages = new StringBuilder();

            if (protocolMeetingOwner.Attachment != null && !protocolMeetingOwner.Attachment.IsUploaded())
            {
                messages.Append("Attachment.Guid");
            }

            return new ValidateObjectResult
            {
                Id = protocolMeetingOwner.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Протокол собрания собственников"
            };
        }

        /// <summary>
        /// Проверка документа договора перед импортом
        /// </summary>
        /// <param name="attachment">Документ договора</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckAttachment(RisContractAttachment attachment)
        {
            StringBuilder messages = new StringBuilder();

            if (attachment.Attachment == null || !attachment.Attachment.IsValid())
            {
                messages.Append("Attachment ");
            }

            if (attachment.Attachment != null && attachment.Attachment.Name.IsNullOrEmpty())
            {
                messages.Append("Attachment description");
            }

            return new ValidateObjectResult
            {
                Id = attachment.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Документ устава"
            };
        }

        /// <summary>
        /// Проверка загруженности документа договора перед импортом
        /// </summary>
        /// <param name="attachment">Документ договора</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckAttachmentUploaded(RisContractAttachment attachment)
        {
            StringBuilder messages = new StringBuilder();

            if (attachment.Attachment != null && !attachment.Attachment.IsUploaded())
            {
                messages.Append("Attachment.Guid");
            }

            return new ValidateObjectResult
            {
                Id = attachment.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Документ устава"
            };
        }

        /// <summary>
        /// Проверка объекта управления перед импортом
        /// </summary>
        /// <param name="contractObject">Объект управления</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckContractObject(ContractObject contractObject)
        {
            StringBuilder messages = new StringBuilder();

            if (contractObject.FiasHouseGuid.IsEmpty())
            {
                messages.Append("FiasHouseGUID ");
            }

            if (contractObject.StartDate == null)
            {
                messages.Append("StartDate ");
            }

            return new ValidateObjectResult
            {
                Id = contractObject.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Объект управления"
            };
        }

        /// <summary>
        /// Проверка устава перед импортом
        /// </summary>
        /// <param name="charter">Устав</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckCharter(Charter charter)
        {
            StringBuilder messages = new StringBuilder();

            if (charter.DocNum.IsEmpty())
            {
                messages.Append("DocNum ");
            }

            if (!charter.DocDate.HasValue)
            {
                messages.Append("DocDate ");
            }

            if (this.protocolsMeetingOwners.All(x => x.Charter.Id != charter.Id))
            {
                messages.Append("ProtocolMeetingOwners ");
            }

            if (this.contractObjects.All(x => x.Charter.Id != charter.Id))
            {
                messages.Append("ContractObject ");
            }

            if (this.charterAttachments.All(x => x.Charter.Id != charter.Id))
            {
                messages.Append("AttachmentCharter ");
            }

            return new ValidateObjectResult
            {
                Id = charter.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Устав"
            };
        }

        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса.
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<Charter>> GetPortions()
        {
            var result = new List<IEnumerable<Charter>>();

            if (this.charters.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.charters.Skip(startIndex).Take(CharterPrepareDataTask.Portion));
                    startIndex += CharterPrepareDataTask.Portion;
                }
                while (startIndex < this.charters.Count);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Список объектов для импорта</param>
        /// <returns>Объект запроса</returns>
        private importCharterRequest GetRequestObject(IEnumerable<Charter> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var charter = listForImport.First();
            var transportGuid = Guid.NewGuid().ToString();

            var charterTransportGuidDictionary = new Dictionary<string, long>
            {
                { transportGuid, charter.Id }
            };

            transportGuidDictionary.Add(typeof(Charter), charterTransportGuidDictionary);
            transportGuidDictionary.Add(typeof(ContractObject), new Dictionary<string, long>());

            var importCharterRequest = new importCharterRequest
            {
                Item = this.GetImportCharterRequestItem(charter, transportGuidDictionary),
                TransportGUID = transportGuid
            };
            if (importCharterRequest.Item == null)
                return null;

            return importCharterRequest;
        }

        /// <summary>
        /// Получить Item раздела importCharterRequest
        /// </summary>
        /// <param name="charter">Устав</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Item раздела importCharterRequest</returns>
        private object GetImportCharterRequestItem(Charter charter, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            object result = null;

            if (charter.Operation == RisEntityOperation.Create)
            {
                result = this.GetImportCharterCreateRequestContract(charter, transportGuidDictionary);
            }
            else if (charter.Operation == RisEntityOperation.Update)
            {
                result = this.GetImportCharterEditRequestContract(charter, transportGuidDictionary);
            }
            else if (charter.Operation == RisEntityOperation.Delete)
            {
                result = new importCharterRequestAnnulmentCharter
                {
                    ReasonOfAnnulment = "Устав управления был удален",
                    CharterVersionGUID = charter.Guid
                };
            }

            return result;
        }

        /// <summary>
        /// Получить DateDetails
        /// </summary>
        /// <param name="charter">Устав</param>
        /// <returns>DateDetails(</returns>
        private CharterDateDetailsType GetDateDetails(Charter charter)
        {
            bool thisMonthPaymentDocDate = charter.ThisMonthPaymentDocDate ?? false;
            bool thisMonthPaymentServiceDate = charter.ThisMonthPaymentServiceDate ?? false;

            return new CharterDateDetailsType
            {
                PeriodMetering = new CharterDateDetailsTypePeriodMetering
                {
                    StartDate = new DeviceMeteringsDaySelectionType
                    {
                        Item = (sbyte)(charter.PeriodMeteringStartDate ?? 0),
                        IsNextMonth = charter.PeriodMeteringStartDateThisMonth ?? false

                    },
                    EndDate = new DeviceMeteringsDaySelectionType
                    {
                        Item = (sbyte)(charter.PeriodMeteringEndDate ?? 0),
                        IsNextMonth = charter.PeriodMeteringEndDateThisMonth ?? false
                    }
                },
                PaymentDocumentInterval = new CharterDateDetailsTypePaymentDocumentInterval
                {
                    Item = (sbyte)(charter.PaymentDateStartDate ?? 0),
                    Item1 = true,
                    Item1ElementName = thisMonthPaymentDocDate ? Item1ChoiceType2.CurrentMounth : Item1ChoiceType2.NextMounth
                },
                PaymentInterval = new CharterDateDetailsTypePaymentInterval
                {
                    Item = (sbyte)(charter.PaymentServicePeriodDate ?? 0),
                    Item1 = true,
                    Item1ElementName = thisMonthPaymentServiceDate ? Item1ChoiceType3.CurrentMounth : Item1ChoiceType3.NextMounth
                }
            };
        }

        /// <summary>
        /// Создать объект importCharterRequestPlacingCharter
        /// </summary>
        /// <param name="charter">Объект типа Charter</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект типа importCharterRequestPlacingCharter</returns>
        private object GetImportCharterCreateRequestContract(Charter charter, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var attachmentCharter = this.GetAttachments(charter.Id);
            var contractObject = this.GetContractObjectsCreate(charter, transportGuidDictionary);

            if (attachmentCharter.Length!= 0  && contractObject.Length != 0)
            {
                return new importCharterRequestPlacingCharter
                {
                    Date = charter.DocDate ?? DateTime.MinValue,
                    DateDetails = this.GetDateDetails(charter),
                    MeetingProtocol = new CharterTypeMeetingProtocol
                    {
                        Items = this.GetProtocolMeetingOwners(charter.Id)
                    },

                    //ChiefExecutive = new CharterTypeChiefExecutive
                    //{
                    //    Head = new CharterTypeChiefExecutiveHead
                    //    {
                    //        Item = new RegOrgType
                    //        {
                    //            orgRootEntityGUID = this.Contragent.OrgRootEntityGuid
                    //        },
                    //        ItemElementName = ItemChoiceType8.Org
                    //    },
                    //    Managers = charter.Managers
                    //},
                    AttachmentCharter = attachmentCharter,
                    ContractObject = contractObject
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Получить массив объектов ContractObject по заданному charterId для раздела PlacingCharter
        /// </summary>
        /// <param name="charter">Устав</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Массив объектов ContractObject</returns>
        private importCharterRequestPlacingCharterContractObject[] GetContractObjectsCreate(Charter charter, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var result = new List<importCharterRequestPlacingCharterContractObject>();

            foreach (var contractObject in this.contractObjects.Where(x => x.Charter.Id == charter.Id))
            {
                var transportGuid = Guid.NewGuid().ToString();
                transportGuidDictionary[typeof(ContractObject)].Add(transportGuid, contractObject.Id);

                result.Add(new importCharterRequestPlacingCharterContractObject
                {
                    FIASHouseGuid = contractObject.FiasHouseGuid,
                    StartDate = contractObject.StartDate ?? DateTime.MinValue,
                    EndDate = contractObject.EndDate ?? DateTime.MinValue,
                    EndDateSpecified = contractObject.EndDate != null,
                    TransportGUID = transportGuid,
                    BaseMService = new BaseServiceCharterType
                    {
                        Item = true
                    },
                    IsManagedByContract = charter.IsManagedByContract,
                    IsManagedByContractSpecified = charter.IsManagedByContract
                });
            }

            return result.ToArray();
        }

        /// <summary>
        /// Создать объект importCharterRequestEditCharter
        /// </summary>
        /// <param name="charter">Объект типа Charter</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект типа importCharterRequestEditCharter</returns>
        private importCharterRequestEditCharter GetImportCharterEditRequestContract(Charter charter, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var attachmentCharter = this.GetAttachments(charter.Id);
            var meetingProtocol = this.GetProtocolMeetingOwners(charter.Id);

            if (meetingProtocol.Length != 0 && attachmentCharter.Length != 0)
            {
                return new importCharterRequestEditCharter
                {
                    Date = charter.DocDate ?? DateTime.MinValue,
                    DateDetails = this.GetDateDetails(charter),
                    MeetingProtocol = new CharterTypeMeetingProtocol
                    {
                        Items = meetingProtocol
                    },
                    //ChiefExecutive = new CharterTypeChiefExecutive
                    //{
                    //    Head = new CharterTypeChiefExecutiveHead
                    //    {
                    //        Item = new RegOrgType
                    //        {
                    //            orgRootEntityGUID = this.Contragent.OrgRootEntityGuid
                    //        },
                    //        ItemElementName = ItemChoiceType8.Org
                    //    },
                    //    Managers = charter.Managers
                    //},
                    ContractObject = this.GetContractObjectsEdit(charter.Id, transportGuidDictionary),
                    AttachmentCharter = attachmentCharter,
                    CharterVersionGUID = charter.Guid
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Сформировать массив приложений устава
        /// </summary>
        /// <param name="charterId">Иднетификатор объекта устава</param>
        /// <returns>Массив приложений контракта</returns>
        private AttachmentType[] GetAttachments(long charterId)
        {
            var result = new List<AttachmentType>();
            var attachmentsList = this.charterAttachments.Where(x => x.Charter.Id == charterId).ToList();

            if (attachmentsList.Count > 0)
            {
                foreach (var attachment in attachmentsList)
                {
                    result.Add(
                        new AttachmentType
                        {
                            Name = attachment.Attachment.Name,
                            Description = attachment.Attachment.Description.IsEmpty() ? attachment.Attachment.Name : attachment.Attachment.Description,
                            Attachment = new Attachment
                            {
                                AttachmentGUID = attachment.Attachment.Guid
                            },
                            AttachmentHASH = attachment.Attachment.Hash
                        });
                }
            }
            else
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Нет вложений документа устава id " + charterId));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получить массив объектов ContractObject по заданному charterId для раздела EditCharter
        /// </summary>
        /// <param name="charterId">Идентификатор устава</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Массив объектов ContractObject</returns>
        private importCharterRequestEditCharterContractObject[] GetContractObjectsEdit(long charterId, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            List<importCharterRequestEditCharterContractObject> result = new List<importCharterRequestEditCharterContractObject>();

            foreach (var contractObject in this.contractObjects.Where(x => x.Charter.Id == charterId))
            {
                var transportGuid = Guid.NewGuid().ToString();
                transportGuidDictionary[typeof(ContractObject)].Add(transportGuid, contractObject.Id);

                result.Add(new importCharterRequestEditCharterContractObject
                {
                    Item = this.GetEditCharterContractObject(contractObject),
                    TransportGUID = transportGuid
                });
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получить Item раздела importCharterRequest
        /// </summary>
        /// <param name="contractObject">объект устава</param>
        /// <returns>Item раздела importCharterRequest</returns>
        private object GetEditCharterContractObject(ContractObject contractObject)
        {
            object result = null;

            if (contractObject.Operation == RisEntityOperation.Create)
            {
                result = new importCharterRequestEditCharterContractObjectAdd
                {
                    FIASHouseGuid = contractObject.FiasHouseGuid,
                    StartDate = contractObject.StartDate ?? DateTime.MinValue,
                    EndDate = contractObject.EndDate ?? DateTime.MinValue,
                    EndDateSpecified = contractObject.EndDate.HasValue,
                    BaseMService = new BaseServiceCharterType
                    {
                        Item = true
                    }
                };
            }
            else if (contractObject.Operation == RisEntityOperation.Update)
            {
                result = new importCharterRequestEditCharterContractObjectEdit
                {
                    FIASHouseGuid = contractObject.FiasHouseGuid,
                    StartDate = contractObject.StartDate ?? DateTime.MinValue,
                    EndDate = contractObject.EndDate ?? DateTime.MinValue,
                    EndDateSpecified = contractObject.EndDate.HasValue,
                    BaseMService = new BaseServiceCharterType
                    {
                        Item = true
                    },
                    ContractObjectVersionGUID = contractObject.Guid
                };
            }
            else if (contractObject.Operation == RisEntityOperation.Delete)
            {
                result = new importCharterRequestEditCharterContractObjectAnnulment
                {
                    ContractObjectVersionGUID = contractObject.Guid
                };
            }

            return result;
        }
        
        /// <summary>
        /// Сформировать массив протоколов контракта
        /// </summary>
        /// <param name="charterId">Иднетификатор объекта устава</param>
        /// <returns>Массив протоколов контракта</returns>
        private object[] GetProtocolMeetingOwners(long charterId)
        {
            var result = new List<object>();

            foreach (var protocol in this.protocolsMeetingOwners.Where(x => x.Charter.Id == charterId))
            {
                result.Add(new AttachmentType
                {
                    Name = protocol.Attachment.FileInfo.Name,
                    Description = protocol.Attachment.Description.IsEmpty() ? protocol.Attachment.Name : protocol.Attachment.Description,
                    Attachment = new Attachment
                    {
                        AttachmentGUID = protocol.Attachment.Guid
                    },
                    AttachmentHASH = protocol.Attachment.Hash
                });
            }
            return result.ToArray();
        }
    }
}