namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус контрагента
    /// </summary>
    public enum ContragentState
    {
        /// <summary>
        /// Действует
        /// </summary>
        [Display("Действует")]
        Active = 10,

        /// <summary>
        /// Не предоставляет услуги управления
        /// </summary>
        [Display("Не предоставляет услуги управления")]
        NotManagementService = 20,

        /// <summary>
        /// Банкрот
        /// </summary>
        [Display("Банкрот")]
        Bankrupt = 30,

        /// <summary>
        /// Ликвидирован
        /// </summary>
        [Display("Ликвидирован")]
        Liquidated = 40,

        /// <summary>
        /// Реорганизация
        /// </summary>
        [Display("Реорганизация")]
        Reorganized = 50,

        /// <summary>
        /// Регистрация признана недействительной
        /// </summary>
        [Display("Регистрация признана недействительной")]
        NotValidRegistration = 60,

        /// <summary>
        /// В процессе ликвидации
        /// </summary>
        [Display("В процессе ликвидации")]
        LiquidationProcess = 70,

        /// <summary>
        /// Предстоит исключение недействующего ЮЛ из ЕГРЮЛ
        /// </summary>
        [Display("Предстоит исключение недействующего ЮЛ из ЕГРЮЛ")]
        DelistingSoon = 80,

        /// <summary>
        /// В процессе уменьшения уставного капитала
        /// </summary>
        [Display("В процессе уменьшения уставного капитала")]
        MinimiseAutorizedCapital = 90,

        /// <summary>
        /// Принято решение об изменении места нахождения
        /// </summary>
        [Display("Принято решение об изменении места нахождения")]
        ChangeAddress = 100,

        /// <summary>
        /// Принято решение о ликвидации
        /// </summary>
        [Display("Принято решение о ликвидации")]
        LiquidationSoon = 110,

        /// <summary>
        /// Исключен из ЕГРЮЛ
        /// </summary>
        [Display("Исключен из ЕГРЮЛ")]
        Delisted = 120
    }
}