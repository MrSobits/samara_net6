namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System;
    using System.ComponentModel;
    using Config;
    using Config.Attributes;
    using Enums;

    /// <summary>
    /// Настройка займов
    /// </summary>
    public class LoanConfig : IGkhConfigSection
    {
        /// <summary>
        /// Ежедневное обновление реестра займов в
        /// </summary>
        [GkhConfigProperty(DisplayName = "Ежедневное обновление реестра займов в")]
        [GkhConfigPropertyEditor("B4.ux.form.field.GkhTimeField", "gkhtimefield")]
        public virtual TimeSpan? RepaymentTime { get; set; }

        /// <summary>
        /// Ежедневное обновление реестра займов в
        /// </summary>
        [GkhConfigProperty(DisplayName = "Уровень формирования займов")]
        [DefaultValue(LoanLevel.Region)]
        public virtual LoanLevel Level { get; set; }

        /// <summary>
        /// Расчет необходимых средств
        /// </summary>
        [GkhConfigProperty(DisplayName = "Расчет необходимых средств")]
        [DefaultValue(LoanFormationType.ByPerformedWorkAct)]
        public virtual LoanFormationType LoanFormationType { get; set; }
    }
}