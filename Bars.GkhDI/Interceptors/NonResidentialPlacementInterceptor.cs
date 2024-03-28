namespace Bars.GkhDi.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhDi.Entities;

    public class NonResidentialPlacementInterceptor : EmptyDomainInterceptor<NonResidentialPlacement>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<NonResidentialPlacement> service, NonResidentialPlacement entity)
        {
            var meterDeviceService = this.Container.Resolve<IDomainService<NonResidentialPlacementMeteringDevice>>();
            var idCollection = meterDeviceService.GetAll().Where(x => x.NonResidentialPlacement.Id == entity.Id).Select(x => x.Id).ToArray();

            foreach (var id in idCollection)
            {
                meterDeviceService.Delete(id);
            }

            return Success();
        }        
    }
}