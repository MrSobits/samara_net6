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
    using SupplyResourceContractType = HouseManagementAsync.SupplyResourceContractType;

    /// <summary>
    /// Задача подготовки данных договора ресурсоснабжения
    /// </summary>
    public class SupplyResourceContractPrepareDataTask : BasePrepareDataTask<importSupplyResourceContractRequest>
    {
        private List<SupplyResourceContract> contracts;
        private List<SupResContractAttachment> attachments;
        private List<SupResContractServiceResource> serviceResources;
        private List<SupResContractTemperatureChart> temperatureCharts;
        private List<SupResContractSubject> contractSubjects;
        private List<SupResContractSubjectOtherQuality> contractSubjectQualities;

        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 1000;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var supplyResourceContractExtractor = this.Container.Resolve<IDataExtractor<SupplyResourceContract>>("SupplyResourceContractExtractor");
            var supResContractServiceResourceExtractor = this.Container.Resolve<IDataExtractor<SupResContractServiceResource>>("SupResContractServiceResourceExtractor");
            var supResContractSubjectOtherQualityExtractor = this.Container.Resolve<IDataExtractor<SupResContractSubjectOtherQuality>>("SupResContractSubjectOtherQualityExtractor");
            var supResContractTemperatureChartExtractor = this.Container.Resolve<IDataExtractor<SupResContractTemperatureChart>>("SupResContractTemperatureChartExtractor");
            var supResContractSubjectExtractor = this.Container.Resolve<IDataExtractor<SupResContractSubject>>("SupResContractSubjectExtractor");
            var supResContractAttachmentExtractor = this.Container.Resolve<IDataExtractor<SupResContractAttachment>>("SupResContractAttachmentExtractor");

            try
            {
                this.contracts = this.RunExtractor(supplyResourceContractExtractor, parameters);
                parameters.Add("selectedContracts", this.contracts);

                this.attachments = this.RunExtractor(supResContractAttachmentExtractor, parameters);
                this.serviceResources = this.RunExtractor(supResContractServiceResourceExtractor, parameters);
                this.temperatureCharts = this.RunExtractor(supResContractTemperatureChartExtractor, parameters);
                this.contractSubjects = this.RunExtractor(supResContractSubjectExtractor, parameters);
                parameters.Add("selectedContractSubjects", this.contractSubjects);

                this.contractSubjectQualities = this.RunExtractor(supResContractSubjectOtherQualityExtractor, parameters);
            }
            finally
            {
                this.Container.Release(supplyResourceContractExtractor);
                this.Container.Release(supResContractServiceResourceExtractor);
                this.Container.Release(supResContractSubjectOtherQualityExtractor);
                this.Container.Release(supResContractTemperatureChartExtractor);
                this.Container.Release(supResContractSubjectExtractor);
                this.Container.Release(supResContractAttachmentExtractor);
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
            result.AddRange(this.ValidateObjectList(this.attachments, this.CheckAttachment));
            result.AddRange(this.ValidateObjectList(this.serviceResources, this.CheckServiceResource));
            result.AddRange(this.ValidateObjectList(this.temperatureCharts, this.CheckTemperatureChart));
            result.AddRange(this.ValidateObjectList(this.contractSubjects, this.CheckContractSubject));
            result.AddRange(this.ValidateObjectList(this.contractSubjectQualities, this.CheckContractSubjectQuality));

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
        protected override Dictionary<importSupplyResourceContractRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importSupplyResourceContractRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList, transportGuidDictionary);

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса.
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект запроса</returns>
        private importSupplyResourceContractRequest GetRequestObject(IEnumerable<SupplyResourceContract> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var contractList = new List<importSupplyResourceContractRequestContract>();

            var contractTransportGuidDictionary = new Dictionary<string, long>();

            foreach (var contract in listForImport)
            {
                var listItem = this.GetImportSupplyResourceContractRequestContract(contract);
                contractList.Add(listItem);

                contractTransportGuidDictionary.Add(listItem.TransportGUID, contract.Id);
            }

            transportGuidDictionary.Add(typeof(RisContract), contractTransportGuidDictionary);

            return new importSupplyResourceContractRequest { Contract = contractList.ToArray() };
        }

        /// <summary>
        /// Получить объект запроса для договора
        /// </summary>
        /// <param name="contract">Договор ресуронабжения</param>
        /// <returns>Объект запроса</returns>
        private importSupplyResourceContractRequestContract GetImportSupplyResourceContractRequestContract(SupplyResourceContract contract)
        {
            var importContractRequest = new importSupplyResourceContractRequestContract
            {
                TransportGUID = Guid.NewGuid().ToString(),
                Item = this.GetRequestContractItem(contract)
            };

            if (contract.Operation != RisEntityOperation.Create)
            {
                importContractRequest.ContractGUID = contract.Guid;
            }

            return importContractRequest;
        }

        /// <summary>
        /// Получить объект Item раздела importSupplyResourceContractRequestContract
        /// </summary>
        /// <param name="contract">Договор управления</param>
        /// <returns>Объект Item</returns>
        private object GetRequestContractItem(SupplyResourceContract contract)
        {
            object item = null;

            if (contract.Operation == RisEntityOperation.Create || contract.Operation == RisEntityOperation.Update)
            {
                item = new SupplyResourceContractType
                {
                    ComptetionDate = contract.ComptetionDate ?? DateTime.MinValue,
                    ComptetionDateSpecified = contract.ComptetionDate.HasValue,
                    Period = new SupplyResourceContractTypePeriod
                    {
                        Start = new SupplyResourceContractTypePeriodStart
                        {
                            StartDate = (sbyte)(contract.StartDate ?? 0),
                            NextMonth = contract.StartDateNextMonth ?? false,
                            NextMonthSpecified = contract.StartDateNextMonth.HasValue
                        },
                        End = new SupplyResourceContractTypePeriodEnd
                        {
                            EndDate = (sbyte)(contract.EndDate ?? 0),
                            NextMonth = contract.EndDateNextMonth ?? false,
                            NextMonthSpecified = contract.EndDateNextMonth.HasValue
                        }
                    },
                    ContractBase = new nsiRef
                    {
                        Code = contract.ContractBaseCode,
                        GUID = contract.ContractBaseGuid
                    },
                    Item = this.GetSupplyResourceContractTypeItem(contract),
                    CountingResource = contract.CommercialMeteringResourceType == SupResCommercialMeteringResourceType.CommunalServicesExecutor ? SupplyResourceContractTypeCountingResource.P : SupplyResourceContractTypeCountingResource.R,
                    CountingResourceSpecified = contract.CommercialMeteringResourceType.HasValue,
                    ObjectAddress = !contract.FiasHouseGuid.IsEmpty() ? this.GetObjectAddress(contract) : null,
                    TemperatureChart = this.GetTemperatureChart(contract.Id),
                    Item1 = new SupplyResourceContractTypeIsContract
                    {
                        ContractNumber = contract.ContractNumber,
                        SigningDate = contract.SigningDate ?? DateTime.MinValue,
                        EffectiveDate = contract.EffectiveDate ?? DateTime.MinValue,
                       // ContractSubject = this.GetContractSubject(contract.Id),
                        ContractAttachment = this.GetContractAttachment(contract.Id),
                        //BillingDate = (sbyte)(contract.BillingDate ?? 0),
                        //PaymentDate = (sbyte)(contract.PaymentDate ?? 0),
                        //ProvidingInformationDate = (sbyte)(contract.ProvidingInformationDate ?? 0)
                    }
                };
            }
            else if (contract.Operation == RisEntityOperation.Delete)
            {
                item = new AnnulmentType
                {
                    ReasonOfAnnulment = "Договор ресурсоснабжения с РСО был удален"
                };
            }

            return item;
        }

        /// <summary>
        /// Получить раздел ContractAttachment для SupplyResourceContractTypeIsContract
        /// </summary>
        /// <param name="contractId">Идентификатор договора</param>
        /// <returns>ContractAttachment для SupplyResourceContractTypeIsContract</returns>
        private AttachmentType[] GetContractAttachment(long contractId)
        {
            var result = new List<AttachmentType>();

            foreach (var attachment in this.attachments.Where(x => x.Contract.Id == contractId))
            {
                result.Add(new AttachmentType
                {
                    Name = attachment.Attachment.Name,
                    Description = attachment.Attachment.Description,
                    Attachment = new HouseManagementAsync.Attachment
                    {
                        AttachmentGUID = attachment.Attachment.Guid
                    },
                    AttachmentHASH = attachment.Attachment.Hash
                });
            }

            return result.ToArray();
        }

        ///// <summary>
        ///// Получить раздел ContractSubject для SupplyResourceContractTypeIsContract
        ///// </summary>
        ///// <param name="contractId">Идентификатор договора</param>
        ///// <returns>ContractSubject для SupplyResourceContractTypeIsContract</returns>
        //private SupplyResourceContractTypeIsContractContractSubject[] GetContractSubject(long contractId)
        //{
        //    var result = new List<SupplyResourceContractTypeIsContractContractSubject>();

        //    foreach (var contractSubject in this.contractSubjects.Where(x => x.Contract.Id == contractId))
        //    {
        //        result.Add(new SupplyResourceContractTypeIsContractContractSubject
        //        {
        //            ServiceType = new ContractSubjectTypeServiceType
        //            {
        //                Code = contractSubject.ServiceTypeCode,
        //                GUID = contractSubject.ServiceTypeGuid
        //            },
        //            MunicipalResource = new ContractSubjectTypeMunicipalResource
        //            {
        //                Code = contractSubject.MunicipalResourceCode,
        //                GUID = contractSubject.MunicipalResourceGuid
        //            },
        //            HeatingSystemType = contractSubject.HeatingSystemType == HeatingSystemType.Closed ? HouseManagementAsync.HeatingSystemType.Closed : HouseManagementAsync.HeatingSystemType.Opened,
        //            HeatingSystemTypeSpecified = contractSubject.HeatingSystemType.HasValue,
        //           // ConnectionScheme = contractSubject.ConnectionSchemeType == Enums.HouseManagement.ConnectionSchemeType.Independent ? ConnectionSchemeType.Independent : ConnectionSchemeType.Dependent,
        //         //   ConnectionSchemeSpecified = contractSubject.ConnectionSchemeType.HasValue,
        //            StartSupplyDate = contractSubject.StartSupplyDate ?? DateTime.MinValue,
        //            EndSupplyDate = contractSubject.EndSupplyDate ?? DateTime.MinValue,
        //            OtherQualityIndicator = this.GetOtherQualityIndicator(contractSubject.Id),
        //            //PlannedVolume = new SupplyResourceContractTypeIsContractContractSubjectPlannedVolume
        //            //{
        //            //    Volume = contractSubject.PlannedVolume ?? 0,
        //            //    Unit = contractSubject.Unit,
        //            //    FeedingMode = contractSubject.FeedingMode
        //            //}
        //        });
        //    }

        //    return result.ToArray();
        //}

        ///// <summary>
        ///// Получить раздел OtherQualityIndicator для SupplyResourceContractTypeIsContractContractSubject
        ///// </summary>
        ///// <param name="contractSubjectId">Идентификатор субъекта договора</param>
        ///// <returns>OtherQualityIndicator для SupplyResourceContractTypeIsContractContractSubject</returns>
        //private ContractSubjectTypeOtherQualityIndicator[] GetOtherQualityIndicator(long contractSubjectId)
        //{
        //    var result = new List<ContractSubjectTypeOtherQualityIndicator>();

        //    foreach (var contractSubjectQuality in this.contractSubjectQualities.Where(x => x.ContractSubject.Id == contractSubjectId))
        //    {
        //        result.Add(new ContractSubjectTypeOtherQualityIndicator
        //        {
        //            IndicatorName = contractSubjectQuality.IndicatorName,
        //            Number = contractSubjectQuality.Number ?? 0,
        //            OKEI = contractSubjectQuality.Okei
        //        });
        //    }

        //    return result.ToArray();
        //}

        /// <summary>
        /// Получить раздел TemperatureChart для RequestContractItem
        /// </summary>
        /// <param name="contractId">Идентификатор договора</param>
        /// <returns>TemperatureChart для RequestContractItem</returns>
        private SupplyResourceContractTypeTemperatureChart[] GetTemperatureChart(long contractId)
        {
            var result = new List<SupplyResourceContractTypeTemperatureChart>();

            foreach (var chart in this.temperatureCharts.Where(x => x.Contract.Id == contractId))
            {
                result.Add(new SupplyResourceContractTypeTemperatureChart
                {
                    FlowLineTemperature = Convert.ToDecimal(chart.FlowLineTemperature),
                    OppositeLineTemperature = Convert.ToDecimal(chart.OppositeLineTemperature),
                    OutsideTemperature = chart.OutsideTemperature ?? 0
                });
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получить раздел ObjectAddress для RequestContractItem
        /// </summary>
        /// <param name="contract">Договор</param>
        /// <returns>ObjectAddress для RequestContractItem</returns>
        private SupplyResourceContractTypeObjectAddress[] GetObjectAddress(SupplyResourceContract contract)
        {
            var pairs = new List<SupplyResourceContractTypeObjectAddressPair>();

            foreach (var pair in this.serviceResources.Where(x => x.Contract.Id == contract.Id))
            {
                pairs.Add(new SupplyResourceContractTypeObjectAddressPair
                {
                    //ServiceType = new SupplyResourceContractTypeObjectAddressPairServiceType
                    //{
                    //    Code = pair.ServiceTypeCode,
                    //    GUID = pair.ServiceTypeGuid
                    //},
                    //MunicipalResource = new SupplyResourceContractTypeObjectAddressPairMunicipalResource
                    //{
                    //    Code = pair.MunicipalResourceCode,
                    //    GUID = pair.MunicipalResourceGuid
                    //},
                    StartSupplyDate = pair.StartSupplyDate ?? DateTime.MinValue,
                    EndSupplyDate = pair.EndSupplyDate ?? DateTime.MinValue
                });
            }

            return new[]
            {
                new SupplyResourceContractTypeObjectAddress
                {
                    FIASHouseGuid = contract.FiasHouseGuid,
                    Pair = pairs.ToArray()
                }
            };
        }

        /// <summary>
        /// Получить Item для раздела SupplyResourceContractType
        /// </summary>
        /// <param name="contract">Договор ресурсоснабжения</param>
        /// <returns>Item для раздела SupplyResourceContractType</returns>
        private object GetSupplyResourceContractTypeItem(SupplyResourceContract contract)
        {
            object result = null;
            var contractType = contract.ContractType;

            if (contractType == Enums.SupplyResourceContractType.OfferContract)
            {
                result = true;
            }
            else if (contractType == Enums.SupplyResourceContractType.RsoAndServicePerformerContract)
            {
                //result = new SupplyResourceContractTypeOrganization
                //{
                //    orgRootEntityGUID = this.Contragent.OrgRootEntityGuid
                //};
            }
            else if (contractType == Enums.SupplyResourceContractType.RsoAndOwnersContact)
            {
                var contractPersonType = contract.PersonType;

                if (contractPersonType == SupplyResourceContactPersonType.Owner)
                {
                    //result = new SupplyResourceContractTypeOwner
                    //{
                    //    Item = this.GetRsoAndOwnersContactItem(contract)
                    //};
                }
                else if (contractPersonType == SupplyResourceContactPersonType.RepresentativeOwners)
                {
                    //result = new SupplyResourceContractTypeAgentOwners
                    //{
                    //    Item = this.GetRsoAndOwnersContactItem(contract)
                    //};
                }
                else if (contractPersonType == SupplyResourceContactPersonType.TenantNonResidentialRoom)
                {
                    //result = new SupplyResourceContractTypeRenter
                    //{
                    //    Item = this.GetRsoAndOwnersContactItem(contract)
                    //};
                }
            }

            return result;
        }

        /// <summary>
        /// Получить раздел Item для типа договоров RsoAndOwnersContact
        /// </summary>
        /// <param name="contract"></param>
        private object GetRsoAndOwnersContactItem(SupplyResourceContract contract)
        {
            object result = null;
            var typeOrganization = contract.PersonTypeOrganization;

            if (typeOrganization == SupplyResourceContactPersonTypeOrganization.RegOrg)
            {
                result = new RegOrgType
                {
                    orgRootEntityGUID = contract.JurPerson?.OrgRootEntityGuid
                };
            }
            else if (typeOrganization == SupplyResourceContactPersonTypeOrganization.Ind)
            {
                result = new IndType
                {
                    Surname = contract.IndSurname,
                    FirstName = contract.IndFirstName,
                    Patronymic = contract.IndPatronymic,
                    Sex = contract.IndSex == RisGender.M ? Sex.M : Sex.F,
                    DateOfBirth = contract.IndDateOfBirth ?? DateTime.MinValue,
                    PlaceBirth = contract.IndPlaceBirth,
                    Item = new ID
                    {
                        Type = new nsiRef
                        {
                            Code = contract.IndIdentityTypeCode,
                            GUID = contract.IndIdentityTypeGuid
                        },
                        Series = contract.IndIdentitySeries,
                        Number = contract.IndIdentityNumber,
                        IssueDate = contract.IndIdentityIssueDate ?? DateTime.MinValue
                    }
                };
            }

            return result;
        }

        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<SupplyResourceContract>> GetPortions()
        {
            var result = new List<IEnumerable<SupplyResourceContract>>();

            if (this.contracts.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.contracts.Skip(startIndex).Take(SupplyResourceContractPrepareDataTask.Portion));
                    startIndex += SupplyResourceContractPrepareDataTask.Portion;
                }
                while (startIndex < this.contracts.Count);
            }

            return result;
        }

        /// <summary>
        /// Проверка показателя качества коммунального ресурса договора ресурсоснабжения
        /// </summary>
        /// <param name="subjectQuality">Показатель качества коммунального ресурса</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckContractSubjectQuality(SupResContractSubjectOtherQuality subjectQuality)
        {
            StringBuilder messages = new StringBuilder();

            if (subjectQuality.Number == null)
            {
                messages.Append("Number ");
            }

            if (subjectQuality.IndicatorName.IsEmpty())
            {
                messages.Append("IndicatorName ");
            }

            if (subjectQuality.Okei.IsEmpty())
            {
                messages.Append("Okei ");
            }

            return new ValidateObjectResult
            {
                Id = subjectQuality.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Показатель качества коммунального ресурса"
            };
        }

        /// <summary>
        /// Проверка предмета договора ресурсоснабжения
        /// </summary>
        /// <param name="contractSubject">Предмет договора</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckContractSubject(SupResContractSubject contractSubject)
        {
            StringBuilder messages = new StringBuilder();

            if (contractSubject.ServiceTypeCode.IsEmpty() || contractSubject.ServiceTypeGuid.IsEmpty())
            {
                messages.Append("ServiceType ");
            }

            if (contractSubject.MunicipalResourceCode.IsEmpty() || contractSubject.MunicipalResourceGuid.IsEmpty())
            {
                messages.Append("MunicipalResource ");
            }

            if (contractSubject.StartSupplyDate == null)
            {
                messages.Append("StartSupplyDate ");
            }

            if (contractSubject.EndSupplyDate == null)
            {
                messages.Append("EndSupplyDate ");
            }

            return new ValidateObjectResult
            {
                Id = contractSubject.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Предмет договора"
            };
        }

        /// <summary>
        /// Проверка информации о температурном графике договора ресурсоснабжения
        /// </summary>
        /// <param name="temperatureChart">Информация о температурном графике договора</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckTemperatureChart(SupResContractTemperatureChart temperatureChart)
        {
            StringBuilder messages = new StringBuilder();

            if (temperatureChart.FlowLineTemperature.IsEmpty())
            {
                messages.Append("FlowLineTemperature ");
            }

            if (temperatureChart.OppositeLineTemperature.IsEmpty())
            {
                messages.Append("OppositeLineTemperature ");
            }

            if (temperatureChart.OutsideTemperature == null)
            {
                messages.Append("OutsideTemperature ");
            }

            return new ValidateObjectResult
            {
                Id = temperatureChart.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Информация о температурном графике договора"
            };
        }

        /// <summary>
        /// Проверка договора ресурсоснабжения перед импортом
        /// </summary>
        /// <param name="contract">Договор ресурсоснабжения</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckContract(SupplyResourceContract contract)
        {
            StringBuilder messages = new StringBuilder();

            if (contract.ContractNumber.IsEmpty())
            {
                messages.Append("ContractNumber ");
            }

            if (contract.StartDate == null)
            {
                messages.Append("StartDate ");
            }

            if (contract.EndDate == null)
            {
                messages.Append("EndDate ");
            }

            if (contract.SigningDate == null)
            {
                messages.Append("SigningDate ");
            }

            if (contract.EffectiveDate == null)
            {
                messages.Append("EffectiveDate ");
            }

            if (contract.BillingDate == null)
            {
                messages.Append("BillingDate ");
            }

            if (contract.PaymentDate == null)
            {
                messages.Append("PaymentDate ");
            }

            if (contract.ProvidingInformationDate == null)
            {
                messages.Append("ProvidingInformationDate ");
            }

            if (this.contractSubjects.All(x => x.Contract.Id != contract.Id))
            {
                messages.Append("ContractSubject ");
            }

            if (this.attachments.All(x => x.Contract.Id != contract.Id))
            {
                messages.Append("ContractAttachment ");
            }

            if (contract.ContractType == Enums.SupplyResourceContractType.RsoAndServicePerformerContract &&
                this.Contragent.OrgRootEntityGuid.IsEmpty())
            {
                messages.Append("Organization ");
            }

            if (contract.ContractType == Enums.SupplyResourceContractType.RsoAndOwnersContact &&
                contract.PersonTypeOrganization == SupplyResourceContactPersonTypeOrganization.RegOrg &&
                contract.JurPerson == null)
            {
                messages.Append("RegOrg ");
            }

            if (contract.ContractType == Enums.SupplyResourceContractType.RsoAndOwnersContact &&
                contract.PersonTypeOrganization == SupplyResourceContactPersonTypeOrganization.Ind)

            {
                if (contract.IndSurname.IsEmpty())
                {
                    messages.Append("Surname ");
                }

                if (contract.IndFirstName.IsEmpty())
                {
                    messages.Append("FirstName ");
                }

                if (contract.IndSex == null)
                {
                    messages.Append("Sex ");
                }

                if (contract.IndDateOfBirth == null)
                {
                    messages.Append("DateOfBirth ");
                }

                if (contract.IndIdentityTypeCode.IsEmpty())
                {
                    messages.Append("IdentityTypeCode ");
                }

                if (contract.IndIdentityTypeGuid.IsEmpty())
                {
                    messages.Append("IdentityTypeGuid ");
                }

                if (contract.IndIdentityNumber.IsEmpty())
                {
                    messages.Append("ID_Number ");
                }

                if (contract.IndIdentityIssueDate == null)
                {
                    messages.Append("ID_IssueDate ");
                }
            }

            return new ValidateObjectResult
            {
                Id = contract.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Договор ресурсоснабжения"
            };
        }

        /// <summary>
        /// Проверка коммунальной услуги и коммунального ресурса договора ресурсоснабжения
        /// </summary>
        /// <param name="serviceResource">Коммунальная услуга и коммунальный ресурс договора ресурсоснабжения</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckServiceResource(SupResContractServiceResource serviceResource)
        {
            StringBuilder messages = new StringBuilder();

            if (serviceResource.ServiceTypeCode.IsEmpty() || serviceResource.ServiceTypeGuid.IsEmpty())
            {
                messages.Append("ServiceType ");
            }

            if (serviceResource.MunicipalResourceCode.IsEmpty() || serviceResource.MunicipalResourceGuid.IsEmpty())
            {
                messages.Append("MunicipalResource ");
            }

            if (serviceResource.StartSupplyDate == null)
            {
                messages.Append("StartSupplyDate ");
            }

            if (serviceResource.EndSupplyDate == null)
            {
                messages.Append("EndSupplyDate ");
            }

            return new ValidateObjectResult
            {
                Id = serviceResource.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Коммунальная услуга и коммунальный ресурс"
            };
        }

        /// <summary>
        /// Проверка файла договора ресурсоснабжения перед импортом
        /// </summary>
        /// <param name="attachment">Файл договора ресурсоснабжения</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckAttachment(SupResContractAttachment attachment)
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
                Description = "Файл договора"
            };
        }

        /// <summary>
        /// Проверка загруженности файла договора ресурсоснабжения перед импортом
        /// </summary>
        /// <param name="attachment">Файл договора ресурсоснабжения</param>
        /// <returns>Результат валидации</returns>
        private ValidateObjectResult CheckAttachmentUploaded(SupResContractAttachment attachment)
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
                Description = "Файл договора"
            };
        }
    }
}
