namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Enums;
    using Bars.B4.Modules.FileStorage;
    using System.Xml.Serialization;

    [XmlRootAttribute("request", Namespace = "urn://ru/mvd/ibd-m/convictions/search/1.0.2", IsNullable = false)]
    public class MvdSendRequest
    {
        [XmlAttribute(DataType = "ID", AttributeName = "Id")]
        public String Id { get; set; }

        [XmlElement(ElementName = "birthDate", Order = 1)]
        public BirthDate BirthDate { get; set; }

        [XmlElement(ElementName = "SNILS", Order = 2)]
        public String SNILS { get; set; }

        [XmlElement(ElementName = "surname", Order = 3)]
        public String Surname { get; set; }

        [XmlElement(ElementName = "name", Order = 4)]
        public String Name { get; set; }

        [XmlElement(ElementName = "patronymicName", Order = 5)]
        public String PatronymicName { get; set; }

        [XmlElement(ElementName = "registrationPlace", Order = 6)]
        public List<RegistrationPlace> RegistrationPlaces { get; set; }
    }

    public class BirthDate
    {
        [XmlElement(ElementName = "year", Order = 1)]
        public Int32 Year { get; set; }

        [XmlElement(ElementName = "month", Order = 2)]
        public Int32 Month { get; set; }

        [XmlElement(ElementName = "day", Order = 3)]
        public Int32 Day { get; set; }
    }

    public class RegistrationPlace
    {
        [XmlElement(ElementName = "type", Order = 1)]
        public String Type { get; set; }

        [XmlElement(ElementName = "regionCode", Order = 2)]
        public String RegionCode { get; set; }

        [XmlElement(ElementName = "place", Order = 3)]
        public String Place { get; set; }
    }
}
