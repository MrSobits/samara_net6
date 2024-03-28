namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Года
    /// </summary>
    public enum QuestionStatus
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Находится на рассмотрении
        /// </summary>
        [Display("Находится на рассмотрении")]
        InWork = 10,

        /// <summary>
        /// Рассмотрено. Разъяснено
        /// </summary>
        [Display("Рассмотрено. Разъяснено")]
        Explained = 20,

        /// <summary>
        ///  Рассмотрено. Поддержано
        /// </summary>
        [Display(" Рассмотрено. Поддержано")]
        Supported = 30,

        /// <summary>
        /// Рассмотрено. Не поддержано
        /// </summary>
        [Display("Рассмотрено. Не поддержано")]
        NotSupported = 40,

        /// <summary>
        /// Направлено по компетенции
        /// </summary>
        [Display("Направлено по компетенции")]
        Transferred = 50,

        /// <summary>
        /// Дан ответ автору
        /// </summary>
        [Display("Дан ответ автору")]
        Answered = 60,

        /// <summary>
        /// Оставлено без ответа автору
        /// </summary>
        [Display("Оставлено без ответа автору")]
        LeftWithoutAnswer = 70,

        /// <summary>
        /// - Рассмотрено. Поддержано. Меры приняты
        /// </summary>
        [Display("Рассмотрено. Поддержано. Меры приняты")]
        Plizdu = 80,

        /// <summary>
        /// - Рассмотрение продлено
        /// </summary>
        [Display("Рассмотрение продлено")]
        ConsiderationExtended = 90
    }
}