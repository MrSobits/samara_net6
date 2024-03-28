namespace Bars.GisIntegration.Inspection.DataExtractors.Examination
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.Gkh.Quartz.Scheduler.Log;
    using Bars.GkhGji.Entities;
    using Base.Service;

    /// <summary>
    /// Экстрактор других документов проверки
    /// </summary>
    public class OtherProtocolDocumentExtractor : BaseDataExtractor<ExaminationOtherDocument, ProtocolAnnex>
    {
        private List<long> protocolIds;
        private Dictionary<long, Examination> examinationsByProtocolId;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            var examinations = parameters.GetAs<List<Examination>>("Examinations");
            var disposalIds = examinations.Select(x => x.ExternalSystemEntityId).ToList();

            var protocolDomain = this.Container.ResolveDomain<Protocol>();
            var disposalDomain = this.Container.ResolveDomain<Disposal>();

            try
            {

                var disposalByDisposalStages = disposalDomain.GetAll()
                    .Where(x => disposalIds.Contains(x.Id))
                    .ToDictionary(x => x.Stage);

                var disposalStages = disposalByDisposalStages.Keys.ToList();

                var protocolIdsByDisposal = protocolDomain.GetAll()
                    .Where(x => x.Stage != null && x.Stage.Parent != null && disposalStages.Contains(x.Stage.Parent))
                    .ToList()
                    .Select(
                        x => new
                        {
                            x.Stage.Position,
                            Disposal = disposalByDisposalStages.Get(x.Stage.Parent),
                            ProtocolId = x.Id
                        })
                    .ToList()
                    .GroupBy(x => x.Disposal)
                    .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Position).First().ProtocolId);

                this.protocolIds = protocolIdsByDisposal
                    .Values
                    .ToList();

                this.examinationsByProtocolId = examinations
                    .Select(
                        x => new
                        {
                            Examination = x,
                            ProtocolId = protocolIdsByDisposal.FirstOrDefault(y => y.Key.Id == x.ExternalSystemEntityId).Value
                        })
                    .GroupBy(x => x.ProtocolId)
                    .ToDictionary(x => x.Key, x => x.First().Examination);
            }
            finally
            {
                this.Container.Release(disposalDomain);
                this.Container.Release(protocolDomain);
            }
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<ProtocolAnnex> GetExternalEntities(DynamicDictionary parameters)
        {
            var protocolAnnexDomain = this.Container.ResolveDomain<ProtocolAnnex>();

            try
            {
                return protocolAnnexDomain.GetAll()
                    .Where(x => x.Protocol != null && x.File != null)
                    .Where(x => this.protocolIds.Contains(x.Protocol.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(protocolAnnexDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(ProtocolAnnex externalEntity, ExaminationOtherDocument risEntity)
        {
            var fileUploadService = this.Container.Resolve<IAttachmentService>();

            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.Examination = this.examinationsByProtocolId?.Get(externalEntity.Protocol.Id);
            try
            {
                risEntity.Attachment = fileUploadService.CreateAttachment(externalEntity.File, externalEntity.Description);
            }
            catch (FileNotFoundException)
            {
                risEntity.Attachment = null;
                this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл приложения протокола ГЖИ " + externalEntity.Description + " не найден"));
            }
            finally
            {
                this.Container.Release(fileUploadService);
            }
        }
    }
}
