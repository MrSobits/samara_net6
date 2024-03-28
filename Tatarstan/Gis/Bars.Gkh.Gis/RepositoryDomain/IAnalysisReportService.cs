namespace Bars.Gkh.Gis.DomainService
{
    using B4;
    using NHibernate.Persister.Entity;

    public interface IAnalysisReportService
    {
        IDataResult GetAnalysisReportData(BaseParams baseParams);
    }
}
