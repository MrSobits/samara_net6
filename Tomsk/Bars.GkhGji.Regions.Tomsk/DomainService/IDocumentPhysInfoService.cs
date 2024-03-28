namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using B4;
    using Entities;
    using GkhGji.Entities;

    public interface IDocumentPhysInfoService
    {
        DocumentPhysInfo GetByDocument(DocumentGji document);
    }
}