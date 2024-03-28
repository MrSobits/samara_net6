namespace Bars.Gkh.ConfigSections.ClaimWork.BuilderContract
{
    using System;
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Enums;

    [GkhConfigSection("BuilderContractClaimWork", DisplayName = "Настройки реестра подрядчиков", UIParent = typeof(ClaimWorkConfig))]
    [Navigation]
    [Permissionable]
    public class BuilderContractClaimWorkConfig : IGkhConfigSection
    {
        /// <summary>
        /// Уведомление о нарушении
        /// </summary>
        [GkhConfigProperty(DisplayName = "Уведомление о нарушении")]
        public virtual ViolationNotificationConfig ViolationNotification { get; set; }

        /// <summary>
        /// Претензия
        /// </summary>
        [GkhConfigProperty(DisplayName = "Претензия")]
        public virtual PretensionConfig Pretension { get; set; }

        /// <summary>
        /// Исковое заявление
        /// </summary>
        [GkhConfigProperty(DisplayName = "Исковое заявление")]
        public virtual PetitionConfig Petition { get; set; }

        /// <summary>
        /// Ежедневное обновление реестра
        /// </summary>
        [GkhConfigProperty(DisplayName = "Ежедневное обновление реестра")]
        [GkhConfigPropertyEditor("B4.ux.form.field.GkhTimeField", "gkhtimefield")]
        public virtual TimeSpan RegisterUpdateTime { get; set; }

        /// <summary>
        /// Группировка по МО
        /// </summary>
        [DefaultValue(TypeUsage.Used)]
        [GkhConfigProperty(DisplayName = "Группировка по МО")]
        public virtual TypeUsage TypeUsage { get; set; }
    }
}