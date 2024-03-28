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
    /// Экстрактор других документов проверки
    /// </summary>
    public class OtherActDocumentExtractor : BaseDataExtractor<ExaminationOtherDocument, ActCheckAnnex>
    {
        private List<long> actChecksIds;
        private Dictionary<long, Examination> examinationsByActCheckId;

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
            var examinations = parameters.GetAs<List<Examination>>("Examinations");
            var disposalIds = examinations.Select(x => x.ExternalSystemEntityId).ToList();

            var actCheckDomain = this.Container.ResolveDomain<ActCheck>();
            var disposalDomain = this.Container.ResolveDomain<Disposal>();

            try
            {
                var disposalByDisposalStages = disposalDomain.GetAll()
                    .Where(x => disposalIds.Contains(x.Id))
                    .ToDictionary(x => x.Stage);

                var disposalStages = disposalByDisposalStages.Keys.ToList();

                var actCheckIdsByDisposal = actCheckDomain.GetAll()
                    .Where(x => x.Stage != null && x.Stage.Parent != null && disposalStages.Contains(x.Stage.Parent))
                    .ToList()
                    .Select(
                        x => new
                        {
                            x.Stage.Position,
                            Disposal = disposalByDisposalStages.Get(x.Stage.Parent),
                            ActId = x.Id
                        })
                    .ToList()
                    .GroupBy(x => x.Disposal)
                    .ToDictionary(x => x.Key, x => x.OrderBy(y => y.Position).First().ActId);
                    
                this.actChecksIds = actCheckIdsByDisposal
                    .Values
                    .ToList();

                this.examinationsByActCheckId = examinations
                    .Select(
                        x => new
                        {
                            Examination = x,
                            ActCheckId = actCheckIdsByDisposal.FirstOrDefault(y => y.Key.Id == x.ExternalSystemEntityId).Value
                        })
                    .GroupBy(x => x.ActCheckId)
                    .ToDictionary(x => x.Key, x => x.First().Examination);

                this.examinationResultDocTypeDic = this.DictionaryManager.GetDictionary("ExaminationResultDocTypeDictionary");
            }
            finally
            {
                this.Container.Release(disposalDomain);
                this.Container.Release(actCheckDomain);
            }
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<ActCheckAnnex> GetExternalEntities(DynamicDictionary parameters)
        {
            var actCheckAnnexDomain = this.Container.ResolveDomain<ActCheckAnnex>();

            try
            {
                return actCheckAnnexDomain.GetAll()
                    .Where(x => x.ActCheck != null && x.File != null)
                    .Where(x => this.actChecksIds.Contains(x.ActCheck.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(actCheckAnnexDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(ActCheckAnnex externalEntity, ExaminationOtherDocument risEntity)
        {
            var fileUploadService = this.Container.Resolve<IAttachmentService>();
            var examinationResultDocTypeDicValue = this.examinationResultDocTypeDic.GetDictionaryRecord((long) ExaminationResultDocType.ActCheck);
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.Examination = this.examinationsByActCheckId?.Get(externalEntity.ActCheck.Id);
            risEntity.Examination.ResultDocumentTypeCode = examinationResultDocTypeDicValue.GisCode;
            risEntity.Examination.ResultDocumentTypeGuid = examinationResultDocTypeDicValue.GisGuid;

            try
            {
                risEntity.Attachment = fileUploadService.CreateAttachment(externalEntity.File, externalEntity.Description);
            }
            catch (FileNotFoundException)
            {
                risEntity.Attachment = null;
                this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл приложения акта ГЖИ " + externalEntity.Description + " не найден"));
            }
            finally
            {
                this.Container.Release(fileUploadService);
            }
        }
    }
}
