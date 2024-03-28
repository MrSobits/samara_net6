namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    public enum ConnectionSchemeType
    {
        /// <summary>
        /// Зависимая
        /// </summary>
        [Display("Зависимая")]
        Dependent = 10,

        /// <summary>
        /// Независимая
        /// </summary>
        [Display("Независимая")]
        Independent = 20
    }
}
