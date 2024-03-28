using System;
using System.IO;
using System.Net;
using Castle.Windsor;
using GisGkhLibrary.Enums;
using GisGkhLibrary.Services;

namespace Sobits.GisGkh.File.Impl
{
    public class WebUtilsService
    {
        //5 МБ = 5242880 Б 
        protected const int FilePartSize = 5242880;

        /// <summary>
        /// IoC Container
        /// </summary>
        protected IWindsorContainer Container { get; set; }

        /// <summary>
        /// Service provider для файлового сервиса
        /// </summary>
        protected FileServiceProvider FileServiceProvider { get; set; }

        ///// <summary>
        ///// Создать веб реквест
        ///// </summary>
        //protected HttpWebRequest CreateRequest(GisFileRepository repository, string requestUrl)
        //{
        //    try
        //    {
        //        var result = (HttpWebRequest)WebRequest.Create(requestUrl);
        //        ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;

        //        result.Date = DateTime.Now;

        //        return result;
        //    }
        //    finally
        //    {
        //        this.Container.Release(gisIntegrationConfig);
        //    }
        //}

        /// <summary>
        /// Получить хэш файла по алгоритму МД5
        /// </summary>
        /// <param name="stream">Поток данных</param>
        /// <returns>Хэш файла</returns>
        protected string GetMd5Hash(Stream stream)
        {
            if (stream.Position != 0) stream.Seek(0, SeekOrigin.Begin);

            try
            {
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    return Convert.ToBase64String(md5.ComputeHash(stream));
                }
            }
            finally
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.Flush();
            }
        }

        protected string GetMd5Hash(byte[] inputArray)
        {
            byte[] hash;

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = md5.ComputeHash(inputArray);
            }

            return Convert.ToBase64String(hash);
        }

        protected string GetMd5Hash(byte[] inputArray, int offset, int count)
        {
            byte[] hash;

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                hash = md5.ComputeHash(inputArray, offset, count);
            }

            return Convert.ToBase64String(hash);
        }
    }
}