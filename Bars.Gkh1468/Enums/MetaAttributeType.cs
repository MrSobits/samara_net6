using Bars.B4.Utils;

namespace Bars.Gkh1468.Enums
{
    public enum MetaAttributeType
    {
        [Display("Простой")]
        Simple = 10,

        [Display("Групповой")]
        Grouped = 20,

        [Display("Групповой со значением")]
        GroupedWithValue = 30,

        [Display("Групповой-множественный")]
        GroupedComplex = 40
    }
}
