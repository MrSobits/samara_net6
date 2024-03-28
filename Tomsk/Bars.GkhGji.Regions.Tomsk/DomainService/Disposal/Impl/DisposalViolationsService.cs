namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DisposalViolationsService : IDisposalViolationsService
    {
        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public ActionResult GetListNotRemovedViolations(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");

            if (documentId == 0)
            {
                throw new Exception("Нет приказа");
            }

            var data = PrescriptionViolDomain.GetAll()
                                      .Where(
                                          x => ChildrenDomain.GetAll()
                                                        .Any(
                                                            y =>
                                                            y.Children.Id == documentId
                                                            && y.Parent.TypeDocumentGji == TypeDocumentGji.Prescription
                                                            && x.Document.Id == y.Parent.Id))
                                                            .Select(x => new
                                                            {
                                                                x.InspectionViolation.Id,
                                                                MunicipalityName = x.InspectionViolation.RealityObject.Municipality.Name,
                                                                x.InspectionViolation.RealityObject.Address,
                                                                x.InspectionViolation.Violation.Name,
                                                                x.Description
                                                            })
                                                            .ToList();

            /* тут получали наршуния котоыре не устранены
            var data = PrescriptionViolDomain.GetAll()
                                      .Where(x => !x.DateFactRemoval.HasValue || (x.DateFactRemoval.HasValue && x.DateFactRemoval.Value != DateTime.MinValue))
                                      .Where(
                                          x => ChildrenDomain.GetAll()
                                                        .Any(
                                                            y =>
                                                            y.Children.Id == documentId
                                                            && y.Parent.TypeDocumentGji == TypeDocumentGji.Prescription
                                                            && x.Document.Id == y.Parent.Id))
                                                            .Select(x => new
                                                                            {
                                                                                x.InspectionViolation.Id,
                                                                                MunicipalityName = x.InspectionViolation.RealityObject.Municipality.Name,
                                                                                x.InspectionViolation.RealityObject.Address,
                                                                                x.InspectionViolation.DatePlanRemoval,
                                                                                x.InspectionViolation.Violation.Name,
                                                                                x.Description
                                                                            })
                                                                            .ToList();
             */

            return new JsonListResult(data, data.Count);
        }
    }
}
