namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Castle.Windsor;

    public class AdminCaseProvidedDocService : IAdminCaseProvidedDocService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddProvidedDocs(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<AdministrativeCaseProvidedDoc>>();
            var serviceAdminCase = Container.Resolve<IDomainService<AdministrativeCase>>();
            var serviceProvDoc = Container.Resolve<IDomainService<ProvidedDocGji>>();

            var documentId = baseParams.Params.GetAs<long>("documentId");
            var ids = baseParams.Params.GetAs<long[]>("objectIds");

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var existRecs =
                        service.GetAll()
                            .Where(x => x.AdministrativeCase.Id == documentId)
                            .Where(x => ids.Contains(x.ProvidedDoc.Id))
                            .Select(x => x.ProvidedDoc.Id)
                            .Distinct()
                            .ToList();

                    var adminCase = serviceAdminCase.Load(documentId);

                    foreach (var id in ids)
                    {
                        if (existRecs.Contains(id))
                        {
                            continue;
                        }

                        var newRec = new AdministrativeCaseProvidedDoc
                        {
                            AdministrativeCase = adminCase,
                            ProvidedDoc = serviceProvDoc.Load(id)
                        };

                        service.Save(newRec);
                    }

                    tr.Commit();
                }
                catch (ValidationException e)
                {
                    tr.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(service);
                    Container.Release(serviceAdminCase);
                    Container.Release(serviceProvDoc);
                }
            }

            return new BaseDataResult();
        }
    }
}