namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Этап проверки ГЖИ
    /// </summary>
    public enum TypeStage
    {
        [Display("Распоряжение")]
        Disposal = 10,

        // TODO : Разобраться какое значение нужно
      /*  [Display("Решение")]
        Decision = 15,*/
      
        [Display("Распоряжение проверки предписания")]
        DisposalPrescription = 20,
        
        [Display("Предостережение")]
        WarningDoc = 11,

        [Display("Мотивировочное заключение")]
        MotivationConclusion = 12,

        [Display("Задание")]
        TaskDisposal = 13,
        
        // TODO : Разобраться какое значение нужно
       /* [Display("Решение проверки предписания")]
        DecisionPrescription = 25,*/

        [Display("Акт проверки")]
        ActCheck = 30,

        [Display("Акт проверки (общий)")]
        ActCheckGeneral = 40,

        [Display("Акт без взаимодействия")]
        ActIsolated = 41,

        [Display("Акт обследования")]
        ActSurvey = 50,

        [Display("Предписание")]
        Prescription = 60,

        [Display("Протокол")]
        Protocol = 70,

        [Display("Постановление")]
        Resolution = 80,

        [Display("Акт проверки предписания")]
        ActRemoval = 90,

        [Display("Постановление прокуратуры")]
        ResolutionProsecutor = 100,

        [Display("Представление")]
        Presentation = 110,

        [Display("Акт визуального осмотра")] // Тип используется в Томске
        ActVisual = 120,

        [Display("Административное дело")] // Тип используется в Томске
        AdministrativeCase = 130,

        [Display("Протокол МВД")]
        ProtocolMvd = 140,

        [Display("Протокол МЖК")]
        ProtocolMhc = 150,

        [Display("Акт осмотра")]
        ActView = 160,

        [Display("Протокол по ст.19.7 КоАП РФ")] // Тип используется в НСО
        Protocol197 = 170,

        [Display("Постановление Роспотребнадзора")]
        ResolutionRospotrebnadzor = 180,
        

        [Display("Акт профилактического визита")]
        PreventiveVisit = 220,
        
        /// <summary>
        /// Протокол ГЖИ
        /// для РТ
        /// </summary>
        [Display("Протокол ГЖИ")]
        ProtocolGji = 190,

        /// <summary>
        /// Постановление протокола ГЖИ
        /// для РТ
        /// </summary>
        [Display("Постановление протокола ГЖИ")]
        ResolutionGji = 200,

        /// <summary>
        /// Задание КНМ без взаимодействия с контролируемыми лицами
        /// РТ
        /// </summary>
        [Display("Задание КНМ без взаимодействия с контролируемыми лицами")]
        TaskActionIsolated = 210,
        
        /// <summary>
        /// Профилактическое мероприятие
        /// </summary>
        [Display("Профилактическое мероприятие")]
        PreventiveAction = 221,
        
        /// <summary>
        /// Акт по КНМ без взаимодействия с контролируемыми лицами
        /// </summary>
        [Display("Акт по КНМ без взаимодействия с контролируемыми лицами")]
        ActActionIsolated = 230,

        /// <summary>
        /// Лист визита
        /// </summary>
        [Display("Лист визита")]
        VisitSheet = 240,
        
        /// <summary>
        /// Задание профилактического мероприятия
        /// </summary>
        [Display("Задание профилактического мероприятия")]
        PreventiveActionTask = 250,
        
        /// <summary>
        /// Мотивированное представление
        /// </summary>
        [Display("Мотивированное представление")]
        MotivatedPresentation = 260,
        
        /// <summary>
        /// Решение
        /// </summary>
        [Display("Решение")]
        Decision = 270,

        /// <summary>
        /// Решение проверки предписания
        /// </summary>
        [Display("Решение проверки предписания")]
        DecisionPrescription = 280,

        /// <summary>
        /// Мотивированное представление по обращению гражданина
        /// </summary>
        [Display("Мотивированное представление по обращению гражданина")]
        MotivatedPresentationAppealCits = 290
    }
}