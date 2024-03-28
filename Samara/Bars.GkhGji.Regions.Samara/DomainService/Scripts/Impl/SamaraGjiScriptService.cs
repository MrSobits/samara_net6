namespace Bars.GkhGji.Regions.Samara.DomainService.Scripts.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class SamaraGjiScriptService : GjiScriptService
    {
        public IDomainService<Disposal> DisposalDomain { get; set; }
        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }
        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }


        protected override List<DisposalForReminder> FilterDisposalsForSave(List<DisposalForReminder> disposalsForSave)
        {
            // (1) Распоряжения, сконвертированные с б3, которые не нужно выводить в доску 
            var disposalsToExclude = this.DisposalDomain.GetAll()
                .Where(x => x.ExternalId != null)
                .Select(x => x.Id)
                .ToList();

            // (2) Но не исключать те распоряжения на проверку предписания, если хотя бы у одного нарушения в предписании, которые были сконвертированы с b3 "Срок устранения" >= 01.01.2014.
            var disposalsNotToExclude = this.DocumentGjiChildrenDomain.GetAll()
               .Join(
                   this.PrescriptionViolDomain.GetAll(),
                   x => x.Parent.Id,
                   y => y.Document.Id,
                   (c, b) => new { disposal = c.Children, prescription = c.Parent, prescriptionViol = b })
               .Where(x => x.prescription.TypeDocumentGji == TypeDocumentGji.Prescription)
               .Where(x => x.disposal.TypeDocumentGji == TypeDocumentGji.Disposal)
               .Where(x => x.disposal.ExternalId != null)
               .Where(x => x.prescription.ExternalId != null)
               .Select(x => new { disposalId = x.disposal.Id, x.prescriptionViol.DatePlanRemoval })
               .AsEnumerable()
               .GroupBy(x => x.disposalId)
               .Where(x => x.Any(y => y.DatePlanRemoval >= new DateTime(2014, 1, 1)))
               .Select(x => x.Key)
               .ToList();

            disposalsToExclude = disposalsToExclude.Except(disposalsNotToExclude).ToList();

            var result = disposalsForSave.Where(x => !disposalsToExclude.Contains(x.Id)).ToList();
            
            return result;
        }
    }
}