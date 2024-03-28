namespace Bars.GisIntegration.Smev.ConfigSections
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using Bars.GisIntegration.Base.Entities;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройка параметров интеграции с ЕРП
    /// </summary>
    [GkhConfigSection("SmevIntegrationConfig", DisplayName = "Настройка параметров интеграции с ЕРП")]
    [Navigation]
    public class SmevIntegrationConfig : IGkhConfigSection
    {
        /// <summary>
        /// Интеграция включена
        /// </summary>
        [GkhConfigProperty(DisplayName = "Интеграция включена")]
        [DefaultValue(false)]
        public virtual bool IsEnabled { get; set; }

        /// <summary>
        /// Адрес прокси-сервера
        /// </summary>
        [GkhConfigProperty(DisplayName = "Адрес прокси-сервера")]
        public virtual string Endpoint { get; set; }

        /// <summary>
        /// Интервал запроса ответа (мин.)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Интервал запроса ответа (мин.)")]
        [DefaultValue(5)]
        public virtual int ReqestInterval { get; set; }

        /// <summary>
        /// Наименование органа контроля (надзора)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Наименование органа контроля (надзора)")]
        public virtual string ControlContractorName { get; set; }

        /// <summary>
        /// Адрес органа контроля (надзора)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Адрес органа контроля (надзора)")]
        public virtual string ControlContractorAddress { get; set; }

        /// <summary>
        /// Идентификатор Участника в ФРГУ-ГЖИ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор Участника в ФРГУ-ГЖИ")]
        public virtual string FrguParticipantId { get; set; }

        /// <summary>
        /// Идентификатор органа контроля в ФРГУ-ГЖИ (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор органа контроля в ФРГУ-ГЖИ (формата GUID)")]
        public virtual string FrguSupervisoryBodyId { get; set; }

        /// <summary>
        /// Идентификатор справочника прокуратур (формата GUID)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Идентификатор справочника прокуратур (формата GUID)")]
        public virtual string GuideProsecutorId { get; set; }

        /// <summary>
        /// Функции из ФРГУ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Функции из ФРГУ")]
        [GkhConfigPropertyEditor("B4.ux.config.FrguFunctionEditor", "frgufunctioneditor")]
        public virtual List<FrguFunction> FrguFunction { get; set; }

        /// <summary>
        /// Услуги ФРГУ
        /// </summary>
        // [GkhConfigProperty(DisplayName = "Услуги ФРГУ", HideToolbar = true)]
        // [GkhConfigPropertyEditor("B4.ux.config.ContextSettingsEditor", "contextsettingseditor")]
        // public virtual List<FrguServiceConfig> FrguServices { get; set; }
    }

    /// <summary>
    /// Услуги ФРГУ
    /// </summary>
    [DisplayName(@"Услуги ФРГУ")]
    public class FrguServiceConfig
    {
        /// <summary>
        /// Код
        /// </summary>
        [DisplayName(@"Код")]
        public string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [DisplayName(@"Наименование")]
        public string Name { get; set; }
    }
}
