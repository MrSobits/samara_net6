namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Common
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Newtonsoft.Json;

    /// <summary>
    /// Модель получения "Информация о файле"
    /// </summary>
    public class FileInfoGet : BaseFileInfo
    {
    }

    /// <summary>
    /// Модель создания "Информация о файле"
    /// </summary>
    public class FileInfoCreate : BaseFileInfo
    {
        /// <inheritdoc />
        [JsonIgnore]
        public override long? Id { get; set; }

    }

    /// <summary>
    /// Модель обновления "Информация о файле"
    /// </summary>
    public class FileInfoUpdate : BaseFileInfo
    {
    }

    /// <summary>
    /// Базовая модель "Информация о файле"
    /// </summary>
    public class BaseFileInfo : INestedEntityId
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public virtual long? Id { get; set; }

        /// <summary>
        /// Уникальный идентификатор файла
        /// </summary>
        [RequiredExistsEntity(typeof(FileInfo))]
        public virtual long? FileId { get; set; }

        /// <summary>
        /// Наименование файла
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        /// Дата файла
        /// </summary>
        public virtual DateTime? FileDate { get; set; }

        /// <summary>
        /// Описание файла
        /// </summary>
        public virtual string FileDescription { get; set; }
    }
}