using System.Linq;

namespace Bars.Gkh.Interceptors.ServOrg
{
    using B4;
    using Entities;

    public class ServiceOrgContractInterceptor : EmptyDomainInterceptor<ServiceOrgContract>
    {

        public override IDataResult AfterCreateAction(IDomainService<ServiceOrgContract> service,
            ServiceOrgContract entity)
        {
            var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();
            var serviceContractRobject = Container.Resolve<IDomainService<ServiceOrgRealityObjectContract>>();

            var newRecord = new ServiceOrgRealityObjectContract
            {
                ServOrgContract = entity,
                RealityObject = serviceRobject.Load(entity.RealityObjectId)
            };

            serviceContractRobject.Save(newRecord);

            Container.Release(serviceRobject);
            Container.Release(serviceContractRobject);
            
            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ServiceOrgContract> service, ServiceOrgContract entity)
        {
            var sOrgContractRealObjService = Container.Resolve<IDomainService<ServiceOrgRealityObjectContract>>();
            var sOrgContractRealObjIds = sOrgContractRealObjService.GetAll().Where(x => x.ServOrgContract.Id == entity.Id).Select(x => x.Id).ToList();
            foreach (var value in sOrgContractRealObjIds)
            {
                sOrgContractRealObjService.Delete(value);
            }

            Container.Release(sOrgContractRealObjService);

            return Success();
        }

        public override IDataResult BeforeCreateAction(IDomainService<ServiceOrgContract> service, ServiceOrgContract entity)
        {
            return CheckFields(entity);
        }

        private IDataResult CheckFields(ServiceOrgContract entity)
        {
            var dateStart = entity.DateStart;
            var dateEnd = entity.DateEnd;
            var dateDoc = entity.DocumentDate;

            if (dateDoc != null && dateDoc > dateStart)
            {
                return Failure("Дата договора не должна быть позже даты начала действия договора!");
            }

            if (dateStart > dateEnd)
            {
                return Failure("Дата начала действия не должна быть позже даты окончания действия договора!");
            }
            return Success();
        }

    }
}