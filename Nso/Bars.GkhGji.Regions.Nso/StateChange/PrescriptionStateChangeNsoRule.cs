

namespace Bars.GkhGji.Regions.Nso.StateChange
{
    using Gkh.Utils;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Castle.Windsor;

    public class PrescriptionStateChangeNsoRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }

        public IDomainService<Prescription> PrescriptionDomain { get; set; }

        public string Id { get { return "gji_nso_prescription_state_change_rule"; } }

        public string Name { get { return "НСО - Проверка на наличие предписания в конечном статусе с такими нарушениями"; } }

        public string TypeId { get { return "gji_document_prescr"; } }

        public string Description { get { return "НСО - Проверка на наличие предписания в конечном статусе с такими нарушениями"; } }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var result = new ValidateResult();
            result.Success = true;

            var prescription = statefulEntity as Prescription;

            if (prescription != null)
            {
                var prescriptionViolQuery = PrescriptionViolDomain.GetAll()
                    .Where(x => x.Document.Id == prescription.Id);

                var anyPrescriptionWithSameViols = PrescriptionViolDomain.GetAll()
                    .Where(x => x.Document.Inspection.Id == prescription.Inspection.Id && x.Document.Id != prescription.Id)
                    .Where(x => prescriptionViolQuery.Any(y => y.InspectionViolation.Id == x.InspectionViolation.Id))
                    .Select(x => new
                    {
                        x.InspectionViolation,
                        documentId = x.Document.Id
                    })
                    .ToList();

                var anyPrescriptionVithSameViolsAndState = anyPrescriptionWithSameViols
                    .Where(x => PrescriptionDomain.GetAll().Any(y => y.Id == x.documentId && y.State == newState))
                    .Select(x => x.InspectionViolation)
                    .ToList();

                if (anyPrescriptionVithSameViolsAndState.Count > 0)
                {
                    result.Success = false;
                    result.Message = "Найдено Предписание № {0} от {1}, в котором выбраны пересекающиеся с текущим нарушения: {2}."
                        .FormatUsing(prescription.DocumentNumber, 
                        prescription.DocumentDate.ToDateString(),
                        anyPrescriptionVithSameViolsAndState.Select(x => x.Violation.NormativeDocNames).AggregateWithSeparator(", "));
                }
            }

            return result;
        }
    }
}