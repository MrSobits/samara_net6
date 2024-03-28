namespace ExportChargesProxy
{
    using System.Xml.Serialization;

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Charge/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Charge/2.5.0", IsNullable = false)]
    public partial class Payer : PayerType1
    {

        private string payerNameField;

        private string additionalPayerIdentifierField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string payerName
        {
            get
            {
                return this.payerNameField;
            }
            set
            {
                this.payerNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string additionalPayerIdentifier
        {
            get
            {
                return this.additionalPayerIdentifierField;
            }
            set
            {
                this.additionalPayerIdentifierField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "PayerType", Namespace = "http://roskazna.ru/gisgmp/xsd/Charge/2.5.0")]
    public partial class PayerType1 : PayerType
    {
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PayerType1))]
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
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Charge/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute("AdditionalOffense", Namespace = "http://roskazna.ru/gisgmp/xsd/Charge/2.5.0", IsNullable = false)]
    public partial class OffenseType
    {

        private System.DateTime offenseDateField;

        private string offensePlaceField;

        private string legalActField;

        private string digitalLinkField;

        private string departmentNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime offenseDate
        {
            get
            {
                return this.offenseDateField;
            }
            set
            {
                this.offenseDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string offensePlace
        {
            get
            {
                return this.offensePlaceField;
            }
            set
            {
                this.offensePlaceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string legalAct
        {
            get
            {
                return this.legalActField;
            }
            set
            {
                this.legalActField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string digitalLink
        {
            get
            {
                return this.digitalLinkField;
            }
            set
            {
                this.digitalLinkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string departmentName
        {
            get
            {
                return this.departmentNameField;
            }
            set
            {
                this.departmentNameField = value;
            }
        }
    }

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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-charges/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-charges/2.5.0", IsNullable = false)]
    public partial class ExportChargesRequest : ExportRequestType
    {

        private EsiaUserInfoType esiaUserInfoField;

        private ChargesExportConditions chargesExportConditionsField;

        private ExportChargesRequestExternal externalField;

        private bool externalFieldSpecified;

        /// <remarks/>
        public EsiaUserInfoType EsiaUserInfo
        {
            get
            {
                return this.esiaUserInfoField;
            }
            set
            {
                this.esiaUserInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
        public ChargesExportConditions ChargesExportConditions
        {
            get
            {
                return this.chargesExportConditionsField;
            }
            set
            {
                this.chargesExportConditionsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ExportChargesRequestExternal external
        {
            get
            {
                return this.externalField;
            }
            set
            {
                this.externalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool externalSpecified
        {
            get
            {
                return this.externalFieldSpecified;
            }
            set
            {
                this.externalFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class EsiaUserInfoType
    {

        private object itemField;

        private string userIdField;

        private string sessionIndexField;

        private System.DateTime sessionDateField;

        private bool sessionDateFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("IndividualBusiness", typeof(EsiaUserInfoTypeIndividualBusiness))]
        [System.Xml.Serialization.XmlElementAttribute("Person", typeof(EsiaUserInfoTypePerson))]
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
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string userId
        {
            get
            {
                return this.userIdField;
            }
            set
            {
                this.userIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string sessionIndex
        {
            get
            {
                return this.sessionIndexField;
            }
            set
            {
                this.sessionIndexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime sessionDate
        {
            get
            {
                return this.sessionDateField;
            }
            set
            {
                this.sessionDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool sessionDateSpecified
        {
            get
            {
                return this.sessionDateFieldSpecified;
            }
            set
            {
                this.sessionDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class EsiaUserInfoTypeIndividualBusiness
    {

        private string personINNField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string personINN
        {
            get
            {
                return this.personINNField;
            }
            set
            {
                this.personINNField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class EsiaUserInfoTypePerson
    {

        private EsiaUserInfoTypePersonDocumentIdentity documentIdentityField;

        private string snilsField;

        private string personINNField;

        /// <remarks/>
        public EsiaUserInfoTypePersonDocumentIdentity DocumentIdentity
        {
            get
            {
                return this.documentIdentityField;
            }
            set
            {
                this.documentIdentityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string snils
        {
            get
            {
                return this.snilsField;
            }
            set
            {
                this.snilsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string personINN
        {
            get
            {
                return this.personINNField;
            }
            set
            {
                this.personINNField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class EsiaUserInfoTypePersonDocumentIdentity
    {

        private EsiaUserInfoTypePersonDocumentIdentityCode codeField;

        private string seriesField;

        private string numberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public EsiaUserInfoTypePersonDocumentIdentityCode code
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
        public string series
        {
            get
            {
                return this.seriesField;
            }
            set
            {
                this.seriesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public enum EsiaUserInfoTypePersonDocumentIdentityCode
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-charges/2.5.0")]
    public enum ExportChargesRequestExternal
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        Item0,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-charges/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-charges/2.5.0", IsNullable = false)]
    public partial class ExportChargesResponse : ResponseType
    {

        private object[] itemsField;

        private bool hasMoreField;

        private bool needReRequestField;

        public ExportChargesResponse()
        {
            this.needReRequestField = false;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ChargeInfo", typeof(ExportChargesResponseChargeInfo))]
        [System.Xml.Serialization.XmlElementAttribute("ChargeOffense", typeof(ExportChargesResponseChargeOffense))]
        public object[] Items
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool needReRequest
        {
            get
            {
                return this.needReRequestField;
            }
            set
            {
                this.needReRequestField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-charges/2.5.0")]
    public partial class ExportChargesResponseChargeInfo : ChargeType
    {

        private ExportChargesResponseChargeInfoReconcileWithoutPayment[] reconcileWithoutPaymentField;

        private ChargeDetailsType chargeDetailsField;

        private string[] excludePaymentsField;

        private ChangeStatusInfo changeStatusInfoField;

        private long amountToPayField;

        private string acknowledgmentStatusField;

        private string requisiteCheckCodeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ReconcileWithoutPayment")]
        public ExportChargesResponseChargeInfoReconcileWithoutPayment[] ReconcileWithoutPayment
        {
            get
            {
                return this.reconcileWithoutPaymentField;
            }
            set
            {
                this.reconcileWithoutPaymentField = value;
            }
        }

        /// <remarks/>
        public ChargeDetailsType ChargeDetails
        {
            get
            {
                return this.chargeDetailsField;
            }
            set
            {
                this.chargeDetailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PaymentId", IsNullable = false)]
        public string[] ExcludePayments
        {
            get
            {
                return this.excludePaymentsField;
            }
            set
            {
                this.excludePaymentsField = value;
            }
        }

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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long amountToPay
        {
            get
            {
                return this.amountToPayField;
            }
            set
            {
                this.amountToPayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string acknowledgmentStatus
        {
            get
            {
                return this.acknowledgmentStatusField;
            }
            set
            {
                this.acknowledgmentStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string requisiteCheckCode
        {
            get
            {
                return this.requisiteCheckCodeField;
            }
            set
            {
                this.requisiteCheckCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-charges/2.5.0")]
    public partial class ExportChargesResponseChargeInfoReconcileWithoutPayment
    {

        private string reconcileIDField;

        private ulong amountReconcileField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string reconcileID
        {
            get
            {
                return this.reconcileIDField;
            }
            set
            {
                this.reconcileIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong amountReconcile
        {
            get
            {
                return this.amountReconcileField;
            }
            set
            {
                this.amountReconcileField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class ChargeDetailsType
    {

        private ChargeDetailsTypeDetailsInfo[] detailsInfoField;

        private ulong amountBalanceField;

        private ulong amountDebtField;

        private bool amountDebtFieldSpecified;

        private ulong amountUpcomingField;

        private bool amountUpcomingFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DetailsInfo")]
        public ChargeDetailsTypeDetailsInfo[] DetailsInfo
        {
            get
            {
                return this.detailsInfoField;
            }
            set
            {
                this.detailsInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong amountBalance
        {
            get
            {
                return this.amountBalanceField;
            }
            set
            {
                this.amountBalanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong amountDebt
        {
            get
            {
                return this.amountDebtField;
            }
            set
            {
                this.amountDebtField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool amountDebtSpecified
        {
            get
            {
                return this.amountDebtFieldSpecified;
            }
            set
            {
                this.amountDebtFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong amountUpcoming
        {
            get
            {
                return this.amountUpcomingField;
            }
            set
            {
                this.amountUpcomingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool amountUpcomingSpecified
        {
            get
            {
                return this.amountUpcomingFieldSpecified;
            }
            set
            {
                this.amountUpcomingFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class ChargeDetailsTypeDetailsInfo
    {

        private string typeField;

        private string nameField;

        private ulong amountPaymentsField;

        private System.DateTime dueDateField;

        private bool dueDateFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
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
        public ulong amountPayments
        {
            get
            {
                return this.amountPaymentsField;
            }
            set
            {
                this.amountPaymentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime dueDate
        {
            get
            {
                return this.dueDateField;
            }
            set
            {
                this.dueDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool dueDateSpecified
        {
            get
            {
                return this.dueDateFieldSpecified;
            }
            set
            {
                this.dueDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Charge/2.5.0")]
    public partial class ChargeType : AbstractChargtType
    {

        private string[] linkedChargesIdentifiersField;

        private Payee payeeField;

        private Payer payerField;

        private BudgetIndexType budgetIndexField;

        private ExecutiveProcedureInfoType executiveProcedureInfoField;

        private OffenseType additionalOffenseField;

        private DiscountType itemField;

        private AdditionalDataType[] additionalDataField;

        private string supplierBillIDField;

        private System.DateTime billDateField;

        private System.DateTime validUntilField;

        private bool validUntilFieldSpecified;

        private ulong totalAmountField;

        private string purposeField;

        private string kbkField;

        private string oktmoField;

        private System.DateTime deliveryDateField;

        private bool deliveryDateFieldSpecified;

        private string legalActField;

        private System.DateTime paymentTermField;

        private bool paymentTermFieldSpecified;

        private string originField;

        private string noticeTermField;

        private string oKVEDField;

        private string chargeOffenseField;

        public ChargeType()
        {
            this.chargeOffenseField = "1";
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("SupplierBillID", IsNullable = false)]
        public string[] LinkedChargesIdentifiers
        {
            get
            {
                return this.linkedChargesIdentifiersField;
            }
            set
            {
                this.linkedChargesIdentifiersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Organization/2.5.0")]
        public Payee Payee
        {
            get
            {
                return this.payeeField;
            }
            set
            {
                this.payeeField = value;
            }
        }

        /// <remarks/>
        public Payer Payer
        {
            get
            {
                return this.payerField;
            }
            set
            {
                this.payerField = value;
            }
        }

        /// <remarks/>
        public BudgetIndexType BudgetIndex
        {
            get
            {
                return this.budgetIndexField;
            }
            set
            {
                this.budgetIndexField = value;
            }
        }

        /// <remarks/>
        public ExecutiveProcedureInfoType ExecutiveProcedureInfo
        {
            get
            {
                return this.executiveProcedureInfoField;
            }
            set
            {
                this.executiveProcedureInfoField = value;
            }
        }

        /// <remarks/>
        public OffenseType AdditionalOffense
        {
            get
            {
                return this.additionalOffenseField;
            }
            set
            {
                this.additionalOffenseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DiscountFixed", typeof(DiscountFixed), Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        [System.Xml.Serialization.XmlElementAttribute("DiscountSize", typeof(DiscountSize), Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        [System.Xml.Serialization.XmlElementAttribute("MultiplierSize", typeof(MultiplierSize), Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        public DiscountType Item
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
        public string supplierBillID
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime billDate
        {
            get
            {
                return this.billDateField;
            }
            set
            {
                this.billDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime validUntil
        {
            get
            {
                return this.validUntilField;
            }
            set
            {
                this.validUntilField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool validUntilSpecified
        {
            get
            {
                return this.validUntilFieldSpecified;
            }
            set
            {
                this.validUntilFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong totalAmount
        {
            get
            {
                return this.totalAmountField;
            }
            set
            {
                this.totalAmountField = value;
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
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime deliveryDate
        {
            get
            {
                return this.deliveryDateField;
            }
            set
            {
                this.deliveryDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryDateSpecified
        {
            get
            {
                return this.deliveryDateFieldSpecified;
            }
            set
            {
                this.deliveryDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string legalAct
        {
            get
            {
                return this.legalActField;
            }
            set
            {
                this.legalActField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime paymentTerm
        {
            get
            {
                return this.paymentTermField;
            }
            set
            {
                this.paymentTermField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool paymentTermSpecified
        {
            get
            {
                return this.paymentTermFieldSpecified;
            }
            set
            {
                this.paymentTermFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string origin
        {
            get
            {
                return this.originField;
            }
            set
            {
                this.originField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string noticeTerm
        {
            get
            {
                return this.noticeTermField;
            }
            set
            {
                this.noticeTermField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string OKVED
        {
            get
            {
                return this.oKVEDField;
            }
            set
            {
                this.oKVEDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string chargeOffense
        {
            get
            {
                return this.chargeOffenseField;
            }
            set
            {
                this.chargeOffenseField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class BudgetIndexType
    {

        private string statusField;

        private string paytReasonField;

        private string taxPeriodField;

        private string taxDocNumberField;

        private string taxDocDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string paytReason
        {
            get
            {
                return this.paytReasonField;
            }
            set
            {
                this.paytReasonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string taxPeriod
        {
            get
            {
                return this.taxPeriodField;
            }
            set
            {
                this.taxPeriodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string taxDocNumber
        {
            get
            {
                return this.taxDocNumberField;
            }
            set
            {
                this.taxDocNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string taxDocDate
        {
            get
            {
                return this.taxDocDateField;
            }
            set
            {
                this.taxDocDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class ExecutiveProcedureInfoType
    {

        private DeedInfo deedInfoField;

        private ExecutiveProcedureInfoTypeExecutOrgan executOrganField;

        private ExecutiveProcedureInfoTypeDebtor debtorField;

        private string idDeloNoField;

        private string deloPlaceField;

        private System.DateTime idDesDateField;

        private System.DateTime aktDateField;

        private string srokPrIspField;

        private string srokPrIspTypeField;

        private string claimerAdrField;

        private System.DateTime notifFSSPDateField;

        /// <remarks/>
        public DeedInfo DeedInfo
        {
            get
            {
                return this.deedInfoField;
            }
            set
            {
                this.deedInfoField = value;
            }
        }

        /// <remarks/>
        public ExecutiveProcedureInfoTypeExecutOrgan ExecutOrgan
        {
            get
            {
                return this.executOrganField;
            }
            set
            {
                this.executOrganField = value;
            }
        }

        /// <remarks/>
        public ExecutiveProcedureInfoTypeDebtor Debtor
        {
            get
            {
                return this.debtorField;
            }
            set
            {
                this.debtorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string idDeloNo
        {
            get
            {
                return this.idDeloNoField;
            }
            set
            {
                this.idDeloNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string deloPlace
        {
            get
            {
                return this.deloPlaceField;
            }
            set
            {
                this.deloPlaceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime idDesDate
        {
            get
            {
                return this.idDesDateField;
            }
            set
            {
                this.idDesDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime aktDate
        {
            get
            {
                return this.aktDateField;
            }
            set
            {
                this.aktDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string srokPrIsp
        {
            get
            {
                return this.srokPrIspField;
            }
            set
            {
                this.srokPrIspField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string srokPrIspType
        {
            get
            {
                return this.srokPrIspTypeField;
            }
            set
            {
                this.srokPrIspTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string claimerAdr
        {
            get
            {
                return this.claimerAdrField;
            }
            set
            {
                this.claimerAdrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime notifFSSPDate
        {
            get
            {
                return this.notifFSSPDateField;
            }
            set
            {
                this.notifFSSPDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class ExecutiveProcedureInfoTypeExecutOrgan
    {

        private string organOkoguField;

        private string organCodeField;

        private string organField;

        private string organAdrField;

        private string organSignCodePostField;

        private string organSignField;

        private string organSignFIOField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string organOkogu
        {
            get
            {
                return this.organOkoguField;
            }
            set
            {
                this.organOkoguField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string organCode
        {
            get
            {
                return this.organCodeField;
            }
            set
            {
                this.organCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string organ
        {
            get
            {
                return this.organField;
            }
            set
            {
                this.organField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string organAdr
        {
            get
            {
                return this.organAdrField;
            }
            set
            {
                this.organAdrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string organSignCodePost
        {
            get
            {
                return this.organSignCodePostField;
            }
            set
            {
                this.organSignCodePostField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string organSign
        {
            get
            {
                return this.organSignField;
            }
            set
            {
                this.organSignField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string organSignFIO
        {
            get
            {
                return this.organSignFIOField;
            }
            set
            {
                this.organSignFIOField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class ExecutiveProcedureInfoTypeDebtor
    {

        private ExecutiveProcedureInfoTypeDebtorPerson personField;

        private string debtorTypeField;

        private string debtorAdrField;

        private string debtorAdrFaktField;

        private string debtorCountryCodeField;

        /// <remarks/>
        public ExecutiveProcedureInfoTypeDebtorPerson Person
        {
            get
            {
                return this.personField;
            }
            set
            {
                this.personField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string debtorType
        {
            get
            {
                return this.debtorTypeField;
            }
            set
            {
                this.debtorTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string debtorAdr
        {
            get
            {
                return this.debtorAdrField;
            }
            set
            {
                this.debtorAdrField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string debtorAdrFakt
        {
            get
            {
                return this.debtorAdrFaktField;
            }
            set
            {
                this.debtorAdrFaktField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string debtorCountryCode
        {
            get
            {
                return this.debtorCountryCodeField;
            }
            set
            {
                this.debtorCountryCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class ExecutiveProcedureInfoTypeDebtorPerson
    {

        private string debtorRegPlaceField;

        private System.DateTime debtorBirthDateField;

        private bool debtorBirthDateFieldSpecified;

        private ExecutiveProcedureInfoTypeDebtorPersonDebtorGender debtorGenderField;

        private string debtorBirthPlaceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string debtorRegPlace
        {
            get
            {
                return this.debtorRegPlaceField;
            }
            set
            {
                this.debtorRegPlaceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime debtorBirthDate
        {
            get
            {
                return this.debtorBirthDateField;
            }
            set
            {
                this.debtorBirthDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool debtorBirthDateSpecified
        {
            get
            {
                return this.debtorBirthDateFieldSpecified;
            }
            set
            {
                this.debtorBirthDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ExecutiveProcedureInfoTypeDebtorPersonDebtorGender debtorGender
        {
            get
            {
                return this.debtorGenderField;
            }
            set
            {
                this.debtorGenderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string debtorBirthPlace
        {
            get
            {
                return this.debtorBirthPlaceField;
            }
            set
            {
                this.debtorBirthPlaceField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public enum ExecutiveProcedureInfoTypeDebtorPersonDebtorGender
    {

        /// <remarks/>
        мужской,

        /// <remarks/>
        женский,
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ChargeTemplateType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ChargeType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Charge/2.5.0")]
    public abstract partial class AbstractChargtType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Charge/2.5.0")]
    public partial class ChargeTemplateType : AbstractChargtType
    {

        private Payee payeeField;

        private Payer payerField;

        private BudgetIndexType budgetIndexField;

        private DiscountType itemField;

        private AdditionalDataType[] additionalDataField;

        private string supplierBillIDField;

        private System.DateTime billDateField;

        private System.DateTime validUntilField;

        private bool validUntilFieldSpecified;

        private ulong totalAmountField;

        private string purposeField;

        private string kbkField;

        private string oktmoField;

        private System.DateTime deliveryDateField;

        private bool deliveryDateFieldSpecified;

        private string legalActField;

        private System.DateTime paymentTermField;

        private bool paymentTermFieldSpecified;

        private string originField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Organization/2.5.0")]
        public Payee Payee
        {
            get
            {
                return this.payeeField;
            }
            set
            {
                this.payeeField = value;
            }
        }

        /// <remarks/>
        public Payer Payer
        {
            get
            {
                return this.payerField;
            }
            set
            {
                this.payerField = value;
            }
        }

        /// <remarks/>
        public BudgetIndexType BudgetIndex
        {
            get
            {
                return this.budgetIndexField;
            }
            set
            {
                this.budgetIndexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DiscountFixed", typeof(DiscountFixed), Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        [System.Xml.Serialization.XmlElementAttribute("DiscountSize", typeof(DiscountSize), Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        [System.Xml.Serialization.XmlElementAttribute("MultiplierSize", typeof(MultiplierSize), Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        public DiscountType Item
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
        public string supplierBillID
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime billDate
        {
            get
            {
                return this.billDateField;
            }
            set
            {
                this.billDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime validUntil
        {
            get
            {
                return this.validUntilField;
            }
            set
            {
                this.validUntilField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool validUntilSpecified
        {
            get
            {
                return this.validUntilFieldSpecified;
            }
            set
            {
                this.validUntilFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ulong totalAmount
        {
            get
            {
                return this.totalAmountField;
            }
            set
            {
                this.totalAmountField = value;
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
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime deliveryDate
        {
            get
            {
                return this.deliveryDateField;
            }
            set
            {
                this.deliveryDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool deliveryDateSpecified
        {
            get
            {
                return this.deliveryDateFieldSpecified;
            }
            set
            {
                this.deliveryDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string legalAct
        {
            get
            {
                return this.legalActField;
            }
            set
            {
                this.legalActField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime paymentTerm
        {
            get
            {
                return this.paymentTermField;
            }
            set
            {
                this.paymentTermField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool paymentTermSpecified
        {
            get
            {
                return this.paymentTermFieldSpecified;
            }
            set
            {
                this.paymentTermFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string origin
        {
            get
            {
                return this.originField;
            }
            set
            {
                this.originField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-charges/2.5.0")]
    public partial class ExportChargesResponseChargeOffense
    {

        private OffenseType additionalOffenseField;

        private string supplierBillIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Charge/2.5.0")]
        public OffenseType AdditionalOffense
        {
            get
            {
                return this.additionalOffenseField;
            }
            set
            {
                this.additionalOffenseField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string supplierBillID
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