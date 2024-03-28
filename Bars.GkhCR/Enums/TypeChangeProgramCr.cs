namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип программы капитального ремонта
    /// </summary>
    public enum TypeChangeProgramCr
    {
        [Display("Вручную")]
        Manually = 0,

        [Display("На основе ДПКР")]
        FromDpkr = 10
    }
}
