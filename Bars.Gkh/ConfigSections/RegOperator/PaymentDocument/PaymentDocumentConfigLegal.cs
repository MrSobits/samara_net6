namespace Bars.Gkh.ConfigSections.RegOperator
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.ConfigSections.RegOperator.Enums;
    using Config.Attributes.UI;
    using PaymentDocument;

    /// <summary>
    /// Настройка печати квитанций (юр.лица)
    /// </summary>
    //[GkhConfigSection("PaymentDocument", DisplayName = "Настройки печати квитанций?", UIParent = typeof(RegOperatorConfig))]
    [Navigation]
    public class PaymentDocumentConfigLegal : IGkhConfigSection
    {
        /// <summary>
        /// Группировка квитанции юр.лица с 1 помещением
        /// </summary>
        [GkhConfigProperty(DisplayName = "Группировка квитанции юр.лица с 1 помещением")]
        [DefaultValue(GroupingForLegalWithOneOpenRoom.LegalFolder)]
        public virtual GroupingForLegalWithOneOpenRoom GroupingForLegalWithOneOpenRoom { get; set; }

        /// <summary>
        /// Группировка по организационно-правовой форме
        /// </summary>
        [GkhConfigProperty(DisplayName = "Группировка по организационно-правовой форме")]
        [DefaultValue(OrgFormGroup.NoGroup)]
        public virtual OrgFormGroup OrgFormGroup { get; set; }

        /// <summary>
        /// Учитывать ЛС, у которых есть задолженность, но нет начислений
        /// </summary>
        [GkhConfigProperty(DisplayName = "Учитывать ЛС, у которых есть задолженность, но нет начислений")]
        [DefaultValue(AllowAccWithoutCharge.DontAllow)]
        public virtual AllowAccWithoutCharge AllowAccWithoutCharge { get; set; }

        /// <summary>
        /// Организационно-правовые формы
        /// </summary>
        [GkhConfigProperty(DisplayName = "Организационно-правовые формы")]
        [GkhConfigPropertyEditor("B4.ux.config.OrganizationFormsEditor", "organizationformseditor")]
        public virtual List<OrganizationForms> OrganizationForms { get; set; }

        /// <summary>
        /// Настройка номера квитанций для юр. лица
        /// </summary>
        [GkhConfigProperty(DisplayName = "Настройка номера квитанций для юр. лица")]
        [Navigation]
        public virtual PaymentDocumentConfigLegalNumber PaymentDocumentConfigLegalNumber { get; set; }
    }
}