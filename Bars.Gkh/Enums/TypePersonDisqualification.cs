namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип основания дисквалифиации 
    /// </summary>
    public enum TypePersonDisqualification
    {
        [Display("Назначение наказания в виде дисквалификации")]
        DisqualificationSanction = 10,

        [Display("Аннулирование лицензии")]
        CancelationLicenze = 20
    }
}
