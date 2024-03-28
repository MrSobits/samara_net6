namespace Sobits.GisGkh.File.Impl
{
    using Castle.Windsor;

    /// <summary>
    /// Базовый класс скачивания файлов
    /// </summary>
    public abstract class BaseFileDownloader : WebUtilsService, IFileDownloader
    {
        /// <summary>
        /// Конструктор класса скачивания файлов
        /// </summary>
        /// <param name="container">IoC Container</param>
        protected BaseFileDownloader(IWindsorContainer container)
        {
            this.Container = container;
            //this.FileServiceProvider = this.Container.Resolve<IGisServiceProvider>("FileServiceProvider");
        }

        /// <summary>
        /// Получить файл с рест-сервиса
        /// </summary>
        /// <param name="fileGuid">Гуид файла</param>
        /// <param name="orgPpaGuid">Идентификатор зарегистрированной организации</param>
        /// <returns>Файл</returns>
        public abstract FileDownloadResult DownloadFile(string fileGuid, string orgPpaGuid);
    }
}