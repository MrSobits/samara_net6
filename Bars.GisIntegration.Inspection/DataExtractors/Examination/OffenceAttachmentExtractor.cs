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
    using Base.Dictionaries;
    using Enums;

    /// <summary>
    /// Экстрактор приложений протоколов
    /// </summary>
    public class OffenceAttachmentExtractor : BaseDataExtractor<OffenceAttachment, ProtocolAnnex>
    {
        private Dictionary<long, Offence> offencesByProtocolId;
        private List<long> protocolIds;

        private IDictionary examinationResultDocTypeDic;

        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            var offences = parameters.GetAs<List<Offence>>("Offences");
            this.protocolIds = offences.Select(x => x.ExternalSystemEntityId).ToList();

            this.offencesByProtocolId = offences
                .GroupBy(x => x.ExternalSystemEntityId) //ProtocolId
                .ToDictionary(x => x.Key, x => x.First());
            this.examinationResultDocTypeDic = this.DictionaryManager.GetDictionary("ExaminationResultDocTypeDictionary");
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
                    .Where(x => x.Protocol!= null && x.File != null)
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
        protected override void UpdateRisEntity(ProtocolAnnex externalEntity, OffenceAttachment risEntity)
        {
            var fileUploadService = this.Container.Resolve<IAttachmentService>();
            var examinationResultDocTypeDicValue = this.examinationResultDocTypeDic.GetDictionaryRecord((long)ExaminationResultDocType.ActCheck);
            risEntity.Offence.Examination.ResultDocumentTypeCode = examinationResultDocTypeDicValue.GisCode;
            risEntity.Offence.Examination.ResultDocumentTypeGuid = examinationResultDocTypeDicValue.GisGuid;

            risEntity.ExternalSystemName = "gkh";
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.Offence = this.offencesByProtocolId?.Get(externalEntity.Protocol.Id);
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
