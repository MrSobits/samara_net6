namespace Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PersonalAccountPaymentInfoOut
    {
        [DataMember]
        public string PaymentStatus { get; set; }
        [DataMember]
        public string Error { get; set; }
    }
}