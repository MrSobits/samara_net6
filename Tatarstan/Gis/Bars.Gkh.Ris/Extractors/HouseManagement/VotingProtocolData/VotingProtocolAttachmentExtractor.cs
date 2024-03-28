namespace Bars.Gkh.Ris.Extractors.HouseManagement.VotingProtocolData
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Service;
    using Bars.Gkh.Overhaul.Tat.Entities;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;
    using Quartz.Scheduler.Log;

    /// <summary>
    /// Экстрактор файлов протокола общего собрания собственников
    /// </summary>
    public class VotingProtocolAttachmentExtractor : BaseDataExtractor<RisVotingProtocolAttachment, FileInfo>
    {
        private List<RisVotingProtocol> protocols;
        private Dictionary<long, RisVotingProtocol> protocolsById;
        private Dictionary<long, long> protocolGkhIdByFileId; 
        private Dictionary<long, string> descriptionsByProtocolIds;
        private List<long> selectedProtocolIds = new List<long>();

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.protocols = parameters.GetAs<List<RisVotingProtocol>>("selectedProtocols");

            this.protocolsById = this.protocols?
                .GroupBy(x => x.ExternalSystemEntityId)
                .ToDictionary(x => x.Key, x => x.First());

            this.selectedProtocolIds = this.protocols?.Select(x => x.ExternalSystemEntityId).ToList();

            var propertyOwnerProtocolsDomain = this.Container.ResolveDomain<PropertyOwnerProtocols>();

            try
            {
                var selectedProtocols = propertyOwnerProtocolsDomain.GetAll()
                    .Where(x => this.selectedProtocolIds.Contains(x.Id));

                this.protocolGkhIdByFileId =
                    selectedProtocols.Select(x => new
                    {
                        x.DocumentFile,
                        x.Id
                    })
                    .ToList()
                    .GroupBy(x => x.DocumentFile.Id)
                    .ToDictionary(x => x.Key, x => x.First().Id);

                this.descriptionsByProtocolIds = 
                    selectedProtocols.Select(x => new
                    {
                        x.Description,
                        x.Id
                    })
                    .ToList()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.First().Description);
            }
            finally 
            {
                this.Container.Release(propertyOwnerProtocolsDomain);
            }
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<FileInfo> GetExternalEntities(DynamicDictionary parameters)
        {
            var propertyOwnerProtocolsDomain = this.Container.ResolveDomain<PropertyOwnerProtocols>();

            try
            {
                return propertyOwnerProtocolsDomain.GetAll()
                    .Where(x => this.selectedProtocolIds.Contains(x.Id))
                    .Select(x => x.DocumentFile)
                    .ToList();
            }
            finally
            {
                this.Container.Release(propertyOwnerProtocolsDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(FileInfo externalEntity, RisVotingProtocolAttachment risEntity)
        {
            var fileUploadService = this.Container.Resolve<IAttachmentService>();
            var protocolGkhId = this.protocolGkhIdByFileId?.Get(externalEntity.Id) ?? 0;
            var protocol = this.protocolsById?.Get(protocolGkhId); 

            try
            {
                if (externalEntity != null)
                {
                    risEntity.VotingProtocol = protocol;
                    risEntity.ExternalSystemEntityId = externalEntity.Id;
                    risEntity.ExternalSystemName = "gkh";

                    risEntity.Attachment = fileUploadService.CreateAttachment(
                        externalEntity,
                        this.descriptionsByProtocolIds?.Get(protocolGkhId));

                    risEntity.Attachment.Name = externalEntity.Name;
                }
            }
            catch (FileNotFoundException)
            {
                risEntity.Attachment = null;
                this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл протокола голосования жильцов " + protocol?.ProtocolNum+ " не найден"));
            }
            finally
            {
                this.Container.Release(fileUploadService);
            }
        }
    }
}
