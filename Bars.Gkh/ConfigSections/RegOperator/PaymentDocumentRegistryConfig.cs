namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;

    /// <summary>
    /// Настройка формирования суммы к оплате для документов на оплату
    /// </summary>
    public class PaymentDocumentRegistryConfig : IGkhConfigSection
    {
        /// <summary>
        /// Расчет суммы к оплате для физ.лиц
        /// </summary>
        [Permissionable]
        [DefaultValue(CalcAmountType.SaldoOut)]
        [GkhConfigProperty(DisplayName = "Расчет суммы к оплате для физ.лиц")]
        public virtual CalcAmountType CalcAmountIndividual { get; set; }

        /// <summary>
        /// Расчет суммы к оплате для юр.лиц
        /// </summary>
        [Permissionable]
        [DefaultValue(CalcAmountType.ChargeAndRecalc)]
        [GkhConfigProperty(DisplayName = "Расчет суммы к оплате для юр.лиц")]
        public virtual CalcAmountType CalcAmountLegal { get; set; }
    }
}