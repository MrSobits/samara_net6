namespace Bars.GisIntegration.Gkh.ConfigSections
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройки интеграции с ГИС РФ
    /// </summary>
    // [GkhConfigSection("GisIntegrationConfig", DisplayName = "Настройки интеграции с ГИС РФ", UIParent = typeof(GisConfig))]
    [GkhConfigSection("GisIntegrationConfig", DisplayName = "Настройки интеграции с ГИС РФ")]
    [Navigation]
    public class GisIntegrationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Настройки сервисов (не использовать)
        /// </summary>
        [Obsolete("Использовать IGisSettingsService")]
        [GkhConfigProperty(DisplayName = "Настройки сервисов (не использовать)", HideToolbar = true, Hidden = true)]
        public virtual List<ServiceSettingConfig> ServiceSettingConfigs { get; set; }

        /// <summary>
        /// Настройки сервисов
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки сервисов", HideToolbar = true)]
        [GkhConfigPropertyEditor("B4.ux.config.ServiceSettingsEditor", "servicesettingseditor")]
        public virtual int ServiceSettingsConfigs { get; set; }

        /// <summary>
        /// Подписывать сообщение
        /// </summary>
        [GkhConfigProperty(DisplayName = "Подписывать сообщение")]
        [Group("Общие")]
        [DefaultValue(false)]
        public virtual bool SingXml { get; set; }

        /// <summary>
        /// Указывать логин/пароль
        /// </summary>
        [GkhConfigProperty(DisplayName = "Указывать логин/пароль")]
        [Group("Общие")]
        [DefaultValue("false")]
        public virtual bool UseLoginCredentials { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [GkhConfigProperty(DisplayName = "Логин")]
        [Group("Общие")]
        [DefaultValue("test")]
        public virtual string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [GkhConfigProperty(DisplayName = "Пароль")]
        [Group("Общие")]
        [DefaultValue("SDldfls4lz5@!82d")]
        public virtual string Password { get; set; }

        /// <summary>
        /// Настройки подсистем
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки подсистем", HideToolbar = true)]
        [GkhConfigPropertyEditor("B4.ux.config.ContextSettingsEditor", "contextsettingseditor")]
        public virtual int ContextSettingsConfigs { get; set; }
    }

    /// <summary>
    /// Настройка сервиса
    /// </summary>
    [DisplayName(@"Настройки сервиса")]
    public class ServiceSettingConfig
    {
        /// <summary>
        /// Сервис интеграции
        /// </summary>
        [DisplayName(@"Сервис интеграции")]
        [ReadOnly(true)]
        public IntegrationService IntegrationService { get; set; }

        /// <summary>
        /// Отображаемое наименование
        /// </summary>
        [DisplayName(@"Отображаемое наименование")]
        public string Name { get; set; }

        /// <summary>
        /// Адрес сервиса
        /// </summary>
        [DisplayName(@"Адрес сервиса")]
        public string ServiceAddress { get; set; }

        /// <summary>
        /// Адрес асинхронного сервиса
        /// </summary>
        [DisplayName(@"Адрес асинхронного сервиса")]
        public string AsyncServiceAddress { get; set; }
    }
}