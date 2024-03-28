namespace Bars.Gkh.ConfigSections.RegOperator.PaymentPenalties
{
    using System;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;

    /// <summary>
    /// Группа для отображения настроек периода расчёта пени с отсрочкой
    /// </summary>
    public class PenaltyCalcConfig : IGkhConfigSection
    {
        /// <summary>
        /// Расчет пени с отсрочкой
        /// </summary>
        [GkhConfigProperty(DisplayName = "Выполнять расчет пени с отсрочкой")]
        [Permissionable]
        public virtual bool SimpleCalculatePenalty { get; set; }

        /// <summary>
        /// Настройки параметров расчета пени с отсрочкой
        /// </summary>
        [GkhConfigProperty(DisplayName = "Статусы ЛС")]
        [GkhConfigPropertyEditor("B4.ux.config.PenaltiesWithDeferredEditor", "penaltieswithdeferrededitor")]
        public virtual int PenaltiesWithDeferredEditor { get; set; }
    }
}
