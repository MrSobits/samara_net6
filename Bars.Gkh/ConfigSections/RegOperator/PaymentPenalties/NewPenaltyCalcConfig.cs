namespace Bars.Gkh.ConfigSections.RegOperator.PaymentPenalties
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Расчет пени согласно изменению ЖК РФ от 03.07.2016
    /// </summary>
    public class NewPenaltyCalcConfig : IGkhConfigSection
    {
        /// <summary>
        /// Дата вступления в силу
        /// </summary>
        [GkhConfigProperty(DisplayName = "Дата вступления в силу")]
        [Permissionable]
        [Range(typeof(DateTime), "2016-07-01", "9999-12-01")]
        public virtual DateTime? NewPenaltyCalcStart { get; set; }

        /// <summary>
        /// Допустимая просрочка
        /// </summary>
        [GkhConfigProperty(DisplayName = "Допустимая просрочка")]
        [Permissionable]
        [DefaultValue(30)]
        [ReadOnly(true)]
        [UIExtraParam("maxWidth", 400)]
        public virtual int? NewPenaltyCalcDays { get; set; }
    }
}