namespace Bars.GisIntegration.Gkh.DataExtractors.HouseManagement.ContractData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Экстрактор для ContractObject
    /// </summary>
    public class ContractObjectDataExtractor: BaseDataExtractor<ContractObject, ManOrgBaseContract>
    {
        private List<RisContract> contracts;
        private List<Charter> charters;
        private Dictionary<long, RealityObject> realityObjsByContractIds;
        private Dictionary<long, RisContract> contractsById;
        private Dictionary<long, Charter> chartersById;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.contracts = parameters.GetAs<List<RisContract>>("selectedContracts");
            this.charters = parameters.GetAs<List<Charter>>("selectedCharters");

            this.contractsById = this.contracts?
                .GroupBy(x => x.ExternalSystemEntityId)
                .ToDictionary(x => x.Key, x => x.First());

            this.chartersById = this.charters?
                .GroupBy(x => x.ExternalSystemEntityId)
                .ToDictionary(x => x.Key, x => x.First());

            var manOrgRoDomain = this.Container.ResolveDomain<ManOrgContractRealityObject>();

            try
            {
                this.realityObjsByContractIds = manOrgRoDomain.GetAll()
                    .Where(
                        x =>
                            x.ManOrgContract != null &&
                            x.RealityObject != null &&
                            x.RealityObject.HouseGuid != null)
                    .Select(
                        x => new
                        {
                            ManOrgContractId = x.ManOrgContract.Id,
                            Ro = x.RealityObject
                        })
                    .ToList()
                    .GroupBy(x => x.ManOrgContractId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Ro).FirstOrDefault());
            }
            finally
            {
                this.Container.Release(manOrgRoDomain);
            }
        }

        /// <summary>
        /// Получить сущности сторонней системы - договора
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы - договора</returns>
        public override List<ManOrgBaseContract> GetExternalEntities(DynamicDictionary parameters)
        {
            var selectedContractIds =
                this.contracts?.Select(x => x.ExternalSystemEntityId).ToArray() ??
                this.charters?.Select(x => x.ExternalSystemEntityId).ToArray() ?? new long[] {};

            var manOrgBaseContractDomain = this.Container.ResolveDomain<ManOrgBaseContract>();

            try
            {
                return manOrgBaseContractDomain.GetAll()
                    .Where(x => selectedContractIds.Contains(x.Id))
                    .ToList();
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
        protected override void UpdateRisEntity(ManOrgBaseContract externalEntity, ContractObject risEntity)
        {
            var ro = this.realityObjsByContractIds.Get(externalEntity.Id);
            var ownersContract = externalEntity as ManOrgContractOwners;

            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.FiasHouseGuid = ro.ReturnSafe(x => x.HouseGuid);
            risEntity.RealityObjectId = ro.ReturnSafe(x => x.Id);
            risEntity.Contract = this.contractsById?.Get(externalEntity.Id);
            risEntity.Charter = this.chartersById?.Get(externalEntity.Id);
            risEntity.StartDate = externalEntity.StartDate;
            risEntity.EndDate = externalEntity.EndDate;
            risEntity.DateExclusion = ((!externalEntity.TerminateReason.IsEmpty() || externalEntity.ContractStopReason != 0)
                && ownersContract != null)
                ? ownersContract.TerminationDate
                : DateTime.MinValue;
        }
    }
}
