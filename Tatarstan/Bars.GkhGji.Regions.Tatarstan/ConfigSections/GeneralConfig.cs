namespace Bars.GkhGji.Regions.Tatarstan.ConfigSections
{
    using System.Collections.Generic;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.GkhGji.ConfigSections;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ConfigSections;

    /// <summary>
    /// Общие настройки ГЖИ
    /// </summary>
    public class GeneralConfig : IGkhConfigSection
    {
        /// <summary>
        /// Заполнение поля Исполнитель при добавлении Ответа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Заполнение поля Исполнитель при добавлении Ответа")]
        [Permissionable]
        public virtual bool ExecutorIsCurrentOperator { get; set; }

        /// <summary>
        /// Отображать обработанные обращения как "просроченные" 
        /// </summary>
        [GkhConfigProperty(DisplayName = "Отображать обработанные обращения как \"просроченные\"")]
        [Permissionable]
        public virtual bool ShowStatementsWithAnswerAsOverdue { get; set; }

        /// <summary>
        /// Ограничение срока устранения нарушений
        /// </summary>
        [GkhConfigProperty(DisplayName = "Ограничение срока устранения нарушений")]
        [Permissionable]
        public virtual TimeForCorrectingViol TimeForCorrectingViol { get; set; }

        /// <summary>
        /// Периоды действия документов ГЖИ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Периоды действия документов ГЖИ")]
        [GkhConfigPropertyEditor("B4.ux.config.ValidityDocPeriod", "validitydocperiod")]
        [Permissionable]
        public virtual List<GjiValidityDocPeriod> ValidityDocPeriods { get; set; }
    }
}
