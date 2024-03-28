namespace ExportRefundsProxy
{
    using System.Xml.Serialization;

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute("AdditionalData", Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0", IsNullable = false)]
    public partial class AdditionalDataType
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class BankType
    {

        private string nameField;

        private string bikField;

        private string correspondentBankAccountField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string bik
        {
            get
            {
                return this.bikField;
            }
            set
            {
                this.bikField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string correspondentBankAccount
        {
            get
            {
                return this.correspondentBankAccountField;
            }
            set
            {
                this.correspondentBankAccountField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class AccountType
    {

        private BankType bankField;

        private string accountNumberField;

        /// <remarks/>
        public BankType Bank
        {
            get
            {
                return this.bankField;
            }
            set
            {
                this.bankField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string accountNumber
        {
            get
            {
                return this.accountNumberField;
            }
            set
            {
                this.accountNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public abstract partial class DiscountType
    {

        private string valueField;

        private string expiryField;

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public string Expiry
        {
            get
            {
                return this.expiryField;
            }
            set
            {
                this.expiryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public abstract partial class ChangeStatusType
    {

        private string meaningField;

        private string reasonField;

        private System.DateTime changeDateField;

        private bool changeDateFieldSpecified;

        /// <remarks/>
        public string Meaning
        {
            get
            {
                return this.meaningField;
            }
            set
            {
                this.meaningField = value;
            }
        }

        /// <remarks/>
        public string Reason
        {
            get
            {
                return this.reasonField;
            }
            set
            {
                this.reasonField = value;
            }
        }

        /// <remarks/>
        public System.DateTime ChangeDate
        {
            get
            {
                return this.changeDateField;
            }
            set
            {
                this.changeDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ChangeDateSpecified
        {
            get
            {
                return this.changeDateFieldSpecified;
            }
            set
            {
                this.changeDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0", IsNullable = false)]
    public partial class ChangeStatus : ChangeStatusType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0", IsNullable = false)]
    public partial class ChangeStatusInfo : ChangeStatusType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0", IsNullable = false)]
    public partial class DiscountSize : DiscountType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0", IsNullable = false)]
    public partial class DiscountFixed : DiscountType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0", IsNullable = false)]
    public partial class MultiplierSize : DiscountType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0", IsNullable = false)]
    public partial class OrgAccount : AccountType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute("TimeInterval", Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0", IsNullable = false)]
    public partial class TimeIntervalType
    {

        private System.DateTime startDateField;

        private System.DateTime endDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime startDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime endDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0", IsNullable = false)]
    public partial class KBKlist
    {

        private string[] kBKField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("KBK")]
        public string[] KBK
        {
            get
            {
                return this.kBKField;
            }
            set
            {
                this.kBKField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0", IsNullable = false)]
    public partial class DeedInfo
    {

        private string iDTypeField;

        private string idDocNoField;

        private System.DateTime idDocDateField;

        private string subjCodeField;

        private string subjNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string IDType
        {
            get
            {
                return this.iDTypeField;
            }
            set
            {
                this.iDTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string idDocNo
        {
            get
            {
                return this.idDocNoField;
            }
            set
            {
                this.idDocNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime idDocDate
        {
            get
            {
                return this.idDocDateField;
            }
            set
            {
                this.idDocDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string subjCode
        {
            get
            {
                return this.subjCodeField;
            }
            set
            {
                this.subjCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string subjName
        {
            get
            {
                return this.subjNameField;
            }
            set
            {
                this.subjNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Organization/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Organization/2.5.0", IsNullable = false)]
    public partial class Payee : OrganizationType
    {

        private OrgAccount orgAccountField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        public OrgAccount OrgAccount
        {
            get
            {
                return this.orgAccountField;
            }
            set
            {
                this.orgAccountField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Organization/2.5.0")]
    public partial class OrganizationType
    {

        private string nameField;

        private string innField;

        private string kppField;

        private string ogrnField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string inn
        {
            get
            {
                return this.innField;
            }
            set
            {
                this.innField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string kpp
        {
            get
            {
                return this.kppField;
            }
            set
            {
                this.kppField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ogrn
        {
            get
            {
                return this.ogrnField;
            }
            set
            {
                this.ogrnField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Organization/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Organization/2.5.0", IsNullable = false)]
    public partial class RefundPayer : OrganizationType
    {

        private string codeUBPField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string codeUBP
        {
            get
            {
                return this.codeUBPField;
            }
            set
            {
                this.codeUBPField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0", IsNullable = false)]
    public partial class ClarificationsExportConditions : Conditions
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    public abstract partial class Conditions
    {

        private object itemField;

        private ItemChoiceType itemElementNameField;

        private string kindField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ChargesConditions", typeof(ChargesConditionsType))]
        [System.Xml.Serialization.XmlElementAttribute("ClarificationsConditions", typeof(ClarificationsConditionsType))]
        [System.Xml.Serialization.XmlElementAttribute("IncomesConditions", typeof(IncomesConditionsType))]
        [System.Xml.Serialization.XmlElementAttribute("PayersConditions", typeof(PayersConditionsType))]
        [System.Xml.Serialization.XmlElementAttribute("PaymentsConditions", typeof(PaymentsConditionsType))]
        [System.Xml.Serialization.XmlElementAttribute("RefundsConditions", typeof(RefundsConditionsType))]
        [System.Xml.Serialization.XmlElementAttribute("TimeConditions", typeof(TimeConditionsType))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string kind
        {
            get
            {
                return this.kindField;
            }
            set
            {
                this.kindField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    public partial class ChargesConditionsType
    {

        private string[] supplierBillIDField;

        private TimeIntervalType timeIntervalField;

        private PaymentMethodType paymentMethodField;

        private bool paymentMethodFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("SupplierBillID")]
        public string[] SupplierBillID
        {
            get
            {
                return this.supplierBillIDField;
            }
            set
            {
                this.supplierBillIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        public TimeIntervalType TimeInterval
        {
            get
            {
                return this.timeIntervalField;
            }
            set
            {
                this.timeIntervalField = value;
            }
        }

        /// <remarks/>
        public PaymentMethodType paymentMethod
        {
            get
            {
                return this.paymentMethodField;
            }
            set
            {
                this.paymentMethodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool paymentMethodSpecified
        {
            get
            {
                return this.paymentMethodFieldSpecified;
            }
            set
            {
                this.paymentMethodFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public enum PaymentMethodType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    public partial class ClarificationsConditionsType
    {

        private string[] clarificationIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ClarificationID")]
        public string[] ClarificationID
        {
            get
            {
                return this.clarificationIDField;
            }
            set
            {
                this.clarificationIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    public partial class IncomesConditionsType
    {

        private string[] incomeIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("IncomeId")]
        public string[] IncomeId
        {
            get
            {
                return this.incomeIdField;
            }
            set
            {
                this.incomeIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    public partial class PayersConditionsType
    {

        private string[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        private TimeIntervalType timeIntervalField;

        private string[] kBKlistField;

        private PayersConditionsTypeBeneficiary[] beneficiaryField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PayerIdentifier", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("PayerInn", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public string[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        public TimeIntervalType TimeInterval
        {
            get
            {
                return this.timeIntervalField;
            }
            set
            {
                this.timeIntervalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        [System.Xml.Serialization.XmlArrayItemAttribute("KBK", IsNullable = false)]
        public string[] KBKlist
        {
            get
            {
                return this.kBKlistField;
            }
            set
            {
                this.kBKlistField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Beneficiary")]
        public PayersConditionsTypeBeneficiary[] Beneficiary
        {
            get
            {
                return this.beneficiaryField;
            }
            set
            {
                this.beneficiaryField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        PayerIdentifier,

        /// <remarks/>
        PayerInn,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    public partial class PayersConditionsTypeBeneficiary
    {

        private string innField;

        private string kppField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string inn
        {
            get
            {
                return this.innField;
            }
            set
            {
                this.innField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string kpp
        {
            get
            {
                return this.kppField;
            }
            set
            {
                this.kppField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    public partial class PaymentsConditionsType
    {

        private string[] paymentIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PaymentId")]
        public string[] PaymentId
        {
            get
            {
                return this.paymentIdField;
            }
            set
            {
                this.paymentIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    public partial class RefundsConditionsType
    {

        private string[] refundIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RefundId")]
        public string[] RefundId
        {
            get
            {
                return this.refundIdField;
            }
            set
            {
                this.refundIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    public partial class TimeConditionsType
    {

        private TimeIntervalType timeIntervalField;

        private TimeConditionsTypeBeneficiary[] beneficiaryField;

        private string[] kBKlistField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        public TimeIntervalType TimeInterval
        {
            get
            {
                return this.timeIntervalField;
            }
            set
            {
                this.timeIntervalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Beneficiary")]
        public TimeConditionsTypeBeneficiary[] Beneficiary
        {
            get
            {
                return this.beneficiaryField;
            }
            set
            {
                this.beneficiaryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        [System.Xml.Serialization.XmlArrayItemAttribute("KBK", IsNullable = false)]
        public string[] KBKlist
        {
            get
            {
                return this.kBKlistField;
            }
            set
            {
                this.kBKlistField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    public partial class TimeConditionsTypeBeneficiary
    {

        private string innField;

        private string kppField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string inn
        {
            get
            {
                return this.innField;
            }
            set
            {
                this.innField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string kpp
        {
            get
            {
                return this.kppField;
            }
            set
            {
                this.kppField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0", IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// <remarks/>
        ChargesConditions,

        /// <remarks/>
        ClarificationsConditions,

        /// <remarks/>
        IncomesConditions,

        /// <remarks/>
        PayersConditions,

        /// <remarks/>
        PaymentsConditions,

        /// <remarks/>
        RefundsConditions,

        /// <remarks/>
        TimeConditions,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0", IsNullable = false)]
    public partial class PaymentsExportConditions : Conditions
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0", IsNullable = false)]
    public partial class ChargesExportConditions : Conditions
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0", IsNullable = false)]
    public partial class QuittancesExportConditions : Conditions
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0", IsNullable = false)]
    public partial class RefundsExportConditions : Conditions
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0", IsNullable = false)]
    public partial class IncomesExportConditions : Conditions
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-refunds/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-refunds/2.5.0", IsNullable = false)]
    public partial class ExportRefundsRequest : ExportRequestType
    {

        private RefundsExportConditions refundsExportConditionsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
        public RefundsExportConditions RefundsExportConditions
        {
            get
            {
                return this.refundsExportConditionsField;
            }
            set
            {
                this.refundsExportConditionsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class ExportRequestType : RequestType
    {

        private PagingType pagingField;

        private string originatorIdField;

        /// <remarks/>
        public PagingType Paging
        {
            get
            {
                return this.pagingField;
            }
            set
            {
                this.pagingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string originatorId
        {
            get
            {
                return this.originatorIdField;
            }
            set
            {
                this.originatorIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class PagingType
    {

        private string pageNumberField;

        private string pageLengthField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "nonNegativeInteger")]
        public string pageNumber
        {
            get
            {
                return this.pageNumberField;
            }
            set
            {
                this.pageNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "nonNegativeInteger")]
        public string pageLength
        {
            get
            {
                return this.pageLengthField;
            }
            set
            {
                this.pageLengthField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ExportRequestType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class RequestType
    {

        private string idField;

        private System.DateTime timestampField;

        private string senderIdentifierField;

        private string senderRoleField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime timestamp
        {
            get
            {
                return this.timestampField;
            }
            set
            {
                this.timestampField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string senderIdentifier
        {
            get
            {
                return this.senderIdentifierField;
            }
            set
            {
                this.senderIdentifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string senderRole
        {
            get
            {
                return this.senderRoleField;
            }
            set
            {
                this.senderRoleField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-refunds/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-refunds/2.5.0", IsNullable = false)]
    public partial class ExportRefundsResponse : ResponseType
    {

        private ExportRefundsResponseRefund[] refundField;

        private bool hasMoreField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Refund")]
        public ExportRefundsResponseRefund[] Refund
        {
            get
            {
                return this.refundField;
            }
            set
            {
                this.refundField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool hasMore
        {
            get
            {
                return this.hasMoreField;
            }
            set
            {
                this.hasMoreField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-refunds/2.5.0")]
    public partial class ExportRefundsResponseRefund : RefundType
    {

        private ChangeStatusInfo changeStatusInfoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        public ChangeStatusInfo ChangeStatusInfo
        {
            get
            {
                return this.changeStatusInfoField;
            }
            set
            {
                this.changeStatusInfoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Refund/2.5.0")]
    public partial class RefundType
    {

        private RefundPayer refundPayerField;

        private RefundTypeRefundApplication refundApplicationField;

        private RefundTypeRefundBasis refundBasisField;

        private RefundTypeRefundPayee refundPayeeField;

        private AdditionalDataType[] additionalDataField;

        private string refundIdField;

        private System.DateTime refundDocDateField;

        private RefundTypeBudgetLevel budgetLevelField;

        private bool budgetLevelFieldSpecified;

        private string kbkField;

        private string oktmoField;

        private string sourceIDDocField;

        private string sourceIDISField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Organization/2.5.0")]
        public RefundPayer RefundPayer
        {
            get
            {
                return this.refundPayerField;
            }
            set
            {
                this.refundPayerField = value;
            }
        }

        /// <remarks/>
        public RefundTypeRefundApplication RefundApplication
        {
            get
            {
                return this.refundApplicationField;
            }
            set
            {
                this.refundApplicationField = value;
            }
        }

        /// <remarks/>
        public RefundTypeRefundBasis RefundBasis
        {
            get
            {
                return this.refundBasisField;
            }
            set
            {
                this.refundBasisField = value;
            }
        }

        /// <remarks/>
        public RefundTypeRefundPayee RefundPayee
        {
            get
            {
                return this.refundPayeeField;
            }
            set
            {
                this.refundPayeeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AdditionalData", Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        public AdditionalDataType[] AdditionalData
        {
            get
            {
                return this.additionalDataField;
            }
            set
            {
                this.additionalDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string refundId
        {
            get
            {
                return this.refundIdField;
            }
            set
            {
                this.refundIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime refundDocDate
        {
            get
            {
                return this.refundDocDateField;
            }
            set
            {
                this.refundDocDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public RefundTypeBudgetLevel budgetLevel
        {
            get
            {
                return this.budgetLevelField;
            }
            set
            {
                this.budgetLevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool budgetLevelSpecified
        {
            get
            {
                return this.budgetLevelFieldSpecified;
            }
            set
            {
                this.budgetLevelFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string kbk
        {
            get
            {
                return this.kbkField;
            }
            set
            {
                this.kbkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string oktmo
        {
            get
            {
                return this.oktmoField;
            }
            set
            {
                this.oktmoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sourceIDDoc
        {
            get
            {
                return this.sourceIDDocField;
            }
            set
            {
                this.sourceIDDocField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sourceIDIS
        {
            get
            {
                return this.sourceIDISField;
            }
            set
            {
                this.sourceIDISField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Refund/2.5.0")]
    public partial class RefundTypeRefundApplication
    {

        private string appNumField;

        private System.DateTime appDateField;

        private string paymentIdField;

        private int cashTypeField;

        private ulong amountField;

        private string purposeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string appNum
        {
            get
            {
                return this.appNumField;
            }
            set
            {
                this.appNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime appDate
        {
            get
            {
                return this.appDateField;
            }
            set
            {
                this.appDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string paymentId
        {
            get
            {
                return this.paymentIdField;
            }
            set
            {
                this.paymentIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int cashType
        {
            get
            {
                return this.cashTypeField;
            }
            set
            {
                this.cashTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string purpose
        {
            get
            {
                return this.purposeField;
            }
            set
            {
                this.purposeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Refund/2.5.0")]
    public partial class RefundTypeRefundBasis
    {

        private string docKindField;

        private string docNumberField;

        private System.DateTime docDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string docKind
        {
            get
            {
                return this.docKindField;
            }
            set
            {
                this.docKindField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string docNumber
        {
            get
            {
                return this.docNumberField;
            }
            set
            {
                this.docNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime docDate
        {
            get
            {
                return this.docDateField;
            }
            set
            {
                this.docDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Refund/2.5.0")]
    public partial class RefundTypeRefundPayee : PayeeType
    {

        private AccountType bankAccountNumberField;

        private string payeeAccountField;

        private string nameField;

        private string kbkField;

        private string oktmoField;

        /// <remarks/>
        public AccountType BankAccountNumber
        {
            get
            {
                return this.bankAccountNumberField;
            }
            set
            {
                this.bankAccountNumberField = value;
            }
        }

        /// <remarks/>
        public string PayeeAccount
        {
            get
            {
                return this.payeeAccountField;
            }
            set
            {
                this.payeeAccountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string kbk
        {
            get
            {
                return this.kbkField;
            }
            set
            {
                this.kbkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string oktmo
        {
            get
            {
                return this.oktmoField;
            }
            set
            {
                this.oktmoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Refund/2.5.0")]
    public partial class PayeeType : PayerType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PayeeType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class PayerType
    {

        private string payerIdentifierField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string payerIdentifier
        {
            get
            {
                return this.payerIdentifierField;
            }
            set
            {
                this.payerIdentifierField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Refund/2.5.0")]
    public enum RefundTypeBudgetLevel
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Item6,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImportPackageResponseType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class ResponseType
    {

        private string idField;

        private string rqIdField;

        private string recipientIdentifierField;

        private System.DateTime timestampField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string RqId
        {
            get
            {
                return this.rqIdField;
            }
            set
            {
                this.rqIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string recipientIdentifier
        {
            get
            {
                return this.recipientIdentifierField;
            }
            set
            {
                this.recipientIdentifierField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime timestamp
        {
            get
            {
                return this.timestampField;
            }
            set
            {
                this.timestampField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class ImportPackageResponseType : ResponseType
    {

        private ImportProtocolType[] importProtocolField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ImportProtocol")]
        public ImportProtocolType[] ImportProtocol
        {
            get
            {
                return this.importProtocolField;
            }
            set
            {
                this.importProtocolField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class ImportProtocolType
    {

        private string entityIDField;

        private string codeField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string entityID
        {
            get
            {
                return this.entityIDField;
            }
            set
            {
                this.entityIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
    }

}