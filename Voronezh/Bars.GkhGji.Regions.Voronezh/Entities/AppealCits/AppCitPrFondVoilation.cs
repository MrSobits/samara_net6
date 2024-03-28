namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Обращения граждан ФКР - Предписание
    /// </summary>
    public class AppCitPrFondVoilation : BaseGkhEntity
    {
        /// <summary>
        /// Предостережение
        /// </summary>
        public virtual AppealCitsPrescriptionFond AppealCitsPrescriptionFond { get; set; }

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