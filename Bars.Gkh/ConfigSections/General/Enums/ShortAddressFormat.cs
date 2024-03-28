namespace Bars.Gkh.ConfigSections.General.Enums
{
    using B4.Utils;

    /// <summary>
    /// Краткий формат адреса, используемый при сохранении карточки дома для последующего отображения в Реестре жилых домов.
    /// </summary>
    public enum ShortAddressFormat
    {
        [Display("Адрес, начиная с муниципального образования")]
        StartsFromUrbanArea,

        [Display("Адрес, начиная с муниципального образования самого нижнего уровня")]
        StartsFromLowestUrbanArea,

        [Display("Адрес без муниципального образования")]
        NoUrbanArea
    }
}
