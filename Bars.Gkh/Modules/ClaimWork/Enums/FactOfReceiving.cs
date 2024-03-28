namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Факт получения
    /// </summary>
    public enum FactOfReceiving
    {
        [Display("Не потверждено")]
        NotConfirmed = 0,

        [Display("Потверждено")]
        Confirmed = 10
    }
}