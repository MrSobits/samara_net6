namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип результата мотивированного представления
    /// </summary>
    public enum MotivatedPresentationResultType
    {
        /// <summary>
        /// Требуется провести контрольно-надзорное мероприятие
        /// </summary>
        [Display("Требуется провести контрольно-надзорное мероприятие")]
        NeedKnmExecuting = 1,

        /// <summary>
        /// Требуется объявить предостережение
        /// </summary>
        [Display("Требуется объявить предостережение")]
        NeedWarningDocAnnouncement = 2,

        /// <summary>
        /// Отсутствуют основания для проведения контрольно-надзорного мероприятия
        /// </summary>
        [Display("Отсутствуют основания для проведения контрольно-надзорного мероприятия")]
        NoBaseKnmExecuting = 3
    }
}