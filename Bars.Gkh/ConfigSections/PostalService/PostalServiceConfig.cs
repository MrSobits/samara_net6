namespace Bars.Gkh.ConfigSections.PostalService
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Настройка параметров почтового сервиса
    /// </summary>
    [GkhConfigSection("PostalServiceConfig", DisplayName = "Настройка параметров почтового сервиса")]
    [Navigation]
    public class PostalServiceConfig : IGkhConfigSection
    {
        /// <summary>
        /// Включить почтовый сервис
        /// </summary>
        [GkhConfigProperty(DisplayName = "Включить почтовый сервис")]
        [DefaultValue(false)]
        public virtual bool EnablePostalService { get; set; }

        /// <summary>
        /// Почта отправителя
        /// </summary>
        [GkhConfigProperty(DisplayName = "Почта отправителя")]
        public virtual string SenderPost { get; set; }
        
        /// <summary>
        /// Логин
        /// </summary>
        [GkhConfigProperty(DisplayName = "Логин")]
        public virtual string Login { get; set; }
        
        /// <summary>
        /// Пароль
        /// </summary>
        [GkhConfigProperty(DisplayName = "Пароль")]
        public virtual string Password { get; set; }
        
        /// <summary>
        /// Адрес SMTP сервера
        /// </summary>
        [GkhConfigProperty(DisplayName = "Адрес SMTP сервера")]
        public virtual string SMTPServerAddress { get; set; }
        
        /// <summary>
        /// Порт исходящей почты
        /// </summary>
        [GkhConfigProperty(DisplayName = "Порт исходящей почты")]
        public virtual string SenderPostPort { get; set; }
    }
}