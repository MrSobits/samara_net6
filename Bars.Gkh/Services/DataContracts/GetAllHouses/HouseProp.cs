namespace Bars.Gkh.Services.DataContracts.GetAllHouses
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "HouseProp")]
    public class HouseProp
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Id")]
        public long Id { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Address")]
        public string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeHouse")]
        public TypeHouse TypeHouse { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "HouseNumber")]
        public string HouseNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ConditionHouse")]
        public ConditionHouse ConditionHouse { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "BuildYear")]
        public int BuildYear { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DateCommissioning")]
        public DateTime DateCommissioning { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "HasPrivatizedFlats")]
        public bool HasPrivatizedFlats { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PrivatizationDateFirstApartment")]
        public DateTime PrivatizationDateFirstApartment { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NecessaryConductCr")]
        public YesNoNotSet NecessaryConductCr { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DateLastOverhaul")]
        public DateTime DateLastOverhaul { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeProject")]
        public string TypeProject { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "PhysicalWear")]
        public decimal PhysicalWear { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "CadastreNumber")]
        public string CadastreNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaMkd")]
        public decimal AreaMkd { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaOwned")]
        public decimal AreaOwned { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaMunicipalOwned")]
        public decimal AreaMunicipalOwned { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaGovernmentOwned")]
        public decimal AreaGovernmentOwned { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaLivingNotLivingMkd")]
        public decimal AreaLivingNotLivingMkd { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaLiving")]
        public decimal AreaLiving { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaLivingOwned")]
        public decimal AreaLivingOwned { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaNotLivingFunctional")]
        public decimal AreaNotLivingFunctional { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaCommonUsage")]
        public decimal AreaCommonUsage { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "AreaCleaning")]
        public decimal AreaCleaning { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Floors")]
        public int Floors { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "MaximumFloors")]
        public int MaximumFloors { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "FloorHeight")]
        public decimal FloorHeight { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NumberEntrances")]
        public int NumberEntrances { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NumberApartments")]
        public int NumberApartments { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NumberLiving")]
        public int NumberLiving { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "NumberLifts")]
        public int NumberLifts { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "RoofingMaterial")]
        public string RoofingMaterial { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "WallMaterial")]
        public string WallMaterial { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TypeRoof")]
        public TypeRoof TypeRoof { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "HeatingSystem")]
        public HeatingSystem HeatingSystem { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ManOrgs")]
        public string ManOrgs { get; set; }
    }
}