namespace Bars.Gkh.Gis.Enum
{
    using B4.Utils;

    /// <summary>
    /// Тип потребителя
    /// </summary>
    [Display("Тип потребителя")]
    public enum ConsumerType
    {
        /// <summary>
        /// Население
        /// </summary>
        [Display("Население")]
        People = 1,

        /// <summary>
        /// Не население
        /// </summary>
        [Display("Не население")]
        NotPeople = 2,

        /// <summary>
        /// Общий
        /// </summary>
        [Display("Общий")]
        Common = 3
    } 
}