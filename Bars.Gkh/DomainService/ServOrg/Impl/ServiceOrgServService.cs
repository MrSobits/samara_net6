namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Entities;

    using Castle.Windsor;

    public class ServiceOrgServService : IServiceOrgServService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddTypeServiceObjects(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var serviceTypeService = Container.Resolve<IDomainService<TypeService>>();
                var serviceOrg = Container.Resolve<IDomainService<ServiceOrganization>>();
                var service = Container.Resolve<IDomainService<ServiceOrgService>>();

                try
                {
                    var servorgId = baseParams.Params.GetAs<long>("servorgId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

                    var existsRecs =
                        service.GetAll()
                            .Where(x => x.ServiceOrganization.Id == servorgId)
                            .Where(x => objectIds.Contains(x.TypeService.Id))
                            .Select(x => x.TypeService.Id)
                            .ToList();

                    var org = serviceOrg.Load(servorgId);

                    foreach (var id in objectIds)
                    {
                        if (existsRecs.Contains(id))
                            continue;

                        var newServiceOrgService = new ServiceOrgService
                            {
                                ServiceOrganization = org,
                                TypeService = serviceTypeService.Load(id)
                            };

                        service.Save(newServiceOrgService);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult(false, e.Message);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(serviceTypeService);
                    Container.Release(serviceOrg);
                    Container.Release(service);
                }
            }
        }
    }
}