namespace Bars.Gkh.Controllers.ManOrg.ManOrgContract
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Domain;

    /// <summary>
    /// Контролер "Управление домами"
    /// </summary>
    public class ManOrgBaseContractController : FileStorageDataController<ManOrgBaseContract>
    {
        /// <summary>
        /// Получить список для фильтрации в экспорте по формату
        /// </summary>
        public ActionResult FormatDataExportList(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IFormatDataExportRepository<ManOrgBaseContract>>();
            using (this.Container.Using(service))
            {
                return service.List(baseParams).ToJsonResult();
            }
        }
    }
}