namespace Bars.Gkh.RegOperator.Imports.VTB24.Dto.Origin
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Serialization;

    [XmlRoot(ElementName = "Message")]
    public class VtbMessage
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string VtbType = "ATFep";

        [XmlAttribute(AttributeName = "ID")]
        public long Id { get; set; }

        [XmlAttribute]
        public string Sender { get; set; }

        [XmlAttribute]
        public string Receiver { get; set; }

        [XmlAttribute]
        public string Type { get; set; }

        #region Date field depended

        [XmlIgnore]
        public DateTime Date { get; set; }

        [XmlAttribute(AttributeName = "Date")]
        public string DateAsString
        {
            get { return Date.ToString(DateFormat); }
            set { Date = DateTime.ParseExact(value, DateFormat, CultureInfo.InvariantCulture); }
        }

        #endregion

        [XmlArray(ElementName = "Operations", IsNullable = false)]
        [XmlArrayItem(ElementName = "Operation")]
        public List<VtbOperation> Operations { get; set; }

        [XmlElement(IsNullable = false)]
        public VtbOperationsTotalCounters Total { get; set; }

        public bool IsValid()
        {
            var result = Id > 0 && !String.IsNullOrEmpty(Sender) && !String.IsNullOrEmpty(Receiver) && Type == VtbType &&
                         Operations.TrueForAll(v => v.IsValid());
            if (result)
            {
                var count = Operations.Count;
                var amount = Operations.Sum(v => v.Amount);
                var commission = Operations.Sum(v => v.Commission ?? Decimal.Zero);
                result = Total.Count == count && Total.Amount == amount &&
                         (Total.Commission ?? Decimal.Zero) == commission;
            }
            return result;
        }
    }
}
