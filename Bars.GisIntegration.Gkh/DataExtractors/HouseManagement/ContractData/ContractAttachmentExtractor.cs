namespace Bars.GisIntegration.Gkh.DataExtractors.HouseManagement.ContractData
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Service;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Quartz.Scheduler.Log;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Экстрактор для приложения ДУ
    /// </summary>
    public class ContractAttachmentExtractor : BaseDataExtractor<RisContractAttachment, FileInfo>
    {
        private List<RisContract> contracts;
        private Dictionary<long, RisContract> contractsById;
        private List<Charter> charters;
        private Dictionary<long, Charter> chartersById;
        private Dictionary<long, ManOrgBaseContract> contractByFileId;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.contracts = parameters.GetAs<List<RisContract>>("selectedContracts");

            this.contractsById = this.contracts?
                .GroupBy(x => x.ExternalSystemEntityId)
                .ToDictionary(x => x.Key, x => x.First());

            this.charters = parameters.GetAs<List<Charter>>("selectedCharters");

            this.chartersById = this.charters?
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
                ?? this.charters?.Select(x => x.ExternalSystemEntityId).ToArray()
                ?? new long[] {};

            var manOrgBaseContractDomain = this.Container.ResolveDomain<ManOrgBaseContract>();

            try
            {
                var contractsWithFile = manOrgBaseContractDomain.GetAll()
                    .Where(x => selectedContractIds.Contains(x.Id))
                    .Where(x => x.FileInfo != null)
                    .ToList();
                
                this.contractByFileId = contractsWithFile.GroupBy(x => x.FileInfo.Id)
                    .ToDictionary(x => x.Key, x => x.First());

                return contractsWithFile.Select(x => x.FileInfo).ToList();
            }
            finally
            {
                this.Container.Release(manOrgBaseContractDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(FileInfo externalEntity, RisContractAttachment risEntity)
        {
            var fileUploadService = this.Container.Resolve<IAttachmentService>();
            
            try
            {
                if (externalEntity != null)
                {
                    var contract = this.contractByFileId.Get(externalEntity.Id);

                    risEntity.Contract = this.contractsById?.Get(contract.Id);
                    risEntity.Charter = this.chartersById?.Get(contract.Id);
                    risEntity.ExternalSystemEntityId = externalEntity.Id;
                    risEntity.ExternalSystemName = "gkh";
                    try
                    {
                        risEntity.Attachment = fileUploadService.CreateAttachment(
                            externalEntity,
                            contract.Note);
                    }
                    catch (FileNotFoundException)
                    {
                        risEntity.Attachment = null;
                        this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл приложения договора управления номер " + contract.DocumentNumber + " не найден"));
                    }
                }
            }
            finally
            {
                this.Container.Release(fileUploadService);
            }
        }
    }
}