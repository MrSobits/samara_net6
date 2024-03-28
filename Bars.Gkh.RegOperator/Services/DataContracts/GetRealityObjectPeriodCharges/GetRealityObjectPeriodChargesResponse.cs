namespace Bars.Gkh.RegOperator.Services.DataContracts
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;
    using B4.Utils;
    using Gkh.Services.DataContracts;

    [DataContract]
    [XmlType(TypeName = "GetRealityObjectPeriodChargesResponse")]
    public class GetRealityObjectPeriodChargesResponse
    {
        [DataMember]
        [XmlArray(ElementName = "ChargedAndPaidSums")]
        public List<ChargedAndPaidSum> ChargedAndPaidSums { get; set; }

        [DataMember]
        [XmlElement(ElementName = "Charge")]
        public ChargeProxy ChargeSum { get; set; }

        [DataMember]
        [XmlElement(ElementName = "TotalChargedSum")]
        public decimal TotalChargedSum { get; set; }

        [DataMember]
        [XmlElement(ElementName = "TotalPaidSum")]
        public decimal TotalPaidSum { get; set; }

        [DataMember]
        [XmlElement(ElementName = "IncreaseTotalChargedSum")]
        public decimal IncreaseTotalChargedSum
        {
            get
            {
                if (ChargedAndPaidSums.IsEmpty())
                    return 0m;

                return ChargedAndPaidSums.Sum(x => x.ChargedSum);
            }
        }

        [DataMember]
        [XmlElement(ElementName = "IncreaseTotalPaidSum")]
        public decimal IncreaseTotalPaidSum
        {
            get
            {
                if (ChargedAndPaidSums.IsEmpty())
                    return 0m;

                return ChargedAndPaidSums.Sum(x => x.PaidSum);
            }
        }

        [DataMember]
        [XmlElement(ElementName = "Result")]
        public Result Result { get; set; }
    }
}