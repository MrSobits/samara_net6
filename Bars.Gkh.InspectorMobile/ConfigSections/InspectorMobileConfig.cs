using Bars.Gkh.Config;
using Bars.Gkh.Config.Attributes;
using Bars.Gkh.Config.Attributes.UI;
using Bars.Gkh.Dto;
using Bars.Gkh.Entities;
using System.ComponentModel;

namespace Bars.Gkh.InspectorMobile.ConfigSections
{
    /// <summary>
    /// Настройки работы мобильного приложения
    /// </summary>
    [GkhConfigSection("InspectorMobile", DisplayName = "Настройки мобильного приложения")]
    [Permissionable]

    public class InspectorMobileConfig : IGkhConfigSection
    {
        /// <summary>
        /// Интеграция включена
        /// </summary>
        [GkhConfigProperty(DisplayName = "Интеграция включена")]
        [DefaultValue(false)]
        public virtual bool IntegrationEnabled { get; set; }

        /// <summary>
        /// Организация - пользователь
        /// </summary>
        [GkhConfigProperty(DisplayName = "Организация - пользователь")]
        [GkhConfigPropertyEditor("B4.ux.config.ContragentSelectField", "contragentselectfield")]
        public virtual EntityDto<Contragent> Organization { get; set; }

        /// <summary>
        /// Роли пользователей в ГИС МЖФ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Роли пользователей в МП")]
        public virtual RoleConfig RoleConfig { get; set; }
    }
}
