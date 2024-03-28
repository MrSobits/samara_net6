namespace Bars.Gkh.Ris.Extractors.HouseManagement.SupplyResourceContract
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    /// <summary>
    /// Экстрактор информации о температурном графике договоров с поставщиками ресурсов
    /// </summary>
    public class SupResContractTemperatureChartExtractor : BaseDataExtractor<SupResContractTemperatureChart, PublicServiceOrgTemperatureInfo>
    {
        private List<SupplyResourceContract> contracts;
        private Dictionary<long, SupplyResourceContract> contractsById;

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
        public override List<PublicServiceOrgTemperatureInfo> GetExternalEntities(DynamicDictionary parameters)
        {
            var selectedContractIds = this.contracts?.Select(x => x.ExternalSystemEntityId).ToArray()
                ?? new long[] { };

            var publicServiceOrgTemperatureInfoDomain = this.Container.ResolveDomain<PublicServiceOrgTemperatureInfo>();

            try
            {
                return publicServiceOrgTemperatureInfoDomain.GetAll()
                    .Where(x => x.Contract != null)
                    .Where(x => selectedContractIds.Contains(x.Contract.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(publicServiceOrgTemperatureInfoDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(PublicServiceOrgTemperatureInfo externalEntity, SupResContractTemperatureChart risEntity)
        {
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.Contract = this.contractsById?.Get(externalEntity.Contract.Id);
            risEntity.OutsideTemperature = externalEntity.OutdoorAirTemp;
            risEntity.FlowLineTemperature = externalEntity.CoolantTempSupplyPipeline.ToString();
            risEntity.OppositeLineTemperature = externalEntity.CoolantTempReturnPipeline.ToString();
        }
    }
}
