namespace Bars.GkhCr.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Способ формирования записи претензеонной работы
    /// </summary>
    public enum BuildContractCreationType
    {
        [Display("Автоматически")]
        Auto = 10,

        [Display("Вручную")]
        User = 20
    }
}