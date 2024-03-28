namespace Bars.Gkh.ConfigSections.Administration.PortalExport
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    [DisplayName("Параметры экспорта")]
    public class PortalExportParamsConfig : IGkhConfigSection
    {
        [GkhConfigProperty(DisplayName = "Выводить дома на счетах")]
        public virtual PortalExportTypeHouseConfig TypeHouseConfig { get; set; }
    }
}