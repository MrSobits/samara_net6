namespace Bars.Gkh.Reforma.Impl.DataCollectors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Entities.Dict;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.ReformaService;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Работа с хранимыми файлами (базовая реализация)
    /// </summary>
    public abstract class CollectedFileBase
    {
        /// <summary>
        /// Разрешённые типы файлов
        /// </summary>
        public static readonly string[] AllowedExtensions =
            {
                "odt",
                "ods",
                "odp",
                "doc",
                "docx",
                "xls",
                "xlsx",
                "ppt",
                "pptx",
                "txt",
                "dat",
                "jpg",
                "jpeg",
                "png",
                "pdf",
                "gif",
                "tif",
                "rtf"
            };

        /// <summary>
        /// Контейнер
        /// </summary>
        protected readonly IWindsorContainer Container;

        /// <summary>
        /// Период раскрытия информации в Реформе
        /// </summary>
        protected ReportingPeriodDict Period;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container">Контейнер</param>
        /// <param name="period">Период раскрытия информации в Реформе</param>
        protected CollectedFileBase(IWindsorContainer container, ReportingPeriodDict period)
        {
            this.Container = container;
            this.Period = period;
        }

        /// <summary>
        /// Загрузить файл
        /// </summary>
        /// <param name="fileInfo">Файл</param>
        /// <param name="refFileService">Домен-сервис для работы с файлами реформы</param>
        /// <param name="syncProvider">Провайдер синхронизации с Реформой ЖКХ</param>
        /// <returns>Идентификатор файла в реформе</returns>
        protected int Upload(FileInfo fileInfo, IDomainService<RefFile> refFileService, ISyncProvider syncProvider)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            try
            {
                if (fileInfo.Size > 15 * 1024 * 1024)
                {
                    syncProvider.Logger.AddMessage(
                        string.Format("Загрузка {0}: Размер файла больше 15Мб", fileInfo.FullName));
                    return 0;
                }

                if (!AllowedExtensions.Contains(fileInfo.Extention.ToLower()))
                {
                    syncProvider.Logger.AddMessage(
                        string.Format(
                            "Загрузка {0}: Недопустимый тип файла: {1}",
                            fileInfo.Extention,
                            fileInfo.FullName));
                    return 0;
                }

                var content = fileManager.GetBase64String(fileInfo);

                var id = this.UploadContent(fileInfo.FullName, content, syncProvider);
                if (id == 0)
                {
                    return 0;
                }

                refFileService.Save(new RefFile { FileInfo = fileInfo, ExternalId = id, ReportingPeriod = this.Period });

                return id;
            }
            catch (Exception e)
            {
                syncProvider.Logger.AddMessage(string.Format("{0}: Исключение: {1}", fileInfo.FullName, e));
                return 0;
            }
            finally
            {
                this.Container.Release(fileManager);
            }
        }

        /// <summary>
        /// Выполнить метод SetUploadFile и загрузить файл
        /// </summary>
        /// <param name="name">Имя файла</param>
        /// <param name="content">Данные</param>
        /// <param name="syncProvider">Провайдер синхронизации с Реформой ЖКХ</param>
        /// <returns>Идентификатор файла</returns>
        protected int UploadContent(string name, string content, ISyncProvider syncProvider)
        {
            try
            {
                var result =
                    syncProvider.Client.SetUploadFile(new FileObject { name = name, data = content });

                return result.file_id;
            }
            catch (Exception e)
            {
                syncProvider.Logger.AddMessage(string.Format("{0}: Исключение: {1}", name, e));
                return 0;
            }
        }
    }
}