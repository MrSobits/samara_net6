namespace Bars.GisIntegration.Base.Enums
{
    using B4.Utils;

    public enum MeteringDeviceArchivation
    {
        /// <summary>
        /// Архивация без замены
        /// </summary>
        [Display("Архивация без замены")]
        NoReplacing = 10,

        /// <summary>
        /// Архивация с заменой на другой ПУ
        /// </summary>
        [Display("Архивация с заменой на другой ПУ")]
        Replacing = 20
    }
}
