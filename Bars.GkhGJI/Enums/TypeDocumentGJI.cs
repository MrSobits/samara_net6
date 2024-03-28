namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа ГЖИ
    /// </summary>
    public enum TypeDocumentGji
    {
        /// <summary>
        /// Распоряжение
        /// </summary>
        [Display("Распоряжение")]
        Disposal = 10,
        
        /// Предостережение
        /// </summary>
        [Display("Предостережение")]
        WarningDoc = 11,

        /// <summary>
        /// Мотивировочное заключение
        /// </summary>
        [Display("Мотивировочное заключение")]
        MotivationConclusion = 12,

        /// <summary>
        /// Задание
        /// </summary>
        [Display("Задание")]
        TaskDisposal = 13,

        /// <summary>
        /// Акт проверки
        /// </summary>
        [Display("Акт проверки")]
        ActCheck = 20,

        /// <summary>
        /// Акт без взаимодействия
        /// </summary>
        [Display("Акт без взаимодействия")]
        ActIsolated = 21,

        /// <summary>
        /// Акт устранения нарушений
        /// </summary>
        [Display("Акт устранения нарушений")]
        ActRemoval = 30,

        /// <summary>
        /// Акт обследования
        /// </summary>
        [Display("Акт обследования")]
        ActSurvey = 40,

        /// <summary>
        /// Предписание
        /// </summary>
        [Display("Предписание")]
        Prescription = 50,

        /// <summary>
        /// Протокол
        /// </summary>
        [Display("Протокол")]
        Protocol = 60,

        /// <summary>
        /// Постановление
        /// </summary>
        [Display("Постановление")]
        Resolution = 70,

        /// <summary>
        /// Постановление прокуратуры
        /// </summary>
        [Display("Постановление прокуратуры")]
        ResolutionProsecutor = 80,

        /// <summary>
        /// Представление
        /// </summary>
        [Display("Представление")]
        Presentation = 90,

        /// <summary>
        /// Акт визуального обследования
        /// <para>Используется в Томске</para>
        /// </summary>
        [Display("Акт визуального обследования")]
        ActVisual = 100,

        /// <summary>
        /// Административное дело
        /// <para>Используется в Томске</para>
        /// </summary>
        [Display("Административное дело")]
        AdministrativeCase = 110,

        /// <summary>
        /// Протокол МВД
        /// </summary>
        [Display("Протокол МВД")]
        ProtocolMvd = 120,

        /// <summary>
        /// Протокол МЖК
        /// </summary>
        [Display("Протокол МЖК")]
        ProtocolMhc = 130,

        /// <summary>
        /// Протокол по ст.19.7 КоАП РФ
        /// <para>Используется в НСО</para>
        /// </summary>
        [Display("Протокол по ст.19.7 КоАП РФ")]
        Protocol197 = 140,

        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        [Display("Постановление Роспотребнадзора")]
        ResolutionRospotrebnadzor = 150,
        
        /// <summary>
        /// Протокол РСО
        /// </summary>
        [Display("Протокол РСО")]
        ProtocolRSO = 190,

        [Display("Акт профилактического визита")]
        PreventiveVisit = 220,

        /// <summary>
        /// Протокол по ст.20.6.1 КоАП РФ
        /// <para>Используется в РТ</para>
        /// </summary>
        [Display("Протокол по ст.20.6.1 КоАП РФ")]
        Protocol2061 = 160,

        /// <summary>
        /// Постановление по протокол ст.20.6.1 КоАП РФ
        /// <para>Используется в РТ</para>
        /// </summary>
        [Display("Постановление по протокол ст.20.6.1 КоАП РФ")]
        Resolution2061 = 170,

        /// <summary>
        /// Задание КНМ без взаимодействия с контролируемыми лицами
        /// <para>Используется в РТ</para>
        /// </summary>
        [Display("Задание КНМ без взаимодействия с контролируемыми лицами")]
        TaskActionIsolated = 180,

        /// <summary>
        /// Профилактическое мероприятие
        /// </summary>
        [Display("Профилактическое мероприятие")]
        PreventiveAction = 191,

        /// <summary>
        /// Акт по КНМ без взаимодействия с контролируемыми лицами
        /// <para>Используется в РТ</para>
        /// </summary>
        [Display("Акт по КНМ без взаимодействия с контролируемыми лицами")]
        ActActionIsolated = 200,

        /// <summary>
        /// Лист визита
        /// </summary>
        [Display("Лист визита")]
        VisitSheet = 210,

        /// <summary>
        /// Задание профилактического мероприятия
        /// </summary>
        [Display("Задание профилактического мероприятия")]
        PreventiveActionTask = 221,

        /// <summary>
        /// Мотивированное представление
        /// </summary>
        [Display("Мотивированное представление")]
        MotivatedPresentation = 230,

        /// <summary>
        /// Решение
        /// </summary>
        [Display("Решение")]
        Decision = 240,

        /// <summary>
        /// Мотивированное представление по обращению граждан
        /// </summary>
        [Display("Мотивированное представление по обращению граждан")]
        MotivatedPresentationAppealCits = 250
    }
}