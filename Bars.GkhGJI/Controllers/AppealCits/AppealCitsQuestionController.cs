namespace Bars.GkhGji.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Контроллер видов вопросов обращения
    /// </summary>
    public class AppealCitsQuestionController: B4.Alt.DataController<AppealCitsQuestion>
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
                .Select(x => x.QuestionKind.Id)
                .ToArray();

            this.Container.InTransaction(() =>
            {
                foreach (var id in recordIds.Except(existsIds))
                {
                    this.DomainService.Save(new AppealCitsQuestion
                    {
                        AppealCits = new AppealCits { Id = appealCitizensId },
                        QuestionKind = new QuestionKind { Id = id }
                    });
                }
            });

            return this.JsSuccess();
        }
    }
}