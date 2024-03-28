namespace Bars.Gkh.RegOperator.Imports.VTB24.Dto.Origin
{
    using System;
    using System.Globalization;
    using System.Xml.Serialization;

    public class VtbOperationsTotalCounters
    {
        #region Commission

        [XmlIgnore]
        public decimal? Commission { get; set; }

        [XmlAttribute(AttributeName = "Commission")]
        public string CommissionString
        {
            get
            {
                return Commission == null
                    ? null
                    : ((decimal) Commission).ToString(VtbOperation.MoneyFormat, CultureInfo.InvariantCulture);
            }
            set
            {
                Commission = String.IsNullOrEmpty(value)
                    ? (decimal?) null
                    : Decimal.Parse(value, CultureInfo.InvariantCulture);
            }
        }

        #endregion

        #region Amount

        [XmlIgnore]
        public decimal Amount { get; set; }

        [XmlAttribute(AttributeName = "Amount")]
        public string AmountString
        {
            get { return Amount.ToString(VtbOperation.MoneyFormat, CultureInfo.InvariantCulture); }
            set
            {
                Amount = String.IsNullOrEmpty(value)
                    ? Decimal.Zero
                    : Decimal.Parse(value, CultureInfo.InvariantCulture);
            }
        }

        #endregion Amount

        [XmlAttribute]
        public int Count { get; set; }
    }
}
