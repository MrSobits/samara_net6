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
    /// Экстрактор мест проверок
    /// </summary>
    public class ExaminationPlaceExtractor : BaseDataExtractor<ExaminationPlace, InspectionGjiRealityObject>
    {
        private List<Examination> examinations;
        private List<long> inspectionIds;
        private Dictionary<long, Examination> examinationsByInspectionId;
        private int orderNumber;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.examinations = parameters.GetAs<List<Examination>>("Examinations");

            var disposalDomain = this.Container.ResolveDomain<Disposal>();

            try
            {
                var examinationsExternalIds = this.examinations.Select(y => y.ExternalSystemEntityId).ToList();

                var inspectionsByDisposalId = disposalDomain.GetAll()
                    .Where(x => examinationsExternalIds.Contains(x.Id))
                    .Select(
                        x => new
                        {
                            InspectionId = x.Inspection.Id,
                            DisposalId = x.Id
                        })
                    .ToList()
                    .GroupBy(x => x.DisposalId)
                    .ToDictionary(x => x.Key, x => x.First().InspectionId);

                this.inspectionIds = inspectionsByDisposalId.Values.ToList();

                this.examinationsByInspectionId =
                    this.examinations?
                    .Select(
                        x => new
                        {
                            InspectionId = inspectionsByDisposalId.Get(x.ExternalSystemEntityId),
                            Examination = x
                        })
                    .ToList()
                    .GroupBy(x => x.InspectionId)
                    .ToDictionary(x => x.Key, x => x.First().Examination);
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
        public override List<InspectionGjiRealityObject> GetExternalEntities(DynamicDictionary parameters)
        {
            var inspectionGjiRealityObjectDomain = this.Container.ResolveDomain<InspectionGjiRealityObject>();

            try
            {
                return inspectionGjiRealityObjectDomain.GetAll()
                    .Where(x => x.Inspection != null)
                    .Where(x => this.inspectionIds.Contains(x.Inspection.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(inspectionGjiRealityObjectDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(InspectionGjiRealityObject externalEntity, ExaminationPlace risEntity)
        {
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.FiasHouseGuid = externalEntity.RealityObject?.HouseGuid;
            risEntity.OrderNumber = ++this.orderNumber;
            risEntity.Examination = this.examinationsByInspectionId?.Get(externalEntity.Inspection.Id);
        }
    }
}
