namespace Bars.GkhGji.Regions.Smolensk.Interceptors.Prescription
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Smolensk.Entities;

    public class PrescriptionViolInterceptor : EmptyDomainInterceptor<PrescriptionViol>
    {
        public IDomainService<PrescriptionViolDescription> DescriptionService { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<PrescriptionViol> service, PrescriptionViol entity)
        {
            var description = this.DescriptionService.GetAll().FirstOrDefault(x => x.PrescriptionViol.Id == entity.Id);
            if (description != null)
            {
                this.DescriptionService.Delete(description.Id);
            }

            return this.Success();
        }
    }
}