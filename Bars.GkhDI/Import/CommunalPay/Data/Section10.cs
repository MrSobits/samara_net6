namespace Bars.GkhDi.Import.Data
{
    using System;

    using Bars.B4.Utils;

    public class Section10
    {
        public string CodeErc { get; set; }

        public DateTime? From { get; set; }

        public string Number { get; set; }

        public string ContractMembers { get; set; }

        public DateTime? DateStart { get; set; }

        public DateTime? DateEnd { get; set; }

        public decimal? Cost { get; set; }

        public string Description { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            CodeErc = parts[0];
            From = !string.IsNullOrEmpty(parts[1]) ? parts[1].ToDateTime().To<DateTime?>() : null;
            Number = parts[2];
            ContractMembers = parts[3];
            DateStart = !string.IsNullOrEmpty(parts[4]) ? parts[4].ToDateTime().To<DateTime?>() : null;
            DateEnd = !string.IsNullOrEmpty(parts[5]) ? parts[5].ToDateTime().To<DateTime?>() : null;
            Cost = !string.IsNullOrEmpty(parts[6]) ? parts[6].ToDecimal().To<decimal?>() : null;
            Description = parts[7];
        }
    }
}

