namespace Bars.Gkh.RegOperator.Imports.VTB24.Dto.Origin
{
    using System.Xml.Serialization;

    public enum VtbPaymentType : byte
    {
        [XmlEnum("1")] 
        Payment = 1,

        [XmlEnum("2")] 
        Penalty = 2
    }
}
