namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип заключения ГЖИ
    /// </summary>
    public enum TypeConclusion
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("О не соответствии ЖК")]
        NotMatch = 20,

        [Display("О соответствии ЖК")]
        Match = 30
    }
}