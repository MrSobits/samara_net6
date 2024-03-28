namespace Bars.Gkh.Ris.Extractors.HouseManagement.SupplyResourceContract
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Service;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;
    using Quartz.Scheduler.Log;

    /// <summary>
    /// Экстрактор файлов договоров с поставщиками ресурсов
    /// </summary>
    public class SupResContractAttachmentExtractor : BaseDataExtractor<SupResContractAttachment, FileInfo>
    {
        private List<SupplyResourceContract> contracts;
        private Dictionary<long, SupplyResourceContract> contractsById;
        private Dictionary<long, PublicServiceOrgContract> contractByFileId;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.contracts = parameters.GetAs<List<SupplyResourceContract>>("selectedContracts");

            this.contractsById = this.contracts?
                .GroupBy(x => x.ExternalSystemEntityId)
                .ToDictionary(x => x.Key, x => x.First());

        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<FileInfo> GetExternalEntities(DynamicDictionary parameters)
        {
            var selectedContractIds = this.contracts?.Select(x => x.ExternalSystemEntityId).ToArray()
                ?? new long[] { };

            var realObjPublicServiceOrgDomain = this.Container.ResolveDomain<PublicServiceOrgContract>();

            try
            {
                var contractsWithFile = realObjPublicServiceOrgDomain.GetAll()
                    .Where(x => selectedContractIds.Contains(x.Id))
                    .Where(x => x.FileInfo != null)
                    .ToList();

                this.contractByFileId = contractsWithFile.GroupBy(x => x.FileInfo.Id)
                    .ToDictionary(x => x.Key, x => x.First());

                return contractsWithFile.Select(x => x.FileInfo).ToList();
            }
            finally
            {
                this.Container.Release(realObjPublicServiceOrgDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(FileInfo externalEntity, SupResContractAttachment risEntity)
        {
            var fileUploadService = this.Container.Resolve<IAttachmentService>();

            try
            {
                if (externalEntity != null)
                {
                    var contract = this.contractByFileId.Get(externalEntity.Id);
                    risEntity.Contract = this.contractsById?.Get(contract.Id);
                    risEntity.ExternalSystemEntityId = externalEntity.Id;
                    risEntity.ExternalSystemName = "gkh";
                    risEntity.Attachment = fileUploadService.CreateAttachment(
                        externalEntity,
                        contract.Note);
                }
            }
            catch (FileNotFoundException)
            {
                risEntity.Attachment = null;
                this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл договора с поставщиками ресурсов не найден"));
            }
            finally
            {
                this.Container.Release(fileUploadService);
            }
        }
    }
}
