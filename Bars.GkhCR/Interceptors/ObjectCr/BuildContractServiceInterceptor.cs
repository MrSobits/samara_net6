namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.Entities;

    public class BuildContractServiceInterceptor : EmptyDomainInterceptor<BuildContract>
    {
        public override IDataResult BeforeCreateAction(IDomainService<BuildContract> service, BuildContract entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<BuildContract> service, BuildContract entity)
        {
            var buildContractTypeWorkService = Container.Resolve<IDomainService<BuildContractTypeWork>>();
            var buildContractTypeWorList =
                buildContractTypeWorkService.GetAll()
                    .Where(x => x.BuildContract.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToArray();
            foreach (var id in buildContractTypeWorList)
            {
                buildContractTypeWorkService.Delete(id);
            }

            return Success();
        }
    }
}