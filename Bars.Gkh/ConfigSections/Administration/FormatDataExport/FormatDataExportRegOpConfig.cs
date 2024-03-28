namespace Bars.Gkh.ConfigSections.Administration.FormatDataExport
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Modules.Gkh1468.Enums;

    /// <summary>
    /// Общие
    /// </summary>
    [DisplayName("Настройки для выгрузки сведений по Региональному оператору")]
    public class FormatDataExportRegOpConfig : IGkhConfigSection
    {
        /// <summary>
        /// День месяца, до которого выставляются ЕПД
        /// </summary>
        [GkhConfigProperty(DisplayName = "День месяца, до которого выставляются ЕПД")]
        public virtual int? EpdDay { get; set; }

        /// <summary>
        /// Месяц, в котором выставляются ЕПД
        /// </summary>
        [GkhConfigProperty(DisplayName = "Месяц, в котором выставляются ЕПД")]
        public virtual MonthType? EpdMonth { get; set; }
    }
}