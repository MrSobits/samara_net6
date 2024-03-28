namespace Bars.GisIntegration.UI.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities.Dictionaries;
    using Bars.GisIntegration.UI.Service;
    using Bars.GisIntegration.UI.ViewModel;
    using Bars.Gkh.Extensions;

    //using GisGkhLibrary.Entities.Dictionaries;
    //using GisGkhLibrary.HouseManagement;
    //using GisGkhLibrary.Managers;
    using GisGkhLibrary.Services;

    /// <summary>
    /// Контроллер справочников
    /// </summary>
    public class DictionaryController : BaseController
    {
        /// <summary>
        /// Получить список справочников
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Результат выполнения операции, 
        /// содержащий список справочников</returns>
        public ActionResult List(BaseParams baseParams)
        {
            var viewModel = this.Container.Resolve<IDictionaryViewModel>();
            try
            {
                return new JsonNetResult(viewModel.List(baseParams));
            }
            finally
            {
                this.Container.Release(viewModel);
            }
        }

        //public ActionResult Test()
        //{
        //    var item = NsiServiceCommon.exportNsiList("10.0.1.2")
        //        .OrderBy(x=>x.RegistryNumber.ToInt()).ToList();
        //    var dictDomain = this.Container.ResolveDomain<DictInfo>();
        //    foreach (var infoType in item)
        //    {
        //        dictDomain.Save(new DictInfo()
        //        {
        //            RegistryNumber = infoType.RegistryNumber.ToInt(),
        //            Modified = infoType.Modified,
        //            Name = infoType.Name,
        //            LastRequest = DateTime.MinValue,
        //            RawReply = ""
        //        });
        //    }

        //    this.Container.Release(dictDomain);
        //    return new JsonNetResult(item);
        //}

        //public ActionResult TestOne(int one)
        //{
        //    var item = NsiServiceCommon.ExportNsiItem(one)
        //        .ToList();
        //    return new JsonNetResult(item);
        //}

        //public ActionResult TestAll()
        //{
        //    var dictDomain = this.Container.ResolveDomain<DictInfo>();
        //    var allDicts = dictDomain.GetAll();
        //    foreach (var dict in allDicts)
        //    {
        //        var item = NsiServiceCommon.ExportNsiItem(dict.RegistryNumber.ToInt())
        //            .ToList().ToJson();

        //        dict.LastRequest=DateTime.Now;
        //        dict.RawReply = item;
        //        dictDomain.Update(dict);
        //    }

        //    this.Container.Release(dictDomain);
            
        //    return new JsonNetResult(null);
        //}

        public ActionResult TestExportHouse(string guid)
        {
            //var res = HouseManagementService.ExportHouseData(new Guid(guid));
            return new JsonNetResult();
        }
        
        public ActionResult ListRecords(BaseParams baseParams)
        {
            var viewModel = this.Container.Resolve<IDictionaryViewModel>();
            try
            {
                return new JsonNetResult(viewModel.ListRecords(baseParams));
            }
            finally
            {
                this.Container.Release(viewModel);
            }
        }

        /// <summary>
        /// Получить параметры выполнения подписывания и отправки данных
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Параметры выполнения подписывания и отправки данных</returns>
        public ActionResult GetUpdateStatesParams(BaseParams baseParams)
        {
            var dictionaryService = this.Container.Resolve<IDictionaryService>();

            try
            {
                var result = dictionaryService.GetUpdateStatesParams(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally 
            {
                this.Container.Release(dictionaryService);
            }
        }

        /// <summary>
        /// Получить параметры выполнения сопоставления справочника
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// код справочника</param>
        /// <returns>Параметры сопоставления справочника</returns>
        public ActionResult GetCompareDictionaryParams(BaseParams baseParams)
        {
            var dictionaryService = this.Container.Resolve<IDictionaryService>();

            try
            {
                var result = dictionaryService.GetCompareDictionaryParams(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(dictionaryService);
            }
        }

        /// <summary>
        /// Получить параметры выполнения сопоставления записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// код справочника</param>
        /// <returns>Параметры сопоставления записей справочника</returns>
        public ActionResult GetCompareDictionaryRecordsParams(BaseParams baseParams)
        {
            var dictionaryService = this.Container.Resolve<IDictionaryService>();

            try
            {
                var result = dictionaryService.GetCompareDictionaryRecordsParams(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(dictionaryService);
            }
        }

        /// <summary>
        /// Получить пакеты с запросами списка справочников
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список пакетов</returns>
        public ActionResult GetDictionaryListPackages(BaseParams baseParams)
        {
            var dictionaryService = this.Container.Resolve<IDictionaryService>();

            try
            {
                var result = dictionaryService.GetDictionaryListPackages(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(dictionaryService);
            }
        }

        /// <summary>
        /// Получить пакеты с запросами записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Список пакетов</returns>
        public ActionResult GetDictionaryRecordsPackages(BaseParams baseParams)
        {
            var dictionaryService = this.Container.Resolve<IDictionaryService>();

            try
            {
                var result = dictionaryService.GetDictionaryRecordsPackages(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(dictionaryService);
            }
        }

        public ActionResult GetRecordComparisonResult(BaseParams baseParams)
        {
            var dictionaryService = this.Container.Resolve<IDictionaryService>();

            try
            {
                var result = dictionaryService.GetRecordComparisonResult(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(dictionaryService);
            }
        }

        /// <summary>
        /// Обновить статусы справочников
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие идентификаторы пакетов</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult UpdateStates(BaseParams baseParams)
        {
            var dictionaryService = this.Container.Resolve<IDictionaryService>();

            try
            {
                var result = dictionaryService.UpdateStates(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(dictionaryService);
            }
        }

        /// <summary>
        /// Получить список справочников ГИС ЖКХ
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие 
        /// идентификаторы пакетов для получения списка справочников</param>
        /// <returns>Список справочников ГИС ЖКХ</returns>
        public ActionResult GetGisDictionariesList(BaseParams baseParams)
        {
            var viewModel = this.Container.Resolve<IDictionaryViewModel>();

            try
            {
                var result = viewModel.GisDictionariesList(baseParams);

                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(viewModel);
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
        public ActionResult CompareDictionary(BaseParams baseParams)
        {
            var dictionaryService = this.Container.Resolve<IDictionaryService>();

            try
            {
                var result = dictionaryService.CompareDictionary(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(dictionaryService);
            }
        }

        /// <summary>
        /// Сохранить сопоставление записей справочника
        /// </summary>
        /// <param name="baseParams">Параметры: код справочника, список сопоставлений</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult PersistRecordComparison(BaseParams baseParams)
        {
            var dictionaryService = this.Container.Resolve<IDictionaryService>();

            try
            {
                var result = dictionaryService.PersistRecordComparison(baseParams);
                return result.Success ? new JsonGetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(dictionaryService);
            }
        }
    }
}
