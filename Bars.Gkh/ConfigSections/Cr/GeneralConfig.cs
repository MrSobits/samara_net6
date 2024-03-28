namespace Bars.Gkh.ConfigSections.Cr
{
    using System.ComponentModel;

    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.Cr.Enums;

    public class GeneralConfig : IGkhConfigSection
    {
        /// <summary>
        /// Дефектная ведомость - Тип ведомости
        /// </summary>
        [GkhConfigProperty]
        [Display("Тип ведомости")]
        [DefaultValue(DefectListUsage.DontUse)]
        [Group("Дефектная ведомость")]
        public virtual DefectListUsage DefectListUsage { get; set; }

        /// <summary>
        /// Виды работ - Проверка объема с объемом в паспорте дома
        /// </summary>
        [GkhConfigProperty]
        [Display("Проверка объема с объемом в паспорте дома")]
        [Group("Виды работ")]
        [DefaultValue(TypeChecking.NoCheck)]
        public virtual TypeChecking TypeVolumeChecking { get; set; }

        /// <summary>
        /// Средства по источникам финансирования - Форма источников финансирования
        /// </summary>
        [GkhConfigProperty]
        [Display("Форма источников финансирования")]
        [Group("Средства по источникам финансирования")]
        [DefaultValue(FormFinanceSource.WithTypeWork)]
        public virtual FormFinanceSource FormFinanceSource { get; set; }

        /// <summary>
        /// Средства по источникам финансирования - Иные источники финансирования
        /// </summary>
        [GkhConfigProperty]
        [Display("Иные источники финансирования")]
        [Group("Средства по источникам финансирования")]
        [DefaultValue(TypeOtherFinSourceCalc.Yes)]
        public virtual TypeOtherFinSourceCalc TypeOtherFinSourceCalc { get; set; }

        /// <summary>
        /// Сметы - Количество смет по работе
        /// </summary>
        [GkhConfigProperty]
        [Display("Количество смет по работе")]
        [Group("Сметы")]
        [DefaultValue(TypeWorkCrCountEstimations.OnlyOne)]
        public virtual TypeWorkCrCountEstimations CountEstimatesByWork { get; set; }

        /// <summary>
        /// Сметы - Тип сметы
        /// </summary>
        [GkhConfigProperty]
        [Display("Тип сметы")]
        [Group("Сметы")]
        [DefaultValue(EstimationTypeParam.DoNotUse)]
        public virtual EstimationTypeParam EstimationTypeParam { get; set; }

        /// <summary>
        /// Отбор подрядных организаций - Тип реестра
        /// </summary>
        [GkhConfigProperty]
        [Display("Тип реестра")]
        [Group("Отбор подрядных организаций")]
        [DefaultValue(TypeBuilderSelection.Qualification)]
        public virtual TypeBuilderSelection BuilderSelection { get; set; }

        /// <summary>
        /// Акты выполненных работ - Нумерация
        /// </summary>
        [GkhConfigProperty]
        [Display("Нумерация")]
        [Group("Акты выполненных работ")]
        public virtual TypeWorkActNumeration TypeWorkActNumeration { get; set; }

        /// <summary>
        /// Протоколы, акты - Порядок добавления документов
        /// </summary>
        [GkhConfigProperty]
        [Display("Порядок добавления документов")]
        [Group("Протоколы, акты")]
        [DefaultValue(DocumentAddingOrder.Use)]
        public virtual DocumentAddingOrder DocumentAddingOrder { get; set; }
    }
}