namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип адреса
    /// </summary>
    public enum EmailDenailReason
    {
        /// <summary>
        /// брать из контрагента юрадрес
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        [Display("Не является обращением")]
        NotAppeal = 10,

        [Display("Неполнота сведений")]
        LessInform = 20,

        [Display("Иное")]
        Other = 50
    }
}
