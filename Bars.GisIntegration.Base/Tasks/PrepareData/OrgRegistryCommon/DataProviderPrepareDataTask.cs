namespace Bars.GisIntegration.Base.Tasks.PrepareData.OrgRegistryCommon
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.OrgRegistryCommonAsync;

    /// <summary>
    /// Задача подготовки сведений о поставщиках информации
    /// </summary>
    [Obsolete("Метод exportDataProvider упразднен")]
    public class DataProviderPrepareDataTask : BasePrepareDataTask<exportDataProviderRequest>
    {
        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры экспорта</param>
        protected override void ExtractData(DynamicDictionary parameters)
        {
        }

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected override List<ValidateObjectResult> ValidateData()
        {
            return new List<ValidateObjectResult>();
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected override Dictionary<exportDataProviderRequest, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var result = new Dictionary<exportDataProviderRequest, Dictionary<Type, Dictionary<string, long>>>();

            var request = new exportDataProviderRequest
            {
                IsActual = true,
                IsActualSpecified = true
            };

            if (this.DataForSigning)
            {
                request.Id = Guid.NewGuid().ToString();
            }

            result.Add(request, new Dictionary<Type, Dictionary<string, long>>());

            return result;
        }
    }
}