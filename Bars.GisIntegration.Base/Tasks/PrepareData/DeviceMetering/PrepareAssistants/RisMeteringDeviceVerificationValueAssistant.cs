namespace Bars.GisIntegration.Base.Tasks.PrepareData.DeviceMetering.PrepareAssistants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DeviceMeteringAsync;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.DeviceMetering;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.GisIntegration.Base.Tasks.PrepareData.DeviceMetering;

    /// <summary>
    /// Помощник валидации и создания запроса показаний проверки ПУ
    /// </summary>
    public class RisMeteringDeviceVerificationValueAssistant : MeteringDeviceBasePrepareAssistant<RisMeteringDeviceVerificationValue>
    {
        /// <summary>
        /// Метод возвращает подготовленный запрос
        /// </summary>
        /// <param name="entity">Показание ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Транспортные Guid'ы</param>
        /// <returns>Запрос</returns>
        protected override importMeteringDeviceValuesRequestMeteringDevicesValues GetRequestPartInternal(
            RisMeteringDeviceVerificationValue entity,
            IDictionary<string, long> deviceValuesTransportGuidDictionary)
        {
            object item;

            if (entity.MeteringDeviceData.MeteringDeviceType == MeteringDeviceType.ElectricMeteringDevice)
            {
                item = new importMeteringDeviceValuesRequestMeteringDevicesValuesElectricDeviceValue
                {
                    VerificationValue = this.PrepareElectricDeviceValueVerificationValue(entity, deviceValuesTransportGuidDictionary)
                };
            }
            else
            {
                item = new importMeteringDeviceValuesRequestMeteringDevicesValuesOneRateDeviceValue
                {
                    VerificationValue = this.PrepareOneRateDeviceValueVerificationValue(entity, deviceValuesTransportGuidDictionary)
                };
            }

            return new importMeteringDeviceValuesRequestMeteringDevicesValues
            {
                Item = entity.MeteringDeviceData.Guid,
                ItemElementName = ItemChoiceType.MeteringDeviceRootGUID
            };
        }

        /// <summary>
        /// Выполнить валидацию сущности
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат валидации</returns>
        protected override ValidateObjectResult ValidateEntity(RisMeteringDeviceVerificationValue entity)
        {
            var messages = new StringBuilder();

            if (entity.StartVerificationReadoutDate == DateTime.MinValue)
            {
                messages.Append("StartDateValue ");
            }

            if (entity.EndVerificationReadoutDate == DateTime.MinValue)
            {
                messages.Append("EndDateValue ");
            }

            if (!entity.SealDate.HasValue)
            {
                messages.Append("SealDate ");
            }

            if (!entity.PlannedVerification &&
                    (entity.VerificationReasonCode.IsEmpty() || entity.VerificationReasonGuid.IsEmpty()))
            {
                messages.Append("VerificationReason ");
            }

            if (entity.MeteringDeviceData.MeteringDeviceType == MeteringDeviceType.OneRateMeteringDevice)
            {
                if (entity.MunicipalResourceCode.IsEmpty() || entity.MunicipalResourceGuid.IsEmpty())
                {
                    messages.Append("MunicipalResource ");
                }
            }

            return new ValidateObjectResult
            {
                Id = entity.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Показание поверки ПУ"
            };
        }

        /// <summary>
        /// Получить данные
        /// </summary>
        /// <param name="parameters">Параметры задачи</param>
        /// <returns>Список показаний</returns>
        public override List<RisMeteringDeviceVerificationValue> GetData(DynamicDictionary parameters)
        {
            var meteringDeviceDataDomain = this.Container.ResolveDomain<RisMeteringDeviceVerificationValue>();
            var selectedHouses = parameters.GetAs("selectedList", string.Empty);
            var meteringDeviceType = parameters.GetAs<MeteringDeviceType?>("meteringDeviceType");
            var selectedIds = selectedHouses.ToUpper() == "ALL" ? null : selectedHouses.ToLongArray();

            try
            {
                var result = meteringDeviceDataDomain.GetAll()
                    .Where(x => x.Contragent.Id == this.Contragent.Id)
                    .WhereIf(selectedIds != null, x => x.MeteringDeviceData.House != null && selectedIds.Contains(x.MeteringDeviceData.House.Id))
                    .Where(x => x.Guid == null) //данные, которые ещё не выгружались
                    .WhereIf(meteringDeviceType.HasValue, x => x.MeteringDeviceData.MeteringDeviceType == meteringDeviceType.Value)
                    .ToList();

                IDictionary<string, List<BaseRisEntity>> paramDict = result
                   .GroupBy(x => x.MeteringDeviceData.House.Guid)
                   .ToDictionary(x => x.Key, x => x.Cast<BaseRisEntity>().ToList());

                parameters.SetValue("meteringDeviceValuesIdsByHouseGuid", paramDict);
                return result;
            }
            finally
            {
                this.Container.Release(meteringDeviceDataDomain);
            }
        }

        /// <summary>
        /// Получить блок VerificationValue для OneRateDeviceValue
        /// </summary>
        /// <param name="verificationValue">Показание проверки ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>VerificationValue для OneRateDeviceValue</returns>
        private importMeteringDeviceValuesRequestMeteringDevicesValuesOneRateDeviceValueVerificationValue PrepareOneRateDeviceValueVerificationValue
            (RisMeteringDeviceVerificationValue verificationValue, IDictionary<string, long> deviceValuesTransportGuidDictionary)
        {
            var transportGuid = Guid.NewGuid().ToString();

            var result = new importMeteringDeviceValuesRequestMeteringDevicesValuesOneRateDeviceValueVerificationValue
            {
                StartDateValue = verificationValue.StartVerificationReadoutDate,
                EndDateValue = verificationValue.EndVerificationReadoutDate,
                SealDate = verificationValue.SealDate.GetValueOrDefault(),
                StartValue = new[] {new OneRateMeteringValueType
                {
                    MunicipalResource = new nsiRef
                    {
                        Code = verificationValue.MunicipalResourceCode,
                        GUID = verificationValue.MunicipalResourceGuid
                    },
                    MeteringValue = verificationValue.StartVerificationValueT1
                } },
                EndValue = new[] {new OneRateMeteringValueType
                {
                    MunicipalResource = new nsiRef
                    {
                        Code = verificationValue.MunicipalResourceCode,
                        GUID = verificationValue.MunicipalResourceGuid
                    },
                    MeteringValue = verificationValue.EndVerificationValueT1
                } },
                TransportGUID = transportGuid,
                Item = this.GetVerificationType(verificationValue)
            };

            deviceValuesTransportGuidDictionary.Add(transportGuid, verificationValue.Id);

            return result;
        }

        /// <summary>
        /// Получить блок VerificationValue для ElectricDeviceValue
        /// </summary>
        /// <param name="verificationValue">Показание проверки ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>VerificationValue для ElectricDeviceValue</returns>
        private importMeteringDeviceValuesRequestMeteringDevicesValuesElectricDeviceValueVerificationValue[] PrepareElectricDeviceValueVerificationValue
            (RisMeteringDeviceVerificationValue verificationValue, IDictionary<string, long> deviceValuesTransportGuidDictionary)
        {
            var transportGuid = Guid.NewGuid().ToString();

            var result = new importMeteringDeviceValuesRequestMeteringDevicesValuesElectricDeviceValueVerificationValue
            {
                StartDateValue = verificationValue.StartVerificationReadoutDate,
                EndDateValue = verificationValue.EndVerificationReadoutDate,
                SealDate = verificationValue.SealDate.GetValueOrDefault(),
                StartValue = new ElectricMeteringValueType
                {
                    MeteringValueT1 = verificationValue.StartVerificationValueT1,
                    MeteringValueT2 = verificationValue.StartVerificationValueT2.GetValueOrDefault(),
                    MeteringValueT2Specified = verificationValue.StartVerificationValueT2.HasValue,
                    MeteringValueT3 = verificationValue.StartVerificationValueT3.GetValueOrDefault(),
                    MeteringValueT3Specified = verificationValue.StartVerificationValueT3.HasValue
                },
                EndValue = new ElectricMeteringValueType
                {
                    MeteringValueT1 = verificationValue.EndVerificationValueT1,
                    MeteringValueT2 = verificationValue.EndVerificationValueT2.GetValueOrDefault(),
                    MeteringValueT2Specified = verificationValue.EndVerificationValueT2.HasValue,
                    MeteringValueT3 = verificationValue.EndVerificationValueT3.GetValueOrDefault(),
                    MeteringValueT3Specified = verificationValue.EndVerificationValueT3.HasValue
                },
                TransportGUID = transportGuid,
                Item = this.GetVerificationType(verificationValue)
            };

            deviceValuesTransportGuidDictionary.Add(transportGuid, verificationValue.Id);

            return new[] { result };
        }

        /// <summary>
        /// Получить раздел Item для VerificationValue
        /// </summary>
        /// <param name="verificationValue">Показание проверки ПУ</param>
        /// <returns>Item для VerificationValue</returns>
        private object GetVerificationType(RisMeteringDeviceVerificationValue verificationValue)
        {
            object result;

            if (verificationValue.PlannedVerification)
            {
                result = true;
            }
            else
            {
                result = new nsiRef
                {
                    GUID = verificationValue.VerificationReasonGuid,
                    Code = verificationValue.VerificationReasonCode
                };
            }

            return result;
        }
    }
}