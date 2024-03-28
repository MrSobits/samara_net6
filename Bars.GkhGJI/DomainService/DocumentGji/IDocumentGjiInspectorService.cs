namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using System.Linq;
    using Bars.GkhGji.Entities;

    public interface IDocumentGjiInspectorService
    {
        IDataResult AddInspectors(BaseParams baseParams);
        IQueryable<DocumentGjiInspector> GetInspectorsByDocumentId(long? documentId);
    }
}