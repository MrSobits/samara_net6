namespace Bars.Gkh.ConfigSections.Overhaul
{
    using System;
    using System.ComponentModel;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Dto;
    using Bars.Gkh.Overhaul.Entities.Dict;

    /// <summary>
    /// Настройки основания ДПКР
    /// </summary>
    [GkhConfigSection("Overhaul.BasisOverhaulDocConfig", DisplayName = "Настройки основания ДПКР", UIParent = typeof(OverhaulConfig))]
    [Permissionable]
    [Navigation]
    public class BasisOverhaulDocConfig : IGkhConfigSection
    {
        private const int MaxWidth = 750;

        /// <summary>
        /// Вид документа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Вид документа")]
        [GkhConfigPropertyEditor("B4.ux.config.BasisOverhaulDocKindSelectField", "basisoverhauldockindselectfield")]
        [Permissionable]
        [UIExtraParam("maxWidth", BasisOverhaulDocConfig.MaxWidth)]
        public virtual EntityDto<BasisOverhaulDocKind> Kind { get; set; }

        /// <summary>
        /// Наименование документа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Наименование документа")]
        [Permissionable]
        [UIExtraParam("maxWidth", BasisOverhaulDocConfig.MaxWidth)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Номер документа")]
        [Permissionable]
        [UIExtraParam("maxWidth", BasisOverhaulDocConfig.MaxWidth)]
        public virtual string Number { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Дата документа")]
        [Permissionable]
        [UIExtraParam("maxWidth", BasisOverhaulDocConfig.MaxWidth)]
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// Орган, принявшие документ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Орган, принявшие документ")]
        [Permissionable]
        [UIExtraParam("maxWidth", BasisOverhaulDocConfig.MaxWidth)]
        public virtual string DecisionMaker { get; set; }

        /// <summary>
        /// Статус документа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Статус документа")]
        [Permissionable]
        [DefaultValue(DocumentState.Active)]
        [UIExtraParam("maxWidth", BasisOverhaulDocConfig.MaxWidth)]
        public virtual DocumentState State { get; set; }
    }
}