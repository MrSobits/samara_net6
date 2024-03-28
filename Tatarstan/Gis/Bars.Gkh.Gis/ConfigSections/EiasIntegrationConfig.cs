namespace Bars.Gkh.Gis.ConfigSections
{
    using System.ComponentModel;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройки интеграции с ЕИАС
    /// </summary>
    public class EiasIntegrationConfig : IGkhConfigSection
    {
        private const int MaxWidth = 720;

        /// <summary>
        /// Адрес сервиса
        /// </summary>
        [GkhConfigProperty(DisplayName = "Адрес сервиса")]
        [DefaultValue("http://odata_ru_2_16.regportal-tariff.ru")]
        [UIExtraParam("maxWidth", EiasIntegrationConfig.MaxWidth)]
        public virtual string ServiceAddress { get; set; }

        /// <summary>
        /// Наименование объекта
        /// </summary>
        [GkhConfigProperty(DisplayName = "Наименование объекта")]
        [DefaultValue("ANALYTIC_RU_2_16.VU_NTKU12_M_TAT")]
        [UIExtraParam("maxWidth", EiasIntegrationConfig.MaxWidth)]
        public virtual string ObjectName { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        [GkhConfigProperty(DisplayName = "Логин")]
        [UIExtraParam("maxWidth", EiasIntegrationConfig.MaxWidth)]
        public virtual string Login { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        [GkhConfigProperty(DisplayName = "Пароль", UIHidden = true)]
        [UIExtraParam("inputType", "password")]
        [UIExtraParam("maxWidth", EiasIntegrationConfig.MaxWidth)]
        public virtual string Password { get; set; }
    }
}