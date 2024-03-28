namespace Bars.Gkh.Overhaul.Tat.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Overhaul.Tat.DomainService;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class ShortProgramDefectListController : FileStorageDataController<ShortProgramDefectList>
    {
        public IShortProgramDefectListService DefectListService { get; set; }

        public ActionResult GetWorks(BaseParams baseParams)
        {
            var result = (ListDataResult)DefectListService.GetWorks(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}
