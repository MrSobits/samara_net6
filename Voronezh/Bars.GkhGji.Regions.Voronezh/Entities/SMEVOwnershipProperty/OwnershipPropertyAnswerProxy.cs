﻿//------------------------------------------------------------------------------
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
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.information-provision/1.0.0")]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "urn://ru.sgio.information-provision/1.0.0", IsNullable = false)]
public partial class Excerpt
{

    private byte numberField;

    private System.DateTime dateField;

    private string issuerField;

    private string signerFullNameField;

    private string signerPositionField;

    private string typeField;

    private byte registerNumberField;

    private System.DateTime registerNumberDateField;

    private ulong initialCostField;

    private ExcerptAddress addressField;

    private ExcerptOwnershipRight ownershipRightField;

    private ExcerptRemovalFromTurnoverDocument removalFromTurnoverDocumentField;

    private ExcerptTurnoverRestrictionDocument turnoverRestrictionDocumentField;

    private ExcerptRightholder rightholderField;

    private ExcerptOtherCost otherCostField;

    private ExcerptLand landField;

    /// <remarks/>
    public byte Number
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
    public System.DateTime Date
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
    public string Issuer
    {
        get
        {
            return this.issuerField;
        }
        set
        {
            this.issuerField = value;
        }
    }

    /// <remarks/>
    public string SignerFullName
    {
        get
        {
            return this.signerFullNameField;
        }
        set
        {
            this.signerFullNameField = value;
        }
    }

    /// <remarks/>
    public string SignerPosition
    {
        get
        {
            return this.signerPositionField;
        }
        set
        {
            this.signerPositionField = value;
        }
    }

