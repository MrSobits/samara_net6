namespace Bars.Gkh.Ris.Extractors.Services
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Services;
    using Bars.GisIntegration.Base.Service;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Repair.Entities;
    using Quartz.Scheduler.Log;

    public class RisCompletedWorkExtractor : BaseDataExtractor<RisCompletedWork, PerformedRepairWorkAct>
    {
        public IAttachmentService AttachmentService { get; set; }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<PerformedRepairWorkAct> GetExternalEntities(DynamicDictionary parameters)
        {
            long[] selectedIds = {};

            var selectedContracts = parameters.GetAs("selectedList", string.Empty);

            if (selectedContracts.ToUpper() == "ALL")
            {
                selectedIds = null; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedIds = selectedContracts.ToLongArray();
            }

            var result = new List<PerformedRepairWorkAct>();

            var performedRepairWorkActDomain = this.Container.ResolveDomain<PerformedRepairWorkAct>();
            try
            {
                result = performedRepairWorkActDomain.GetAll()
                    .WhereIf(selectedIds != null, x => selectedIds.Contains(x.Id))
                    .Where(x => x.ActFile != null)
                    .Where(x => x.ObjectPhoto != null)
                    .ToList();

                return result;
            }
            finally
            {
                this.Container.Release(performedRepairWorkActDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(PerformedRepairWorkAct externalEntity, RisCompletedWork risEntity)
        {
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.ActFile = this.AttachmentService.CreateAttachment(
                externalEntity.ActFile,
                externalEntity.ActDescription);
            try
            {
                risEntity.ObjectPhoto = this.AttachmentService.CreateAttachment(
                    externalEntity.ObjectPhoto,
                    externalEntity.ObjectPhotoDescription);
            }
            catch (FileNotFoundException)
            {
                risEntity.ObjectPhoto = null;
                this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл фотографии " + externalEntity.ActDescription + " не найден"));
            }
            
            risEntity.ActDate = externalEntity.ActDate;
            risEntity.ActNumber = externalEntity.ActNumber;
        }
    }
}