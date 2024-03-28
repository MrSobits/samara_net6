namespace Bars.Gkh.RegOperator.Wcf.Contracts.PersonalAccount
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PersonalAccountInfoIn
    {
        [DataMember]
        public string Fio { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string Inn { get; set; }
        [DataMember]
        public string Kpp { get; set; }
    }
}