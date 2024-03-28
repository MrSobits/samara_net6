namespace Bars.Gkh.ConfigSections.Cr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// количество загружаемых смет по работе
    /// </summary>
    public enum TypeWorkCrCountEstimations
    {
        [Display("1 смета по 1 работе")]
        OnlyOne = 0,

        [Display("Неограниченное количество смет по 1 работе")]
        OneMore = 10
    }
}
