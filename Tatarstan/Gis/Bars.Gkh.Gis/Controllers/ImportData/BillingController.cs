namespace Bars.Gkh.Gis.Controllers.ImportData
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.ImportData;

    public class BillingController : BaseController
    {
        /// <summary>
        /// Загрузка данных из БД биллинга
        /// </summary>
        private ILoadFromBillingBasesService loadFromBillingBasesService;

        private ILoadFromBillingBasesService LoadFromBillingBasesService
            => this.loadFromBillingBasesService ?? (this.loadFromBillingBasesService = this.Container.Resolve<ILoadFromBillingBasesService>());

        /// <summary>
        /// Новый метод переноса данных для аналитики из нижних банков биллинга
        /// в разрезе одного месяца
        /// </summary>
        /// <param name="month">Расчетный месяц</param>
        /// <returns></returns>
        public ActionResult GetAnalyzeDataFromBillingBases(int month, int year)
        {
            try
            {
                LoadFromBillingBasesService.GetAnalyzeDataFromBillingBases(year, month);
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при загрузке данных из таблицы биллинга - " + e);
            }
            return new JsonNetResult(new
            {
                success = true
            });
        }
        
        /// <summary>
        /// Заполнение справочника bil_dict_service услугами из нижних банков биллинга 
        /// </summary>
        /// <returns></returns>
        public ActionResult FillServicesDictionaryFromBillingBases()
        {
            try
            {
                LoadFromBillingBasesService.FillServicesDictionaryFromBillingBases();
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при загрузке данных из таблицы биллинга - " + e);
            }
            return new JsonNetResult(new
            {
                success = true
            });
        }

        /// <summary>
        /// Заполнение справочника тарифов из нижних банков биллинга 
        /// </summary>
        /// <returns></returns>
        public ActionResult FillTarifsDictionaryFromBillingBases(int year)
        {
            try
            {
                LoadFromBillingBasesService.FillTarifsDictionaryFromBillingBases(year);
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при загрузке данных из таблицы биллинга - " + e);
            }
            return new JsonNetResult(new
            {
                success = true
            });
        }

        /// <summary>
        /// Заполнение нормативов из нижних банков биллинга 
        /// </summary>
        /// <returns></returns>
        public ActionResult FillNormativeStorageFromBillingBases()
        {
            try
            {
                LoadFromBillingBasesService.FillNormativeStorageFromBillingBases();
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при загрузке данных из таблицы биллинга - " + e);
            }
            return new JsonNetResult(new
            {
                success = true
            });
        }

        /// <summary>
        /// Заполнение справочника bil_manorg_storage УО из нижних банков биллинга
        /// </summary>
        /// <returns></returns>
        public ActionResult FillManOrgStorageFromBillingBases()
        {
            try
            {
                LoadFromBillingBasesService.FillManOrgStorageFromBillingBases();
            }
            catch (Exception e)
            {
                return JsFailure("Ошибка при загрузке данных из таблицы биллинга - " + e);
            }
            return new JsonNetResult(new
            {
                success = true
            });
        }
    }
}