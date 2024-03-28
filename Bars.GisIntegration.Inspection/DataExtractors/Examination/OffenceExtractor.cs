namespace Bars.GisIntegration.Inspection.DataExtractors.Examination
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Экстрактор протоколов
    /// </summary>
    public class OffenceExtractor : BaseDataExtractor<Offence, Protocol>
    {
        private List<Examination> examinations;
        private List<InspectionGjiStage> disposalStages;
        private Dictionary<InspectionGjiStage, Disposal> disposalByDisposalStages;
        private Dictionary<long, List<ProtocolViolation>> violationsByProtocolId;
        private Dictionary<long, Examination> examinationsByProtocolId;

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
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<Protocol> GetExternalEntities(DynamicDictionary parameters)
        {
            var protocolDomain = this.Container.ResolveDomain<Protocol>();
            var protocolViolationDomain = this.Container.ResolveDomain<ProtocolViolation>();

            try
            {
                var protocolIdsByDisposal = protocolDomain.GetAll()
                        .Where(x => x.Stage != null && x.Stage.Parent != null && this.disposalStages.Contains(x.Stage.Parent))
                        .ToList()
                        .Select(
                            x => new
                            {
                                x.Stage.Position,
                                Disposal = this.disposalByDisposalStages.Get(x.Stage.Parent),
                                ProtocolId = x.Id
                            })
                        .ToList()
                        .GroupBy(x => x.Disposal)
                        .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Position).First().ProtocolId);

                this.examinationsByProtocolId = this.examinations
                    .Select(
                        x => new
                        {
                            Examination = x,
                            ProtocolId = protocolIdsByDisposal.FirstOrDefault(y => y.Key.Id == x.ExternalSystemEntityId).Value
                        })
                    .GroupBy(x => x.ProtocolId)
                    .ToDictionary(x => x.Key, x => x.First().Examination);

                var protocolIds = protocolIdsByDisposal
                    .Values
                    .ToList();

                this.violationsByProtocolId = protocolViolationDomain.GetAll()
                    .Where(x => x.Document != null && x.InspectionViolation != null)
                    .Where(x => protocolIds.Contains(x.Document.Id))
                    .Select(
                        x => new
                        {
                            Violation = x,
                            ProtocolId = x.Document.Id
                        })
                    .ToList()
                    .GroupBy(x => x.ProtocolId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Violation).ToList());

                return protocolDomain.GetAll()
                    .Where(x => protocolIds.Contains(x.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(protocolDomain);
                this.Container.Release(protocolViolationDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="protocol">Сущность внешней системы</param>
        /// <param name="offence">Ris сущность</param>
        protected override void UpdateRisEntity(Protocol protocol, Offence offence)
        {
            var violations = this.violationsByProtocolId?.Get(protocol.Id);
            var isFulfiled = violations?.All(x => x.DateFactRemoval.HasValue);

            offence.ExternalSystemEntityId = protocol.Id;
            offence.ExternalSystemName = "gkh";
            offence.Examination = this.examinationsByProtocolId?.Get(protocol.Id);
            offence.Number = protocol.DocumentNumber;
            offence.Date = protocol.DocumentDate;
            offence.IsFulfiledOffence = isFulfiled;
        }
    }
}
