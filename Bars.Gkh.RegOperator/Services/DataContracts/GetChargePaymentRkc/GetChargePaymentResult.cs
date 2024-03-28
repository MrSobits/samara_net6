namespace Bars.Gkh.RegOperator.Services.DataContracts
{
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    [XmlType(TypeName = "RESULT")]
    public class GetChargePaymentResult
    {
        [DataMember]
        [XmlAttribute(AttributeName = "KOD")]
        public string Code { get; set; }

        [DataMember]
        [XmlAttribute(AttributeName = "DESCRIPTION")]
        public string Name { get; set; }

        public static GetChargePaymentResult NoErrors
        {
            get
            {
                return new GetChargePaymentResult { Code = "00", Name = "Успешно" };
            }
        }

        public static GetChargePaymentResult DataNotFound
        {
            get
            {
                return new GetChargePaymentResult { Code = "01", Name = "По запрашиваемому периоду нет информации" };
            }
        }

        public static GetChargePaymentResult NotCashPaymentCenter
        {
            get
            {
                return new GetChargePaymentResult { Code = "02", Name = "РКЦ не зарегистрирован в системе" };
            }
        }
    }
}