namespace Bars.Gkh.Regions.Tatarstan.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.FormatDataExport.Domain;

    /// <summary>
    /// Контроллер экспорта в формате 4.0
    /// </summary>
    public class Format40DataExportController : BaseController
    {
        /// <summary>
        /// Интерфейс сервиса экспорта в формате 4.0
        /// </summary>
        public IFormat40DataExportService Format40DataExportService { get; set; }

        /// <summary>
        /// Метод отправки ТехПаспорта в ГИС ЖКХ
        /// </summary>
        public ActionResult SendTechPassport(BaseParams baseParams)
        {
            return this.Format40DataExportService.SendTechPassport(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Метод отправки договора/устава в ГИС ЖКХ
        /// </summary>
        public ActionResult SendDuUstav(BaseParams baseParams)
        {
            return this.Format40DataExportService.SendDuUstav(baseParams).ToJsonResult();
        }
    }
}
