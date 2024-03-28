namespace Bars.GkhGji.Enums
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
        AppealCitsAttachment = 57,

        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        [Display("Документ ГЖИ")]
        DocumentGji = 4,

        /// <summary>
        /// Документы ГЖИ - Приложение
        /// </summary>
        [Display("Документы ГЖИ - Приложение")]
        DocumentGjiAnnex = 5,

        /// <summary>
        /// Документы ГЖИ - Дом
        /// </summary>
        [Display("Документы ГЖИ - Дом")]
        DocumentGjiRealityObject = 6,

        /// <summary>
        /// Документы ГЖИ - КНД
        /// </summary>
        [Display("Документы ГЖИ - КНД")]
        DocumentGjiControlMeasures = 7,

        /// <summary>
        /// Документы ГЖИ - Лица, присутствующие при проверке
        /// </summary>
        [Display("Документы ГЖИ - Лица, присутствующие при проверке")]
        DocumentGjiWitness = 8,

        /// <summary>
        /// Документы ГЖИ - Инспектируемые части
        /// </summary>
        [Display("Документы ГЖИ - Инспектируемые части")]
        DocumentGjiInspectedPart = 9,

        /// <summary>
        /// Документы ГЖИ - Предоставляемые документы
        /// </summary>
        [Display("Документы ГЖИ - Предоставляемые документы")]
        DocumentGjiProvidedDoc = 10,

        /// <summary>
        /// Документы ГЖИ - Дата и время проведения проверки
        /// </summary>
        [Display("Документы ГЖИ - Дата и время проведения проверки")]
        DocumentGjiPeriod = 11,

        /// <summary>
        /// Документы ГЖИ - Определение
        /// </summary>
        [Display("Документы ГЖИ - Определение")]
        DocumentGjiDefinition = 12,

        /// <summary>
        /// Документы ГЖИ - Статья закона
        /// </summary>
        [Display("Документы ГЖИ - Статья закона")]
        DocumentGjiArticleLaw = 13,

        /// <summary>
        /// Документы ГЖИ - Нарушения
        /// </summary>
        [Display("Документы ГЖИ - Нарушения")]
        DocumentGjiViolation = 14,

        /// <summary>
        /// Документы ГЖИ - Акт обследования - Сведения о собственниках
        /// </summary>
        [Display("Документы ГЖИ - Акт обследования - Сведения о собственниках")]
        ActSurveyOwner = 15,

        /// <summary>
        /// Документы ГЖИ - Акт обследования - Фотоматериалы
        /// </summary>
        [Display("Документы ГЖИ - Акт обследования - Фотоматериалы")]
        ActSurveyPhoto = 16,

        /// <summary>
        /// Документы ГЖИ - Ответы на вопросы проверочного листа
        /// </summary>
        [Display("Документы ГЖИ - Ответы на вопросы проверочного листа")]
        ActCheckControlListAnswer = 17,

        /// <summary>
        /// Документы ГЖИ - Деятельность субъекта проверки
        /// </summary>
        [Display("Документы ГЖИ - Деятельность субъекта проверки")]
        DocumentGjiActivityDirection = 18,

        /// <summary>
        /// Документы ГЖИ - Постановление - Оспаривания
        /// </summary>
        [Display("Документы ГЖИ - Постановление - Оспаривания")]
        ResolutionDispute = 19,

        /// <summary>
        /// Документы ГЖИ - Постановление - Оплата штрафов
        /// </summary>
        [Display("Документы ГЖИ - Постановление - Оплата штрафов")]
        ResolutionPayFine = 20,

        /// <summary>
        /// Документы ГЖИ - Акт профилактического визита - Результат
        /// </summary>
        [Display("Документы ГЖИ - Акт профилактического визита - Результат")]
        PreventiveVisitResult = 21,

        /// <summary>
        /// Документы ГЖИ - Административные регламенты
        /// </summary>
        [Display("Документы ГЖИ - Административные регламенты")]
        DocumentGjiAdminRegulation = 22,

        /// <summary>
        /// Документы ГЖИ - Предметы проверки
        /// </summary>
        [Display("Документы ГЖИ - Предметы проверки")]
        DocumentGjiVerificationSubject = 23,

        /// <summary>
        /// Документы ГЖИ - Субъекты проверки
        /// </summary>
        [Display("Документы ГЖИ - Субъекты проверки")]
        DecisionControlSubjects = 24,

        /// <summary>
        /// Доументы ГЖИ - Решение - Основание
        /// </summary>
        [Display("Доументы ГЖИ - Решение - Основание")]
        DecisionInspectionReason = 25,

        /// <summary>
        /// Докумены ГЖИ - Эксперты
        /// </summary>
        [Display("Докумены ГЖИ - Эксперты")]
        DocumentGjiExpert = 26,

        /// <summary>
        /// Документы ГЖИ - Решение - Надзорные действия
        /// </summary>
        [Display("Документы ГЖИ - Решение - Надзорные действия")]
        DecisionControlList = 27,

        /// <summary>
        /// Доукменты ГЖИ - Предписание - Решение
        /// </summary>
        [Display("Доукменты ГЖИ - Предписание - Решение")]
        PrescriptionCancel = 28,

        /// <summary>
        /// Документы ГЖИ - Документ-основание
        /// </summary>
        [Display("Документы ГЖИ - Документ-основание")]
        DocumentGjiBaseDocument = 29,

        /// <summary>
        /// Документы ГЖИ - Протокол - Перечень требований
        /// </summary>
        [Display("Документы ГЖИ - Протокол - Перечень требований")]
        ProtocolSurveySubjectRequirement = 30,

        /// <summary>
        /// Документы ГЖИ - Распоряжения - Документ основания
        /// </summary>
        [Display("Документы ГЖИ - Распоряжения - Документ основания")]
        DisposalInspFoundation = 32,

        /// <summary>
        /// Документы ГЖИ - Распоряжения - НПА проверки
        /// </summary>
        [Display("Документы ГЖИ - Распоряжения - НПА проверки")]
        DisposalInspFoundationCheck = 33,

        /// <summary>
        /// Документы ГЖИ - Распоряжения - Требования НПА проверки
        /// </summary>
        [Display("Документы ГЖИ - Распоряжения - Требования НПА проверки")]
        DisposalInspFoundCheckNormDocItem = 34,

        /// <summary>
        /// Документы ГЖИ - Распоряжения - Задачи проверки
        /// </summary>
        [Display("Документы ГЖИ - Распоряжения - Задачи проверки")]
        DisposalSurveyObjective = 35,

        /// <summary>
        /// Документы ГЖИ - Распоряжения - Цели проверки
        /// </summary>
        [Display("Документы ГЖИ - Распоряжения - Цели проверки")]
        DisposalSurveyPurpose = 36,

        /// <summary>
        /// Документы ГЖИ - Распоряжения - Тип обследования
        /// </summary>
        [Display("Документы ГЖИ - Распоряжения - Тип обследования")]
        DisposalTypeSurvey = 37,

        /// <summary>
        /// Документы ГЖИ - Распоряжения - Факты нарушения
        /// </summary>
        [Display("Документы ГЖИ - Распоряжения - Факты нарушения")]
        DisposalFactViolation = 38,

        /// <summary>
        /// сание - Служебная записка 
        /// </summary>
        [Display("сание - Служебная записка ")]
        PrescriptionOfficialReport = 39,

        /// <summary>
        /// Документы ГЖИ - Предписание - Предоставление
        /// </summary>
        [Display("Документы ГЖИ - Предписание - Предоставление")]
        PrescriptionCloseDoc = 40,

        /// <summary>
        /// Документы ГЖИ - Постановление - Реквизиты физического лица
        /// </summary>
        [Display("Документы ГЖИ - Постановление - Реквизиты физического лица")]
        ResolutionFiz = 41,

        /// <summary>
        /// Документы ГЖИ - Постановление - Решение
        /// </summary>
        [Display("Документы ГЖИ - Постановление - Решение")]
        ResolutionDecision = 42,

        /// <summary>
        /// Документы ГЖИ - Распоряжение - Дополнительные документы
        /// </summary>
        [Display("Документы ГЖИ - Распоряжение - Дополнительные документы")]
        DisposalAdditionalDoc = 43,

        /// <summary>
        /// Документы ГЖИ - Распоряжение - Документы на согласование
        /// </summary>
        [Display("Документы ГЖИ - Распоряжение - Документы на согласование")]
        DisposalDocConfirm = 44,

        /// <summary>
        /// Жилищная инспекция - риск-ориентированный подход - расчет категории риска
        /// </summary>
        [Display("Жилищная инспекция - риск-ориентированный подход - расчет категории риска")]
        VprPrescription = 45,
    }
}
