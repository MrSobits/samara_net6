namespace Bars.GisIntegration.Base.Dictionaries.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.GisServiceProvider;
    using Bars.GisIntegration.Base.NsiCommon;
    using Bars.GisIntegration.Base.Package;
    using Bars.GisIntegration.Base.Package.Impl;

    using Castle.Windsor;

    /// <summary>
    /// Менеджер справочников
    /// </summary>
    public class DictionaryManager : IDictionaryManager
    {
        /// <summary>
        /// IoC Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Поставщик объектов для работы с сервисом общей НСИ
        /// </summary>
        public IGisServiceProvider<NsiPortsTypeClient> ServiceProvider { get; set; }

        /// <summary>
        /// Менеджер пакетов
        /// </summary>
        public IPackageManager<TempPackageInfo, Guid> PackageManager { get; set; }

        /// <summary>
        /// Подписывать запросы к сервису общей НСИ
        /// </summary>
        public bool SignCommonNsiRequests => true;

        /// <summary>
        /// Получить все справочники
        /// </summary>
        /// <returns>Список справочников</returns>
        public List<IDictionary> GetAllDictionaries()
        {
            return this.Container.ResolveAll<IDictionary>().ToList();
        }

        /// <summary>
        /// Получить справочник
        /// </summary>
        /// <param name="dictionaryCode">Код справочника</param>
        /// <returns>Справочник</returns>
        public IDictionary GetDictionary(string dictionaryCode)
        {
            var result = this.Container.Resolve<IDictionary>(dictionaryCode);

            if (result == null)
            {
                throw new Exception($"Ошибка получения справочника c кодом {dictionaryCode}");
            }

            return result;
        }

        /// <summary>
        /// Обновить статусы словарей
        /// </summary>
        /// <param name="packageIds">Идентификаторы пакетов</param>
        public void UpdateStates(Guid[] packageIds)
        {
            var gisDictionariesLists = this.GetGisDictionariesLists(packageIds);
            var comparedDictionaries = this.GetAllDictionaries().Where(x => x.State != DictionaryState.NotCompared);

            foreach (var dictionary in comparedDictionaries)
            {
                this.UpdateDictionaryState(dictionary, gisDictionariesLists);
            }
        }

        /// <summary>
        /// Получить пакеты с запросами списка справочников
        /// </summary>
        /// <returns>Список пакетов</returns>
        public List<TempPackageInfo> GetDictionaryListPackages()
        {
            var result = new List<TempPackageInfo>();

            result.Add(this.CreateExportNsiListRequest(DictionaryGroup.Nsi));
            result.Add(this.CreateExportNsiListRequest(DictionaryGroup.NsiRao));

            return result;
        }

        /// <summary>
        /// Получить пакеты с запросами записей справочников
        /// </summary>
        /// <param name="dictionaryCode">Код справочника</param>
        /// <returns>Список пакетов</returns>
        public List<TempPackageInfo> GetDictionaryRecordsPackages(string dictionaryCode)
        {
            return new List<TempPackageInfo> { this.CreateExportNsiItemRequest(dictionaryCode) };
        }

        /// <summary>
        /// Получить список справочников ГИС
        /// </summary>
        /// <param name="packageIds">Идентификаторы пакетов</param>
        /// <returns>Список справочников ГИС</returns>
        public List<GisDictionary> GetGisDictionariesList(Guid[] packageIds)
        {
            var gisDictionariesLists = this.GetGisDictionariesLists(packageIds);

            return this.ConvertToGisDictionaryList(gisDictionariesLists);
        }

        /// <summary>
        /// Выполнить предварительное сопоставление записей и вернуть результат
        /// </summary>
        /// <param name="dictionaryCode">Код справочника</param>
        /// <param name="packageIds">Идентификаторы пакетов</param>
        /// <returns>Результат предварительного сопоставления</returns>
        public RecordComparisonResult PerformRecordComparison(string dictionaryCode, Guid[] packageIds)
        {
            var records = this.GetGisDictionaryRecords(packageIds);

            var dict = this.GetDictionary(dictionaryCode);

            try
            {
                return new RecordComparisonResult { Records = dict.PerformRecordComparison(records), GisRecords = records };
            }
            finally
            {
                this.Container.Release(dict);
            }
        }

        /// <summary>
        /// Сохранить список сопоставленных записей справочника
        /// </summary>
        /// <param name="dictionaryCode">Код справочника</param>
        /// <param name="records">Список сопоставленных записей</param>
        public void PersistRecordComparison(string dictionaryCode, List<RecordComparisonProxy> records)
        {
            var dict = this.GetDictionary(dictionaryCode);

            try
            {
                dict.PersistRecordComparison(records);
            }
            finally
            {
                this.Container.Release(dict);
            }
        }

        /// <summary>
        /// Сопоставить справочник
        /// </summary>
        /// <param name="dictionaryCode">Код справочника</param>
        /// <param name="gisDictionaryGroup">Группа справочников ГИС</param>
        /// <param name="gisDictionaryRegisryRegistryNumber">Номер справочника ГИС</param>
        public void CompareDictionary(string dictionaryCode, DictionaryGroup gisDictionaryGroup, string gisDictionaryRegisryRegistryNumber)
        {
            var dict = this.GetDictionary(dictionaryCode);

            try
            {
                dict.CompareDictionary(gisDictionaryGroup, gisDictionaryRegisryRegistryNumber);
            }
            finally
            {
                this.Container.Release(dict);
            }
        }

        private TempPackageInfo CreateExportNsiListRequest(DictionaryGroup dictionaryGroup)
        {
            ListGroup group = dictionaryGroup.ToGisGroup();

            var request = new exportNsiListRequest {ListGroup = group , ListGroupSpecified = true};

            if (this.SignCommonNsiRequests)
            {
                request.Id = "block-to-sign";
            }

            return this.PackageManager.CreatePackage($"Справочники группы \"{dictionaryGroup.GetDisplayName()}\"", request);
        }

        private TempPackageInfo CreateExportNsiItemRequest(string dictionaryCode)
        {
            var dictionary = this.GetDictionary(dictionaryCode);

            try
            {
                if (dictionary.Group == null)
                {
                    throw new InvalidOperationException($"У справочника \"{dictionary.Name}\" отсутствует группа");
                }

                var listGroup = dictionary.Group.Value.ToGisGroup();
                var request = new exportNsiItemRequest { RegistryNumber = dictionary.GisRegistryNumber, ListGroup = listGroup };

                if (this.SignCommonNsiRequests)
                {
                    request.Id = "block-to-sign";
                }

                return this.PackageManager.CreatePackage($"Записи справочника \"{dictionary.Name}\"", request);
            }
            finally
            {
                this.Container.Release(dictionary);
            }
        }

        private List<NsiListType> GetGisDictionariesLists(Guid[] packageIds)
        {
            var result = new List<NsiListType>();

            foreach (var packageId in packageIds)
            {
                var request = this.PackageManager.GetData<exportNsiListRequest>(packageId, this.SignCommonNsiRequests);

                result.Add(this.GetGisDictionariesList(request));
            }

            return result;
        }

        private NsiListType GetGisDictionariesList(exportNsiListRequest request)
        {
            var soapClient = this.ServiceProvider.GetSoapClient();
            exportNsiListResult listResult;

            var requestHeader = new HeaderType
            {
                Date = DateTime.Now,
                MessageGUID = Guid.NewGuid().ToStr()
            };

            soapClient.exportNsiList((ISRequestHeader)requestHeader, request, out listResult);

            var error = listResult.Item as ErrorMessageType;

            if (error != null)
            {
                var messageText = $"Ошибка при загрузке списка справочников: ErrorCode: {error.ErrorCode}, Description: {error.Description}";

                throw new Exception(messageText);
            }

            return (NsiListType)listResult.Item;
        }

        private List<GisRecordProxy> GetGisDictionaryRecords(Guid[] packageIds)
        {
            var results = new List<GisRecordProxy>();
            var soapClient = this.ServiceProvider.GetSoapClient();
            foreach (var packageId in packageIds)
            {
                var request = this.PackageManager.GetData<exportNsiItemRequest>(packageId, this.SignCommonNsiRequests);

                exportNsiItemResult result;
                soapClient.exportNsiItem(new ISRequestHeader { Date = DateTime.Now, MessageGUID = Guid.NewGuid().ToString() }, request, out result);

                var error = result.Item as ErrorMessageType;

                if (error != null)
                {
                    var messageText = $"Ошибка при загрузке списка записей справочника: ErrorCode: {error.ErrorCode}, Description: {error.Description}";

                    throw new Exception(messageText);
                }

                var item = result.Item as NsiItemType;
                if (item == null)
                {
                    throw new InvalidOperationException("Неожиданный тип ответа от ГИС ЖКХ");
                }

                if (item.NsiElement.Any())
                {
                    results.AddRange(
                        item.NsiElement.Select(
                            x =>
                                {
                                    var nsiElementField = x.NsiElementField.FirstOrDefault(y => y.GetType() == typeof(NsiElementStringFieldType));

                                    return new GisRecordProxy
                                               {
                                                   Code = x.Code,
                                                   Name =
                                                       nsiElementField is NsiElementStringFieldType
                                                           ? ((NsiElementStringFieldType)nsiElementField).Value
                                                           : string.Empty,
                                                   Guid = x.GUID
                                               };
                                }).Where(x => x.Name.IsNotEmpty()));
                }
            }

            return results;
        }

        private void UpdateDictionaryState(IDictionary dictionary, List<NsiListType> gisDictionaries)
        {
            if (dictionary.Group == null || string.IsNullOrEmpty(dictionary.GisRegistryNumber))
            {
                dictionary.UpdateState(DictionaryState.NotCompared);
                return;
            }

            var gisGroup = dictionary.Group.Value.ToGisGroup();

            var gisDictionaryList = gisDictionaries.FirstOrDefault(x => x.ListGroup == gisGroup);

            if (gisDictionaryList == null)
            {
                dictionary.UpdateState(DictionaryState.GisDictionaryDeleted);
                return;
            }

            var gisDictionary = gisDictionaryList.NsiItemInfo.FirstOrDefault(x => x.RegistryNumber == dictionary.GisRegistryNumber);

            if (gisDictionary == null)
            {
                dictionary.UpdateState(DictionaryState.GisDictionaryDeleted);
                return;
            }

            if (gisDictionary.Modified > dictionary.LastRecordsCompareDate)
            {
                dictionary.UpdateState(DictionaryState.GisDictionaryChanged);
            }
        }

        private List<GisDictionary> ConvertToGisDictionaryList(List<NsiListType> gisDictionariesGroups)
        {
            var result = new List<GisDictionary>();

            foreach (var gisDictionariesGroup in gisDictionariesGroups)
            {
                var group = gisDictionariesGroup.ListGroup.ToDictionaryGroup();

                foreach (var gisDictionary in gisDictionariesGroup.NsiItemInfo)
                {
                    result.Add(new GisDictionary
                    {
                        Name = gisDictionary.Name,
                        RegistryNumber = gisDictionary.RegistryNumber,
                        Group = group,
                        Modified = gisDictionary.Modified
                    });
                }
            }

            return result;
        }
    }
}
