namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Базовый докумет
    /// </summary>
    public class BaseDocument : BaseGkhEntity
    {
        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Дата от
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }
    }
}
