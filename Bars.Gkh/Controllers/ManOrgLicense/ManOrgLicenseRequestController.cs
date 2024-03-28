namespace Bars.Gkh.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    public class ManOrgLicenseRequestController : FileStorageDataController<ManOrgLicenseRequest>
    {
        public IManOrgLicenseRequestService Service { get; set; }

        /// <summary>
        /// Добавить должностное лицо
        /// </summary>
        public ActionResult AddPersons(BaseParams baseParams)
        {
            var result = Service.AddPersons(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Добавить документ к заявке
        /// </summary>
        public ActionResult AddProvDocs(BaseParams baseParams)
        {
            var result = Service.AddProvDocs(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Получить информацию по контрагенту
        /// </summary>
        public ActionResult GetContragentInfo(BaseParams baseParams)
        {
            var result = Service.GetContragentInfo(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Получить должностное лицо по контрагенту
        /// </summary>
        public ActionResult GetListPersonByContragent(BaseParams baseParams)
        {
            int totalCount;
            var result = Service.GetListPersonByContragent(baseParams, true, out totalCount);

            return result.Success ? new JsonListResult((IList) result.Data, totalCount) : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Получить список УО
        /// </summary>
        public ActionResult ListManOrg(BaseParams baseParams)
        {
            var manOrgLicenseRequestService = this.Container.Resolve<IManOrgLicenseRequestService>();
            try
            {
                return manOrgLicenseRequestService.ListManOrg(baseParams).ToJsonResult();
            }
            finally
            {
                this.Container.Release(this.Service);
            }
        }

        /// <summary>
        /// Получить список УО по типу обращения
        /// </summary>
        public ActionResult ListManOrgForRequestType(BaseParams baseParams)
        {
            return this.Service.ListManOrgForRequestType(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Получить список доступных для создания видов документов
        /// </summary>
        public ActionResult GetListRules(BaseParams baseParams)
        {
            var rulesProvider = this.Container.Resolve<IManorgLicenceApplicantsProvider>();

            using (this.Container.Using(rulesProvider))
            {
                return new JsonListResult(rulesProvider.GetRules(baseParams));
            }
        }

        /// <summary>
        /// Добавить документ к заявке
        /// </summary>
        public ActionResult AddSMEVRequest(BaseParams baseParams, RequestSMEVType requestType, long requestId)
        {
            var result = this.Service.AddSMEVRequest(baseParams, requestType, requestId);

            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }
    }
}