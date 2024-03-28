namespace Bars.Gkh.Domain
{
    using System.Collections.Generic;
    using B4;

    using Bars.Gkh.Controllers;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Расширение интерфейса <see cref="IDataResult"/>
    /// </summary>
    public static class DataResultExtensions
    {
        /// <summary>
        /// Приводит <see cref="IDataResult"/> к <see cref="ListDataResult"/>, возвращает коллекцию и количество элементов
        /// в объекте <see cref="JsonListResult"/>
        /// <para>Либо преобразовывает результат в <see cref="JsonNetResult"/></para>
        /// </summary>
        /// <returns><see cref="JsonNetResult"/></returns>
        public static ActionResult ToJsonResult(this IDataResult result)
        {
            if (result.Success && result.Data != null)
            {
                var listResult = result as ListDataResult;
                var jsResult = listResult != null
                    ? BaseGkhControllerHelper.GetJsonListResult((IEnumerable<object>)listResult.Data, listResult.TotalCount)
                    : BaseGkhControllerHelper.GetJsonNetResult(result.Data);

                return jsResult;
            }

            return result.Success
                ? JsonNetResult.Success
                : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Приводит <see cref="IDataResult"/> к <see cref="ListDataResult"/>, возвращает коллекцию и количество элементов
        /// в объекте <see cref="JsonListResult"/>
        /// <para>Либо преобразовывает результат в <see cref="JsonNetResult"/></para>
        /// </summary>
        /// <param name="result"></param>
        /// <param name="contentType">Тип контента</param>
        /// <returns><see cref="JsonNetResult"/></returns>
        public static ActionResult ToJsonResult(this IDataResult result, string contentType)
        {
            var jsResult = result.ToJsonResult() as JsonNetResult;

            jsResult.ContentType = contentType;

            return jsResult;
        }
        
        /// <summary>
        /// Использовать объект как данные в <see cref="BaseDataResult"/>
        /// </summary>
        /// <param name="data">Данные</param>
        public static BaseDataResult ToBaseDataResult(this object data)
        {
            return new BaseDataResult(data);
        }
    }
}