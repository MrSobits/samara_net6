namespace Bars.GisIntegration.Base.Tasks.PrepareData.DeviceMetering.PrepareAssistants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using DeviceMeteringAsync;
    using Entities.DeviceMetering;

    /// <summary>
    /// Помощник валидации и создания запроса контрольных показаний ПУ
    /// </summary>
    public class RisMeteringDeviceControlValueAssistant : MeteringDeviceBasePrepareAssistant<RisMeteringDeviceControlValue>
    {
        /// <summary>
        /// Метод возвращает подготовленный запрос
        /// </summary>
        /// <param name="entity">Показание ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Транспортные Guid'ы</param>
        /// <returns>Запрос</returns>
        protected override importMeteringDeviceValuesRequestMeteringDevicesValues GetRequestPartInternal(
            RisMeteringDeviceControlValue entity,
            IDictionary<string, long> deviceValuesTransportGuidDictionary)
        {
            object item;

            if (entity.MeteringDeviceData.MeteringDeviceType == MeteringDeviceType.ElectricMeteringDevice)
            {
                item = new importMeteringDeviceValuesRequestMeteringDevicesValuesElectricDeviceValue
                {
                    ControlValue = this.PrepareElectricDeviceValueControlValue(entity, deviceValuesTransportGuidDictionary)
                };
            }
            else
            {
                item = new importMeteringDeviceValuesRequestMeteringDevicesValuesOneRateDeviceValue
                {
                    ControlValue = this.PrepareOneRateDeviceValueControlValue(entity, deviceValuesTransportGuidDictionary)
                };
            }

            return new importMeteringDeviceValuesRequestMeteringDevicesValues
            {
                Item = entity.MeteringDeviceData.Guid,
                ItemElementName = ItemChoiceType.MeteringDeviceRootGUID
            };
        }

        /// <summary>
        /// Получить данные
        /// </summary>
        /// <param name="parameters">Параметры задачи</param>
        /// <returns>Список показаний</returns>
        public override List<RisMeteringDeviceControlValue> GetData(DynamicDictionary parameters)
        {
            var meteringDeviceDataDomain = this.Container.ResolveDomain<RisMeteringDeviceControlValue>();
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
        /// Выполнить валидацию сущности
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат валидации</returns>
        protected override ValidateObjectResult ValidateEntity(RisMeteringDeviceControlValue entity)
        {
            var messages = new StringBuilder();

            if (entity.MeteringDeviceData.MeteringDeviceType == MeteringDeviceType.OneRateMeteringDevice)
            {
                if (entity.MunicipalResourceCode.IsEmpty() || entity.MunicipalResourceGuid.IsEmpty())
                {
                    messages.Append("MunicipalResource ");
                }
            }

            if (entity.ReadoutDate == DateTime.MinValue)
            {
                messages.Append("DateValue ");
            }

            return new ValidateObjectResult
            {
                Id = entity.Id,
                State = messages.Length == 0 ? ObjectValidateState.Success : ObjectValidateState.Error,
                Message = messages.ToString(),
                Description = "Контрольные показания ПУ"
            };
        }

        /// <summary>
        /// Получить раздел ControlValue для OneRateDeviceValue
        /// </summary>
        /// <param name="controlValue">Контрольное значение ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>ControlValue для OneRateDeviceValue</returns>
        private importMeteringDeviceValuesRequestMeteringDevicesValuesOneRateDeviceValueControlValue[]
            PrepareOneRateDeviceValueControlValue(RisMeteringDeviceControlValue controlValue, IDictionary<string, long> deviceValuesTransportGuidDictionary)
        {
            var transportGuid = Guid.NewGuid().ToString();

            var result = new importMeteringDeviceValuesRequestMeteringDevicesValuesOneRateDeviceValueControlValue
            {
                MunicipalResource = new nsiRef
                {
                    Code = controlValue.MunicipalResourceCode,
                    GUID = controlValue.MunicipalResourceGuid
                },
                MeteringValue = controlValue.ValueT1,
                DateValue = controlValue.ReadoutDate.Date,
                TransportGUID = transportGuid
            };

            deviceValuesTransportGuidDictionary.Add(transportGuid, controlValue.Id);

            return new[] { result };
        }

        /// <summary>
        /// Получить раздел ControlValue для ElectricDeviceValue
        /// </summary>
        /// <param name="controlValue">Контрольное значение ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>ControlValue для ElectricDeviceValue</returns>
        private importMeteringDeviceValuesRequestMeteringDevicesValuesElectricDeviceValueControlValue[]
            PrepareElectricDeviceValueControlValue(RisMeteringDeviceControlValue controlValue, IDictionary<string, long> deviceValuesTransportGuidDictionary)
        {
            var transportGuid = Guid.NewGuid().ToString();

            var result = new importMeteringDeviceValuesRequestMeteringDevicesValuesElectricDeviceValueControlValue
            {
                MeteringValueT1 = controlValue.ValueT1,
                MeteringValueT2 = controlValue.ValueT2.GetValueOrDefault(),
                MeteringValueT2Specified = controlValue.ValueT2.HasValue,
                MeteringValueT3 = controlValue.ValueT3.GetValueOrDefault(),
                MeteringValueT3Specified = controlValue.ValueT3.HasValue,
                DateValue = controlValue.ReadoutDate.Date,
                TransportGUID = transportGuid
            };

            deviceValuesTransportGuidDictionary.Add(transportGuid, controlValue.Id);

            return new[] { result };
        }
    }
}