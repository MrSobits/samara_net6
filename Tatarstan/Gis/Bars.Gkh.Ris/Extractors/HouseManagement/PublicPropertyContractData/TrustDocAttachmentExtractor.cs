namespace Bars.Gkh.Ris.Extractors.HouseManagement.PublicPropertyContractData
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Service;
    using Bars.GkhDi.Entities;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;
    using Quartz.Scheduler.Log;

    /// <summary>
    /// Экстрактор файлов протоколов
    /// </summary>
    public class TrustDocAttachmentExtractor : BaseDataExtractor<RisTrustDocAttachment, FileInfo>
    {
        private List<RisPublicPropertyContract> contracts;
        private Dictionary<long, RisPublicPropertyContract> contractsById;
        private Dictionary<long, InfoAboutUseCommonFacilities> contractByFileId;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.contracts = parameters.GetAs<List<RisPublicPropertyContract>>("selectedContracts");

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

            var infoAboutUseCommonFacilitiesDomain = this.Container.ResolveDomain<InfoAboutUseCommonFacilities>();

            try
            {
                var contractsWithFile = infoAboutUseCommonFacilitiesDomain.GetAll()
                    .Where(x => selectedContractIds.Contains(x.Id))
                    .Where(x => x.ProtocolFile != null)
                    .ToList();

                this.contractByFileId = contractsWithFile.GroupBy(x => x.ProtocolFile.Id)
                    .ToDictionary(x => x.Key, x => x.First());

                return contractsWithFile.Select(x => x.ProtocolFile).ToList();
            }
            finally
            {
                this.Container.Release(infoAboutUseCommonFacilitiesDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(FileInfo externalEntity, RisTrustDocAttachment risEntity)
        {
            var fileUploadService = this.Container.Resolve<IAttachmentService>();

            try
            {
                if (externalEntity != null)
                {
                    var contract = this.contractByFileId.Get(externalEntity.Id);
                    risEntity.PublicPropertyContract = this.contractsById?.Get(contract.Id);
                    risEntity.ExternalSystemEntityId = externalEntity.Id;
                    risEntity.ExternalSystemName = "gkh";
                    risEntity.Attachment = fileUploadService.CreateAttachment(
                        externalEntity,
                        externalEntity.Name);
                }
            }
            catch (FileNotFoundException)
            {
                risEntity.Attachment = null;
                this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл документа, подтверждающего полномочие заключать договор не найден"));
            }
            finally
            {
                this.Container.Release(fileUploadService);
            }
        }
    }
}
