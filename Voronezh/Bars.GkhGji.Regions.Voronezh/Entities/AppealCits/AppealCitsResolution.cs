namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Резолюция АС ДОУ
    /// </summary>
    public class AppealCitsResolution : BaseGkhEntity
    {
        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Текст резолюции
        /// </summary>
        public virtual string ResolutionText { get; set; }

        /// <summary>
        /// Срок резолюции
        /// </summary>
        public virtual DateTime ResolutionTerm { get; set; }

        /// <summary>
        /// Автор резолюции
        /// </summary>
        public virtual string ResolutionAuthor { get; set; }

        /// <summary>
        /// Дата резолюции
        /// </summary>
        public virtual DateTime ResolutionDate { get; set; }

        /// <summary>
        /// Содержание резолюции, включая текст.
        /// </summary>
        public virtual string ResolutionContent { get; set; }

        /// <summary>
        /// ID АС ДОУ
        /// </summary>
        public virtual string ImportId { get; set; }

        /// <summary>
        /// Родитель (ID АС ДОУ)
        /// </summary>
        public virtual string ParentId { get; set; }

        /// <summary>
        /// отчет принят
        /// </summary>
        public virtual YesNoNotSet Executed { get; set; }
    }
}