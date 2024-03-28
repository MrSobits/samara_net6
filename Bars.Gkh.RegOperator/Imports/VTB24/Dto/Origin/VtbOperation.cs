namespace Bars.Gkh.RegOperator.Imports.VTB24.Dto.Origin
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Xml.Serialization;

    public class VtbOperation
    {
        private const string DateFormat = "s";
        private const string InputAccountConst = "40703810015020001040";
        private const string PaymentPeriodFormat = "MMyyyy";
        public const string MoneyFormat = "F2";

        private static readonly Regex AccountValidator = new Regex(@"^\d{9}$");

        [XmlAttribute]
        public string Uni { get; set; }

        [XmlAttribute(AttributeName = "Number")]
        public long ReceiptNumber { get; set; }

        #region DateOperation

        [XmlAttribute(AttributeName = "DateOperation")]
        public string DateOperationString
        {
            get { return DateOperation.ToString(DateFormat); }
            set { DateOperation = DateTime.ParseExact(value, DateFormat, CultureInfo.InvariantCulture); }
        }

        [XmlIgnore]
        public DateTime DateOperation { get; set; }

        #endregion

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
                    : ((decimal) Commission).ToString(MoneyFormat, CultureInfo.InvariantCulture);
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
            get { return Amount.ToString(MoneyFormat, CultureInfo.InvariantCulture); }
            set
            {
                Amount = String.IsNullOrEmpty(value)
                    ? Decimal.Zero
                    : Decimal.Parse(value, CultureInfo.InvariantCulture);
            }
        }

        #endregion Amount

        [XmlAttribute(AttributeName = "PHONE")]
        public string Phone { get; set; }

        [XmlAttribute(AttributeName = "FIELD1")]
        public string InputAccount { get; set; }

        [XmlAttribute(AttributeName = "FIELD2")]
        public VtbPaymentType PaymentType { get; set; }

        #region PaymentPeriod

        [XmlAttribute(AttributeName = "FIELD3")]
        public string PaymentPeriodString
        {
            get { return PaymentPeriod.ToString(PaymentPeriodFormat); }
            set { PaymentPeriod = DateTime.ParseExact(value, PaymentPeriodFormat, CultureInfo.InvariantCulture); }
        }

        [XmlIgnore]
        public DateTime PaymentPeriod { get; set; }

        #endregion

        public bool IsValid()
        {
            return (!String.IsNullOrEmpty(Uni) && Uni.Length == 25) && ReceiptNumber > 0 &&
                   InputAccount == InputAccountConst && AccountValidator.IsMatch(Phone);
        }
    }
}
