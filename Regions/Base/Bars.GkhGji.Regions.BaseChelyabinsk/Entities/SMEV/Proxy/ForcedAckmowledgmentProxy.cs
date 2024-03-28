namespace ForcedAckmowledgmentProxy
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0", IsNullable = false)]
    public partial class ForcedAcknowledgementRequest : RequestType
    {

        private object itemField;

        private string originatorIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AnnulmentExcludeQuittance", typeof(ForcedAcknowledgementRequestAnnulmentExcludeQuittance))]
        [System.Xml.Serialization.XmlElementAttribute("AnnulmentReconcile", typeof(ForcedAcknowledgementRequestAnnulmentReconcile))]
        [System.Xml.Serialization.XmlElementAttribute("AnnulmentServiceProvided", typeof(ForcedAcknowledgementRequestAnnulmentServiceProvided))]
        [System.Xml.Serialization.XmlElementAttribute("ExcludeQuittance", typeof(ForcedAcknowledgementRequestExcludeQuittance))]
        [System.Xml.Serialization.XmlElementAttribute("Reconcile", typeof(ForcedAcknowledgementRequestReconcile))]
        [System.Xml.Serialization.XmlElementAttribute("ServiceProvided", typeof(ForcedAcknowledgementRequestServiceProvided))]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    public partial class ForcedAcknowledgementRequestAnnulmentExcludeQuittance
    {

        private string[] paymentIdField;

        private string supplierBillIdField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string supplierBillId
        {
            get
            {
                return this.supplierBillIdField;
            }
            set
            {
                this.supplierBillIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    public partial class ForcedAcknowledgementRequestAnnulmentReconcile
    {

        private object[] itemsField;

        private string supplierBillIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PaymentId", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("PaymentNotLoaded", typeof(ForcedAcknowledgementRequestAnnulmentReconcilePaymentNotLoaded))]
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
        public string supplierBillId
        {
            get
            {
                return this.supplierBillIdField;
            }
            set
            {
                this.supplierBillIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    public partial class ForcedAcknowledgementRequestAnnulmentReconcilePaymentNotLoaded
    {

        private string reconcileIDField;

        private bool valueField;

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
        [System.Xml.Serialization.XmlTextAttribute()]
        public bool Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    public partial class ForcedAcknowledgementRequestAnnulmentServiceProvided
    {

        private ForcedAcknowledgementRequestAnnulmentServiceProvidedPaymentDataID[] paymentDataIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PaymentDataID")]
        public ForcedAcknowledgementRequestAnnulmentServiceProvidedPaymentDataID[] PaymentDataID
        {
            get
            {
                return this.paymentDataIDField;
            }
            set
            {
                this.paymentDataIDField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    public partial class ForcedAcknowledgementRequestAnnulmentServiceProvidedPaymentDataID
    {

        private string paymentIdField;

        private string serviceDataIDField;

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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    public partial class ForcedAcknowledgementRequestExcludeQuittance
    {

        private string[] paymentIdField;

        private string supplierBillIdField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string supplierBillId
        {
            get
            {
                return this.supplierBillIdField;
            }
            set
            {
                this.supplierBillIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    public partial class ForcedAcknowledgementRequestReconcile
    {

        private object[] itemsField;

        private string supplierBillIdField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PaymentId", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("PaymentNotLoaded", typeof(ForcedAcknowledgementRequestReconcilePaymentNotLoaded))]
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
        public string supplierBillId
        {
            get
            {
                return this.supplierBillIdField;
            }
            set
            {
                this.supplierBillIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    public partial class ForcedAcknowledgementRequestReconcilePaymentNotLoaded
    {

        private ulong amountReconcileField;

        private bool amountReconcileFieldSpecified;

        private bool valueField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool amountReconcileSpecified
        {
            get
            {
                return this.amountReconcileFieldSpecified;
            }
            set
            {
                this.amountReconcileFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public bool Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    public partial class ForcedAcknowledgementRequestServiceProvided
    {

        private ForcedAcknowledgementRequestServiceProvidedPaymentDataInfo[] paymentDataInfoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("PaymentDataInfo")]
        public ForcedAcknowledgementRequestServiceProvidedPaymentDataInfo[] PaymentDataInfo
        {
            get
            {
                return this.paymentDataInfoField;
            }
            set
            {
                this.paymentDataInfoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    public partial class ForcedAcknowledgementRequestServiceProvidedPaymentDataInfo
    {

        private ServiceDataType serviceDataField;

        private string paymentIdField;

        /// <remarks/>
        public ServiceDataType ServiceData
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0", IsNullable = false)]
    public partial class ForcedAcknowledgementResponse : ResponseType
    {

        private object[] itemsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Done", typeof(ForcedAcknowledgementResponseDone))]
        [System.Xml.Serialization.XmlElementAttribute("Quittance", typeof(ForcedAcknowledgementResponseQuittance))]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    public partial class ForcedAcknowledgementResponseDone
    {

        private string serviceDataIDField;

        private string paymentIdField;

        private string codeField;

        private bool valueField;

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
        [System.Xml.Serialization.XmlTextAttribute()]
        public bool Value
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://roskazna.ru/gisgmp/xsd/services/forced-ackmowledgement/2.5.0")]
    public partial class ForcedAcknowledgementResponseQuittance : QuittanceType
    {

        private string reconcileIDField;

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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://roskazna.ru/gisgmp/xsd/Quittance/2.5.0")]
    public partial class QuittanceType
    {

        private DiscountType itemField;

        private QuittanceTypeRefund[] refundField;

        private QuittanceTypeIncome[] incomeField;

        private QuittanceTypeClarification[] clarificationField;

        private string supplierBillIDField;

        private ulong totalAmountField;

        private bool totalAmountFieldSpecified;

        private System.DateTime creationDateField;

        private string billStatusField;

        private long balanceField;

        private bool balanceFieldSpecified;

        private string paymentIdField;

        private ulong amountPaymentField;

        private bool amountPaymentFieldSpecified;

        private string payeeINNField;

        private string payeeKPPField;

        private string kbkField;

        private string oktmoField;

        private string payerIdentifierField;

        private string accountNumberField;

        private string bikField;

        private bool isRevokedField;

        private bool isRevokedFieldSpecified;

        private bool paymentPortalField;

        private bool paymentPortalFieldSpecified;

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
        [System.Xml.Serialization.XmlElementAttribute("Refund")]
        public QuittanceTypeRefund[] Refund
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
        [System.Xml.Serialization.XmlElementAttribute("Income")]
        public QuittanceTypeIncome[] Income
        {
            get
            {
                return this.incomeField;
            }
            set
            {
                this.incomeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Clarification")]
        public QuittanceTypeClarification[] Clarification
        {
            get
            {
                return this.clarificationField;
            }
            set
            {
                this.clarificationField = value;
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool totalAmountSpecified
        {
            get
            {
                return this.totalAmountFieldSpecified;
            }
            set
            {
                this.totalAmountFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime creationDate
        {
            get
            {
                return this.creationDateField;
            }
            set
            {
                this.creationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string billStatus
        {
            get
            {
                return this.billStatusField;
            }
            set
            {
                this.billStatusField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long balance
        {
            get
            {
                return this.balanceField;
            }
            set
            {
                this.balanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool balanceSpecified
        {
            get
            {
                return this.balanceFieldSpecified;
            }
            set
            {
                this.balanceFieldSpecified = value;
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
        public ulong amountPayment
        {
            get
            {
                return this.amountPaymentField;
            }
            set
            {
                this.amountPaymentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool amountPaymentSpecified
        {
            get
            {
                return this.amountPaymentFieldSpecified;
            }
            set
            {
                this.amountPaymentFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string payeeINN
        {
            get
            {
                return this.payeeINNField;
            }
            set
            {
                this.payeeINNField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string payeeKPP
        {
            get
            {
                return this.payeeKPPField;
            }
            set
            {
                this.payeeKPPField = value;
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
        public bool isRevoked
        {
            get
            {
                return this.isRevokedField;
            }
            set
            {
                this.isRevokedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool isRevokedSpecified
        {
            get
            {
                return this.isRevokedFieldSpecified;
            }
            set
            {
                this.isRevokedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool paymentPortal
        {
            get
            {
                return this.paymentPortalField;
            }
            set
            {
                this.paymentPortalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool paymentPortalSpecified
        {
            get
            {
                return this.paymentPortalFieldSpecified;
            }
            set
            {
                this.paymentPortalFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Quittance/2.5.0")]
    public partial class QuittanceTypeRefund
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Quittance/2.5.0")]
    public partial class QuittanceTypeIncome
    {

        private string incomeIdField;

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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://roskazna.ru/gisgmp/xsd/Quittance/2.5.0")]
    public partial class QuittanceTypeClarification
    {

        private string clarificationIdField;

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