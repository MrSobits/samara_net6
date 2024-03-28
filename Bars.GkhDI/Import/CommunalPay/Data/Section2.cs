namespace Bars.GkhDi.Import.Data
{
    using System;

    using Bars.B4.Utils;

    public class Section2
    {
        public string CodeErc { get; set; }

        public decimal? HouseArea { get; set; }

        public DateTime? DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public string TerminateReason { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            CodeErc = parts[0];
            HouseArea = !string.IsNullOrEmpty(parts[5]) ? parts[5].ToDecimal().To<decimal?>() : null;
            DateBegin = !string.IsNullOrEmpty(parts[6]) ? parts[6].To<DateTime?>() : null;
            DateEnd = !string.IsNullOrEmpty(parts[7]) ? parts[7].To<DateTime?>() : null;
            TerminateReason = parts[8];
        }
    }
}

