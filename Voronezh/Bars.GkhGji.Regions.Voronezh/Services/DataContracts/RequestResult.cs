namespace Bars.GkhGji.Regions.Voronezh.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "RequestResult")]
    public class RequestResult
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

        public static RequestResult NoErrors => new RequestResult { Code = "00", Name = "NO_ERRORS" };

        public static RequestResult DataNotFound => new RequestResult { Code = "01", Name = "DATA_NOT_FOUND" };

        public static RequestResult IncorrectToken => new RequestResult { Code = "02", Name = "INCORRECT_TOKEN" };

        public static RequestResult InspectorUnknown => new RequestResult { Code = "03", Name = "OPERATOR_UNKNOWN" };

        public static RequestResult NotStreetCode => new RequestResult { Code = "04", Name = "NOT_STREET_CODE" };

        public static RequestResult AuthorizationFailed => new RequestResult { Code = "05", Name = "AUTHORIZATION_FAILED" };

        public static RequestResult NoLoginPassword => new RequestResult { Code = "06", Name = "LOGIN_NOT_FOUND_OR_NULL" };
    }
}