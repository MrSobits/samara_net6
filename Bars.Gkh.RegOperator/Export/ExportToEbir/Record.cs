namespace Bars.Gkh.RegOperator.Export.ExportToEbir
{
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "EbirResponse")]
    public class EbirResponse
    {
        [XmlArray(ElementName = "Records")]
        public Record[] Records { get; set; }
    }
    
    [XmlType(TypeName = "Record")]
    public class Record
    {
        [XmlElementAttribute(IsNullable = true)]
        public string AccountOperator { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string AccountNum { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string Surname { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string Name { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string Patronymic { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string Ulname { get; set; }

        public string INN { get; set; }

        public string KPP { get; set; }

        public decimal Dolya { get; set; }

        public string Address { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string ServiceCode { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string ProviderCode { get; set; }

        public int TYear { get; set; }

        public int TMonth { get; set; }
        
        public decimal SaldoIn { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string SaldoFineIn { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string ChargeType { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string UnitCode { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string ChargeVolume { get; set; }

        public string Tarif { get; set; }

        public decimal Area { get; set; }

        public decimal ChargeSum { get; set; }

        public decimal ReChargeSum { get; set; }

        public decimal CostsSum { get; set; }

        public decimal FineSum { get; set; }

        public decimal PaySum { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string PayBreak { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string LastPayDate { get; set; }

        public decimal PayFineSum { get; set; }

        public decimal SaldoOut { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string SaldoFineOut { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public string NameKO { get; set; }

        public string KredOrg { get; set; }

        public string BIKKO { get; set; }
    }
}