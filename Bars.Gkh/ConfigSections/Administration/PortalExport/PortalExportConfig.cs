namespace Bars.Gkh.ConfigSections.Administration.PortalExport
{
    using System.ComponentModel;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    [DisplayName(@"Настройка экспорта данных на портал")]
    public class PortalExportConfig : IGkhConfigSection
    {
        /// <summary>
        /// Максимальная длина поля
        /// </summary>
        public const int MaxWidth = 720;

        /// <summary>
        /// Параметры экспорта
        /// </summary>
        [GkhConfigProperty(DisplayName = "Параметры экспорта")]
        public virtual PortalExportParamsConfig PortalExportParams { get; set; }
    }
}