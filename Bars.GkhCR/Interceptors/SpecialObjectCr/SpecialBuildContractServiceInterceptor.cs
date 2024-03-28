namespace Bars.GkhCr.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhCr.Entities;

    public class SpecialBuildContractServiceInterceptor : EmptyDomainInterceptor<SpecialBuildContract>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SpecialBuildContract> service, SpecialBuildContract entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<SpecialBuildContract> service, SpecialBuildContract entity)
        {
            var buildContractTypeWorkService = this.Container.Resolve<IDomainService<SpecialBuildContractTypeWork>>();
            var buildContractTypeWorList =
                buildContractTypeWorkService.GetAll()
                    .Where(x => x.BuildContract.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToArray();
            foreach (var id in buildContractTypeWorList)
            {
                buildContractTypeWorkService.Delete(id);
            }

            return this.Success();
        }
    }
}