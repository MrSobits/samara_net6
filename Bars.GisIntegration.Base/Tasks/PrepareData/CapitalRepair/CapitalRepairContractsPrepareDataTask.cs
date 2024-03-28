namespace Bars.GisIntegration.Base.Tasks.PrepareData.CapitalRepair
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.CapitalRepair;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;

    /// <summary>
    /// Задача получения результатов для экспортера договоров на выполнение работ (оказание услуг) по капитальному ремонту
    /// </summary>
    public class CapitalRepairContractsPrepareDataTask : BasePrepareDataTask<importContractsRequest>
    {
        /// <summary>
        /// Максимальное количество записей обособленных подразделений, которые можно экспортировать одним запросом
        /// </summary>
        private const int PortionSize = 1000;

        private List<RisCrContract> crContractToExport = new List<RisCrContract>();
        private Dictionary<long, List<RisCrAttachContract>> contractAttachmentListById;
        private Dictionary<long, List<RisCrAttachOutlay>> outlayAttachmentListById;
        private Dictionary<long, List<RisCrWork>> crWorkListById;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var crContractExtractor = this.Container.Resolve<IDataExtractor<RisCrContract>>("CapitalRepairContractsDataExtractor");
            var crContractAttachmentExtractor = this.Container.Resolve<IDataExtractor<RisCrAttachContract>>("CapitalRepairAttachContractDataExtractor");
            var crOutlayAttachmentExtractor = this.Container.Resolve<IDataExtractor<RisCrAttachOutlay>>("CapitalRepairAttachOutlayDataExtractor");
            var crWorkExtractor = this.Container.Resolve<IDataExtractor<RisCrWork>>("CapitalRepairWorksDataExtractor");

            try
            {
                this.crContractToExport = this.RunExtractor(crContractExtractor, parameters);
                
                parameters["risCrContracts"] = this.crContractToExport.Select(x => x);

                this.crWorkListById = this.RunExtractor(crWorkExtractor, parameters)
                      .GroupBy(x => x.Contract.Id)
                      .ToDictionary(x => x.Key, x => x.ToList());

                this.contractAttachmentListById = this.RunExtractor(crContractAttachmentExtractor, parameters)
                      .GroupBy(x => x.Contract.Id)
                      .ToDictionary(x => x.Key, x => x.ToList());

                this.outlayAttachmentListById = this.RunExtractor(crOutlayAttachmentExtractor, parameters)
                      .GroupBy(x => x.Contract.Id)
                      .ToDictionary(x => x.Key, x => x.ToList());
            }
            finally
            {
                this.Container.Release(crContractExtractor);
                this.Container.Release(crContractAttachmentExtractor);
                this.Container.Release(crOutlayAttachmentExtractor);
                this.Container.Release(crWorkExtractor);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();

            var crContractToRemove = new List<RisCrContract>();

            var crContractToValidate = this.crContractToExport
                .Where(x => x.Operation != RisEntityOperation.Delete)
                .ToList();

            foreach (var crContract in crContractToValidate)
            {
                var messages = new StringBuilder();

                if (crContract.Number.IsEmpty())
                {
                    messages.Append($"{nameof(crContract.Number)} ");
                }

                if (!crContract.Date.HasValue)
                {
                    messages.Append($"{nameof(crContract.Date)} ");
                }

                if (!crContract.StartDate.HasValue)
                {
                    messages.Append($"{nameof(crContract.StartDate)} ");
                }

                if (!crContract.EndDate.HasValue)
                {
                    messages.Append($"{nameof(crContract.EndDate)} ");
                }

                if (!crContract.Sum.HasValue)
                {
                    messages.Append($"{nameof(crContract.Sum)} ");
                }

                if (crContract.Customer.IsEmpty())
                {
                    messages.Append($"{nameof(crContract.Customer)} ");
                }

                if (crContract.Performer.IsEmpty())
                {
                    messages.Append($"{nameof(crContract.Performer)} ");
                }

                result.Add(new ValidateObjectResult
                {
                    Id = crContract.Id,
                    State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                    Message = messages.ToString(),
                    Description = "Сведения о договорах на выполнение работ (оказание услуг) по капитальному ремонту"
                });

                result.AddRange(this.ValidateContractWorkData(crContract.Id, ref crContractToRemove));

                if (messages.Length > 0)
                {
                    crContractToRemove.Add(crContract);
                }
            }

            foreach (var risContract in crContractToRemove)
            {
                this.crContractToExport.Remove(risContract);
            }

            return result;
        }

        private List<ValidateObjectResult> ValidateContractWorkData(long entityId, ref List<RisCrContract> crContractToRemove)
        {
            var result = new List<ValidateObjectResult>();

            foreach (var entity in this.crWorkListById[entityId])
            {
                var messages = new StringBuilder();

                if (!entity.StartDate.HasValue)
                {
                    messages.Append("WorkContractType.StartDate ");
                }

                if (!entity.EndDate.HasValue)
                {
                    messages.Append("WorkContractType.EndDate ");
                }

                if (!entity.Cost.HasValue)
                {
                    messages.Append("WorkContractType.Cost ");
                }

                if (!entity.CostPlan.HasValue)
                {
                    messages.Append("WorkContractType.CostPlan ");
                }

                if (!entity.Volume.HasValue)
                {
                    messages.Append("WorkContractType.Volume ");
                }

                result.Add(new ValidateObjectResult
                {
                    Id = entity.Id,
                    State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                    Message = messages.ToString(),
                    Description = "Сведения о работах договора на выполнение работ (оказание услуг) по капитальному ремонту"
                });

                if (messages.Length > 0)
                {
                    crContractToRemove.Add(this.crContractToExport.First(x => x.Id == entityId));
                }
            }
            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importContractsRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importContractsRequest, Dictionary<Type, Dictionary<string, long>>>();

            var listToCreateOrUpdate = this.crContractToExport
                .Where(x => x.Operation != RisEntityOperation.Delete)
                .ToList();

            var crContractsPortions = this.SplitToPortions(listToCreateOrUpdate, CapitalRepairContractsPrepareDataTask.PortionSize);

            foreach (var portion in crContractsPortions)
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.CreateImportRequest(portion, transportGuidDictionary);

                if (this.DataForSigning)
                {
                    request.Id = Guid.NewGuid().ToString();
                }

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }


        /// <summary>
        /// Разбивание списка на порции
        /// </summary>
        /// <param name="subsidiaries">Исходный список</param>
        /// <param name="portionSize">Размер порции</param>
        /// <returns>Список порций</returns>
        private List<IEnumerable<RisCrContract>> SplitToPortions(List<RisCrContract> subsidiaries, int portionSize)
        {
            var result = new List<IEnumerable<RisCrContract>>();

            if (!subsidiaries.Any())
            {
                return result;
            }

            var startIndex = 0;

            do
            {
                result.Add(subsidiaries.Skip(startIndex).Take(portionSize));
                startIndex += portionSize;
            }
            while (startIndex < subsidiaries.Count);

            return result;
        }

        /// <summary>
        /// Создание объекта запроса importContractsRequest
        /// </summary>
        /// <param name="entities">Импортируемые договора</param>
        /// <param name="transportGuidDictionary">Словарь транспортных гуидов</param>
        /// <returns></returns>
        private importContractsRequest CreateImportRequest(
            IEnumerable<RisCrContract> entities,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var crContractElements = new List<importContractsRequestImportContract>();
            var crContractTransportGuidDictionary = new Dictionary<string, long>();

            foreach (var entity in entities)
            {
                object item;

                if (entity.Operation == RisEntityOperation.Delete)
                {
                    item = true;
                }
                else
                {
                    dynamic outlayObject = entity.OutlayMissing ? (object)entity.OutlayMissing : this.GetOutlayAttachments(entity);
                    item = new ContractType
                    {
                        Number = entity.Number,
                        Date = entity.Date.GetValueOrDefault(),
                        Item1 = entity.TenderInetAddress,
                        AttachContract = this.GetContractAttachments(entity),
                        Customer = new RegOrgType
                        {
                            orgRootEntityGUID = entity.Customer
                        },
                        StartDate = entity.StartDate.GetValueOrDefault(),
                        EndDate = entity.EndDate.GetValueOrDefault(),
                        Item = entity.WarrantyMonthCount,
                        Sum = entity.Sum.GetValueOrDefault(),
                        Items = outlayObject,
                        Performer = new RegOrgType
                        {
                            orgRootEntityGUID = entity.Performer,
                        },
                        Work = this.GetContractWorks(entity)
                    };

                }
                var transportGuid = Guid.NewGuid().ToString();
                var crContractElement = new importContractsRequestImportContract
                {
                    TransportGuid = transportGuid,
                    Item = item,
                    ContractGuid = (entity.Operation == RisEntityOperation.Update) ? entity.Guid : null
                };

                crContractElements.Add(crContractElement);
                crContractTransportGuidDictionary.Add(transportGuid, entity.Id);
            }

            transportGuidDictionary.Add(typeof(importContractsRequestImportContract), crContractTransportGuidDictionary);

            return new importContractsRequest { importContract = crContractElements.ToArray(), };
        }

        /// <summary>
        /// Получить раздел OutlayAttachments
        /// </summary>
        /// <param name="entity">Импортируемый договор</param>
        /// <returns>Раздел OutlayAttachments</returns>
        private AttachmentType[] GetOutlayAttachments(RisCrContract entity)
        {
            List<AttachmentType> result = new List<AttachmentType>();

            if (this.outlayAttachmentListById.ContainsKey(entity.Id))
            {
                foreach (var attach in this.outlayAttachmentListById[entity.Id])
                {
                    if (attach.Attachment != null)
                    {
                        result.Add(new AttachmentType
                        {
                            Name = attach.Attachment.Name,
                            Description = attach.Attachment.Description,
                            Attachment = new Attachment
                            {
                                AttachmentGUID = attach.Attachment.Guid
                            },
                            AttachmentHASH = attach.Attachment.Hash
                        });
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получить раздел ContractAttachments
        /// </summary>
        /// <param name="entity">Контракт</param>
        /// <returns>Раздел ContractAttachments</returns>
        private AttachmentType[] GetContractAttachments(RisCrContract entity)
        {
            List<AttachmentType> result = new List<AttachmentType>();

            if (this.contractAttachmentListById.ContainsKey(entity.Id))
            {
                foreach (var attach in this.contractAttachmentListById[entity.Id])
                {
                    if (attach.Attachment != null)
                    {
                        result.Add(new AttachmentType
                        {
                            Name = attach.Attachment.Name,
                            Description = attach.Attachment.Description,
                            Attachment = new Attachment
                            {
                                AttachmentGUID = attach.Attachment.Guid
                            },
                            AttachmentHASH = attach.Attachment.Hash
                        });
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получить раздел ContractWorks
        /// </summary>
        /// <param name="entity">Контракт</param>
        /// <returns>Раздел ContractWorks</returns>
        private WorkContractType[] GetContractWorks(RisCrContract entity)
        {
            List<WorkContractType> result = new List<WorkContractType>();

            if (this.crWorkListById.ContainsKey(entity.Id))
            {
                foreach (var item in this.crWorkListById[entity.Id])
                {
                    result.Add(new WorkContractType
                    {
                        StartDate = item.StartDate.GetValueOrDefault(),
                        EndDate = item.EndDate.GetValueOrDefault(),
                        Item1 = item.OtherUnit,
                        Item = new WorkPlanIdentityType
                        {
                            ApartmentBilding = item.ApartmentBuildingFiasGuid,
                            EndMonthYear = item.EndMonthYear,
                            WorkKind = new nsiRef
                            {
                                Code = item.WorkKindCode,
                                GUID = item.WorkKindGuid
                            }
                        },
                        AdditionalInfo = item.AdditionalInfo,
                        Cost = item.Cost.GetValueOrDefault(),
                        CostPlan = item.CostPlan.GetValueOrDefault(),
                        Volume = item.Volume.GetValueOrDefault(),
                        Item1ElementName = Item1ChoiceType.OtherUnit
                    });
                }
            }

            return result.ToArray();
        }
    }
}