namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums
{
    using Bars.B4.Utils;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Типы документов печатных форм
    /// </summary>
    /// <remarks>
    /// !!! Наименования соответствуют Id из <see cref="IGkhBaseReport"/> !!!
    /// </remarks>
    public enum PrintedFormDocumentType
    {
        /// <summary>
        /// Решение
        /// </summary>
        [Display("Решение")]
        [CustomValue(nameof(TypeDocumentGji), TypeDocumentGji.Decision)]
        Decision = 1,

        /// <summary>
        /// Акт проверки
        /// </summary>
        [Display("Акт проверки")]
        [CustomValue(nameof(TypeDocumentGji), TypeDocumentGji.ActCheck)]
        ActCheckTat = 2,

        /// <summary>
        /// Предписание
        /// </summary>
        [Display("Предписание")]
        [CustomValue(nameof(TypeDocumentGji), TypeDocumentGji.Prescription)]
        Prescription = 3,

        /// <summary>
        /// Протокол
        /// </summary>
        [Display("Протокол")]
        [CustomValue(nameof(TypeDocumentGji), TypeDocumentGji.Protocol)]
        Protocol = 4,

        /// <summary>
        /// Задание КНМ без взаимодействия с контролируемыми лицами
        /// </summary>
        [Display("Задание КНМ без взаимодействия с контролируемыми лицами")]
        [CustomValue(nameof(TypeDocumentGji), TypeDocumentGji.TaskActionIsolated)]
        TaskAction = 5,

        /// <summary>
        /// Акт КНМ без взаимодействия с контролируемыми лицами
        /// </summary>
        [Display("Акт КНМ без взаимодействия с контролируемыми лицами")]
        [CustomValue(nameof(TypeDocumentGji), TypeDocumentGji.ActActionIsolated)]
        ActActionIsolated = 6,

        /// <summary>
        /// Мотивированное представление
        /// </summary>
        [Display("Мотивированное представление")]
        [CustomValue(nameof(TypeDocumentGji), TypeDocumentGji.MotivatedPresentation)]
        MotivatedPresentation = 7,

        /// <summary>
        /// Задание профилактического мероприятия
        /// </summary>
        [Display("Задание профилактического мероприятия")]
        [CustomValue(nameof(TypeDocumentGji), TypeDocumentGji.PreventiveActionTask)]
        PreventiveActionTaskSheet = 8,

        /// <summary>
        /// Лист визита
        /// </summary>
        [Display("Лист визита")]
        [CustomValue(nameof(TypeDocumentGji), TypeDocumentGji.VisitSheet)]
        PreventiveActionVisitSheet = 9,

        /// <summary>
        /// Действие акта проверки
        /// </summary>
        [Display("Действие акта проверки")]
        ProtocolActAction = 10,
        
        /// <summary>
        /// Уведомление о составлении протокола
        /// </summary>
        [Display("Уведомление о составлении протокола")]
        [CustomValue(nameof(TypeDocumentGji), TypeDocumentGji.ActCheck)]
        ProtocolNotice = 11,
    }
}