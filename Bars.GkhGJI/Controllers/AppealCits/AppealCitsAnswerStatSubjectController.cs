namespace Bars.GkhGji.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;

    public class AppealCitsAnswerStatSubjectController : B4.Alt.DataController<AppealCitsAnswerStatSubject>
    {
        /// <summary>
        /// Добавить тематику
        /// </summary>
        public ActionResult AddAnswer(BaseParams baseParams)
        {
            var recordIds = baseParams.Params.GetAs<long[]>("recordIds");
            var answerId = baseParams.Params.GetAsId("answerId");

            this.Container.InTransaction(() =>
            {
                var existsIds = this.DomainService.GetAll()
                    .Where(x => x.AppealCitsAnswer.Id == answerId)
                    .Select(x => x.StatSubject.Id)
                    .ToArray();

                foreach (var id in recordIds.Except(existsIds))
                {
                    this.DomainService.Save(new AppealCitsAnswerStatSubject
                    {
                        AppealCitsAnswer = new AppealCitsAnswer { Id = answerId },
                        StatSubject = new AppealCitsStatSubject { Id = id }
                    });
                }
            });

            return this.JsSuccess();
        }
    }
}