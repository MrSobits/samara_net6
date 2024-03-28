namespace Bars.Gkh.ConfigSections.RegOperator.Debtor
{
    using System.Collections.Generic;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.Dto;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Параметры настройки реестра неплательщиков
    /// </summary>
    public class DebtorRegistryConfig : IGkhConfigSection
    {
        private const int MaxWidth = 720;

        /// <summary>
        /// Количество дней просрочки
        /// </summary>
        [GkhConfigProperty(DisplayName = "Количество дней просрочки")]
        [UIExtraParam("maxWidth", DebtorRegistryConfig.MaxWidth)]
        public virtual int ExpirationDaysCount { get; set; }

        /// <summary>
        /// Тип определения суммы задолженности
        /// </summary>
        [GkhConfigProperty(DisplayName = "Тип определения суммы задолженности")]
        [Permissionable]
        [UIExtraParam("maxWidth", DebtorRegistryConfig.MaxWidth)]
        public virtual DebtorSumCheckerType DebtorSumCheckerType { get; set; }

        /// <summary>
        /// Сумма задолженности по базовому тарифу
        /// </summary>
        [GkhConfigProperty(DisplayName = "Сумма задолженности по базовому тарифу")]
        [Permissionable]
        [UIExtraParam("maxWidth", DebtorRegistryConfig.MaxWidth)]
        public virtual decimal DebtSumBaseTariff { get; set; }

        /// <summary>
        /// Сумма задолженности по тарифу решения
        /// </summary>
        [GkhConfigProperty(DisplayName = "Сумма задолженности по тарифу решения")]
        [Permissionable]
        [UIExtraParam("maxWidth", DebtorRegistryConfig.MaxWidth)]
        public virtual decimal DebtSumDecisionTariff { get; set; }

        /// <summary>
        /// Сумма задолженности
        /// </summary>
        [GkhConfigProperty(DisplayName = "Общая сумма задолженности")]
        [Permissionable]
        [UIExtraParam("maxWidth", DebtorRegistryConfig.MaxWidth)]
        public virtual decimal DebtSum { get; set; }

        /// <summary>
        /// Сумма задолженности по пени
        /// </summary>
        [GkhConfigProperty(DisplayName = "Сумма задолженности по пени")]
        [UIExtraParam("maxWidth", DebtorRegistryConfig.MaxWidth)]
        public virtual decimal PenaltyDebt { get; set; }

        /// <summary>
        /// Тип операции
        /// </summary>
        [GkhConfigProperty(DisplayName = "Тип операции")]
        [UIExtraParam("maxWidth", DebtorRegistryConfig.MaxWidth)]
        public virtual DebtorLogicalOperands DebtOperand { get; set; }

        /// <summary>
        /// Статусы ЛС
        /// </summary>
        [GkhConfigProperty(DisplayName = "Статусы ЛС")]
        [GkhConfigPropertyEditor("B4.ux.config.StatesSelectField", "statesselectfield")]
        [UIExtraParam("maxWidth", DebtorRegistryConfig.MaxWidth)]
        public virtual List<StateDto> States { get; set; }
    }
}