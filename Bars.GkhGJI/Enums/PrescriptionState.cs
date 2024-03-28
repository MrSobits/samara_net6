namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус предписания
    /// </summary>
    public enum PrescriptionState
    {
        [Display("Действует")]
        Active = 0,

        [Display("Отменено судом")]
        CancelledByCourt = 10,

        [Display("Отменено ГЖИ")]
        CancelledByGJI = 20    
    }
}