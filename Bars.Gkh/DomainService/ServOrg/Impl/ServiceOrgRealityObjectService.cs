namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Entities;

    using Castle.Windsor;

    public class ServiceOrgRealityObjectService : IServiceOrgRealityObjectService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var serviceOrg = Container.Resolve<IDomainService<ServiceOrganization>>();
                var serviceOrgRobject = Container.Resolve<IDomainService<ServiceOrgRealityObject>>();
                var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();

                try
                {
                    var servorgId = baseParams.Params.GetAs<long>("servorgId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

                    var listObjects =serviceOrgRobject.GetAll()
                            .Where(x => x.ServiceOrg.Id ==servorgId)
                            .Select(x => x.RealityObject.Id)
                            .Distinct()
                            .ToList();

                    var servorg = serviceOrg.Load(servorgId);

                    foreach (var id in objectIds)
                    {
                        if (!listObjects.Contains(id))
                        {
                            var newServorgRealobj = new ServiceOrgRealityObject
                            {
                                RealityObject = serviceRobject.Load(id),
                                ServiceOrg = servorg
                            };

                            serviceOrgRobject.Save(newServorgRealobj);
                        }
                    }

                    transaction.Commit();
                    return new BaseDataResult(true, "");

                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(serviceOrg);
                    Container.Release(serviceOrgRobject);
                    Container.Release(serviceRobject);
                }
            }
        }

//


        public IDataResult GetInfo(BaseParams baseParams)
        {
            var contractId = baseParams.Params.GetAs<long>("contractId");

            var robject =
                Container.Resolve<IDomainService<ServiceOrgRealityObjectContract>>().GetAll()
                    .Where(x => x.ServOrgContract.Id == contractId)
                    .Select(x => x.RealityObject)
                    .FirstOrDefault();

            return new BaseDataResult(robject);
        }
        
      }
}