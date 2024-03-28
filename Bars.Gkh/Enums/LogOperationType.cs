namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип операции(для логирования)
    /// </summary>
    public enum LogOperationType
    {
        /// <summary>
        /// Формирование квитанций
        /// </summary>
        [Display("Формирование квитанций")]
        PrintPaymentDocument = 10,

        /// <summary>
        /// Копирование услуг из дома в дом
        /// </summary>
        [Display("Копирование услуг из дома в дом")]
        CopyServByRo = 20,

        /// <summary>
        /// Копирование услуг из периода в период
        /// </summary>
        [Display("Копирование услуг из периода в период")]
        CopyServByPeriod = 30,

        /// <summary>
        /// Копирование услуг из периода в период
        /// </summary>
        [Display("Обработка обращений по рубрикам")]
        RunCitizenSuggestion = 40,

        /// <summary>
        /// Копирование льготной категории из карточки абонента в ЛС
        /// </summary>
        [Display("Выполнение действий")]
        RunAction = 50,

        /// <summary>
        /// Проверка реестра оплат
        /// </summary>
        [Display("Проверка реестра оплат")]
        CheckBankDocumentImport = 60,

        /// <summary>
        /// Выполнение контрольных проверок
        /// </summary>
        [Display("Выполнение контрольных проверок")]
        PerformingControlChecks = 70,

        /// <summary>
        /// Изменение классификации домов
        /// </summary>
        [Display("Изменение классификации домов")]
        UpdateRoTypes = 80,

        /// <summary>
        /// Отчет по наличию файла в директории FileDirectory\Year\Month
        /// </summary>
        [Display("Отчет по наличию файла в директории FileDirectory\\Year\\Month")]
        FileReport = 90,

        /// <summary>
        /// Подтверждение реестра
        /// </summary>
        [Display("Подтверждение реестра оплат")]
        AcceptBankDocumentImport = 100,

        /// <summary>
        /// Экспорт данных со сведениями ЖКХ
        /// </summary>
        [Display("Экспорт данных со сведениями ЖКХ")]
        FormatDataExport = 110,

        /// <summary>
        /// Рассылка квитанций на электронную почту
        /// </summary>
        [Display("Рассылка квитанций на электронную почту")]
        SendEmails = 120,

        /// <summary>
        /// Cмена состояния дома на аварийный
        /// </summary>
        [Display("Cмена состояния дома на аварийный")]
        StateChangeToEmergency = 130,

        /// <summary>
        /// Регистрация запроса на создание обращения
        /// </summary>
        [Display("Регистрация запроса на создание обращения")]
        AppealReg = 140,

        /// <summary>
        /// Регистрация запроса на создание обращения
        /// </summary>
        [Display("Регистрация запроса на создание обращения")]
        AppealPosReg = 145,

        /// <summary>
        /// Подтверждение онлайн оплат
        /// </summary>
        [Display("Подтверждение онлайн оплат")]
        UnconfimmedPayments = 150,
        
        /// <summary>
        /// Проверка ОКТМО с деревом МО
        /// </summary>
        [Display("Проверка ОКТМО с деревом МО")]
        CheckOktmo = 141
    }
}
