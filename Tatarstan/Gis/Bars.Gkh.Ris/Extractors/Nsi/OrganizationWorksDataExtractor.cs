namespace Bars.Gkh.Ris.Extractors.Nsi
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities.Nsi;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Gis.Entities.ManOrg;

    /// <summary>
    /// Извлечение записей раздела «Работы и услуги организации»
    /// </summary>
    public class OrganizationWorksDataExtractor : BaseDataExtractor<RisOrganizationWork, ManOrgBilWorkService>
    {
        private IDictionary serviceWorkPurposeDictionary;
        private IDictionary contentRepairMkdWorkDictionary;
        private Dictionary<long, List<ContentRepairMkdWork>> repairWorksByWorkServiceDictionary;

        private List<ManOrgBilWorkService> entitiesByContragent;

        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Список объектов УО текущего контрагента
        /// </summary>
        public List<ManOrgBilWorkService> EntitiesByContragent
        {
            get
            {
                return this.entitiesByContragent ?? (this.entitiesByContragent = this.GetEntitiesByContragent());
            }

            set
            {
                this.entitiesByContragent = value;
            }
        }

        /// <summary>
        /// Получить сущности сторонней системы - работы и услуги организации
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы - работы и услуги организации</returns>
        public override List<ManOrgBilWorkService> GetExternalEntities(DynamicDictionary parameters)
        {

            var selectedIds = parameters.GetAs("selectedList", string.Empty).ToLongArray();

            return this.EntitiesByContragent
                .WhereIf(selectedIds.Length > 0 && selectedIds[0] != 0, x => selectedIds.Contains(x.Id))
                .ToList();
        }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.serviceWorkPurposeDictionary = this.DictionaryManager.GetDictionary("ServiceWorkPurposeDictionary");

            this.contentRepairMkdWorkDictionary = this.DictionaryManager.GetDictionary("ContentRepairMkdWorkDictionary");

            var manOrgBilMkdWorkDomain = this.Container.ResolveDomain<ManOrgBilMkdWork>();

            try
            {
                var servicesArray = this.EntitiesByContragent.Select(y => y.Id).ToArray();

                this.repairWorksByWorkServiceDictionary = manOrgBilMkdWorkDomain.GetAll()
                    .Where(x => servicesArray.Contains(x.WorkService.Id))
                    .ToList()
                    .Select(x => new
                    {
                        WorkServiceId = x.WorkService.Id,
                        x.MkdWork
                    })
                    .ToList()
                    .GroupBy(x => x.WorkServiceId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.MkdWork).ToList());
            }
            finally
            {
                this.Container.Release(manOrgBilMkdWorkDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов «Работы и услуги организации»
        /// </summary>
        /// <param name="manOrgBilWorkService">Работы и услуги управляющей организации</param>
        /// <param name="risOrgWork">Работы и услуги организации</param>
        protected override void UpdateRisEntity(ManOrgBilWorkService manOrgBilWorkService, RisOrganizationWork risOrgWork)
        {
            var serviceType = this.serviceWorkPurposeDictionary.GetDictionaryRecord((int) manOrgBilWorkService.Purpose);

            risOrgWork.ExternalSystemEntityId = manOrgBilWorkService.Id;
            risOrgWork.ExternalSystemName = "gkh";
            risOrgWork.Name = manOrgBilWorkService.BilService != null
                ? manOrgBilWorkService.BilService.ServiceName
                : string.Empty;
            risOrgWork.ServiceTypeCode = serviceType?.GisCode;
            risOrgWork.ServiceTypeGuid = serviceType?.GisGuid;

            var repairWorks = this.repairWorksByWorkServiceDictionary.ContainsKey(manOrgBilWorkService.Id)
                ? this.repairWorksByWorkServiceDictionary[manOrgBilWorkService.Id]
                : new List<ContentRepairMkdWork>();

            var requiredServices = new List<RequiredService>();

            foreach (var repairWork in repairWorks)
            {
                var work = this.contentRepairMkdWorkDictionary.GetDictionaryRecord(repairWork.Id);

                if (work != null)
                {
                    requiredServices.Add(new RequiredService
                    {
                        RequiredServiceCode = work.GisCode,
                        RequiredServiceGuid = work.GisGuid
                    });
                }
            }

            risOrgWork.RequiredServices = requiredServices;

            risOrgWork.StringDimensionUnit = manOrgBilWorkService.BilService != null 
                ? manOrgBilWorkService.BilService.MeasureName
                : string.Empty;
        }

        public List<ManOrgBilWorkService> GetEntitiesByContragent()
        {
            if (this.Contragent == null)
            {
                return new List<ManOrgBilWorkService>();
            }

            var manOrgBilWorkServiceDomain = this.Container.ResolveDomain<ManOrgBilWorkService>();

            try
            {
                return manOrgBilWorkServiceDomain.GetAll()
                    .Where(x => x.ManagingOrganization.ActivityGroundsTermination == GroundsTermination.NotSet)
                    .Where(x => x.ManagingOrganization.Contragent.Id == this.Contragent.GkhId)
                    .ToList();
            }
            finally
            {
                this.Container.Release(manOrgBilWorkServiceDomain);
            }
        }
    }
}