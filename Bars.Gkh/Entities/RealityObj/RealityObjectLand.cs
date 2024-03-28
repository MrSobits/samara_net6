namespace Bars.Gkh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Земельный участок жилого дома
    /// </summary>
    public class RealityObjectLand : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public virtual DateTime? DateLastRegistration { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public virtual string CadastrNumber { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}
