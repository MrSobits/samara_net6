namespace Bars.GisIntegration.Base.Tasks.PrepareData.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.Gkh.Quartz.Scheduler.Log;
    using Entities.External.Housing.OKI;
    using Entities.HouseManagement;
    using Entities.Infrastructure;
    using InfrastructureAsync;

    using Castle.Core.Internal;

    /// <summary>
    /// Задача подготовки данных по ОКИ
    /// </summary>
    public class RkiPrepareDataTask : BasePrepareDataTask<importOKIRequest>
    {
        private List<RisRkiItem> rkiList = new List<RisRkiItem>();
        private Dictionary<long, List<RisRkiAttachment>> rkiAttachmentListByRkiId;
        private Dictionary<long, List<RisRkiCommunalService>> communalServiceListByRkiId;
        private Dictionary<long, List<RisResource>> rkiResourceListByRkiId;
        private Dictionary<long, List<RisTransportationResources>> rkiTransportationResourceListByRkiId;
        private Dictionary<long, List<RisNetPieces>> rkiNetPiecesListByRkiId;
        private Dictionary<long, List<RisRkiSource>> rkiSourceListByRkiId;
        private Dictionary<long, List<RisRkiReceiver>> rkiReceiverListByRkiId;
        private Dictionary<long, List<RisAttachmentsEnergyEfficiency>> rkiAttachmentEnergyEfficiencyListByRkiId;

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
            var rkiDataExtractor = this.Container.Resolve<BaseDataExtractor<RisRkiItem, OkiObject>>("RkiItemDataExtractor");
            var rkiAttachmentExtractor = this.Container.Resolve<BaseDataExtractor<RisRkiAttachment, OkiDoc>>("RkiAttachmentExtractor");
            var rkiCommunalServiceExtractor = this.Container.Resolve<BaseDataExtractor<RisRkiCommunalService, OkiCommunalService>>("RkiCommunalServiceExtractor");
            var rkiResourceExtractor = this.Container.Resolve<BaseDataExtractor<RisResource, OkiCommunalSource>>("RkiResourceExtractor");
            var rkiTransportationResourceExtractor = this.Container.Resolve<BaseDataExtractor<RisTransportationResources, OkiCommunalSource>>("RkiTransportationResourceExtractor");
            var rkiNetPiecesExtractor = this.Container.Resolve<BaseDataExtractor<RisNetPieces, NetPart>>("RkiNetPiecesExtractor");
            var rkiSourceExtractor = this.Container.Resolve<BaseDataExtractor<RisRkiSource, SourceReceiver>>("RkiSourceExtractor");
            var rkiReceiverExtractor = this.Container.Resolve<BaseDataExtractor<RisRkiReceiver, SourceReceiver>>("RkiReceiverExtractor");
            var rkiAttachmentEnergyEfficiencyExtractor = this.Container.Resolve<BaseDataExtractor<RisAttachmentsEnergyEfficiency, OkiDoc>>("RkiAttachmentEnergyEfficiencyExtractor");

            try
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато извлечение данных по ОКИ"));

                //Объекты ОКИ
                this.rkiList = this.RunExtractor(rkiDataExtractor, parameters);

                //Документы ОКИ
                this.rkiAttachmentListByRkiId = this.RunExtractor(rkiAttachmentExtractor, parameters)
                    .GroupBy(x => x.RkiItem.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());

                //Коммунальные услуги
                this.communalServiceListByRkiId = this.RunExtractor(rkiCommunalServiceExtractor, parameters)
                    .GroupBy(x => x.RkiItem.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());

                //Ресурсы ОКИ
                this.rkiResourceListByRkiId = this.RunExtractor(rkiResourceExtractor, parameters)
                    .GroupBy(x => x.RkiItem.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());

                //Ресурсы транспортировки
                this.rkiTransportationResourceListByRkiId = this.RunExtractor(rkiTransportationResourceExtractor, parameters)
                    .GroupBy(x => x.RkiItem.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());

                //Части сети
                this.rkiNetPiecesListByRkiId = this.RunExtractor(rkiNetPiecesExtractor, parameters)
                    .GroupBy(x => x.RkiItem.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());

                //Источники
                this.rkiSourceListByRkiId = this.RunExtractor(rkiSourceExtractor, parameters)
                    .GroupBy(x => x.RkiItem.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());

                //Получатели
                this.rkiReceiverListByRkiId = this.RunExtractor(rkiReceiverExtractor, parameters)
                    .GroupBy(x => x.RkiItem.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());

                //Документы, подтверждающие соответсвие требованиям энергоэффективности
                this.rkiAttachmentEnergyEfficiencyListByRkiId = this.RunExtractor(rkiAttachmentEnergyEfficiencyExtractor, parameters)
                    .GroupBy(x => x.RkiItem.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());

                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершено извлечение данных по ОКИ"));
            }
            finally
            {
                this.Container.Release(rkiDataExtractor);
                this.Container.Release(rkiAttachmentExtractor);
                this.Container.Release(rkiCommunalServiceExtractor);
                this.Container.Release(rkiResourceExtractor);
                this.Container.Release(rkiTransportationResourceExtractor);
                this.Container.Release(rkiNetPiecesExtractor);
                this.Container.Release(rkiSourceExtractor);
                this.Container.Release(rkiReceiverExtractor);
                this.Container.Release(rkiAttachmentEnergyEfficiencyExtractor);
            }
        }

        #region Валидация данных

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var validationResult = new List<ValidateObjectResult>();

            validationResult.AddRange(this.rkiList
                    .Select(this.CheckRkiItem)
                    .Where(checkResult => checkResult.State != ObjectValidateState.Success)
                    .ToList());

            validationResult.AddRange(this.communalServiceListByRkiId
                    .SelectMany(x => x.Value)
                    .Select(this.CheckCommunalService)
                    .Where(checkResult => checkResult.State != ObjectValidateState.Success)
                    .ToList());

            validationResult.AddRange(this.rkiResourceListByRkiId
                    .SelectMany(x => x.Value)
                    .Select(this.CheckResource)
                    .Where(checkResult => checkResult.State != ObjectValidateState.Success)
                    .ToList());

            validationResult.AddRange(this.rkiTransportationResourceListByRkiId
                    .SelectMany(x => x.Value)
                    .Select(this.CheckTransportationResource)
                    .Where(checkResult => checkResult.State != ObjectValidateState.Success)
                    .ToList());

            validationResult.AddRange(this.rkiNetPiecesListByRkiId
                    .SelectMany(x => x.Value)
                    .Select(this.CheckNetPieces)
                    .Where(checkResult => checkResult.State != ObjectValidateState.Success)
                    .ToList());

            return validationResult;
        }

        private ValidateObjectResult CheckRkiItem(RisRkiItem rki)
        {
            var guidRegex = new Regex("([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}");
            var messages = new StringBuilder();

            if (rki.Name.IsNullOrEmpty() || rki.Name.Length > 140)
            {
                messages.Append("Name ");
            }

            messages.Append(this.CheckNsiRef("Base", rki.BaseCode, rki.BaseGuid, allowBlank: false));

            if ((!rki.IndefiniteManagement.HasValue || !rki.IndefiniteManagement.Value) && rki.EndManagmentDate == null)
            {
                messages.Append("EndManagmentDate ");
            }
            if (!rki.Municipalities.HasValue || !rki.Municipalities.Value)
            {
                if (rki.Contragent.FullName.IsNullOrEmpty() || rki.Contragent.FullName.Length > 500)
                {
                    messages.Append("ContragentFullName ");
                }
                if (rki.Contragent.OrgVersionGuid.IsNullOrEmpty() || !guidRegex.IsMatch(rki.Contragent.OrgVersionGuid))
                {
                    messages.Append("OrgVersionGuid ");
                }
            }

            messages.Append(this.CheckNsiRef("OKIType", rki.TypeCode, rki.TypeGuid, allowBlank: false));
            messages.Append(this.CheckNsiRef("WaterIntake", rki.WaterIntakeCode, rki.WaterIntakeGuid));
            messages.Append(this.CheckNsiRef("ESubstation", rki.ESubstationCode, rki.ESubstationGuid));
            messages.Append(this.CheckNsiRef("PowerPlant", rki.PowerPlantCode, rki.PowerPlantGuid));
            messages.Append(this.CheckNsiRef("FuelType", rki.FuelCode, rki.FuelGuid));
            messages.Append(this.CheckNsiRef("WaterIntake", rki.WaterIntakeCode, rki.WaterIntakeGuid));
            messages.Append(this.CheckNsiRef("GasNetwork", rki.GasNetworkCode, rki.GasNetworkGuid));

            if (!this.communalServiceListByRkiId.ContainsKey(rki.Id))
            {
                messages.Append("Services ");
            }
            if (rki.OktmoCode.IsNullOrEmpty())
            {
                messages.Append("OktmoCode ");
            }
            if (rki.FiasAddress != null && !rki.FiasAddress.AddressName.IsNullOrEmpty() && rki.FiasAddress.AddressName.Length > 140)
            {
                messages.Append("Address ");
            }
            if (rki.CommissioningYear < 1600)
            {
                messages.Append("CommissioningYear ");
            }
            if (rki.Deterioration == null || rki.Deterioration > 100)
            {
                messages.Append("Deterioration ");
            }
            if (rki.CountAccidents.HasValue && rki.CountAccidents < 0)
            {
                messages.Append("CountAccidents ");
            }
            if (!rki.AddInfo.IsNullOrEmpty() && rki.AddInfo.Length > 2000)
            {
                messages.Append("AddInfo ");
            }

            return new ValidateObjectResult
            {
                Id = rki.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "ОКИ"
            };
        }

        private ValidateObjectResult CheckCommunalService(RisRkiCommunalService service)
        {
            var messages = this.CheckNsiRef("Service", service.ServiceCode, service.ServiceGuid, service.ServiceName, false);

            return new ValidateObjectResult
            {
                Id = service.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages,
                Description = "Услуга"
            };
        }

        private ValidateObjectResult CheckResource(RisResource resource)
        {
            var messages = new StringBuilder();

            messages.Append(this.CheckNsiRef("MunicipalResource", resource.MunicipalResourceCode, resource.MunicipalResourceGuid, resource.MunicipalResourceName, false));

            messages.Append(this.CheckPowerType("TotalLoad", resource.TotalLoad));
            messages.Append(this.CheckPowerType("IndustrialLoad", resource.IndustrialLoad));
            messages.Append(this.CheckPowerType("SocialLoad", resource.SocialLoad));
            messages.Append(this.CheckPowerType("PopulationLoad", resource.PopulationLoad));
            messages.Append(this.CheckPowerType("SetPower", resource.SetPower));
            messages.Append(this.CheckPowerType("SitingPower", resource.SitingPower));

            return new ValidateObjectResult
            {
                Id = resource.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Ресурс"
            };
        }

        private ValidateObjectResult CheckTransportationResource(RisTransportationResources resource)
        {
            var messages = new StringBuilder();

            messages.Append(this.CheckNsiRef("MunicipalResource", resource.MunicipalResourceCode, resource.MunicipalResourceGuid, resource.MunicipalResourceName, false));

            messages.Append(this.CheckPowerType("TotalLoad", resource.TotalLoad));
            messages.Append(this.CheckPowerType("IndustrialLoad", resource.IndustrialLoad));
            messages.Append(this.CheckPowerType("SocialLoad", resource.SocialLoad));
            messages.Append(this.CheckPowerType("PopulationLoad", resource.PopulationLoad));
            messages.Append(this.CheckPowerType("VolumeLosses", resource.VolumeLosses, false));

            messages.Append(this.CheckNsiRef("Coolant", resource.CoolantCode, resource.CoolantGuid, resource.CoolantName, false));

            return new ValidateObjectResult
            {
                Id = resource.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Ресурс"
            };
        }

        private ValidateObjectResult CheckNetPieces(RisNetPieces netPiece)
        {
            var messages = new StringBuilder();

            if (!netPiece.Name.IsNullOrEmpty() && netPiece.Name.Length > 140)
            {
                messages.Append("Name ");
            }

            messages.Append(this.CheckPowerType("Diameter", netPiece.Diameter, false));
            messages.Append(this.CheckPowerType("Length", netPiece.Length, false));
            messages.Append(this.CheckPowerType("NeedReplaced", netPiece.NeedReplaced));

            if (netPiece.Wearout.HasValue && netPiece.Wearout > 100)
            {
                messages.Append("Wearout ");
            }

            messages.Append(this.CheckNsiRef("Pressure", netPiece.PressureCode, netPiece.PressureGuid, netPiece.PressureName));
            messages.Append(this.CheckNsiRef("Voltage", netPiece.VoltageCode, netPiece.VoltageGuid, netPiece.VoltageName));

            return new ValidateObjectResult
            {
                Id = netPiece.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Участок сети"
            };
        }

        private string CheckPowerType(string fieldName, decimal? value, bool allowBlank = true)
        {
            if (!allowBlank && !value.HasValue || value.HasValue && value >= 1000000000)
            {
                return fieldName + " ";
            }

            return "";
        }

        private string CheckNsiRef(string fieldName, string code, string guid, string name = "", bool allowBlank = true)
        {
            var messages = new StringBuilder();

            var codeRegex = new Regex("(\\d{1,3}(\\.)?)+");
            var guidRegex = new Regex("([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}");

            if (allowBlank && code.IsNullOrEmpty() && guid.IsNullOrEmpty())
            {
                return "";
            }

            if (code.IsNullOrEmpty() || code.Length > 20 || !codeRegex.IsMatch(code))
            {
                messages.Append(fieldName + "Code ");
            }
            if (guid.IsNullOrEmpty() || !guidRegex.IsMatch(guid))
            {
                messages.Append(fieldName + "Guid ");
            }
            if (!name.IsNullOrEmpty() && name.Length > 2000)
            {
                messages.Append(fieldName + "Name ");
            }

            return messages.ToString();
        }

        #endregion

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importOKIRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importOKIRequest, Dictionary<Type, Dictionary<string, long>>>();

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
        private List<IEnumerable<RisRkiItem>> GetPortions()
        {
            var result = new List<IEnumerable<RisRkiItem>>();

            if (this.rkiList.Count > 0)
            {
                var startIndex = 0;
                do
                {
                    result.Add(this.rkiList.Skip(startIndex).Take(RkiPrepareDataTask.Portion));
                    startIndex += RkiPrepareDataTask.Portion;
                }
                while (startIndex < this.rkiList.Count);
            }

            return result;
        }

        #region Формирование объекта запроса

        /// <summary>
        /// Получить объект запроса.
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Список объектов для импорта</param>
        /// <returns>Объект запроса</returns>
        private importOKIRequest GetRequestObject(IEnumerable<RisRkiItem> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var okiList = new List<importOKIRequestRKIItem>();

            var rkiTransportGuidDictionary = new Dictionary<string, long>();

            foreach (var rki in listForImport)
            {
                var listItem = this.GetImportRkiRequestContract(rki);
                okiList.Add(listItem);

                rkiTransportGuidDictionary.Add(listItem.TransportGUID, rki.Id);
            }

            transportGuidDictionary.Add(typeof(RisAccount), rkiTransportGuidDictionary);

            return new importOKIRequest { RKIItem = okiList.ToArray() };
        }

        /// <summary>
        /// Создать объект importOKIRequestRKIItem по RisRkiItem
        /// </summary>
        /// <param name="rki">Объект типа RisRkiItem</param>
        /// <returns>Объект типа importOKIRequestRKIItem</returns>
        private importOKIRequestRKIItem GetImportRkiRequestContract(RisRkiItem rki)
        {
            var importOkiRequest = new importOKIRequestRKIItem
            {
                TransportGUID = Guid.NewGuid().ToString(),
                Item = this.GetRequestRkiItem(rki)
            };

            if (rki.Operation == RisEntityOperation.Update)
            {
                importOkiRequest.OKIGUID = rki.Guid;
            }

            return importOkiRequest;
        }

        /// <summary>
        /// Получить объект Item раздела importOKIRequestRKIItem
        /// </summary>
        /// <param name="rki">Объект типа RisRkiItem</param>
        /// <returns>Объект Item</returns>
        private object GetRequestRkiItem(RisRkiItem rki)
        {
            if (rki.Operation == RisEntityOperation.Delete)
            {
                return true;
            }

            object management;
            object managerOki;
            OKTMORefType oktmo;

            //Item
            if (rki.IndefiniteManagement != null && rki.IndefiniteManagement.Value)
            {
                management = true;
            }
            else
            {
                management = rki.EndManagmentDate;
            }

            //ManagerOKI
            if (rki.Municipalities != null && rki.Municipalities.Value)
            {
                managerOki = true;
            }
            else
            {
                managerOki = new ManagerOKITypeRSO
                {
                    Name = rki.Contragent.FullName,
                    RSOOrganizationGUID = rki.Contragent.OrgVersionGuid
                };
            }

            //OKTMO
            oktmo = new OKTMORefType
            {
                code = rki.OktmoCode
            };
            if (!rki.OktmoName.IsNullOrEmpty())
            {
                oktmo.name = rki.OktmoName;
            }

            return new importOKIRequestRKIItemOKI
            {
                Name = rki.Name,
                Base = new nsiRef
                {
                    Code = rki.BaseCode,
                    GUID = rki.BaseGuid
                },
                AttachmentList = this.GetAttachments(rki),
                Item = management,
                ManagerOKI = new ManagerOKIType
                {
                    Item = managerOki
                },
                Adress = rki.FiasAddress?.AddressName,
                Deterioration = rki.Deterioration?.RoundDecimal(1) ?? default(decimal),
                IndependentSource = rki.IndependentSource ?? default(bool),
                IndependentSourceSpecified = rki.IndependentSource.HasValue,
                OKIType = this.GetOkiType(rki),
                Services = this.GetCommunalServices(rki),
                OKTMO = oktmo,
                CommissioningYear = rki.CommissioningYear,
                ObjectProperty = this.GetObjectProperty(rki),
                AddInfo = rki.AddInfo,
                AttachmentsEnergyEfficiency = this.GetEnergyEfficiencyAttachments(rki)
            };
        }

        private AttachmentType[] GetAttachments(RisRkiItem rki)
        {
            List<AttachmentType> result = new List<AttachmentType>();

            if (this.rkiAttachmentListByRkiId.ContainsKey(rki.Id))
            {
                foreach (var attach in this.rkiAttachmentListByRkiId[rki.Id])
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

        private nsiRef[] GetCommunalServices(RisRkiItem rki)
        {
            if (this.communalServiceListByRkiId.ContainsKey(rki.Id))
            {
                return this.communalServiceListByRkiId[rki.Id]
                .Select(x => new nsiRef
                {
                    Code = x.ServiceCode,
                    GUID = x.Guid
                }).ToArray();
            }

            return new nsiRef[0];
        }

        private InfrastructureTypeOKIType GetOkiType(RisRkiItem rki)
        {
            nsiRef okiTypeItem = null;
            ItemChoiceType okiTypeItemName = default(ItemChoiceType);

            if (!rki.WaterIntakeGuid.IsNullOrEmpty())
            {
                okiTypeItem = new nsiRef
                {
                    Code = rki.WaterIntakeCode,
                    GUID = rki.WaterIntakeGuid
                };
                okiTypeItemName = ItemChoiceType.WaterIntakeType;
            }
            else if (!rki.ESubstationGuid.IsNullOrEmpty())
            {
                okiTypeItem = new nsiRef
                {
                    Code = rki.ESubstationCode,
                    GUID = rki.ESubstationGuid
                };
                okiTypeItemName = ItemChoiceType.ESubstationType;
            }
            else if (!rki.PowerPlantGuid.IsNullOrEmpty())
            {
                okiTypeItem = new nsiRef
                {
                    Code = rki.PowerPlantCode,
                    GUID = rki.PowerPlantGuid
                };
                okiTypeItemName = ItemChoiceType.PowerPlantType;
            }
            else if (!rki.FuelGuid.IsNullOrEmpty())
            {
                okiTypeItem = new nsiRef
                {
                    Code = rki.FuelCode,
                    GUID = rki.FuelGuid
                };
                okiTypeItemName = ItemChoiceType.FuelType;
            }
            else if (!rki.GasNetworkGuid.IsNullOrEmpty())
            {
                okiTypeItem = new nsiRef
                {
                    Code = rki.GasNetworkCode,
                    GUID = rki.GasNetworkGuid
                };
                okiTypeItemName = ItemChoiceType.GasNetworkType;
            }

            var okiType = new InfrastructureTypeOKIType
            {
                Code = rki.TypeCode,
                GUID = rki.TypeGuid
            };
            if (okiTypeItem != null)
            {
                okiType.Item = okiTypeItem;
                okiType.ItemElementName = okiTypeItemName;
            }

            return okiType;
        }

        private InfrastructureTypeObjectProperty GetObjectProperty(RisRkiItem rki)
        {
            var items = new List<object>();

            //Resources
            if (this.rkiResourceListByRkiId.ContainsKey(rki.Id))
            {
                items.AddRange(this.rkiResourceListByRkiId[rki.Id]
                    .Select(resource => new InfrastructureTypeObjectPropertyResources
                    {
                        MunicipalResource = new nsiRef
                        {
                            Code = resource.MunicipalResourceCode,
                            GUID = resource.MunicipalResourceGuid
                        },
                        TotalLoad = resource.TotalLoad?.RoundDecimal(3) ?? default(decimal),
                        TotalLoadSpecified = resource.TotalLoad.HasValue,
                        IndustrialLoad = resource.IndustrialLoad?.RoundDecimal(3) ?? default(decimal),
                        IndustrialLoadSpecified = resource.IndustrialLoad.HasValue,
                        SocialLoad = resource.SocialLoad?.RoundDecimal(3) ?? default(decimal),
                        SocialLoadSpecified = resource.SocialLoad.HasValue,
                        PopulationLoad = resource.PopulationLoad?.RoundDecimal(3) ?? default(decimal),
                        PopulationLoadSpecified = resource.PopulationLoad.HasValue,
                        SetPower = resource.SetPower?.RoundDecimal(3) ?? default(decimal),
                        SetPowerSpecified = resource.SetPower.HasValue,
                        SitingPower = resource.SitingPower?.RoundDecimal(3) ?? default(decimal),
                        SitingPowerSpecified = resource.SitingPower.HasValue
                    }));
            }

            //TransportationResource
            if (this.rkiTransportationResourceListByRkiId.ContainsKey(rki.Id))
            {
                items.AddRange(this.rkiTransportationResourceListByRkiId[rki.Id]
                    .Select(resource => new InfrastructureTypeObjectPropertyTransportationResources
                    {
                        MunicipalResource = new nsiRef
                        {
                            Code = resource.MunicipalResourceCode,
                            GUID = resource.MunicipalResourceGuid
                        },
                        TotalLoad = resource.TotalLoad?.RoundDecimal(3) ?? default(decimal),
                        TotalLoadSpecified = resource.TotalLoad.HasValue,
                        IndustrialLoad = resource.IndustrialLoad?.RoundDecimal(3) ?? default(decimal),
                        IndustrialLoadSpecified = resource.IndustrialLoad.HasValue,
                        SocialLoad = resource.SocialLoad?.RoundDecimal(3) ?? default(decimal),
                        SocialLoadSpecified = resource.SocialLoad.HasValue,
                        PopulationLoad = resource.PopulationLoad?.RoundDecimal(3) ?? default(decimal),
                        PopulationLoadSpecified = resource.PopulationLoad.HasValue,
                        VolumeLosses = resource.VolumeLosses?.RoundDecimal(3) ?? default(decimal),
                        CoolantType = !resource.CoolantGuid.IsNullOrEmpty()
                        ? new nsiRef
                        {
                            Code = resource.CoolantCode,
                            GUID = resource.Guid
                        } : null
                    }));
            }

            //NetPieces
            if (this.rkiNetPiecesListByRkiId.ContainsKey(rki.Id))
            {
                items.AddRange(this.rkiNetPiecesListByRkiId[rki.Id]
                    .Select(netPiece => new InfrastructureTypeObjectPropertyNetPieces
                    {
                        Name = netPiece.Name,
                        Diameter = netPiece.Diameter?.RoundDecimal(3) ?? default(decimal),
                        Length = netPiece.Length?.RoundDecimal(3) ?? default(decimal),
                        NeedReplaced = netPiece.NeedReplaced?.RoundDecimal(3) ?? default(decimal),
                        NeedReplacedSpecified = netPiece.NeedReplaced.HasValue,
                        Wearout = netPiece.Wearout?.RoundDecimal(1) ?? default(decimal),
                        WearoutSpecified = netPiece.Wearout.HasValue,
                        PressureType = !netPiece.PressureCode.IsNullOrEmpty()
                        ? new nsiRef
                        {
                            Code = netPiece.PressureCode,
                            GUID = netPiece.Guid
                        } : null,
                        VoltageType = !netPiece.VoltageGuid.IsNullOrEmpty()
                        ? new nsiRef
                        {
                            Code = netPiece.VoltageCode,
                            GUID = netPiece.VoltageGuid
                        } : null
                    }));
            }

            //CountAccidents
            if (rki.CountAccidents != null && rki.CountAccidents > 0)
            {
                items.Add(rki.CountAccidents);
            }

            //OkiLinks
            if (this.rkiSourceListByRkiId.ContainsKey(rki.Id))
            {
                var okiLinks = new InfrastructureTypeObjectPropertyOKILinks
                {
                    SourceOKI = this.rkiSourceListByRkiId[rki.Id].Select(x => x.SourceRkiItem.Guid).ToArray()
                };

                if (this.rkiReceiverListByRkiId.ContainsKey(rki.Id))
                {
                    okiLinks.ReceiverOKI = this.rkiReceiverListByRkiId[rki.Id].Select(x => x.ReceiverRkiItem.Guid).ToArray();
                }
                items.Add(okiLinks);
            }

            if (items.Any())
            {
                return new InfrastructureTypeObjectProperty
                {
                    Items = items.ToArray()
                };
            }
            else
            {
                return null;
            }
        }

        private AttachmentType[] GetEnergyEfficiencyAttachments(RisRkiItem rki)
        {
            List<AttachmentType> result = new List<AttachmentType>();

            if (this.rkiAttachmentEnergyEfficiencyListByRkiId.ContainsKey(rki.Id))
            {
                foreach (var attach in this.rkiAttachmentEnergyEfficiencyListByRkiId[rki.Id])
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

        #endregion
    }
}