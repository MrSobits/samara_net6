namespace Bars.Gkh.Overhaul.Hmao.ConfigSections.OverhaulHmao
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;

    /// <summary>
    /// Субсидирование
    /// </summary>
    public class SubsidyConfig : IGkhConfigSection
    {
        /// <summary>
        /// Учет собираемости по дому
        /// </summary>
        [GkhConfigProperty(DisplayName = "Учет собираемости по дому")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage UseRealObjCollection { get; set; }

        /// <summary>
        /// Фиксация опубликованного года
        /// </summary>
        [GkhConfigProperty(DisplayName = "Фиксация опубликованного года")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage UseFixationPublishedYears { get; set; }

        /// <summary>
        /// Фиксация опубликованного года
        /// </summary>
        [GkhConfigProperty(DisplayName = "Приоритет записей при расчета бюджета")]
        [DefaultValue(PriorityBudget.NotFixed)]
        public virtual PriorityBudget PriorityBudget { get; set; }

        /// <summary>
        /// Способ корректировки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Способ корректировки")]
        [DefaultValue(TypeCorrection.WithoutFixYear)]
        public virtual TypeCorrection TypeCorrection { get; set; }

        /// <summary>
        /// Фиксация записей актуализации
        /// </summary>
        [GkhConfigProperty(DisplayName = "Фиксация записей актуализации")]
        [DefaultValue(TypeCorrectionActualizeRecs.NotUsed)]
        public virtual TypeCorrectionActualizeRecs TypeCorrectionActualizeRecs { get; set; }

        /// <summary>
        /// Период корректировки года с
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период корректировки года с")]
        public virtual int CorrectionPeriodStart { get; set; }

        /// <summary>
        /// Исключить попадание новых работ в ближайшие года
        /// </summary>
        [GkhConfigProperty(DisplayName = "Корректировка сдвига года")]
        [DefaultValue(TypeUsage.NoUsed)]
        public virtual TypeUsage UseCorrectionShift { get; set; }

        /// <summary>
        /// Количество лет для исключения попадания
        /// </summary>
        [GkhConfigProperty(DisplayName = "Количество лет")]
        public virtual int ShiftInterval { get; set; }
    }
}