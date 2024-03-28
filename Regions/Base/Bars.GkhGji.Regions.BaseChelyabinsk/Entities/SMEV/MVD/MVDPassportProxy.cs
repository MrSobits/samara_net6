﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.MVDPassportProxy
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/basic-types/1.1.0")]
    [System.Xml.Serialization.XmlRootAttribute("russianPassport", Namespace = "urn://mvd/guvm/basic-types/1.1.0", IsNullable = false)]
    public partial class RussianPassport : RussianPassportBase
    {

        private System.DateTime issueDateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime issueDate
        {
            get
            {
                return this.issueDateField;
            }
            set
            {
                this.issueDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PassportDossierPassport))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(RussianPassport))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/basic-types/1.1.0")]
    public partial class RussianPassportBase
    {

        private string seriesField;

        private string numberField;

        /// <remarks/>
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
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/passport-validity-commons/1.1.0")]
    public partial class InvalidDocInfo
    {

        private InvalidityReason invalidityReasonField;

        private System.DateTime invalidityDateField;

        /// <remarks/>
        public InvalidityReason invalidityReason
        {
            get
            {
                return this.invalidityReasonField;
            }
            set
            {
                this.invalidityReasonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime invalidityDate
        {
            get
            {
                return this.invalidityDateField;
            }
            set
            {
                this.invalidityDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/passport-validity-commons/1.1.0")]
    public enum InvalidityReason
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("601")]
        Item601,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("602")]
        Item602,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("603")]
        Item603,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("604")]
        Item604,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("605")]
        Item605,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("606")]
        Item606,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("607")]
        Item607,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("609")]
        Item609,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/passport-validity-commons/1.1.0")]
    public partial class ValidDocInfo
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/passport-dossier/1.1.0")]
    public partial class PassportDossierPassport : RussianPassportBase
    {

        private string issuerCodeField;

        private string issuerNameField;

        private System.DateTime issueDateField;

        private object itemField;

        /// <remarks/>
        public string issuerCode
        {
            get
            {
                return this.issuerCodeField;
            }
            set
            {
                this.issuerCodeField = value;
            }
        }

        /// <remarks/>
        public string issuerName
        {
            get
            {
                return this.issuerNameField;
            }
            set
            {
                this.issuerNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime issueDate
        {
            get
            {
                return this.issueDateField;
            }
            set
            {
                this.issueDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("invalidDoc", typeof(InvalidDocInfo))]
        [System.Xml.Serialization.XmlElementAttribute("validDoc", typeof(ValidDocInfo))]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/basic-types/1.1.0")]
    [System.Xml.Serialization.XmlRootAttribute("foreignPassport", Namespace = "urn://mvd/guvm/basic-types/1.1.0", IsNullable = false)]
    public partial class ForeignPassport
    {

        private string seriesField;

        private string numberField;

        private System.DateTime issueDateField;

        /// <remarks/>
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
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime issueDate
        {
            get
            {
                return this.issueDateField;
            }
            set
            {
                this.issueDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/basic-types/1.1.0")]
    [System.Xml.Serialization.XmlRootAttribute("birthCertificate", Namespace = "urn://mvd/guvm/basic-types/1.1.0", IsNullable = false)]
    public partial class BirthCertificate
    {

        private string seriesField;

        private string numberField;

        private System.DateTime issueDateField;

        /// <remarks/>
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
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime issueDate
        {
            get
            {
                return this.issueDateField;
            }
            set
            {
                this.issueDateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://mvd/guvm/passport-dossier/1.1.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://mvd/guvm/passport-dossier/1.1.0", IsNullable = false)]
    public partial class passportDossierRequest
    {

        private object itemField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("passport", typeof(RussianPassportBase))]
        [System.Xml.Serialization.XmlElementAttribute("person", typeof(PersonInfo))]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/basic-types/1.1.0")]
    public partial class PersonInfo : PersonInfoBase
    {

        private System.DateTime birthDateField;

        private string birthPlaceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime birthDate
        {
            get
            {
                return this.birthDateField;
            }
            set
            {
                this.birthDateField = value;
            }
        }

        /// <remarks/>
        public string birthPlace
        {
            get
            {
                return this.birthPlaceField;
            }
            set
            {
                this.birthPlaceField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PassportDossierPerson))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(ResponsePerson))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(PersonInfo))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/basic-types/1.1.0")]
    public partial class PersonInfoBase
    {

        private string lastNameField;

        private string firstNameField;

        private string middleNameField;

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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/passport-dossier/1.1.0")]
    public partial class PassportDossierPerson : PersonInfoBase
    {

        private Gender genderField;

        private bool genderFieldSpecified;

        /// <remarks/>
        public Gender gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool genderSpecified
        {
            get
            {
                return this.genderFieldSpecified;
            }
            set
            {
                this.genderFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/basic-types/1.1.0")]
    public enum Gender
    {

        /// <remarks/>
        M,

        /// <remarks/>
        F,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/passport-dossier/1.1.0")]
    public partial class ResponsePerson : PersonInfoBase
    {

        private System.DateTime birthDateField;

        private bool birthDateFieldSpecified;

        private Gender genderField;

        private bool genderFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime birthDate
        {
            get
            {
                return this.birthDateField;
            }
            set
            {
                this.birthDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool birthDateSpecified
        {
            get
            {
                return this.birthDateFieldSpecified;
            }
            set
            {
                this.birthDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        public Gender gender
        {
            get
            {
                return this.genderField;
            }
            set
            {
                this.genderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool genderSpecified
        {
            get
            {
                return this.genderFieldSpecified;
            }
            set
            {
                this.genderFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://mvd/guvm/passport-dossier/1.1.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://mvd/guvm/passport-dossier/1.1.0", IsNullable = false)]
    public partial class passportDossierResponse
    {

        private ResponsePerson personField;

        private PassportDossier[] passportDossierField;

        /// <remarks/>
        public ResponsePerson person
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
        [System.Xml.Serialization.XmlElementAttribute("passportDossier")]
        public PassportDossier[] passportDossier
        {
            get
            {
                return this.passportDossierField;
            }
            set
            {
                this.passportDossierField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://mvd/guvm/passport-dossier/1.1.0")]
    public partial class PassportDossier
    {

        private PassportDossierPerson personField;

        private PassportDossierPassport passportField;

        private string commentField;

        /// <remarks/>
        public PassportDossierPerson person
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
        public PassportDossierPassport passport
        {
            get
            {
                return this.passportField;
            }
            set
            {
                this.passportField = value;
            }
        }

        /// <remarks/>
        public string comment
        {
            get
            {
                return this.commentField;
            }
            set
            {
                this.commentField = value;
            }
        }
    }



}