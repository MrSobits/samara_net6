namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус сопоставления аккаунтов с выписками
    /// </summary>
    public enum PretensionType
    {
        /// <summary>
        /// Не указана
        /// </summary>
        [Display("Не указана")]
        NotSet = 0,

        /// <summary>
        /// Частная
        /// </summary>
        [Display("Частная")]
        Private = 10,

        /// <summary>
        /// Апелляционная
        /// </summary>
        [Display("Апелляционная")]
        Appeal = 20,

        /// <summary>
        /// Кассационная
        /// </summary>
        [Display("Кассационная")]
        Cassation = 30,


        /// <summary>
        /// Надзорная
        /// </summary>
        [Display("Надзорная")]
        Supervisor = 40
    }
}