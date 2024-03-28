namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using B4.Utils;
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.DeviceMetering;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Entities.HouseManagement;
    using HouseManagementAsync;

    /// <summary>
    /// Задача подготовки данных о ПУ
    /// </summary>
    public class MeteringDevicePrepareDataTask : BasePrepareDataTask<importMeteringDeviceDataRequest>
    {
        private List<RisMeteringDeviceData> devices;
        private List<RisMeteringDeviceAccount> deviceAccounts;
        private List<RisMeteringDeviceLivingRoom> deviceLivingRooms;
        private Dictionary<long, string[]> accountGuidsByDeviceId;
        private Dictionary<long, string[]> livingRoomGuidsByDeviceId;
        private readonly List<long> hasValuesDeviceIds = new List<long>();

        /// <summary>
        /// Размер блока предаваемых данных (максимальное количество записей)
        /// </summary>
        private const int Portion = 100;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var devicesExtractor = this.Container.Resolve<IDataExtractor<RisMeteringDeviceData>>("MeteringDeviceDataExtractor");
            var deviceAccountsExtractor = this.Container.Resolve<IDataExtractor<RisMeteringDeviceAccount>>("MeteringDeviceAccountExtractor");
            var deviceLivingRoomsExtractor = this.Container.Resolve<IDataExtractor<RisMeteringDeviceLivingRoom>>("MeteringDeviceLivingRoomExtractor");
            var meteringDeviceCurrentValueDomain = this.Container.ResolveDomain<RisMeteringDeviceCurrentValue>();
            var meteringDeviceControlValueDomain = this.Container.ResolveDomain<RisMeteringDeviceControlValue>();
            var meteringDeviceVerificationValueDomain = this.Container.ResolveDomain<RisMeteringDeviceVerificationValue>();

            try
            {
                this.devices = this.RunExtractor(devicesExtractor, parameters);
                parameters.Add("selectedDevices", this.devices);

                this.deviceAccounts = this.RunExtractor(deviceAccountsExtractor, parameters);
                this.deviceLivingRooms = this.RunExtractor(deviceLivingRoomsExtractor, parameters);

                this.accountGuidsByDeviceId = this.deviceAccounts
                    .GroupBy(x => x.MeteringDeviceData.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Account.Guid).ToArray());

                this.livingRoomGuidsByDeviceId = this.deviceLivingRooms
                    .GroupBy(x => x.MeteringDeviceData.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.LivingRoom.Guid).ToArray());

                meteringDeviceCurrentValueDomain.GetAll()
                    .Where(x => this.devices.Contains(x.MeteringDeviceData))
                    .Select(x => x.MeteringDeviceData.Id).ToList().AddTo(this.hasValuesDeviceIds);
                meteringDeviceControlValueDomain.GetAll()
                    .Where(x => this.devices.Contains(x.MeteringDeviceData))
                    .Select(x => x.MeteringDeviceData.Id).ToList().AddTo(this.hasValuesDeviceIds);
                meteringDeviceVerificationValueDomain.GetAll()
                    .Where(x => this.devices.Contains(x.MeteringDeviceData))
                    .Select(x => x.MeteringDeviceData.Id).ToList().AddTo(this.hasValuesDeviceIds);

            }
            finally
            {
                this.Container.Release(devicesExtractor);
                this.Container.Release(deviceAccountsExtractor);
                this.Container.Release(deviceLivingRoomsExtractor);
                this.Container.Release(meteringDeviceCurrentValueDomain);
                this.Container.Release(meteringDeviceVerificationValueDomain);
                this.Container.Release(meteringDeviceControlValueDomain);
            }
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            var result = new List<ValidateObjectResult>();
            var itemsToRemove = new List<RisMeteringDeviceData>();

            foreach (var item in this.devices)
            {
                StringBuilder messages = new StringBuilder();

                if (item.Operation == RisEntityOperation.Create) // DeviceDataToCreate
                {
                    messages.Append(this.CheckNewDevice(item));
                }
                else // DeviceDataToUpdate
                {
                    if (this.hasValuesDeviceIds.Contains(item.Id)) // UpdateAfterDevicesValues
                    {
                        messages.Append(this.CheckCollectiveDevice(item));
                        messages.Append(this.CheckDeviceByMeteringDeviceType(item));

                        if (item.FirstVerificationDate == null)
                        {
                            messages.Append("FirstVerificationDate ");
                        }
                    }
                    else if(item.Archivation == null) // UpdateBeforeDevicesValues
                    {
                        messages.Append(this.CheckNewDevice(item));
                    }
                    else if (item.Archivation == MeteringDeviceArchivation.NoReplacing) // ArchiveDevice
                    {
                        if (item.ArchivingReasonCode.IsEmpty() || item.ArchivingReasonGuid.IsEmpty())
                        {
                            messages.Append("ArchivingReason ");
                        }
                    }
                    else // ReplaceDevice
                    {
                        if (item.VerificationDate == null)
                        {
                            messages.Append("VerificationDate ");
                        }

                        if (item.SealDate == null)
                        {
                            messages.Append("SealDate ");
                        }

                        messages.Append(this.CheckDeviceByMeteringDeviceType(item));
                    }
                }

                var validateResult = new ValidateObjectResult
                {
                    Id = item.Id,
                    State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                    Message = messages.ToString(),
                    Description = "Прибор учёта"
                };

                if (validateResult.State != ObjectValidateState.Success)
                {
                    result.Add(validateResult);
                    itemsToRemove.Add(item);
                }
            }

            foreach (var itemToRemove in itemsToRemove)
            {
                this.devices.Remove(itemToRemove);
            }

            return result;
        }

        /// <summary>
        /// Проверить новый ПУ и ПУ без показаний
        /// </summary>
        /// <param name="item">ПУ</param>
        /// <returns>Результат валидации</returns>
        private StringBuilder CheckNewDevice(RisMeteringDeviceData item)
        {
            StringBuilder result = new StringBuilder();

            if (item.MeteringDeviceNumber.IsEmpty())
            {
                result.Append("MeteringDeviceNumber ");
            }

            if (item.MeteringDeviceStamp.IsEmpty())
            {
                result.Append("MeteringDeviceStamp ");
            }

            if (item.MeteringDeviceModel.IsEmpty())
            {
                result.Append("MeteringDeviceModel ");
            }

            if (item.CommissioningDate == null)
            {
                result.Append("CommissioningDate ");
            }

            if (item.ManualModeMetering == null)
            {
                result.Append("ManualModeMetering ");
            }

            if (item.TemperatureSensor == null)
            {
                result.Append("TemperatureSensor ");
            }

            if (item.PressureSensor == null)
            {
                result.Append("PressureSensor ");
            }

            if ((item.DeviceType == DeviceType.ResidentialPremiseDevice && (item.ResidentialPremises == null || item.ResidentialPremises.Guid.IsEmpty())) ||
            (item.DeviceType == DeviceType.NonResidentialPremiseDevice && (item.NonResidentialPremises == null || item.NonResidentialPremises.Guid.IsEmpty())) ||
            (item.DeviceType == DeviceType.CollectiveApartmentDevice && (item.ResidentialPremises == null || item.ResidentialPremises.Guid.IsEmpty())))
            {
                result.Append("PremiseGUID ");
            }

            if (item.DeviceType != DeviceType.CollectiveDevice
                && (!this.accountGuidsByDeviceId.ContainsKey(item.Id) || this.accountGuidsByDeviceId[item.Id].Length == 0))
            {
                result.Append("AccountGUID ");
            }
            else
            {
                result.Append(this.CheckCollectiveDevice(item));
            }

            if (item.DeviceType == DeviceType.LivingRoomDevice
                && (!this.livingRoomGuidsByDeviceId.ContainsKey(item.Id) || this.livingRoomGuidsByDeviceId[item.Id].Length == 0))
            {
                result.Append("LivingRoomGUID ");
            }

            result.Append(this.CheckDeviceByMeteringDeviceType(item));

            return result;
        }

        /// <summary>
        /// Проверить общедомовой ПУ
        /// </summary>
        /// <param name="item">ПУ</param>
        /// <returns>Результат валидации</returns>
        private StringBuilder CheckCollectiveDevice(RisMeteringDeviceData item)
        {
            StringBuilder result = new StringBuilder();

            if (item.ManualModeMetering.GetValueOrDefault() && item.ManualModeInformation.IsEmpty())
            {
                result.Append("ManualModeInformation ");
            }

            if (item.TemperatureSensor.GetValueOrDefault() && item.TemperatureSensorInformation.IsEmpty())
            {
                result.Append("TemperatureSensorInformation ");
            }

            if (item.PressureSensor.GetValueOrDefault() && item.PressureSensorInformation.IsEmpty())
            {
                result.Append("PressureSensorInformation ");
            }

            return result;
        }

        /// <summary>
        /// Проверить ПУ в зависимости от типа ПУ
        /// </summary>
        /// <param name="item">ПУ</param>
        /// <returns>Результат валидации</returns>
        private StringBuilder CheckDeviceByMeteringDeviceType(RisMeteringDeviceData item)
        {
            StringBuilder result = new StringBuilder();

            if (item.MeteringDeviceType == MeteringDeviceType.OneRateMeteringDevice)
            {
                if (item.MunicipalResourceCode.IsEmpty() || item.MunicipalResourceGuid.IsEmpty())
                {
                    result.Append("MunicipalResource ");
                }

                if (item.OneRateDeviceValue == null)
                {
                    result.Append("MeteringValue ");
                }
            }
            else if (item.MeteringDeviceType == MeteringDeviceType.ElectricMeteringDevice && item.MeteringValueT1 == null)
            {
                result.Append("MeteringValueT1 ");
            }

            return result;
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importMeteringDeviceDataRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importMeteringDeviceDataRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var iterationList in this.GetPortions())
            {
                var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                var request = this.GetRequestObject(iterationList.ToList(), transportGuidDictionary);

                result.Add(request, transportGuidDictionary);
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса
        /// </summary>
        /// <param name="listForImport">Список объектов для импорта</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект запроса</returns>
        private importMeteringDeviceDataRequest GetRequestObject(List<RisMeteringDeviceData> listForImport, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var deviceTransportGuidDictionary = new Dictionary<string, long>();
            var deviceList = new List<importMeteringDeviceDataRequestMeteringDevice>();

            foreach (var device in listForImport)
            {
                var deviceItem = this.GetImportMeteringDeviceDataRequest(device);

                deviceList.Add(deviceItem);
                deviceTransportGuidDictionary.Add(deviceItem.TransportGUID, device.Id);
            }

            transportGuidDictionary.Add(typeof(RisMeteringDeviceData), deviceTransportGuidDictionary);

            return new importMeteringDeviceDataRequest
            {
                FIASHouseGuid = listForImport.First().House.FiasHouseGuid,
                MeteringDevice = deviceList.ToArray()
            };
        }

        /// <summary>
        /// Получить объект запроса для прибора учёта
        /// </summary>
        /// <param name="device">Прибор учёта</param>
        /// <returns>Объект запроса</returns>
        private importMeteringDeviceDataRequestMeteringDevice GetImportMeteringDeviceDataRequest(RisMeteringDeviceData device)
        {
            object resultItem;

            if (device.Operation == RisEntityOperation.Create)
            {
                resultItem = this.GetMeteringDeviceToCreate(device);
            }
            else// if (device.Operation == RisEntityOperation.Update)
            {
                resultItem = this.GetMeteringDeviceToUpdate(device);
            }

            return new importMeteringDeviceDataRequestMeteringDevice
            {
                TransportGUID = Guid.NewGuid().ToString(),
                Item = resultItem
            };
        }

        /// <summary>
        /// Получить объект запроса для нового прибора учёта
        /// </summary>
        /// <param name="device">Прибор учёта</param>
        /// <returns>Объект запроса</returns>
        private MeteringDeviceFullInformationType GetMeteringDeviceToCreate(RisMeteringDeviceData device)
        {
            return new MeteringDeviceFullInformationType
            {
                BasicChatacteristicts = new MeteringDeviceBasicCharacteristicsType
                {
                    MeteringDeviceNumber = device.MeteringDeviceNumber,
                    MeteringDeviceStamp = device.MeteringDeviceStamp,
                    MeteringDeviceModel = device.MeteringDeviceModel,
                    InstallationDate = device.InstallationDate.GetValueOrDefault(),
                    InstallationDateSpecified = device.InstallationDate.HasValue,
                    CommissioningDate = device.CommissioningDate.GetValueOrDefault(),
                    //ManualModeMetering = device.ManualModeMetering ?? false,
                    //VerificationCharacteristics = device.FirstVerificationDate.HasValue && !device.VerificationInterval.IsEmpty() && !device.VerificationIntervalGuid.IsEmpty() ?
                    //    new MeteringDeviceBasicCharacteristicsTypeVerificationCharacteristics
                    //    {
                    //        VerificationInterval = new nsiRef
                    //        {
                    //            Code = device.VerificationInterval,
                    //            GUID = device.VerificationIntervalGuid
                    //        },
                    //        FirstVerificationDate = device.FirstVerificationDate.GetValueOrDefault()
                    //    } : null,
                    FactorySealDate = device.FactorySealDate.GetValueOrDefault(),
                    FactorySealDateSpecified = device.FactorySealDate != null,
                    TemperatureSensor = device.TemperatureSensor.GetValueOrDefault(),
                    PressureSensor = device.PressureSensor.GetValueOrDefault(),
                    Item = this.GetBasicChatacteristictsItem(device)
                },
                Items = this.GetMeteringDeviceTypeItems(device)
            };
        }

        /// <summary>
        /// Получить сведения о коммунальном ресурсе и базовые показания ПУ
        /// </summary>
        /// <param name="device">ПУ</param>
        /// <returns>Сведения о коммунальном ресурсе и базовые показания ПУ</returns>
        private object[] GetMeteringDeviceTypeItems(RisMeteringDeviceData device)
        {
            object result;

            if (device.MeteringDeviceType == MeteringDeviceType.OneRateMeteringDevice)
            {
                result = new MunicipalResourceNotElectricType // MunicipalResourceNotEnergy
                {
                    MunicipalResource = new nsiRef
                    {
                        Code = device.MunicipalResourceCode,
                        GUID = device.MunicipalResourceGuid
                    },
                    MeteringValue = device.OneRateDeviceValue.GetValueOrDefault()
                };
            }
            else
            {
                result = new MunicipalResourceElectricType // MunicipalResourceEnergy
                {
                    MeteringValueT1 = device.MeteringValueT1.GetValueOrDefault(),
                    MeteringValueT2 = device.MeteringValueT2.GetValueOrDefault(),
                    MeteringValueT2Specified = device.MeteringValueT2.HasValue,
                    MeteringValueT3 = device.MeteringValueT3.GetValueOrDefault(),
                    MeteringValueT3Specified = device.MeteringValueT3.HasValue,
                    TransformationRatio = device.TransformationRatio.GetValueOrDefault(),
                    TransformationRatioSpecified = device.TransformationRatio.HasValue
                };
            }

            return new [] {result};
        }

        /// <summary>
        /// Получить показания по ПУ
        /// </summary>
        /// <param name="device">ПУ</param>
        /// <returns>Показания по ПУ</returns>
        private object[] GetMeteringValueDeviceTypeItems(RisMeteringDeviceData device)
        {
            object result;

            if (device.MeteringDeviceType == MeteringDeviceType.OneRateMeteringDevice)
            {
                result = new OneRateMeteringValueType // DeviceValueMunicipalResourceNotElectric
                {
                    MunicipalResource = new nsiRef
                    {
                        Code = device.MunicipalResourceCode,
                        GUID = device.MunicipalResourceGuid
                    },
                    MeteringValue = device.OneRateDeviceValue.GetValueOrDefault()
                };
            }
            else
            {
                result = new ElectricMeteringValueType // DeviceValueMunicipalResourceElectric
                {
                    MeteringValueT1 = device.MeteringValueT1.GetValueOrDefault(),
                    MeteringValueT2 = device.MeteringValueT2.GetValueOrDefault(),
                    MeteringValueT2Specified = device.MeteringValueT2.HasValue,
                    MeteringValueT3 = device.MeteringValueT3.GetValueOrDefault(),
                    MeteringValueT3Specified = device.MeteringValueT3.HasValue
                };
            }

            return new[] { result };
        }

        /// <summary>
        /// Получить Item для раздела BasicChatacteristicts
        /// </summary>
        /// <param name="device">ПУ</param>
        /// <returns>Item для раздела BasicChatacteristicts</returns>
        private object GetBasicChatacteristictsItem(RisMeteringDeviceData device)
        {
            object result = null;
            string[] accountGuids = this.accountGuidsByDeviceId?.Get(device.Id);
            string[] livingRoomGuids = this.livingRoomGuidsByDeviceId?.Get(device.Id);

            switch (device.DeviceType)
            {
                case DeviceType.ApartmentHouseDevice:
                    result = new MeteringDeviceBasicCharacteristicsTypeApartmentHouseDevice
                    {
                        AccountGUID = accountGuids
                    };
                    break;
                case DeviceType.CollectiveApartmentDevice:
                    result = new MeteringDeviceBasicCharacteristicsTypeCollectiveApartmentDevice
                    {
                        PremiseGUID = device.ResidentialPremises.Guid,
                        AccountGUID = accountGuids
                    };
                    break;
                case DeviceType.LivingRoomDevice:
                    result = new MeteringDeviceBasicCharacteristicsTypeLivingRoomDevice
                    {
                        LivingRoomGUID = livingRoomGuids,
                        AccountGUID = accountGuids
                    };
                    break;
                case DeviceType.ResidentialPremiseDevice:
                    result = new MeteringDeviceBasicCharacteristicsTypeResidentialPremiseDevice
                    {
                        PremiseGUID = device.ResidentialPremises.Guid,
                        AccountGUID = accountGuids
                    };
                    break;
                case DeviceType.NonResidentialPremiseDevice:
                    result = new MeteringDeviceBasicCharacteristicsTypeNonResidentialPremiseDevice
                    {
                        PremiseGUID = device.NonResidentialPremises.Guid,
                        AccountGUID = accountGuids
                    };
                    break;
                case DeviceType.CollectiveDevice:
                    result = new MeteringDeviceBasicCharacteristicsTypeCollectiveDevice
                    {
                        //ManualModeInformation = device.ManualModeInformation,
                        //PressureSensorInformation = device.PressureSensorInformation,
                        //TemperatureSensorInformation = device.TemperatureSensorInformation,
                        Certificate = null, // нет данных
                        ProjectRegistrationNode = null // нет данных
                    };
                    break;
            }

            return result;
        }

        /// <summary>
        /// Получить объект запроса для обновляемого прибора учёта
        /// </summary>
        /// <param name="device">Прибор учёта</param>
        /// <returns>Объект запроса</returns>
        private importMeteringDeviceDataRequestMeteringDeviceDeviceDataToUpdate GetMeteringDeviceToUpdate(RisMeteringDeviceData device)
        {
            object item;

            if (device.Archivation == null)
            {
                if (this.hasValuesDeviceIds.Contains(device.Id)) // UpdateAfterDevicesValues
                {
                    item = new MeteringDeviceToUpdateAfterDevicesValuesType
                    {
                        InstallationDate = device.InstallationDate.GetValueOrDefault(),
                        InstallationDateSpecified = device.InstallationDate.HasValue,
                        //ManualModeMetering = device.ManualModeMetering.GetValueOrDefault(),
                        //ManualModeMeteringSpecified = device.ManualModeMetering.HasValue,
                        TemperatureSensor = device.TemperatureSensor.GetValueOrDefault(),
                        TemperatureSensorSpecified = device.TemperatureSensor.HasValue,
                        PressureSensor = device.PressureSensor.GetValueOrDefault(),
                        PressureSensorSpecified = device.PressureSensor.HasValue,
                        CollectiveDevice = device.DeviceType == DeviceType.CollectiveDevice ?
                            new MeteringDeviceToUpdateAfterDevicesValuesTypeCollectiveDevice
                            {
                                //ManualModeInformation = device.ManualModeInformation,
                                TemperatureSensorInformation = device.TemperatureSensorInformation,
                                PressureSensorInformation = device.PressureSensorInformation
                            } : null,
                        AccountGUID = this.accountGuidsByDeviceId?.Get(device.Id),
                        Items = this.GetMeteringDeviceTypeItems(device),
                        FirstVerificationDate = device.FirstVerificationDate.GetValueOrDefault(),
                        FirstVerificationDateSpecified = device.FirstVerificationDate.HasValue
                    };
                }
                else // UpdateBeforeDevicesValues
                {
                    item = this.GetMeteringDeviceToCreate(device);
                }
            }
            else if (device.Archivation == MeteringDeviceArchivation.NoReplacing) // ArchiveDevice
            {
                item = new importMeteringDeviceDataRequestMeteringDeviceDeviceDataToUpdateArchiveDevice
                {
                    ArchivingReason = new nsiRef
                    {
                        Code = device.ArchivingReasonCode,
                        GUID = device.ArchivingReasonGuid
                    }
                };
            }
            else // ReplaceDevice
            {
                object itemItem;

                if (device.PlannedVerification.GetValueOrDefault())
                {
                    itemItem = true;
                }
                else
                {
                    itemItem = new nsiRef
                    {
                        Code = device.ReasonVerificationCode,
                        GUID = device.ReasonVerificationGuid
                    };
                }

                item = new importMeteringDeviceDataRequestMeteringDeviceDeviceDataToUpdateReplaceDevice
                {
                    VerificationDate = device.VerificationDate.GetValueOrDefault(),
                    //SealDate = device.SealDate.GetValueOrDefault(),
                    Item = itemItem,
                    Items = this.GetMeteringValueDeviceTypeItems(device),
                    ReplacingMeteringDeviceVersionGUID = device.ReplacingMeteringDeviceVersionGuid
                };
            }

            return new importMeteringDeviceDataRequestMeteringDeviceDeviceDataToUpdate
            {
                MeteringDeviceVersionGUID = device.Guid,
                Item = item
            };
        }

        /// <summary>
        /// Получает список порций объектов ГИС для формирования объектов для запроса
        /// </summary>
        /// <returns>Список порций объектов ГИС</returns>
        private List<IEnumerable<RisMeteringDeviceData>> GetPortions()
        {
            var result = new List<IEnumerable<RisMeteringDeviceData>>();
            var meteringDeviceByFiasHouseGuidDict = this.devices.GroupBy(x => x.House.FiasHouseGuid).ToDictionary(g => g.Key, g => g.ToList());

            foreach (var deviceList in meteringDeviceByFiasHouseGuidDict.Values)
            {
                var startIndex = 0;
                do
                {
                    result.Add(deviceList.Skip(startIndex).Take(MeteringDevicePrepareDataTask.Portion));
                    startIndex += MeteringDevicePrepareDataTask.Portion;
                }
                while (startIndex < deviceList.Count);
            }

            return result;
        }
    }
}
