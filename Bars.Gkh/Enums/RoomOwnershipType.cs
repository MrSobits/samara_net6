using Bars.B4.Utils;

namespace Bars.Gkh.Enums
{
    /// <summary>
    /// Тип собственности помещения.
    /// </summary>
    public enum RoomOwnershipType
    {
        /// <summary>
        /// Частная.
        /// </summary>
        [Display("Частная")]
        Private = 10,

        /// <summary>
        /// Муниципальная.
        /// </summary>
        [Display("Муниципальная")]
        Municipal = 30,

        /// <summary>
        /// Государственная.
        /// </summary>
        [Display("Государственная")]
        Goverenment = 40,

        /// <summary>
        /// Коммерческая.
        /// </summary>
        [Display("Коммерческая")]
        Commerse = 50,

        /// <summary>
        /// Смешанная.
        /// </summary>
        [Display("Смешанная")]
        Mixed = 60,

        /// <summary>
        /// Не указано.
        /// </summary>
        [Display("Не указано")]
        NotSet = 70,

        /// <summary>
        /// Федеральная.
        /// </summary>
        [Display("Федеральная")]
        Federal = 80,

        /// <summary>
        /// Областная.
        /// </summary>
        [Display("Областная")]
        Regional = 90,

        /// <summary>
        /// Муниципальная (Адм) - Только Воронеж
        /// </summary>
        [Display("Муниципальная (Адм)")]
        MunicipalAdm = 100,

        /// <summary>
        /// Республиканская.
        /// </summary>
        [Display("Республиканская")]
        Republican = 110
    }
}
