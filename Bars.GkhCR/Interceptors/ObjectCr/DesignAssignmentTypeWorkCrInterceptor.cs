namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.Entities;

    public class DesignAssignmentTypeWorkCrInterceptor : EmptyDomainInterceptor<DesignAssignmentTypeWorkCr>
    {

        public override IDataResult BeforeCreateAction(IDomainService<DesignAssignmentTypeWorkCr> service, DesignAssignmentTypeWorkCr entity)
        {
            var TWCR = entity.TypeWorkCr;


            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<DesignAssignmentTypeWorkCr> service, DesignAssignmentTypeWorkCr entity)
        {
            var TWCR = entity.TypeWorkCr;


            return Success();
        }
    }
}