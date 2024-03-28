namespace Bars.Gkh.ConfigSections.RegOperator.PaymentDocument
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Организационно-правовые формы
    /// </summary>
    public class OrganizationForms : IGkhConfigSection
    {
        /// <summary>
        /// Организационно-правовые формы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Элементы группировки")]

        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
    }
}