namespace Bars.GkhGji.Regions.Smolensk.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    public class ResolutionServiceInterceptor : GkhGji.Interceptors.ResolutionServiceInterceptor
    {
        public override IDataResult BeforeDeleteAction(IDomainService<Resolution> service, Resolution entity)
        {

            var physInfoService = Container.Resolve<IDomainService<DocumentGJIPhysPersonInfo>>();
            var longDescService = Container.Resolve<IDomainService<ResolutionLongDescription>>();
            
            try
            {
                physInfoService.GetAll()
                    .Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => physInfoService.Delete(x));

                longDescService.GetAll()
                    .Where(x => x.Resolution.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => longDescService.Delete(x));

                return base.BeforeDeleteAction(service, entity);
            }
            finally 
            {
                Container.Release(physInfoService);
                Container.Release(longDescService);
            }
        }
    }
}