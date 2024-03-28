namespace Bars.Gkh.ConfigSections.Cr
{
    using System.ComponentModel;

    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.Cr.Enums;

    /// <summary>
    /// Конфигурация ДПКР
    /// </summary>
    public class DpkrConfig : IGkhConfigSection
    {
        /// <summary>
        /// Дефектные ведомости - Форма дефектной ведомости
        /// </summary>
        [GkhConfigProperty]
        [Display("Форма дефектной ведомости")]
        [Group("Дефектные ведомости")]
        [DefaultValue(TypeDefectListView.WithOverhaulData)]
        public virtual TypeDefectListView TypeDefectListView { get; set; }

        /// <summary>
        /// Дефектные ведомости - Проверка стоимости с Долгосрочной программой
        /// </summary>
        [GkhConfigProperty]
        [Display("Проверка стоимости с Долгосрочной программой")]
        [Group("Дефектные ведомости")]
        [DefaultValue(TypeChecking.Check)]
        public virtual TypeChecking TypeOverhaulSumChecking { get; set; }

        /// <summary>
        /// Отображение данных по ДПКР
        /// </summary>
        [GkhConfigProperty]
        [Display("Отображение данных по ДПКР")]
        [Group("Виды работ")]
        [DefaultValue(DisplayDataByDpkr.Previous)]
        public virtual DisplayDataByDpkr DisplayDataByDpkr { get; set; }

        /// <summary>
        /// Виды работ - Ввод данных по Краткосрочной программе
        /// </summary>
        [GkhConfigProperty]
        [Display("Ввод данных по Краткосрочной программе")]
        [Group("Виды работ")]
        [DefaultValue(TypeCheckWork.WithDefectList)]
        public virtual TypeCheckWork TypeCheckWork { get; set; }

        /// <summary>
        /// Виды работ - Добавление работ из ДПКР
        /// </summary>
        [GkhConfigProperty]
        [Display("Добавление работ из ДПКР")]
        [Group("Виды работ")]
        [DefaultValue(AddTypeWorkKind.NotSpecifyBase)]
        public virtual AddTypeWorkKind AddTypeWorkKind { get; set; }

        /// <summary>
        /// Виды работ - Перенос КЭ по одному виду работы
        /// </summary>
        [GkhConfigProperty]
        [Display("Перенос КЭ по одному виду работы")]
        [Group("Виды работ")]
        [DefaultValue(TypeTransfer.NoTransfer)]
        public virtual TypeTransfer TypeWorkTransferType { get; set; }
    }
}