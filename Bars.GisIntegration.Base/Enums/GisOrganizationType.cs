namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип организации в ГИС ЖКХ
    /// </summary>
    public enum GisOrganizationType
    {
        /// <summary>
        /// Индивидуальный предприниматель
        /// </summary>
        [Display("Индивидуальный предприниматель")]
        Entps = 10,

        /// <summary>
        /// Юридическое лицо
        /// </summary>
        [Display("Юридическое лицо")]
        Legal = 20,

        /// <summary>
        /// Обособленное подразделение
        /// </summary>
        [Display("Обособленное подразделение")]
        Subsidiary = 30
    }
}
