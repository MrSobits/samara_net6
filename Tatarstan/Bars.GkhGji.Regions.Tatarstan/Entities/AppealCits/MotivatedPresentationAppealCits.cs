namespace Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    /// <summary>
    /// Мотивированное представление по обращению гражданина
    /// </summary>
    public class MotivatedPresentationAppealCits : DocumentGji
    {
        /// <summary>
        /// Обращение гражданина
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Вид мотивированного представления
        /// </summary>
        public virtual MotivatedPresentationType? PresentationType { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public virtual Inspector Official { get; set; }

        /// <summary>
        /// Тип результата мотивированного представления
        /// </summary>
        public virtual MotivatedPresentationResultType? ResultType { get; set; }
    }
}