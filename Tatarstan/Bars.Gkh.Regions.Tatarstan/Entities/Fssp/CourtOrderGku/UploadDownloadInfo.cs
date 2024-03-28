namespace Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Информация о загрузке файла
    /// </summary>
    public class UploadDownloadInfo : BaseEntity
    {
        /// <summary>
        ///  Загруженный файл
        /// </summary>
        public virtual FileInfo DownloadFile { get; set; }

        /// <summary>
        /// Дата загрузки файла
        /// </summary>
        public virtual DateTime DateDownloadFile { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual FsspFileState Status { get; set; }

        /// <summary>
        /// Файл лога загрузки
        /// </summary>
        public virtual FileInfo LogFile { get; set; }

        /// <summary>
        /// Выгруженный файл
        /// </summary>
        public virtual FileInfo UploadFile { get; set; }
    }
}