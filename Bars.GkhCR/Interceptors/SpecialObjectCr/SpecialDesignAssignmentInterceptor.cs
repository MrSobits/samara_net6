namespace Bars.GkhCr.Interceptors
{
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhCr.Entities;

    public class SpecialDesignAssignmentInterceptor : EmptyDomainInterceptor<SpecialDesignAssignment>
    {
        public override IDataResult BeforeCreateAction(IDomainService<SpecialDesignAssignment> service, SpecialDesignAssignment entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return this.Success();
        }
    }
}