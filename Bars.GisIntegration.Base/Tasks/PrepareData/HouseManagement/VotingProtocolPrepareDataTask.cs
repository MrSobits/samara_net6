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
    using Entities.HouseManagement;
    using HouseManagementAsync;

    /// <summary>
    /// Задача подготовки данных протокола общего собрания собственников
    /// </summary>
    public class VotingProtocolPrepareDataTask : BasePrepareDataTask<importVotingProtocolRequest>
    {
        private List<RisVotingProtocol> protocols;
        private List<RisVotingProtocolAttachment> attachments;
        private List<RisDecisionList> decisions;

        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 1;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var votingProtocolExtractor = this.Container.Resolve<IDataExtractor<RisVotingProtocol>>("VotingProtocolExtractor");
            var votingProtocolAttachmentExtractor = this.Container.Resolve<IDataExtractor<RisVotingProtocolAttachment>>("VotingProtocolAttachmentExtractor");
            var decisionExtractor = this.Container.Resolve<IDataExtractor<RisDecisionList>>("DecisionExtractor");

            try
            {
                this.protocols = this.RunExtractor(votingProtocolExtractor, parameters);
                parameters.Add("selectedProtocols", this.protocols);

                this.attachments = this.RunExtractor(votingProtocolAttachmentExtractor, parameters);
                this.decisions = this.RunExtractor(decisionExtractor, parameters);
            }
            finally
            {
                this.Container.Release(votingProtocolExtractor);
                this.Container.Release(votingProtocolAttachmentExtractor);
                this.Container.Release(decisionExtractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            result.AddRange(this.ValidateObjectList(this.protocols, this.CheckProtocol));
            result.AddRange(this.ValidateObjectList(this.attachments, this.CheckAttachment));
            result.AddRange(this.ValidateObjectList(this.decisions, this.CheckDecision));

            return result;
        }

        protected override List<ValidateObjectResult> ValidateAttachments()
        {
            var result = new List<ValidateObjectResult>();

            result.AddRange(this.ValidateObjectList(this.attachments, this.CheckAttachmentUploaded));

            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importVotingProtocolRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importVotingProtocolRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);
                request.Id = Guid.NewGuid().ToString();

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Список объектов для импорта</param>
        /// <returns>Объект запроса</returns>
        private importVotingProtocolRequest GetRequestObject(IEnumerable<RisVotingProtocol> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var protocol = listForImport.First();
            var transportGuid = Guid.NewGuid().ToString();

            var protocolTransportGuidDictionary = new Dictionary<string, long>
            {
                { transportGuid, protocol.Id }
            };

            transportGuidDictionary.Add(typeof(RisVotingProtocol), protocolTransportGuidDictionary);

            var importVotingProtocolRequest = new importVotingProtocolRequest
            {
                Item = new importVotingProtocolRequestProtocol
                {
                    FIASHouseGuid = protocol.House?.FiasHouseGuid,
                    ProtocolNum = protocol.ProtocolNum,
                    ProtocolDate = protocol.ProtocolDate ?? DateTime.MinValue,
                    Item = new ProtocolTypeMeeting
                    {
                        VotingPlace = protocol.House?.Adress,
                        MeetingDate = protocol.ProtocolDate ?? DateTime.MinValue,
                        Attachments = this.GetAttachmets(protocol.Id)
                    },
                    Item1 = !protocol.VotingTimeType.HasValue || (RisVotingTimeType)protocol.VotingTimeType == RisVotingTimeType.AnnualVoting,
                    MeetingEligibility = ProtocolTypeMeetingEligibility.C,
                    DecisionList = this.GetDecisions(protocol.Id)
                },
                ItemElementName = ItemChoiceType12.Protocol,
                TransportGUID = transportGuid
            };

            if (protocol.Operation == RisEntityOperation.Update)
            {
                importVotingProtocolRequest.ProtocolGUID = protocol.Guid;
            }

            return importVotingProtocolRequest;
        }

        /// <summary>
        /// Получить список решений для протокола
        /// </summary>
        /// <param name="protocolId">Идентификатор протокола</param>
        /// <returns>Список решений для протокола</returns>
        private ProtocolTypeDecisionList[] GetDecisions(long protocolId)
        {
            var result = new List<ProtocolTypeDecisionList>();

            foreach (var decision in this.decisions.Where(x => x.VotingProtocol.Id == protocolId))
            {
                result.Add(new ProtocolTypeDecisionList
                {
                    QuestionNumber = (decision.QuestionNumber ?? 0).ToString(),
                    QuestionName = decision.QuestionName,
                    DecisionsType = new nsiRef
                    {
                        Code = decision.DecisionsTypeCode,
                        GUID = decision.DecisionsTypeGuid
                    },
                    Agree = decision.Agree ?? 0,
                    FormingFund = new nsiRef
                    {
                        Code = decision.FormingFundCode,
                        GUID = decision.FormingFundGuid
                    },
                    votingResume = ProtocolTypeDecisionListVotingResume.M
                });
            }
             
            return result.ToArray();
        }

        /// <summary>
        /// Получить список файлов протокола
        /// </summary>
        /// <param name="protocolId">Идентификатор протокола</param>
        /// <returns>Список файлов протокола</returns>
        private Attachments[] GetAttachmets(long protocolId)
        {
            var result = new List<Attachments>();

            foreach (var attachment in this.attachments.Where(x => x.VotingProtocol.Id == protocolId))
            {
                result.Add(new Attachments
                {
                    Name = attachment.Attachment.Name,
                    Description = attachment.Attachment.Description,
                    AttachmentHASH = attachment.Attachment.Hash,
                    Attachment = new HouseManagementAsync.Attachment
                    {
                        AttachmentGUID = attachment.Attachment.Guid
                    }
                });
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<RisVotingProtocol>> GetPortions()
        {
            var result = new List<IEnumerable<RisVotingProtocol>>();

            if (this.protocols.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.protocols.Skip(startIndex).Take(VotingProtocolPrepareDataTask.Portion));
                    startIndex += VotingProtocolPrepareDataTask.Portion;
                }
                while (startIndex < this.protocols.Count);
            }

            return result;
        }

        /// <summary>
        /// Проверка решения протокола перед импортом
        /// </summary>
        /// <param name="decision">Решение протокола</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckDecision(RisDecisionList decision)
        {
            StringBuilder messages = new StringBuilder();

            if (decision.QuestionName.IsEmpty())
            {
                messages.Append("QuestionName ");
            }

            if (decision.DecisionsTypeCode.IsEmpty())
            {
                messages.Append("DecisionsTypeCode ");
            }

            if (decision.DecisionsTypeGuid.IsEmpty())
            {
                messages.Append("DecisionsTypeGuid ");
            }

            if (!decision.VotingResume.HasValue)
            {
                messages.Append("VotingResume ");
            }

            return new ValidateObjectResult
            {
                Id = decision.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Решение протокола"
            };
        }

        /// <summary>
        /// Проверка протокола перед импортом
        /// </summary>
        /// <param name="protocol">Протокол</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckProtocol(RisVotingProtocol protocol)
        {
            StringBuilder messages = new StringBuilder();

            if (protocol.House == null || protocol.House.FiasHouseGuid.IsEmpty())
            {
                messages.Append("FIASHouseGuid ");
            }

            if (!protocol.ProtocolDate.HasValue)
            {
                messages.Append("ProtocolDate ");
            }

            if (protocol.VotingPlace.IsEmpty())
            {
                messages.Append("VotingPlace ");
            }

            if (this.attachments.TrueForAll(x => x.VotingProtocol.Id != protocol.Id))
            {
                messages.Append("Attachments ");
            }

            if (!protocol.VotingTimeType.HasValue)
            {
                messages.Append("ProtocolDate ");
            }

            if (!protocol.MeetingEligibility.HasValue)
            {
                messages.Append("MeetingEligibility ");
            }

            if (this.decisions.TrueForAll(x => x.VotingProtocol.Id != protocol.Id))
            {
                messages.Append("DecisionList ");
            }

            return new ValidateObjectResult
            {
                Id = protocol.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Протокол"
            };
        }

        /// <summary>
        /// Проверка файла протокола
        /// </summary>
        /// <param name="attachment">Файл протокола</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckAttachment(RisVotingProtocolAttachment attachment)
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
                Description = "Файл протокола"
            };
        }

        /// <summary>
        /// Проверка загруженности файла протокола
        /// </summary>
        /// <param name="attachment">Файл протокола</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckAttachmentUploaded(RisVotingProtocolAttachment attachment)
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
                Description = "Файл протокола"
            };
        }
    }
}
