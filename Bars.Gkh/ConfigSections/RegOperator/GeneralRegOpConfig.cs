namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.Config.Attributes.Validators;
    using Bars.Gkh.ConfigSections.RegOperator.Debtor;
    using Bars.Gkh.Dto;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Раздел Региональный фонд - Общие
    /// </summary>
    public class GeneralRegOpConfig : IGkhConfigSection
    {
        /// <summary>
        /// Импорт лицевых счетов. Ограничить размер загружаемых файлов, Мб
        /// </summary>
        [GkhConfigProperty(DisplayName = "Импорт лицевых счетов. Ограничить размер загружаемых файлов, Мб")]
        [DefaultValue(60)]
        [Permissionable]
        public virtual int ImportPersonalAccountFileSize { get; set; }
        
        /// <summary>
        /// Способ генерации номера ЛС
        /// </summary>
        [GkhConfigProperty(DisplayName = "Способ генерации номера ЛС")]
        [DefaultValue(TypeAccountNumber.Short)]
        [CustomValidationProvider("AccountNumberTypeValidator")]
        public virtual TypeAccountNumber TypeAccountNumber { get; set; }

        /// <summary>
        /// Списывать средства за открытие спец счета
        /// </summary>
        [GkhConfigProperty(DisplayName = "Списывать средства за открытие спец счета")]
        public virtual bool OpenAccCredit { get; set; }

        /// <summary>
        /// Учет типов домов
        /// </summary>
        [GkhConfigProperty(DisplayName = "Учет типов домов")]
        [DefaultValue(UsageRealEstateType.Auto)]
        public virtual UsageRealEstateType UsageRealEstateType { get; set; }

        /// <summary>
        /// Привязка к РКЦ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Привязка к РКЦ")]
        [DefaultValue(CachPaymentCenterConnectionType.ByAccount)]
        public virtual CachPaymentCenterConnectionType CachPaymentCenterConnectionType { get; set; }

        /// <summary>
        /// Неснижаемый размер фонда (%)
        /// </summary>
        [GkhConfigProperty(DisplayName = "Неснижаемый размер фонда (%)")]
        [Range(0, 100)]
        public virtual decimal? FundMinSize { get; set; }

        /// <summary>
        /// Отображение оплаты л/с одним полем
        /// </summary>
        [GkhConfigProperty(DisplayName = "Отображение оплаты л/с одним полем")]
        public virtual bool IsPersonalAccountPaymentSingleField { get; set; }

        /// <summary>
        /// Реестр документов на оплату
        /// </summary>
        [GkhConfigProperty(DisplayName = "Реестр документов на оплату")]
        public virtual PaymentDocumentRegistryConfig PaymentDocumentRegistryConfig { get; set; }

        /// <summary>
        /// Параметры настройки реестра неплательщиков
        /// </summary>
        [GkhConfigProperty(DisplayName = "Параметры настройки реестра неплательщиков", Hidden = true)]
        [Obsolete("Use RegOperatorConfig.DebtorConfig.DebtorRegistryConfig")]
        public virtual DebtorRegistryConfig DebtorRegistryConfig { get; set; }

        /// <summary>
        /// Расчет вести для домов, у которых способ формирования фонда
        /// </summary>
        [GkhConfigProperty(DisplayName = "Расчет вести для домов, у которых способ формирования фонда")]
        public virtual HouseCalculationConfig HouseCalculationConfig { get; set; }

        /// <summary>
        /// Настройки займов
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройки займов")]
        public virtual LoanConfig LoanConfig { get; set; }

        /// <summary>
        /// Настройка зачета средств за ранее выполненные работы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройка зачета средств за ранее выполненные работы")]
        public virtual PerfWorkChargeConfig PerfWorkChargeConfig { get; set; }

        /// <summary>
        /// Блокировка операций
        /// </summary>
        [GkhConfigProperty(DisplayName = "Блокировка операций")]
        [Permissionable]
        public virtual OperationLockConfig OperationLock { get; set; }

        /// <summary>
        /// Ежедневная проверка реестров оплат в
        /// </summary>
        [GkhConfigProperty(DisplayName = "Время ежедневной проверки реестров оплат")]
        [GkhConfigPropertyEditor("B4.ux.form.field.GkhTimeField", "gkhtimefield")]
        public virtual TimeSpan? BankDocumentImportCheckTime { get; set; }

        /// <summary>
        /// Отображение начислений в лицевом счете после расчета
        /// </summary>
        [GkhConfigProperty(DisplayName = "Отображение начислений в лицевом счете после расчета")]
        [Permissionable]
        public virtual DisplayAfterCalculation DisplayAfterCalculation { get; set; }

        /// <summary>
        /// Подтверждать оплаты по статусам ЛС
        /// </summary>
        [GkhConfigProperty(DisplayName = "Подтверждать оплаты по статусам ЛС")]
        [GkhConfigPropertyEditor("B4.ux.config.StatesSelectField", "statesselectfield")]
        public virtual List<StateDto> BankDocumentImportAccountStates { get; set; }

        /// <summary>
        /// Подтверждение банковских выписок через задачи
        /// </summary>
        [GkhConfigProperty(DisplayName = "Подтверждение банковских выписок через задачи")]
        public virtual bool DistributeStatementsOnExecutor { get; set; }

        /// <summary>
        /// Настройка доли собственности
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройка доли собственности")]
        public virtual AreaShareConfig AreaShareConfig { get; set; }
    }
}
