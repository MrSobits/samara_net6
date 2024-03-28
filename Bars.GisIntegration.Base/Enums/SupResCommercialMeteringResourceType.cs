namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид договора с поставщиком ресурсов
    /// </summary>
    public enum SupResCommercialMeteringResourceType
    {
        /// <summary>
        /// РСО
        /// </summary>
        [Display("РСО")]
        Rso = 10,

        /// <summary>
        /// Исполнитель коммунальных услуг
        /// </summary>
        [Display("Исполнитель коммунальных услуг")]
        CommunalServicesExecutor = 20
    }
}
