namespace Bars.GisIntegration.Base.Entities.External.Attachment
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using External.Administration.System;

    /// <summary>
    /// Приложение
    /// </summary>
    public class ExtAttachment : BaseEntity
    {
        /// <summary>
        /// Поставщик информации
        /// </summary>
        public virtual DataSupplier DataSupplier { get; set; }
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
        /// <summary>
        /// Комментарий
        /// </summary>
        public virtual string Note { get; set; }
        /// <summary>
        /// ГУИД
        /// </summary>
        public virtual string Guid { get; set; }
        /// <summary>
        /// Хэш
        /// </summary>
        public virtual string Hash { get; set; }
        /// <summary>
        /// Приложение
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }
        /// <summary>
        /// Пользователь 
        /// </summary>
        public virtual int ChangedBy { get; set; }
        /// <summary>
        /// Дата изменения 
        /// </summary>
        public virtual DateTime ChangedOn { get; set; }
    }
}
