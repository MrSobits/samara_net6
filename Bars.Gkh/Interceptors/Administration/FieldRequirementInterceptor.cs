namespace Bars.Gkh.Interceptors
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class FieldRequirementInterceptor : EmptyDomainInterceptor<FieldRequirement>
    {
        public override IDataResult BeforeCreateAction(IDomainService<FieldRequirement> service, FieldRequirement entity)
        {
            var exist = service.GetAll().Any(x => x.RequirementId == entity.RequirementId);
            if (exist)
            {
                
            }

            return Success();
        }
    }
}

