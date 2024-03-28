namespace Bars.GkhGji.Regions.Tyumen.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип файла СМЭВ
    /// </summary>
    public enum SMEVFileType
    {
        /// <summary>
        /// Подписанный запрос
        /// </summary>
        [Display("Подписаный запрос")]
        SendRequestSig = 10,

        /// <summary>
        /// Ответ о статусе запроса
        /// </summary>
        [Display("Ответ о статусе запроса")]
        SendRequestSigAnswer = 20,

        /// <summary>
        /// запрос на получение сведений
        /// </summary>
        [Display("Запрос на получение сведений")]
        SendResponceSig = 30,

        /// <summary>
        /// Ответ со сведениями
        /// </summary>
        [Display("Ответ по ВС")]
        SendResponceSigAnswer = 40,

        /// <summary>
        /// Удаление запроса из очереди
        /// </summary>
        [Display("Удаление запроса из очереди")]
        AckRequestSig = 50,

        /// <summary>
        /// Запрос на добавление начисления
        /// </summary>
        [Display("Запрос на добавление начисления")]
        AddCalculate = 60,

        /// <summary>
        /// Ответ на добавление начисления
        /// </summary>
        [Display("Ответ на добавление начисления")]
        AddCalculateAnswer = 70,

        /// <summary>
        /// Запрос на получение платежа
        /// </summary>
        [Display("Запрос на получение платежей")]
        GetPayments = 80,

        /// <summary>
        /// Ответ на получение платежа
        /// </summary>
        [Display("Ответ с платежами")]
        GetPaymentsAmswer = 90,

        /// <summary>
        /// Необернутая начинка запроса
        /// </summary>
        [Display("Запрос")]
        Request = 100,

        /// <summary>
        /// Приложение к запросу
        /// </summary>
        [Display("Приложение к запросу")]
        RequestAttachment = 110,

        /// <summary>
        /// Приложение к ответу
        /// </summary>
        [Display("Приложение к ответу")]
        ResponseAttachment = 120,
        /// <summary>
        /// Приложение к ответу
        /// </summary>
        [Display("Приложение с ФТП")]
        ResponseAttachmentFTP = 130,
        /// <summary>
        /// Приложение к ответу
        /// </summary>
        [Display("Ошибка")]
        Error = 140
    }
}