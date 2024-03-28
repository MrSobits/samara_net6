﻿

using System.Xml.Serialization;
namespace Bars.GkhGji.Regions.Habarovsk.SGIO.SMEVSocialHire
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://ru.sgio.hiringcontract/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute("SECRequest", Namespace = "urn://ru.sgio.hiringcontract/1.0.0", IsNullable = false)]
    public partial class SECRequestType
    {

        private string regionCodeField;

        private string districtField;

        private string cityField;

        private string localityField;

        private string streetField;

        private string houseField;

        private string buildingField;

        private string structureField;

        private string flatField;

        private string receiverOKTMOField;

        /// <remarks/>
        public string RegionCode
        {
            get
            {
                return this.regionCodeField;
            }
            set
            {
                this.regionCodeField = value;
            }
        }

        /// <remarks/>
        public string District
        {
            get
            {
                return this.districtField;
            }
            set
            {
                this.districtField = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string Locality
        {
            get
            {
                return this.localityField;
            }
            set
            {
                this.localityField = value;
            }
        }

        /// <remarks/>
        public string Street
        {
            get
            {
                return this.streetField;
            }
            set
            {
                this.streetField = value;
            }
        }

        /// <remarks/>
        public string House
        {
            get
            {
                return this.houseField;
            }
            set
            {
                this.houseField = value;
            }
        }

        /// <remarks/>
        public string Building
        {
            get
            {
                return this.buildingField;
            }
            set
            {
                this.buildingField = value;
            }
        }

        /// <remarks/>
        public string Structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
            }
        }

        /// <remarks/>
        public string Flat
        {
            get
            {
                return this.flatField;
            }
            set
            {
                this.flatField = value;
            }
        }

        /// <remarks/>
        public string ReceiverOKTMO
        {
            get
            {
                return this.receiverOKTMOField;
            }
            set
            {
                this.receiverOKTMOField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://ru.sgio.hiringcontract/1.0.0")]
    public partial class SECResponseEmployersEmployerType
    {

        private string lastNameField;

        private string firstNameField;

        private string middleNameField;

        private string birthdayField;

        private string birthplaceField;

        private string citizenshipField;

        private SECResponseEmployersEmployerTypeDocumentType documentTypeField;

        private string documentNumberField;

        private string documentSeriesField;

        private string documentDateField;

        private bool is_contract_ownerField;

        private bool is_contract_ownerFieldSpecified;

        /// <remarks/>
        public string LastName
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
        public string FirstName
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
        public string MiddleName
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
        public string Birthday
        {
            get
            {
                return this.birthdayField;
            }
            set
            {
                this.birthdayField = value;
            }
        }

        /// <remarks/>
        public string Birthplace
        {
            get
            {
                return this.birthplaceField;
            }
            set
            {
                this.birthplaceField = value;
            }
        }

        /// <remarks/>
        public string Citizenship
        {
            get
            {
                return this.citizenshipField;
            }
            set
            {
                this.citizenshipField = value;
            }
        }

        /// <remarks/>
        public SECResponseEmployersEmployerTypeDocumentType DocumentType
        {
            get
            {
                return this.documentTypeField;
            }
            set
            {
                this.documentTypeField = value;
            }
        }

        /// <remarks/>
        public string DocumentNumber
        {
            get
            {
                return this.documentNumberField;
            }
            set
            {
                this.documentNumberField = value;
            }
        }

        /// <remarks/>
        public string DocumentSeries
        {
            get
            {
                return this.documentSeriesField;
            }
            set
            {
                this.documentSeriesField = value;
            }
        }

        /// <remarks/>
        public string DocumentDate
        {
            get
            {
                return this.documentDateField;
            }
            set
            {
                this.documentDateField = value;
            }
        }

        /// <remarks/>
        public bool Is_contract_owner
        {
            get
            {
                return this.is_contract_ownerField;
            }
            set
            {
                this.is_contract_ownerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool Is_contract_ownerSpecified
        {
            get
            {
                return this.is_contract_ownerFieldSpecified;
            }
            set
            {
                this.is_contract_ownerFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.hiringcontract/1.0.0")]
    public enum SECResponseEmployersEmployerTypeDocumentType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("11")]
        Item11,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("21")]
        Item21,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("6")]
        Item6,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("13")]
        Item13,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("7")]
        Item7,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("8")]
        Item8,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("17")]
        Item17,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("22")]
        Item22,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("16")]
        Item16,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("9")]
        Item9,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("5")]
        Item5,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("20")]
        Item20,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("14")]
        Item14,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("15")]
        Item15,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("10")]
        Item10,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("2")]
        Item2,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("18")]
        Item18,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("4")]
        Item4,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("12")]
        Item12,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("19")]
        Item19,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("3")]
        Item3,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://ru.sgio.hiringcontract/1.0.0")]
    public partial class SECResponseObjectAddressType
    {

        private string regionField;

        private string districtField;

        private string cityField;

        private string localityField;

        private string streetField;

        private string houseField;

        private string buildingField;

        private string structureField;

        private string flatField;

        /// <remarks/>
        public string Region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        public string District
        {
            get
            {
                return this.districtField;
            }
            set
            {
                this.districtField = value;
            }
        }

        /// <remarks/>
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string Locality
        {
            get
            {
                return this.localityField;
            }
            set
            {
                this.localityField = value;
            }
        }

        /// <remarks/>
        public string Street
        {
            get
            {
                return this.streetField;
            }
            set
            {
                this.streetField = value;
            }
        }

        /// <remarks/>
        public string House
        {
            get
            {
                return this.houseField;
            }
            set
            {
                this.houseField = value;
            }
        }

        /// <remarks/>
        public string Building
        {
            get
            {
                return this.buildingField;
            }
            set
            {
                this.buildingField = value;
            }
        }

        /// <remarks/>
        public string Structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
            }
        }

        /// <remarks/>
        public string Flat
        {
            get
            {
                return this.flatField;
            }
            set
            {
                this.flatField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://ru.sgio.hiringcontract/1.0.0")]
    public partial class SECResponseObjectDescriptionType
    {

        private string contractSeriesField;

        private string contractNumberField;

        private SECResponseObjectDescriptionTypeContractType contractTypeField;

        private string nameField;

        private string purposeField;

        private string totalAreaField;

        private string liveAreaField;

        /// <remarks/>
        public string ContractSeries
        {
            get
            {
                return this.contractSeriesField;
            }
            set
            {
                this.contractSeriesField = value;
            }
        }

        /// <remarks/>
        public string ContractNumber
        {
            get
            {
                return this.contractNumberField;
            }
            set
            {
                this.contractNumberField = value;
            }
        }

        /// <remarks/>
        public SECResponseObjectDescriptionTypeContractType ContractType
        {
            get
            {
                return this.contractTypeField;
            }
            set
            {
                this.contractTypeField = value;
            }
        }

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
        public string Purpose
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
        public string TotalArea
        {
            get
            {
                return this.totalAreaField;
            }
            set
            {
                this.totalAreaField = value;
            }
        }

        /// <remarks/>
        public string LiveArea
        {
            get
            {
                return this.liveAreaField;
            }
            set
            {
                this.liveAreaField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.hiringcontract/1.0.0")]
    public enum SECResponseObjectDescriptionTypeContractType
    {

        /// <remarks/>
        Commercial,

        /// <remarks/>
        Social,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn://ru.sgio.hiringcontract/1.0.0")]
    [System.Xml.Serialization.XmlRootAttribute("SECResponse", Namespace = "urn://ru.sgio.hiringcontract/1.0.0", IsNullable = false)]
    public partial class SECResponseType
    {

        private SECResponseObjectDescriptionType objectDescriptionField;

        private SECResponseObjectAddressType objectAddressField;

        private SECResponseEmployersEmployerType[] employersField;

        private string fileDocField;

        /// <remarks/>
        public SECResponseObjectDescriptionType ObjectDescription
        {
            get
            {
                return this.objectDescriptionField;
            }
            set
            {
                this.objectDescriptionField = value;
            }
        }

        /// <remarks/>
        public SECResponseObjectAddressType ObjectAddress
        {
            get
            {
                return this.objectAddressField;
            }
            set
            {
                this.objectAddressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Employer", IsNullable = false)]
        public SECResponseEmployersEmployerType[] Employers
        {
            get
            {
                return this.employersField;
            }
            set
            {
                this.employersField = value;
            }
        }

        /// <remarks/>
        public string fileDoc
        {
            get
            {
                return this.fileDocField;
            }
            set
            {
                this.fileDocField = value;
            }
        }
    }


}