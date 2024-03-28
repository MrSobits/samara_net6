namespace Bars.Gkh.Gis.DomainService.Analysis
{
    using Bars.B4;

    public interface IRegressionAnalysisService
    {
        IDataResult GroupedTypeWithoutEmptyGroupList(BaseParams baseParams);
        IDataResult IndicatorsRegressionAnalysis(BaseParams baseParams);
        IDataResult ChartRegressionAnalysis(BaseParams baseParams);
    }
}
