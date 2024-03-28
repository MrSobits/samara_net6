namespace Bars.GkhGji.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    public class AppealCitsCategoryController : B4.Alt.DataController<AppealCitsCategory>
    {
        /// <summary>
        /// Метод добавления связанных категорий заявителя
        /// </summary>
        public ActionResult AddAppealCitizens(BaseParams baseParams)
        {
            var recordIds = baseParams.Params.GetAs<long[]>("recordIds");
            var appealCitizensId = baseParams.Params.GetAsId("appealCitizensId");

            var existsIds = this.DomainService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .Select(x => x.ApplicantCategory.Id)
                .ToArray();

            this.Container.InTransaction(() =>
            {
                foreach (var id in recordIds.Except(existsIds))
                {
                    this.DomainService.Save(new AppealCitsCategory
                    {
                        AppealCits = new AppealCits {Id = appealCitizensId},
                        ApplicantCategory = new ApplicantCategory {Id = id}
                    });
                }
            });

            return this.JsSuccess();
        }
    }
}