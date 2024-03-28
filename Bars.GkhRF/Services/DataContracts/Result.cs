namespace Bars.GkhRf.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "Result")]
    public class Result
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Code")]
        public string Code { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Name")]
        public string Name { get; set; }

        public static Result NoErrors
        {
            get
            {
                return new Result { Code = "00", Name = "NO_ERRORS" };
            }
        }

        public static Result DataNotFound
        {
            get
            {
                return new Result { Code = "01", Name = "DATA_NOT_FOUND" };
            }
        }

        public static Result NotMoCode
        {
            get
            {
                return new Result { Code = "02", Name = "NOT_MO_CODE" };
            }
        }

        public static Result NotCityCode
        {
            get
            {
                return new Result { Code = "03", Name = "NOT_CITY_CODE" };
            }
        }

        public static Result NotStreetCode
        {
            get
            {
                return new Result { Code = "04", Name = "NOT_STREET_CODE" };
            }
        }
    }
}