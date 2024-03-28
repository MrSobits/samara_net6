using System.Linq;

namespace Bars.GkhGji.DomainService
{
    using B4;
    using Bars.GkhGji.DomainService.Contracts;
    using Entities;

    public interface IDisposalService
    {
        DisposalInfo GetInfo(long documentId);

        IDataResult GetInfo(BaseParams baseParams);

        IDataResult ListView(BaseParams baseParams);

        IDataResult ListNullInspection(BaseParams baseParams);

        IDataResult ListForExport(BaseParams baseParams);

        // для Сахи
        IDataResult AutoAddProvidedDocuments(BaseParams baseParams);

        IQueryable<ViewDisposal> GetViewList();
        IDataResult ListControlType(BaseParams baseParams);
    }
}