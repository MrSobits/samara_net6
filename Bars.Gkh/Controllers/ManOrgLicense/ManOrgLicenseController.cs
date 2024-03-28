namespace Bars.Gkh.Controllers
{
    using System.Collections;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class ManOrgLicenseController : B4.Alt.DataController<ManOrgLicense>
    {
        public virtual IManOrgLicenseService Service { get; set; }

        public ActionResult GetPrintFormResult(BaseParams baseParams)
        {
            var result = (BaseDataResult)Service.GetPrintFormResult(baseParams);

            string fileName;

            // Хак для отображения русских имен файлов
            if (result.Success == false)
            {
                return JsonNetResult.Failure(result.Message);
            }

            return new FileStreamResult((MemoryStream)result.Data, "application/vnd.ms-excel") { FileDownloadName = "Export.xlsx" };
        }

        /// <summary>
        /// Получить информацию
        /// </summary>
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = this.Service.GetInfo(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Получить информацию о статусе лицензии
        /// </summary>
        public ActionResult GetStateInfo(BaseParams baseParams)
        {
            var result = this.Service.GetStateInfo(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Получить список по типу обращения
        /// </summary>
        public ActionResult ListForRequestType(BaseParams baseParams)
        {
            return this.Service.ListForRequestType(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Добавить должностное лицо
        /// </summary>
        public ActionResult AddPersons(BaseParams baseParams)
        {
            var result = this.Service.AddPersons(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Получить должностное лицо по контрагенту
        /// </summary>
        public ActionResult GetListPersonByContragent(BaseParams baseParams)
        {
            int totalCount;
            var result = this.Service.GetListPersonByContragent(baseParams, true, out totalCount);

            return result.Success ? new JsonListResult((IList)result.Data, totalCount) : this.JsFailure(result.Message);
        }

        /// <summary>
        /// Получить информацию по контрагенту
        /// </summary>
        public ActionResult GetContragentInfo(BaseParams baseParams)
        {
            var result = this.Service.GetContragentInfo(baseParams);

            return result.Success ? new JsonNetResult(result.Data) : this.JsFailure(result.Message);
        }
    }
}