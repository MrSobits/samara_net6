namespace Bars.Gkh.Ris.Extractors.Nsi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Nsi;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Извлечение записей справочника «Коммунальные услуги»
    /// </summary>
    public class MunicipalServicesDataExtractor : GisIntegrationDataExtractorBase
    {
        //private Dictionary<long, GisDictRef> municipalServicesDictionary;
        private IDictionary municipalResourcesDictionary;

        /// <summary>
        /// Менеджер справочников
        /// </summary>
        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Подготовить словари с данными
        /// </summary>
        protected override void FillDictionaries()
        {
                //this.municipalServicesDictionary = gisDictRefDomain.GetAll()
                //    .Where(x => x.Dict.ActionCode == "Коммунальные услуги")
                //    .GroupBy(x => x.GkhId)
                //    .ToDictionary(x => x.Key, x => x.First());

                this.municipalResourcesDictionary = this.DictionaryManager.GetDictionary("MunicipalResourceDictionary");
        }

        /// <summary>
        /// Сохранить новые записи РИС
        /// </summary>
        protected override Dictionary<Type, List<BaseRisEntity>> ExtractInternal(DynamicDictionary parameters)
        {
            var risMunicipalServiceDomain = this.Container.ResolveDomain<RisMunicipalService>();

            try
            {
                var uploadedEntitiesDict = risMunicipalServiceDomain.GetAll()
                    .WhereIf(this.Contragent != null, x => x.Contragent != null && x.Contragent == this.Contragent)
                    .Where(x => x.Operation != RisEntityOperation.Delete)
                    .Where(x => x.Guid != null && x.Guid != "")
                    .Select(x => new
                    {
                        x.Id,
                        x.ExternalSystemEntityId
                    })
                    .ToList()
                    .GroupBy(x => x.ExternalSystemEntityId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Id).First());

                var municipalServicesToSave = new List<RisMunicipalService>();

                foreach (var service in this.GetMunicipalServices(parameters))
                {
                    var resource = this.municipalResourcesDictionary.GetDictionaryRecord(service.TypeCommunalResource.GetHashCode());

                    municipalServicesToSave.Add(new RisMunicipalService
                    {
                        ExternalSystemEntityId = service.Id,
                        ExternalSystemName = "gkh",
                        Id = uploadedEntitiesDict.ContainsKey(service.Id)
                            ? uploadedEntitiesDict[service.Id]
                            : 0,
                        Operation = uploadedEntitiesDict.ContainsKey(service.Id)
                            ? RisEntityOperation.Update
                            : RisEntityOperation.Create,
                        GeneralNeeds = service.IsProvidedForAllHouseNeeds,
                        MainMunicipalServiceName = service.Name,
                        //MunicipalServiceRefCode = this.municipalServicesDictionary.ContainsKey(service.Id)
                        //    ? this.municipalServicesDictionary[service.Id].GisCode
                        //    : string.Empty,
                        //MunicipalServiceRefGuid = this.municipalServicesDictionary.ContainsKey(service.Id)
                        //    ? this.municipalServicesDictionary[service.Id].GisGuid
                        //    : string.Empty,
                        MunicipalResourceRefCode = resource?.GisCode,
                        MunicipalResourceRefGuid = resource?.GisGuid,
                        SortOrderNotDefined = true
                    });
                }

                TransactionHelper.InsertInManyTransactions(this.Container, municipalServicesToSave);

                return new Dictionary<Type, List<BaseRisEntity>>
                {
                    { typeof(RisMunicipalService), municipalServicesToSave.Cast<BaseRisEntity>().ToList() }
                };
            }
            finally
            {
                this.Container.Release(risMunicipalServiceDomain);
            }
        }

        public List<ServiceDictionary> GetMunicipalServices(DynamicDictionary parameters)
        {
            var servicesDomain = this.Container.ResolveDomain<ServiceDictionary>();

            try
            {
                var selectedIds = parameters.GetAs("selectedList", string.Empty).ToLongArray();

                return servicesDomain.GetAll()
                    .WhereIf(selectedIds.Length > 0 && selectedIds[0] != 0, x => selectedIds.Contains(x.Id))
                    .Where(x => x.TypeService == TypeServiceGis.Communal && x.TypeCommunalResource.HasValue)
                    .ToList();
            }
            finally
            {
                this.Container.Release(servicesDomain);
            }
        }
    }
}