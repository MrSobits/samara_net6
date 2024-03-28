namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Entities;

    public interface ITechPassportService 
    {
        IDataResult GetForm(BaseParams baseParams);

        IDataResult UpdateForm(BaseParams baseParams);

        IDataResult GetReportId(BaseParams baseParams);

        IDataResult GetPrintFormResult(BaseParams baseParams);

        TehPassportValue GetValue(long realityObjectId, string formId, string cellCode);
    }
}