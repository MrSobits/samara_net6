namespace Bars.Gkh.RegOperator.Services.DataContracts.Accounts
{
    using Castle.MicroKernel.SubSystems.Conversion;
    using System;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "PersonalAccountDebtInfo")]
    public class PersonalAccountDebtInfo
    {
        /// <summary>
        /// номер счета
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "AccountNumber")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// дата открытия
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "OpenDate")]
        public DateTime OpenDate { get; set; }

        /// <summary>
        /// дата закрытия
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "CloseDate")]
        public DateTime? CloseDate { get; set; }

        /// <summary>
        /// итого задолженность
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "TotalDebt")]
        public decimal TotalDebt { get; set; }

        /// <summary>
        /// задолженность по взносам всего
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "FeeDebtTotal")]
        public decimal FeeDebtTotal { get; set; }

        /// <summary>
        /// задолженность пени всего
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "TotalPenaltyDebt")]
        public decimal TotalPenaltyDebt { get; set; }

        /// <summary>
        /// задолженность пени всего
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "LastPaymentSum")]
        public decimal LastPaymentSum { get; set; }

        /// <summary>
        /// дата закрытия
        /// </summary>
        [DataMember]
        [XmlElement(ElementName = "LastPaymentDate")]
        public DateTime? LastPaymentDate { get; set; }
    }

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

    [DataContract]
    [XmlRoot(ElementName = "PersonalAccountDebtInfoResponse")]
    public class PersonalAccountDebtInfoResponse
    {
        [DataMember]
        [XmlElement(ElementName = "PersonalAccountDebtInfo")]
        public PersonalAccountDebtInfo PersonalAccountDebtInfo { get; set; }

        [DataMember]
        [XmlElement(ElementName = "RequestResult")]
        public RequestResult RequestResult { get; set; }
    }
}