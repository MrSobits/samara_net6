namespace Bars.GisIntegration.Base.Tasks.PrepareData.DeviceMetering
{ 
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.GisIntegration.Base.Utils;
    using Bars.Gkh.Quartz.Scheduler.Log;
    using DeviceMeteringAsync;
    using Entities.DeviceMetering;

    /// <summary>
    /// Задача подготовки данных для экстрактора показаний ПУ
    /// </summary>
    public class MeteringDeviceValuesPrepareDataTask : BasePrepareDataTask<importMeteringDeviceValuesRequest>
    {
        /// <summary>
        /// Максимальное количество записей, которые можно экспортировать одним запросом
        /// </summary>
        private const int portionSize = 1000;

        private List<BaseRisEntity> meteringDeviceValues;
        private IDictionary<string, List<BaseRisEntity>> meteringDeviceValuesIdsByHouseGuid;
        private IMeteringDevicePrepareAssistant meteringDeviceBasePrepareAssistant;

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
            var extractDeviceValuesType = MeteringDeviceValuesPrepareDataTask.GetMetteringValueType(parameters);
            this.meteringDeviceBasePrepareAssistant = this.GetAssistant(extractDeviceValuesType);
            parameters.SetValue("meteringDeviceBasePrepareAssistant", this.meteringDeviceBasePrepareAssistant);

            this.ExtractData(extractDeviceValuesType, parameters);
            this.meteringDeviceValuesIdsByHouseGuid = parameters.GetAs<IDictionary<string, List<BaseRisEntity>>>("meteringDeviceValuesIdsByHouseGuid");
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            return this.meteringDeviceBasePrepareAssistant.ValidateData(this.meteringDeviceValues);
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<importMeteringDeviceValuesRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<importMeteringDeviceValuesRequest, Dictionary<Type, Dictionary<string, long>>>();

            foreach (var part in this.meteringDeviceValuesIdsByHouseGuid)
            {
                foreach (var meteringDeviceValueSection in part.Value.Section(MeteringDeviceValuesPrepareDataTask.portionSize))
                {
                    var transportGuidDictionary = new Dictionary<Type, Dictionary<string, long>>();
                    var request = this.CreatetMeteringDeviceValuesRequest(meteringDeviceValueSection, transportGuidDictionary);

                    request.FIASHouseGuid = part.Key;
                    result.Add(request, transportGuidDictionary);
                }
            }

            return result;
        }

        /// <summary>
        /// Сформировать объект запроса
        /// </summary>
        /// <param name="deviceValues">Показания ПУ</param>
        /// <param name="transportGuidDictionary">Словарь транспотных идентификаторов</param>
        /// <returns>Объект запроса</returns>
        private importMeteringDeviceValuesRequest CreatetMeteringDeviceValuesRequest(
            IEnumerable<BaseRisEntity> deviceValues,
            IDictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var deviceValuesTransportGuidDictionary = new Dictionary<string, long>();
            var deviceValuesArray = deviceValues.ToArray();

            var deviceValuesRequest = deviceValuesArray
                .Select(deviceValue => this.meteringDeviceBasePrepareAssistant.GetRequestPart(deviceValue, deviceValuesTransportGuidDictionary))
                .ToArray();

            transportGuidDictionary.Add(this.meteringDeviceBasePrepareAssistant.GetType().GenericTypeArguments[0], deviceValuesTransportGuidDictionary);

            return new importMeteringDeviceValuesRequest
            {
                MeteringDevicesValues = deviceValuesRequest,
                Id = Guid.NewGuid().ToStr(),
                FIASHouseGuid = this.GetFiasHouseGuid(deviceValuesArray)
            };
        }

        /// <summary>
        /// Получить FiasHouseGuid
        /// </summary>
        /// <param name="deviceValues">Показания ПУ</param>
        /// <returns>FiasHouseGuid</returns>
        private string GetFiasHouseGuid(IEnumerable<BaseRisEntity> deviceValues)
        {
            var deviceValue = deviceValues.FirstOrDefault();

            return
                 (deviceValue as RisMeteringDeviceCurrentValue)?.MeteringDeviceData?.House?.FiasHouseGuid ??
                 (deviceValue as RisMeteringDeviceControlValue)?.MeteringDeviceData?.House?.FiasHouseGuid ??
                 (deviceValue as RisMeteringDeviceVerificationValue)?.MeteringDeviceData?.House?.FiasHouseGuid ??
                 string.Empty;
        }

        /// <summary>
        /// Получить тип показаний ПУ
        /// </summary>
        /// <param name="parameters">Параметры выполнения метода</param>
        /// <returns>Тип показаний ПУ</returns>
        private static Type GetMetteringValueType(DynamicDictionary parameters)
        {
            var meteringDeviceValueType = parameters.GetAs<MeteringDeviceValueType>("meteringDeviceValueType", ignoreCase: true);

            switch (meteringDeviceValueType)
            {
                case MeteringDeviceValueType.ControlValue:
                    return typeof(RisMeteringDeviceControlValue);

                case MeteringDeviceValueType.CurrentValue:
                    return typeof(RisMeteringDeviceCurrentValue);

                case MeteringDeviceValueType.VerificationValue:
                    return typeof(RisMeteringDeviceVerificationValue);

                default:
                    throw new InvalidOperationException("Не передан тип экспортируемых показаний ПУ");
            }
        }

        /// <summary>
        /// Получить данные экстрактором
        /// </summary>
        /// <param name="meteringDeviceValueType">Тип показаний ПУ</param>
        /// <param name="parameters">Параметры выполнения метода</param>
        private void ExtractData(Type meteringDeviceValueType, DynamicDictionary parameters)
        {
            var extractorGenericType = typeof(IDataExtractor<>);
            var extractorTargetType = extractorGenericType.MakeGenericType(meteringDeviceValueType);
            var extractor = this.Container.Resolve(extractorTargetType, $"MeteringDeviceValuesBaseDataExtractor.{meteringDeviceValueType.Name}");
            var extractorType = extractor.GetType();
            try
            {
                extractorType.GetProperty("Contragent").GetSetMethod()?.Invoke(extractor, new object[] { this.Contragent });
                var extractMethod = extractorType.GetMethod("Extract");
                this.meteringDeviceValues = ((IList)extractMethod.Invoke(extractor, new object[] { parameters })).Cast<BaseRisEntity>().ToList();

                this.AddLogRecord(extractorType.GetProperty("Log").GetValue(extractor, null) as List<ILogRecord> ?? new List<ILogRecord>());
            }
            finally
            {
                this.Container.Release(extractor);
            }
        }

        /// <summary>
        /// Получить конечный тип помощника для извлечения показаний ПУ
        /// </summary>
        /// <param name="meteringDeviceValueType">Тип показаний ПУ</param>
        /// <returns>Тип помощника для извлечения показаний ПУ</returns>
        private IMeteringDevicePrepareAssistant GetAssistant(Type meteringDeviceValueType)
        {
            var genericType = typeof(IMeteringDeviceBasePrepareAssistant<>);
            var assistantType = genericType.MakeGenericType(meteringDeviceValueType);
            var assistant = (IMeteringDevicePrepareAssistant)this.Container.Resolve(assistantType);
            assistant.Contragent = this.Contragent;

            return assistant;
        }
    }
}