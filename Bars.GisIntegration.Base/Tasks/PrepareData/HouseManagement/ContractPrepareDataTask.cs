namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4.Utils;
    using DataExtractors;
    using Enums;
    using PrepareData;
    using Entities.HouseManagement;
    using HouseManagementAsync;

    using Attachment = HouseManagementAsync.Attachment;

    /// <summary>
    /// Задача подготовки данных по договорам
    /// </summary>
    public class ContractPrepareDataTask : BasePrepareDataTask<importContractRequest>
    {
        private List<RisContract> contracts;
        private List<ContractObject> contractObjects;
        private List<RisContractAttachment> attachments;
        private List<RisProtocolOk> protocolOks;
        private List<ProtocolMeetingOwner> protocolsMeetingOwner;

        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 100;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var contractExtractor = this.Container.Resolve<IDataExtractor<RisContract>>("ContractDataExtractor");
            var contractObjectExtractor = this.Container.Resolve<IDataExtractor<ContractObject>>("ContractObjectDataExtractor");
            var contractAttachmentExtractor = this.Container.Resolve<IDataExtractor<RisContractAttachment>>("ContractAttachmentExtractor");
            var protocolOkExtractor = this.Container.Resolve<IDataExtractor<RisProtocolOk>>("OwnersProtocolOkExtractor");
            var ownersProtocolMeetingOwnerExtractor = this.Container.Resolve<IDataExtractor<ProtocolMeetingOwner>>("OwnersProtocolMeetingOwnerExtractor");
            var jskTsjProtocolMeetingOwnerExtractor = this.Container.Resolve<IDataExtractor<ProtocolMeetingOwner>>("JskTsjProtocolMeetingOwnerExtractor");

            try
            {
                this.contracts = this.RunExtractor(contractExtractor, parameters);
                parameters.Add("selectedContracts", this.contracts);

                this.attachments = this.RunExtractor(contractAttachmentExtractor, parameters);
                this.protocolOks = this.RunExtractor(protocolOkExtractor, parameters);
                this.protocolsMeetingOwner = this.RunExtractor(ownersProtocolMeetingOwnerExtractor, parameters);
                this.protocolsMeetingOwner.AddRange(this.RunExtractor(jskTsjProtocolMeetingOwnerExtractor, parameters));
                this.contractObjects = this.RunExtractor(contractObjectExtractor, parameters);

                parameters.Add("contractObjects", this.contractObjects);

            }
            finally
            {
                this.Container.Release(contractExtractor);
                this.Container.Release(contractObjectExtractor);
                this.Container.Release(contractAttachmentExtractor);
                this.Container.Release(protocolOkExtractor);
                this.Container.Release(ownersProtocolMeetingOwnerExtractor);
                this.Container.Release(jskTsjProtocolMeetingOwnerExtractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            result.AddRange(this.ValidateObjectList(this.contracts, this.CheckContract));
            result.AddRange(this.ValidateObjectList(this.contractObjects, this.CheckContractObject));
            result.AddRange(this.ValidateObjectList(this.attachments, this.CheckAttachment));
            result.AddRange(this.ValidateObjectList(this.protocolOks, this.CheckProtocolOk));
            result.AddRange(this.ValidateObjectList(this.protocolsMeetingOwner, this.CheckProtocolMeetingOwner));

            return result;
        }

        protected override List<ValidateObjectResult> ValidateAttachments()
        {
            var result = new List<ValidateObjectResult>();

            result.AddRange(this.ValidateObjectList(this.attachments, this.CheckAttachmentUploaded));
            result.AddRange(this.ValidateObjectList(this.protocolOks, this.CheckProtocolOkAttachments));

            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importContractRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importContractRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Проверка договора управления перед импортом
        /// </summary>
        /// <param name="contract">Договор управления</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckContract(RisContract contract)
        {
            StringBuilder messages = new StringBuilder();

            if (contract.DocNum.IsEmpty())
            {
                messages.Append("DocNum ");
            }

            if (!contract.SigningDate.HasValue)
            {
                messages.Append("SigningDate ");
            }

            if (!contract.EffectiveDate.HasValue)
            {
                messages.Append("EffectiveDate ");
            }

            if (!contract.PlanDateComptetion.HasValue)
            {
                messages.Append("PlanDateComptetion ");
            }

            if (contract.ContractBaseCode.IsEmpty())
            {
                messages.Append("ContractBase/Code ");
            }

            if (contract.ContractBaseGuid.IsEmpty())
            {
                messages.Append("ContractBase/GUID ");
            }

            if (this.contractObjects.All(x => x.Contract.Id != contract.Id) ||
                (contract.OwnersType == RisContractOwnersType.Owners && this.contractObjects.Count(x => x.Contract.Id == contract.Id) != 1))
            {
                messages.Append("ContractObject ");
            }

            if (this.attachments.All(x => x.Contract.Id != contract.Id))
            {
                messages.Append("ContractAttachment ");
            }

            if (contract.ContractBaseCode == "1" &&
                this.protocolsMeetingOwner.All(x => x.Contract.Id != contract.Id))

            // Если (ContractBase) = «Решение собрания собственников» (код записи 1), то необходимо обязательно передавать Contract/Protocol/ ProtocolAdd /ProtocolMeetingOwners или Contract/Protocol/VotingProtocolGUID.
            {
                messages.Append("ProtocolMeetingOwners ");
            }

            return new ValidateObjectResult
            {
                Id = contract.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Договор управления"
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

            return new ValidateObjectResult
            {
                Id = contractObject.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Объект управления"
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
                messages.Append("ServiceType/Code ");
            }

            if (houseManService.ServiceTypeGuid.IsEmpty())
            {
                messages.Append("ServiceType/GUID ");
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
        /// Проверка дополнительной услуги перед импортом
        /// </summary>
        /// <param name="addService">Дополнительная услуга</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckAddService(AddService addService)
        {
            StringBuilder messages = new StringBuilder();

            if (addService.ServiceTypeCode.IsEmpty())
            {
                messages.Append("ServiceType/Code ");
            }

            if (addService.ServiceTypeGuid.IsEmpty())
            {
                messages.Append("ServiceType/GUID ");
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

            return new ValidateObjectResult
            {
                Id = attachment.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Документ договора"
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
                messages.Append("Attachment.Guid ");
            }

            return new ValidateObjectResult
            {
                Id = attachment.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Документ договора"
            };
        }

        /// <summary>
        /// Проверка протокола открытого конкурса перед импортом
        /// </summary>
        /// <param name="protocolOk">Протокол открытого конкурса</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckProtocolOk(RisProtocolOk protocolOk)
        {
            StringBuilder messages = new StringBuilder();

            if (protocolOk.Attachment == null || !protocolOk.Attachment.IsValid())
            {
                messages.Append("Attachment ");
            }

            return new ValidateObjectResult
            {
                Id = protocolOk.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Протокол открытого конкурса"
            };
        }

        /// <summary>
        /// Проверка прикреплений протокола открытого конкурса перед импортом
        /// </summary>
        /// <param name="protocolOk">Протокол открытого конкурса</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckProtocolOkAttachments(RisProtocolOk protocolOk)
        {
            StringBuilder messages = new StringBuilder();

            if (protocolOk.Attachment != null && !protocolOk.Attachment.IsUploaded())
            {
                messages.Append("Attachment.Guid ");
            }

            return new ValidateObjectResult
            {
                Id = protocolOk.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Протокол открытого конкурса"
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
                messages.Append("Attachment ");
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
        /// Получает список порций объектов ГИС для формирования объектов для запроса
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<RisContract>> GetPortions()
        {
            var result = new List<IEnumerable<RisContract>>();

            if (this.contracts.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.contracts.Skip(startIndex).Take(ContractPrepareDataTask.Portion));
                    startIndex += ContractPrepareDataTask.Portion;
                }
                while (startIndex < this.contracts.Count);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса.
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект запроса</returns>
        private importContractRequest GetRequestObject(IEnumerable<RisContract> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var contractList = new List<importContractRequestContract>();
            var contractTransportGuidDictionary = new Dictionary<string, long>();
            transportGuidDictionary.Add(typeof(ContractObject), new Dictionary<string, long>());

            foreach (var contract in listForImport)
            {
                var listItem = this.GetImportContractRequestContract(contract, transportGuidDictionary);

                contractList.Add(listItem);
                contractTransportGuidDictionary.Add(listItem.TransportGUID, contract.Id);
            }

            transportGuidDictionary.Add(typeof(RisContract), contractTransportGuidDictionary);

            return new importContractRequest { Contract = contractList.ToArray(), Id = Guid.NewGuid().ToStr() };
        }

        /// <summary>
        /// Создать объект importContractRequestContract по RisContract
        /// </summary>
        /// <param name="contract">Объект типа RisContract</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект типа importContractRequestContract</returns>
        private importContractRequestContract GetImportContractRequestContract(RisContract contract, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var importContractRequest = new importContractRequestContract
            {
                TransportGUID = Guid.NewGuid().ToString(),
                Item = this.GetRequestContractItem(contract, transportGuidDictionary)
            };

            return importContractRequest;
        }

        /// <summary>
        /// Получить объект Item раздела importContractRequestContract
        /// </summary>
        /// <param name="contract">Договор управления</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект Item</returns>
        private object GetRequestContractItem(RisContract contract, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            object item = null;

            if (contract.Operation == RisEntityOperation.Create)
            {
                item = this.GetPlacingContract(contract, transportGuidDictionary);

            }
            else if (contract.Operation == RisEntityOperation.Update)
            {
                item = this.GetEditContract(contract, transportGuidDictionary);
            }
            else if (contract.Operation == RisEntityOperation.Delete)
            {
                item = new importContractRequestContractAnnulmentContract
                {
                    ReasonOfAnnulment = "Договор управления был удален",
                    ContractVersionGUID = contract.Guid
                };
            }

            return item;
        }

        /// <summary>
        /// Получить PlacingContract
        /// </summary>
        /// <param name="contract">Договор управления</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>PlacingContract</returns>
        private importContractRequestContractPlacingContract GetPlacingContract(RisContract contract, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            return new importContractRequestContractPlacingContract
            {
                DocNum = contract.DocNum,
                SigningDate = contract.SigningDate ?? DateTime.MinValue,
                EffectiveDate = contract.EffectiveDate ?? DateTime.MinValue,
                PlanDateComptetion = contract.PlanDateComptetion ?? DateTime.MinValue,
                Item = this.GetPlacingContractItem(contract),
                ItemElementName = this.GetPlacingContractItemName(contract.OwnersType),
                Protocol = new ContractTypeProtocol
                {
                    Items = this.GetProtocolObjects(contract.Id)
                },

                ContractBase = new nsiRef
                {
                    Code = contract.ContractBaseCode,
                    GUID = contract.ContractBaseGuid
                },
                DateDetails = this.GetDateDetails(contract),
                ContractAttachment = this.GetContractAttachments(contract.Id),
                LicenseRequest = true,
                ContractObject = this.GetContractObjects(contract.Id, transportGuidDictionary),
            };
        }

        /// <summary>
        /// Получить объект Item раздела importContractRequestContractPlacingContract
        /// </summary>
        /// <param name="contract">Договор управления</param>
        /// <returns>Объект Item</returns>
        private object GetPlacingContractItem(RisContract contract)
        {
            object item = null;

            switch (contract.OwnersType)
            {
                case RisContractOwnersType.BuildingOwner:
                    item = new RegOrgType
                    {
                        orgRootEntityGUID = contract.Org.ReturnSafe(x => x.OrgRootEntityGuid)
                    };
                    break;
                case RisContractOwnersType.Cooperative:
                    item = new RegOrgType
                    {
                        orgRootEntityGUID = contract.Org.ReturnSafe(x => x.OrgRootEntityGuid)
                    };
                    break;
                case RisContractOwnersType.MunicipalHousing:
                    item = new RegOrgType
                    {
                        orgRootEntityGUID = contract.Org.ReturnSafe(x => x.OrgRootEntityGuid)
                    };
                    break;
                case RisContractOwnersType.Owners:
                    item = true;
                    break;
            }

            return item;
        }

        /// <summary>
        /// Получить ItemName раздела importContractRequestContractPlacingContract
        /// </summary>
        /// <param name="contractType">Тип владельца договора</param>
        /// <returns>ItemName</returns>
        private ItemChoiceType3 GetPlacingContractItemName(RisContractOwnersType? contractType)
        {
            ItemChoiceType3 itemName = ItemChoiceType3.Owners;

            switch (contractType)
            {
                case RisContractOwnersType.BuildingOwner:
                    itemName = ItemChoiceType3.BuildingOwner;
                    break;
                case RisContractOwnersType.Cooperative:
                    itemName = ItemChoiceType3.Cooperative;
                    break;
                case RisContractOwnersType.MunicipalHousing:
                    itemName = ItemChoiceType3.MunicipalHousing;
                    break;
            }

            return itemName;
        }

        /// <summary>
        /// Получить ContractObject для PlacingContract
        /// </summary>
        /// <param name="contractId">Идентификатор контракта</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>ContractObject</returns>
        private importContractRequestContractPlacingContractContractObject[] GetContractObjects(long contractId, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var result = new List<importContractRequestContractPlacingContractContractObject>();
            var contractObjectsList = this.contractObjects.Where(x => x.Contract.Id == contractId).ToList();

            if (contractObjectsList.Count > 0)
            {
                foreach (var contractObject in contractObjectsList)
                {
                    var transportGuid = Guid.NewGuid().ToString();
                    transportGuidDictionary[typeof(ContractObject)].Add(transportGuid, contractObject.Id);

                    result.Add(new importContractRequestContractPlacingContractContractObject
                    {
                        FIASHouseGuid = contractObject.FiasHouseGuid,
                        StartDate = contractObject.StartDate ?? DateTime.MinValue,
                        EndDate = contractObject.EndDate.GetValueOrDefault(),
                        EndDateSpecified = contractObject.EndDate.HasValue,
                        TransportGUID = transportGuid,
                        BaseMService = new BaseServiceType
                        {
                            Item = true
                        }
                    });
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получить содержимое раздела Protocol
        /// </summary>
        /// <param name="contractId">Идентификатор договора</param>
        /// <returns>Содержимое раздела Protocol</returns>
        private object[] GetProtocolObjects(long contractId)
        {
            var items = new List<object>();

            var protocolOKs = this.GetProtocolOKs(contractId);
            var protocolMeetingOwners = this.GetProtocolMeetingOwners(contractId);

            if (protocolOKs.Length > 0)
            {
                items.Add(new ContractTypeProtocolProtocolAdd
                {
                    Items = this.GetProtocolOKs(contractId),
                    ItemsElementName = new[] { ItemsChoiceType6.ProtocolOK }
                });
            }

            if (protocolMeetingOwners.Length > 0)
            {
                items.Add(new ContractTypeProtocolProtocolAdd
                {
                    Items = this.GetProtocolMeetingOwners(contractId),
                    ItemsElementName = new[] { ItemsChoiceType6.ProtocolMeetingOwners }
                });
            }

            return items.ToArray();
        }

        /// <summary>
        /// Сформировать массив приложений контракта
        /// </summary>
        /// <param name="contractId">Иднетификатор объекта контракта</param>
        /// <returns>Массив приложений контракта</returns>
        private AttachmentType[] GetContractAttachments(long contractId)
        {
            var result = new List<AttachmentType>();
            var attachmentsList = this.attachments.Where(x => x.Contract.Id == contractId).ToList();

            if (attachmentsList.Count > 0)
            {
                foreach (var attachment in attachmentsList)
                {
                    result.Add(new AttachmentType
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

            return result.ToArray();
        }

        /// <summary>
        /// Сформировать массив протоколов контракта
        /// </summary>
        /// <param name="contractId">Иднетификатор объекта контракта</param>
        /// <returns>Массив протоколов контракта</returns>
        private object[] GetProtocolOKs(long contractId)
        {
            var result = new List<object>();
            var listProtocolOk = this.protocolOks.Where(x => x.Contract.Id == contractId).ToList();

            if (listProtocolOk.Count > 0)
            {
                foreach (var attachment in listProtocolOk)
                {
                    result.Add(new AttachmentType
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

            return result.ToArray();
        }

        /// <summary>
        /// Сформировать массив протоколов контракта
        /// </summary>
        /// <param name="contractId">Иднетификатор объекта контракта</param>
        /// <returns>Массив протоколов контракта</returns>
        private object[] GetProtocolMeetingOwners(long contractId)
        {
            var result = new List<object>();
            var listProtocolMeetingOwner = this.protocolsMeetingOwner.Where(x => x.Contract.Id == contractId).ToList();

            if (listProtocolMeetingOwner.Count > 0)
            {
                foreach (var attachment in listProtocolMeetingOwner)
                {
                    result.Add(new AttachmentType
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

            return result.ToArray();
        }

        /// <summary>
        /// Получить EditContract
        /// </summary>
        /// <param name="contract">Договор управления</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>EditContract</returns>
        private importContractRequestContractEditContract GetEditContract(RisContract contract, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            return new importContractRequestContractEditContract
            {
                DocNum = contract.DocNum,
                SigningDate = contract.SigningDate ?? DateTime.MinValue,
                EffectiveDate = contract.EffectiveDate ?? DateTime.MinValue,
                PlanDateComptetion = contract.PlanDateComptetion ?? DateTime.MinValue,
                Item = this.GetPlacingContractItem(contract),
                ItemElementName = this.GetPlacingContractItemName(contract.OwnersType),
                Protocol = new ContractTypeProtocol
                {
                    Items = this.GetProtocolObjects(contract.Id)
                },
                ContractBase = new nsiRef
                {
                    Code = contract.ContractBaseCode,
                    GUID = contract.ContractBaseGuid
                },
                DateDetails = this.GetDateDetails(contract),
                ContractAttachment = this.GetContractAttachments(contract.Id),
                ContractObject = this.GetEditContractObjects(contract.Id, transportGuidDictionary),
                ContractVersionGUID = contract.Guid
            };
        }

        /// <summary>
        /// Получить DateDetails
        /// </summary>
        /// <param name="contract">Договор управления</param>
        /// <returns>DateDetails(</returns>
        private DateDetailsType GetDateDetails(RisContract contract)
        {
            return new DateDetailsType
            {
                PeriodMetering = new DateDetailsTypePeriodMetering
                {
                    StartDate = new DeviceMeteringsDaySelectionType
                    {
                        Item = (sbyte)(contract.InputMeteringDeviceValuesBeginDate ?? 0),
                        IsNextMonth = !contract.PeriodMeteringStartDateThisMonth ?? false
                    },
                    EndDate = new DeviceMeteringsDaySelectionType
                    {
                        Item = (sbyte)(contract.InputMeteringDeviceValuesEndDate ?? 0),
                        IsNextMonth = !contract.PeriodMeteringEndDateThisMonth ?? false
                    }
                },
                PaymentDocumentInterval = new DateDetailsTypePaymentDocumentInterval
                {
                    Item = (sbyte)(contract.DrawingPaymentDocumentDate ?? 0),
                    Item1 = true,
                    Item1ElementName = contract.ThisMonthPaymentDocDate ? Item1ChoiceType.CurrentMounth : Item1ChoiceType.NextMounth
                },
                PaymentInterval = new DateDetailsTypePaymentInterval
                {
                    Item = (sbyte)(contract.PaymentServicePeriodDate ?? 0),
                    Item1 = true,
                    Item1ElementName = (contract.ThisMonthPaymentServiceDate ?? false) ? Item1ChoiceType1.CurrentMounth : Item1ChoiceType1.NextMounth
                }
            };
        }

        /// <summary>
        /// Получить ContractObject для PlacingContract
        /// </summary>
        /// <param name="contractId">Идентификатор контракта</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>ContractObject</returns>
        private importContractRequestContractEditContractContractObject[] GetEditContractObjects(long contractId, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var result = new List<importContractRequestContractEditContractContractObject>();
            var contractObjectsList = this.contractObjects.Where(x => x.Contract.Id == contractId).ToList();

            if (contractObjectsList.Count > 0)
            {
                foreach (var contractObject in contractObjectsList)
                {
                    var transportGuid = Guid.NewGuid().ToString();
                    transportGuidDictionary[typeof(ContractObject)].Add(transportGuid, contractObject.Id);

                    result.Add(new importContractRequestContractEditContractContractObject
                    {
                        Item = this.EditContractContractObjectItem(contractObject),
                        TransportGUID = transportGuid
                    });
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получить Item для EditContractObject
        /// </summary>
        /// <param name="contractObject">Объект управления</param>
        /// <returns>Item для EditContractObject</returns>
        private object EditContractContractObjectItem(ContractObject contractObject)
        {
            object result = null;

            if (contractObject.Operation == RisEntityOperation.Create)
            {
                result = new importContractRequestContractEditContractContractObjectAdd
                {
                    FIASHouseGuid = contractObject.FiasHouseGuid,
                    StartDate = contractObject.StartDate ?? DateTime.MinValue,
                    EndDate = contractObject.EndDate ?? DateTime.MinValue,
                    EndDateSpecified = contractObject.EndDate.HasValue,
                    BaseMService = new BaseServiceType
                    {
                        Item = true
                    }
                };
            }
            else if (contractObject.Operation == RisEntityOperation.Update)
            {
                result = new importContractRequestContractEditContractContractObjectEdit
                {
                    FIASHouseGuid = contractObject.FiasHouseGuid,
                    StartDate = contractObject.StartDate ?? DateTime.MinValue,
                    EndDate = contractObject.EndDate ?? DateTime.MinValue,
                    EndDateSpecified = contractObject.EndDate.HasValue,
                    BaseMService = new BaseServiceType
                    {
                        Item = true
                    },
                    ContractObjectVersionGUID = contractObject.Guid
                };
            }
            else if (contractObject.Operation == RisEntityOperation.Delete)
            {
                result = new importContractRequestContractEditContractContractObjectAnnulment
                {
                    ContractObjectVersionGUID = contractObject.Guid
                };
            }

            return result;
        }
    }
}
