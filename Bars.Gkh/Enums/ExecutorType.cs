namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    public enum ExecutorType
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        [Display("Управляющая организация")]
        Mo = 10,

        /// <summary>
        /// Администрация МО
        /// </summary>
        [Display("Администрация МО")]
        Mu = 20,

        /// <summary>
        /// ГЖИ
        /// </summary>
        [Display("ГЖИ")]
        Gji = 30,

        /// <summary>
        /// Фонд КР
        /// </summary>
        [Display("Фонд КР")]
        CrFund = 40,

        /// <summary>
        /// Не выбран
        /// </summary>
        [Display("-")]
        None = 50
    }
}