namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class ActCheckRealityObjectInterceptor : EmptyDomainInterceptor<ActCheckRealityObject>
    {
        public IDomainService<ActCheckRealityObjectDescription> DescriptionService { get; set; } 

        public override IDataResult BeforeDeleteAction(IDomainService<ActCheckRealityObject> service, ActCheckRealityObject entity)
        {
            var description = this.DescriptionService.GetAll().FirstOrDefault(x => x.ActCheckRealityObject.Id == entity.Id);
            if (description != null)
            {
                this.DescriptionService.Delete(description.Id);
            }

            return this.Success();
        }
    }
}