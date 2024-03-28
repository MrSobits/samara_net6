namespace ImportChargesProxy
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PayeeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PayerType2))]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Refund/2.5.0")]
    public partial class PayeeType : PayerType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "PayerType", Namespace = "http://roskazna.ru/gisgmp/xsd/Payment/2.5.0")]
    public partial class PayerType2 : PayerType
    {
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0", IsNullable = false)]
    public partial class ChargesPackage : PackageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    public partial class PackageType
    {

        private object[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ImportedChange", typeof(ImportedChangeType))]
        [System.Xml.Serialization.XmlElementAttribute("ImportedCharge", typeof(ImportedChargeType))]
        [System.Xml.Serialization.XmlElementAttribute("ImportedClarification", typeof(ImportedClarificationType))]
        [System.Xml.Serialization.XmlElementAttribute("ImportedIncome", typeof(ImportedIncomeType))]
        [System.Xml.Serialization.XmlElementAttribute("ImportedPayment", typeof(ImportedPaymentType))]
        [System.Xml.Serialization.XmlElementAttribute("ImportedRefund", typeof(ImportedRefundType))]
        [System.Xml.Serialization.XmlElementAttribute("ImportedRenouncement", typeof(ImportedRenouncementType))]
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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    public partial class ImportedChangeType
    {

        private string itemField;

        private ItemChoiceType itemElementNameField;

        private ChangeType[] changeField;

        private ChangeStatus changeStatusField;

        private string originatorIdField;

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ClarificationId", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("IncomeId", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("PaymentId", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("RefundId", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("RenouncementID", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("SupplierBillId", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
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
        [System.Xml.Serialization.XmlElementAttribute("Change")]
        public ChangeType[] Change
        {
            get
            {
                return this.changeField;
            }
            set
            {
                this.changeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
        public ChangeStatus ChangeStatus
        {
            get
            {
                return this.changeStatusField;
            }
            set
            {
                this.changeStatusField = value;
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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0", IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// <remarks/>
        ClarificationId,

        /// <remarks/>
        IncomeId,

        /// <remarks/>
        PaymentId,

        /// <remarks/>
        RefundId,

        /// <remarks/>
        RenouncementID,

        /// <remarks/>
        SupplierBillId,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    public partial class ChangeType
    {

        private ChangeTypeChangeValue[] changeValueField;

        private string fieldNumField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ChangeValue")]
        public ChangeTypeChangeValue[] ChangeValue
        {
            get
            {
                return this.changeValueField;
            }
            set
            {
                this.changeValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string fieldNum
        {
            get
            {
                return this.fieldNumField;
            }
            set
            {
                this.fieldNumField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    public partial class ChangeTypeChangeValue
    {

        private string nameField;

        private string valueField;

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
        public string value
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    public partial class ImportedChargeType : ChargeType
    {

        private string originatorIdField;

        private string idField;

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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImportedChargeType))]
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImportedChargeType))]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    public partial class ImportedClarificationType : ClarificationType
    {

        private string originatorIdField;

        private string idField;

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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImportedClarificationType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Clarification/2.5.0")]
    public partial class ClarificationType : AbstractClarificationType
    {

        private ClarificationApplicationType clarificationApplicationField;

        private SignsClarificationType signsField;

        private string clarificationNumberField;

        private System.DateTime clarificationDateField;

        private string clarificationIdField;

        private string paymentIdField;

        private string supplierBillIDField;

        private string authorAccountField;

        private string authorNameField;

        private string codeUBPField;

        private string mainAuthorNameField;

        private string kbkGlavaCodeField;

        private ClarificationTypeBudgetLevel budgetLevelField;

        private bool budgetLevelFieldSpecified;

        private string okpoField;

        private string finBodyAccountField;

        private string financialBodyField;

        private string tofkNameField;

        private string tofkCodeField;

        private string payerNameField;

        private string payerIdentifierField;

        private string innField;

        private string kppField;

        private string payerDocumentField;

        private string payerAccountField;

        private string findingoutRequestNumField;

        private System.DateTime findingoutRequestDateField;

        private bool findingoutRequestDateFieldSpecified;

        /// <remarks/>
        public ClarificationApplicationType ClarificationApplication
        {
            get
            {
                return this.clarificationApplicationField;
            }
            set
            {
                this.clarificationApplicationField = value;
            }
        }

        /// <remarks/>
        public SignsClarificationType Signs
        {
            get
            {
                return this.signsField;
            }
            set
            {
                this.signsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string clarificationNumber
        {
            get
            {
                return this.clarificationNumberField;
            }
            set
            {
                this.clarificationNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime clarificationDate
        {
            get
            {
                return this.clarificationDateField;
            }
            set
            {
                this.clarificationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string clarificationId
        {
            get
            {
                return this.clarificationIdField;
            }
            set
            {
                this.clarificationIdField = value;
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
        public string authorAccount
        {
            get
            {
                return this.authorAccountField;
            }
            set
            {
                this.authorAccountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string authorName
        {
            get
            {
                return this.authorNameField;
            }
            set
            {
                this.authorNameField = value;
            }
        }

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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string mainAuthorName
        {
            get
            {
                return this.mainAuthorNameField;
            }
            set
            {
                this.mainAuthorNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string kbkGlavaCode
        {
            get
            {
                return this.kbkGlavaCodeField;
            }
            set
            {
                this.kbkGlavaCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ClarificationTypeBudgetLevel budgetLevel
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
        public string okpo
        {
            get
            {
                return this.okpoField;
            }
            set
            {
                this.okpoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string finBodyAccount
        {
            get
            {
                return this.finBodyAccountField;
            }
            set
            {
                this.finBodyAccountField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string financialBody
        {
            get
            {
                return this.financialBodyField;
            }
            set
            {
                this.financialBodyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tofkName
        {
            get
            {
                return this.tofkNameField;
            }
            set
            {
                this.tofkNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tofkCode
        {
            get
            {
                return this.tofkCodeField;
            }
            set
            {
                this.tofkCodeField = value;
            }
        }

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
        public string payerDocument
        {
            get
            {
                return this.payerDocumentField;
            }
            set
            {
                this.payerDocumentField = value;
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string findingoutRequestNum
        {
            get
            {
                return this.findingoutRequestNumField;
            }
            set
            {
                this.findingoutRequestNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime findingoutRequestDate
        {
            get
            {
                return this.findingoutRequestDateField;
            }
            set
            {
                this.findingoutRequestDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool findingoutRequestDateSpecified
        {
            get
            {
                return this.findingoutRequestDateFieldSpecified;
            }
            set
            {
                this.findingoutRequestDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Clarification/2.5.0")]
    public partial class ClarificationApplicationType
    {

        private originalDetailType originalDetailsField;

        private setDetailType setDetailsField;

        private string ordinalNumberField;

        private string applicationNameField;

        private ClarificationApplicationTypeAppCode appCodeField;

        private bool appCodeFieldSpecified;

        private string appNumField;

        private System.DateTime appDateField;

        private bool appDateFieldSpecified;

        private string incomeIdField;

        private string sourceIDDocField;

        private string sourceIDISField;

        private string applicationNumberField;

        private System.DateTime applicationDateField;

        private bool applicationDateFieldSpecified;

        /// <remarks/>
        public originalDetailType OriginalDetails
        {
            get
            {
                return this.originalDetailsField;
            }
            set
            {
                this.originalDetailsField = value;
            }
        }

        /// <remarks/>
        public setDetailType SetDetails
        {
            get
            {
                return this.setDetailsField;
            }
            set
            {
                this.setDetailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ordinalNumber
        {
            get
            {
                return this.ordinalNumberField;
            }
            set
            {
                this.ordinalNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string applicationName
        {
            get
            {
                return this.applicationNameField;
            }
            set
            {
                this.applicationNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ClarificationApplicationTypeAppCode appCode
        {
            get
            {
                return this.appCodeField;
            }
            set
            {
                this.appCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool appCodeSpecified
        {
            get
            {
                return this.appCodeFieldSpecified;
            }
            set
            {
                this.appCodeFieldSpecified = value;
            }
        }

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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool appDateSpecified
        {
            get
            {
                return this.appDateFieldSpecified;
            }
            set
            {
                this.appDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string incomeId
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string applicationNumber
        {
            get
            {
                return this.applicationNumberField;
            }
            set
            {
                this.applicationNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime applicationDate
        {
            get
            {
                return this.applicationDateField;
            }
            set
            {
                this.applicationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool applicationDateSpecified
        {
            get
            {
                return this.applicationDateFieldSpecified;
            }
            set
            {
                this.applicationDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Clarification/2.5.0")]
    public partial class originalDetailType : paymentDetailType
    {

        private ulong amountField;

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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(setDetailType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(originalDetailType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Clarification/2.5.0")]
    public partial class paymentDetailType
    {

        private string payeeNameField;

        private string innField;

        private string kppField;

        private string payeeAccountField;

        private string oktmoField;

        private string kbkField;

        private string subsidyField;

        private string purposeField;

        private string descriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string payeeName
        {
            get
            {
                return this.payeeNameField;
            }
            set
            {
                this.payeeNameField = value;
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
        public string payeeAccount
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
        public string subsidy
        {
            get
            {
                return this.subsidyField;
            }
            set
            {
                this.subsidyField = value;
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Clarification/2.5.0")]
    public partial class setDetailType : paymentDetailType
    {

        private ulong amountField;

        private bool amountFieldSpecified;

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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool amountSpecified
        {
            get
            {
                return this.amountFieldSpecified;
            }
            set
            {
                this.amountFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Clarification/2.5.0")]
    public enum ClarificationApplicationTypeAppCode
    {

        /// <remarks/>
        PP,

        /// <remarks/>
        PL,

        /// <remarks/>
        ZR,

        /// <remarks/>
        ZK,

        /// <remarks/>
        ZS,

        /// <remarks/>
        ZN,

        /// <remarks/>
        UF,

        /// <remarks/>
        ZV,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Clarification/2.5.0")]
    public partial class SignsClarificationType
    {

        private string headPostField;

        private string headNameField;

        private string executorPostField;

        private string executorNameField;

        private string executorNumField;

        private System.DateTime signDateField;

        private bool signDateFieldSpecified;

        private string tOFKheadPostField;

        private string tOFKheadNameField;

        private string tOFKexecutorPostField;

        private string tOFKexecutorNameField;

        private string tOFKexecutorNumField;

        private System.DateTime tOFKsignDateField;

        private bool tOFKsignDateFieldSpecified;

        /// <remarks/>
        public string HeadPost
        {
            get
            {
                return this.headPostField;
            }
            set
            {
                this.headPostField = value;
            }
        }

        /// <remarks/>
        public string HeadName
        {
            get
            {
                return this.headNameField;
            }
            set
            {
                this.headNameField = value;
            }
        }

        /// <remarks/>
        public string ExecutorPost
        {
            get
            {
                return this.executorPostField;
            }
            set
            {
                this.executorPostField = value;
            }
        }

        /// <remarks/>
        public string ExecutorName
        {
            get
            {
                return this.executorNameField;
            }
            set
            {
                this.executorNameField = value;
            }
        }

        /// <remarks/>
        public string ExecutorNum
        {
            get
            {
                return this.executorNumField;
            }
            set
            {
                this.executorNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime SignDate
        {
            get
            {
                return this.signDateField;
            }
            set
            {
                this.signDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SignDateSpecified
        {
            get
            {
                return this.signDateFieldSpecified;
            }
            set
            {
                this.signDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string TOFKheadPost
        {
            get
            {
                return this.tOFKheadPostField;
            }
            set
            {
                this.tOFKheadPostField = value;
            }
        }

        /// <remarks/>
        public string TOFKheadName
        {
            get
            {
                return this.tOFKheadNameField;
            }
            set
            {
                this.tOFKheadNameField = value;
            }
        }

        /// <remarks/>
        public string TOFKexecutorPost
        {
            get
            {
                return this.tOFKexecutorPostField;
            }
            set
            {
                this.tOFKexecutorPostField = value;
            }
        }

        /// <remarks/>
        public string TOFKexecutorName
        {
            get
            {
                return this.tOFKexecutorNameField;
            }
            set
            {
                this.tOFKexecutorNameField = value;
            }
        }

        /// <remarks/>
        public string TOFKexecutorNum
        {
            get
            {
                return this.tOFKexecutorNumField;
            }
            set
            {
                this.tOFKexecutorNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime TOFKsignDate
        {
            get
            {
                return this.tOFKsignDateField;
            }
            set
            {
                this.tOFKsignDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TOFKsignDateSpecified
        {
            get
            {
                return this.tOFKsignDateFieldSpecified;
            }
            set
            {
                this.tOFKsignDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Clarification/2.5.0")]
    public enum ClarificationTypeBudgetLevel
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ClarificationType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImportedClarificationType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Clarification/2.5.0")]
    public abstract partial class AbstractClarificationType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    public partial class ImportedIncomeType : IncomeType
    {

        private string originatorIdField;

        private string idField;

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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImportedIncomeType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Income/2.5.0")]
    public partial class IncomeType : PaymentBaseType
    {

        private IncomeTypeIncomeIndex incomeIndexField;

        private string transactionIDField;

        private System.DateTime edDateField;

        private bool edDateFieldSpecified;

        private string incomeIdField;

        private System.DateTime incomeDateField;

        private string edNoField;

        private string edCodeField;

        private System.DateTime chargeOffDateField;

        private bool chargeOffDateFieldSpecified;

        private bool isUncertainField;

        private string paymentIdField;

        private string sourceIDDocField;

        private string sourceIDISField;

        /// <remarks/>
        public IncomeTypeIncomeIndex IncomeIndex
        {
            get
            {
                return this.incomeIndexField;
            }
            set
            {
                this.incomeIndexField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string transactionID
        {
            get
            {
                return this.transactionIDField;
            }
            set
            {
                this.transactionIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime edDate
        {
            get
            {
                return this.edDateField;
            }
            set
            {
                this.edDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool edDateSpecified
        {
            get
            {
                return this.edDateFieldSpecified;
            }
            set
            {
                this.edDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string incomeId
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
        public System.DateTime incomeDate
        {
            get
            {
                return this.incomeDateField;
            }
            set
            {
                this.incomeDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string edNo
        {
            get
            {
                return this.edNoField;
            }
            set
            {
                this.edNoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string edCode
        {
            get
            {
                return this.edCodeField;
            }
            set
            {
                this.edCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime chargeOffDate
        {
            get
            {
                return this.chargeOffDateField;
            }
            set
            {
                this.chargeOffDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool chargeOffDateSpecified
        {
            get
            {
                return this.chargeOffDateFieldSpecified;
            }
            set
            {
                this.chargeOffDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isUncertain
        {
            get
            {
                return this.isUncertainField;
            }
            set
            {
                this.isUncertainField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Income/2.5.0")]
    public partial class IncomeTypeIncomeIndex
    {

        private string kbkField;

        private string oktmoField;

        private string innField;

        private string kppField;

        private string accountNumberField;

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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PaymentType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImportedPaymentType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(IncomeType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImportedIncomeType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Payment/2.5.0")]
    public partial class PaymentBaseType
    {

        private PaymentOrgType paymentOrgField;

        private Payer1 payerField;

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
        public Payer1 Payer
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Payment/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute("Payer", Namespace = "http://roskazna.ru/gisgmp/xsd/Payment/2.5.0", IsNullable = false)]
    public partial class Payer1 : PayerType
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
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImportedPaymentType))]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    public partial class ImportedPaymentType : PaymentType
    {

        private string originatorIdField;

        private string idField;

        private ImportedPaymentTypePaymentMethod paymentMethodField;

        private bool paymentMethodFieldSpecified;

        private string requisiteCheckCodeField;

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
        public ImportedPaymentTypePaymentMethod paymentMethod
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    public enum ImportedPaymentTypePaymentMethod
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    public partial class ImportedRefundType : RefundType
    {

        private string originatorIdField;

        private string idField;

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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImportedRefundType))]
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    public partial class ImportedRenouncementType : RenouncementType
    {

        private string originatorIdField;

        private string idField;

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
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ImportedRenouncementType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Renouncement/2.5.0")]
    public partial class RenouncementType
    {

        private RenouncementTypeApprover approverField;

        private RenouncementTypeExecutor executorField;

        private DeedInfo deedInfoField;

        private string supplierBillIDField;

        private string renouncementIDField;

        private System.DateTime rulingDateField;

        private string rulingNumField;

        private string refusalGroundField;

        private string reasonCodeField;

        /// <remarks/>
        public RenouncementTypeApprover Approver
        {
            get
            {
                return this.approverField;
            }
            set
            {
                this.approverField = value;
            }
        }

        /// <remarks/>
        public RenouncementTypeExecutor Executor
        {
            get
            {
                return this.executorField;
            }
            set
            {
                this.executorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
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
        public string renouncementID
        {
            get
            {
                return this.renouncementIDField;
            }
            set
            {
                this.renouncementIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime rulingDate
        {
            get
            {
                return this.rulingDateField;
            }
            set
            {
                this.rulingDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string rulingNum
        {
            get
            {
                return this.rulingNumField;
            }
            set
            {
                this.rulingNumField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string refusalGround
        {
            get
            {
                return this.refusalGroundField;
            }
            set
            {
                this.refusalGroundField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string reasonCode
        {
            get
            {
                return this.reasonCodeField;
            }
            set
            {
                this.reasonCodeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Renouncement/2.5.0")]
    public partial class RenouncementTypeApprover
    {

        private string positionCodeField;

        private string positionNameField;

        private string personApprovingField;

        private System.DateTime approvalDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string positionCode
        {
            get
            {
                return this.positionCodeField;
            }
            set
            {
                this.positionCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string positionName
        {
            get
            {
                return this.positionNameField;
            }
            set
            {
                this.positionNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string personApproving
        {
            get
            {
                return this.personApprovingField;
            }
            set
            {
                this.personApprovingField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime approvalDate
        {
            get
            {
                return this.approvalDateField;
            }
            set
            {
                this.approvalDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Renouncement/2.5.0")]
    public partial class RenouncementTypeExecutor
    {

        private string vKSPCodeField;

        private string structuralUnitNameField;

        private string structuralUnitAddressField;

        private string structuralLocalityField;

        private string executorFullNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string VKSPCode
        {
            get
            {
                return this.vKSPCodeField;
            }
            set
            {
                this.vKSPCodeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string structuralUnitName
        {
            get
            {
                return this.structuralUnitNameField;
            }
            set
            {
                this.structuralUnitNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string structuralUnitAddress
        {
            get
            {
                return this.structuralUnitAddressField;
            }
            set
            {
                this.structuralUnitAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string structuralLocality
        {
            get
            {
                return this.structuralLocalityField;
            }
            set
            {
                this.structuralLocalityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string executorFullName
        {
            get
            {
                return this.executorFullNameField;
            }
            set
            {
                this.executorFullNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0", IsNullable = false)]
    public partial class PaymentsPackage : PackageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0", IsNullable = false)]
    public partial class RefundsPackage : PackageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0", IsNullable = false)]
    public partial class IncomesPackage : PackageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0", IsNullable = false)]
    public partial class ClarificationsPackage : PackageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0", IsNullable = false)]
    public partial class RenouncementPackage : PackageType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/import-charges/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://roskazna.ru/gisgmp/xsd/services/import-charges/2.5.0", IsNullable = false)]
    public partial class ImportChargesRequest : RequestType
    {

        private ChargesPackage chargesPackageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Package/2.5.0")]
        public ChargesPackage ChargesPackage
        {
            get
            {
                return this.chargesPackageField;
            }
            set
            {
                this.chargesPackageField = value;
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Common/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute("ImportChargesResponse", Namespace = "urn://roskazna.ru/gisgmp/xsd/services/import-charges/2.5.0", IsNullable = false)]
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

}