namespace Bars.Gkh.ConfigSections.ClaimWork
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.ClaimWork;

    /// <summary>
    ///     Настройки исковой работы
    /// </summary>
    public class GeneralConfigs : IGkhConfigSection
    {
        /// <summary>
        ///     Тип суда
        /// </summary>
        [GkhConfigProperty(DisplayName = "Тип суда")]
        [DefaultValue(FillType.Manual)]
        public virtual FillType CourtType { get; set; }

        /// <summary>
        ///     Наименование суда
        /// </summary>
        [GkhConfigProperty(DisplayName = "Наименование суда")]
        [DefaultValue(FillType.Manual)]
        public virtual FillType CourtName { get; set; }

        /// <summary>
        ///     Служба судебных приставов
        /// </summary>
        [GkhConfigProperty(DisplayName = "Служба судебных приставов")]
        [DefaultValue(FillType.Manual)]
        public virtual FillType BailiffsService { get; set; }

        /// <summary>
        ///     Гос. пошлина
        /// </summary>
        [GkhConfigProperty(DisplayName = "Гос. пошлина")]
        [DefaultValue(CalculationType.Manual)]
        public virtual CalculationType StateFee { get; set; }

        /// <summary>
        /// Погашение задолженности
        /// </summary>
        [GkhConfigProperty(DisplayName = "Погашение задолженности"), DefaultValue(DebtCalc.Early)]
        public virtual DebtCalc DebtCalc { get; set; }

        /// <summary>
        /// Начисление пени
        /// </summary>
        [GkhConfigProperty(DisplayName = "Начисление пени"), DefaultValue(PenaltyCharge.CumulativeTotal)]
        public virtual PenaltyCharge PenaltyCharge { get; set; }
    }
}