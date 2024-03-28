namespace Bars.Gkh.Gis.DomainService.Analysis
{
    using Bars.Gkh.Entities.Dicts;

    using Entities.Register.HouseRegister;
    using Enum;

    public class MultipleAnalysisIndicatorProxy
    {
        public HouseRegister House { get; set; }
        public GisTypeIndicator GisTypeIndicator { get; set; }
        public ServiceDictionary Service { get; set; }
        public decimal? Value { get; set; }
    }
}