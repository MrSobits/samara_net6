namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Modules.ClaimWork.Entities;

    public class ObjectCrInterceptor : EmptyDomainInterceptor<ObjectCr>
    {
        public ObjectCrInterceptor(IDomainService<BuildContractClaimWork> buildContractClwService)
        {
            this.BuildContractClwService = buildContractClwService;
        }

        private IDomainService<BuildContractClaimWork> BuildContractClwService { get; set; }

        public override IDataResult AfterUpdateAction(IDomainService<ObjectCr> service, ObjectCr entity)
        {
            var claimWorks = BuildContractClwService.GetAll().Where(x => x.BuildContract.ObjectCr.Id == entity.Id);
            foreach (var claimWork in claimWorks)
            {
                claimWork.RealityObject = entity.RealityObject;
                BuildContractClwService.Update(claimWork);
            }

            return base.AfterUpdateAction(service, entity);
        }
    }
}