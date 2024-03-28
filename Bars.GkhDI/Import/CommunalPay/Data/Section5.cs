namespace Bars.GkhDi.Import.Data
{
    using System;

    using Bars.B4.Utils;

    public class Section5
    {
        public string OrganizationName { get; set; }

        public int? AmountViolation { get; set; }

        public DateTime? DateImpositionPenalty { get; set; }

        public decimal? SumPenalty { get; set; }

        public DateTime? DatePaymentPenalty { get; set; }

        public string FileName { get; set; }

        public string Actions { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            OrganizationName = parts[0];
            AmountViolation = !string.IsNullOrEmpty(parts[1]) ? parts[1].ToLong().To<int?>() : null;
            DateImpositionPenalty = !string.IsNullOrEmpty(parts[2]) ? parts[2].ToDateTime().To<DateTime?>() : null;
            SumPenalty = !string.IsNullOrEmpty(parts[3]) ? parts[3].ToDecimal().To<decimal?>() : null;
            DatePaymentPenalty = !string.IsNullOrEmpty(parts[4]) ? parts[4].ToDateTime().To<DateTime?>() : null;
            FileName = parts[5];
            Actions = parts[6];
        }
    }
}

