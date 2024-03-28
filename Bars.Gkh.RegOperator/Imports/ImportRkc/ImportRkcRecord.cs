namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [Serializable]
    [XmlRoot(ElementName = "FILE")]
    public class ImportRkcRecord
    {
        [DataMember]
        [XmlArray("PERSONAL-ACCOUNTS")]
        [XmlArrayItem("PERSONAL-ACCOUNT", typeof(ImportRkcPersonalAccount))]
        public ImportRkcPersonalAccount[] PersonalAccounts { get; set; }

        [DataMember]
        [XmlAttribute("FILE-DATE")]
        public string FileDate { get; set; }

        [DataMember]
        [XmlAttribute("FILE-NUM")]
        public string FileNumber { get; set; }

        [DataMember]
        [XmlAttribute("FORMAT_VERSION")]
        public string FormatVersion { get; set; }

        [DataMember]
        [XmlAttribute("RKC_INN")]
        public string RkcInn { get; set; }
    }

    [DataContract]
    [Serializable]
    [XmlType(TypeName = "PERSONAL-ACCOUNT")]
    public class ImportRkcPersonalAccount
    {
        [DataMember]
        [XmlAttribute("ACC_NUMBER")]
        public string AccNumber { get; set; }

        [DataMember]
        [XmlAttribute("OLD_ACC_NUMBER")]
        public string OldAccNumber { get; set; }

        [DataMember]
        [XmlAttribute("ACC_NUMBER_DATE")]
        public string AccNumberDate { get; set; }

        [DataMember]
        [XmlAttribute("CHANGE_DATE_ACC_NUM_DATE")]
        public string ChangeDateAccNumberDate { get; set; }

        [DataMember]
        [XmlAttribute("MU")]
        public string Mu { get; set; }

        [DataMember]
        [XmlAttribute("TYPE_CITY")]
        public string TypeCity { get; set; }

        [DataMember]
        [XmlAttribute("CITY")]
        public string City { get; set; }

        [DataMember]
        [XmlAttribute("TYPE_STREET")]
        public string TypeStreet { get; set; }

        [DataMember]
        [XmlAttribute("STREET")]
        public string Street { get; set; }

        [DataMember]
        [XmlAttribute("HOUSE_NUM")]
        public string HouseNum { get; set; }

        [DataMember]
        [XmlAttribute("LITER")]
        public string Liter { get; set; }

        [DataMember]
        [XmlAttribute("KORPUS")]
        public string Korpus { get; set; }

        [DataMember]
        [XmlAttribute("BUILDING")]
        public string Building { get; set; }

        [DataMember]
        [XmlAttribute("FLAT_PLACE_NUM")]
        public string FlatPlaceNum { get; set; }

        [DataMember]
        [XmlAttribute("TOTAL_AREA")]
        public string TotalArea { get; set; }

        [DataMember]
        [XmlAttribute("CHANGE_DATE_TA")]
        public string ChangeDateTotalArea { get; set; }

        [DataMember]
        [XmlAttribute("LIVE_AREA")]
        public string LiveArea { get; set; }

        [DataMember]
        [XmlAttribute("CHANGE_DATE_LA")]
        public string ChangeDateLiveArea { get; set; }

        [DataMember]
        [XmlAttribute("FLAT_PLACE_TYPE")]
        public string FlatPlaceType { get; set; }

        [DataMember]
        [XmlAttribute("PROPERTY_TYPE")]
        public string PropertyType { get; set; }

        [DataMember]
        [XmlAttribute("ACC_TYPE")]
        public string AccType { get; set; }

        [DataMember]
        [XmlAttribute("SURNAME")]
        public string Surname { get; set; }

        [DataMember]
        [XmlAttribute("NAME")]
        public string Name { get; set; }

        [DataMember]
        [XmlAttribute("LASTNAME")]
        public string LastName { get; set; }

        [DataMember]
        [XmlAttribute("INN")]
        public string Inn { get; set; }

        [DataMember]
        [XmlAttribute("KPP")]
        public string Kpp { get; set; }

        [DataMember]
        [XmlAttribute("RENTER_NAME")]
        public string RenterName { get; set; }

        [DataMember]
        [XmlAttribute("SHARE")]
        public string Share { get; set; }

        [DataMember]
        [XmlAttribute("CHANGE_DATE_SHARE")]
        public string ChangeDateShare { get; set; }
    }
}