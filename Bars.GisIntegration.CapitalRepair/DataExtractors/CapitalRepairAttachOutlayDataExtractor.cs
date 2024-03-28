namespace Bars.GisIntegration.CapitalRepair.DataExtractors
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GisIntegration.Base.Service;
    using Bars.GkhCr.Entities;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;
    using Gkh.Quartz.Scheduler.Log;

    /// <summary>
    /// Экстрактор данных файлов сметной документации
    /// </summary>
    public class CapitalRepairAttachOutlayDataExtractor : BaseDataExtractor<RisCrAttachOutlay, EstimateCalculation>
    {
        private Dictionary<EstimateCalculation, RisCrContract> estimateCalculation;

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<EstimateCalculation> GetExternalEntities(DynamicDictionary parameters)
        {
            var risCrContracts = parameters.GetAs<IEnumerable<RisCrContract>>("risCrContracts");
            var buildContractTypeWorkDomain = this.Container.ResolveDomain<BuildContractTypeWork>();
            var estimateCalculations = this.Container.ResolveDomain<EstimateCalculation>();

            try
            {
                this.estimateCalculation =
                    (from risContract in risCrContracts
                        join gkhContract in buildContractTypeWorkDomain.GetAll()
                            on risContract.ExternalSystemEntityId equals gkhContract.Id
                        join calculation in
                            estimateCalculations.GetAll()
                            //выбираем те, у которых есть файлы
                                .Where(x => x.EstimateFile != null || x.FileEstimateFile != null || x.ResourceStatmentFile != null)
                            on gkhContract.TypeWork equals calculation.TypeWorkCr
                        select new {risContract, calculation}).ToDictionary(x => x.calculation, x => x.risContract);

                return this.estimateCalculation.Keys.ToList();
            }
            finally
            {
                this.Container.Release(estimateCalculations);
                this.Container.Release(buildContractTypeWorkDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(EstimateCalculation externalEntity, RisCrAttachOutlay risEntity)
        {
            var fileUploadService = this.Container.Resolve<IAttachmentService>();

            //берем файл и описание из того, что есть среди документов 
            var file = externalEntity.EstimateFile != null
                ? new KeyValuePair<FileInfo, string>(externalEntity.EstimateFile, externalEntity.EstimateDocumentName)
                : externalEntity.FileEstimateFile != null
                    ? new KeyValuePair<FileInfo, string>(externalEntity.FileEstimateFile, externalEntity.FileEstimateDocumentName)
                    : new KeyValuePair<FileInfo, string>(externalEntity.ResourceStatmentFile, externalEntity.ResourceStatmentDocumentName);
            
                risEntity.ExternalSystemName = "gkh";
                risEntity.ExternalSystemEntityId = externalEntity.Id;
            try
            {
                risEntity.Attachment = fileUploadService.CreateAttachment(
                    file.Key,
                    file.Value);

                risEntity.Contract = this.estimateCalculation[externalEntity];
            }
            catch (FileNotFoundException)
            {
                risEntity.Attachment = null;
                this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл документа ведомости ресурсов " + file.Value + " не найден"));
            }
            finally
            {
                this.Container.Release(fileUploadService);
            }
        }
    }
}