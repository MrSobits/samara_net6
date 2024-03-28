namespace Bars.GisIntegration.Inspection.DataExtractors.Examination
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Service;
    using Bars.Gkh.Quartz.Scheduler.Log;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Экстрактор приложений предписаний
    /// </summary>
    public class PreceptAttachmentExtractor : BaseDataExtractor<PreceptAttachment, PrescriptionAnnex>
    {
        private Dictionary<long, Precept> preceptsByPrescriptionId;
        private List<long> prescriptionIds;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            var precepts = parameters.GetAs<List<Precept>>("Precepts");
            this.prescriptionIds = precepts.Select(x => x.ExternalSystemEntityId).ToList();

            this.preceptsByPrescriptionId = precepts
                .GroupBy(x => x.ExternalSystemEntityId) //PrescriptionId
                .ToDictionary(x => x.Key, x => x.First());
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<PrescriptionAnnex> GetExternalEntities(DynamicDictionary parameters)
        {
            var prescriptionAnnexDomain = this.Container.ResolveDomain<PrescriptionAnnex>();

            try
            {
                return prescriptionAnnexDomain.GetAll()
                    .Where(x => x.Prescription != null && x.File != null)
                    .Where(x => this.prescriptionIds.Contains(x.Prescription.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(prescriptionAnnexDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(PrescriptionAnnex externalEntity, PreceptAttachment risEntity)
        {
            var fileUploadService = this.Container.Resolve<IAttachmentService>();
            risEntity.ExternalSystemName = "gkh";
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.Precept = this.preceptsByPrescriptionId?.Get(externalEntity.Prescription.Id);
            try
            {
                risEntity.Attachment = fileUploadService.CreateAttachment(externalEntity.File, externalEntity.Description);
            }

            catch (FileNotFoundException)
            {
                risEntity.Attachment = null;
                this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл приложения предписания ГЖИ " + externalEntity.Description + " не найден"));
            }
            finally
            {
                this.Container.Release(fileUploadService);
            }
        }
    }
}
