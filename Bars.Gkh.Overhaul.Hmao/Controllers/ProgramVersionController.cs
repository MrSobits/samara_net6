namespace Bars.Gkh.Overhaul.Hmao.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Import;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Import.ImportPublishYear;

    /// <summary>
    /// Контроллер версии программы
    /// </summary>
    public class ProgramVersionController : B4.Alt.DataController<ProgramVersion>
    {
        /// <summary>
        /// Изменить данные версии
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ChangeVersionData(BaseParams baseParams)
        {
            var programVersionService = this.Container.Resolve<IProgramVersionService>();

            try
            {
                return programVersionService.ChangeVersionData(baseParams).ToJsonResult("text/html; charset=utf-8");
            }
            finally
            {
                this.Container.Release(programVersionService);
            }
        }

        /// <summary>
        /// Экспорт
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("VersionRecordsExport");
            try
            {
                return export != null ? export.ExportData(baseParams) : null;
            }
            finally
            {
                this.Container.Release(export);
            }
        }

        /// <summary>
        /// Импорт сведений о сроках проведения капитального ремонта
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult Import(BaseParams baseParams)
        {
            var importer = this.Container.Resolve<IGkhImport>(PublishYearsImport.Id);

            using (this.Container.Using(importer))
            {
                var result = importer.Import(baseParams);
               
                return result.StatusImport == Enums.Import.StatusImport.CompletedWithoutError ? this.JsSuccess() : this.JsFailure(string.Empty);
            }
        }

        /// <summary>
        /// Получить дату расчета ДПКР
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetDateCalcDpkr(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IVersionDateCalcService>();
            try
            {
                var result = service.GetDateCalcDpkr(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить дату расчета показателей собираемости
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetDateCalcOwnerCollection(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IVersionDateCalcService>();

            try
            {
                var result = service.GetDateCalcOwnerCollection(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить дату расчета корректировки
        /// </summary>
        /// <param name="baseParams">Получить дату опубликования</param>
        /// <returns></returns>
        public ActionResult GetDateCalcCorrection(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IVersionDateCalcService>();

            try
            {
                var result = service.GetDateCalcCorrection(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить дату опубликования
        /// </summary>
        /// <param name="baseParams">Получить дату опубликования</param>
        /// <returns></returns>
        public ActionResult GetDateCalcPublished(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IVersionDateCalcService>();

            try
            {
                var result = service.GetDateCalcPublished(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Создать новую версию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult MakeNewVersion(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = service.MakeNewVersion(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Создать новую версию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult MakeNewVersionAll(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = this.Container.Resolve<IProgramVersionService>().MakeNewVersionAll(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Добавить новые записи
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult AddNewRecords(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.AddNewRecords(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать стоимость
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ActualizeSum(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeSum(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать год
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ActualizeYear(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeYear(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать год для Ставрополя
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ActualizeYearForStavropol(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeYearForStavropol(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать очередность
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ActualizePriority(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizePriority(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Акутализировать с КПКР
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ActualizeFromShortCr(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeFromShortCr(baseParams);
                return result.Success ? this.JsSuccess() : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить предупреждение
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetWarningMessage(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.GetWarningMessage(baseParams);
                return result.Success ? this.JsSuccess(result.Message) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить список записей на удаление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetDeletedEntriesList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.GetDeletedEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить список записей на добавление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetAddEntriesList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.GetAddEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить список записей на актуализацию стоимости
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetActualizeSumEntriesList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.GetActualizeSumEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить список записей на актуализацию года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetActualizeYearEntriesList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.GetActualizeYearEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Получить список записей на актуализацию изменения года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult GetActualizeYearChangeEntriesList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.GetActualizeYearChangeEntriesList(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Список для массового изменения года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ListForMassChangeYear(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = (ListDataResult)service.ListForMassChangeYear(baseParams);
                return new JsonListResult((IList)result.Data, result.TotalCount);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Массовое изменение года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult MassChangeYear(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = service.MassChangeYear(baseParams);
                return result.Success ? this.JsSuccess(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать удаление записей
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult ActualizeDeletedEntries(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.ActualizeDeletedEntries(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Копировать версию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult CopyVersion(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            try
            {
                var result = service.CopyVersion(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать удаление записей
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult RoofCorrection(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.RoofCorrection(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать удаление записей
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult CopyCorrectedYears(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.CopyCorrectedYears(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
        /// Актуализировать удаление записей
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        public ActionResult DeleteRepeatedWorks(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            try
            {
                var result = service.DeleteRepeatedWorks(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public ActionResult ListMainVersions(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IProgramVersionService>();

            using (this.Container.Using(service))
            {
                return service.ListMainVersions(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Разделить работу
        /// </summary>
        public ActionResult SplitWork(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IActualizeVersionService>();

            using (this.Container.Using(service))
            {
                return service.SplitWork(baseParams).ToJsonResult();
            }
        }
    }
}