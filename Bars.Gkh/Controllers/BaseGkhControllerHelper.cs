namespace Bars.Gkh.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Utils;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// Хэлпер контроллера для исправления ошибки сераилизации даты
    /// </summary>
    public static class BaseGkhControllerHelper
    {
        /// <summary>
        /// Получить JSON с исправленным форматом даты
        /// </summary>
        public static JsonNetResult GetJsonNetResult(object data)
        {
            var jsResult = new JsonNetResult(data)
            {
                IgnoreDefaultConverters = true
            };

            foreach (var converter in JsonNetConvert.GetSerializationConverters(ApplicationContext.Current.Container))
            {
                var timeConverter = converter as IsoDateTimeConverter;
                if (timeConverter != null)
                {
                    timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'sszzz";
                    timeConverter.DateTimeStyles = DateTimeStyles.AssumeLocal;
                }

                var settings = (JsonSerializerSettings)jsResult.SerializerSettings ?? new JsonSerializerSettings();
                settings.Converters.Add(converter);

                jsResult.SerializerSettings = settings;
            }

            return jsResult;
        }

        /// <summary>
        /// Получить JSON с исправленным форматом даты
        /// </summary>
        public static JsonNetResult GetJsonListResult<TData>(IEnumerable<TData> data, int count = 0)
        {
            var objList = data.IsEmpty() ? new List<TData>() : data.ToList();
            var totalCount = count > 0 ? count : objList.Count;

            return BaseGkhControllerHelper.GetJsonNetResult(new
            {
                success = true,
                data,
                totalCount
            });
        }
    }
}