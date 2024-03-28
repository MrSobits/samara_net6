namespace Bars.Gkh.Overhaul.Domain
{
    using System.Collections.Generic;

    public class PublProgRecRep
    {
        public string MuName { get; set; }

        public string Settlement { get; set; }

        public string Address { get; set; }

        public string Ooi { get; set; }

        public decimal? TotalAreaMkd { get; set; }

        public decimal? TotalAreaLivNotLiv { get; set; }

        public decimal? TotalAreaLiving { get; set; }

        public int Year { get; set; }

        public decimal Cost { get; set; }

        public long House { get; set; }
    }

    public interface IPublishedProgramReportDataProvider
    {
        IEnumerable<PublProgRecRep> GetData(long[] municipalityIds, int startYear, int endYear);
    }
}