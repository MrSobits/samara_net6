namespace Bars.GkhGji.Regions.Tatarstan.Controller.ActionIsolated
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class MotivatedPresentationController : B4.Alt.DataController<MotivatedPresentation>
    {
        private readonly IMotivatedPresentationService service;

        public MotivatedPresentationController(IMotivatedPresentationService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Получить сведения о проверках
        /// </summary>
        public ActionResult GetInspectionInfoList(BaseParams baseParams)
        {
            var result = this.service.GetInspectionInfoList(baseParams);
            return result.Success ? new JsonListResult((IEnumerable)result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить перечень нарушений
        /// </summary>
        public ActionResult GetViolationInfoList(BaseParams baseParams)
        {
            var result = this.service.GetViolationInfoList(baseParams);
            return result.Success ? new JsonListResult((IEnumerable)result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить сведения-основание новой проверки
        /// </summary>
        public ActionResult GetNewInspectionBasementInfo(BaseParams baseParams)
        {
            var result = this.service.GetNewInspectionBasementInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить список мотивированных представлений для реестра документов ГЖИ
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Список документов</returns>
        public ActionResult ListForRegistry(BaseParams baseParams) =>
            this.service.ListForRegistry(baseParams).ToJsonResult();
        
        /// <summary>
        /// Экспорт данных реестра "Мотивированное представление" в Excel файл
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var motivatedPresentationDataExport = this.Container.Resolve<IDataExportService>("MotivatedPresentationDataExport");

            using (this.Container.Using(motivatedPresentationDataExport))
            {
                return motivatedPresentationDataExport.ExportData(baseParams);
            }
        }
    }
}