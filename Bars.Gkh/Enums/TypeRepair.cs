namespace Bars.Gkh.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип ремонта
    /// </summary>
    public enum TypeRepair
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Ремонт")]
        Repair = 20,

        [Display("Замена")]
        Replace = 30,

        [Display("Реконструкция")]
        Reconstruction = 40,

        [Display("Модернизация")]
        Upgrade = 50
    }
}