namespace Bars.Gkh.Overhaul.Interceptors
{
    using System.Linq;

    using B4;
    using Entities;

    using Bars.B4.Utils;
    using Gkh.Entities.CommonEstateObject;

    public class StructuralElementInterceptor : EmptyDomainInterceptor<StructuralElement>
    {
        public IDomainService<RealityObjectStructuralElement> RealityObjectStructuralElementDomain { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<StructuralElement> service, StructuralElement entity)
        {
            var workService = Container.Resolve<IDomainService<StructuralElementWork>>();

            workService.GetAll()
                .Where(x => x.StructuralElement.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => workService.Delete(x));

            var realityObjectStructuralElement = RealityObjectStructuralElementDomain.GetAll().Count(x => x.StructuralElement.Id == entity.Id);
            if (realityObjectStructuralElement >= 1)
            {
                return Failure("Существуют связанные записи в разделе Конструктивные характеристики жилого дома;");
            }

            return Success();
        }
    }
}