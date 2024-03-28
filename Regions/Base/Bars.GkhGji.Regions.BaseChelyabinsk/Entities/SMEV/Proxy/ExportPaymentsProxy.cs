namespace ExportPaymentsProxy
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Payment/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Payment/2.5.0", IsNullable = false)]
    public partial class Payer : PayerType
    {

        private string payerNameField;

        private string payerAccountField;

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
        public string payerAccount
        {
            get
            {
                return this.payerAccountField;
            }
            set
            {
                this.payerAccountField = value;
            }
        }
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
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "PayerType", Namespace = "http://roskazna.ru/gisgmp/xsd/Payment/2.5.0")]
    public partial class PayerType1 : PayerType
    {
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.5.0", IsNullable = false)]
    public partial class ExportPaymentsRequest : ExportRequestType
    {

        private PaymentsExportConditions paymentsExportConditionsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/SearchConditions/2.5.0")]
        public PaymentsExportConditions PaymentsExportConditions
        {
            get
            {
                return this.paymentsExportConditionsField;
            }
            set
            {
                this.paymentsExportConditionsField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.5.0", IsNullable = false)]
    public partial class ExportPaymentsResponse : ResponseType
    {

        private ExportPaymentsResponsePaymentInfo[] paymentInfoField;

        private bool hasMoreField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PaymentInfo")]
        public ExportPaymentsResponsePaymentInfo[] PaymentInfo
        {
            get
            {
                return this.paymentInfoField;
            }
            set
            {
                this.paymentInfoField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.5.0")]
    public partial class ExportPaymentsResponsePaymentInfo : PaymentType
    {

        private ExportPaymentsResponsePaymentInfoAcknowledgmentInfo acknowledgmentInfoField;

        private ExportPaymentsResponsePaymentInfoRefundInfo[] refundInfoField;

        private IncomeInfoType incomeInfoField;

        private ChangeStatusInfo changeStatusInfoField;

        /// <remarks/>
        public ExportPaymentsResponsePaymentInfoAcknowledgmentInfo AcknowledgmentInfo
        {
            get
            {
                return this.acknowledgmentInfoField;
            }
            set
            {
                this.acknowledgmentInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("RefundInfo")]
        public ExportPaymentsResponsePaymentInfoRefundInfo[] RefundInfo
        {
            get
            {
                return this.refundInfoField;
            }
            set
            {
                this.refundInfoField = value;
            }
        }

        /// <remarks/>
        public IncomeInfoType IncomeInfo
        {
            get
            {
                return this.incomeInfoField;
            }
            set
            {
                this.incomeInfoField = value;
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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.5.0")]
    public partial class ExportPaymentsResponsePaymentInfoAcknowledgmentInfo
    {

        private object itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ServiceProvidedInfo", typeof(ExportPaymentsResponsePaymentInfoAcknowledgmentInfoServiceProvidedInfo))]
        [System.Xml.Serialization.XmlElementAttribute("SupplierBillID", typeof(string))]
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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.5.0")]
    public partial class ExportPaymentsResponsePaymentInfoAcknowledgmentInfoServiceProvidedInfo
    {

        private ExportPaymentsResponsePaymentInfoAcknowledgmentInfoServiceProvidedInfoAdditionalRepaymenInfo additionalRepaymenInfoField;

        private string serviceProvidedField;

        /// <remarks/>
        public ExportPaymentsResponsePaymentInfoAcknowledgmentInfoServiceProvidedInfoAdditionalRepaymenInfo AdditionalRepaymenInfo
        {
            get
            {
                return this.additionalRepaymenInfoField;
            }
            set
            {
                this.additionalRepaymenInfoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string serviceProvided
        {
            get
            {
                return this.serviceProvidedField;
            }
            set
            {
                this.serviceProvidedField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.5.0")]
    public partial class ExportPaymentsResponsePaymentInfoAcknowledgmentInfoServiceProvidedInfoAdditionalRepaymenInfo
    {

        private ExportPaymentsResponsePaymentInfoAcknowledgmentInfoServiceProvidedInfoAdditionalRepaymenInfoServiceData[] serviceDataField;

        private long residualAmountField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ServiceData")]
        public ExportPaymentsResponsePaymentInfoAcknowledgmentInfoServiceProvidedInfoAdditionalRepaymenInfoServiceData[] ServiceData
        {
            get
            {
                return this.serviceDataField;
            }
            set
            {
                this.serviceDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long residualAmount
        {
            get
            {
                return this.residualAmountField;
            }
            set
            {
                this.residualAmountField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.5.0")]
    public partial class ExportPaymentsResponsePaymentInfoAcknowledgmentInfoServiceProvidedInfoAdditionalRepaymenInfoServiceData : ServiceDataType
    {

        private string serviceDataIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string serviceDataID
        {
            get
            {
                return this.serviceDataIDField;
            }
            set
            {
                this.serviceDataIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class ServiceDataType
    {

        private ServiceDataTypePersoneOfficial personeOfficialField;

        private ulong amountField;

        private string courtNameField;

        private string lawsuitInfoField;

        /// <remarks/>
        public ServiceDataTypePersoneOfficial personeOfficial
        {
            get
            {
                return this.personeOfficialField;
            }
            set
            {
                this.personeOfficialField = value;
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
        public string courtName
        {
            get
            {
                return this.courtNameField;
            }
            set
            {
                this.courtNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string lawsuitInfo
        {
            get
            {
                return this.lawsuitInfoField;
            }
            set
            {
                this.lawsuitInfoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class ServiceDataTypePersoneOfficial
    {

        private string nameField;

        private string officialPositionField;

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
        public string officialPosition
        {
            get
            {
                return this.officialPositionField;
            }
            set
            {
                this.officialPositionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/export-payments/2.5.0")]
    public partial class ExportPaymentsResponsePaymentInfoRefundInfo
    {

        private string refundIdField;

        private ulong amountField;

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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class IncomeInfoType
    {

        private string[] incomeIdField;

        private string receiptIncomeStatusField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string receiptIncomeStatus
        {
            get
            {
                return this.receiptIncomeStatusField;
            }
            set
            {
                this.receiptIncomeStatusField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Payment/2.5.0")]
    public partial class PaymentType : PaymentBaseType
    {

        private PaymentTypePartialPayt partialPaytField;

        private string paymentIdField;

        private System.DateTime paymentDateField;

        private System.DateTime deliveryDateField;

        private bool deliveryDateFieldSpecified;

        private string eSIA_IDField;

        /// <remarks/>
        public PaymentTypePartialPayt PartialPayt
        {
            get
            {
                return this.partialPaytField;
            }
            set
            {
                this.partialPaytField = value;
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
        public System.DateTime paymentDate
        {
            get
            {
                return this.paymentDateField;
            }
            set
            {
                this.paymentDateField = value;
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
        public string ESIA_ID
        {
            get
            {
                return this.eSIA_IDField;
            }
            set
            {
                this.eSIA_IDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Payment/2.5.0")]
    public partial class PaymentTypePartialPayt
    {

        private AccDocType accDocField;

        private string transKindField;

        private string paytNoField;

        private string transContentField;

        private string sumResidualPaytField;

        /// <remarks/>
        public AccDocType AccDoc
        {
            get
            {
                return this.accDocField;
            }
            set
            {
                this.accDocField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string transKind
        {
            get
            {
                return this.transKindField;
            }
            set
            {
                this.transKindField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string paytNo
        {
            get
            {
                return this.paytNoField;
            }
            set
            {
                this.paytNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string transContent
        {
            get
            {
                return this.transContentField;
            }
            set
            {
                this.transContentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string sumResidualPayt
        {
            get
            {
                return this.sumResidualPaytField;
            }
            set
            {
                this.sumResidualPaytField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    public partial class AccDocType
    {

        private string accDocNoField;

        private System.DateTime accDocDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string accDocNo
        {
            get
            {
                return this.accDocNoField;
            }
            set
            {
                this.accDocNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime accDocDate
        {
            get
            {
                return this.accDocDateField;
            }
            set
            {
                this.accDocDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Payment/2.5.0")]
    public partial class PaymentBaseType
    {

        private PaymentOrgType paymentOrgField;

        private Payer payerField;

        private Payee payeeField;

        private BudgetIndexType budgetIndexField;

        private AccDocType accDocField;

        private AdditionalDataType[] additionalDataField;

        private string supplierBillIDField;

        private string purposeField;

        private ulong amountField;

        private System.DateTime receiptDateField;

        private bool receiptDateFieldSpecified;

        private string kbkField;

        private string oktmoField;

        private string transKindField;

        /// <remarks/>
        public PaymentOrgType PaymentOrg
        {
            get
            {
                return this.paymentOrgField;
            }
            set
            {
                this.paymentOrgField = value;
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
        public AccDocType AccDoc
        {
            get
            {
                return this.accDocField;
            }
            set
            {
                this.accDocField = value;
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
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime receiptDate
        {
            get
            {
                return this.receiptDateField;
            }
            set
            {
                this.receiptDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool receiptDateSpecified
        {
            get
            {
                return this.receiptDateFieldSpecified;
            }
            set
            {
                this.receiptDateFieldSpecified = value;
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
        public string transKind
        {
            get
            {
                return this.transKindField;
            }
            set
            {
                this.transKindField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Organization/2.5.0")]
    public partial class PaymentOrgType
    {

        private object itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Bank", typeof(BankType))]
        [System.Xml.Serialization.XmlElementAttribute("Other", typeof(PaymentOrgTypeOther))]
        [System.Xml.Serialization.XmlElementAttribute("UFK", typeof(string))]
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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Organization/2.5.0")]
    public enum PaymentOrgTypeOther
    {

        /// <remarks/>
        CASH,
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