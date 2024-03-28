namespace Bars.Gkh.Gis.ConfigSections
{
    using System.Collections.Generic;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Gis.Entities.Kp50;

    /// <summary>
    /// Настройка строк соединения к серверам БД биллинга
    /// </summary>
    public class BilConnectionConfig : IGkhConfigSection
    {
        /// <summary>
        /// Настройки подключения к БД биллинга
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки подключений к БД биллинга")]
        [GkhConfigPropertyEditor("B4.ux.config.BilConnectionEditor", "bilconnection")]
        public virtual List<BilConnection> BilConnectionConfigs { get; set; }
    }
}