namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.Entities;

    public class DesignAssignmentInterceptor : EmptyDomainInterceptor<DesignAssignment>
    {
        public override IDataResult BeforeCreateAction(IDomainService<DesignAssignment> service, DesignAssignment entity)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<DesignAssignment> service, DesignAssignment entity)
        {
            var TWCR = entity.TypeWorksCr;


            return Success();
        }
    }
}