namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Контроллер <see cref="AppealCitsStatSubject"/>
    /// </summary>
    public class AppealCitsStatSubjectController : B4.Alt.DataController<AppealCitsStatSubject>
    {
        private readonly IAppealCitsStatSubjectService _appealCeatsStatSubjectService;
        
        public AppealCitsStatSubjectController(IAppealCitsStatSubjectService appealCeatsStatSubjectService)
        {
            _appealCeatsStatSubjectService = appealCeatsStatSubjectService;
        }
        
        /// <summary>
        /// Добавление тематики
        /// </summary>
        public ActionResult AddStatementSubject(BaseParams baseParams)
        {
            var result = _appealCeatsStatSubjectService.AddStatementSubject(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}