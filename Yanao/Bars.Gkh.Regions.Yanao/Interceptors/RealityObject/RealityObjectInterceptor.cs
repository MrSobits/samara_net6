namespace Bars.Gkh.Regions.Yanao.Interceptors.RealityObject
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Regions.Yanao.Entities;

    public class RealityObjectInterceptor : Gkh.Interceptors.RealityObjectInterceptor
    {
        public IDomainService<RealityObjectExtension> serviceRealityObjectExtension { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<Gkh.Entities.RealityObject> service, Gkh.Entities.RealityObject entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            var listToDelete = serviceRealityObjectExtension.GetAll()
                .Where(x => x.RealityObject.Id == entity.Id)
                .Select(x => x.Id)
                .ToList();

            listToDelete.ForEach(x => serviceRealityObjectExtension.Delete(x));

            return result;
        }
    }
}
