namespace Bars.Gkh.Overhaul.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;

    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities;

    // Пустышка на случай если в регионах 
    public class WorkPriceController : WorkPriceController<WorkPrice>
    {
    }

    // Generic класс для того чтобы расширять в регионы
    public class WorkPriceController<T> : B4.Alt.DataController<T>
        where T : WorkPrice
    {
        public ActionResult YearList(BaseParams baseParams)
        {
            var service = Container.Resolve<IWorkPriceService<T>>();
            try
            {
                var result = (ListDataResult)service.YearList(baseParams);

                if (result.Success)
                {
                    return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
                }

                return JsonNetResult.Message(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        /// <summary>
        /// Вовзращает уникальный список муниципалов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Уникальный список муниципальных образований в виду объекта JsonNetResult</returns>
        public ActionResult MunicipalityList(BaseParams baseParams)
        {
            var service = Container.Resolve<IWorkPriceService<T>>();
            try
            {
                var result = (ListDataResult)service.MunicipalityList(baseParams);

                if (result.Success)
                {
                    return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
                }

                return JsonNetResult.Message(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult MassAddition(BaseParams baseParams)
        {
            var service = Container.Resolve<IWorkPriceService<T>>();

            try
            {
                var result = service.DoMassAddition(baseParams);
                return new JsonNetResult(result);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        /// <summary>
        /// Экспорт реестра расценок по работам в Excel
        /// </summary>
        /// <param name="baseParams">Параметры с данными запроса</param>
        /// <returns>Файл work_price_export.xlsx с выгруженным реестром</returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var export = Container.Resolve<IDataExportService>("WorkPriceDataExport");
            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally 
            {
                Container.Release(export);
            }
        }

        public ActionResult ListByFromMunicipality(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IWorkPriceService<T>>();

            try
            {
                var result = service.ListByFromMunicipality(baseParams);
                return new JsonNetResult(result);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult ListByToMunicipality(BaseParams baseParams)
        {
            var service = Container.Resolve<IWorkPriceService<T>>();

            try
            {
                var result = service.ListByToMunicipality(baseParams);
                return new JsonNetResult(result);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        public ActionResult AddWorkPricesByMunicipality(BaseParams baseParams)
        {
            var service = Container.Resolve<IWorkPriceService<T>>();

            try
            {
                var result = service.AddWorkPricesByMunicipality(baseParams);
                if (result.Success)
                {
                    return new JsonNetResult(new { success = true });
                }

                return JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}