﻿namespace Bars.GkhGji.Regions.Nso.Interceptors.Prescription
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nso.Entities;

    public class PrescriptionViolInterceptor : EmptyDomainInterceptor<PrescriptionViol>
    {
        public IDomainService<PrescriptionCancelViolReference> PrescriptionCancelViolRefDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocGjiChildrenDomain { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        private bool IsLastViol { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<PrescriptionViol> service, PrescriptionViol entity)
        {
            IsLastViol = !service.GetAll().Any(x => x.Document.Id == entity.Document.Id && x.Id != entity.Id);

            if (IsLastViol)
            {
                if (DocGjiChildrenDomain.GetAll().Any(x => x.Parent.Id == entity.Document.Id))
                {
                    return Failure("Невозможно удалить предписание, так как имеются дочерние документы");
                }
            }

            var prescriptionCancelViolRef = this.PrescriptionCancelViolRefDomain.GetAll().FirstOrDefault(x => x.InspectionViol.Id == entity.Id);

            if (prescriptionCancelViolRef != null)
            {
                this.PrescriptionCancelViolRefDomain.Delete(prescriptionCancelViolRef.Id);
            }

            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<PrescriptionViol> service, PrescriptionViol entity)
        {
            if (IsLastViol)
            {
                PrescriptionDomain.Delete(entity.Document.Id);
            }

            return Success();
        }
    }
}