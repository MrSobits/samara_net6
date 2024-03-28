using Bars.B4.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Sobits.GisGkh.File
{
    /// <summary>
    /// Результат загрузки/выгрузки файла
    /// </summary>
    public abstract class FileLoadResult
    {
        /// <summary>
        /// Маппинг исключений ГИС
        /// </summary>
        protected abstract IDictionary<string, string> ExceptionMap { get; }

        /// <summary>
        /// Результат скачивания: успешно - true, в противном случае false
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// Конструктор результата скачивания файла
        /// </summary>
        protected FileLoadResult()
        {
            Success = true;
        }

        /// <summary>
        /// Конструктор результата скачивания файла
        /// </summary>
        /// <param name="exception">Исключение при скачивании файла</param>
        protected FileLoadResult(Exception exception)
        {
            Success = false;
            Message = GetErrorMessage(exception);
        }

        private string GetErrorMessage(Exception exception)
        {
            if (exception is AggregateException aggregateException)
            {
                return string.Join("; ", aggregateException.InnerExceptions.Select(GetErrorMessage));
            }

            var result = string.Empty;

            if (exception is WebException webException)
            {
                var gisException = webException.Response?.Headers.Get("X-Upload-Error");
                if (!string.IsNullOrWhiteSpace(gisException))
                {
                    result = $"{exception.Message} ({gisException}: {ExceptionMap.Get(gisException, "Отсутствует описание полученной ошибки")})";
                }
            }


            if (string.IsNullOrEmpty(result))
            {
                result = exception.Message;
            }

            return result;
        }

        private string GetErrorMessage(string errorValue)
        {
            if (errorValue != null)
            {
                foreach (var key in ExceptionMap)
                {
                    if (errorValue.Contains(key.Key)) return ExceptionMap[key.Key];
                }
            }
            return string.Empty;
        }
    }
}