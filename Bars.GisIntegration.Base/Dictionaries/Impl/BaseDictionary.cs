namespace Bars.GisIntegration.Base.Dictionaries.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Enums;

    using Bars.GisIntegration.Base.Entities;

    using Castle.Windsor;

    public abstract class BaseDictionary : IDictionary
    {
        private readonly GisDict storableDictionary;

        /// <summary>
        /// IoC Контейнер
        /// </summary>
        public IWindsorContainer Container { get; }

        /// <summary>
        /// Код справочника
        /// </summary>
        public virtual string Code => this.GetType().Name;

        /// <summary>
        /// Реестровый номер справочника в ГИС
        /// </summary>
        public string GisRegistryNumber => this.storableDictionary.NsiRegistryNumber;

        /// <summary>
        /// Группа справочника
        /// </summary>
        public DictionaryGroup? Group => this.storableDictionary.Group;

        /// <summary>
        /// Идентификатор справочника
        /// </summary>
        public long Id => this.storableDictionary.Id;

        /// <summary>
        /// Дата последнего сопоставления записей справочника
        /// </summary>
        public DateTime? LastRecordsCompareDate
        {
            get
            {
                var result = this.storableDictionary.LastRecordsCompareDate;

                if (result.HasValue)
                {
                    result = DateTime.SpecifyKind(result.Value, DateTimeKind.Local);
                }

                return result;
            }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Использовать кеширование записей
        /// </summary>
        protected virtual bool UseCache => false;

        /// <summary>
        /// Кеш записей
        /// </summary>
        protected List<GisDictRef> cache;

        /// <summary>
        /// Состояние справочника
        /// </summary>
        public DictionaryState State => this.storableDictionary.State;

        protected BaseDictionary(IWindsorContainer container)
        {
            this.Container = container;

            this.storableDictionary = this.GetStorableDictionary();
        }

        protected IQueryable<GisDictRef> GetDictionaryRecordsInternal()
        {
            if (this.UseCache && this.cache != null)
            {
                return this.cache.AsQueryable();
            }


            var refDomain = this.Container.ResolveDomain<GisDictRef>();
            try
            {
                var query = refDomain.GetAll().Where(x => x.Dict.Id == this.storableDictionary.Id);
                if (!this.UseCache)
                {
                    return query;
                }

                if (this.cache == null)
                {
                    this.cache = query.ToList();
                }

                return this.cache.AsQueryable();
            }
            finally
            {
                this.Container.Release(refDomain);
            }
        }

        public virtual List<IDictionaryRecord> GetDictionaryRecords()
        {
            var refDomain = this.Container.ResolveDomain<GisDictRef>();

            try
            {
                return this.GetDictionaryRecordsInternal().AsEnumerable().Select(x => new DictionaryRecord(x)).OfType<IDictionaryRecord>().ToList();
            }
            finally
            {
                this.Container.Release(refDomain);
            }
        }

        /// <summary>
        /// Получить запись справочника
        /// </summary>
        /// <param name="externalId">Идентификатор записи справочника внешней системы</param>
        /// <returns>Запись справочника</returns>
        public IDictionaryRecord GetDictionaryRecord(long externalId)
        {
            var refDomain = this.Container.ResolveDomain<GisDictRef>();

            try
            {
                var dictRef = this.GetDictionaryRecordsInternal().FirstOrDefault(x => x.GkhId == externalId);
                if (dictRef == null)
                {
                    return null;
                }

                return new DictionaryRecord(dictRef);
            }
            finally
            {
                this.Container.Release(refDomain);
            }
        }

        /// <summary>
        /// Обновить статус справочника
        /// </summary>
        /// <param name="newState">Новый статус</param>
        public virtual void UpdateState(DictionaryState newState)
        {
            var dictionaryDomainService = this.Container.ResolveDomain<GisDict>();

            try
            {
                this.Container.InTransaction(
                    () =>
                        {
                            this.UpdateStateInternal(newState);
                            dictionaryDomainService.Update(this.storableDictionary);
                        });
            }
            finally
            {
                this.Container.Release(dictionaryDomainService);
            }
        }

        /// <summary>
        /// Метод для внутреннего потребления: изменение статуса справочника.
        /// Может очистить таблицу сопоставлений записей справочника.
        /// </summary>
        /// <param name="state"></param>
        protected virtual void UpdateStateInternal(DictionaryState state)
        {
            this.storableDictionary.State = state;

            if (state == DictionaryState.RecordsNotCompared || state == DictionaryState.GisDictionaryDeleted)
            {
                var refDomain = this.Container.ResolveDomain<GisDictRef>();
                try
                {
                    refDomain.GetAll().Where(x => x.Dict.Id == this.storableDictionary.Id).ForEach(x => refDomain.Delete(x.Id));
                }
                finally
                {
                    this.Container.Release(refDomain);
                }
            }
        }

        /// <summary>
        /// Сопоставить справочник
        /// Текущему справочнику поставить в соответствие справочник ГИС
        /// </summary>
        /// <param name="dictionaryGroup">Группа справочника</param>
        /// <param name="gisRegistryNumber">Реестровый номер справочника в ГИС</param>
        public void CompareDictionary(DictionaryGroup dictionaryGroup, string gisRegistryNumber)
        {
            if (this.storableDictionary.Group == dictionaryGroup && this.storableDictionary.NsiRegistryNumber == gisRegistryNumber)
            {
                return;
            }

            var dictionaryDomainService = this.Container.ResolveDomain<GisDict>();

            try
            {
                this.Container.InTransaction(
                    () =>
                        {
                            this.storableDictionary.Group = dictionaryGroup;
                            this.storableDictionary.NsiRegistryNumber = gisRegistryNumber;
                            this.UpdateStateInternal(DictionaryState.RecordsNotCompared);
                            dictionaryDomainService.Update(this.storableDictionary);
                        });
            }
            finally
            {
                this.Container.Release(dictionaryDomainService);
            }
        }

        /// <summary>
        /// Получить список сущностей внешней системы
        /// </summary>
        /// <returns>Список сущностей</returns>
        protected abstract List<ExternalEntityProxy> GetExternalEntities();

        /// <summary>
        /// Выполнить предварительное сопоставление с записями ГИС ЖКХ
        /// </summary>
        /// <param name="gisRecords">Записи справочника ГИС ЖКХ</param>
        /// <returns>Результат предварительного сопоставления</returns>
        public virtual List<RecordComparisonProxy> PerformRecordComparison(List<GisRecordProxy> gisRecords)
        {
            var externalEntities = this.GetExternalEntities();
            var gisRecordsDict = gisRecords.GroupBy(x => x.Name.ToLower()).ToDictionary(x => x.Key, x => x.First());
            var refDomain = this.Container.ResolveDomain<GisDictRef>();

            try
            {
                var oldRecords = refDomain.GetAll().Where(x => x.Dict.Id == this.storableDictionary.Id).ToList();

                var result = new List<RecordComparisonProxy>();
                foreach (var externalEntity in externalEntities)
                {
                    var comparisonRecord = new RecordComparisonProxy { ExternalEntity = externalEntity };

                    result.Add(comparisonRecord);

                    var oldRecord =
                        oldRecords.FirstOrDefault(
                            x =>
                                x.GkhId == externalEntity.Id 
                                && x.GkhName == externalEntity.Name 
                                && !string.IsNullOrEmpty(x.GisCode)
                                && !string.IsNullOrEmpty(x.GisGuid) 
                                && !string.IsNullOrEmpty(x.GisName));

                    GisRecordProxy gisRecord = null;

                    if (oldRecord != null)
                    {
                        if (gisRecordsDict.TryGetValue(oldRecord.GisName.ToLower(), out gisRecord))
                        {
                            comparisonRecord.GisRecord = gisRecord;
                            continue;
                        }
                    }

                    if (gisRecordsDict.TryGetValue(externalEntity.Name.ToLower(), out gisRecord))
                    {
                        comparisonRecord.GisRecord = gisRecord;
                    }
                }

                return result;
            }
            finally 
            {
                this.Container.Release(refDomain);
            }
        }

        /// <summary>
        /// Сохранить результаты сопоставления с записями ГИС ЖКХ
        /// </summary>
        /// <param name="records">Список сопоставленных записей</param>
        public void PersistRecordComparison(List<RecordComparisonProxy> records)
        {
            var dictDomain = this.Container.ResolveDomain<GisDict>();
            var refDomain = this.Container.ResolveDomain<GisDictRef>();
            var comparedCount = 0;
            var totalCount = 0;

            try
            {
                var newRefs = new List<GisDictRef>();
                foreach (var record in records)
                {
                    if (record.ExternalEntity == null)
                    {
                        continue;
                    }

                    if (record.GisRecord != null)
                    {
                        comparedCount++;
                    }

                    totalCount++;

                    newRefs.Add(
                        new GisDictRef
                            {
                                Dict = this.storableDictionary,
                                GkhId = record.ExternalEntity.Id,
                                GkhName = record.ExternalEntity.Name,
                                GisCode = record.GisRecord?.Code,
                                GisGuid = record.GisRecord?.Guid,
                                GisName = record.GisRecord?.Name
                            });
                }

                var newState = totalCount == comparedCount ? DictionaryState.Compared
                    : comparedCount == 0 ? DictionaryState.RecordsNotCompared : DictionaryState.RecordsPartiallyCompared;

                this.Container.InTransaction(
                    () =>
                        {
                            this.storableDictionary.LastRecordsCompareDate = DateTime.Now;

                            refDomain.GetAll().Where(x => x.Dict.Id == this.storableDictionary.Id).ForEach(x => refDomain.Delete(x.Id));
                            foreach (var newRef in newRefs)
                            {
                                refDomain.Save(newRef);
                            }

                            this.UpdateStateInternal(newState);

                            dictDomain.Update(this.storableDictionary);
                        });
            }
            finally
            {
                this.Container.Release(dictDomain);
                this.Container.Release(refDomain);
            }
        }

        private GisDict GetStorableDictionary()
        {
            var dictionaryDomainService = this.Container.ResolveDomain<GisDict>();

            try
            {
                var storableDictionaries = dictionaryDomainService.GetAll().Where(x => x.ActionCode == this.Code).ToArray();

                if (storableDictionaries.Length > 1)
                {
                    throw new Exception($"Получено более одного хранимого справочника с кодом {this.Code}");
                }
                else if (storableDictionaries.Length == 1)
                {
                    return storableDictionaries[0];
                }
                else
                {
                    var newStorableDictionary = new GisDict
                    {
                        ActionCode = this.Code,
                        Name = this.Name,
                        State = DictionaryState.NotCompared
                    };

                    dictionaryDomainService.Save(newStorableDictionary);

                    return newStorableDictionary;
                }
            }
            finally 
            {
                this.Container.Release(dictionaryDomainService);
            }
        }
    }
}
