namespace Bars.GkhGji.Controllers
{

    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    // Заглушка для того чтобы в существующих регионах там где от него наследовались не полетело 
    public class DecisionGjiController : DecisionGjiController<Decision>
    {
        // Внимание !! Код override нужно писать не в этом классе, а в DisposalController<T>
    }

    // Класс переделан на для того чтобы в регионах можно было расширят ьсущность через subclass 
    // и при этом не писать дублирующий серверный код
    public class DecisionGjiController<T> : B4.Alt.DataController<T>
        where T : Decision
    {

        public IDecisionService DecisionService { get; set; }

        /// <summary>
        /// Добавить Требования НПА проверки
        /// </summary>
        public ActionResult AddNormDocItems(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDisposalVerificationSubjectService>();
            using (this.Container.Using(service))
            {
                return service.AddNormDocItems(baseParams).ToJsonResult();
            }
        }

        public ActionResult GetInfo(BaseParams baseParams)
        {
            return this.DecisionService.GetInfo(baseParams).ToJsonResult();
        }

        public ActionResult ListView(BaseParams baseParams)
        {
            return this.DecisionService.ListView(baseParams).ToJsonResult();
        }

        public ActionResult AddDisposalControlMeasures(BaseParams baseParams)
        {
            var result = this.DecisionService.AddDisposalControlMeasures(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
        public ActionResult AddExperts(BaseParams baseParams)
        {
            var result = this.DecisionService.AddExperts(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddControlLists(BaseParams baseParams)
        {
            var result = this.DecisionService.AddControlLists(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
        public ActionResult AddInspectionReasons(BaseParams baseParams)
        {
            var result = this.DecisionService.AddInspectionReasons(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddAdminRegulations(BaseParams baseParams)
        {
            var result = this.DecisionService.AddAdminRegulations(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddSurveySubjects(BaseParams baseParams)
        {
            var result = this.DecisionService.AddSurveySubjects(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddProvidedDocuments(BaseParams baseParams)
        {
            var result = this.DecisionService.AddProvidedDocs(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public IBlobPropertyService<Decision, DecisionLongText> LongTextService { get; set; }

        public ActionResult GetDescription(BaseParams baseParams)
        {
            return this.GetBlob(baseParams);
        }

        public ActionResult SaveDescription(BaseParams baseParams)
        {
            return this.SaveBlob(baseParams);
        }

        private ActionResult SaveBlob(BaseParams baseParams)
        {
            var result = this.LongTextService.Save(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        private ActionResult GetBlob(BaseParams baseParams)
        {
            var result = this.LongTextService.Get(baseParams);

            return result.Success ? new JsonGetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

    }

}