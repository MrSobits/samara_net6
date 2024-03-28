using Microsoft.AspNetCore.Mvc;
using Bars.B4;
using Bars.Gkh.DomainService;
using System.Collections;

namespace Bars.GkhGji.Regions.Habarovsk.Controllers
{
    using Bars.Gkh.Entities;
    using DomainService;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Bars.Gkh.Domain;
    using B4.Modules.DataExport.Domain;

    public class ManOrgLicenseGisController : B4.Alt.DataController<ManOrgLicense>
    {

        public virtual IManOrgLicenseGisService Service { get; set; }

        public ActionResult ListManOrgWithLicenseAndHouse(BaseParams baseParams)
        {
            var manOrgLicenseRequestService = Container.Resolve<IManOrgLicenseGisService>();
            try
            {
                return manOrgLicenseRequestService.ListManOrgWithLicenseAndHouse(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

        public ActionResult ListManOrgWithLicenseAndHouseByType(BaseParams baseParams)
        {
            var manOrgLicenseRequestService = Container.Resolve<IManOrgLicenseGisService>();
            try
            {
                return manOrgLicenseRequestService.ListManOrgWithLicenseAndHouseByType(baseParams).ToJsonResult();
            }
            finally
            {

            }
        }

        public override ActionResult Get(BaseParams baseParams)
        {
            var totalCount = 0;

            var result = Service.GetLicenseRO(baseParams, true, ref totalCount);

            return new JsonListResult(result, totalCount);
        }

        public ActionResult GetContragentInfoById(BaseParams baseParams)
        {
            var result = Service.GetContragentInfoById(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsFailure(result.Message);
        }

        public ActionResult ListManOrgWithLicense(BaseParams baseParams)
        {
            var manOrgLicenseRequestService = Container.Resolve<IManOrgLicenseGisService>();
            try
            {
                return manOrgLicenseRequestService.ListManOrgWithLicense(baseParams).ToJsonResult();
            }
            finally
            {
                
            }
        }

        public ActionResult ListManOrg(BaseParams baseParams)
        {
            var manOrgLicenseRequestService = Container.Resolve<IManOrgLicenseGisService>();
            try
            {
                return manOrgLicenseRequestService.ListManOrg(baseParams).ToJsonResult();
            }
            finally
            {
              //  Container.Release(service);
            }
        }

        public ActionResult GetListWithRO(BaseParams baseParams)
        {
            var totalCount = 0;
            var result = Service.GetListWithRO(baseParams, true, ref totalCount);
            return new JsonListResult(result, totalCount);
        }

        public ActionResult GetResolutionsByMCID(BaseParams baseParams)
        {
            var totalCount = 0;
            var result = Service.GetResolutionsByMCID(baseParams, true, ref totalCount);
            return new JsonListResult(result, totalCount);
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = Service.GetInfo(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : JsFailure(result.Message);
        }

        public ActionResult ExportBaseOmsu(BaseParams baseParams)
        {
            IDataExportService export = null;
            try
            {
                export = this.Container.Resolve<IDataExportService>("BaseOmsuDataExport");
                return export.ExportData(baseParams);
            }
            finally
            {
                if (export != null)
                {
                    this.Container.Release(export);
                }
            }
        }

        public ActionResult GetListPersonByContragentId(BaseParams baseParams)
        {
            int totalCount;
            var result = Service.GetListPersonByContragentId(baseParams, true, out totalCount);

            return result.Success ? new JsonListResult((IList)result.Data, totalCount) : JsFailure(result.Message);
        }


    }
}