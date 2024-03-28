namespace Bars.GkhGji.Regions.Tomsk.Interceptors.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tomsk.Entities.Dict;

    public class TomskViolationGjiInterceptor : ViolationGjiInterceptor<TomskViolationGji>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<TomskViolationGji> service, TomskViolationGji entity)
        {
            var res = base.BeforeDeleteAction(service, entity);
            if (res.Success)
            {
                var descriptionService = this.Container.ResolveDomain<TomskViolationGjiDescription>();
                try
                {
                    var description = descriptionService.GetAll().FirstOrDefault(x => x.ViolationGji.Id == entity.Id);
                    if (description != null)
                    {
                        descriptionService.Delete(description.Id);
                    }
                }
                finally
                {
                    this.Container.Release(descriptionService);
                }
            }

            return res;
        }
    }
}