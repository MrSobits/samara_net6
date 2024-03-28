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
    /// Экстрактор показателей качества услуг договоров с поставщиками ресурсов
    /// </summary>
    public class SupResContractSubjectOtherQualityExtractor : BaseDataExtractor<SupResContractSubjectOtherQuality, PublicOrgServiceQualityLevel>
    {
        private List<SupResContractSubject> contractSubjects;
        private Dictionary<long, SupResContractSubject> contractSubjectsById;
        private IDictionary unitDict;

        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.contractSubjects = parameters.GetAs<List<SupResContractSubject>>("selectedContractSubjects");

            this.contractSubjectsById = this.contractSubjects?
                .GroupBy(x => x.ExternalSystemEntityId)
                .ToDictionary(x => x.Key, x => x.First());

            this.unitDict = this.DictionaryManager.GetDictionary("UnitMeasureDictionary");
        }

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<PublicOrgServiceQualityLevel> GetExternalEntities(DynamicDictionary parameters)
        {
            var selectedContractSubjectIds = this.contractSubjects?.Select(x => x.ExternalSystemEntityId).ToArray()
                ?? new long[] { };

            var publicOrgServiceQualityLevelDomain = this.Container.ResolveDomain<PublicOrgServiceQualityLevel>();

            try
            {
                return publicOrgServiceQualityLevelDomain.GetAll()
                    .Where(x => x.ServiceOrg != null)
                    .Where(x => selectedContractSubjectIds.Contains(x.ServiceOrg.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(publicOrgServiceQualityLevelDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(PublicOrgServiceQualityLevel externalEntity, SupResContractSubjectOtherQuality risEntity)
        {
            var unit = this.unitDict.GetDictionaryRecord(externalEntity.UnitMeasure.Id);


            risEntity.ExternalSystemEntityId = externalEntity.Id;
            risEntity.ExternalSystemName = "gkh";
            risEntity.ContractSubject = this.contractSubjectsById?.Get(externalEntity.ServiceOrg.Id);
            risEntity.IndicatorName = externalEntity.Name;
            risEntity.Number = externalEntity.Value;
            risEntity.Okei = unit?.GisCode;
        }
    }
}
