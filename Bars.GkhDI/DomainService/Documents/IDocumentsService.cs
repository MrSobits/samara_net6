namespace Bars.GkhDi.DomainService
{
    using Bars.B4;

    public interface IDocumentsService
    {
        IDataResult GetIdByDisnfoId(BaseParams baseParams);

        IDataResult CopyDocs(BaseParams baseParams);
    }
}