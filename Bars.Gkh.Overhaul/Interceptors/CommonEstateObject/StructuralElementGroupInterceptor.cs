namespace Bars.Gkh.Overhaul.Interceptors
{
    using System.Linq;

    using B4;

    using Bars.B4.Utils;
    
    using Gkh.Entities.CommonEstateObject;

    public class StructuralElementGroupInterceptor : EmptyDomainInterceptor<StructuralElementGroup>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<StructuralElementGroup> service, StructuralElementGroup entity)
        {
            var atrsService = Container.Resolve<IDomainService<StructuralElementGroupAttribute>>();
            var seService = Container.Resolve<IDomainService<StructuralElement>>();

            atrsService.GetAll()
                .Where(x => x.Group.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => atrsService.Delete(x));

            seService.GetAll()
                .Where(x => x.Group.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => seService.Delete(x));

            return Success();
        }
    }
}