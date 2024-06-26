﻿

using System.Xml.Serialization;
namespace Bars.GkhGji.Regions.Habarovsk.LicenseUndoRequest
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

        private dataGJILicenseRequestInformationdeclarerul informationdeclarerulField;

        private dataGJILicenseRequestInformationdeclarerfl informationdeclarerflField;

        private dataGJILicenseRequestLicense licenseField;

        private dataGJILicenseRequestFiles filesField;

        private dataGJILicenseRequestAgreement agreementField;

        private dataGJILicenseRequestFeedback feedbackField;

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
        public dataGJILicenseRequestInformationdeclarerul informationdeclarerul
        {
            get
            {
                return this.informationdeclarerulField;
            }
            set
            {
                this.informationdeclarerulField = value;
            }
        }

        /// <remarks/>
        public dataGJILicenseRequestInformationdeclarerfl informationdeclarerfl
        {
            get
            {
                return this.informationdeclarerflField;
            }
            set
            {
                this.informationdeclarerflField = value;
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
        public dataGJILicenseRequestFiles files
        {
            get
            {
                return this.filesField;
            }
            set
            {
                this.filesField = value;
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

        /// <remarks/>
        public dataGJILicenseRequestFeedback feedback
        {
            get
            {
                return this.feedbackField;
            }
            set
            {
                this.feedbackField = value;
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
    public partial class dataGJILicenseRequestInformationdeclarerul
    {

        private string fullnameulField;

        private string shortnameulField;

        private string innulField;

        private string ogrnulField;

        private string emailulField;

        private string phoneulField;

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
        public string shortnameul
        {
            get
            {
                return this.shortnameulField;
            }
            set
            {
                this.shortnameulField = value;
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
        public string phoneul
        {
            get
            {
                return this.phoneulField;
            }
            set
            {
                this.phoneulField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequestInformationdeclarerfl
    {

        private string surnameflField;

        private string nameflField;

        private string midlenameflField;

        private string identityDocumentTypeField;

        private string identityDocumentNameField;

        private string identityDocumentSeriesField;

        private string identityDocumentNumberField;

        private System.DateTime identityDocumentDateField;

        private bool identityDocumentDateFieldSpecified;

        private string identityDocumentIssuerField;

        private string positionField;

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
        public string identityDocumentType
        {
            get
            {
                return this.identityDocumentTypeField;
            }
            set
            {
                this.identityDocumentTypeField = value;
            }
        }

        /// <remarks/>
        public string identityDocumentName
        {
            get
            {
                return this.identityDocumentNameField;
            }
            set
            {
                this.identityDocumentNameField = value;
            }
        }

        /// <remarks/>
        public string identityDocumentSeries
        {
            get
            {
                return this.identityDocumentSeriesField;
            }
            set
            {
                this.identityDocumentSeriesField = value;
            }
        }

        /// <remarks/>
        public string identityDocumentNumber
        {
            get
            {
                return this.identityDocumentNumberField;
            }
            set
            {
                this.identityDocumentNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime identityDocumentDate
        {
            get
            {
                return this.identityDocumentDateField;
            }
            set
            {
                this.identityDocumentDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool identityDocumentDateSpecified
        {
            get
            {
                return this.identityDocumentDateFieldSpecified;
            }
            set
            {
                this.identityDocumentDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public string identityDocumentIssuer
        {
            get
            {
                return this.identityDocumentIssuerField;
            }
            set
            {
                this.identityDocumentIssuerField = value;
            }
        }

        /// <remarks/>
        public string position
        {
            get
            {
                return this.positionField;
            }
            set
            {
                this.positionField = value;
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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequestFiles
    {

        private dataGJILicenseRequestFilesAnotherFiles anotherFilesField;

        /// <remarks/>
        public dataGJILicenseRequestFilesAnotherFiles anotherFiles
        {
            get
            {
                return this.anotherFilesField;
            }
            set
            {
                this.anotherFilesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequestFilesAnotherFiles
    {

        private dataGJILicenseRequestFilesAnotherFilesFile fileField;

        /// <remarks/>
        public dataGJILicenseRequestFilesAnotherFilesFile file
        {
            get
            {
                return this.fileField;
            }
            set
            {
                this.fileField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequestFilesAnotherFilesFile
    {

        private string filenameField;

        private string mediatypeField;

        private string sizeField;

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string filename
        {
            get
            {
                return this.filenameField;
            }
            set
            {
                this.filenameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string mediatype
        {
            get
            {
                return this.mediatypeField;
            }
            set
            {
                this.mediatypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string size
        {
            get
            {
                return this.sizeField;
            }
            set
            {
                this.sizeField = value;
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
        [System.Xml.Serialization.XmlTextAttribute(DataType = "anyURI")]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequestAgreement
    {

        private bool conditions_agreementField;

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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.portalinteraction/1.0.0")]
    public partial class dataGJILicenseRequestFeedback
    {

        private string feedbackTypeField;

        private string feedbackPostAddressField;

        /// <remarks/>
        public string feedbackType
        {
            get
            {
                return this.feedbackTypeField;
            }
            set
            {
                this.feedbackTypeField = value;
            }
        }

        /// <remarks/>
        public string feedbackPostAddress
        {
            get
            {
                return this.feedbackPostAddressField;
            }
            set
            {
                this.feedbackPostAddressField = value;
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
