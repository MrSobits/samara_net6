namespace Bars.Gkh.Ris.Extractors.HouseManagement.SupplyResourceContract
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Экстрактор коммунальных услуг и коммунальных ресурсов договоров с поставщиками ресурсов
    /// </summary>
    public class SupResContractServiceResourceExtractor : BaseDataExtractor<SupResContractServiceResource, PublicServiceOrgContractService>
    {
        private List<SupplyResourceContract> contracts;
        private Dictionary<long, SupplyResourceContract> contractsById;
        private IDictionary serviceTypeDict;
        private IDictionary municipalResourceDict;

        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<PublicServiceOrgContractService> GetExternalEntities(DynamicDictionary parameters)
        {
            var selectedContractIds = this.contracts?.Select(x => x.ExternalSystemEntityId).ToArray()
                ?? new long[] { };

            var realObjPublicServiceOrgServiceDomain = this.Container.ResolveDomain<PublicServiceOrgContractService>();

            try
            {
                return realObjPublicServiceOrgServiceDomain.GetAll()
                    .Where(x => x.ResOrgContract != null)
                    .Where(x => selectedContractIds.Contains(x.ResOrgContract.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(realObjPublicServiceOrgServiceDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(PublicServiceOrgContractService externalEntity, SupResContractServiceResource risEntity)
        {
            var contractBase = this.serviceTypeDict.GetDictionaryRecord(externalEntity.Service.Id);
            var municipalResource = this.municipalResourceDict.GetDictionaryRecord(externalEntity.CommunalResource.Id);

            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.Contract = this.contractsById?.Get(externalEntity.ResOrgContract.Id);
            risEntity.ServiceTypeCode = contractBase?.GisCode;
            risEntity.ServiceTypeGuid = contractBase?.GisGuid;
            risEntity.MunicipalResourceCode = municipalResource?.GisCode;
            risEntity.MunicipalResourceGuid = municipalResource?.GisGuid;
            risEntity.StartSupplyDate = externalEntity.StartDate;
            risEntity.EndSupplyDate = externalEntity.EndDate;
        }

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

            this.serviceTypeDict = this.DictionaryManager.GetDictionary("MunicipalServiceDictionary");

            this.municipalResourceDict = this.DictionaryManager.GetDictionary("ChargeableMunicipalResourceDictionary");
        }
    }
}
