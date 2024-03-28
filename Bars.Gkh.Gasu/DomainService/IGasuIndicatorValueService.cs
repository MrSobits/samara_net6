namespace Bars.Gkh.Gasu.DomainService
{
    using Bars.B4;

    public interface IGasuIndicatorValueService
    {
        IDataResult CreateRecords(BaseParams baseParams);
        
        IDataResult GetListYears(BaseParams baseParams);
    }
}
