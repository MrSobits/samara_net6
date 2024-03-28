namespace Bars.GkhGji.Regions.Smolensk.Enums
{
    using Bars.B4.Utils;

    // Основной энум в Sobits.GisGkh, тут переопределяем для фронта

    /// <summary>
    /// Вид запроса
    /// </summary>
    public enum GisGkhTypeRequest
    {
        /// <summary>
        /// Получение списка общесистемных справочников
        /// </summary>
        [Display("Получение списка общесистемных справочников")]
        exportNsiList = 10,

        /// <summary>
        /// Получение списка справочников расширенного описания ОЖФ
        /// </summary>
        [Display("Получение списка справочников расширенного описания ОЖФ")]
        exportNsiRaoList = 11,

        /// <summary>
        /// Получение справочника
        /// </summary>
        [Display("Получение справочника")]
        exportNsiItems = 20,

        ///// <summary>
        ///// Получение рег. программы
        ///// </summary>
        //[Display("Получение рег. программы")]
        //exportRegionalProgram = 30,

        ///// <summary>
        ///// Получение работы рег. программы
        ///// </summary>
        //[Display("Получение работы рег. программы")]
        //exportRegionalProgramWork = 40,

        ///// <summary>
        ///// Получение информации о многоквартирном доме
        ///// </summary>
        //[Display("Получение информации о многоквартирном доме")]
        //exportBriefApartmentHouse = 50,

        /// <summary>
        /// Получение информации об организациях
        /// </summary>
        [Display("Получение информации об организациях")]
        exportOrgRegistry = 60,

        /// <summary>
        /// Получение информации о домах
        /// </summary>
        [Display("Получение информации о домах")]
        exportHouseData = 70,

        /// <summary>
        /// Получение информации о лицевых счетах
        /// </summary>
        [Display("Получение информации о лицевых счетах")]
        exportAccountData = 80,

        ///// <summary>
        ///// Получение КПР
        ///// </summary>
        //[Display("Получение КПР")]
        //exportPlan = 90,

        /// <summary>
        /// Выгрузка ЛС
        /// </summary>
        [Display("Выгрузка ЛС")]
        importAccountData = 100,

        /// <summary>
        /// Экспорт решений о выборе способа формирования ФКР
        /// </summary>
        [Display("Получение решений о выборе способа формирования ФКР")]
        exportDecisionsFormingFund = 110,

        /// <summary>
        /// Выгрузка решений о выборе способа формирования ФКР
        /// </summary>
        [Display("Выгрузка решений о выборе способа формирования ФКР")]
        importDecisionsFormingFund = 120,

        /// <summary>
        /// Получение сведений о платежных документах
        /// </summary>
        [Display("Получение сведений о платежных документах")]
        exportPaymentDocumentData = 140,

        /// <summary>
        /// Выгрузка платежных документов
        /// </summary>
        [Display("Выгрузка платежных документов")]
        importPaymentDocumentData = 150,

        /// <summary>
        /// Получение лицензий
        /// </summary>
        [Display("Получение запросов задолженности")]
        exportDebtRequests = 750,
        /// <summary>
        /// Получение запросов задолженности
        /// </summary>
        [Display("Ответ на запрос задолженности")]
        importDebtRequests = 751

        ///// <summary>
        ///// Выгрузка КПР
        ///// </summary>
        //[Display("Выгрузка КПР")]
        //importPlan = 130
    }
}