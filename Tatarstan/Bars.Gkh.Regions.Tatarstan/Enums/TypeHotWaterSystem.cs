namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид системы горячего водоснабжени
    /// </summary>
    public enum TypeHotWaterSystem
    {
        /// <summary>
        /// С наружной сетью ГВС
        /// </summary>
        [Display("С наружной сетью ГВС")]
        WithExternalNetworkGvs = 10,

        /// <summary>
        /// Без наружной сети ГВС
        /// </summary>
        [Display("Без наружной сети ГВС")]
        WithoutExternalNetweorkGvs = 20
    }
}