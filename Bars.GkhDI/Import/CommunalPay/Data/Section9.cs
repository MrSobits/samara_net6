namespace Bars.GkhDi.Import.Data
{
    using System;

    using Bars.B4.Utils;

    public class Section9
    {
        public string UnionName { get; set; }

        public string Address { get; set; }

        public string OfficialSite { get; set; }

        public DateTime? DateBegin { get; set; }

        public DateTime? DateEnd { get; set; }

        public void Parse(string line)
        {
            var parts = line.Split(';');
            UnionName = parts[0];
            Address = parts[1];
            OfficialSite = parts[2];
            DateBegin = !string.IsNullOrEmpty(parts[3]) ? parts[3].ToDateTime().To<DateTime?>() : null;
            DateEnd = !string.IsNullOrEmpty(parts[4]) ? parts[4].ToDateTime().To<DateTime?>() : null;
        }
    }
}

