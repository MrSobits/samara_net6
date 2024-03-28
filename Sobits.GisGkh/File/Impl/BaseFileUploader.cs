namespace Sobits.GisGkh.File.Impl
{
    using Castle.Windsor;
    using Bars.B4.Modules.FileStorage;
    using GisGkhLibrary.Enums;

    /// <summary>
    /// Базовый загрузчик файлов
    /// </summary>
    public abstract class BaseFileUploader : WebUtilsService, IFileUploader
    {
        /// <summary>
        /// Конструктор загрузчика файлов
        /// </summary>
        /// <param name="container">IoC Container</param>
        protected BaseFileUploader(IWindsorContainer container)
        {
            this.Container = container;
            //this.FileServiceProvider = this.Container.Resolve<IGisServiceProvider>("FileServiceProvider");
        }

        /// <summary>
        /// Отправить файл на рест-сервис
        /// </summary>
        /// <param name="repository">Хранилище ГИС ЖКХ</param>
        /// <param name="fileInfo">Файл</param>
        /// <param name="orgPpaGuid">Идентификатор зарегистрированной организации</param>
        /// <returns>Результат загрузки файла</returns>
        public abstract FileUploadResult UploadFile(GisFileRepository repository, FileInfo fileInfo, string orgPpaGuid);
    }
}