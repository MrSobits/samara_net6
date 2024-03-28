namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;
    using GkhGji.Entities;

    public class DocumentPhysInfoService : IDocumentPhysInfoService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<DocumentPhysInfo> Domain { get; set; }

        public DocumentPhysInfo GetByDocument(DocumentGji document)
        {
            if (document == null)
            {
                return null;
            }

            return Domain.GetAll().FirstOrDefault(x => x.Document.Id == document.Id);
        }
    }
}