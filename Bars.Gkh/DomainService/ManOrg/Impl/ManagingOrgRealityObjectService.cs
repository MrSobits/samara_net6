namespace Bars.Gkh.DomainService
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис работы с УО и домами
    /// </summary>
    public class ManagingOrgRealityObjectService : IManagingOrgRealityObjectService
    {
        private readonly IWindsorContainer container;
        private readonly IDomainService<ManOrgContractRealityObject> manOrgContractDomain;

        /// <summary>
        /// .ctor
        /// </summary>
        public ManagingOrgRealityObjectService(
            IWindsorContainer container,
            IDomainService<ManOrgContractRealityObject> manOrgContractDomain)
        {
            this.container = container;
            this.manOrgContractDomain = manOrgContractDomain;
        }

        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            using (var transaction = this.container.Resolve<IDataTransaction>())
            {
                var serviceManorg = this.container.Resolve<IDomainService<ManagingOrganization>>();
                var serviceManorgRobject = this.container.Resolve<IDomainService<ManagingOrgRealityObject>>();
                var serviceRobject = this.container.Resolve<IDomainService<RealityObject>>();

                try
                {
                    var manorgId = baseParams.Params.GetAs<long>("manorgId");
                    var objectIds = baseParams.Params.GetAs("objectIds", new long[0]);

                    var listObjects =
                        serviceManorgRobject.GetAll()
                            .Where(x => x.ManagingOrganization.Id == manorgId)
                            .Select(x => x.RealityObject.Id)
                            .Distinct()
                            .ToList();

                    var manorg = serviceManorg.Load(manorgId);

                    foreach (var id in objectIds)
                    {
                        if (!listObjects.Contains(id))
                        {
                            var newManorgRealobj = new ManagingOrgRealityObject
                                {
                                    RealityObject = serviceRobject.Load(id),
                                    ManagingOrganization = manorg
                                };

                            serviceManorgRobject.Save(newManorgRealobj);
                        }
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.container.Release(serviceManorg);
                    this.container.Release(serviceManorgRobject);
                    this.container.Release(serviceRobject);
                }
            }
        }

        public ManOrgBaseContract GetCurrentManOrg(RealityObject realityObject)
        {
            return this.GetManOrgOnDate(realityObject, DateTime.Now);
        }

        /// <inheritdoc />
        public ManOrgBaseContract GetManOrgOnDate(RealityObject realityObject, DateTime date)
        {
            return this.manOrgContractDomain.GetAll()
                .Where(x => x.RealityObject.Id == realityObject.Id)
                .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= date)
                .Where(x => x.ManOrgContract.StartDate <= date)
                .OrderByDescending(x => x.ManOrgContract.StartDate)
                .FirstOrDefault().Return(x => x.ManOrgContract);
        }

        public IQueryable<ManOrgContractRealityObject> GetAllActive(DateTime date)
        {
            return this.manOrgContractDomain.GetAll()
                .Where(x => !x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate <= date)
                .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= date);
        }

        public IQueryable<ManOrgContractRealityObject> GetAllActive()
        {
            return this.GetAllActive(DateTime.Now.Date);
        }

        public IQueryable<ManOrgContractRealityObject> GetAllActive(DateTime dateStart, DateTime? dateEnd)
        {
            return this.manOrgContractDomain.GetAll()
                .WhereIf(dateEnd.HasValue, x => x.ManOrgContract.StartDate <= dateEnd.Value)
                .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= dateStart);
        }
    }
}