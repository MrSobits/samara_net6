namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities.Base;
    using Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits;

    /// <summary>
    /// Приложение мотивированного представления по обращению гражданина
    /// </summary>
    public class MotivatedPresentationAppealCitsAnnex : BaseEntity, IAnnexEntity
    {
        /// <summary>
        /// Мотивированное представление 
        /// </summary>
        public virtual MotivatedPresentationAppealCits MotivatedPresentation { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}