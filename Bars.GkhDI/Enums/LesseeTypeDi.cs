namespace Bars.GkhDi.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип арендатора
    /// </summary>
    public enum LesseeTypeDi
    {
        [Display("Юридическое лицо")]
        Legal = 10,

        [Display("Физическое лицо")]
        Individual = 20
    }
}
