namespace Bars.Gkh.ConfigSections.ClaimWork
{
    using System.Collections.Generic;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    public class CommonConfig : IGkhConfigSection
    {
        /// <summary>
        ///     Исковая работа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Исковая работа")]
        public virtual GeneralConfigs General { get; set; }

        /// <summary>
        ///     Рассмотрение дел в суде
        /// </summary>
        [GkhConfigProperty(DisplayName = "Рассмотрение дел в суде")]
        // [InlineGridEditor]
        public virtual List<CourtProceedingConfig> CourtProceeding { get; set; }
    }
}