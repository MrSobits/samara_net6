namespace Bars.Gkh.ConfigSections.ClaimWork.Debtor
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Enums;

    public class GeneralConfig : IGkhConfigSection
    {
        /// <summary>
        /// Группировка по МО
        /// </summary>
        [DefaultValue(TypeUsage.Used)]
        [GkhConfigProperty(DisplayName = "Группировка по МО")]
        public virtual TypeUsage TypeUsage { get; set; }

        /// <summary>
        /// Наименование файла документа ПИР
        /// </summary>
        [DefaultValue(DocumentClwFileName.WithoutAddress)]
        [GkhConfigProperty(DisplayName = "Наименование файла документа ПИР")]
        public virtual DocumentClwFileName DocumentClwFileName { get; set; }

        /// <summary>
        /// Отображать ПИР согласно пользователю
        /// </summary>
        [GkhConfigProperty(DisplayName = "Отображать ПИР согласно пользователю")]
        public virtual bool FilterByUser  { get; set; }
    }
}