namespace Bars.GisIntegration.UI.ViewModel.Dictionary
{
    using System;
    using System.Linq;
    using System.Web.WebPages;

    using Bars.B4;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Extensions;

    using Castle.Windsor;

    /// <summary>
    /// View-модель справочников
    /// </summary>
    public class DictionaryViewModel: IDictionaryViewModel
    {
        /// <summary>
        /// IoC Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить список справочников
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции, 
        /// содержащий список справочников</returns>
        public IDataResult List(BaseParams baseParams)
        {
            var dictionariesManager = this.Container.Resolve<IDictionaryManager>();
            var loadParams = baseParams.GetLoadParam();

            try
            {
                var dictionaries = dictionariesManager.GetAllDictionaries();

                var data = dictionaries
                    .Select(this.CreateDictionaryView)
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(dictionariesManager);
            }
        }

        /// <summary>
        /// Получить список справочников ГИС ЖКХ
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие 
        /// идентификаторы пакетов для получения списка справочников</param>
        /// <returns>Список справочников ГИС ЖКХ</returns>
        public IDataResult GisDictionariesList(BaseParams baseParams)
        {
            var dictionariesManager = this.Container.Resolve<IDictionaryManager>();
            var loadParams = baseParams.GetLoadParam();

            try
            {
                var packageIds = baseParams.Params.GetAs("packageIds", string.Empty).ToGuidArray();

                if (packageIds.Length == 0)
                {
                    throw new Exception("Пустой список пакетов");
                }

                var dictionaries = dictionariesManager.GetGisDictionariesList(packageIds);

                var data = dictionaries
                    .Select(this.CreateGisDictionaryView)
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(dictionariesManager);
            }
        }

        /// <summary>
        /// Получить список записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры: код справочника</param>
        /// <returns></returns>
        public IDataResult ListRecords(BaseParams baseParams)
        {
            var dictionariesManager = this.Container.Resolve<IDictionaryManager>();
            var loadParams = baseParams.GetLoadParam();

            try
            {
                var dictCode = baseParams.Params.GetAs("dictionaryCode", loadParams.Filter.GetAs("dictionaryCode", string.Empty));

                if (dictCode.IsEmpty())
                {
                    throw new Exception("Не передан код справочника");
                }

                var dict = dictionariesManager.GetDictionary(dictCode);

                var data = dict
                    .GetDictionaryRecords()
                    .Select(this.CreateRecordView)
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                this.Container.Release(dictionariesManager);
            }
        }

        private object CreateDictionaryView(IDictionary dictionary)
        {
            return new
            {
                dictionary.Id,
                dictionary.Code,
                dictionary.Name,
                dictionary.Group,
                dictionary.GisRegistryNumber,
                dictionary.LastRecordsCompareDate,
                dictionary.State,
                CompareDictionaryEnabled = true,
                CompareRecordsEnabled = dictionary.State != DictionaryState.NotCompared
            };
        }

        private object CreateGisDictionaryView(GisDictionary dictionary)
        {
            return new
            {
                dictionary.RegistryNumber,
                dictionary.Name,
                dictionary.Group,
                dictionary.Modified
            };
        }

        private object CreateRecordView(IDictionaryRecord record)
        {
            return new
            {
                record.Id,
                record.ExternalId,
                record.ExternalName,
                record.GisCode,
                record.GisName,
                record.GisGuid
            };
        }
    }
}

