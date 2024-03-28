namespace Bars.GkhGji.ConfigSections
{
    using System;
    using System.ComponentModel;

    using Bars.Gkh.Config.Attributes.UI;
    using Bars.GkhGji.Enums;

    using Gkh.Config;
    using Gkh.Config.Attributes;
    /// <summary>
    /// Настройки рабочего дня
    /// </summary>
    public class SettingsOfTheDayConfig : IGkhConfigSection
    {
        /// <summary>
        /// Начало рабочего дня
        /// </summary>
        [GkhConfigProperty(DisplayName = "Начало рабочего дня")]
        [GkhConfigPropertyEditor("B4.ux.form.field.GkhTimeField", "gkhtimefield")]
        public virtual TimeSpan BeginningOfTheDay { get; set; }

        /// <summary>
        /// Окончание рабочего дня
        /// </summary>
        [GkhConfigProperty(DisplayName = "Окончание рабочего дня")]
        [GkhConfigPropertyEditor("B4.ux.form.field.GkhTimeField", "gkhtimefield")]
        public virtual TimeSpan EndOfTheDay { get; set; }

        /// <summary>
        /// Начало обеденного перерыва
        /// </summary>
        [GkhConfigProperty(DisplayName = "Начало обеденного перерыва")]
        [GkhConfigPropertyEditor("B4.ux.form.field.GkhTimeField", "gkhtimefield")]
        public virtual TimeSpan BeginLunchtime { get; set; }

        /// <summary>
        /// Окончание обеденного перерыва
        /// </summary>
        [GkhConfigProperty(DisplayName = "Окончание обеденного перерыва ")]
        [GkhConfigPropertyEditor("B4.ux.form.field.GkhTimeField", "gkhtimefield")]
        public virtual TimeSpan EndLunchtime { get; set; }

        /// <summary>
        /// Дата проверки
        /// </summary>
        [Permissionable]
        [GkhConfigProperty(DisplayName = "Дата проверки")]
        [DefaultValue(CheckDateType.WithIndustrialCalendar)]
        public virtual CheckDateType CheckDateType { get; set; }
    }
}
