namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Entities.HouseManagement;
    using HouseManagementAsync;
    using Attachment = HouseManagementAsync.Attachment;

    /// <summary>
    /// Задача подготовки данных
    /// </summary>
    public class PublicPropertyContractPrepareDataTask : BasePrepareDataTask<importPublicPropertyContractRequest>
    {
        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 100;

        private List<RisPublicPropertyContract> contracts;
        private List<RisContractAttachment> contractAttachment;
        private List<RisTrustDocAttachment> trustDocAttachment;
        private Dictionary<long, List<RisContractAttachment>> contractAttachmentByContractIdMap;
        private Dictionary<long, List<RisTrustDocAttachment>> trustDocAttachmentByContractIdMap;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var publicPropertyContractExtractor = this.Container.Resolve<IDataExtractor<RisPublicPropertyContract>>("PublicPropertyContractExtractor");
            var contractAttachmentExtractor = this.Container.Resolve<IDataExtractor<RisContractAttachment>>("PublicPropertyContractAttachmentExtractor");
            var trustDocAttachmentExtractor = this.Container.Resolve<IDataExtractor<RisTrustDocAttachment>>("TrustDocAttachmentExtractor");

            try
            {
                this.contracts = this.RunExtractor(publicPropertyContractExtractor, parameters);

                parameters.Add("selectedContracts", this.contracts);

                this.contractAttachment = this.RunExtractor(contractAttachmentExtractor, parameters);
                this.trustDocAttachment = this.RunExtractor(trustDocAttachmentExtractor, parameters);

                this.contractAttachmentByContractIdMap = this.contractAttachment.GroupBy(x => x.PublicPropertyContract.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());
                this.trustDocAttachmentByContractIdMap = this.trustDocAttachment.GroupBy(x => x.PublicPropertyContract.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());
            }
            finally
            {
                this.Container.Release(publicPropertyContractExtractor);
                this.Container.Release(contractAttachmentExtractor);
                this.Container.Release(trustDocAttachmentExtractor);
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
            result.AddRange(this.ValidateObjectList(this.contractAttachment, this.CheckContractAttachment));
            result.AddRange(this.ValidateObjectList(this.trustDocAttachment, this.CheckTrustDocAttachment));

            return result;
        }

        protected override List<ValidateObjectResult> ValidateAttachments()
        {
            var result = new List<ValidateObjectResult>();

            result.AddRange(this.ValidateObjectList(this.contractAttachment, this.CheckContractAttachmentUploaded));
            result.AddRange(this.ValidateObjectList(this.trustDocAttachment, this.CheckTrustDocAttachmentUploaded));

            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importPublicPropertyContractRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importPublicPropertyContractRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса.
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<RisPublicPropertyContract>> GetPortions()
        {
            var result = new List<IEnumerable<RisPublicPropertyContract>>();

            if (this.contracts.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.contracts.Skip(startIndex).Take(PublicPropertyContractPrepareDataTask.Portion));
                    startIndex += PublicPropertyContractPrepareDataTask.Portion;
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
        private importPublicPropertyContractRequest GetRequestObject(
            IEnumerable<RisPublicPropertyContract> listForImport,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var contractList = new List<importPublicPropertyContractRequestContract>();

            var contractTransportGuidDictionary = new Dictionary<string, long>();

            foreach (var contract in listForImport)
            {
                var listItem = this.GetImportPublicPropertyContractRequestContract(contract);
                contractList.Add(listItem);

                contractTransportGuidDictionary.Add(listItem.TransportGUID, contract.Id);
            }

            transportGuidDictionary.Add(typeof(RisContract), contractTransportGuidDictionary);

            return new importPublicPropertyContractRequest {Contract = contractList.ToArray()};
        }

        /// <summary>
        /// Создать объект importPublicPropertyContractRequest по RisPublicPropertyContract
        /// </summary>
        /// <param name="contract">Объект типа RisPublicPropertyContract</param>
        /// <returns>Объект типа importPublicPropertyContractRequest</returns>
        private importPublicPropertyContractRequestContract GetImportPublicPropertyContractRequestContract(RisPublicPropertyContract contract)
        {
            var importContractRequest = new importPublicPropertyContractRequestContract
            {
                TransportGUID = Guid.NewGuid().ToString(),
                Item = this.GetRequestContractItem(contract)
            };

            if (contract.Operation == RisEntityOperation.Update)
            {
                importContractRequest.ContractVersionGUID = contract.Guid;
            }

            return importContractRequest;
        }

        /// <summary>
        /// Получить объект Item раздела importPublicPropertyContractRequestContract
        /// </summary>
        /// <param name="contract">Договор управления</param>
        /// <returns>Объект Item</returns>
        private object GetRequestContractItem(RisPublicPropertyContract contract)
        {
            object item = null;

            if (contract.Operation == RisEntityOperation.Create || contract.Operation == RisEntityOperation.Update)
            {
                object orgIndItem = null;

                if (contract.Organization != null)
                {
                    orgIndItem = new RegOrgType
                    {
                        orgRootEntityGUID = contract.Organization.OrgRootEntityGuid
                    };
                }
                else if (contract.Entrepreneur != null)
                {
                    var entrepreneur = contract.Entrepreneur;

                    orgIndItem = new IndType
                    {
                        Surname = entrepreneur.Surname,
                        FirstName = entrepreneur.FirstName,
                        Sex = entrepreneur.Sex == RisGender.F ? Sex.F : Sex.M,
                        DateOfBirth = entrepreneur.DateOfBirth ?? DateTime.MinValue,
                        Item = entrepreneur.Snils,
                        PlaceBirth = entrepreneur.PlaceBirth
                    };
                }

                item = new PublicPropertyContractType
                {
                    Item = orgIndItem,
                    FIASHouseGuid = contract.House.FiasHouseGuid,
                    ContractObject = contract.ContractObject,
                    ContractNumber = contract.ContractNumber,
                    StartDate = contract.StartDate ?? DateTime.MinValue,
                    EndDate = contract.EndDate ?? DateTime.MinValue,
                    ContractAttachment = this.GetContractAttachments(contract),
                    RentAgrConfirmationDocument = this.GetProtocolAttachments(contract)
                };
            }
            else if (contract.Operation == RisEntityOperation.Delete)
            {
                item = new AnnulmentType
                {
                    ReasonOfAnnulment = "Договор был удален"
                };
            }

            return item;
        }

        private AttachmentType[] GetContractAttachments(RisPublicPropertyContract contract)
        {
            List<AttachmentType> result = new List<AttachmentType>();

            List<RisContractAttachment> attachments;
            if (this.contractAttachmentByContractIdMap.TryGetValue(contract.Id, out attachments))
            {
                foreach (var attachment in attachments)
                {
                    result.Add(
                        new AttachmentType
                        {
                            Name = attachment.Attachment.Name,
                            Description = attachment.Attachment.Description,
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

        private PublicPropertyContractTypeRentAgrConfirmationDocument[] GetProtocolAttachments(RisPublicPropertyContract contract)
        {
            var result = new List<PublicPropertyContractTypeRentAgrConfirmationDocument>();
            var protocolList = new List<object>();

            List<RisTrustDocAttachment> attachments;
            if (this.trustDocAttachmentByContractIdMap.TryGetValue(contract.Id, out attachments))
            {
                foreach (var attachment in attachments)
                {
                    protocolList.Add(
                        new PublicPropertyContractTypeRentAgrConfirmationDocumentProtocolMeetingOwners
                        {
                            ProtocolNum = attachment.PublicPropertyContract.ProtocolNumber,
                            ProtocolDate = attachment.PublicPropertyContract.ProtocolDate ?? DateTime.MinValue,
                            TrustDocAttachment = new[]
                            {
                                new AttachmentType
                                {
                                    Name = attachment.Attachment.Name,
                                    Description = attachment.Attachment.Description,
                                    Attachment = new Attachment
                                    {
                                        AttachmentGUID = attachment.Attachment.Guid
                                    },
                                    AttachmentHASH = attachment.Attachment.Hash
                                }
                            }
                        });
                }
            }

            result.Add(
                new PublicPropertyContractTypeRentAgrConfirmationDocument
                {
                    Items = protocolList.ToArray()
                });

            return result.ToArray();
        }

        private ValidateObjectResult CheckContract(RisPublicPropertyContract contract)
        {
            var messages = new StringBuilder();

            if (contract.Organization != null)
            {
                if (contract.Organization.OrgRootEntityGuid.IsEmpty())
                {
                    messages.Append("Organization.OrgRootEntityGuid ");
                }
            }
            else if (contract.Entrepreneur != null)
            {
                if (contract.Entrepreneur.Surname.IsEmpty())
                {
                    messages.Append("Entrepreneur.Surname ");
                }
                if (contract.Entrepreneur.FirstName.IsEmpty())
                {
                    messages.Append("Entrepreneur.FirstName ");
                }
                if (!contract.Entrepreneur.DateOfBirth.HasValue)
                {
                    messages.Append("Entrepreneur.DateOfBirth ");
                }
                if (contract.Entrepreneur.Snils.IsEmpty())
                {
                    messages.Append("Entrepreneur.Snils ");
                }
                if (contract.Entrepreneur.PlaceBirth.IsEmpty())
                {
                    messages.Append("Entrepreneur.PlaceBirth ");
                }
            }

            if (contract.House == null || contract.House.FiasHouseGuid.IsEmpty())
            {
                messages.Append("House.FiasHouseGuid ");
            }

            if (contract.ContractNumber.IsEmpty())
            {
                messages.Append("ContractNumber ");
            }

            if (!contract.StartDate.HasValue)
            {
                messages.Append("StartDate ");
            }

            if (!this.contractAttachmentByContractIdMap.ContainsKey(contract.Id))
            {
                messages.Append("ContractAttachment ");
            }

            if (!this.trustDocAttachmentByContractIdMap.ContainsKey(contract.Id))
            {
                messages.Append("TrustDocAttachment ");
            }

            return new ValidateObjectResult
            {
                Id = contract.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Список ДОИ"
            };
        }

        private ValidateObjectResult CheckContractAttachment(RisContractAttachment attachment)
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
                Description = "Документы договора"
            };
        }

        private ValidateObjectResult CheckContractAttachmentUploaded(RisContractAttachment attachment)
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
                Description = "Документы договора"
            };
        }

        private ValidateObjectResult CheckTrustDocAttachment(RisTrustDocAttachment attachment)
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
                Description = "Документы, подтверждающие полномочия заключать договор"
            };
        }

        private ValidateObjectResult CheckTrustDocAttachmentUploaded(RisTrustDocAttachment attachment)
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
                Description = "Документы, подтверждающие полномочия заключать договор"
            };
        }
    }
}