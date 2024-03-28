namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    public interface ITemplateReplacementService
    {
        IDataResult ListReports(BaseParams baseParams);

        IDataResult GetBaseTemplate(BaseParams baseParams);
    }
}