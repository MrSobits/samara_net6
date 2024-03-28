namespace Bars.Gkh.Ris.Extractors.HouseManagement.PublicPropertyContractData
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.Gkh.Domain;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using NHibernate.Linq;

    /// <summary>
    /// Экстрактор сведений
    /// </summary>
    public class PublicPropertyContractExtractor : BaseDataExtractor<RisPublicPropertyContract, InfoAboutUseCommonFacilities>
    {
        private Dictionary<string, RisInd> risIndsBySnils;
        private Dictionary<string, RisContragent> risContrByOgrn;
        private Dictionary<string, RisHouse> risHouseByFiasGuid;

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Например, подготовить словари с данными
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            var risIndDomain = this.Container.ResolveDomain<RisInd>();
            var risContragentDomain = this.Container.ResolveDomain<RisContragent>();
            var risHouse = this.Container.ResolveDomain<RisHouse>();

            try
            {
                this.risIndsBySnils = risIndDomain.GetAll()
                    .Where(x => x.Snils != "")
                    .GroupBy(x => x.Snils)
                    .ToDictionary(x => x.Key, x => x.First());

                this.risContrByOgrn = risContragentDomain.GetAll()
                   .Where(x => x.Ogrn != "")
                   .GroupBy(x => x.Ogrn)
                   .ToDictionary(x => x.Key, x => x.First());

                this.risHouseByFiasGuid = risHouse.GetAll()
                    .Where(x => x.FiasHouseGuid != "")
                    .GroupBy(x => x.FiasHouseGuid)
                    .ToDictionary(x => x.Key, x => x.First());
            }
            finally
            {
                this.Container.Release(risContragentDomain);
                this.Container.Release(risIndDomain);
                this.Container.Release(risHouse);
            }
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<InfoAboutUseCommonFacilities> GetExternalEntities(DynamicDictionary parameters)
        {
            long[] selectedIds = { };

            var selectedContracts = parameters.GetAs("selectedList", string.Empty);

            if (selectedContracts.ToUpper() == "ALL")
            {
                selectedIds = null; // выбраны все, фильтрацию не накладываем
            }
            else
            {
                selectedIds = selectedContracts.ToLongArray();
            }

            var result = new List<InfoAboutUseCommonFacilities>();

            var infoAboutUseFacilsDomain = this.Container.ResolveDomain<InfoAboutUseCommonFacilities>();
            try
            {
                result = infoAboutUseFacilsDomain.GetAll()
                    .Fetch(x => x.DisclosureInfoRealityObj)
                    .ThenFetch(x => x.RealityObject)
                    .WhereIf(selectedIds != null, x => selectedIds.Contains(x.Id))
                    .Where(x => (x.LesseeType == LesseeTypeDi.Legal && x.Ogrn != "")
                        || (x.LesseeType == LesseeTypeDi.Individual && x.Snils != ""))
                    .ToList();
                
                return result;
            }
            finally
            {
                this.Container.Release(infoAboutUseFacilsDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(InfoAboutUseCommonFacilities externalEntity, RisPublicPropertyContract risEntity)
        {
            if (externalEntity.LesseeType == LesseeTypeDi.Legal)
            {
                risEntity.ExternalSystemEntityId = externalEntity.Id;
                risEntity.ExternalSystemName = "gkh";
                risEntity.Organization = this.risContrByOgrn.ContainsKey(externalEntity.Ogrn) ? this.risContrByOgrn[externalEntity.Ogrn] : null;
                risEntity.ContractObject = externalEntity.ContractSubject;
                risEntity.ContractNumber = externalEntity.ContractNumber;
                risEntity.StartDate = externalEntity.DateStart;
                risEntity.EndDate = externalEntity.DateEnd;
                risEntity.ProtocolNumber = externalEntity.Number;
                risEntity.ProtocolDate = externalEntity.From;
                risEntity.House = externalEntity.DisclosureInfoRealityObj != null 
                        && externalEntity.DisclosureInfoRealityObj.RealityObject != null 
                        && externalEntity.DisclosureInfoRealityObj.RealityObject.HouseGuid != "" 
                        && this.risHouseByFiasGuid.ContainsKey(externalEntity.DisclosureInfoRealityObj.RealityObject.HouseGuid)
                    ? this.risHouseByFiasGuid[externalEntity.DisclosureInfoRealityObj.RealityObject.HouseGuid] 
                    : null;
            }
            else if (externalEntity.LesseeType == LesseeTypeDi.Individual)
            {
                risEntity.ExternalSystemEntityId = externalEntity.Id;
                risEntity.ExternalSystemName = "gkh";
                risEntity.Entrepreneur = this.risIndsBySnils.ContainsKey(externalEntity.Snils) ? this.risIndsBySnils[externalEntity.Snils] : null;
                risEntity.ContractObject = externalEntity.ContractSubject;
                risEntity.ContractNumber = externalEntity.ContractNumber;
                risEntity.StartDate = externalEntity.DateStart;
                risEntity.EndDate = externalEntity.DateEnd;
                risEntity.ProtocolNumber = externalEntity.Number;
                risEntity.ProtocolDate = externalEntity.From;
                risEntity.House = externalEntity.DisclosureInfoRealityObj != null 
                        && externalEntity.DisclosureInfoRealityObj.RealityObject != null 
                        && externalEntity.DisclosureInfoRealityObj.RealityObject.HouseGuid != "" 
                        && this.risHouseByFiasGuid.ContainsKey(externalEntity.DisclosureInfoRealityObj.RealityObject.HouseGuid)
                    ? this.risHouseByFiasGuid[externalEntity.DisclosureInfoRealityObj.RealityObject.HouseGuid] 
                    : null;
            }
        }
    }
}
