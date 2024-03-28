namespace Bars.GkhGji.ConfigSections
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Ограничение срока устранения нарушений
    /// </summary>
    public class TimeForCorrectingViol : IGkhConfigSection
    {
        /// <summary>
        /// Включить ограничение даты
        /// </summary>
        [GkhConfigProperty(DisplayName = "Включить ограничение даты")]
        public virtual bool IsLimitDate { get; set; }

        /// <summary>
        /// Единица периода
        /// </summary>
        [GkhConfigProperty(DisplayName = "Единица периода")]
        [DefaultValue(PeriodKind.Day)]
        public virtual PeriodKind PeriodKind { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период")]
        [UIExtraParam("maxValue", 31)]
        [UIExtraParam("minValue", 0)]
        [UIExtraParam("editorRestrictionIgnoreCondition", 
                 @"{
                        restrictions: ['maxValue', 'minValue'],
                        dependedFieldName: 'HousingInspection.GeneralConfig.TimeForCorrectingViol.PeriodKind',
                        dependedFieldValue: 20
                   }")]

        [DefaultValue(0)]
        public virtual int? Period { get; set; }
    }
}