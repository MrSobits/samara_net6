namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип основания проверки
    /// </summary>
    public enum TypeBase
    {
        /// <summary>
        /// Инспекционная проверка
        /// </summary>
        [Display("Инспекционная проверка")]
        Inspection = 10,

        /// <summary>
        /// Обращение граждан
        /// </summary>
        [Display("Обращение граждан")]
        CitizenStatement = 20,

        /// <summary>
        /// Мотивировочное заключение
        /// </summary>
        [Display("Мотивировочное заключение")]
        MotivationConclusion = 21,

        /// <summary>
        /// Плановая проверка юр.лиц
        /// </summary>
        [Display("Плановая проверка юр. лиц")]
        PlanJuridicalPerson = 30,

        /// <summary>
        /// Плановая проверка ОМСУ
        /// </summary>
        [Display("Плановая проверка ОМСУ")]
        PlanOMSU = 31,

        /// <summary>
        /// Поручение руководства
        /// </summary>
        [Display("Поручение руководства")]
        DisposalHead = 40,

        /// <summary>
        /// Основание предостережения ГЖИ (РТ)
        /// </summary>
        [Display("Предостережение")]
        GjiWarning = 41,

        /// <summary>
        /// Требование прокуратуры
        /// </summary>
        [Display("Требование прокуратуры")]
        ProsecutorsClaim = 50,

        /// <summary>
        /// Постановление прокуратуры
        /// </summary>
        [Display("Постановление прокуратуры")]
        ProsecutorsResolution = 60,

        /// <summary>
        /// Проверка деятельности ТСЖ
        /// </summary>
        [Display("Проверка деятельности ТСЖ")]
        ActivityTsj = 70,

        /// <summary>
        /// Подготовка к отопительному сезону
        /// </summary>
        [Display("Подготовка к отопительному сезону")]
        HeatingSeason = 80,

        /// <summary>
        /// Административное дело
        /// </summary>
        [Display("Административное дело")]
        AdministrativeCase = 90,

        /// <summary>
        /// Протокол МВД
        /// </summary>
        [Display("Протокол МВД")]
        ProtocolMvd = 100,

        /// <summary>
        /// Проверка по плану мероприятий
        /// </summary>
        [Display("Проверка по плану мероприятий")]
        PlanAction = 110,

        /// <summary>
        /// Протокол МЖК
        /// </summary>
        [Display("Протокол МЖК")]
        ProtocolMhc = 120,

        /// <summary>
        /// Проверка соискателей лицензии
        /// </summary>
        [Display("Проверка соискателей лицензии")]
        LicenseApplicants = 130,

        /// <summary>
        /// Проверка лицензиата для переоформления лицензии
        /// </summary>
        [Display("Проверка лицензиата")]
        LicenseReissuance = 135,

        /// <summary>
        /// Протокол по ст.19.7 КоАП РФ
        /// <para>Используется в НСО</para>
        /// </summary>
        [Display("Протокол по ст.19.7 КоАП РФ")]
        Protocol197 = 141,
        
        /// <summary>
        /// Протокол ГЖИ
        /// для РТ
        /// </summary>
        [Display("Протокол ГЖИ")]
        ProtocolGji = 140,

        /// <summary>
        /// Без основания
        /// </summary>
        [Display("Без основания")]
        Default = 150,

        /// <summary>
        /// Протокол РСО
        /// </summary>
        [Display("Протокол РСО")]
        ProtocolRSO = 191,

        /// <summary>
        /// Мероприятие КНМ без взаимодействия с контролируемыми лицами
        /// </summary>
        [Display("КНМ без взаимодействия с контролируемыми лицами")]
        ActionIsolated = 160,
        
        /// <summary>
        /// Профилактическое мероприятие
        /// </summary>
        [Display("Профилактическое мероприятие")]
        PreventiveAction = 170,
        
        /// <summary>
        /// Проверки по мероприятиям без взаимодействия с контролируемыми лицами
        /// </summary>
        [Display("Проверки по мероприятиям без взаимодействия с контролируемыми лицами")]
        InspectionActionIsolated = 180,

        /// <summary>
        /// Мотивированное представление обращения граждан
        /// </summary>
        [Display("Проверки по мероприятиям без взаимодействия с контролируемыми лицами")]
        MotivatedPresentationAppealCits = 190,

        /// <summary>
        /// Проверка по профилактическому мероприятию
        /// </summary>
        [Display("Проверка по профилактическому мероприятию")]
        InspectionPreventiveAction = 200
    }
}