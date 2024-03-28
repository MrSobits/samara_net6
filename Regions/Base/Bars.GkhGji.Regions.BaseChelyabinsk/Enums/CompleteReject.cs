namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Рассмотрение/отклонение
    /// </summary>
    public enum CompleteReject
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Результат рассмотрения")]
        Complete = 10,

        [Display("Отказ в рассмотрении")]
        Reject = 20
    }
}