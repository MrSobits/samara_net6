namespace Bars.GkhGji.Regions.Tatarstan.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    public class RapidResponseSystemAppealController : B4.Alt.DataController<RapidResponseSystemAppeal>
    {
        #region Dependency Injection
        private readonly IRapidResponseSystemAppealService rapidResponseSystemAppealService;

        public RapidResponseSystemAppealController(IRapidResponseSystemAppealService rapidResponseSystemAppealService)
        {
            this.rapidResponseSystemAppealService = rapidResponseSystemAppealService;
        }
        #endregion

        /// <summary>
        /// Экспорт данных в Excel
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            baseParams.Params.Add("isExport", true);
            var exportService = this.Container.Resolve<IDataExportService>("RapidResponseSystemAppealDetailsExport");

            using (this.Container.Using(exportService))
            {
                return exportService?.ExportData(baseParams);
            }
        }

        /// <summary>
        /// Сменить статус
        /// </summary>
        public ActionResult ChangeState(BaseParams baseParams)
        {
            return this.rapidResponseSystemAppealService.ChangeState(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Получить перечень контрагентов доступных для привязки
        /// </summary>
        public ActionResult GetAvailableContragents(BaseParams baseParams)
        {
            return this.rapidResponseSystemAppealService.GetAvailableContragents(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Получить статистику по реестру обращений ГЖИ, связанных с СОПР
        /// </summary>
        public ActionResult GetAppealsStatistic()
        {
            return new JsonGetResult(this.rapidResponseSystemAppealService.GetAppealsStatistic().Data);
        }

        /// <summary>
        /// Получить статистику по реестру обращений СОПР
        /// </summary>
        public ActionResult GetAppealDetailsStatistic()
        {
            return new JsonGetResult(this.rapidResponseSystemAppealService.GetAppealDetailsStatistic().Data);
        }

        /// <summary>
        /// Акутализация поля 'ControlPeriod' согласно новым данным у записей <see cref="RapidResponseSystemAppealDetails"/>
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        public ActionResult UpdateSoprControlPeriod(BaseParams baseParams)
        {
            return this.rapidResponseSystemAppealService.UpdateSoprControlPeriod(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Отправить электронное письмо о новом обращении
        /// </summary>
        public ActionResult SendNotificationMail(BaseParams baseParams)
        {
            return new JsonNetResult(this.rapidResponseSystemAppealService.SendNotificationMail(baseParams));
        }
    }
}