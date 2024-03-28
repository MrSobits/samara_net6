namespace Bars.GkhGji.Regions.Chelyabinsk.Controllers.MKDLicRequest
{
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    public class MKDLicRequestHeadInspectorController : B4.Alt.DataController<MKDLicRequestHeadInspector>
    {
        /// <summary>
        /// Метод добавления связанных руководителей
        /// </summary>
        public ActionResult AddRequests(BaseParams baseParams)
        {
            var recordIds = baseParams.Params.GetAs<long[]>("recordIds");
            var requestId = baseParams.Params.GetAsId("requestId");

            var existsIds = this.DomainService.GetAll()
                .Where(x => x.MKDLicRequest.Id == requestId)
                .Select(x => x.Inspector.Id)
                .ToArray();

            this.Container.InTransaction(() =>
            {
                foreach (var id in recordIds.Except(existsIds))
                {
                    this.DomainService.Save(new MKDLicRequestHeadInspector
                    {
                        MKDLicRequest = new MKDLicRequest { Id = requestId },
                        Inspector = new Inspector { Id = id }
                    });
                }
            });

            return this.JsSuccess();
        }
    }
}