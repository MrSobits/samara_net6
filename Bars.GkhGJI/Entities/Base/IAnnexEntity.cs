namespace Bars.GkhGji.Entities.Base
{
    using System;

    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Интерфейс сущности-приложения
    /// </summary>
    public interface IAnnexEntity
    {
        /// <summary>
        /// Дата документа
        /// </summary>
        DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        FileInfo File { get; set; }
    }
}