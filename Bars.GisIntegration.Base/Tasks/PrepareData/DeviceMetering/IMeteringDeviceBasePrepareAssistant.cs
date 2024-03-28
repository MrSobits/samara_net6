namespace Bars.GisIntegration.Base.Tasks.PrepareData.DeviceMetering
{
    using System.Collections.Generic;
    
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using DeviceMeteringAsync;

    /// <summary>
    /// Помощник при извлении данных для создания запроса и его валидации
    /// </summary>
    /// <typeparam name="TRisEntity">Тип сущности</typeparam>
    public interface IMeteringDeviceBasePrepareAssistant<TRisEntity> : IMeteringDevicePrepareAssistant where TRisEntity : BaseRisEntity
    {
        /// <summary>
        /// Метод возвращает подготовленный запрос
        /// </summary>
        /// <param name="entity">Показание ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Транспортные Guid'ы</param>
        /// <returns>Запрос</returns>
        importMeteringDeviceValuesRequestMeteringDevicesValues GetRequestPart(TRisEntity entity, IDictionary<string, long> deviceValuesTransportGuidDictionary);

        /// <summary>
        /// Выполнить валидацию всех подготовленных сущностей
        /// </summary>
        /// <param name="entities">Сущности</param>
        /// <returns>Результат валидации</returns>
        List<ValidateObjectResult> ValidateData(IList<TRisEntity> entities);

        /// <summary>
        /// Получить данные
        /// </summary>
        /// <param name="parameters">Параметры запроса</param>
        /// <returns>Список показаний</returns>
        List<TRisEntity> GetData(DynamicDictionary parameters);
    }
}