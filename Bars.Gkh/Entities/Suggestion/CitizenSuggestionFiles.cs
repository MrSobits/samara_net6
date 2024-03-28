namespace Bars.Gkh.Entities.Suggestion
{
    using System;

    using B4.Modules.FileStorage;

    /// <summary>
    /// Файл обращения граждан
    /// </summary>
    public class CitizenSuggestionFiles : BaseGkhEntity
    {
        public virtual CitizenSuggestion CitizenSuggestion { get; set; }

        public virtual FileInfo DocumentFile { get; set; }

        public virtual string DocumentNumber { get; set; }

        public virtual DateTime? DocumentDate { get; set; }

        public virtual bool isAnswer { get; set; }

        public virtual string Description { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Hash
        /// </summary>
        public virtual string Hash { get; set; }

        /// <summary>
        /// ГИС ЖКХ Guid
        /// </summary>
        public virtual string GisGkhGuid { get; set; }
    }
}