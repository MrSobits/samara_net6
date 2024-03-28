namespace Bars.GisIntegration.Base.File.Impl
{
    using System;
    
    using System.Net;

    using Bars.B4.Modules.FileStorage;
    using Bars.GisIntegration.Base.GisServiceProvider;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Базовый загрузчик файлов
    /// </summary>
    public abstract class BaseFileUploader : IFileUploader
    {
        /// <summary>
        /// IoC Container
        /// </summary>
        public IWindsorContainer Container { get; }

        /// <summary>
        /// Service provider для файлового сервиса
        /// </summary>
        public IGisServiceProvider FileServiceProvider { get;}

        /// <summary>
        /// Конструктор загрузчика файлов
        /// </summary>
        /// <param name="container">IoC Container</param>
        protected BaseFileUploader(IWindsorContainer container)
        {
            this.Container = container;
            this.FileServiceProvider = this.Container.Resolve<IGisServiceProvider>("FileServiceProvider");
        }

        /// <summary>
        /// Отправить файл на рест-сервис
        /// </summary>
        /// <param name="fileInfo">Файл</param>
        /// <param name="orgPpaGuid">Идентификатор зарегистрированной организации</param>
        /// <returns>Результат загрузки файла</returns>
        public abstract FileUploadResult UploadFile(FileInfo fileInfo, string orgPpaGuid);

        protected WebRequest CreateRequest(string requestUrl)
        {
            var gisIntegrationConfig = this.Container.Resolve<GisIntegrationConfig>();
           
            try
            {
                var result = (HttpWebRequest)WebRequest.Create(requestUrl);
                ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;

                if (gisIntegrationConfig.UseLoginCredentials)
                {
                    result.Credentials = new NetworkCredential(gisIntegrationConfig.Login, gisIntegrationConfig.Password);
                }

                result.Date = DateTime.Now;

                return result;
            }
            finally
            {
                this.Container.Release(gisIntegrationConfig);
            }
        }

        /// <summary>
        /// Получить хэш файла по алгоритму МД5
        /// </summary>
        /// <param name="fileInfo">Файл</param>
        /// <returns>Хэш файла</returns>
        protected string GetMd5Hash(FileInfo fileInfo)
        {
            var fileManager = this.Container.Resolve<IFileManager>();

            try
            {
                byte[] hash;

                using (var stream = fileManager.GetFile(fileInfo))
                {
                    using (var md5 = System.Security.Cryptography.MD5.Create())
                    {
                        hash = md5.ComputeHash(stream);
                    }
                }

                return Convert.ToBase64String(hash);
            }
            finally
            {
                this.Container.Release(fileManager);
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
