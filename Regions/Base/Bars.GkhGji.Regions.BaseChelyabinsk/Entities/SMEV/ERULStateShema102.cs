﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ErulState102
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://rostelekom.ru/LicenseAdditional/1.0.2")]
    [System.Xml.Serialization.XmlRootAttribute("additionalPermitRequest", Namespace = "urn://rostelekom.ru/LicenseAdditional/1.0.2", IsNullable = false)]
    public partial class additionalPermitRequestType
    {

        private string permitNumberField;

        private dictionaryRecordType permitTypeField;

        private System.DateTime permitDateField;

        private validityInfoType validityField;

        private decisionInfoType decisionField;

        private decisionMakerType decisionMakerField;

        /// <remarks/>
        public string permitNumber
        {
            get
            {
                return this.permitNumberField;
            }
            set
            {
                this.permitNumberField = value;
            }
        }

        /// <remarks/>
        public dictionaryRecordType permitType
        {
            get
            {
                return this.permitTypeField;
            }
            set
            {
                this.permitTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime permitDate
        {
            get
            {
                return this.permitDateField;
            }
            set
            {
                this.permitDateField = value;
            }
        }

        /// <remarks/>
        public validityInfoType validity
        {
            get
            {
                return this.validityField;
            }
            set
            {
                this.validityField = value;
            }
        }

        /// <remarks/>
        public decisionInfoType decision
        {
            get
            {
                return this.decisionField;
            }
            set
            {
                this.decisionField = value;
            }
        }

        /// <remarks/>
        public decisionMakerType decisionMaker
        {
            get
            {
                return this.decisionMakerField;
            }
            set
            {
                this.decisionMakerField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://rostelekom.ru/LicenseAdditional/1.0.2")]
    public partial class dictionaryRecordType
    {

        private string codeField;

        private string titleField;

        /// <remarks/>
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
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://rostelekom.ru/LicenseAdditional/1.0.2")]
    public partial class errorType
    {

        private string errorType1Field;

        private string errorDescriptionField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("errorType")]
        public string errorType1
        {
            get
            {
                return this.errorType1Field;
            }
            set
            {
                this.errorType1Field = value;
            }
        }

        /// <remarks/>
        public string errorDescription
        {
            get
            {
                return this.errorDescriptionField;
            }
            set
            {
                this.errorDescriptionField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://rostelekom.ru/LicenseAdditional/1.0.2")]
    public partial class decisionMakerType
    {

        private string nameField;

        private dictionaryRecordType positionField;

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
        public dictionaryRecordType position
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://rostelekom.ru/LicenseAdditional/1.0.2")]
    public partial class numberInfoType
    {

        private string numberField;

        private System.DateTime dateField;

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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://rostelekom.ru/LicenseAdditional/1.0.2")]
    public partial class decisionInfoType
    {

        private dictionaryRecordType decisionTypeField;

        private numberInfoType decisionNumberInfoField;

        /// <remarks/>
        public dictionaryRecordType decisionType
        {
            get
            {
                return this.decisionTypeField;
            }
            set
            {
                this.decisionTypeField = value;
            }
        }

        /// <remarks/>
        public numberInfoType decisionNumberInfo
        {
            get
            {
                return this.decisionNumberInfoField;
            }
            set
            {
                this.decisionNumberInfoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://rostelekom.ru/LicenseAdditional/1.0.2")]
    public partial class validityInfoType
    {

        private dictionaryRecordType validityTypeField;

        private System.DateTime startDateField;

        private System.DateTime expirationDateField;

        private bool expirationDateFieldSpecified;

        /// <remarks/>
        public dictionaryRecordType validityType
        {
            get
            {
                return this.validityTypeField;
            }
            set
            {
                this.validityTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
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
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime expirationDate
        {
            get
            {
                return this.expirationDateField;
            }
            set
            {
                this.expirationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool expirationDateSpecified
        {
            get
            {
                return this.expirationDateFieldSpecified;
            }
            set
            {
                this.expirationDateFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://rostelekom.ru/LicenseAdditional/1.0.2")]
    [System.Xml.Serialization.XmlRootAttribute("additionalPermitResponse", Namespace = "urn://rostelekom.ru/LicenseAdditional/1.0.2", IsNullable = false)]
    public partial class additionalPermitResponseType
    {

        private bool acceptedField;

        private errorType errorField;

        /// <remarks/>
        public bool accepted
        {
            get
            {
                return this.acceptedField;
            }
            set
            {
                this.acceptedField = value;
            }
        }

        /// <remarks/>
        public errorType error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }
    }


}
