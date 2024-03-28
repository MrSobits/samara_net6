namespace Bars.GisIntegration.UI.Service.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.UI.ViewModel;

    using Castle.Windsor;

    /// <summary>
    /// Сервис справочников
    /// </summary>
    public class DictionaryService: IDictionaryService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить параметры выполнения подписывания и отправки данных
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Параметры выполнения подписывания и отправки данных</returns>
        public IDataResult GetUpdateStatesParams(BaseParams baseParams)
        {
            var dictionaryManager = this.Container.Resolve<IDictionaryManager>();

            try
            {
                return new BaseDataResult(new
                {
                    NeedSign = dictionaryManager.SignCommonNsiRequests
                });
            }
            finally
            {
                this.Container.Release(dictionaryManager);
            }
        }

        /// <summary>
        /// Получить параметры выполнения сопоставления справочника
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// код справочника</param>
        /// <returns>Параметры сопоставления справочника</returns>
        public IDataResult GetCompareDictionaryParams(BaseParams baseParams)
        {
            var dictionaryManager = this.Container.Resolve<IDictionaryManager>();

            try
            {
                return new BaseDataResult(new
                {
                    NeedSign = dictionaryManager.SignCommonNsiRequests
                });
            }
            finally
            {
                this.Container.Release(dictionaryManager);
            }
        }

        /// <summary>
        /// Получить параметры выполнения сопоставления записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// код справочника</param>
        /// <returns>Параметры сопоставления справочника</returns>
        public IDataResult GetCompareDictionaryRecordsParams(BaseParams baseParams)
        {
            var dictionaryManager = this.Container.Resolve<IDictionaryManager>();

            try
            {
                return new BaseDataResult(new
                {
                    NeedSign = dictionaryManager.SignCommonNsiRequests
                });
            }
            finally
            {
                this.Container.Release(dictionaryManager);
            }
        }

        /// <summary>
        /// Получить пакеты с запросами списка справочников
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список пакетов</returns>
        public IDataResult GetDictionaryListPackages(BaseParams baseParams)
        {
            var dictionaryManager = this.Container.Resolve<IDictionaryManager>();
            var packageViewModel = this.Container.Resolve<IPackageViewModel>();

            try
            {
                var packageInfoList = dictionaryManager.GetDictionaryListPackages();

                var packageViews = packageInfoList.Select(x => packageViewModel.GetPackageView(x));

                return new BaseDataResult(packageViews);
            }
            finally
            {
                this.Container.Release(dictionaryManager);
                this.Container.Release(packageViewModel);
            }
        }

        /// <summary>
        /// Получить пакеты с запросами записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список пакетов</returns>
        public IDataResult GetDictionaryRecordsPackages(BaseParams baseParams)
        {
            var dictionaryManager = this.Container.Resolve<IDictionaryManager>();
            var packageViewModel = this.Container.Resolve<IPackageViewModel>();

            try
            {
                var dictionaryCode = baseParams.Params.GetAs("dictionaryCode", string.Empty);
                if (dictionaryCode.IsEmpty())
                {
                    throw new ArgumentException("Не передан код справочника (dictionaryCode)");
                }

                var packageInfoList = dictionaryManager.GetDictionaryRecordsPackages(dictionaryCode);

                var packageViews = packageInfoList.Select(x => packageViewModel.GetPackageView(x));

                return new BaseDataResult(packageViews);
            }
            finally
            {
                this.Container.Release(dictionaryManager);
                this.Container.Release(packageViewModel);
            }
        }

        /// <summary>
        /// Обновить статусы справочников
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификаторы пакетов</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult UpdateStates(BaseParams baseParams)
        {
            var dictionaryManager = this.Container.Resolve<IDictionaryManager>();

            try
            {
                var packageIds = baseParams.Params.GetAs("packageIds", string.Empty).ToGuidArray();

                if (packageIds.Length == 0)
                {
                    throw new Exception("Пустой список пакетов");
                }

                dictionaryManager.UpdateStates(packageIds);

                return new BaseDataResult();
            }
            catch (Exception exception)
            {
                return new BaseDataResult(false, exception.Message);
            }
            finally
            {
                this.Container.Release(dictionaryManager);
            }
        }

        /// <summary>
        /// Сопоставить справочник
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// код справочника
        /// реестровый номер справочника ГИС
        /// группу справочника ГИС</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult CompareDictionary(BaseParams baseParams)
        {
            var dictionaryManager = this.Container.Resolve<IDictionaryManager>();

            try
            {
                var dictionaryCode = baseParams.Params.GetAs("dictionaryCode", string.Empty);
                var gisDictionaryRegisryRegistryNumber = baseParams.Params.GetAs("gisDictionaryRegisryRegistryNumber", string.Empty);
                var gisDictionaryGroup = baseParams.Params.GetAs<DictionaryGroup?>("gisDictionaryGroup");

                if (string.IsNullOrEmpty(dictionaryCode) || string.IsNullOrEmpty(gisDictionaryRegisryRegistryNumber)
                    || gisDictionaryGroup == null)
                {
                    throw new Exception(
                        $"Не достаточно исходных данных для выполнения операции: dictionaryCode = {dictionaryCode}, gisDictionaryRegisryRegistryNumber = {gisDictionaryRegisryRegistryNumber}, gisDictionaryGroup = {gisDictionaryGroup}");
                }

                dictionaryManager.CompareDictionary(dictionaryCode, gisDictionaryGroup.Value, gisDictionaryRegisryRegistryNumber);

                return new BaseDataResult();
            }
            catch (Exception exception)
            {
                return new BaseDataResult(false, exception.Message);
            }
            finally
            {
                this.Container.Release(dictionaryManager);
            }
        }

        /// <summary>
        /// Получить результаты предварительного сопоставления записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры: код справочника и идентификаторы пакетов</param>
        /// <returns>Результаты предварительного сопоставления записей</returns>
        public IDataResult GetRecordComparisonResult(BaseParams baseParams)
        {
            var dictionaryCode = baseParams.Params.GetAs("dictionaryCode", string.Empty);
            var packageIds = baseParams.Params.GetAs("packageIds", string.Empty).ToGuidArray();

            if (dictionaryCode.IsEmpty())
            {
                throw new ArgumentException("Не передан код справочника (dictionaryCode)");
            }

            if (packageIds == null || packageIds.Length == 0)
            {
                throw new ArgumentException("Не переданы идентификаторы пакетов (packageIds)");
            }

            var dictionaryManager = this.Container.Resolve<IDictionaryManager>();

            try
            {
                var result = dictionaryManager.PerformRecordComparison(dictionaryCode, packageIds);
                return new BaseDataResult(result);
            }
            finally
            {
                this.Container.Release(dictionaryManager);
            }
        }

        /// <summary>
        /// Сохранить результат сопоставления записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры: список сопоставленных записей</param>
        /// <returns></returns>
        public IDataResult PersistRecordComparison(BaseParams baseParams)
        {
            var dictionaryCode = baseParams.Params.GetAs("dictionaryCode", string.Empty);
            if (dictionaryCode.IsEmpty())
            {
                throw new ArgumentException("Не передан код справочника (dictionaryCode)");
            }

            if (!baseParams.Params.Contains("records"))
            {
                throw new ArgumentException("Не переданы записи для сохранения (records)");
            }

            var rawRecords = (IList)baseParams.Params["records"];
            var records = new List<RecordComparisonProxy>();
            foreach (DynamicDictionary rawRecord in rawRecords)
            {
                records.Add(rawRecord.ReadClass<RecordComparisonProxy>());
            }

            var dictionaryManager = this.Container.Resolve<IDictionaryManager>();

            try
            {
                dictionaryManager.PersistRecordComparison(dictionaryCode, records);
                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(dictionaryManager);
            }
        }
    }
}
