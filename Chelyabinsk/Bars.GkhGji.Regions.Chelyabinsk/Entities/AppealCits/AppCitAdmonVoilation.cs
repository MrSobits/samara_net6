namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Обращениям граждан - Предостережение
    /// </summary>
    public class AppCitAdmonVoilation : BaseGkhEntity
    {
        /// <summary>
        /// Предостережение
        /// </summary>
        public virtual AppealCitsAdmonition AppealCitsAdmonition { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual ViolationGji ViolationGji { get; set; }

        /// <summary>
        /// Планируемая дата устранения
        /// </summary>
        public virtual DateTime? PlanedDate { get; set; }

        /// <summary>
        /// Фактическая дата устранения
        /// </summary>
        public virtual DateTime? FactDate { get; set; }
    }
}