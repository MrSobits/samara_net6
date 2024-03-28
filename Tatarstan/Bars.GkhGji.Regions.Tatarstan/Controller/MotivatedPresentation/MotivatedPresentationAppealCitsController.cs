namespace Bars.GkhGji.Regions.Tatarstan.Controller.MotivatedPresentation
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.AppealCits;
    using Bars.GkhGji.Regions.Tatarstan.ViewModel.AppealCits;

    public class MotivatedPresentationAppealCitsController : B4.Alt.DataController<MotivatedPresentationAppealCits>
    {
        private readonly IMotivatedPresentationAppealCitsService service;

        /// <inheritdoc />
        public MotivatedPresentationAppealCitsController(IMotivatedPresentationAppealCitsService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Получить список для реестра документов ГЖИ
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult ListForRegistry(BaseParams baseParams)
        {
            return this.service
                .ListForRegistry(baseParams)
                .ToJsonResult();
        }
        
        /// <summary>
        /// Экспорт данных реестра "Мотивированное представление" в Excel файл
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var motivatedPresentationDataExport = this.Container.Resolve<IDataExportService>("MotivatedPresentationAppealCitsExport");
            using (this.Container.Using(motivatedPresentationDataExport))
            {
                return motivatedPresentationDataExport.ExportData(baseParams);
            }
        }
    }
}