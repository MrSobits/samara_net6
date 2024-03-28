namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип домофона
    /// </summary>
    public enum IntercomUnitMeasure
    {
        /// <summary>
        /// руб. / кв.м 
        /// </summary>
        [Display("руб./кв.м ")]
        SquareMeter = 1,

        /// <summary>
        /// руб. / квартиру
        /// </summary>
        [Display("руб./квартиру")]
        Apartment = 2
    }
}