    /// <remarks/>
    public string Type
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
    public byte RegisterNumber
    {
        get
        {
            return this.registerNumberField;
        }
        set
        {
            this.registerNumberField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
    public System.DateTime RegisterNumberDate
    {
        get
        {
            return this.registerNumberDateField;
        }
        set
        {
            this.registerNumberDateField = value;
        }
    }

    /// <remarks/>
    public ulong InitialCost
    {
        get
        {
            return this.initialCostField;
        }
        set
        {
            this.initialCostField = value;
        }
    }

    /// <remarks/>
    public ExcerptAddress Address
    {
        get
        {
            return this.addressField;
        }
        set
        {
            this.addressField = value;
        }
    }

    /// <remarks/>
    public ExcerptOwnershipRight OwnershipRight
    {
        get
        {
            return this.ownershipRightField;
        }
        set
        {
            this.ownershipRightField = value;
        }
    }

    /// <remarks/>
    public ExcerptRemovalFromTurnoverDocument RemovalFromTurnoverDocument
    {
        get
        {
            return this.removalFromTurnoverDocumentField;
        }
        set
        {
            this.removalFromTurnoverDocumentField = value;
        }
    }

    /// <remarks/>
    public ExcerptTurnoverRestrictionDocument TurnoverRestrictionDocument
    {
        get
        {
            return this.turnoverRestrictionDocumentField;
        }
        set
        {
            this.turnoverRestrictionDocumentField = value;
        }
    }

    /// <remarks/>
    public ExcerptRightholder Rightholder
    {
        get
        {
            return this.rightholderField;
        }
        set
        {
            this.rightholderField = value;
        }
    }

    /// <remarks/>
    public ExcerptOtherCost OtherCost
    {
        get
        {
            return this.otherCostField;
        }
        set
        {
            this.otherCostField = value;
        }
    }

    /// <remarks/>
    public ExcerptLand Land
    {
        get
        {
            return this.landField;
        }
        set
        {
            this.landField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.information-provision/1.0.0")]
public partial class ExcerptAddress
{

    private string countryField;

    private string regionField;

    private string districtField;

    private string cityField;

    private string localityField;

    private string streetField;

    private uint zipCodeField;

    private ushort houseField;

    private ushort constructField;

    private ushort buildField;

    private ushort flatField;

    /// <remarks/>
    public string Country
    {
        get
        {
            return this.countryField;
        }
        set
        {
            this.countryField = value;
        }
    }

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
    public uint ZipCode
    {
        get
        {
            return this.zipCodeField;
        }
        set
        {
            this.zipCodeField = value;
        }
    }

    /// <remarks/>
    public ushort House
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
    public ushort Construct
    {
        get
        {
            return this.constructField;
        }
        set
        {
            this.constructField = value;
        }
    }

    /// <remarks/>
    public ushort Build
    {
        get
        {
            return this.buildField;
        }
        set
        {
            this.buildField = value;
        }
    }

    /// <remarks/>
    public ushort Flat
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
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.information-provision/1.0.0")]
public partial class ExcerptOwnershipRight
{

    private ulong registrationNumberField;

    private ExcerptOwnershipRightDocument documentField;

    /// <remarks/>
    public ulong RegistrationNumber
    {
        get
        {
            return this.registrationNumberField;
        }
        set
        {
            this.registrationNumberField = value;
        }
    }

    /// <remarks/>
    public ExcerptOwnershipRightDocument Document
    {
        get
        {
            return this.documentField;
        }
        set
        {
            this.documentField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.information-provision/1.0.0")]
public partial class ExcerptOwnershipRightDocument
{

    private string nameField;

    private string numberField;

    private System.DateTime dateField;

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
    public string Number
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
    public System.DateTime Date
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
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.information-provision/1.0.0")]
public partial class ExcerptRemovalFromTurnoverDocument
{

    private string nameField;

    private string numberField;

    private System.DateTime dateField;

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
    public string Number
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
    public System.DateTime Date
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
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.information-provision/1.0.0")]
public partial class ExcerptTurnoverRestrictionDocument
{

    private string nameField;

    private string numberField;

    private System.DateTime dateField;

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
    public string Number
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
    public System.DateTime Date
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
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.information-provision/1.0.0")]
public partial class ExcerptRightholder
{

    private string typeField;

    private string nameField;

    private string cardNumberField;

    private string oGRNField;

    private System.DateTime oGRNDateField;

    private string iNNField;

    private string sNILSField;

    private string lastNameField;

    private string firstNameField;

    private string middleNameField;

    private ExcerptRightholderPassport passportField;

    private ExcerptRightholderAddress addressField;

    /// <remarks/>
    public string Type
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
    public string CardNumber
    {
        get
        {
            return this.cardNumberField;
        }
        set
        {
            this.cardNumberField = value;
        }
    }

    /// <remarks/>
    public string OGRN
    {
        get
        {
            return this.oGRNField;
        }
        set
        {
            this.oGRNField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
    public System.DateTime OGRNDate
    {
        get
        {
            return this.oGRNDateField;
        }
        set
        {
            this.oGRNDateField = value;
        }
    }

    /// <remarks/>
    public string INN
    {
        get
        {
            return this.iNNField;
        }
        set
        {
            this.iNNField = value;
        }
    }

    /// <remarks/>
    public string SNILS
    {
        get
        {
            return this.sNILSField;
        }
        set
        {
            this.sNILSField = value;
        }
    }

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
    public ExcerptRightholderPassport Passport
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
    public ExcerptRightholderAddress Address
    {
        get
        {
            return this.addressField;
        }
        set
        {
            this.addressField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.information-provision/1.0.0")]
public partial class ExcerptRightholderPassport
{

    private uint seriesField;

    private string numberField;

    private string issuedByField;

    private System.DateTime issuedOnField;

    /// <remarks/>
    public uint Series
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
    public string Number
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
    public string IssuedBy
    {
        get
        {
            return this.issuedByField;
        }
        set
        {
            this.issuedByField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
    public System.DateTime IssuedOn
    {
        get
        {
            return this.issuedOnField;
        }
        set
        {
            this.issuedOnField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.information-provision/1.0.0")]
public partial class ExcerptRightholderAddress
{

    private string countryField;

    private string regionField;

    private string districtField;

    private string cityField;

    private string localityField;

    private string streetField;

    private uint zipCodeField;

    private ushort houseField;

    private ushort constructField;

    private ushort buildField;

    private ushort flatField;

    /// <remarks/>
    public string Country
    {
        get
        {
            return this.countryField;
        }
        set
        {
            this.countryField = value;
        }
    }

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
    public uint ZipCode
    {
        get
        {
            return this.zipCodeField;
        }
        set
        {
            this.zipCodeField = value;
        }
    }

    /// <remarks/>
    public ushort House
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
    public ushort Construct
    {
        get
        {
            return this.constructField;
        }
        set
        {
            this.constructField = value;
        }
    }

    /// <remarks/>
    public ushort Build
    {
        get
        {
            return this.buildField;
        }
        set
        {
            this.buildField = value;
        }
    }

    /// <remarks/>
    public ushort Flat
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
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.information-provision/1.0.0")]
public partial class ExcerptOtherCost
{

    private string otherTypeOfCostNameField;

    private ulong costOfOtherTypeOfLandField;

    /// <remarks/>
    public string OtherTypeOfCostName
    {
        get
        {
            return this.otherTypeOfCostNameField;
        }
        set
        {
            this.otherTypeOfCostNameField = value;
        }
    }

    /// <remarks/>
    public ulong CostOfOtherTypeOfLand
    {
        get
        {
            return this.costOfOtherTypeOfLandField;
        }
        set
        {
            this.costOfOtherTypeOfLandField = value;
        }
    }
}

/// <remarks/>
[System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
[System.SerializableAttribute()]
[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.ComponentModel.DesignerCategoryAttribute("code")]
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn://ru.sgio.information-provision/1.0.0")]
public partial class ExcerptLand
{

    private string nameField;

    private string landCategoryField;

    private string allowedTypeOfUseField;

    private ushort areaField;

    private ulong inventoryNumberField;

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
    public string LandCategory
    {
        get
        {
            return this.landCategoryField;
        }
        set
        {
            this.landCategoryField = value;
        }
    }

    /// <remarks/>
    public string AllowedTypeOfUse
    {
        get
        {
            return this.allowedTypeOfUseField;
        }
        set
        {
            this.allowedTypeOfUseField = value;
        }
    }

    /// <remarks/>
    public ushort Area
    {
        get
        {
            return this.areaField;
        }
        set
        {
            this.areaField = value;
        }
    }

    /// <remarks/>
    public ulong InventoryNumber
    {
        get
        {
            return this.inventoryNumberField;
        }
        set
        {
            this.inventoryNumberField = value;
        }
    }
}