namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using B4;

    using Bars.Gkh.Overhaul.Hmao.DomainService;

    using Entities;

    public class DpkrServiceController : B4.Alt.DataController<ProgramVersion>
    {
        public IDpkrService DpkrService { get; set; }

        public Overhaul.DomainService.IDpkrService OverhaulDpkrService { get; set; }

        public ActionResult GetYears(BaseParams baseParams)
        {
            var result = this.DpkrService.GetYears(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetMunicipality(BaseParams baseParams)
        {
            var result = this.DpkrService.GetMunicipality(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetRealityObjects(BaseParams baseParams)
        {
            var result = this.DpkrService.GetRealityObjects(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetRecords(BaseParams baseParams)
        {
            var result = this.DpkrService.GetRecords(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult GetOvrhlYears(BaseParams baseParams)
        {
            var result = OverhaulDpkrService.GetOvrhlYears(baseParams);
            return new JsonListResult(result);
        }

        /// <summary>
        /// Список МО
        /// </summary>
        public ActionResult MunicipalityListWithoutPaging(BaseParams baseParams)
        {
            var result = this.DpkrService.MunicipalityListWithoutPaging(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
        
        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Дома не попавшие в версии ДПКР
        /// </summary>
        public ActionResult GetNotIncludedInCrHousesCount(BaseParams baseParams)
        {
            var result = this.DpkrService.GetNotIncludedInCrHousesCount(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Работы из основной версии ДПКР, не попавшие в опубликованную программу
        /// </summary>
        public ActionResult GetWorksNotIncludedPublishProgramCount(BaseParams baseParams)
        {
            var result = this.DpkrService.GetWorksNotIncludedPublishProgramCount(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Дома, у которых в Реестре жилых домов не заполнен код ФИАС
        /// </summary>
        public ActionResult GetHousesWithNotFilledFias(BaseParams baseParams)
        {
            var result = this.DpkrService.GetHousesWithNotFilledFias(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Дома, с отсутствующими параметрами для расчета ДПКР
        /// </summary>
        public ActionResult GetHousesWithMissingParamsCount(BaseParams baseParams)
        {
            var result = this.DpkrService.GetHousesWithMissingParamsCount(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
        
        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Дома, включенные в ДПКР в разрезе годов
        /// </summary>
        public ActionResult GetIncludedInCrHousesByYearsCount(BaseParams baseParams)
        {
            var result = this.DpkrService.GetIncludedInCrHousesByYearsCount(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Количество работ ДПКР в разрезе ООИ
        /// </summary>
        public ActionResult GetCrCeoWorkCounts(BaseParams baseParams)
        {
            var result = this.DpkrService.GetCrCeoWorkCounts(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
        
        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Данные по стоимости работ в разрезе КЭ
        /// </summary>
        public ActionResult GetCostOfWorksInStructuralElementContext(BaseParams baseParams)
        {
            var result = this.DpkrService.GetCostOfWorksInStructuralElementContext(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }
        
        /// <summary>
        /// Получить количественные данные для виджета Статистические данные КР. Бюджетирование
        /// </summary>
        public ActionResult GetCrBudgetingCount(BaseParams baseParams)
        {
            var result = this.DpkrService.GetCrBudgetingCount(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить Excel выгрузку для виджетов
        /// </summary>
        public ActionResult GetExcelFileExport(BaseParams baseParams)
        {
            return this.DpkrService.GetExcelFileExport(baseParams);
        }
    }
}