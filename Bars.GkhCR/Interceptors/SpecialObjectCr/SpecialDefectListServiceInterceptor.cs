namespace Bars.GkhCr.Interceptors
{
    using B4;
    using B4.Modules.States;
    using Entities;

    public class SpecialDefectListServiceInterceptor : EmptyDomainInterceptor<SpecialDefectList>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SpecialDefectList> service, SpecialDefectList entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            if (entity.TypeWork != null)
            {
                entity.Work = entity.TypeWork.Work;
            }

            return this.Success();
        }
    }
}