namespace Bars.GkhDi.GroupAction
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип группового действия
    /// </summary>
    public enum TypeDiTargetAction
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        [Display("Управляющая организация")]
        ManagingOrganization,

        /// <summary>
        /// Жилой дом
        /// </summary>
        [Display("Жилой дом")]
        RealityObject
    }
}