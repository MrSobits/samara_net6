namespace Bars.GkhGji.Regions.Habarovsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип логируемой сущности
    /// </summary>

    public enum TypeEntityLogging
    {
        /// <summary>
        /// Реестр обращений
        /// </summary>
        [Display("Реестр обращений")]
        AppealCits = 1,

        /// <summary> 
        /// Реестр обращений - Ответ по обращению
        /// </summary>
        [Display("Реестр обращений - Ответ по обращению")]
        AppealCitsAnswer = 2,

        /// <summary>
        /// Реестр обращений - Место возникновения проблемы
        /// </summary>
        [Display("Реестр обращений - Место возникновения проблемы")]
        AppealCitsRealityObject = 3,

        /// <summary>
        /// Реестр обращений - Предостережение
        /// </summary>
        [Display("Реестр обращений - Предостережение")]
        AppealCitsAdmonition = 46,

        /// <summary>
        /// Реестр обращений - Решение
        /// </summary>
        [Display("Реестр обращений - Решение")]
        AppealCitsDecision = 47,

        /// <summary>
        /// Реестр обращений - Запрос
        /// </summary>
        [Display("Реестр обращений - Запрос")]
        AppealCitsRequest = 48,

        /// <summary>
        /// Реестр обращений - Источник
        /// </summary>
        [Display("Реестр обращений - Источник")]
        AppealCitsSource = 49,

        /// <summary>
        /// Реестр обращений - Тематика
        /// </summary>
        [Display("Реестр обращений - Тематика")]
        AppealCitsStatSubject = 50,

        /// <summary>
        /// Реестр обращений - Исполнитель
        /// </summary>
        [Display("Реестр обращений - Исполнитель")]
        AppealCitsExecutant = 51,

        /// <summary>
        /// Реестр обращений - Связанное обращение
        /// </summary>
        [Display("Реестр обращений - Связанное обращение")]
        RelatedAppealCits = 52,

        /// <summary>
        /// Реестр обращений - Вопросы
        /// </summary>
        [Display("Реестр обращений - Вопросы")]
        AppealCitsQuestion = 53,

        /// <summary>
        /// Реестр обращений - Инспектор
        /// </summary>
        [Display("Реестр обращений - Инспектор")]
        AppealCitsHeadInspector = 54,

        /// <summary>
        /// Реестр обращений - Определение
        /// </summary>
        [Display("Реестр обращений - Определение")]
        AppealCitsDefinition = 55,

        /// <summary>
        /// Реестр обращений - Категория заявителя
        /// </summary>
        [Display("Реестр обращений - Категория заявителя")]
        AppealCitsCategory = 56,

        /// <summary>
        /// Реестр обращений - Приложение
        /// </summary>
        [Display("Реестр обращений - Приложение")]
        AppealCitsAttachment = 57
    }
}
