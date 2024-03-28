namespace Bars.GkhCr.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    public class ProgramCrController : B4.Alt.DataController<ProgramCr>
    {
        public IProgramCrService ProgramCrService { get; set; }

        public ActionResult CopyProgram(BaseParams baseParams)
        {
            var result = (BaseDataResult)this.ProgramCrService.CopyProgram(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// метод для получения списка программ Кр для реестра Квалификационный отбор (состояние - активна, текущий период)
        /// </summary>
        public ActionResult ListForQualification(BaseParams baseParams)
        {
            var result = (ListDataResult)this.ProgramCrService.ListForQualification(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult ListWithoutPaging(BaseParams baseParams)
        {
            var result = (ListDataResult)this.ProgramCrService.ListWithoutPaging(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }

        public ActionResult GetAonProgramsList(BaseParams baseParams)
        {
            var result = (ListDataResult)this.ProgramCrService.GetAonProgramsList(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }

        public ActionResult RealityObjectList(BaseParams baseParams)
        {
            var result = (ListDataResult)this.ProgramCrService.RealityObjectList(baseParams);
            return new JsonNetResult(new { success = true, data = result.Data, totalCount = result.TotalCount });
        }

        public ActionResult GjiNumberFill(BaseParams baseParams)
        {
            return this.ProgramCrService.GjiNumberFill(baseParams).ToJsonResult();
        }
    }
}
