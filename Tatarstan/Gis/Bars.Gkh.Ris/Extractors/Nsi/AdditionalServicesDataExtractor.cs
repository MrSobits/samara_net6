namespace Bars.Gkh.Ris.Extractors.Nsi
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Nsi;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Gis.Entities.Kp50;

    /// <summary>
    /// Извлечение записей справочника «Дополнительные услуги»
    /// </summary>
    public class AdditionalServicesDataExtractor : BaseDataExtractor<RisAdditionalService, BilServiceDictionary>
    {
        /// <summary>
        /// Получить сущности сторонней системы - доп услуги
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы - доп услуги</returns>
        public override List<BilServiceDictionary> GetExternalEntities(DynamicDictionary parameters)
        {
            var selectedIds = parameters.GetAs("selectedList", string.Empty).ToLongArray();

            var manOrgRepository = this.Container.ResolveDomain<ManagingOrganization>();
            var manOrgStorageRepository = this.Container.ResolveDomain<BilManOrgStorage>();
            var domain = this.Container.ResolveDomain<BilServiceDictionary>();

            try
            {
                var manOrg = manOrgRepository.GetAll()
                    //  .Where(x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                    .FirstOrDefault(x => x.Contragent.Id == this.Contragent.Id);

                if (manOrg == null || manOrg.Contragent == null)
                {
                    return new List<BilServiceDictionary>();
                }

                var schemaIds = manOrgStorageRepository.GetAll()
                    .Where(x => x.Schema != null)
                    .Where(x => x.ManOrgInn == manOrg.Contragent.Inn && x.ManOrgKpp == manOrg.Contragent.Kpp)
                    .Select(x => x.Schema.Id)
                    .ToList();

                return domain.GetAll()
                    .Where(x => x.Schema != null)
                    .Where(x => x.ServiceTypeCode != 1 && x.ServiceTypeCode != 2)
                    .WhereIf(selectedIds.Length > 0 && selectedIds[0] != 0, x => selectedIds.Contains(x.Id))
                    .Where(x => schemaIds.Contains(x.Schema.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(domain);
                this.Container.Release(manOrgRepository);
                this.Container.Release(manOrgStorageRepository);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="billService">Сущность внешней системы</param>
        /// <param name="risAdditionalService">Ris сущность</param>
        protected override void UpdateRisEntity(BilServiceDictionary billService, RisAdditionalService risAdditionalService)
        {
            risAdditionalService.AdditionalServiceTypeName = billService.ServiceName;
            //risAdditionalService.Okei
            risAdditionalService.StringDimensionUnit = billService.MeasureName;
        }
    }
}