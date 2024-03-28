namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Modules.ClaimWork.Entities;

    public class BuildContractInterceptor : EmptyDomainInterceptor<BuildContract>
    {
        public BuildContractInterceptor(IDomainService<BuildContractClaimWork> buildContractClwService)
        {
            this.BuildContractClwService = buildContractClwService;
        }

        private IDomainService<BuildContractClaimWork> BuildContractClwService { get; set; }

        public override IDataResult AfterUpdateAction(IDomainService<BuildContract> service, BuildContract entity)
        {
            var claimWorks = BuildContractClwService.GetAll().Where(x => x.BuildContract.Id == entity.Id);
            foreach (var claimWork in claimWorks)
            {
                claimWork.BaseInfo = string.Format(
                    "Договор № {0}{1}",
                    entity.DocumentNum,
                    entity.DocumentDateFrom.HasValue
                        ? string.Format(" от {0}", entity.DocumentDateFrom.Value.ToShortDateString())
                        : string.Empty);
                claimWork.RealityObject = entity.ObjectCr.RealityObject;

                BuildContractClwService.Update(claimWork);
            }

            return base.AfterUpdateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<BuildContract> service, BuildContract entity)
        {
            var bcTermService = Container.Resolve<IDomainService<BuildContractTermination>>();
            var buildContractTerminations = bcTermService.GetAll()
               .Where(x => x.BuildContract.Id == entity.Id)
               .ToList();
            foreach(var bcTerm in buildContractTerminations)
            {
                bcTermService.Delete(bcTerm);
            }

            Container.Release(bcTermService);
            return base.BeforeDeleteAction(service, entity);
        }
    }
}