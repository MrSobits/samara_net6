namespace Bars.Gkh.Gis.DomainService.Analysis
{
    using Entities.IndicatorServiceComparison;

    public class MultipleAnalysisProxy
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public decimal? DeviationPercent { get; set; }
        public decimal? ExactValue { get; set; }
        public IndicatorServiceComparison IndicatorServiceComparison { get; set; }

        public bool leaf
        {
            get { return true; }
        }
    }
}