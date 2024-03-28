namespace Bars.GisIntegration.Inspection.DataExtractors.Examination
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Экстрактор предписаний
    /// </summary>
    public class PreceptExtractor : BaseDataExtractor<Precept, Prescription>
    {
        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        private Dictionary<long, string> orgRootEntityGuidByGkhId;
        private List<InspectionGjiStage> disposalStages;
        private Dictionary<InspectionGjiStage, Disposal> disposalByDisposalStages;
        private Dictionary<long, Examination> examinationsByPrescriptionId;
        private Dictionary<long, List<PrescriptionCloseDoc>> closeDocsByPrescriptionId;
        private List<Examination> examinations;
        private Dictionary<long, List<PrescriptionViol>> violationsByPrescriptionId;

        /// <summary>
        /// Cправочник "Причина отмены документа"
        /// </summary>
        private IDictionary prescriptionCloseReasonDictionary;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.examinations = parameters.GetAs<List<Examination>>("Examinations");
            var disposalIds = this.examinations.Select(x => x.ExternalSystemEntityId).ToList();

            var disposalDomain = this.Container.ResolveDomain<Disposal>();
            try
            {
                this.disposalByDisposalStages = disposalDomain.GetAll()
                    .Where(x => disposalIds.Contains(x.Id))
                    .ToDictionary(x => x.Stage);

                this.disposalStages = this.disposalByDisposalStages.Keys.ToList();
            }
            finally 
            {
                this.Container.Release(disposalDomain);
            }

            this.prescriptionCloseReasonDictionary = this.DictionaryManager.GetDictionary("PrescriptionCloseReasonDictionary");
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<Prescription> GetExternalEntities(DynamicDictionary parameters)
        {
            var prescriptionDomain = this.Container.ResolveDomain<Prescription>();
            var prescriptionViolDomain = this.Container.ResolveDomain<PrescriptionViol>();
            var prescriptionCloseDocDomain = this.Container.ResolveDomain<PrescriptionCloseDoc>();

            try
            {
                var prescriptionIdsByDisposal = prescriptionDomain.GetAll()
                    .Where(x => x.Stage != null && x.Stage.Parent != null && this.disposalStages.Contains(x.Stage.Parent))
                    .ToList()
                    .Select(
                        x => new
                        {
                            x.Stage.Position,
                            Disposal = this.disposalByDisposalStages.Get(x.Stage.Parent),
                            PrescriptionId = x.Id
                        })
                    .ToList()
                    .GroupBy(x => x.Disposal)
                    .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Position).First().PrescriptionId);

                var prescriptionIds = prescriptionIdsByDisposal
                    .Values
                    .ToList();

                this.examinationsByPrescriptionId = this.examinations
                    .Select(
                        x => new
                        {
                            Examination = x,
                            PrescriptionId = prescriptionIdsByDisposal.FirstOrDefault(y => y.Key.Id == x.ExternalSystemEntityId).Value
                        })
                    .GroupBy(x => x.PrescriptionId)
                    .ToDictionary(x => x.Key, x => x.First().Examination);

                this.violationsByPrescriptionId = prescriptionViolDomain.GetAll()
                    .Where(x => x.Document != null && x.InspectionViolation != null)
                    .Where(x => prescriptionIds.Contains(x.Document.Id))
                    .Select(
                        x => new
                        {
                            Violation = x,
                            PrescriptionId = x.Document.Id
                        })
                    .ToList()
                    .GroupBy(x => x.PrescriptionId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Violation).ToList());

                this.closeDocsByPrescriptionId = prescriptionCloseDocDomain.GetAll()
                   .Where(x => x.Prescription != null)
                   .Where(x => prescriptionIds.Contains(x.Prescription.Id))
                   .Select(
                       x => new
                       {
                           CloseDoc = x,
                           PrescriptionId = x.Prescription.Id
                       })
                   .ToList()
                   .GroupBy(x => x.PrescriptionId)
                   .ToDictionary(x => x.Key, x => x.Select(y => y.CloseDoc).ToList());

                this.orgRootEntityGuidByGkhId = this.examinations
                    .Where(x => x.GisContragent != null)
                    .Select(
                    x => new
                    {
                        x.GisContragent.GkhId,
                        x.GisContragent.OrgRootEntityGuid
                    })
                    .ToList()
                    .GroupBy(x => x.GkhId)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault()?.OrgRootEntityGuid);

                return prescriptionDomain.GetAll()
                    .Where(x => prescriptionIds.Contains(x.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(prescriptionDomain);
                this.Container.Release(prescriptionViolDomain);
                this.Container.Release(prescriptionCloseDocDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="prescription">Сущность внешней системы</param>
        /// <param name="precept">Ris сущность</param>
        protected override void UpdateRisEntity(Prescription prescription, Precept precept)
        {
            var violations = this.violationsByPrescriptionId?.Get(prescription.Id);
            var deadline = violations?.OrderBy(x => x.DatePlanRemoval).FirstOrDefault()?.DatePlanRemoval;
            var roAddresses = violations?.Select(x => x.InspectionViolation.RealityObject?.HouseGuid).ToList();
            var closeDocs = this.closeDocsByPrescriptionId?.Get(prescription.Id);
            var cancelDate = closeDocs?.OrderByDescending(x => x.Date).FirstOrDefault()?.Date;
            var isFulfiledPrecept = violations?.All(x => x.DateFactRemoval.HasValue);
            var closeReason = this.prescriptionCloseReasonDictionary?.GetDictionaryRecord((long) prescription.CloseReason);

            precept.ExternalSystemEntityId = prescription.Id;
            precept.ExternalSystemName = "gkh";
            precept.Examination = this.examinationsByPrescriptionId?.Get(prescription.Id);
            precept.Number = prescription.DocumentNumber;
            precept.Date = prescription.DocumentDate;
            precept.Deadline = deadline;
            precept.FiasHouseGuid = roAddresses?.FirstOrDefault(); // у всех нарушений предписания один адрес
            precept.IsFulfiledPrecept = isFulfiledPrecept;
            precept.CancelReason = closeReason?.GisCode;
            precept.CancelReasonGuid = closeReason?.GisGuid;
            precept.CancelDate = cancelDate;
            if (prescription.Contragent != null)
            {
                precept.OrgRootEntityGuid = this.orgRootEntityGuidByGkhId?.Get(prescription.Contragent.Id);
            }
            precept.IsCancelled = (prescription.Closed == YesNoNotSet.Yes);
            precept.IsCancelledAndIsFulfiled = prescription.CloseReason == PrescriptionCloseReason.Done;
        }
    }
}
