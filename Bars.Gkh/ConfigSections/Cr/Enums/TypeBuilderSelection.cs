namespace Bars.Gkh.ConfigSections.Cr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Реестр для выбора подрядных организаций
    /// </summary>
    public enum TypeBuilderSelection
    {
        [Display("Квалификационный отбор")]
        Qualification = 0,

        [Display("Конкурсы")]
        Competition = 10,

        [Display("Не задан")]
        NotSet = 20
    }
}