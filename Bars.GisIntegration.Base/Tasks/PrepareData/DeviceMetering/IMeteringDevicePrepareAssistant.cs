namespace Bars.GisIntegration.Base.Tasks.PrepareData.DeviceMetering
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using DeviceMeteringAsync;

    /// <summary>
    /// Помощник при извлении данных для создания запроса и его валидации.
    /// <remarks>(используется в самой задаче, т.к. она точно не знает о типе передаваемых данных)</remarks>
    /// </summary>
    public interface IMeteringDevicePrepareAssistant
    {
        /// <summary>
        /// Поставщик данных
        /// </summary>
        RisContragent Contragent { get; set; }

        /// <summary>
        /// Метод возвращает подготовленный запрос
        /// </summary>
        /// <param name="entity">Показание ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Транспортные Guid'ы</param>
        /// <returns>Запрос</returns>
        importMeteringDeviceValuesRequestMeteringDevicesValues GetRequestPart(BaseRisEntity entity, IDictionary<string, long> deviceValuesTransportGuidDictionary);

        /// <summary>
        /// Выполнить валидацию всех подготовленных сущностей
        /// </summary>
        /// <param name="entities">Сущности</param>
        /// <returns>Результат валидации</returns>
        List<ValidateObjectResult> ValidateData(IList<BaseRisEntity> entities);
    }
}