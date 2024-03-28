namespace Bars.GkhDi.Import.Data
{
    using System;

    using Bars.B4.Utils;

    public class Section6
    {
        public string DocumentName { get; set; }

        public decimal? Size { get; set; }

        public DateTime? DocumentDate { get; set; }

        public decimal? SizePayments { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            DocumentName = parts[0];
            Size = !string.IsNullOrEmpty(parts[1]) ? parts[1].ToLong().ToDecimal().To<decimal?>() : null;
            DocumentDate = !string.IsNullOrEmpty(parts[2]) ? parts[2].ToDateTime().To<DateTime?>() : null;
            SizePayments = !string.IsNullOrEmpty(parts[3]) ? parts[3].ToDecimal().To<decimal?>() : null;
        }
    }
}

