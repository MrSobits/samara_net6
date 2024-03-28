namespace Bars.GkhGji.Regions.Tatarstan.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.Controllers;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class TatarstanDisposalController : TatarstanDisposalController<TatarstanDisposal>
    {
        // Внимание !! Код override нужно писать не в этом классе, а в TatarstanDisposalController<T>
    }

    // Класс переделан для того чтобы можно было использовать функционал в наследниках TatarstanDisposal
    public class TatarstanDisposalController<T> : DisposalController<T> where T : TatarstanDisposal
    {
        /// <summary>
        /// Возвращает строку, содержащую зависимые сущности
        /// </summary>
        public ActionResult GetDependenciesString(BaseParams baseParams)
        {
            var tatarstanService = this.Container.Resolve<ITatarstanDisposalService>();
            using (this.Container.Using(tatarstanService))
            {
                return new JsonNetResult(tatarstanService.GetDependenciesString(baseParams).Data) {ContentType = "text"};
            }
        }

        public override ActionResult Export(BaseParams baseParams)
        {
            baseParams.Params.Add("isExport", true);
            baseParams.Params.Add("DisposalService", this.DisposalService);

            var export = this.Container.Resolve<IDataExportService>("TatarstanDisposalDataExport");
            using (this.Container.Using(export))
            {
                return export?.ExportData(baseParams);
            }
        }
    }
}