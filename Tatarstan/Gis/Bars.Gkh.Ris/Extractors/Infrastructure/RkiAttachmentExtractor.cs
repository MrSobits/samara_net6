namespace Bars.Gkh.Ris.Extractors.Infrastructure
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.External.Housing.OKI;
    using Bars.GisIntegration.Base.Entities.Infrastructure;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Service;
    using Bars.Gkh.Domain;
    using Quartz.Scheduler.Log;

    /// <summary>
    /// Экстрактор приложений ОКИ
    /// </summary>
    public class RkiAttachmentExtractor : BaseDataExtractor<RisRkiAttachment, OkiDoc>
    {
        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<OkiDoc> GetExternalEntities(DynamicDictionary parameters)
        {
            var okiDocRepository = this.Container.ResolveDomain<OkiDoc>();

            long[] selectedRecordsIds = { };

            var selectedRecords = parameters.GetAs("selectedRecords", string.Empty);
            if (selectedRecords.ToUpper() == "ALL")
            {
                selectedRecordsIds = new long[0]; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedRecordsIds = selectedRecords.ToLongArray();
            }

            try
            {
                var res = okiDocRepository
                    .GetAll()
                    .Where(x => x.DataSupplier != null && x.DataSupplier.Ogrn == this.Contragent.Ogrn)
                    .Where(x => x.OkiObject != null && x.OkiDocType.Id == 1) //Основание управления объектом
                    .WhereIf(selectedRecordsIds.Any(), x => selectedRecordsIds.Contains(x.OkiObject.Id))
                    .ToList();
                return res;
            }
            finally
            {
                this.Container.Release(okiDocRepository);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(OkiDoc externalEntity, RisRkiAttachment risEntity)
        {
            var rkiRepository = this.Container.ResolveDomain<RisRkiItem>();
            var fileUploadService = this.Container.Resolve<IAttachmentService>();
            var file = externalEntity.Attachment.FileInfo;

            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "ris";
            risEntity.Contragent = this.Contragent;
            risEntity.Operation = string.IsNullOrEmpty(risEntity.Guid)
                ? RisEntityOperation.Create
                : RisEntityOperation.Update;
            risEntity.RkiItem = rkiRepository
                .GetAll()
                .Where(x => x.ExternalSystemName == "ris")
                .FirstOrDefault(x => x.ExternalSystemEntityId == externalEntity.OkiObject.Id);
            try
            {
                if (file != null)
                {
                    risEntity.Attachment = fileUploadService.CreateAttachment(
                        file,
                        externalEntity.Attachment.Description);
                }
            }
            catch (FileNotFoundException)
            {
                risEntity.Attachment = null;
                this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл приложения ОКИ " + file?.FullName + " не найден"));
            }
            finally
            {
                this.Container.Release(rkiRepository);
                this.Container.Release(fileUploadService);
            }
        }
    }
}