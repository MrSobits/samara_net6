namespace Bars.Gkh.ConfigSections.RegOperator.PaymentPenalties
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Раздел Региональный фонд - Параметры начисления пени
    /// </summary>
    public class PaymentPenaltiesNodeConfig : IGkhConfigSection
    {
        /// <summary>
        /// Списывать средства за открытие спец счета
        /// </summary>
        [GkhConfigProperty(DisplayName = "Расчитывать пени")]
        public virtual bool CalculatePenalty { get; set; }

        /// <summary>
        /// Списывать средства за открытие спец счета
        /// </summary>
        [GkhConfigProperty(DisplayName = "Перерасчет только за период действия ставки")]
        public virtual bool RecalcPenaltyByCurrentRefinancingRate { get; set; }

        /// <summary>
        /// Расчет количества дней просрочки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Расчет количества дней просрочки"), Permissionable, DefaultValue(NumberDaysDelay.StartDateMonth)]
        public virtual NumberDaysDelay NumberDaysDelay { get; set; }

        /// <summary>
        /// Начислять пени на доп. взносы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Начислять пени на доп. взносы"), DefaultValue(true)]
        public virtual bool CalculatePenaltyForDecisionTarif { get; set; }

        /// <summary>
        /// Начислять пени на муниципальную собственность раз в год
        /// </summary>
        [GkhConfigProperty(DisplayName = "Начислять пени на муниципальную собственность раз в год"), DefaultValue(true)]
        public virtual bool CalculatePenaltyMunicipalPropertyOnePerYear { get; set; }

        /// <summary>
        /// Расчёт пени с отсрочкой
        /// </summary>
        [GkhConfigProperty(DisplayName = "Расчёт пени с отсрочкой"), Permissionable]
        public virtual PenaltyCalcConfig PenaltyCalcConfig { get; set; }

        /// <summary>
        /// Настройка фиксированного периода расчета пени
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройка фиксированного периода расчета пени")]
        [Permissionable]
        public virtual FixedPeriodCalcPenaltiesConfig FixedPeriodCalcPenaltiesConfig { get; set; }

        /// <summary>
        /// Расчет пени согласно изменению ЖК РФ от 03.07.2016
        /// </summary>
        [GkhConfigProperty(DisplayName = "Расчет пени согласно изменению ЖК РФ от 03.07.2016"), Permissionable]
        public virtual NewPenaltyCalcConfig NewPenaltyCalcConfig { get; set; }

        /// <summary>
        /// Ставка рефинансирования (при отсутствии оплат)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Ставка рефинансирования (при отсутствии оплат)"), Permissionable, DefaultValue(RefinancingRate.CurrentPeriod)]
        public virtual RefinancingRate RefinancingRate { get; set; }

        /// <summary>
        /// Список параметров
        /// </summary>
        [GkhConfigProperty(DisplayName = "Список параметров")]
        public virtual PaymentPenaltiesFieldSetConfig PaymentPenaltiesFieldSetConfig { get; set; }
    }
}