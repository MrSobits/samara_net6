namespace Bars.GkhGji.Regions.Tatarstan.ConfigSections.Appeal
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Настройки СОПР
    /// </summary>
    public class RapidResponseSystemConfig : IGkhConfigSection
    {
        /// <summary>
        /// Включить отправку уведомлений о направленных обращениях в СОПР
        /// </summary>
        [GkhConfigProperty(DisplayName = "Включить отправку уведомлений о направленных обращениях в СОПР")]
        [DefaultValue(false)]
        public virtual bool EnableSendingNotifications { get; set; }

        /// <summary>
        /// Параметр для расчета контрольного срока в СОПР
        /// </summary>
        [GkhConfigProperty(DisplayName = "Параметр для расчета контрольного срока в СОПР")]
        [DefaultValue(3)]
        public virtual int ControlPeriodParameter { get; set; }

        /// <summary>
        /// Включить подсветку записей реестра обращений ГЖИ, связанных с СОПР
        /// </summary>
        [GkhConfigProperty(DisplayName = "Включить подсветку записей реестра обращений ГЖИ, связанных с СОПР")]
        [DefaultValue(false)]
        public virtual bool EnableGjiAppealRecordsBacklight { get; set; }
        
        /// <summary>
        /// Включить подсветку записей с истекающем сроком в реестре СОПР
        /// </summary>
        [GkhConfigProperty(DisplayName = "Включить подсветку записей с истекающем сроком в реестре СОПР")]
        [DefaultValue(false)]
        public virtual bool EnableSoprExpiringRecordsBacklight { get; set; }

        /// <summary>
        /// Учитывать закрытые обращения
        /// </summary>
        [GkhConfigProperty(DisplayName = "Учитывать закрытые обращения")]
        [DefaultValue(false)]
        public virtual bool ConsiderClosedAppeals { get; set; }
    }
}