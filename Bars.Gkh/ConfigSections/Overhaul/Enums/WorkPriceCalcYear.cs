namespace Bars.Gkh.ConfigSections.Overhaul.Enums
{
    using B4.Utils;

    /// <summary>
    /// Расценки расчета стоимости ремонта КЭ  
    /// </summary>
    public enum WorkPriceCalcYear
    {
        [Display("Расценки по первому году программы")]
        First = 10,

        [Display("Расценки по всем годам программы")]
        Current = 20
    }
}