namespace Bars.GkhGji.Regions.Tatarstan.Controller
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Controllers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Orgs;

    public class TatarstanZonalInspectionController: ZonalInspectionController<TatarstanZonalInspection>
    {
        /// <summary>
        /// Обновление ссылки на КНО у зональных инспекций.
        /// </summary>
        /// <param name="baseParams">Базовые праметры.</param>
        public ActionResult SaveControlOrgReference(BaseParams baseParams)
        {
            var controlOrganizationId = baseParams.Params.GetAs<long>("controlOrganizationId");
            var modifiedRecords = baseParams.Params.GetAs<string>("modifiedRecords")?.Split(',');
            var removedRecords = baseParams.Params.GetAs<string>("removedRecords")?.Split(',');
            if (controlOrganizationId != default(long) && (modifiedRecords != null || removedRecords != null))
            {
                var tatarstanInspectionDomain = this.Container.Resolve<IDomainService<TatarstanZonalInspection>>();
                var controlOrganizationDomain = this.Container.Resolve<IDomainService<ControlOrganization>>();
                using (this.Container.Using(tatarstanInspectionDomain, controlOrganizationDomain))
                {
                    var controlOrg = controlOrganizationDomain.Get(controlOrganizationId);

                    this.UpdateTatarstanZonalInspection(tatarstanInspectionDomain, modifiedRecords, controlOrg);
                    this.UpdateTatarstanZonalInspection(tatarstanInspectionDomain, removedRecords, null);
                }
            }

            return new JsonNetResult(true);
        }

        /// <summary>
        /// Обновление ссылки на КНО у зональных инспекций.
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="records">String[], содержащий инспекций.</param>
        /// <param name="controlOrg">Ссылка на КНО</param>
        private void UpdateTatarstanZonalInspection(IDomainService<TatarstanZonalInspection> domainService, string[] records, ControlOrganization controlOrg)
        {
            if (records == null)
            {
                return;
            }

            foreach (var record in records)
            {
                if (!long.TryParse(record, out var inspectionId))
                {
                    continue;
                }
                    
                var inspection = domainService.Get(inspectionId);
                inspection.ControlOrganization = controlOrg;
                domainService.Update(inspection);
            }
        }
    }
}
