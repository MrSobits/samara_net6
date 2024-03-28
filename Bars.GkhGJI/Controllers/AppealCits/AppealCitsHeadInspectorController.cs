namespace Bars.GkhGji.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    public class AppealCitsHeadInspectorController : B4.Alt.DataController<AppealCitsHeadInspector>
    {
        /// <summary>
        /// Метод добавления связанных руководителей
        /// </summary>
        public ActionResult AddAppealCitizens(BaseParams baseParams)
        {
            var recordIds = baseParams.Params.GetAs<long[]>("recordIds");
            var appealCitizensId = baseParams.Params.GetAsId("appealCitizensId");

            var existsIds = this.DomainService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .Select(x => x.Inspector.Id)
                .ToArray();

            this.Container.InTransaction(() =>
            {
                foreach (var id in recordIds.Except(existsIds))
                {
                    this.DomainService.Save(new AppealCitsHeadInspector
                    {
                        AppealCits = new AppealCits { Id = appealCitizensId },
                        Inspector = new Inspector { Id = id }
                    });
                }
            });

            return this.JsSuccess();
        }
    }
}