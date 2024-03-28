namespace Bars.Gkh.Services.DataContracts
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

        [DataMember]
        [XmlAttribute(AttributeName = "Message")]
        public string Message { get; set; }

        public static Result NoErrors => new Result { Code = "00", Name = "NO_ERRORS" };

        public static Result DataNotFound => new Result { Code = "01", Name = "DATA_NOT_FOUND" };

        public static Result NotMoCode => new Result { Code = "02", Name = "NOT_MO_CODE" };

        public static Result NotCityCode => new Result { Code = "03", Name = "NOT_CITY_CODE" };

        public static Result NotStreetCode => new Result { Code = "04", Name = "NOT_STREET_CODE" };

        public static Result AuthorizationFailed => new Result { Code = "05", Name = "AUTHORIZATION_FAILED" };

        public static Result NoLoginPassword => new Result { Code = "06", Name = "LOGIN_NOT_FOUND_OR_NULL" };
    }
}