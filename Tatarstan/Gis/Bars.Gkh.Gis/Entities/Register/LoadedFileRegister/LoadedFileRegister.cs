namespace Bars.Gkh.Gis.Entities.Register.LoadedFileRegister
{
    using System;
    using B4.DataAccess;
    using B4.Modules.FileStorage;
    using B4.Modules.Security;

    using Bars.Gkh.Entities;

    using Enum;
    using SupplierRegister;

    /// <summary>
    /// Реестр загруженных файлов
    /// </summary>
    public class LoadedFileRegister : BaseEntity
    {
        /// <summary>
        /// Название файла
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public virtual User B4User { get; set; }

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public virtual long Size { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual TypeStatus TypeStatus { get; set; }

        /// <summary>
        /// Идентификатор файла
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Идентификатор лога
        /// </summary>
        public virtual FileInfo Log { get; set; }

        /// <summary>
        /// Наименование поставщика
        /// </summary>
        public virtual string SupplierName { get; set; }

        /// <summary>
        /// Формат загрузки
        /// </summary>
        public virtual TypeImportFormat Format { get; set; }

        /// <summary>
        /// Результат импорта
        /// </summary>
        public virtual ImportResult ImportResult { get; set; }

        /// <summary>
        /// Название импорта
        /// </summary>
        public virtual string ImportName { get; set; }


        /// <summary>
        /// Расчетный месяц
        /// </summary>
        public virtual DateTime? CalculationDate { get; set; }
    }
}