﻿

using System.Xml.Serialization;
namespace Bars.GkhGji.Regions.Habarovsk.LicenseInfoRequest
{
    //------------------------------------------------------------------------------
    // <auto-generated>
    //     Этот код создан программой.
    //     Исполняемая версия:4.0.30319.42000
    //
    //     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
    //     повторной генерации кода.
    // </auto-generated>
    //------------------------------------------------------------------------------

    using System.Xml.Serialization;

    // 
    // Этот исходный код был создан с помощью xsd, версия=4.8.3928.0.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://ru.sgio.portalinteraction/1.0.0", IsNullable = false)]
    public partial class orderRequest
    {

        private string receiverField;

        private object itemField;

        private string idField;

        /// <remarks/>
        public string receiver
        {
            get
            {
                return this.receiverField;
            }
            set
            {
                this.receiverField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("cancellation", typeof(cancellation))]
        [System.Xml.Serialization.XmlElementAttribute("order", typeof(order))]
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
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string id
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
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://ru.sgio.portalinteraction/1.0.0", IsNullable = false)]
    public partial class cancellation
    {

        private string numberField;

        private object messageField;

        /// <remarks/>
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

        /// <remarks/>
        public object message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://ru.sgio.portalinteraction/1.0.0", IsNullable = false)]
    public partial class order
    {

        private string numberField;

        private System.DateTime dateField;

        private applicant applicantField;

        private organization organizationField;

        private service serviceField;

        private data dataField;

        /// <remarks/>
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

        /// <remarks/>
        public System.DateTime date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        public applicant applicant
        {
            get
            {
                return this.applicantField;
            }
            set
            {
                this.applicantField = value;
            }
        }

        /// <remarks/>
        public organization organization
        {
            get
            {
                return this.organizationField;
            }
            set
            {
                this.organizationField = value;
            }
        }

        /// <remarks/>
        public service service
        {
            get
            {
                return this.serviceField;
            }
            set
            {
                this.serviceField = value;
            }
        }

        /// <remarks/>
        public data data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://ru.sgio.portalinteraction/1.0.0", IsNullable = false)]
    public partial class applicant
    {

        private type typeField;

        private string esiaIdField;

        private string snilsField;

        private bool agreementField;

        private string lastNameField;

        private string firstNameField;

        private string middleNameField;

        private string phoneField;

        private string emailField;

        private string innField;

        private string okvedField;

        /// <remarks/>
        public type type
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
        public string esiaId
        {
            get
            {
                return this.esiaIdField;
            }
            set
            {
                this.esiaIdField = value;
            }
        }

        /// <remarks/>
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
        public bool agreement
        {
            get
            {
                return this.agreementField;
            }
            set
            {
                this.agreementField = value;
            }
        }

        /// <remarks/>
        public string lastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }

        /// <remarks/>
        public string firstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        public string middleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }

        /// <remarks/>
        public string phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        /// <remarks/>
        public string email
        {
            get
            {
                return this.emailField;
            }
            set
            {
                this.emailField = value;
            }
        }

        /// <remarks/>
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
        public string okved
        {
            get
            {
                return this.okvedField;
            }
            set
            {
                this.okvedField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://ru.sgio.portalinteraction/1.0.0", IsNullable = false)]
    public enum type
    {

        /// <remarks/>
        UL,

        /// <remarks/>
        FL,

        /// <remarks/>
        IP,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://ru.sgio.portalinteraction/1.0.0", IsNullable = false)]
    public partial class organization
    {

        private string idField;

        private string nameField;

        /// <remarks/>
        public string id
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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://ru.sgio.portalinteraction/1.0.0", IsNullable = false)]
    public partial class service
    {

        private long idField;

        private long procedureField;

        private bool procedureFieldSpecified;

        private long targetField;

        private bool targetFieldSpecified;

        private string okatoField;

        /// <remarks/>
        public long id
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
        public long procedure
        {
            get
            {
                return this.procedureField;
            }
            set
            {
                this.procedureField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool procedureSpecified
        {
            get
            {
                return this.procedureFieldSpecified;
            }
            set
            {
                this.procedureFieldSpecified = value;
            }
        }

        /// <remarks/>
        public long target
        {
            get
            {
                return this.targetField;
            }
            set
            {
                this.targetField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool targetSpecified
        {
            get
            {
                return this.targetFieldSpecified;
            }
            set
            {
                this.targetFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string okato
        {
            get
            {
                return this.okatoField;
            }
            set
            {
                this.okatoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://ru.sgio.portalinteraction/1.0.0", IsNullable = false)]
    public partial class data
    {

        private dataGJILicenseRequest gJILicenseRequestField;

        /// <remarks/>
        public dataGJILicenseRequest GJILicenseRequest
        {
            get
            {
                return this.gJILicenseRequestField;
            }
            set
            {
                this.gJILicenseRequestField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequest
    {

        private dataGJILicenseRequestInfo infoField;

        private dataGJILicenseRequestInformation informationField;

        private dataGJILicenseRequestInformationdeclarer informationdeclarerField;

        private dataGJILicenseRequestDutypay dutypayField;

        private dataGJILicenseRequestLicense licenseField;

        private dataGJILicenseRequestAgreement agreementField;

        /// <remarks/>
        public dataGJILicenseRequestInfo info
        {
            get
            {
                return this.infoField;
            }
            set
            {
                this.infoField = value;
            }
        }

        /// <remarks/>
        public dataGJILicenseRequestInformation information
        {
            get
            {
                return this.informationField;
            }
            set
            {
                this.informationField = value;
            }
        }

        /// <remarks/>
        public dataGJILicenseRequestInformationdeclarer informationdeclarer
        {
            get
            {
                return this.informationdeclarerField;
            }
            set
            {
                this.informationdeclarerField = value;
            }
        }

        /// <remarks/>
        public dataGJILicenseRequestDutypay dutypay
        {
            get
            {
                return this.dutypayField;
            }
            set
            {
                this.dutypayField = value;
            }
        }

        /// <remarks/>
        public dataGJILicenseRequestLicense license
        {
            get
            {
                return this.licenseField;
            }
            set
            {
                this.licenseField = value;
            }
        }

        /// <remarks/>
        public dataGJILicenseRequestAgreement agreement
        {
            get
            {
                return this.agreementField;
            }
            set
            {
                this.agreementField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequestInfo
    {

        private string nameField;

        private long serviceIdField;

        private long procedureIdField;

        private long orgIdField;

        private decimal versionField;

        private string descriptionField;

        /// <remarks/>
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
        public long serviceId
        {
            get
            {
                return this.serviceIdField;
            }
            set
            {
                this.serviceIdField = value;
            }
        }

        /// <remarks/>
        public long procedureId
        {
            get
            {
                return this.procedureIdField;
            }
            set
            {
                this.procedureIdField = value;
            }
        }

        /// <remarks/>
        public long orgId
        {
            get
            {
                return this.orgIdField;
            }
            set
            {
                this.orgIdField = value;
            }
        }

        /// <remarks/>
        public decimal version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequestInformation
    {

        private string sourceField;

        private string declarerField;

        /// <remarks/>
        public string source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        /// <remarks/>
        public string declarer
        {
            get
            {
                return this.declarerField;
            }
            set
            {
                this.declarerField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequestInformationdeclarer
    {

        private string fullnameulField;

        private string innulField;

        private string ogrnulField;

        private string placecoordinatField;

        private string emailulField;

        private string phoneField;

        private string surnameflField;

        private string nameflField;

        private string midlenameflField;

        private string registrationAddressField;

        /// <remarks/>
        public string fullnameul
        {
            get
            {
                return this.fullnameulField;
            }
            set
            {
                this.fullnameulField = value;
            }
        }

        /// <remarks/>
        public string innul
        {
            get
            {
                return this.innulField;
            }
            set
            {
                this.innulField = value;
            }
        }

        /// <remarks/>
        public string ogrnul
        {
            get
            {
                return this.ogrnulField;
            }
            set
            {
                this.ogrnulField = value;
            }
        }

        /// <remarks/>
        public string placecoordinat
        {
            get
            {
                return this.placecoordinatField;
            }
            set
            {
                this.placecoordinatField = value;
            }
        }

        /// <remarks/>
        public string emailul
        {
            get
            {
                return this.emailulField;
            }
            set
            {
                this.emailulField = value;
            }
        }

        /// <remarks/>
        public string phone
        {
            get
            {
                return this.phoneField;
            }
            set
            {
                this.phoneField = value;
            }
        }

        /// <remarks/>
        public string surnamefl
        {
            get
            {
                return this.surnameflField;
            }
            set
            {
                this.surnameflField = value;
            }
        }

        /// <remarks/>
        public string namefl
        {
            get
            {
                return this.nameflField;
            }
            set
            {
                this.nameflField = value;
            }
        }

        /// <remarks/>
        public string midlenamefl
        {
            get
            {
                return this.midlenameflField;
            }
            set
            {
                this.midlenameflField = value;
            }
        }

        /// <remarks/>
        public string registrationAddress
        {
            get
            {
                return this.registrationAddressField;
            }
            set
            {
                this.registrationAddressField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequestDutypay
    {

        private string dutypayNumberField;

        /// <remarks/>
        public string dutypayNumber
        {
            get
            {
                return this.dutypayNumberField;
            }
            set
            {
                this.dutypayNumberField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequestLicense
    {

        private System.DateTime licenseDateField;

        private string licenseNumberField;

        private string formatField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime licenseDate
        {
            get
            {
                return this.licenseDateField;
            }
            set
            {
                this.licenseDateField = value;
            }
        }

        /// <remarks/>
        public string licenseNumber
        {
            get
            {
                return this.licenseNumberField;
            }
            set
            {
                this.licenseNumberField = value;
            }
        }

        /// <remarks/>
        public string format
        {
            get
            {
                return this.formatField;
            }
            set
            {
                this.formatField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequestAgreement
    {

        private bool conditions_agreementField;

        private bool sendEmailField;

        private bool sendPostField;

        private string postAddressField;

        private string contactPhoneField;

        /// <remarks/>
        public bool conditions_agreement
        {
            get
            {
                return this.conditions_agreementField;
            }
            set
            {
                this.conditions_agreementField = value;
            }
        }

        /// <remarks/>
        public bool sendEmail
        {
            get
            {
                return this.sendEmailField;
            }
            set
            {
                this.sendEmailField = value;
            }
        }

        /// <remarks/>
        public bool sendPost
        {
            get
            {
                return this.sendPostField;
            }
            set
            {
                this.sendPostField = value;
            }
        }

        /// <remarks/>
        public string postAddress
        {
            get
            {
                return this.postAddressField;
            }
            set
            {
                this.postAddressField = value;
            }
        }

        /// <remarks/>
        public string contactPhone
        {
            get
            {
                return this.contactPhoneField;
            }
            set
            {
                this.contactPhoneField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://ru.sgio.portalinteraction/1.0.0", IsNullable = false)]
    public partial class orderResponse
    {

        private result resultField;

        private string idField;

        /// <remarks/>
        public result result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
        public string id
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
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://ru.sgio.portalinteraction/1.0.0", IsNullable = false)]
    public partial class result
    {

        private sbyte statusField;

        private object messageField;

        private string numberField;

        /// <remarks/>
        public sbyte status
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
        public object message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }

        /// <remarks/>
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



}