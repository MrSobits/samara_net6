namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип состояния ГЖИ
    /// </summary>
    public enum TypeState
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Соответствует")]
        NotMatch = 20,

        [Display("Не соответствует")]
        Match = 30
    }
}