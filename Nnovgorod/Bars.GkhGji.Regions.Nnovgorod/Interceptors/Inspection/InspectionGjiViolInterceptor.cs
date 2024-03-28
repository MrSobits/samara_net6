namespace Bars.GkhGji.Regions.Nnovgorod.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nnovgorod.Entities;

    public class InspectionGjiViolInterceptor : GkhGji.Interceptors.InspectionGjiViolInterceptor
    {
        public IDomainService<InspectionGjiViolWording> InspectionGjiViolWordingService { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<InspectionGjiViol> service, InspectionGjiViol entity)
        {
            var result = base.BeforeDeleteAction(service, entity);
            
            if (!result.Success)
            {
                return result;
            }

            this.InspectionGjiViolWordingService.GetAll()
                .Where(x => x.InspectionViolation.Id == entity.Id)
                .Select(x => x.Id)
                .AsEnumerable()
                .ForEach(x => this.InspectionGjiViolWordingService.Delete(x));

            return this.Success();
        }
    }
}