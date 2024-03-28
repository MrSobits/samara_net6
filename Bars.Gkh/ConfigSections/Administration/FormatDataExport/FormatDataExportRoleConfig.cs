namespace Bars.Gkh.ConfigSections.Administration.FormatDataExport
{
    using System.ComponentModel;

    using Bars.B4.Modules.Security;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.Dto;

    /// <summary>
    /// Сопоставление ролей с поставщиками информации
    /// </summary>
    [DisplayName(@"Сопоставление ролей с поставщиками информации")]
    public class FormatDataExportRoleConfig : IGkhConfigSection
    {
        // TODO Переделать грязь
        /// <summary>
        /// УО
        /// </summary>
        [GkhConfigProperty(DisplayName = "Администратор")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Administrator { get; set; }

        /// <summary>
        /// УО
        /// </summary>
        [GkhConfigProperty(DisplayName = "УО")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Uo { get; set; }

        /// <summary>
        /// РСО
        /// </summary>
        [GkhConfigProperty(DisplayName = "РСО")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Rso { get; set; }

        /// <summary>
        /// ГЖИ
        /// </summary>
        [GkhConfigProperty(DisplayName = "ГЖИ")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Gji { get; set; }

        /// <summary>
        /// ОМС
        /// </summary>
        [GkhConfigProperty(DisplayName = "ОМС")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Oms { get; set; }

        /// <summary>
        /// ОГВ
        /// </summary>
        [GkhConfigProperty(DisplayName = "ОГВ")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> Ogv { get; set; }

        /// <summary>
        /// Региональный оператор капитального ремонта
        /// </summary>
        [GkhConfigProperty(DisplayName = "Региональный оператор капитального ремонта")]
        [GkhConfigPropertyEditor("B4.ux.config.RoleSelectField", "roleselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Role> RegOpCr { get; set; }
    }
}