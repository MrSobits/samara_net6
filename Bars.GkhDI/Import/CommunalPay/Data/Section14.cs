namespace Bars.GkhDi.Import.Data
{
    using System;

    using Bars.B4.Utils;

    public class Section14
    {
        public string CodeErc { get; set; }

        public string ContragentName { get; set; }

        public decimal? Area { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            CodeErc = parts[0];
            ContragentName = parts[2];
            Area = !string.IsNullOrEmpty(parts[3]) ? parts[3].ToDecimal().To<decimal?>() : null;
            DateStart = !string.IsNullOrEmpty(parts[4]) ? parts[4].ToDateTime().To<DateTime?>() : null;
            DateEnd = !string.IsNullOrEmpty(parts[5]) ? parts[5].ToDateTime().To<DateTime?>() : null;
        }
    }
}

