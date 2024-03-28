namespace Bars.Gkh.Overhaul.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "GetHouseInfoDpkrResponse")]
    public class GetHouseInfoDpkrResponse
    {
        [DataMember]
        [XmlElement(ElementName = "House")]
        public House House { get; set; }

        [DataMember]
        [XmlArray(ElementName = "OOIs")]
        public Ceo[] Ceos { get; set; }

        [DataMember]
        [XmlArray(ElementName = "Lifts")]
        public Lift[] Lifts { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public GetHouseInfoDpkrResult Result { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "House")]
    public class House
    {
        [DataMember]
        [XmlAttribute(AttributeName = "address")]
        public virtual string Address { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "yearbuilding")]
        public virtual string YearBuilding { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "totalarea")]
        public virtual string TotalArea { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "series")]
        public virtual string Serial { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "livingarea")]
        public virtual string LivingArea { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "nonlivingarea")]
        public virtual string NoLivingArea { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "floor")]
        public virtual string Floor { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "porch")]
        public virtual string Porch { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "flat")]
        public virtual string Flat { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "point")]
        public virtual int Point { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "OOI")]
    public class Ceo
    {
        [DataMember]
        [XmlAttribute(AttributeName = "OOI_name")]
        public virtual string CeoName { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "KE_name")]
        public virtual string StructElName { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "status")]
        public virtual int Status { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "delay")]
        public virtual string Delay { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "lifetime")]
        public virtual int LifeTime { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "lastyear")]
        public virtual int LastYear { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "ball")]
        public virtual int Point { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "period")]
        public virtual string Period { get; set; }
    }

    [DataContract]
    [XmlType(TypeName = "Lift")]
    public class Lift
    {

        [DataMember]
        [XmlAttribute(AttributeName = "lift_num")]
        public virtual string LiftNum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "porch_num")]
        public virtual string PorchNum { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "capacity")]
        public virtual string Capacity { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "stop")]
        public virtual string Stop { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "year_installation")]
        public virtual string YearInstallation { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "lifetime")]
        public virtual string LifeTime { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "period")]
        public virtual string Period { get; set; }
    }


    [DataContract]
    [XmlType(TypeName = "Result")]
    public class GetHouseInfoDpkrResult
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Kod")]
        public virtual string Kod { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public virtual string Name { get; set; }
    }
}