namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class RealityObjectVidecamInterceptor : EmptyDomainInterceptor<RealityObjectVidecam>
    {
        public override IDataResult AfterCreateAction(IDomainService<RealityObjectVidecam> service, RealityObjectVidecam entity)
        {
            if (entity.Workability == YesNoNotSet.Yes)
            {
                var roRepo = Container.Resolve<IRepository<RealityObject>>();
                var ro = roRepo.Get(entity.RealityObject.Id);
                ro.HasVidecam = true;
                roRepo.Update(ro);
            }

      
            return this.Success();
        }
    }
}
