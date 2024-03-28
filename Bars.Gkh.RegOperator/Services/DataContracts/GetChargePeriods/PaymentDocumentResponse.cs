namespace Bars.Gkh.RegOperator.Services.DataContracts.GetChargePeriods
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "PaymentDocumentResponse")]
    public class PaymentDocumentResponse
    {
        [DataMember]
        [XmlAttribute(AttributeName = "Message")]
        public string Message { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Success")]
        public bool Success { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "Base64File")]
        public string Base64File { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DocNumber")]
        public string DocNumber { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "TotalCharge")]
        public decimal TotalCharge { get; set; }

        public static PaymentDocumentResponse Fail(string message)
        {
            return new PaymentDocumentResponse
            {
                Message = message,
                Success = false
            };
        }

        public static PaymentDocumentResponse Result(string base64File, string docNumber, decimal totalCharge)
        {
            return new PaymentDocumentResponse
            {
                Success = true,
                Base64File = base64File,
                DocNumber = docNumber,
                TotalCharge = totalCharge
            };
        }
    }
}
