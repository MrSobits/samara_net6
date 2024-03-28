namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot(ElementName = "file")]
    public class File
    {
        [XmlArray("personal-accounts")]
        [XmlArrayItem("personal-account", typeof(PersonalAccount))]
        public PersonalAccount[] PersonalAccounts { get; set; }

        [XmlAttribute("file-date")]
        public string FileDate { get; set; }

        [XmlAttribute("file-num")]
        public string FileNumber { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "personal-account")]
    public class PersonalAccount
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("number")]
        public string Number { get; set; }

        [XmlAttribute("surname")]
        public string Surname { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("patronymic")]
        public string Patronymic { get; set; }

        [XmlAttribute("inn")]
        public string Inn { get; set; }

        [XmlAttribute("kpp")]
        public string Kpp { get; set; }

        [XmlAttribute("reason")]
        public string Reason { get; set; }

        [XmlAttribute("cur-account")]
        public string CurrentAccount { get; set; }

        [XmlAttribute("date")]
        public string Date { get; set; }

        [XmlElement("comment")]
        public string Comment { get; set; }

        [XmlAttribute("doc-num")]
        public string DocNum { get; set; }

        [XmlAttribute("doc-date")]
        public string DocDate { get; set; }

        [XmlArray("payments")]
        [XmlArrayItem("payment", typeof(Payment))]
        public Payment[] Payments { get; set; }
    }

    [Serializable]
    [XmlType(TypeName = "payment")]
    public class Payment
    {
        [XmlAttribute("target")]
        public string Target { get; set; }

        [XmlAttribute("sum")]
        public string Sum { get; set; }
    }
}