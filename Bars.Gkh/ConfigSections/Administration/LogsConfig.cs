namespace Bars.Gkh.ConfigSections.Administration
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Логи
    /// </summary>
    public class LogsConfig : IGkhConfigSection
    {
        /// <summary>
        /// Настройка лога операций
        /// </summary>
        [GkhConfigProperty]
        [DisplayName(@"Настройка лога операций")]
        [Navigation]
        public virtual OperationLogsConfig Operation { get; set; } 
    }

    /// <summary>
    /// Настройка лога операций
    /// </summary>
    public class OperationLogsConfig : IGkhConfigSection
    {
        
    }
}