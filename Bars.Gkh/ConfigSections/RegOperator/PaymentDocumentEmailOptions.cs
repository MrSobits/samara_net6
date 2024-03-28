namespace Bars.Gkh.ConfigSections.RegOperator
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Для рассылки квитанций
    /// </summary>
    public class PaymentDocumentEmailOptions : IGkhConfigSection
    {
        /// <summary>
        /// Тема письма
        /// </summary>
        [GkhConfigProperty(DisplayName = "Тема письма")]
        public virtual string Subject { get; set; }

        /// <summary>
        /// Текст письма
        /// </summary>
        [GkhConfigProperty(DisplayName = "Текст письма")]
        [GkhConfigPropertyEditor("Ext.form.TextArea", "textarea")]
        public virtual string Body { get; set; }
    }
}