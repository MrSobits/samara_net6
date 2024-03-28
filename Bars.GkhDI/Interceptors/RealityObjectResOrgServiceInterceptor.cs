namespace Bars.GkhDi.Interceptors
{
    using B4;
    using Entities;
    using System.Linq;

    public class RealityObjectResOrgServiceInterceptor : EmptyDomainInterceptor<RealityObjectResOrgService>
    {
        public override IDataResult BeforeCreateAction(IDomainService<RealityObjectResOrgService> service, RealityObjectResOrgService entity)
        {
            if (service.GetAll().Any(x => (
                x.Id != entity.Id && 
                x.RoResOrg == entity.RoResOrg && 
                x.Service == entity.Service &&
                ((x.EndDate >= entity.StartDate && x.StartDate <= entity.StartDate) ||
                (x.StartDate <= entity.EndDate && x.EndDate >= entity.EndDate)))))// не может быть добавлена одна и та же услуга с пересекающимися периодами предоставления
            {
                return this.Failure("В заданном периоде уже добавлена такая услуга.");
            }

            return this.Success();
        }
    }
}
