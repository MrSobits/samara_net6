namespace Bars.Gkh.Ris.Extractors.HouseManagement.VotingProtocolData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;

    /// <summary>
    /// Экстрактор данных по протоколу общего собрания собственников
    /// </summary>
    public class VotingProtocolExtractor : BaseDataExtractor<RisVotingProtocol, PropertyOwnerProtocols>
    {
        private Dictionary<string, RisHouse> risHouseByFiasGuid;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Например, подготовить словари с данными
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        /// <param name="externalEntities">Выбранные сущности внешней системы</param>
        protected override void BeforePrepareRisEntitiesHandle(DynamicDictionary parameters, List<PropertyOwnerProtocols> externalEntities)
        {
            var risHouseDomain = this.Container.ResolveDomain<RisHouse>();

            try
            {
                this.risHouseByFiasGuid = risHouseDomain.GetAll()
                    .Where(x => x.FiasHouseGuid != "")
                    .GroupBy(x => x.FiasHouseGuid)
                    .ToDictionary(x => x.Key, x => x.First());
            }
            finally 
            {
                this.Container.Release(risHouseDomain);
            }
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<PropertyOwnerProtocols> GetExternalEntities(DynamicDictionary parameters)
        {
            long[] selectedIds;

            var selectedHouses = parameters.GetAs("selectedList", string.Empty);
            if (selectedHouses.ToUpper() == "ALL")
            {
                selectedIds = null; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedIds = selectedHouses.ToLongArray();
            }

            var propertyOwnerProtocolsDomain = this.Container.ResolveDomain<PropertyOwnerProtocols>();
            var manOrgContractRealityObjectDomain = this.Container.ResolveDomain<ManOrgContractRealityObject>();

            try
            {
                var roIds = manOrgContractRealityObjectDomain.GetAll()
                    .Where(
                        x => x.ManOrgContract != null &&
                            x.RealityObject != null &&
                            x.ManOrgContract.ManagingOrganization != null &&
                            x.ManOrgContract.ManagingOrganization.Contragent != null)
                    .Where(x => x.ManOrgContract.TerminateReason == null || x.ManOrgContract.TerminateReason == string.Empty)
                    .Where(x => !x.ManOrgContract.StartDate.HasValue || x.ManOrgContract.StartDate.Value <= DateTime.Now)
                    .Where(x => !x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate.Value >= DateTime.Now)
                    .Where(x => x.ManOrgContract.ManagingOrganization.Contragent.Id == this.Contragent.GkhId)
                    .Select(x => x.RealityObject.Id)
                    .Distinct()
                    .ToList();

                return propertyOwnerProtocolsDomain.GetAll()
                    .WhereIf(selectedIds != null, x => selectedIds.Contains(x.RealityObject.Id))
                    .Where(x => x.TypeProtocol != PropertyOwnerProtocolType.ResolutionOfTheBoard)
                    .Where(x => roIds.Contains(x.RealityObject.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(propertyOwnerProtocolsDomain);
                this.Container.Release(manOrgContractRealityObjectDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(PropertyOwnerProtocols externalEntity, RisVotingProtocol risEntity)
        {
            var risHouse = this.risHouseByFiasGuid?.Get(externalEntity.RealityObject?.HouseGuid ?? string.Empty);

            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.House = risHouse;
            risEntity.ProtocolNum = externalEntity.DocumentNumber;
            risEntity.ProtocolDate = externalEntity.DocumentDate ?? DateTime.MinValue;
            risEntity.VotingTimeType = RisVotingTimeType.AnnualVoting;
            risEntity.MeetingEligibility = RisMeetingEligibility.C;
        }
    }
}
