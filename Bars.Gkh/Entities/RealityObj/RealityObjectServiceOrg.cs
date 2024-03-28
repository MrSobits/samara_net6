namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Поставщик коммунальных услуг жилого дома
    /// </summary>
    public class RealityObjectServiceOrg : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Название документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Контрагент поставщика
        /// </summary>
        public virtual Contragent Organization { get; set; }

        /// <summary>
        /// Тип поставщика услуги
        /// </summary>
        public virtual TypeServiceOrg TypeServiceOrg { get; set; }
    }
}
