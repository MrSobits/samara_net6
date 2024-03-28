namespace Bars.Gkh.ConfigSections.RegOperator.PaymentPenalties
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Настройка фиксированного периода расчета пени
    /// </summary>
    public class FixedPeriodCalcPenaltiesConfig : IGkhConfigSection
    {
        /// <summary>
        /// Дата вступления в силу
        /// </summary>
        [GkhConfigProperty(DisplayName = "Использовать фиксированный период расчета пени")]
        public virtual bool UseFixCalcPeriod { get; set; }

        /// <summary>
        /// Свойство для грида параметров начисления пени.
        /// Само по себе оно ничего не хранит, указанный едитор привязывается к котроллеру, который и управляет сущностями настраиваемых параметров
        /// Для них сделаны отдельные таблицы в БД и кастомное поведение на клиенте
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройка фиксированного периода расчета")]
        [GkhConfigPropertyEditor("B4.ux.config.FixedPeriodCalcPenaltiesEditor", "fixedperiodcalcpenaltieseditor")]
        public virtual int FixedPeriodCalcPenaltiesEditor { get; set; }
    }
}