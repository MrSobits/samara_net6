namespace Bars.Gkh.Overhaul.Hmao.ConfigSections
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.ConfigSections.Overhaul;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Dto;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections.OverhaulHmao;

    /// <summary>
    /// Настройки версии ДПКР с МО
    /// </summary>
    [GkhConfigSection("Overhaul.OverhaulHmao", DisplayName = "Настройки версии ДПКР с МО", UIParent = typeof(OverhaulConfig))]
    [Permissionable]
    [Navigation]
    public class OverhaulHmaoConfig : IGkhConfigSection
    {
        /// <summary>
        /// Наименование долгосрочной программы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Наименование долгосрочной программы")]
        public virtual string ProgrammName { get; set; }

        /// <summary>
        /// Период долгосрочной программы с
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период долгосрочной программы с")]
        public virtual int ProgrammPeriodStart { get; set; }

        /// <summary>
        /// Период долгосрочной программы по
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период долгосрочной программы по")]
        public virtual int ProgrammPeriodEnd { get; set; }

        /// <summary>
        /// Месяц долгосрочной программы с
        /// </summary>
        [GkhConfigProperty(DisplayName = "Месяц долгосрочной программы с")]
        [Permissionable]
        [DefaultValue(Month.January)]
        public virtual Month ProgrammPeriodMonthStart { get; set; }

        /// <summary>
        /// Период долгосрочной программы по
        /// </summary>
        [GkhConfigProperty(DisplayName = "Месяц долгосрочной программы по")]
        [Permissionable]
        [DefaultValue(Month.December)]
        public virtual Month ProgrammPeriodMonthEnd { get; set; }

        /// <summary>
        /// Государственный заказчик
        /// </summary>
        [GkhConfigProperty(DisplayName = "Государственный заказчик")]
        [Permissionable]
        [GkhConfigPropertyEditor("B4.ux.config.ContragentSelectField", "contragentselectfield")]
        public virtual EntityDto<Contragent> GovCustomer { get; set; }

        /// <summary>
        /// Исполнитель программы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Исполнитель программы")]
        [Permissionable]
        [GkhConfigPropertyEditor("B4.ux.config.ContragentSelectField", "contragentselectfield")]
        public virtual EntityDto<Contragent> Executor { get; set; }

        /// <summary>
        /// Период группировки КЭ (год)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период группировки КЭ (год)")]
        public virtual int GroupByCeoPeriod { get; set; }

        /// <summary>
        /// Расчет стоимости ремонта КЭ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Расчет стоимости ремонта КЭ")]
        [DefaultValue(WorkPriceCalcYear.First)]
        public virtual WorkPriceCalcYear WorkPriceCalcYear { get; set; }

        /// <summary>
        /// Расценки по работам
        /// </summary>
        [GkhConfigProperty(DisplayName = "Расценки по работам")]
        [DefaultValue(WorkPriceDetermineType.WithoutCapitalGroup)]
        public virtual WorkPriceDetermineType WorkPriceDetermineType { get; set; }

        /// <summary>
        /// Способ формирования очередности домов
        /// </summary>
        [GkhConfigProperty(DisplayName = "Способ формирования очередности домов")]
        public virtual TypePriority MethodOfCalculation { get; set; }

        /// <summary>
        /// Расценки по работам
        /// </summary>
        [GkhConfigProperty(DisplayName = "Расчет тарифов и собираемости средств по")]
        [DefaultValue(RateCalcTypeArea.AreaMkd)]
        public virtual RateCalcTypeArea RateCalcTypeArea { get; set; }

        /// <summary>
        /// Период группировки ООИ (год)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период группировки ООИ (год)")]
        public virtual int GroupByRoPeriod { get; set; }

        /// <summary>
        /// Стоимость услуг (% от стоимости СМР)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Стоимость услуг (% от стоимости СМР)")]
        public virtual decimal ServiceCost { get; set; }

        /// <summary>
        /// Годовой процент выплаты по займу
        /// </summary>
        [GkhConfigProperty(DisplayName = "Годовой процент выплаты по займу")]
        public virtual decimal YearPercent { get; set; }

        /// <summary>
        /// Период планирования бюджета (год)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Период планирования бюджета (год)")]
        public virtual int ShortTermProgPeriod { get; set; }

        /// <summary>
        /// Дома в программе
        /// </summary>
        [GkhConfigProperty(DisplayName = "Дома в программе")]
        public virtual HouseTypesConfig HouseTypesConfig { get; set; }

        /// <summary>
        /// Ограничения добавления домов в программу
        /// </summary>
        [GkhConfigProperty(DisplayName = "Ограничения добавления домов в программу")]
        public virtual HouseAddInProgramConfig HouseAddInProgramConfig { get; set; }

        /// <summary>
        /// Субсидирование
        /// </summary>
        [GkhConfigProperty(DisplayName = "Субсидирование")]
        public virtual SubsidyConfig SubsidyConfig { get; set; }

        /// <summary>
        /// Опубликованная программа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Опубликованная программа")]
        public virtual PublishProgramConfig PublishProgramConfig { get; set; }

        /// <summary>
        /// Актуализация ДПКР
        /// </summary>
        [GkhConfigProperty(DisplayName = "Актуализация ДПКР")]
        public virtual ActualizeConfig ActualizeConfig { get; set; }

        /// <summary>
        /// Использовать период долгосрочной программы при расчете экономически обоснованного тарифа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Использовать период долгосрочной программы при расчете экономически обоснованного тарифа")]
        [DefaultValue(true)]
        public virtual bool UsePeriodInRateCalc { get; set; }
    }
}