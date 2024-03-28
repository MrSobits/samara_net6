namespace Bars.Gkh.Gis.Enum
{
    using B4.Utils;

    /// <summary>
    /// Вид тарифа <see cref="Entities.Dict.GisTariffDict"/>
    /// </summary>
    [Display("Вид тарифа")]
    public enum GisTariffKind
    {
        /// <summary>
        /// Экономически обоснованный тариф
        /// </summary>
        [Display("Экономически обоснованный тариф")]
        EconomicallyJustified = 1,

        /// <summary>
        /// Льготный тариф
        /// </summary>
        [Display("Тариф для населения")]
        Preferential = 2
    } 
}