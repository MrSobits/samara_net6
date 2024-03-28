namespace Bars.GisIntegration.Base.Tasks.PrepareData.DeviceMetering.PrepareAssistants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.DeviceMetering;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.Tasks.PrepareData;

    using DeviceMeteringAsync;
    using DeviceMetering;

    /// <summary>
    /// Помощник валидации и создания запроса текущих показаний ПУ
    /// </summary>
    public class RisMeteringDeviceCurrentValueAssistant : MeteringDeviceBasePrepareAssistant<RisMeteringDeviceCurrentValue>
    {
        /// <summary>
        /// Метод возвращает подготовленный запрос
        /// </summary>
        /// <param name="entity">Показание ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Транспортные Guid'ы</param>
        /// <returns>Запрос</returns>
        protected override importMeteringDeviceValuesRequestMeteringDevicesValues GetRequestPartInternal(
            RisMeteringDeviceCurrentValue entity,
            IDictionary<string, long> deviceValuesTransportGuidDictionary)
        {
            object item;

            if (entity.MeteringDeviceData.MeteringDeviceType == MeteringDeviceType.ElectricMeteringDevice)
            {
                item = new importMeteringDeviceValuesRequestMeteringDevicesValuesElectricDeviceValue
                {
                    CurrentValue = this.PrepareElectricDeviceValueCurrentValue(entity, deviceValuesTransportGuidDictionary)
                };
            }
            else
            {
                item = new importMeteringDeviceValuesRequestMeteringDevicesValuesOneRateDeviceValue
                {
                    CurrentValue = this.PrepareOneRateDeviceValueCurrentValue(entity, deviceValuesTransportGuidDictionary)
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
        protected override ValidateObjectResult ValidateEntity(RisMeteringDeviceCurrentValue entity)
        {
            var messages = new StringBuilder();

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
                Description = "Текущие показания ПУ"
            };
        }

        /// <summary>
        /// Получить данные
        /// </summary>
        /// <param name="parameters">Параметры задачи</param>
        /// <returns>Список показаний</returns>
        public override List<RisMeteringDeviceCurrentValue> GetData(DynamicDictionary parameters)
        {
            var meteringDeviceDataDomain = this.Container.ResolveDomain<RisMeteringDeviceCurrentValue>();
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
        /// Получить раздел CurrentValue для OneRateDeviceValue
        /// </summary>
        /// <param name="currentValue">Текущие показания ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>CurrentValue для OneRateDeviceValue</returns>
        private importMeteringDeviceValuesRequestMeteringDevicesValuesOneRateDeviceValueCurrentValue[] PrepareOneRateDeviceValueCurrentValue(RisMeteringDeviceCurrentValue currentValue, IDictionary<string, long> deviceValuesTransportGuidDictionary)
        {
            var transportGuid = Guid.NewGuid().ToString();

            var result = new importMeteringDeviceValuesRequestMeteringDevicesValuesOneRateDeviceValueCurrentValue
            {
                MeteringValue = currentValue.ValueT1,
                MunicipalResource = new nsiRef
                {
                    Code = currentValue.MunicipalResourceCode,
                    GUID = currentValue.MunicipalResourceGuid
                },
                TransportGUID = transportGuid
            };

            deviceValuesTransportGuidDictionary.Add(transportGuid, currentValue.Id);

            return new[] { result };
        }

        /// <summary>
        /// Получить раздел CurrentValue для ElectricDeviceValue
        /// </summary>
        /// <param name="currentValue">Текущие показания ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns> CurrentValue для ElectricDeviceValue</returns>
        private importMeteringDeviceValuesRequestMeteringDevicesValuesElectricDeviceValueCurrentValue[] PrepareElectricDeviceValueCurrentValue(RisMeteringDeviceCurrentValue currentValue, IDictionary<string, long> deviceValuesTransportGuidDictionary)
        {
            var transportGuid = Guid.NewGuid().ToString();

            var result = new importMeteringDeviceValuesRequestMeteringDevicesValuesElectricDeviceValueCurrentValue
            {
                MeteringValueT1 = currentValue.ValueT1,
                MeteringValueT2 = currentValue.ValueT2.GetValueOrDefault(),
                MeteringValueT2Specified = currentValue.ValueT2.HasValue,
                MeteringValueT3 = currentValue.ValueT3.GetValueOrDefault(),
                MeteringValueT3Specified = currentValue.ValueT3.HasValue,
                TransportGUID = transportGuid
            };

            deviceValuesTransportGuidDictionary.Add(transportGuid, currentValue.Id);

            return new[] { result };
        }
    }
}