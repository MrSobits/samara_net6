namespace Bars.GisIntegration.Base.Tasks.PrepareData.DeviceMetering
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using DeviceMeteringAsync;

    using Castle.Windsor;

    /// <summary>
    /// Абстрактный помощник для работы с показаниями приборов
    /// </summary>
    /// <typeparam name="TRisEntity">Тип РИС сущности</typeparam>
    public abstract class MeteringDeviceBasePrepareAssistant<TRisEntity> : IMeteringDeviceBasePrepareAssistant<TRisEntity> where TRisEntity : BaseRisEntity
    {
        /// <summary>
        /// Поставщик данных
        /// </summary>
        public RisContragent Contragent { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Метод возвращает подготовленный запрос
        /// </summary>
        /// <param name="entity">Показание ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Транспортные Guid'ы</param>
        /// <returns>Запрос</returns>
        protected abstract importMeteringDeviceValuesRequestMeteringDevicesValues GetRequestPartInternal(TRisEntity entity, IDictionary<string, long> deviceValuesTransportGuidDictionary);

        /// <summary>
        /// Метод возвращает подготовленный запрос
        /// </summary>
        /// <param name="entity">Показание ПУ</param>
        /// <param name="deviceValuesTransportGuidDictionary">Транспортные Guid'ы</param>
        /// <returns>Запрос</returns>
        public importMeteringDeviceValuesRequestMeteringDevicesValues GetRequestPart(TRisEntity entity, IDictionary<string, long> deviceValuesTransportGuidDictionary)
        {
            return this.GetRequestPartInternal(entity, deviceValuesTransportGuidDictionary);
        }

        public importMeteringDeviceValuesRequestMeteringDevicesValues GetRequestPart(
            BaseRisEntity entity,
            IDictionary<string, long> deviceValuesTransportGuidDictionary)
        {
            return this.GetRequestPart((TRisEntity) entity, deviceValuesTransportGuidDictionary);
        }

        /// <summary>
        /// Выполнить валидацию всех подготовленных сущностей
        /// </summary>
        /// <param name="entities">Сущности</param>
        /// <returns>Результат валидации</returns>
        public List<ValidateObjectResult> ValidateData(IList<TRisEntity> entities)
        {
            var result = new List<ValidateObjectResult>();
            var objectsToRemove = new List<TRisEntity>();

            foreach (var obj in entities)
            {
                var validateResult = this.ValidateEntity(obj);

                if (validateResult.State != ObjectValidateState.Success)
                {
                    result.Add(validateResult);
                    objectsToRemove.Add(obj);
                }
            }

            foreach (var objToRemove in objectsToRemove)
            {
                entities.Remove(objToRemove);
            }

            return result;
        }

        /// <summary>
        /// Выполнить валидацию всех подготовленных сущностей
        /// </summary>
        /// <param name="entities">Сущности</param>
        /// <returns>Результат валидации</returns>
        public List<ValidateObjectResult> ValidateData(IList<BaseRisEntity> entities)
        {
            return this.ValidateData(entities.Cast<TRisEntity>().ToList());
        }

        /// <summary>
        /// Получить данные
        /// </summary>
        /// <param name="parameters">Параметры задачи</param>
        /// <returns>Список показаний</returns>
        public abstract List<TRisEntity> GetData(DynamicDictionary parameters);

        /// <summary>
        /// Выполнить валидацию сущности
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат валидации</returns>
        protected abstract ValidateObjectResult ValidateEntity(TRisEntity entity);
    }
}